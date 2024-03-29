﻿using System;
using System.Numerics;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maths
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ModPage : ContentPage
	{
        private string _res = "0";
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
            big1 %= big2; //Now big1 is mod
            _res = big1.ToString();
            if (big1 > (big2 / 2))
            {
                big1 -= big2;
                _res += " ( ";
                if (LanguageC.SavedLanguage() == LanguageE.English)
                    _res += big1.ToString();
                else
                {
                    big1 = -big1;
                    _res += big1 + "-";
                }
                _res += " )";
            }
            if (LanguageC.SavedLanguage() == LanguageE.English)
                Result.Text = "Reminder is " + _res;
            else
                Result.Text = "باقی مانده برابر است با " + _res;
        }
        private async void ToolbarItemCopy_Clicked(object sender, EventArgs e)
        {
            Vibration.Vibrate(100);
            await Clipboard.SetTextAsync(_res);
        }

        private void ModDividend_Completed(object sender, EventArgs e) => ModDivisor.Focus();

        private void ModDivisor_Completed(object sender, EventArgs e) => FindBTN_Clicked(null, null);
    }
}