using System;
using System.Collections.ObjectModel;
using System.Globalization;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maths
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AverageCalculator : ContentPage
    {
        private ObservableCollection<StringInListWithId> ListAdaptor = new ObservableCollection<StringInListWithId>();
        private string _averageRes = "0";
        private BigDecimal _sum = 0;
        private uint _numbersCount;
        private ulong _idCounterListItems;
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
            StringInListWithId mi = (StringInListWithId)((MenuItem)sender).CommandParameter;
            _numbersCount--;
            _sum -= Convert.ToDecimal(mi.ToString());
            for(int i = 0;i<ListAdaptor.Count;i++)
                if(ListAdaptor[i].ID == mi.ID)
                {
                    ListAdaptor.RemoveAt(i);
                    break;
                }
            SetPrecision();
            if (_numbersCount > 0)
            {
                _averageRes = (_sum / (double)_numbersCount).ToString();
                Result.Text = "Sum: " + _sum + "\nCount: " + _numbersCount + "\nAverage: " + _averageRes;
            }
            else
            {
                _averageRes = "0";
                Result.Text = "Sum: 0\nCount: 0\nAverage: 0";
            }
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            decimal num;
            try
            {
                num = decimal.Parse(Input.Text);
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
            ListAdaptor.Add(new StringInListWithId() { ListString = num.ToString(CultureInfo.CurrentCulture),ID = _idCounterListItems++ });
            _sum += num;
            _numbersCount++;
            SetPrecision();
            _averageRes = (_sum / (double)_numbersCount).ToString();
            //Update result
            Result.Text = "Sum: " + _sum + "\nCount: " + _numbersCount + "\nAverage: " + _averageRes;
            Input.Text = "";
        }
        private void SetPrecision() => BigDecimal.Precision = _sum.ToString().Length + 15;

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
            _sum = 0;
            _numbersCount = 0;
            ListAdaptor.Clear();
            Result.Text = "Sum: 0\nCount: 0\nAverage: 0";
        }
        private async void ToolbarItemCopy_Clicked(object sender, EventArgs e)
        {
            Vibration.Vibrate(100);
            await Clipboard.SetTextAsync(_averageRes);
        }
        /// <summary>
        /// This is exactly <see cref="StringInList"/> but also with an ID parameter
        /// </summary>
        private class StringInListWithId
        {
            /// <summary>
            /// String to show in list
            /// </summary>
            public string ListString { get; set; }
            /// <summary>
            /// ID of string
            /// </summary>
            public ulong ID { get; set; }
            public override string ToString() => ListString;
        }

        private void Input_Completed(object sender, EventArgs e)
        {
            Button_Clicked(null, null);
            Input.Focus();
        }
    }
}