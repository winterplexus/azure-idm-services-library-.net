//
//  CommandOptions.cs
//
//  Wiregrass Code Technology 2020-2023
//
namespace IdentityManagement.Services.Utility.Tool
{
    internal static class CommandOptions
    {
        private const char emailAaddressFlag = 'e';
        private const char telephoneNumberFlag = 't';
        private const char generatePasswordFlag = 'p';
        private const char maximumLowercaseLettersFlag = 'l';
        private const char maximumUppercaseLettersFlag = 'u';
        private const char maximumNumbersFlag = 'n';
        private const char maximumSymbolCharactersFlag = 's';

        public static bool Parse(string[] arguments, CommandParameters parameters)
        {
            if (arguments == null || arguments.Length < 1)
            {
                DisplayUsage();
                return false;
            }

            if (parameters == null)
            {
                throw new ArgumentException("parameters argument is null");
            }

            for (var index = 0; index < arguments.Length; index++)
            {
                if (arguments[index][0] != '-')
                {
                    Console.WriteLine($"error-> option or option value is missing (argument index {index}): {arguments[index]}");
                    return false;
                }
                if (arguments[index].Length <= 1)
                {
                    continue;
                }

                var option = arguments[index][1];

                index++;

                bool invalidValue;

                switch (option)
                {
                    case emailAaddressFlag:
                         invalidValue = ParseEmailAddress(arguments, index, parameters);
                         break;
                    case telephoneNumberFlag:
                         invalidValue = ParseTelephoneNumber(arguments, index, parameters);
                         break;
                    case generatePasswordFlag:
                         parameters.GeneratePassword = true;
                         invalidValue = true;
                         break;
                    case maximumLowercaseLettersFlag:
                         invalidValue = ParseMaximumLowercaseLetters(arguments, index, parameters);
                         break;
                    case maximumUppercaseLettersFlag:
                         invalidValue = ParseMaximumUppercaseLetters(arguments, index, parameters);
                         break;
                    case maximumNumbersFlag:
                         invalidValue = ParseMaximumNumbers(arguments, index, parameters);
                         break;
                    case maximumSymbolCharactersFlag:
                         invalidValue = ParseMaximumSymbolCharacters(arguments, index, parameters);
                         break;
                    default:
                         Console.WriteLine($"error-> unknown option: {option}");
                         return false;
                }

                if (!invalidValue)
                {
                    return false;
                }
            }

            return true;
        }

        private static bool ParseEmailAddress(IReadOnlyList<string> arguments, int i, CommandParameters parameters)
        {
            if (arguments.Count <= i || string.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> email address is missing");
                return false;
            }
            parameters.EmailAddress = arguments[i];
            return true;
        }

        private static bool ParseTelephoneNumber(IReadOnlyList<string> arguments, int i, CommandParameters parameters)
        {
            if (arguments.Count <= i || string.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> telephone number is missing");
                return false;
            }
            parameters.TelephoneNumber = arguments[i];
            return true;
        }

        private static bool ParseMaximumLowercaseLetters(IReadOnlyList<string> arguments, int i, CommandParameters parameters)
        {
            if (arguments.Count <= i || string.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> maximum lowercase letters value is missing");
                return false;
            }
            parameters.MaximumLowercaseLetters = Assistant.GetIntegerValue(arguments[i]);
            return true;
        }

        private static bool ParseMaximumUppercaseLetters(IReadOnlyList<string> arguments, int i, CommandParameters parameters)
        {
            if (arguments.Count <= i || string.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> maximum uppercase letters value is missing");
                return false;
            }
            parameters.MaximumUppercaseLetters = Assistant.GetIntegerValue(arguments[i]);
            return true;
        }

        private static bool ParseMaximumNumbers(IReadOnlyList<string> arguments, int i, CommandParameters parameters)
        {
            if (arguments.Count <= i || string.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> maximum numbers value is missing");
                return false;
            }
            parameters.MaximumNumbers = Assistant.GetIntegerValue(arguments[i]);
            return true;
        }

        private static bool ParseMaximumSymbolCharacters(IReadOnlyList<string> arguments, int i, CommandParameters parameters)
        {
            if (arguments.Count <= i || string.IsNullOrEmpty(arguments[i]))
            {
                Console.WriteLine("error-> maximum symbol characters value is missing");
                return false;
            }
            parameters.MaximumSymbolCharacters = Assistant.GetIntegerValue(arguments[i]);
            return true;
        }

        private static void DisplayUsage()
        {
            Console.WriteLine($"usage: IdentityManagement.Services.Utility.Tool (options){Environment.NewLine}");
            Console.WriteLine($"options: -{emailAaddressFlag} [email address to validate]");
            Console.WriteLine($"\t -{telephoneNumberFlag} [telephone number to validate]");
            Console.WriteLine($"\t -{generatePasswordFlag} <generate password>");
            Console.WriteLine($"\t -{maximumLowercaseLettersFlag} [generate password maximum lowercase letters: default 8]");
            Console.WriteLine($"\t -{maximumUppercaseLettersFlag} [generate password maximum uppercase letters: default 8]");
            Console.WriteLine($"\t -{maximumNumbersFlag} [generate password maximum numbers: default 8]");
            Console.WriteLine($"\t -{maximumSymbolCharactersFlag} [generate password maximum symbol characters: default 8]");
        }
    }
}