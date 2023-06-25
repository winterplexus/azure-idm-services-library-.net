//
//  Menus.cs
//
//  Wiregrass Code Technology 2020-2023
//
using System.Globalization;

namespace IdentityManagement.Services.Console.Application
{
    internal sealed class Menus
    {
        private readonly IIdentityManager identityManager;

        internal Menus(IIdentityManager identityManager)
        {
            this.identityManager = identityManager;
        }

        internal async Task MainMenu()
        {
            try
            {
                while (true)
                {
                    WriteMainMenu();

                    var command = ReadMenuOption();
                    switch (command)
                    {
                        case "U": await ProcessManageUsersMenu().ConfigureAwait(false);
                                  break;
                        case "G": await ProcessManageGroupsMenu().ConfigureAwait(false);
                                  break;
                        case "X": return;
                        default:  return;
                    }
                }
            }
            catch (IdentityManagerException ime)
            {
                WriteExceptionMessage(ime);
            }
            catch (ArgumentNullException ane)
            {
                WriteExceptionMessage(ane);
            }
            finally
            {
                ReadContinue();
            }
        }

        private async Task ProcessManageUsersMenu()
        {
            var manageUsers = new ManageUsers(identityManager);

            while (true)
            {
                WriteManageUsersMenu();

                var command = ReadMenuOption();
                switch (command)
                {
                    case "1": await manageUsers.GetUserBySignInName().ConfigureAwait(false);
                              break;
                    case "2": await manageUsers.GetUserByDisplayName().ConfigureAwait(false);
                              break;
                    case "3": await manageUsers.GetUserByObjectId().ConfigureAwait(false);
                              break;
                    case "4": await ProcessManageUsersSubmenu().ConfigureAwait(false);
                              break;
                    case "5": await manageUsers.CreateUser().ConfigureAwait(false);
                              break;
                    case "6": await manageUsers.DeleteUser().ConfigureAwait(false);
                              break;
                    case "7": await manageUsers.SetUserPassword().ConfigureAwait(false);
                              break;
                    case "M": return;
                    default:  return;
                }
            }
        }

        private async Task ProcessManageUsersSubmenu()
        {
            var manageUsers = new ManageUsers(identityManager);

            while (true)
            {
                WriteManageUsersSubmenu();

                var command = ReadMenuOption();
                switch (command)
                {
                    case "1": await manageUsers.GetUsers().ConfigureAwait(false);
                              break;
                    case "2": await manageUsers.GetUsersByName().ConfigureAwait(false);
                              break;
                    case "R": return;
                    default:  return;
                }
            }
        }

        private async Task ProcessManageGroupsMenu()
        {
            var manageGroups = new ManageGroups(identityManager);

            while (true)
            {
                WriteManageGroupsMenu();

                var command = ReadMenuOption();
                switch (command)
                {
                    case "1": await manageGroups.GetGroupByGroupName().ConfigureAwait(false);
                              break;
                    case "2": await manageGroups.GetGroupByObjectId().ConfigureAwait(false);
                              break;
                    case "3": await ProcessManageGroupsSubmenu().ConfigureAwait(false);
                              break;
                    case "4": await manageGroups.CreateGroup().ConfigureAwait(false);
                              break;
                    case "5": await manageGroups.DeleteGroup().ConfigureAwait(false);
                              break;
                    case "6": await manageGroups.AddOwnerToGroup().ConfigureAwait(false);
                              break;
                    case "7": await manageGroups.RemoveOwnerToGroup().ConfigureAwait(false);
                              break;
                    case "8": await manageGroups.AddMemberToGroup().ConfigureAwait(false);
                              break;
                    case "9": await manageGroups.RemoveMemberToGroup().ConfigureAwait(false);
                              break;
                    case "M": return;
                    default:  return;
                }
            }
        }

        private async Task ProcessManageGroupsSubmenu()
        {
            var manageGroups = new ManageGroups(identityManager);

            while (true)
            {
                WriteManageGroupSubmenu();

                var command = ReadMenuOption();
                switch (command)
                {
                    case "1": await manageGroups.GetGroups().ConfigureAwait(false);
                              break;
                    case "2": await manageGroups.GetGroupsByName().ConfigureAwait(false);
                              break;
                    case "R": return;
                    default:  return;
                }
            }
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
            System.Console.Write("PRESS ENTER TO CONTINUE");
            System.Console.ReadKey();
        }

        private void WriteMainMenu()
        {
#if _ENABLE_CLS
            System.Console.Clear();
#endif
            System.Console.WriteLine("");
            System.Console.WriteLine($"IDENTITY MANAGEMENT: MAIN MENU ({identityManager.Tenant})");
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
#if _ENABLE_CLS
            System.Console.Clear();
#endif
            System.Console.WriteLine("");
            System.Console.WriteLine($"IDENTITY MANAGEMENT: MANAGE USERS MENU ({identityManager.Tenant})");
            System.Console.WriteLine("");
            System.Console.WriteLine("COMMAND DESCRIPTION");
            System.Console.WriteLine("================================================================================");
            System.Console.WriteLine("[ 1 ]   GET USER BY USER NAME");
            System.Console.WriteLine("[ 2 ]   GET USER BY DISPLAY NAME");
            System.Console.WriteLine("[ 3 ]   GET USER BY OBJECT ID");
            System.Console.WriteLine("[ 4 ]   GET USERS");
            System.Console.WriteLine("[ 5 ]   CREATE USER");
            System.Console.WriteLine("[ 6 ]   DELETE USER");
            System.Console.WriteLine("[ 7 ]   SET USER PASSWORD");
            System.Console.WriteLine("[ M ]   MAIN MENU");
            System.Console.WriteLine("================================================================================");
        }

        private void WriteManageUsersSubmenu()
        {
#if _ENABLE_CLS
            System.Console.Clear();
#endif
            System.Console.WriteLine("");
            System.Console.WriteLine($"IDENTITY MANAGEMENT: MANAGE USERS SUBMENU -> GET USERS ({identityManager.Tenant})");
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
#if _ENABLE_CLS
            System.Console.Clear();
#endif
            System.Console.WriteLine("");
            System.Console.WriteLine($"IDENTITY MANAGEMENT: MANAGE GROUPS MENU ({identityManager.Tenant})");
            System.Console.WriteLine("");
            System.Console.WriteLine("COMMAND DESCRIPTION");
            System.Console.WriteLine("================================================================================");
            System.Console.WriteLine("[ 1 ]   GET GROUPS BY GROUP NAME");
            System.Console.WriteLine("[ 2 ]   GET GROUPS BY OBJECT ID");
            System.Console.WriteLine("[ 3 ]   GET GROUPS");
            System.Console.WriteLine("[ 4 ]   CREATE GROUP");
            System.Console.WriteLine("[ 5 ]   DELETE GROUP");
            System.Console.WriteLine("[ 6 ]   ADD OWNER TO GROUP");
            System.Console.WriteLine("[ 7 ]   REMOVE OWNER FROM GROUP");
            System.Console.WriteLine("[ 8 ]   ADD USER TO GROUP");
            System.Console.WriteLine("[ 9 ]   REMOVE USER FROM GROUP");
            System.Console.WriteLine("[ M ]   MAIN MENU");
            System.Console.WriteLine("================================================================================");
        }

        private void WriteManageGroupSubmenu()
        {
#if _ENABLE_CLS
            System.Console.Clear();
#endif
            System.Console.WriteLine("");
            System.Console.WriteLine($"IDENTITY MANAGEMENT: MANAGE GROUPS SUBMENU -> GET GROUPS ({identityManager.Tenant})");
            System.Console.WriteLine("");
            System.Console.WriteLine("COMMAND DESCRIPTION");
            System.Console.WriteLine("================================================================================");
            System.Console.WriteLine("[ 1 ]   GET GROUPS (ALL)");
            System.Console.WriteLine("[ 2 ]   GET GROUPS BY GROUP NAME (FULL OR STARTING WITH)");
            System.Console.WriteLine("[ R ]   RETURN TO MANAGE GROUPS MENU");
            System.Console.WriteLine("================================================================================");
        }

        private static void WriteExceptionMessage(Exception ex)
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine($"UNEXPECTED EXCEPTION-> {ex.Message}");
            System.Console.WriteLine($"INNER EXCEPTION-> {ex.InnerException?.Message}");
            System.Console.WriteLine($"STACK TRACE-> {Environment.NewLine}{ex.StackTrace}");
            System.Console.ResetColor();
        }
    }
}