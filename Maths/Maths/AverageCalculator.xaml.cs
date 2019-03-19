using System;
using System.Collections.ObjectModel;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maths
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AverageCalculator : ContentPage
    {
        private ObservableCollection<StringInListWithID> ListAdaptor = new ObservableCollection<StringInListWithID>();
        private BigDecimal sum = 0;
        private uint NumbersCount = 0;
        private ulong IDCounterListItems = 0;
        public AverageCalculator()
        {
            InitializeComponent();
            if (MainPage.SelectedLanguage == LanguageE.Persian)
            {
                Title = "میانگین گیر";
                Info.Text = "لطفا اعداد را یکی یکی وارد کنید.";
                FindBTN.Text = "اضافه کن";
                Input.Placeholder = "عدد را وارد کنید";
            }
            Result.Text = "Sum: 0\nCount: 0\nAverage: 0";
            ResultList.ItemsSource = ListAdaptor;
        }
        public void OnDelete(object sender, EventArgs e)
        {
            StringInListWithID mi = (StringInListWithID)((MenuItem)sender).CommandParameter;
            NumbersCount--;
            sum -= Convert.ToDecimal(mi.ToString());
            for(int i = 0;i<ListAdaptor.Count;i++)
                if(ListAdaptor[i].ID == mi.ID)
                {
                    ListAdaptor.RemoveAt(i);
                    break;
                }
            SetPrecision();
            if (NumbersCount > 0)
                Result.Text = "Sum: " + sum.ToString() + "\nCount: " + NumbersCount + "\nAverage: " + (sum / (double)NumbersCount).ToString();
            else
                Result.Text = "Sum: 0\nCount: 0\nAverage: 0";
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
            ListAdaptor.Add(new StringInListWithID() { ListString = Num.ToString(),ID = IDCounterListItems++ });
            sum += Num;
            NumbersCount++;
            SetPrecision();
            //Update result
            Result.Text = "Sum: " + sum.ToString() + "\nCount: " + NumbersCount + "\nAverage: " + (sum / (double)NumbersCount).ToString();
            Input.Text = "";
        }
        private void SetPrecision() => BigDecimal.Precision = sum.ToString().Length + 15;

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            if (!Preferences.ContainsKey("AverageCalculatorDeleteInfoShowed"))
            {
                if (MainPage.SelectedLanguage == LanguageE.Persian)
                    await DisplayAlert("توجه", "برای پاک کردن تنها یک عدد آن عدد را روی لیست به سمت چپ بکشید." + "\n" + "در صورتی که می خواهید کل اعداد را پاک کنید این آیکون را یکبار دیگر انتخاب کنید. ", "باشه");
                else
                    await DisplayAlert("Note", "If you want to delete only one number, slide that number to left from list.\nIf you want to delete all numbers, select this icon again.", "OK");
                Preferences.Set("AverageCalculatorDeleteInfoShowed", true);
                return;
            }
            sum = 0;
            NumbersCount = 0;
            ListAdaptor.Clear();
            Result.Text = "Sum: 0\nCount: 0\nAverage: 0";
        }
        private class StringInListWithID
        {
            public string ListString { get; set; }
            public ulong ID { get; set; }
            public override string ToString() => ListString;
        }
    }
}