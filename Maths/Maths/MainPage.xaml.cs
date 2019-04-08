using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Maths
{
    public partial class MainPage : ContentPage
    {
        public static readonly Dictionary<string, string> MenuEnglishItems = new Dictionary<string, string>
        {
            { "Factors","Find factors of a number; 6-> 1,2,3,6" },
            { "Factorize","Factorize a number to prime factors" },
            { "Mod","Find remainder of division of two numbers" },
            { "GCD and LCM","Greatest common divisor or least common multiple" },
            { "Prime Checker","Detects if a number is prime or not" },
            { "Average Calculator","Find average of some numbers" },
            { "Quadratic Equation Solver","Solves a quadratic equation"}
        };
        public static readonly Dictionary<string, string> MenuPersianItems = new Dictionary<string, string>()
        {
            { "شمارنده ها","پیدا کردن شمارنده های یک عدد" },
            { "ب.م.م و ک.م.م","بزرگ ترین مقسوم علیه مشترک و کوچکترین مضرب مشترک" },
            { "باقی مانده","پیدا کردن باقی مانده ی دو عدد" },
            { "تشخیص عدد اول","تشخیص دادن اینکه عددی اول است یا نه" },
            { "تجزیه","یک عدد را به عوامل اول تجزیه می کند" },
            { "میانگین گیر","حساب کردن میانگین چندین عدد" },
            {"معادله درجه دو","حل کردن و پیدا کردن ریشه های یک معادله ی درجه دو"}
        };
        public static LanguageE SelectedLanguage = LanguageE.English;
        public MainPage()
        {
            InitializeComponent();
            SelectedLanguage = LanguageC.SavedLanguage();
            //Check for update
            new Task(() =>
            {
                try
                {
                    const string url = "https://hirbodbehnam.github.io//ios_updater.html";
                    using (WebClient client = new WebClient()) 
                    {
                        int remoteBuild = int.Parse(client.DownloadString(url));
                        int localBuild = int.Parse(VersionTracking.CurrentBuild);
                        if (remoteBuild > localBuild)
                        {
                            MainThread.BeginInvokeOnMainThread(() =>
                            {
                                ShowUpdateDialog(remoteBuild);
                            });
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }).Start();
            {//Setup main menu
                ObservableCollection<MenuItems> menu = new ObservableCollection<MenuItems>();
                if (SelectedLanguage == LanguageE.English)
                {
                    foreach (KeyValuePair<string, string> function in MenuEnglishItems.OrderBy(key => key.Key))
                        menu.Add(new MenuItems { Title = function.Key, Info = function.Value });
                    MainList.ItemTemplate = new DataTemplate(typeof(CustomCellLTR));
                }
                else
                {
                    foreach (KeyValuePair<string, string> function in MenuPersianItems.OrderBy(key => key.Key))
                        menu.Add(new MenuItems { Title = function.Key, Info = function.Value });
                    MainList.ItemTemplate = new DataTemplate(typeof(CustomCellRTL));
                }
                MainList.ItemsSource = menu;
                MainList.ItemTapped += ListView_ItemTapped;
                MainList.SelectionMode = ListViewSelectionMode.None;
            }
        }
        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            switch (e.Item.ToString())
            {
                case "Factors":
                case "شمارنده ها":
                    Navigation.PushAsync(new Factors());
                    break;
                case "GCD and LCM":
                case "ب.م.م و ک.م.م":
                    Navigation.PushAsync(new GCD());
                    break;
                case "Mod":
                case "باقی مانده":
                    Navigation.PushAsync(new ModPage());
                    break;
                case "Factorize":
                case "تجزیه":
                    Navigation.PushAsync(new Factorize());
                    break;
                case "Prime Checker":
                case "تشخیص عدد اول":
                    Navigation.PushAsync(new PrimeCheck());
                    break;
                case "Average Calculator":
                case "میانگین گیر":
                    Navigation.PushAsync(new AverageCalculator());
                    break;
                case "Quadratic Equation Solver":
                case "معادله درجه دو":
                    Navigation.PushAsync(new QuadraticEquationSolver());
                    break;
            }
        }
        private async void ShowUpdateDialog(int remoteBuild)
        {
            string message = SelectedLanguage == LanguageE.English ? "An update found for Maths to build version " : "یک آپدیت برای اپلیکیشن به شماره ی ساخت ";
            message += remoteBuild;
            if (SelectedLanguage == LanguageE.English)
                message += ". Do you want to update now?";
            else
            {
                message += " موجود است.";
                message += "آیا می خواهید که الان آپدیت کنید؟";
            }
            bool answer = await DisplayAlert(SelectedLanguage == LanguageE.English ? "Update" : "به روز رسانی",message, SelectedLanguage == LanguageE.English ? "No" : "خیر", SelectedLanguage == LanguageE.English ? "Yes" : "بله");
            if (!answer)
                Device.OpenUri(new Uri("https://new.sibapp.com/applications/maths"));  
        }
        private void MenuToolBarClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new About());
        }
    }
    public class CustomCellLTR : ViewCell
    {
        public CustomCellLTR()
        {
            //instantiate each of our views
            StackLayout cellWrapper = new StackLayout();
            StackLayout horizontalLayout = new StackLayout();
            Label title = new Label()
            {
                FontSize = 20
            };
            Label info = new Label();

            //set bindings
            title.SetBinding(Label.TextProperty, "Title");
            info.SetBinding(Label.TextProperty, "Info");

            //Set padding
            horizontalLayout.Padding = new Thickness(22);

            //add views to the view hierarchy
            horizontalLayout.Children.Add(title);
            horizontalLayout.Children.Add(info);
            cellWrapper.Children.Add(horizontalLayout);
            View = cellWrapper;
        }
    }
    public class CustomCellRTL : ViewCell
    {
        public CustomCellRTL()
        {
            //instantiate each of our views
            StackLayout cellWrapper = new StackLayout();
            StackLayout horizontalLayout = new StackLayout();
            Label title = new Label()
            {
                FontSize = 20,
                HorizontalOptions = new LayoutOptions(LayoutAlignment.End, true)
            };
            Label info = new Label()
            {
                HorizontalOptions = new LayoutOptions(LayoutAlignment.End, true),
                HorizontalTextAlignment = TextAlignment.End
            };

            //set bindings
            title.SetBinding(Label.TextProperty, "Title");
            info.SetBinding(Label.TextProperty, "Info");

            //Set padding
            horizontalLayout.Padding = new Thickness(22);

            //add views to the view hierarchy
            horizontalLayout.Children.Add(title);
            horizontalLayout.Children.Add(info);
            cellWrapper.Children.Add(horizontalLayout);
            View = cellWrapper;
        }
    }
    public class MenuItems
    {
        public string Title { get; set; }
        public string Info { get; set; }
        public override string ToString()
        {
            return Title;
        }
    }
}
