//
//  CommandParameters.cs
//
//  Wiregrass Code Technology 2020-2023
//
namespace IdentityManagement.Services.Utility.Tool
{
    internal sealed class CommandParameters
    {
        public string TelephoneNumber { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public bool GeneratePassword { get; set; }
        public int MaximumLowercaseLetters { get; set; } = 8;
        public int MaximumUppercaseLetters { get; set; } = 8;
        public int MaximumNumbers { get; set; } = 8;
        public int MaximumSymbolCharacters { get; set; } = 8;
    }
}