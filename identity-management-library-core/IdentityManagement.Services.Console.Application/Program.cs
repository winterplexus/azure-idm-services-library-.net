//
//  Program.cs
//
//  Copyright (c) Wiregrass Code Technology 2020
//
using IdentityManagement.Services;

namespace IdentityManagement.Services.Console.Application
{
    public static class Program
    {
        public static void Main()
        {
            IIdentityManager identityManager = new IdentityManager("appsettings.json");

            var menus = new Menus(identityManager);
            menus.MainMenu();
        }
    }
}