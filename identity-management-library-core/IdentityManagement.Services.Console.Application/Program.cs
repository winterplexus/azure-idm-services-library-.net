//
//  Program.cs
//
//  Wiregrass Code Technology 2020-2023
//
namespace IdentityManagement.Services.Console.Application
{
    internal static class Program
    {
        private static async Task Main()
        {
            const string settingsPath = "appsettings.json";

            IIdentityManager identityManager = new IdentityManager(settingsPath);

            var menus = new Menus(identityManager);
            await menus.MainMenu().ConfigureAwait(false);
        }
    }
}