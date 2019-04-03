using System;
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
        private ulong _to, _i;
        private bool _done;
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
            ulong number;
            try
            {
                number = Convert.ToUInt64(InputEntry.Text);
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
            if (number < 2)
            {
                DisplayAlert(MainPage.SelectedLanguage == LanguageE.English ? "Error" : "خطا",
                    MainPage.SelectedLanguage == LanguageE.English ? "Number cannot be less than 2." : "عدد نمی تواند کمتر از 2 باشد.",
                    MainPage.SelectedLanguage == LanguageE.English ? "OK" : "باشه");
                return;
            }
            _done = false;
            _i = 0; _to = 1;
            popupLoadingView.IsVisible = true;
            //setup main task
            new Task(() =>
            {
                ulong pTest = 1;
                //Main prime checking algorithm
                if (number == 2 || number == 3 || number == 5 || number == 7)
                    goto END;
                if (number % 2 == 0)
                {
                    pTest = 2;
                    goto END;
                }
                if (number % 3 == 0)
                {
                    pTest = 3;
                    goto END;
                }
                _to = (uint)Math.Sqrt(number);
                for (_i = 5; _i <= _to; _i += 4)
                {
                    if (number % _i == 0)
                    {
                        pTest = _i;
                        goto END;
                    }
                    _i += 2;
                    if (number % _i == 0)
                    {
                        pTest = _i;
                        goto END;
                    }
                }
            END:
                _done = true;
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (MainPage.SelectedLanguage == LanguageE.English)
                    {
                        if (pTest == 1)
                            Result.Text = number + " is prime.";
                        else
                            Result.Text = number + " is not prime. It can be divided by " + pTest;
                    }
                    else
                    {
                        if (pTest == 1)
                        {
                            Result.Text = "عدد ";
                            Result.Text += number;
                            Result.Text += " اول است.";
                        }
                        else
                        {
                            Result.Text = "عدد ";
                            Result.Text += number;
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
                while (!_done)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        popupProgress.Progress = (double)_i / _to;
                    });
                    Thread.Sleep(200);
                }
            }).Start();
            
        }
    }
}