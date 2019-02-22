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
	public partial class PrimeCheck : ContentPage
	{
		public PrimeCheck ()
		{
			InitializeComponent ();
		}
        private void FindBTN_Clicked(object sender, EventArgs e)
        {
            ulong Number;
            //Get number from input
            try
            {
                Number = Convert.ToUInt64(InputEntry.Text);
            }
            catch (OverflowException)
            {
                DisplayAlert("Error", "The number you entered is too big.", "OK");
                return;
            }
            catch (FormatException)
            {
                DisplayAlert("Error", "Invalid number format. Number must be a natural number.", "OK");
                return;
            }
            catch (Exception ex)
            {
                DisplayAlert("Unhandled Exception", ex.ToString(), "OK");
                return;
            }
            if (Number < 2)
            {
                DisplayAlert("Error", "Number cannot be less than 2.", "OK");
                return;
            }
            ulong pTest = MathFunctions.CheckPrime(Number);
            if (pTest == 1)
                Result.Text = Number + " is prime.";
            else
                Result.Text = Number + " is not prime. It can be divided by " + pTest;
        }
    }
}