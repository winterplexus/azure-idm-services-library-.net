//
//  Program.cs
//
//  Wiregrass Code Technology 2020-2023
//
namespace IdentityManagement.Services.Utility.Tool
{
    internal sealed class Program
    {
        internal static void Main(string[] arguments)
        {
            var parameters = new CommandParameters();
            if (CommandOptions.Parse(arguments, parameters))
            {
                if (!string.IsNullOrEmpty(parameters.EmailAddress))
                {
                    var status = EmailAddressValidator.IsValid(parameters.EmailAddress);
                    if (status)
                    {
                        Console.WriteLine("email address is valid");
                    }
                    else
                    {
                        Console.WriteLine("email address is invalid");
                    }
                }
                if (!string.IsNullOrEmpty(parameters.TelephoneNumber))
                {
                    var status = TelephoneNumberValidator.IsValid(parameters.TelephoneNumber);
                    if (status)
                    {
                        Console.WriteLine("telephone number is valid");
                    }
                    else
                    {
                        Console.WriteLine("telephone number is invalid");
                    }
                }
                if (parameters.GeneratePassword)
                {
                    Console.WriteLine($"generated password: {PasswordGenerator.GenerateRandom(parameters.MaximumLowercaseLetters,
                                                                                              parameters.MaximumUppercaseLetters,
                                                                                              parameters.MaximumNumbers,
                                                                                              parameters.MaximumSymbolCharacters)}");
                }
            }
        }
    }
}