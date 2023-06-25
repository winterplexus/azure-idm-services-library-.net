//
//  PasswordGenerator.cs
//
//  Wiregrass Code Technology 2020-2023
//
using System.Security.Cryptography;

namespace IdentityManagement.Services.Utility
{
    public static class PasswordGenerator
    {
        private const string lowers  = "abcdefghijklmnopqrstuvwxyz";
        private const string uppers  = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string numbers = "0123456789";
        private const string symbols = "@#*()+={}/?~;,.-_";

        public static string GenerateRandom(int maximumLowerCase, int maximumUpperCase)
        {
            var generated = "!";

            for (var i = 1; i <= maximumLowerCase; i++)
            {
                var startIndex = RandomNumberGenerator.GetInt32(generated.Length);
                var valueIndex = RandomNumberGenerator.GetInt32(lowers.Length - 1);

                generated = generated.Insert(startIndex, lowers[valueIndex].ToString());
            }
            for (var i = 1; i <= maximumUpperCase; i++)
            {
                var startIndex = RandomNumberGenerator.GetInt32(generated.Length);
                var valueIndex = RandomNumberGenerator.GetInt32(uppers.Length - 1);

                generated = generated.Insert(startIndex, uppers[valueIndex].ToString());
            }

            return generated.Replace("!", string.Empty, StringComparison.CurrentCulture);
        }

        public static string GenerateRandom(int maximumLowerCase, int maximumUpperCase, int maximumNumbers)
        {
            var generated = "!";

            for (var i = 1; i <= maximumLowerCase; i++)
            {
                var startIndex = RandomNumberGenerator.GetInt32(generated.Length);
                var valueIndex = RandomNumberGenerator.GetInt32(lowers.Length - 1);

                generated = generated.Insert(startIndex, lowers[valueIndex].ToString());
            }
            for (var i = 1; i <= maximumUpperCase; i++)
            {
                var startIndex = RandomNumberGenerator.GetInt32(generated.Length);
                var valueIndex = RandomNumberGenerator.GetInt32(uppers.Length - 1);

                generated = generated.Insert(startIndex, uppers[valueIndex].ToString());
            }
            for (var i = 1; i <= maximumNumbers; i++)
            {
                var startIndex = RandomNumberGenerator.GetInt32(generated.Length);
                var valueIndex = RandomNumberGenerator.GetInt32(numbers.Length - 1);

                generated = generated.Insert(startIndex, numbers[valueIndex].ToString());
            }

            return generated.Replace("!", string.Empty, StringComparison.CurrentCulture);
        }

        public static string GenerateRandom(int maximumLowerCase, int maximumUpperCase, int maximumNumbers, int maximumSymbols)
        {
            var generated = "!";

            for (var i = 1; i <= maximumLowerCase; i++)
            {
                var startIndex = RandomNumberGenerator.GetInt32(generated.Length);
                var valueIndex = RandomNumberGenerator.GetInt32(lowers.Length - 1);

                generated = generated.Insert(startIndex, lowers[valueIndex].ToString());
            }
            for (var i = 1; i <= maximumUpperCase; i++)
            {
                var startIndex = RandomNumberGenerator.GetInt32(generated.Length);
                var valueIndex = RandomNumberGenerator.GetInt32(uppers.Length - 1);

                generated = generated.Insert(startIndex, uppers[valueIndex].ToString());
            }
            for (var i = 1; i <= maximumNumbers; i++)
            {
                var startIndex = RandomNumberGenerator.GetInt32(generated.Length);
                var valueIndex = RandomNumberGenerator.GetInt32(numbers.Length - 1);

                generated = generated.Insert(startIndex, numbers[valueIndex].ToString());
            }
            for (var i = 1; i <= maximumSymbols; i++)
            {
                var startIndex = RandomNumberGenerator.GetInt32(generated.Length);
                var valueIndex = RandomNumberGenerator.GetInt32(symbols.Length - 1);

                generated = generated.Insert(startIndex, symbols[valueIndex].ToString());
            }

            return generated.Replace("!", string.Empty, StringComparison.CurrentCulture);
        }
    }
}