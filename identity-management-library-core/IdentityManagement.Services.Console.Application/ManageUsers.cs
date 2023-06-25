//
//  ManageUsers.cs
//
//  Wiregrass Code Technology 2020-2023
//
using System.Globalization;
using System.Net;
using Microsoft.Graph;

namespace IdentityManagement.Services.Console.Application
{
    internal sealed class ManageUsers
    {
        private readonly IIdentityManager identityManager;

        internal ManageUsers(IIdentityManager identityManager)
        {
            this.identityManager = identityManager;
        }

        internal async Task GetUserBySignInName()
        {
            if (identityManager == null)
            {
                throw new IdentityManagerException("identityManager object is null");
            }
            if (identityManager.UserServices == null)
            {
                throw new IdentityManagerException("identityManager.UserServices object is null");
            }

            try
            {
                var userName = ReadUserName();
                if (userName == null)
                {
                    WriteInvalidInput();
                    return;
                }

                var user = await identityManager.UserServices.GetUserBySignInName(userName).ConfigureAwait(false);
                if (user != null)
                {
                    WriteUser(user);

                    var groups = await identityManager.UserServices.GetGroupMembershipBySignInName(userName).ConfigureAwait(false);
                    if (groups == null)
                    {
                        System.Console.WriteLine($"{Environment.NewLine}INFORMATION-> group membership not found for user name: {userName}");
                        return;
                    }
                    else
                    {
                        WriteUserGroupMembership(groups);
                    }
                }
                else
                {
                    WriteUserNotFoundMessage(userName);
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

        internal async Task GetUserByDisplayName()
        {
            if (identityManager == null)
            {
                throw new IdentityManagerException("identityManager object is null");
            }
            if (identityManager.UserServices == null)
            {
                throw new IdentityManagerException("identityManager.UserServices object is null");
            }

            try
            {
                var displayName = ReadDisplayName();
                if (displayName == null)
                {
                    WriteInvalidInput();
                    return;
                }

                var user = await identityManager.UserServices.GetUserByDisplayName(displayName).ConfigureAwait(false);
                if (user != null)
                {
                    WriteUser(user);

                    var groups = await identityManager.UserServices.GetGroupMembershipBySignInName(displayName).ConfigureAwait(false);
                    if (groups == null)
                    {
                        System.Console.WriteLine($"{Environment.NewLine}INFORMATION-> group membership not found for display name: {displayName}");
                        return;
                    }
                    else
                    {
                        WriteUserGroupMembership(groups);
                    }
                }
                else
                {
                    WriteUserNotFoundMessage(displayName);
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

        internal async Task GetUserByObjectId()
        {
            if (identityManager == null)
            {
                throw new IdentityManagerException("identityManager object is null");
            }
            if (identityManager.UserServices == null)
            {
                throw new IdentityManagerException("identityManager.UserServices object is null");
            }

            try
            {
                var userObjectId = ReadUserObjectId();
                if (userObjectId == null)
                {
                    WriteInvalidInput();
                    return;
                }

                var user = await identityManager.UserServices.GetUserByObjectId(userObjectId).ConfigureAwait(false);
                if (user != null)
                {
                    WriteUser(user);

                    var userName = identityManager.UserServices.GetUserName(user);
                    if (userName != null)
                    {
                        var groups = await identityManager.UserServices.GetGroupMembershipBySignInName(userName).ConfigureAwait(false);
                        if (groups == null)
                        {
                            System.Console.WriteLine($"{Environment.NewLine}INFORMATION-> group membership not found for user name: {userName}");
                            return;
                        }
                        else
                        {
                            WriteUserGroupMembership(groups);
                        }
                    }
                }
                else
                {
                    WriteUserNotFoundMessage(userObjectId);
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

        internal async Task GetUsers()
        {
            if (identityManager == null)
            {
                throw new IdentityManagerException("identityManager object is null");
            }
            if (identityManager.UserServices == null)
            {
                throw new IdentityManagerException("identityManager.UserServices object is null");
            }

            var limit = ReadLimit();

            try
            {
                var usersList = await identityManager.UserServices.GetUsers(limit).ConfigureAwait(false);
                if (usersList == null)
                {
                    return;
                }

                var counter = 0;
                foreach (var user in usersList)
                {
                    WriteUser(user);

                    var userName = identityManager.UserServices.GetUserName(user);
                    if (userName != null)
                    {
                        var groups = await identityManager.UserServices.GetGroupMembershipBySignInName(userName).ConfigureAwait(false);
                        if (groups == null)
                        {
                            System.Console.WriteLine($"{Environment.NewLine}INFORMATION-> group membership not found for user name: {userName}");
                            return;
                        }
                        else
                        {
                            WriteUserGroupMembership(groups);
                        }
                    }
                    else
                    {
                        WriteUserNotFoundMessage(user.DisplayName);
                    }

                    counter++;
                }

                WriteRecordCount(counter);
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

        internal async Task GetUsersByName()
        {
            if (identityManager == null)
            {
                throw new IdentityManagerException("identityManager object is null");
            }
            if (identityManager.UserServices == null)
            {
                throw new IdentityManagerException("identityManager.UserServices object is null");
            }

            var displayName = ReadDisplayName();
            if (displayName == null)
            {
                WriteInvalidInput();
                return;
            }

            try
            {
                var usersList = await identityManager.UserServices.GetUsersByDisplayName(displayName).ConfigureAwait(false);
                if (usersList == null)
                {
                    return;
                }

                var counter = 0;
                foreach (var user in usersList)
                {
                    WriteUser(user);

                    var userName = identityManager.UserServices.GetUserName(user);
                    if (userName != null)
                    {
                        var groups = await identityManager.UserServices.GetGroupMembershipBySignInName(userName).ConfigureAwait(false);
                        if (groups == null)
                        {
                            System.Console.WriteLine($"{Environment.NewLine}INFORMATION-> group membership not found for user name: {userName}");
                            return;
                        }
                        else
                        {
                            WriteUserGroupMembership(groups);
                        }
                    }
                    else
                    {
                        WriteUserNotFoundMessage(user.DisplayName);
                    }

                    counter++;
                }

                WriteRecordCount(counter);
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

        internal async Task CreateUser()
        {
            if (identityManager == null)
            {
                throw new IdentityManagerException("identityManager object is null");
            }
            if (identityManager.UserServices == null)
            {
                throw new IdentityManagerException("identityManager.UserServices object is null");
            }

            try
            {
                var userModel = ReadUser();
                if (userModel == null)
                {
                    WriteInvalidInput();
                    return;
                }

                var userObjectId = await identityManager.UserServices.CreateUser(userModel).ConfigureAwait(false);
                if (userObjectId == null)
                {
                    System.Console.WriteLine($"{Environment.NewLine}INFORMATION-> unable to create user: {userModel.DisplayName}");
                    return;
                }
                else
                {
                    WriteUserObjectIdentifier(userObjectId);
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
            catch (ServiceException se)
            {
                if (se.StatusCode != HttpStatusCode.OK)
                {
                    if (se.StatusCode == HttpStatusCode.BadRequest)
                    {
                        if (se.Error.Message.Contains("userPrincipalName already exists", StringComparison.CurrentCultureIgnoreCase))
                        {
                            System.Console.WriteLine($"{Environment.NewLine}ERROR-> username already exists (choose another username)");
                        }
                        else
                        {
                            WriteServiceException(se);
                        }
                    }
                    else
                    {
                        WriteServiceException(se);
                    }
                }
                else
                {
                    WriteServiceException(se);
                }
            }
            finally
            {
                ReadContinue();
            }
        }

        internal async Task DeleteUser()
        {
            if (identityManager == null)
            {
                throw new IdentityManagerException("identityManager object is null");
            }
            if (identityManager.UserServices == null)
            {
                throw new IdentityManagerException("identityManager.UserServices object is null");
            }

            try
            {
                var userName = ReadUserName();
                if (userName == null)
                {
                    WriteInvalidInput();
                    return;
                }

                var status = await identityManager.UserServices.DeleteUser(userName).ConfigureAwait(false);
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

        internal async Task SetUserPassword()
        {
            if (identityManager == null)
            {
                throw new IdentityManagerException("identityManager object is null");
            }
            if (identityManager.UserServices == null)
            {
                throw new IdentityManagerException("identityManager.UserServices object is null");
            }

            try
            {
                var userName = ReadUserName();
                if (userName == null)
                {
                    WriteInvalidInput();
                    return;
                }

                var replacementPassword = ReadReplacementPassword();
                if (replacementPassword == null)
                {
                    WriteInvalidInput();
                    return;
                }

                var status = await identityManager.UserServices.SetUserPasswordByObjectId(userName, replacementPassword).ConfigureAwait(false);
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

        private static void ReadContinue()
        {
            System.Console.WriteLine("");
            System.Console.Write("PRESS ENTER TO CONTINUE");
            System.Console.ReadKey();
        }

        private static int ReadLimit()
        {
            System.Console.WriteLine("");
            System.Console.Write("ENTER TOP USERS LIMIT: ");

            var line = System.Console.ReadLine();
            return !int.TryParse(line, out var number) ? 0 : number;
        }

        private static string? ReadUserName()
        {
            System.Console.WriteLine("");

            return GetInputField("USER NAME", 20);
        }

        private static string? ReadDisplayName()
        {
            System.Console.WriteLine("");

            return GetInputField("DISPLAY NAME", 20);
        }

        private static string? ReadUserObjectId()
        {
            System.Console.WriteLine("");

            return GetInputField("USER OBJECT ID", 20);
        }

        private static UserModel? ReadUser()
        {
            var username = GetInputField("USERNAME", 20);
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }
            var password = GetInputField("PASSWORD", 20);
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password));
            }

            var userModel = new UserModel
            {
                DisplayName   = GetInputField("DISPLAY NAME", 20),
                GivenName     = GetInputField("GIVEN NAME", 20),
                Surname       = GetInputField("SURNAME", 20),
                CompanyName   = GetInputField("COMPANY NAME", 20),
                Department    = GetInputField("DEPARTMENT", 20),
                StreetAddress = GetInputField("STREET ADDRESS", 20),
                City          = GetInputField("CITY", 20),
                State         = GetInputField("STATE", 20),
                PostalCode    = GetInputField("POSTAL CODE", 20),
                Mail          = GetInputField("MAIL", 20)
            };

            var otherMails = GetOtherMails();
            if (otherMails != null)
            {
                userModel.OtherMails = otherMails;
            }

            userModel.SetIdentity(username);
            userModel.SetPasswordProfile(password);

            return userModel;
        }

        private static string? ReadReplacementPassword()
        {
            return GetInputField("REPLACEMENT PASSWORD", 20);
        }

        private static void WriteUserNotFoundMessage(string identifier)
        {
            System.Console.WriteLine($"{Environment.NewLine}INFORMATION-> unable to locate user using: {identifier}");
        }

        private static void WriteUser(UserModel userModel)
        {
            if (userModel == null)
            {
                System.Console.WriteLine($"{Environment.NewLine}INFORMATION-> unable to locate user");
                return;
            }

            System.Console.WriteLine("");
            System.Console.WriteLine("USER");
            System.Console.WriteLine("--------------------------------------------------------------------------------");
            System.Console.WriteLine($"OBJECT ID            = {userModel.Id}");
            System.Console.WriteLine($"ASSIGNED ID          = {userModel.AssignedId}");
            System.Console.WriteLine($"ACCOUNT ENABLED      = {userModel.AccountEnabled}");
            System.Console.WriteLine($"CREATED DATE TIME    = {userModel.CreatedDateTime}");
            System.Console.WriteLine($"CREATION TYPE        = {userModel.CreationType}");
            System.Console.WriteLine($"DELETED DATE TIME    = {userModel.DeletedDateTime}");
            System.Console.WriteLine($"DISPLAY NAME         = {userModel.DisplayName}");
            System.Console.WriteLine($"GIVEN NAME           = {userModel.GivenName}");
            System.Console.WriteLine($"SURNAME              = {userModel.Surname}");
            System.Console.WriteLine($"COMPANY NAME         = {userModel.CompanyName}");
            System.Console.WriteLine($"DEPARTMENT           = {userModel.Department}");
            System.Console.WriteLine($"STREET ADDRESS       = {userModel.StreetAddress}");
            System.Console.WriteLine($"CITY                 = {userModel.City}");
            System.Console.WriteLine($"STATE                = {userModel.State}");
            System.Console.WriteLine($"POSTAL CODE          = {userModel.PostalCode}");
            System.Console.WriteLine($"MAIL                 = {userModel.Mail}");

            if (userModel.OtherMails != null && userModel.OtherMails.Any())
            {
                System.Console.WriteLine($"ALTERNATE MAIL       = {userModel.OtherMails.ElementAt(0)}");
            }

            WriteUserIdentities(userModel.Identities);
        }

        private static void WriteUserIdentities(IEnumerable<ObjectIdentity> identities)
        {
            if (identities == null)
            {
                return;
            }

            var identitiesList = identities.ToList();

            System.Console.WriteLine($"IDENTITIES           = {identitiesList.Count} (COUNT)");
            foreach (var identity in identitiesList)
            {
                System.Console.WriteLine($"IDENTITY ............. SIGNIN TYPE        = {identity.SignInType}");
                System.Console.WriteLine($"                       ISSUER             = {identity.Issuer}");
                System.Console.WriteLine($"                       ISSUER ASSIGNED ID = {identity.IssuerAssignedId}");
            }
        }

        private static void WriteUserGroupMembership(IList<GroupModel> groups)
        {
            if (groups == null)
            {
                return;
            }

            var groupList = groups.ToList();

            System.Console.WriteLine($"GROUP MEMBERSHIPS    = {groupList.Count} (COUNT)");
            foreach (var group in groups)
            {
                System.Console.WriteLine($"GROUP ................ OBJECT             = {group.Id}");
                System.Console.WriteLine($"                       DISPLAY NAME       = {group.DisplayName}");
            }
        }

        private static void WriteUserObjectIdentifier(string userObjectId)
        {
            System.Console.WriteLine("");
            System.Console.WriteLine($"USER OBJECT ID       = {userObjectId}");
        }

        private static void WriteRecordCount(int counter)
        {
            System.Console.WriteLine("");
            System.Console.WriteLine($"TOTAL RECORDS: {counter}");
        }

        private static void WriteInvalidInput()
        {
            System.Console.WriteLine($"{Environment.NewLine}ERROR-> invalid input");
        }

        private static void WriteExceptionMessage(Exception ex)
        {
            System.Console.WriteLine($"EXCEPTION-> {ex.Message}");
            System.Console.WriteLine($"INNER {ex.InnerException?.Message}");
            System.Console.WriteLine($"STACK TRACE-> {Environment.NewLine}{ex.StackTrace}");
        }

        private static void WriteServiceException(ServiceException se)
        {
            System.Console.WriteLine($"SERVICE STATUS CODE-> {se.StatusCode}");
            System.Console.WriteLine($"SERVICE EXCEPTION-> {se.Error.Message}");
            System.Console.WriteLine($"SERVICE INNER EXCEPTION-> {se.Error.InnerError}");
            System.Console.WriteLine($"SERVICE STACK TRACE-> {Environment.NewLine}{se.StackTrace}");
        }

        private static string? GetInputField(string fieldLabel, int fieldLabelWidth)
        {
            if (string.IsNullOrEmpty(fieldLabel))
            {
                throw new ArgumentNullException(nameof(fieldLabel));
            }

            var width = fieldLabelWidth.ToString(CultureInfo.CurrentCulture);
            var format = "{0,-" + width + "}";

            System.Console.Write("- ");
            System.Console.Write(format, fieldLabel);
            System.Console.Write(" : ");

            var field = System.Console.ReadLine();
            if (field != null && field.Length < 1)
            {
                field = null;
            }
            return field;
        }

        private static IEnumerable<string>? GetOtherMails()
        {
            var alternateMail = GetInputField("ALTERNATE MAIL", 20);
            if (string.IsNullOrEmpty(alternateMail))
            {
                return null;
            }

            IEnumerable<string> otherMails = new List<string>() { alternateMail };
            return otherMails;
        }
    }
}