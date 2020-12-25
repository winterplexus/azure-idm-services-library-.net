﻿//
//  Menus.cs
//
//  Copyright (c) Wiregrass Code Technology 2020
//
using System;
using System.Globalization;

namespace IdentityManagement.Services.Console.Application
{
    internal class Menus
    {
        private readonly IIdentityManager identityManager;

        internal Menus(IIdentityManager identityManager)
        {
            this.identityManager = identityManager;
        }

        internal void MainMenu()
        {
            try
            {
                while (true)
                {
                    WriteMainMenu();

                    var command = ReadMenuOption();
                    switch (command)
                    {
                        case "U": ProcessManageUsersMenu();
                                  break;
                        case "G": ProcessManageGroupsMenu();
                                  break;
                        case "X": return;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteUnexpectedException(ex);
                ReadContinue();
            }
        }

        private void ProcessManageUsersMenu()
        {
            var manageUsers = new ManageUsers(identityManager);

            while (true)
            {
                WriteManageUsersMenu();

                var command = ReadMenuOption();
                switch (command)
                {
                    case "1": manageUsers.GetUserBySignInName();
                              break;
                    case "2": manageUsers.GetUserByDisplayName();
                              break;
                    case "3": manageUsers.GetUserByObjectId();
                              break;
                    case "4": ProcessManageUsersSubmenu();
                              break;
                    case "5": manageUsers.SetUserPassword();
                              break;
                    case "6": manageUsers.CreateUser();
                              break;
                    case "7": manageUsers.DeleteUser();
                              break;
                    case "M": return;
                }
            }
        }

        private void ProcessManageUsersSubmenu()
        {
            var manageUsers = new ManageUsers(identityManager);

            while (true)
            {
                WriteManageUsersSubmenu();

                var command = ReadMenuOption();
                switch (command)
                {
                    case "1": manageUsers.GetUsers();
                              break;
                    case "2": manageUsers.GetUsersByName();
                              break;
                    case "R": return;
                }
            }
        }

        private void ProcessManageGroupsMenu()
        {
            var manageGroups = new ManageGroups(identityManager);

            while (true)
            {
                WriteManageGroupsMenu();

                var command = ReadMenuOption();
                switch (command)
                {
                    case "1": manageGroups.GetGroupByGroupName();
                              break;
                    case "2": manageGroups.GetGroupByObjectId();
                              break;
                    case "3": ProcessManageGroupsSubmenu();
                              break;
                    case "4": manageGroups.CreateGroup();
                              break;
                    case "5": manageGroups.DeleteGroup();
                              break;
                    case "6": manageGroups.AddMemberToGroup();
                              break;
                    case "7": manageGroups.RemoveMemberToGroup();
                              break;
                    case "8": manageGroups.AddOwnerToGroup();
                              break;
                    case "9": manageGroups.RemoveOwnerToGroup();
                              break;
                    case "M": return;
                }
            }
        }

        private void ProcessManageGroupsSubmenu()
        {
            var manageGroups = new ManageGroups(identityManager);

            while (true)
            {
                WriteManageGroupSubmenu();

                var command = ReadMenuOption();
                switch (command)
                {
                    case "1": manageGroups.GetGroups();
                              break;
                    case "2": manageGroups.GetGroupsByName();
                              break;
                    case "R": return;
                }
            }
        }

        private void WriteMainMenu()
        {
            System.Console.Clear();
            System.Console.WriteLine("IDENTITY MANAGEMENT: MAIN MENU ({0})", identityManager.Domain);
            System.Console.WriteLine("");
            System.Console.WriteLine("COMMAND DESCRIPTION");
            System.Console.WriteLine("================================================================================");
            System.Console.WriteLine("[ U ]   MANAGE USERS");
            System.Console.WriteLine("[ G ]   MANAGE GROUPS");
            System.Console.WriteLine("[ X ]   EXIT");
            System.Console.WriteLine("================================================================================");
        }

        private void WriteManageUsersMenu()
        {
            System.Console.Clear();
            System.Console.WriteLine("IDENTITY MANAGEMENT: MANAGE USERS MENU ({0})", identityManager.Domain);
            System.Console.WriteLine("");
            System.Console.WriteLine("COMMAND DESCRIPTION");
            System.Console.WriteLine("================================================================================");
            System.Console.WriteLine("[ 1 ]   GET USER BY USER NAME");
            System.Console.WriteLine("[ 2 ]   GET USER BY DISPLAY NAME");
            System.Console.WriteLine("[ 3 ]   GET USER BY OBJECT ID");
            System.Console.WriteLine("[ 4 ]   GET USERS");
            System.Console.WriteLine("[ 5 ]   SET USER PASSWORD");
            System.Console.WriteLine("[ 6 ]   CREATE USER");
            System.Console.WriteLine("[ 7 ]   DELETE USER");
            System.Console.WriteLine("[ M ]   MAIN MENU");
            System.Console.WriteLine("================================================================================");
        }

        private void WriteManageUsersSubmenu()
        {
            System.Console.Clear();
            System.Console.WriteLine("IDENTITY MANAGEMENT: MANAGE USERS SUBMENU -> GET USERS ({0})", identityManager.Domain);
            System.Console.WriteLine("");
            System.Console.WriteLine("COMMAND DESCRIPTION");
            System.Console.WriteLine("================================================================================");
            System.Console.WriteLine("[ 1 ]   GET USERS (ALL)");
            System.Console.WriteLine("[ 2 ]   GET USERS BY DISPLAY NAME (FULL OR STARTING WITH)");
            System.Console.WriteLine("[ R ]   RETURN TO MANAGE USERS MENU");
            System.Console.WriteLine("================================================================================");
        }

        private void WriteManageGroupsMenu()
        {
            System.Console.Clear();
            System.Console.WriteLine("IDENTITY MANAGEMENT: MANAGE GROUPS MENU ({0})", identityManager.Domain);
            System.Console.WriteLine("");
            System.Console.WriteLine("COMMAND DESCRIPTION");
            System.Console.WriteLine("================================================================================");
            System.Console.WriteLine("[ 1 ]   GET GROUPS BY GROUP NAME");
            System.Console.WriteLine("[ 2 ]   GET GROUPS BY OBJECT ID");
            System.Console.WriteLine("[ 3 ]   GET GROUPS");
            System.Console.WriteLine("[ 4 ]   CREATE GROUP");
            System.Console.WriteLine("[ 5 ]   DELETE GROUP");
            System.Console.WriteLine("[ 6 ]   ADD USER TO GROUP");
            System.Console.WriteLine("[ 7 ]   REMOVE USER FROM GROUP");
            System.Console.WriteLine("[ 8 ]   ADD OWNER TO GROUP");
            System.Console.WriteLine("[ 9 ]   REMOVE OWNER FROM GROUP");
            System.Console.WriteLine("[ M ]   MAIN MENU");
            System.Console.WriteLine("================================================================================");
        }

        private void WriteManageGroupSubmenu()
        {
            System.Console.Clear();
            System.Console.WriteLine("IDENTITY MANAGEMENT: MANAGE GROUPS SUBMENU -> GET GROUPS ({0})", identityManager.Domain);
            System.Console.WriteLine("");
            System.Console.WriteLine("COMMAND DESCRIPTION");
            System.Console.WriteLine("================================================================================");
            System.Console.WriteLine("[ 1 ]   GET GROUPS (ALL)");
            System.Console.WriteLine("[ 2 ]   GET GROUPS BY GROUP NAME (FULL OR STARTING WITH)");
            System.Console.WriteLine("[ R ]   RETURN TO MANAGE GROUPS MENU");
            System.Console.WriteLine("================================================================================");
        }

        private static void WriteUnexpectedException(Exception ex)
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine("unexcepted exception-> {0}", ex.Message);
            System.Console.WriteLine("inner exception-> {0}", ex.InnerException?.Message);
            System.Console.WriteLine("stack trace-> {0}{1}", Environment.NewLine, ex.StackTrace);
            System.Console.ResetColor();
        }

        private static string ReadMenuOption()

        {
            System.Console.WriteLine("");
            System.Console.Write("ENTER COMMAND AND PRESS ENTER: ");

            var command = System.Console.ReadLine();
            return !string.IsNullOrEmpty(command) ? command.ToUpper(CultureInfo.CurrentCulture) : "X";
        }

        private static void ReadContinue()
        {
            System.Console.WriteLine("");
            System.Console.Write("PRESS ENTER TO CONTINUE ");
            System.Console.ReadKey();
        }
    }
}