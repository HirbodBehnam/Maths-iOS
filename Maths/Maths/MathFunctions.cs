using System;
using System.Collections.Generic;

namespace Maths
{
    public static class MathFunctions
    {
        /// <summary>
        /// Find GCD of two numbers
        /// </summary>
        /// <param name="a">First number</param>
        /// <param name="b">Second Number</param>
        /// <returns>The GCD of two numbers</returns>
        public static ulong GCD(ulong a , ulong b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }
        /// <summary>
        /// Find GCD of multiple numbers
        /// </summary>
        /// <param name="numbers">Numbers</param>
        /// <returns>GCD</returns>
        public static ulong MultiGCD(params ulong[] numbers)
        {
            if(numbers.Length < 2)
                throw new ArgumentException("Enter at least 2 numbers.");
            ulong res = GCD(numbers[0], numbers[1]);
            for (int i = 2; i < numbers.Length; i++)
                res = GCD(res, numbers[i]);
            return res;
        }
        /// <summary>
        /// Factorize numbers number to prime factors
        /// </summary>
        /// <param name="num">The number to factorize</param>
        /// <returns>List of prime factors</returns>
        public static ulong[] Factorize(ulong num)
        {
            List<ulong> factorized = new List<ulong>();
            while (num % 2 == 0)
            {
                num /= 2;
                factorized.Add(2);
            }
            while (num % 3 == 0)
            {
                num /= 3;
                factorized.Add(3);
            }
            for(ulong i = 5;num > 1; i+=4)
            {
                while(num % i == 0)
                {
                    num /= i;
                    factorized.Add(i);
                }
                i += 2;
                while (num % i == 0)
                {
                    num /= i;
                    factorized.Add(i);
                }
            }
            return factorized.ToArray();
        }
        /// <summary>
        /// Checks if numbers double is integer https://stackoverflow.com/numbers/2751597/4213397
        /// </summary>
        /// <param name="d">Double to check</param>
        /// <returns>True if integer</returns>
        public static bool IsInteger(this double d) => Math.Abs(d % 1) < double.Epsilon;
    }
    /// <summary>
    /// This class is used for single cell list views
    /// </summary>
    public class StringInList
    {
        public string ListString { get; set; }
        public override string ToString() => ListString;
    }
}
