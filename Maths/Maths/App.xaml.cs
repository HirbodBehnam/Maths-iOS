using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Maths
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = Xamarin.Essentials.Preferences.ContainsKey(LanguageC.SavedName) ? new NavigationPage(new MainPage()) : new NavigationPage(new Language());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
