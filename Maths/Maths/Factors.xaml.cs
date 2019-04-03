using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maths
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Factors : ContentPage
	{
        private ulong _i;
        private bool _done;
        public Factors ()
		{
			InitializeComponent ();
            if(MainPage.SelectedLanguage == LanguageE.Persian)
            {
                Title = "شمارنده ها";
                Info.Text = MainPage.MenuPersianItems["شمارنده ها"];
                FindBTN.Text = "پیدا کن";
                FactorsInput.Placeholder = "عدد را وارد کنید";
                Result.HorizontalOptions = new LayoutOptions(LayoutAlignment.End, true);
                Result.HorizontalTextAlignment = TextAlignment.End;
                popupLabel.Text = "در حال محاسبه...";
            }
		}
        public async void OnCopy(object sender, EventArgs e)
        {
            StringInList mi = (StringInList)((MenuItem)sender).CommandParameter;
            Vibration.Vibrate(100);
            await Clipboard.SetTextAsync(mi.ListString);
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            ulong number;
            //Get number from input
            try
            {
                number = Convert.ToUInt64(FactorsInput.Text);
                if (number < 1)
                    throw new FormatException();
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
            _done = false;
            ulong to = (ulong)Math.Sqrt(number);
            _i = 0;
            popupLoadingView.IsVisible = true;
            new Task(() =>
            {
                List<ulong> factors = new List<ulong>();
                for (_i = 1; _i <= to; _i++)
                {
                    if (number % _i == 0)
                    {
                        factors.Add(_i);
                        factors.Add(number / _i);
                    }
                }
                factors.Sort();
                if (factors[factors.Count / 2] == factors[factors.Count / 2 - 1])
                    factors.RemoveAt(factors.Count / 2);
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    ObservableCollection<StringInList> factorsAdapter = new ObservableCollection<StringInList>();
                    //Show
                    foreach (ulong a in factors)
                        factorsAdapter.Add(new StringInList { ListString = a.ToString() });
                    ResultList.ItemsSource = factorsAdapter;
                    if (MainPage.SelectedLanguage == LanguageE.English)
                        Result.Text = number + " has " + factors.Count + " factors.";
                    else
                    {
                        Result.Text = "عدد ";
                        Result.Text += number;
                        Result.Text += " دارای ";
                        Result.Text += factors.Count;
                        Result.Text += " شمارنده است.";
                    }
                    popupLoadingView.IsVisible = false;
                });
                _done = true;
            }).Start();
            new Task(() =>
            {
                while (!_done)
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        popupProgress.Progress = (double)_i / to;
                    });
                    Thread.Sleep(200);
                }
            }).Start();
        }

        private void FactorsInput_Completed(object sender, EventArgs e)
        {
            Button_Clicked(null, null);
        }
    }
}