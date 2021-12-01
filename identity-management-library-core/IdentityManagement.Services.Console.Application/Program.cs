//
//  Program.cs
//
//  Wiregrass Code Technology 2020-2022
//
namespace IdentityManagement.Services.Console.Application
{
    internal static class Program
    {
        private static void Main()
        {
            const string settingsPath = "appsettings.json";

            IIdentityManager identityManager = new IdentityManager(settingsPath);

            var menus = new Menus(identityManager);
            menus.MainMenu();
        }
    }
}