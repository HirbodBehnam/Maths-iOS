using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maths
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GCD : ContentPage
	{
		public GCD ()
		{
			InitializeComponent ();
		}

        private void Find_Clicked(object sender, EventArgs e)
        {
            ulong[] nums;
            {//Get the numbers
                int i = 0;
                string[] numbersSplitted = EntryNumbers.Text.Split(',');
                nums = new ulong[numbersSplitted.Length];
                try
                {
                    for (; i < numbersSplitted.Length; i++)
                    {
                        nums[i] = Convert.ToUInt64(numbersSplitted[i].Trim());
                        if (nums[i] == 0)
                            throw new FormatException();
                        if (nums[i] == 1)
                        {
                            ResultLBL.Text = "The GCD is: 1";
                            return;
                        }
                    }
                }
                catch (OverflowException)
                {
                    DisplayAlert("Error", $"Too big number on index {i + 1} ({numbersSplitted[i]}).", "OK");
                    return;
                }
                catch (FormatException)
                {
                    DisplayAlert("Error", $"Invalid number format on index {i+1} ({numbersSplitted[i]}). Number must be a natural number.", "OK");
                    return;
                }
                catch (Exception ex)
                {
                    DisplayAlert("Unhandled Exception",ex.ToString() , "OK");
                    return;
                }
            }
            if (nums.Length < 2)
            {
                DisplayAlert("Error", "Enter at least 2 numbers.", "OK");
                return;
            }
            if (LCMorGCDSwitch.IsToggled) //LCM
            {
                BigInteger big = new BigInteger(nums[0]);
                BigInteger numTemp;
                for(int i = 1; i < nums.Length; i++)
                {
                    numTemp = nums[i];
                    big = (big / BigInteger.GreatestCommonDivisor(big, numTemp) * numTemp);
                }
                ResultLBL.Text = "The LCM is: " + big;
            }
            else //GCD
            {
                for(int i = nums.Length; i-- > 1;)
                    nums[i - 1] = MathFunctions.GCD(nums[i - 1], nums[i]);           
                ResultLBL.Text = "The GCD is: " + nums[0];
            }
        }

        private void LCMorGCDSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            ResultLBL.Text = "The " + (LCMorGCDSwitch.IsToggled ? "LCM" : "GCD") +" is:" ;
        }
    }
}