//
//  PasswordGenerator.cs
//
//  Copyright (c) Wiregrass Code Technology 2020-2021
//
using System;

namespace IdentityManagement.Services
{
    internal static class PasswordGenerator
    {
        private const string lowers = "abcdefghijklmnopqrstuvwxyz";
        private const string uppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string number = "0123456789";

        internal static string GenerateRandom(int maximumLowerCase, int maximumUpperCase, int maximumNumerics)
        {
            var random = new Random();

            var generated = "!";

            for (var i = 1; i <= maximumLowerCase; i++)
            {
                generated = generated.Insert(random.Next(generated.Length), lowers[random.Next(lowers.Length - 1)].ToString());
            }
            for (var i = 1; i <= maximumUpperCase; i++)
            {
                generated = generated.Insert(random.Next(generated.Length), uppers[random.Next(uppers.Length - 1)].ToString());
            }
            for (var i = 1; i <= maximumNumerics; i++)
            {
                generated = generated.Insert(random.Next(generated.Length), number[random.Next(number.Length - 1)].ToString());
            }

            return generated.Replace("!", string.Empty, StringComparison.CurrentCulture);
        }
    }
}