using System;

using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maths
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Language : ContentPage
    {
        public Language()
        {
            InitializeComponent();
        }

        private void English_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
            Navigation.RemovePage(this);
            Preferences.Set(LanguageC.SavedName, 0);
        }

        private void Persian_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
            Navigation.RemovePage(this);
            Preferences.Set(LanguageC.SavedName, 1);
        }
    }
}