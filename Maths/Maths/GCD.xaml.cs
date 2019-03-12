using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
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
        private int i;
        private bool Done = false;
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
                i = 0;
                string[] numbersSplitted = EntryNumbers.Text.Split(',');
                nums = new ulong[numbersSplitted.Length];
                try
                {
                    for (; i < numbersSplitted.Length; i++)
                    {
                        nums[i] = Convert.ToUInt64(numbersSplitted[i].Trim());
                        if (nums[i] == 0)
                        {
                            DisplayAlert(MainPage.SelectedLanguage == LanguageE.English ? "Error" : "خطا",
                                MainPage.SelectedLanguage == LanguageE.English ? "You cannot input 0." : "عدد نمی تواند صفر باشد.",
                                MainPage.SelectedLanguage == LanguageE.English ? "OK" : "باشه");
                            return;
                        }
                        if (nums[i] == 1)
                        {
                            Result.Text = MainPage.SelectedLanguage == LanguageE.English ? "The GCD is: 1" : "ب.م.م برابر یک است.";
                            return;
                        }
                    }
                }
                catch (OverflowException)
                {
                    DisplayAlert(MainPage.SelectedLanguage == LanguageE.English ? "Error" : "خطا",
                        MainPage.SelectedLanguage == LanguageE.English ? $"Too big number on index {i + 1} ({numbersSplitted[i]})." : "عدد خیلی بزرگی وارد کریدید:\n" + numbersSplitted[i],
                        MainPage.SelectedLanguage == LanguageE.English ? "OK" : "باشه");
                    return;
                }
                catch (FormatException)
                {
                    DisplayAlert(MainPage.SelectedLanguage == LanguageE.English ? "Error" : "خطا",
                        MainPage.SelectedLanguage == LanguageE.English ? $"Invalid formatted number on index {i + 1} ({numbersSplitted[i]})." : "عدد نامعتبری وارد کریدید:\n" + numbersSplitted[i],
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
            Done = false;
            popupLoadingView.IsVisible = true;
            if (LCMorGCDSwitch.IsToggled) //LCM
            {
                i = 0;
                //Main calculation
                new Task(() =>
                {
                    BigInteger big = new BigInteger(nums[0]);
                    BigInteger numTemp;
                    for (i = 1; i < nums.Length; i++)
                    {
                        numTemp = nums[i];
                        big = big / BigInteger.GreatestCommonDivisor(big, numTemp) * numTemp;
                    }
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        if (MainPage.SelectedLanguage == LanguageE.English)
                            Result.Text = "The LCM is: " + big;
                        else
                            Result.Text = "ک.م.م برابر است با " + big;
                    });
                    popupLoadingView.IsVisible = false;
                }).Start();
                //Progress report
                new Task(() =>
                {
                    while (!Done)
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            popupProgress.Progress = (double)i / nums.Length;
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
                    for (i = nums.Length; i-- > 1;)
                        nums[i - 1] = MathFunctions.GCD(nums[i - 1], nums[i]);
                    Done = true;
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        if (MainPage.SelectedLanguage == LanguageE.English)
                            Result.Text = "The GCD is: " + nums[0];
                        else
                            Result.Text = "ب.م.م برابر است با " + nums[0];
                    });
                    popupLoadingView.IsVisible = false;
                }).Start();
                //Progress report
                new Task(() =>
                {
                    while (!Done)
                    {
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            popupProgress.Progress = 1d - ((double)i / nums.Length);
                        });
                        Thread.Sleep(200);
                    }
                }).Start();
            }
        }
    }
}