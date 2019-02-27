using System;

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
            LanguageC.SaveLanguage(LanguageE.English);
            Navigation.PushAsync(new MainPage());
            Navigation.RemovePage(this);
        }

        private void Persian_Clicked(object sender, EventArgs e)
        {
            LanguageC.SaveLanguage(LanguageE.Persian);
            Navigation.PushAsync(new MainPage());
            Navigation.RemovePage(this);
        }
    }
}