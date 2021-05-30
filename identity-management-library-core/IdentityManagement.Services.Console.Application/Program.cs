//
//  Program.cs
//
//  Wiregrass Code Technology 2020-2021
//
namespace IdentityManagement.Services.Console.Application
{
    internal static class Program
    {
        private static void Main()
        {
            IIdentityManager identityManager = new IdentityManager("appsettings.json");

            var menus = new Menus(identityManager);
            menus.MainMenu();
        }
    }
}