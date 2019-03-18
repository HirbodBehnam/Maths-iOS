using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maths
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AverageCalculator : ContentPage
    {
        BigDecimal sum = 0;
        uint NumbersCount = 0;
        public AverageCalculator()
        {
            InitializeComponent();
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            double Num;
            try
            {
                Num = double.Parse(Input.Text);
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
            sum += Num;
            NumbersCount++;
            //Update result
            Result.Text = "Sum: " + sum.ToString() + "\nCount: " + NumbersCount + "\nAverage: " + (sum / (double)NumbersCount).ToString();
        }

        private void ResultList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            
        }
    }
}