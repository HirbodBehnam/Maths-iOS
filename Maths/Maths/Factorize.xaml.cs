using System;
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
                string s = "<span style=\"font-size: 20px\">";
                { //Build the HTML code
                    s += array[0] + "<sup><small>";
                    ulong index = 0;
                    ulong j = array[0];
                    foreach (ulong i in array)
                    {
                        if (j == i) index++;
                        else
                        {
                            if (index != 1)
                                s += index + "</small></sup>×" + i + "<sup><small>";
                            else
                            {
                                s = s.Substring(0, s.Length - 12);
                                s += "×" + i + "<sup><small>";
                            }
                            index = 1;
                            j = i;
                        }
                    }
                    if (index != 1)
                        s += index + "</small></sup>";
                    s += "</span>";
                }
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    /*
                    * About how I print the results.
                    * This a little bit stupid cause I have to remove and add another
                    * web view in order to make the content visible. I searched the
                    * net and read about bindings and other stuff but none of them
                    * worked. This is a little bit safer way to show the results but
                    * it have to create another web view every time it factorizes a
                    * number.
                    */
                    try
                    {
                        StackResult.Children.RemoveAt(0);
                    }
                    catch (Exception) { }
                    var browser = new WebView
                    {
                        Source = new HtmlWebViewSource() { Html = s },
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        Margin = new Thickness(10, 0)
                    };
                    StackResult.Children.Add(browser);
                    popupLoadingView.IsVisible = false;
                });
            }).Start();  
        }
    }
}