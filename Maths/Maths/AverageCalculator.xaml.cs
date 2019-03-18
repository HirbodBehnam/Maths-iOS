using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        ObservableCollection<StringInList> ListAdaptor = new ObservableCollection<StringInList>();
        BigDecimal sum = 0;
        uint NumbersCount = 0;
        public AverageCalculator()
        {
            InitializeComponent();
            if (MainPage.SelectedLanguage == LanguageE.Persian)
            {
                Title = "میانگین گیر";
                Info.Text = "لطفا اعداد را یکی یکی وارد کنید.";
                FindBTN.Text = "اضافه کن";
                Input.Placeholder = "عدد را وارد کنید.";
            }
            Result.Text = "Sum: 0\nCount: 0\nAverage: 0";
            ResultList.ItemsSource = ListAdaptor;
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            decimal Num;
            try
            {
                Num = decimal.Parse(Input.Text);
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
            ListAdaptor.Add(new StringInList() { ListString = Num.ToString() });
            sum += Num;
            NumbersCount++;
            SetPrecision();
            //Update result
            Result.Text = "Sum: " + sum.ToString() + "\nCount: " + NumbersCount + "\nAverage: " + (sum / (double)NumbersCount).ToString();
        }
        private void SetPrecision() => BigDecimal.Precision = sum.ToString().Length + 15;
        private async void ResultList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            string action = await DisplayActionSheet(MainPage.SelectedLanguage == LanguageE.Persian ? "آیا مایلید که عدد " + e.Item.ToString() + " را پاک کنید؟" : "Delete the number " + e.Item.ToString() + "?", MainPage.SelectedLanguage == LanguageE.Persian ? "بیخیال" : "Cancel", MainPage.SelectedLanguage == LanguageE.Persian ? "پاک کن" : "Delete");
            if(action == "Delete")
            {
                NumbersCount--;
                sum -= Convert.ToDouble(e.Item.ToString());
                ListAdaptor.RemoveAt(e.ItemIndex);
                SetPrecision();
                if (NumbersCount > 0)
                    Result.Text = "Sum: " + sum.ToString() + "\nCount: " + NumbersCount + "\nAverage: " + (sum / (double)NumbersCount).ToString();
                else
                    Result.Text = "Sum: 0\nCount: 0\nAverage: 0";
            }
        }
    }
}