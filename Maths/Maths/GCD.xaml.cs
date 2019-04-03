using System;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maths
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GCD : ContentPage
	{
        private string _res = "0";
        private int _i;
        private bool _done;
		public GCD ()
		{
			InitializeComponent ();
            if (MainPage.SelectedLanguage == LanguageE.Persian)
            {
                Title = "ب.م.م و ک.م.م";
                Info.Text = MainPage.MenuPersianItems["ب.م.م و ک.م.م"];
                Info.Text += ". اعداد را به مانند زیر وارد کنید:";
                Info.Text += "\nnum1,num2,num3,num4,...";
                FindBTN.Text = "حساب کن";
                Result.HorizontalOptions = new LayoutOptions(LayoutAlignment.End, true);
                Result.HorizontalTextAlignment = TextAlignment.End;
                LCMSwitchTXT.Text = "ک.م.م";
                GCDSwitchTXT.Text = "ب.م.م";
                EntryNumbers.Placeholder = "اعداد را وارد کنید";
                popupLabel.Text = "در حال محاسبه...";
            }
        }

        private void Find_Clicked(object sender, EventArgs e)
        {
            ulong[] nums;
            {//Get the numbers
                _i = 0;
                string[] numbersSplit = EntryNumbers.Text.Split(',');
                nums = new ulong[numbersSplit.Length];
                try
                {
                    for (; _i < numbersSplit.Length; _i++)
                    {
                        nums[_i] = Convert.ToUInt64(numbersSplit[_i].Trim());
                        if (nums[_i] == 0)
                        {
                            DisplayAlert(MainPage.SelectedLanguage == LanguageE.English ? "Error" : "خطا",
                                MainPage.SelectedLanguage == LanguageE.English ? "You cannot input 0." : "عدد نمی تواند صفر باشد.",
                                MainPage.SelectedLanguage == LanguageE.English ? "OK" : "باشه");
                            return;
                        }
                        if (nums[_i] == 1)
                        {
                            Result.Text = MainPage.SelectedLanguage == LanguageE.English ? "The GCD is: 1" : "ب.م.م برابر یک است.";
                            return;
                        }
                    }
                }
                catch (OverflowException)
                {
                    DisplayAlert(MainPage.SelectedLanguage == LanguageE.English ? "Error" : "خطا",
                        MainPage.SelectedLanguage == LanguageE.English ? $"Too big number on index {_i + 1} ({numbersSplit[_i]})." : "عدد خیلی بزرگی وارد کریدید:\n" + numbersSplit[_i],
                        MainPage.SelectedLanguage == LanguageE.English ? "OK" : "باشه");
                    return;
                }
                catch (FormatException)
                {
                    DisplayAlert(MainPage.SelectedLanguage == LanguageE.English ? "Error" : "خطا",
                        MainPage.SelectedLanguage == LanguageE.English ? $"Invalid formatted number on index {_i + 1} ({numbersSplit[_i]})." : "عدد نامعتبری وارد کریدید:\n" + numbersSplit[_i],
                        MainPage.SelectedLanguage == LanguageE.English ? "OK" : "باشه");
                    return;
                }
                catch (Exception ex)
                {
                    DisplayAlert("Unhandled Exception",ex.ToString() , "OK");
                    return;
                }
            }
            if (nums.Length < 2)
            {
                DisplayAlert(MainPage.SelectedLanguage == LanguageE.English ? "Error" : "خطا",
                    MainPage.SelectedLanguage == LanguageE.English ? "Enter at least 2 numbers." : "دست کم دو عدد وارد کنید.",
                    MainPage.SelectedLanguage == LanguageE.English ? "OK" : "باشه");
                return;
            }
            _done = false;
            popupLoadingView.IsVisible = true;
            if (LCMorGCDSwitch.IsToggled) //LCM
            {
                _i = 0;
                //Main calculation
                new Task(() =>
                {
                    BigInteger big = new BigInteger(nums[0]);
                    BigInteger numTemp;
                    for (_i = 1; _i < nums.Length; _i++)
                    {
                        numTemp = nums[_i];
                        big = big / BigInteger.GreatestCommonDivisor(big, numTemp) * numTemp;
                    }
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        _res = big.ToString();
                        if (MainPage.SelectedLanguage == LanguageE.English)
                            Result.Text = "The LCM is: " + _res;
                        else
                            Result.Text = "ک.م.م برابر است با " + _res;
                    });
                    popupLoadingView.IsVisible = false;
                }).Start();
                //Progress report
                new Task(() =>
                {
                    while (!_done)
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            popupProgress.Progress = (double)_i / nums.Length;
                        });
                        Thread.Sleep(200);
                    }
                }).Start();
            }
            else //GCD
            {
                //Main calculation
                new Task(() =>
                {
                    for (_i = nums.Length; _i-- > 1;)
                        nums[_i - 1] = MathFunctions.GCD(nums[_i - 1], nums[_i]);
                    _done = true;
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        _res = nums[0].ToString();
                        if (MainPage.SelectedLanguage == LanguageE.English)
                            Result.Text = "The GCD is: " + _res;
                        else
                            Result.Text = "ب.م.م برابر است با " + _res;
                    });
                    popupLoadingView.IsVisible = false;
                }).Start();
                //Progress report
                new Task(() =>
                {
                    while (!_done)
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            popupProgress.Progress = 1d - ((double)_i / nums.Length);
                        });
                        Thread.Sleep(200);
                    }
                }).Start();
            }
        }
        private async void ToolbarItemCopy_Clicked(object sender, EventArgs e)
        {
            Vibration.Vibrate(100);
            await Clipboard.SetTextAsync(_res);
        }

        private void EntryNumbers_Completed(object sender, EventArgs e) => Find_Clicked(null, null);
    }
}