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
        /// Factorize a number to prime factors
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
        /// Checks if a number is prime. Returns 1 on prime numbers; Otherwise the first divisor is returned
        /// </summary>
        /// <param name="num">Number to check</param>
        /// <returns></returns>
        public static ulong CheckPrime(ulong number)
        {
            if (number == 2 || number == 3 || number == 5 || number == 7)
                return 1;
            if (number % 2 == 0)
                return 2;
            if (number % 3 == 0)
                return 3;
            ulong TO = (uint)Math.Sqrt(number);
            for (ulong i = 5; i <= TO; i += 6)
            {
                if (number % i == 0)
                    return i;
                if (number % (i + 2) == 0)
                    return i + 2;
            }
            return 1;
        }
    }
}
