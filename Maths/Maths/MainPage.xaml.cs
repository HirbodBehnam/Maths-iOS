using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Maths
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            switch (e.Item.ToString())
            {
                case "Factors":
                    Navigation.PushAsync(new Factors());
                    break;
                case "GCD and LCM":
                    Navigation.PushAsync(new GCD());
                    break;
                case "Mod":
                    Navigation.PushAsync(new ModPage());
                    break;
                case "Factorize":
                    Navigation.PushAsync(new Factorize());
                    break;
                case "Prime Checker":
                    Navigation.PushAsync(new PrimeCheck());
                    break;
            }
        }

        private void MenuToolBarClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new About());
        }
    }
}
