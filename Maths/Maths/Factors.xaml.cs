using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maths
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Factors : ContentPage
	{
		public Factors ()
		{
			InitializeComponent ();
		}
        private void Button_Clicked(object sender, EventArgs e)
        {
            ulong Number, Max;
            List<ulong> factors = new List<ulong>();
            //Get number from input
            try
            {
                Number = Convert.ToUInt64(FactorsInput.Text);
                if (Number < 1)
                    throw new FormatException();
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
            Max = (ulong)Math.Sqrt(Number);
            for (ulong i = 1; i <= Max; i++)
            {
                if (Number % i == 0)
                {
                    factors.Add(i);
                    factors.Add(Number / i);
                }
            }
            factors.Sort();
            if (factors[factors.Count / 2] == factors[factors.Count / 2 - 1])
                factors.RemoveAt(factors.Count / 2);
            ObservableCollection<StringInList> FactorsAdaptor = new ObservableCollection<StringInList>();
            //Show
            foreach(long a in factors)
            {
                FactorsAdaptor.Add(new StringInList { ListString = a.ToString() });
            }
            ResultList.ItemsSource = FactorsAdaptor;
            Result.Text = Number + " has " + factors.Count + " factors.";
        }
    }
    public class StringInList
    {
        public string ListString { get; set; }
    }
}