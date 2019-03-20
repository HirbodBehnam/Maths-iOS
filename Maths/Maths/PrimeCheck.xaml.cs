using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maths
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PrimeCheck : ContentPage
	{
        private ulong TO = 0, i = 0;
        private bool Done = false;
		public PrimeCheck ()
		{
			InitializeComponent ();
            if (MainPage.SelectedLanguage == LanguageE.Persian)
            {
                Title = "تشخیص عدد اول";
                Info.Text = MainPage.MenuPersianItems["تشخیص عدد اول"];
                Result.HorizontalOptions = new LayoutOptions(LayoutAlignment.End, true);
                Result.HorizontalTextAlignment = TextAlignment.End;
                InputEntry.Placeholder = "عدد را وارد کنید";
                CheckBTN.Text = "چک کن";
                popupLabel.Text = "در حال حساب";
            }
        }

        private void InputEntry_Completed(object sender, EventArgs e)
        {
            FindBTN_Clicked(null, null);
        }

        private void FindBTN_Clicked(object sender, EventArgs e)
        {
            //Get number from input
            ulong Number;
            try
            {
                Number = Convert.ToUInt64(InputEntry.Text);
            }
            catch (OverflowException)
            {
                DisplayAlert(MainPage.SelectedLanguage == LanguageE.English ? "Error" : "خطا",
                    MainPage.SelectedLanguage == LanguageE.English ? "The number you entered is too big." : "عدد وارد شده بیش از حد بزرگ است.",
                    MainPage.SelectedLanguage == LanguageE.English ? "OK" : "باشه");
                return;
            }
            catch (FormatException)
            {
                DisplayAlert(MainPage.SelectedLanguage == LanguageE.English ? "Error" : "خطا",
                    MainPage.SelectedLanguage == LanguageE.English ? "Invalid number format. Number must be a natural number." : "عدد وارد شده نامعتبر است. عدد باید عدد طبیعی باشد.",
                    MainPage.SelectedLanguage == LanguageE.English ? "OK" : "باشه");
                return;
            }
            catch (Exception ex)
            {
                DisplayAlert("Unhandled Exception", ex.ToString(), "OK");
                return;
            }
            if (Number < 2)
            {
                DisplayAlert(MainPage.SelectedLanguage == LanguageE.English ? "Error" : "خطا",
                    MainPage.SelectedLanguage == LanguageE.English ? "Number cannot be less than 2." : "عدد نمی تواند کمتر از 2 باشد.",
                    MainPage.SelectedLanguage == LanguageE.English ? "OK" : "باشه");
                return;
            }
            Done = false;
            i = 0; TO = 1;
            popupLoadingView.IsVisible = true;
            //setup main task
            new Task(() =>
            {
                ulong pTest = 1;
                //Main prime checking algorithm
                if (Number == 2 || Number == 3 || Number == 5 || Number == 7)
                    goto END;
                if (Number % 2 == 0)
                {
                    pTest = 2;
                    goto END;
                }
                if (Number % 3 == 0)
                {
                    pTest = 3;
                    goto END;
                }
                TO = (uint)Math.Sqrt(Number);
                for (i = 5; i <= TO; i += 4)
                {
                    if (Number % i == 0)
                    {
                        pTest = i;
                        goto END;
                    }
                    i += 2;
                    if (Number % i == 0)
                    {
                        pTest = i;
                        goto END;
                    }
                }
            END:
                Done = true;
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (MainPage.SelectedLanguage == LanguageE.English)
                    {
                        if (pTest == 1)
                            Result.Text = Number + " is prime.";
                        else
                            Result.Text = Number + " is not prime. It can be divided by " + pTest;
                    }
                    else
                    {
                        if (pTest == 1)
                        {
                            Result.Text = "عدد ";
                            Result.Text += Number;
                            Result.Text += " اول است.";
                        }
                        else
                        {
                            Result.Text = "عدد ";
                            Result.Text += Number;
                            Result.Text += " اول نیست. این عدد بر ";
                            Result.Text += pTest;
                            Result.Text += " بخش پذیر است.";
                        }
                    }
                    popupLoadingView.IsVisible = false;
                });
            }).Start();
            //setup progress bar
            new Task(() =>
            {
                while (!Done)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        popupProgress.Progress = (double)i / TO;
                    });
                    Thread.Sleep(200);
                }
            }).Start();
            
        }
    }
}