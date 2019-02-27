using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maths
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PrimeCheck : ContentPage
	{
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
            }
        }
        private void FindBTN_Clicked(object sender, EventArgs e)
        {
            ulong Number;
            //Get number from input
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
            ulong pTest = MathFunctions.CheckPrime(Number);
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
        }
    }
}