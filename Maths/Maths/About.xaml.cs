using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Maths
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class About : ContentPage
	{
		public About ()
		{
			InitializeComponent ();
		}

        private void AndroidButton_Clicked(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("https://cafebazaar.ir/app/com.hirbod.maths"));
        }

        private void BugReport_Clicked(object sender, EventArgs e)
        {
            Device.OpenUri(new Uri("mailto:chrome.hiri.angry@gmail.com"));
        }
    }
}