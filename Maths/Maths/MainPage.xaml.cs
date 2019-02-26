using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Maths
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            //Check for update
            new Task(() =>
            {
                try
                {
                    string URL = "https://hirbodbehnam.github.io//ios_updater.html";
                    using (WebClient client = new WebClient()) 
                    {
                        int remoteBuild = int.Parse(client.DownloadString(URL));
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
        async void ShowUpdateDialog(int RemoteBuild)
        {
            bool answer = await DisplayAlert("Update", "An update found for Maths to build version " + RemoteBuild + ". Do you want to update now?", "No", "Yes");
            if (!answer)
                Device.OpenUri(new Uri("https://new.sibapp.com/applications/maths"));  
        }
        private void MenuToolBarClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new About());
        }
    }
}
