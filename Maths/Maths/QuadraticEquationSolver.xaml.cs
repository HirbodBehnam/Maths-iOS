using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Maths
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class QuadraticEquationSolver : ContentPage
	{
		public QuadraticEquationSolver ()
		{
			InitializeComponent ();
            if (MainPage.SelectedLanguage == LanguageE.Persian)
            {
                LabelInfo.Text = "معادله ی زیر را در نظر بگیرید. آنگاه a و b و c را وارد کنید.";
                ButtonCalculate.Text = "حل کن";
                Title = "معادله درجه دو";
            }
		}

        private void EntryC_OnCompleted(object sender, EventArgs e)
        {
            ButtonCalculate_OnClicked(null, null);
        }

        private void ButtonCalculate_OnClicked(object sender, EventArgs e)
        {
            double a, b, c;
            try
            {
                a = Convert.ToDouble(EntryA.Text);
                b = Convert.ToDouble(EntryB.Text);
                c = Convert.ToDouble(EntryC.Text);
                if (CountDecimal(EntryA.Text) > 15 || CountDecimal(EntryB.Text) > 15 ||
                    CountDecimal(EntryC.Text) > 15)
                {
                    DisplayAlert(MainPage.SelectedLanguage == LanguageE.English ? "Warning" : "اخطار",
                        MainPage.SelectedLanguage == LanguageE.English ? "One of your variables has more than 15 decimal points. I rounded it up to 100 decimal points." : "یکی از اعداد شما بیشتر از پانزده رقم اعشار دارد. برنامه آن عدد را به پانزده رفم اعشار گرد کرد.",
                        MainPage.SelectedLanguage == LanguageE.English ? "OK" : "باشه");
                    a = Math.Round(a, 15);
                    b = Math.Round(b, 15);
                    c = Math.Round(c, 15);
                }
                if (Math.Abs(a) < double.Epsilon)
                {
                    DisplayAlert(MainPage.SelectedLanguage == LanguageE.English ? "Error" : "خطا",
                        MainPage.SelectedLanguage == LanguageE.English ? "\"a\" cannot be zero." : "مقدار a نمی تواند صفر باشد.",
                        MainPage.SelectedLanguage == LanguageE.English ? "OK" : "باشه");
                    return;
                }
            }
            catch (OverflowException)
            {
                DisplayAlert(MainPage.SelectedLanguage == LanguageE.English ? "Error" : "خطا",
                    MainPage.SelectedLanguage == LanguageE.English ? "The number you entered is too big." : "عدد وارد شده بیش از حد بزرگ است.",
                    MainPage.SelectedLanguage == LanguageE.English ? "OK" : "باشه");
                return;
            }
            catch (FormatException)
            {
                DisplayAlert(MainPage.SelectedLanguage == LanguageE.English ? "Error" : "خطا",
                    MainPage.SelectedLanguage == LanguageE.English ? "Invalid number format. Number must be a natural number." : "عدد وارد شده نامعتبر است. عدد باید عدد طبیعی باشد.",
                    MainPage.SelectedLanguage == LanguageE.English ? "OK" : "باشه");
                return;
            }
            catch (Exception ex)
            {
                DisplayAlert("Unhandled Exception", ex.ToString(), "OK");
                return;
            }
            double delta = b * b - 4 * a * c;
            if (delta < 0)
            {
                LabelResult.Text = "Δ(Delta) is less than 0.";
                return;
            }
            if (Math.Abs(delta) < double.Epsilon)
            {
                LabelResult.Text = "𝑥 = " + Convert.ToString((-b / (2 * a)), CultureInfo.CurrentCulture);
                return;
            }
            //Normalize first root
            b = -b;
            a *= 2;
            if (delta.IsInteger() && delta < ulong.MaxValue && a.IsInteger() && a < ulong.MaxValue && b.IsInteger() && b <ulong.MaxValue)
            {
                ulong uDelta = Convert.ToUInt64(delta);
                ulong[] deltaNormalized = {1, 1}; //deltaNormalized[0] = multiplier, deltaNormalized[1] = insideRoot
                foreach (var variable in NumberOfOccurring(MathFunctions.Factorize(uDelta)))
                {
                    deltaNormalized[0] *= SimplePower(variable.Key, variable.Value / 2);
                    deltaNormalized[1] *= SimplePower(variable.Key, variable.Value % 2);
                }

                ulong newA = (ulong)Math.Abs(a),newB = (ulong)Math.Abs(b);
                ulong gcd = MathFunctions.MultiGCD(deltaNormalized[0], newB, newA);
                if (gcd != 1)
                {
                    deltaNormalized[0] /= gcd;
                    newA /= gcd;
                    newB /= gcd;
                }
                delta = Math.Sqrt(delta);
                string x1 = Convert.ToString(((b + delta) / a), CultureInfo.CurrentCulture),x2 = Convert.ToString(((b - delta) / a), CultureInfo.CurrentCulture);
                if (newA == 1)
                {
                    if (a > 0)
                        LabelResult.Text = "𝑥 = " + Sign(b) + newB + " ± " +
                                           RootSign(deltaNormalized[0], deltaNormalized[1]);
                    else
                        LabelResult.Text = "𝑥 = " + (b > 0 ? "-" : "") + newB + " ± " +
                                           RootSign(deltaNormalized[0], deltaNormalized[1]);
                    LabelResult.Text += "\n";
                }
                else
                {
                    LabelResult.Text = "𝑥 = (" + Sign(b) + newB + " ± " +
                                       RootSign(deltaNormalized[0], deltaNormalized[1]) + ") / " + Sign(a) + newA;
                }
                LabelResult.Text += "𝑥₁ = " + x1 + "\n"
                                   + "𝑥₂ = " + x2;
            }
            else
            {
                delta = Math.Sqrt(delta);
                LabelResult.Text = "𝑥₁ = " + Convert.ToString(((b + delta) / a), CultureInfo.CurrentCulture) + "\n"
                                    + "𝑥₂ = " + Convert.ToString(((b - delta) / a), CultureInfo.CurrentCulture);
            }
        }
        /// <summary>
        /// Find occurring of elements in a SORTED array
        /// </summary>
        /// <param name="array">Array to check</param>
        /// <returns></returns>
        private static Dictionary<ulong, ulong> NumberOfOccurring(ulong[] array)
        {
            List<ulong> l = array.ToList();
            Dictionary<ulong, ulong> res = new Dictionary<ulong, ulong>();
            while (l.Count > 0)
            {
                ulong now = l[0];
                ulong occur = 0;
                while (l.Count > 0)
                {
                    if (l[0] == now)
                    {
                        l.RemoveAt(0);
                        occur++;
                    }
                    else
                    {
                        break;
                    }
                }
                res.Add(now,occur);
            }
            return res;
        }
        /// <summary>
        /// Count decimal points in string
        /// </summary>
        /// <param name="s">Number to check</param>
        /// <returns></returns>
        private static int CountDecimal(string s)
        {
            if (!s.Contains("."))
                return 0;
            while (s[s.Length - 1] == '0')
                s = s.Remove(s.Length - 1);
            s = s.Substring(s.IndexOf('.'));
            return s.Length - 1;
        }
        /// <summary>
        /// Simple power algorithm for small ulong values
        /// </summary>
        /// <param name="Base">Base of power</param>
        /// <param name="power">Power</param>
        /// <returns></returns>
        private static ulong SimplePower(ulong Base, ulong power)
        {
            ulong res = 1;
            for (; power > 0; power--)
                res *= Base;
            return res;
        }
        private static string Sign(double num) => num < 0 ? "-" :"";
        private static string RootSign(ulong multiplier, ulong inside)
        {
            if (inside == 1)
                return multiplier.ToString();
            if (multiplier == 1)
                return "√(" + inside + ")";
            return multiplier + "√(" + inside + ")";
        }
    }
}