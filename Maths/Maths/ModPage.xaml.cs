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
            if(LanguageC.SavedLanguage() == LanguageE.Persian)
            {
                Title = "باقی مانده";
                Info.Text = "مقسوم و مقسوم علیه را وارد کنید";
                ModDividend.Placeholder = "مقسوم";
                ModDivisor.Placeholder = "مقسوم علیه";
                FindBTN.Text = "حساب کن";
                Result.HorizontalOptions = new LayoutOptions(LayoutAlignment.End, true);
                Result.HorizontalTextAlignment = TextAlignment.End;
            }
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
            string res = (big1 % big2).ToString();
            if (LanguageC.SavedLanguage() == LanguageE.English)
                Result.Text = "Reminder is " + res;
            else
                Result.Text = "باقی مانده برابر است با " + res;
        }
    }
}