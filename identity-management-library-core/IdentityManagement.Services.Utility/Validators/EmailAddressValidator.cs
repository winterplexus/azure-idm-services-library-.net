//
//  EmailAddressValidator.cs
//
//  Wiregrass Code Technology 2020-2023
//
using System.Text.RegularExpressions;

namespace IdentityManagement.Services.Utility
{
    public static partial class EmailAddressValidator
    {
        [GeneratedRegex("^[a-zA-Z0-9.!#$%&'*+\\/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$", RegexOptions.IgnoreCase, "en-US")]
        private static partial Regex EmailAddressRegex();

        public static bool IsValid(string emailAddress, bool fullStopRequired = false)
        {
            var status = false;

            if (emailAddress != null)
            {
                status = EmailAddressRegex().IsMatch(emailAddress);
                if (status && fullStopRequired)
                {
                    var parts = emailAddress.Split('@', StringSplitOptions.RemoveEmptyEntries);
                    status = parts.Length == 2 && parts[1].Contains('.', StringComparison.InvariantCulture);
                }
            }

            return status;
        }
    }
}