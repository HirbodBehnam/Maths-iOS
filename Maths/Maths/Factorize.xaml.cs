using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maths
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Factorize : ContentPage
	{
        public Factorize ()
		{
			InitializeComponent ();
            if(MainPage.SelectedLanguage == LanguageE.Persian)
            {
                Title = "تجزیه";
                Info.Text = MainPage.MenuPersianItems["تجزیه"];
                FindBTN.Text = "پیدا کن";
                InputEntry.Placeholder = "عدد را وارد کنید";
                popupLabel.Text = "در حال محاسبه...";
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
            if(Number < 2)
            {
                DisplayAlert(MainPage.SelectedLanguage == LanguageE.English ? "Error" : "خطا",
                    MainPage.SelectedLanguage == LanguageE.English ? "Number cannot be less than 2." : "عدد نمی تواند کمتر از 2 باشد.",
                    MainPage.SelectedLanguage == LanguageE.English ? "OK" : "باشه");
                return;
            }
            popupLoadingView.IsVisible = true;
            new Task(() =>
            {
                ulong[] array = MathFunctions.Factorize(Number);
                Dictionary<string, string> numberOfOccurring = new Dictionary<string, string>();
                {//Count Occurring
                    int Occurr = 0;
                    ulong Now = array[0];
                    foreach(ulong i in array)
                    {
                        if(Now != i)
                        {
                            numberOfOccurring.Add(Now.ToString(), Occurr.ToString());
                            Now = i;
                            Occurr = 0;
                        }
                        Occurr++;
                    }
                    numberOfOccurring.Add(Now.ToString(), Occurr.ToString());
                }
                char[] SuperScripts = new char[] { '⁰', '¹', '²', '³', '⁴', '⁵', '⁶', '⁷', '⁸', '⁹' };
                StringBuilder sb = new StringBuilder();
                foreach(var i in numberOfOccurring)
                {
                    sb.Append(i.Key);
                    if(i.Value != "1")
                        foreach(char c in i.Value)
                            sb.Append(SuperScripts[Convert.ToInt32(c.ToString())]);
                    sb.Append(" × ");
                }
                sb.Length -= 2;
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    ResultLabel.Text = sb.ToString();
                    popupLoadingView.IsVisible = false;
                });
            }).Start();  
        }

        private void InputEntry_Completed(object sender, EventArgs e)
        {
            FindBTN_Clicked(null, null);
        }
    }
}