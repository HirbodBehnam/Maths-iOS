using System;
using System.Numerics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maths
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ModPage : ContentPage
	{
		public ModPage ()
		{
			InitializeComponent ();
		}

        private void FindBTN_Clicked(object sender, EventArgs e)
        {
            BigInteger big1,big2;
            try
            {
                big1 = BigInteger.Parse(ModDividend.Text);
                big2 = BigInteger.Parse(ModDivisor.Text);
            }catch(Exception)
            {
                DisplayAlert("Error", "Cannot parse numbers. Decimal numbers are not allowed.", "OK");
                return;
            }
            if(big1.Sign < 1 || big2.Sign < 1)
            {
                DisplayAlert("Error", "Numbers cannot be less than 1.", "OK");
                return;
            }
            Result.Text = "Reminder is " + (big1 % big2);
        }
    }
}