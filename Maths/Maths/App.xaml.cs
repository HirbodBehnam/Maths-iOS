using System;
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
            if (Xamarin.Essentials.Preferences.ContainsKey(LanguageC.SavedName))
                MainPage = new NavigationPage(new MainPage());
            else
                MainPage = new NavigationPage(new Language());
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
