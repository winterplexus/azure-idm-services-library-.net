//
//  TelephoneNumberValidator.cs
//
//  Wiregrass Code Technology 2020-2023
//
using System.Text.RegularExpressions;

namespace IdentityManagement.Services.Utility
{
    public static partial class TelephoneNumberValidator
    {
        [GeneratedRegex("^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$")]
        private static partial Regex PhoneNumberRegex();

        public static bool IsValid(string phoneNumber)
        {
            if (phoneNumber != null)
            {
                return PhoneNumberRegex().IsMatch(phoneNumber);
            }
            else
            {
                return false;
            }
        }
    }
}