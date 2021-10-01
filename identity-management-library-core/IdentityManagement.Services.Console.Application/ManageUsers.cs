//
//  ManageUsers.cs
//
//  Wiregrass Code Technology 2020-2021
//
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Graph;

namespace IdentityManagement.Services.Console.Application
{
    internal class ManageUsers
    {
        private readonly IIdentityManager identityManager;

        internal ManageUsers(IIdentityManager identityManager)
        {
            this.identityManager = identityManager;
        }

        internal void GetUserBySignInName()
        {
            try
            {
                var userName = ReadUserName();

                var user = Task.Run(async () => await identityManager.UserServices.GetUserBySignInName(userName).ConfigureAwait(false)).Result;
                if (user != null)
                {
                    WriteUser(user);

                    var groups = Task.Run(async () => await identityManager.UserServices.GetGroupMembershipBySignInName(userName).ConfigureAwait(false)).Result;

                    WriteUserGroupMembership(groups);
                }
                else
                {
                    WriteUserNotFoundMessage(userName);
                }

                ReadContinue();
            }
            catch (Exception ex)
            {
                WriteException(ex);

                ReadContinue(false);
            }
        }

        internal void GetUserByDisplayName()
        {
            try
            {
                var displayName = ReadDisplayName();

                var user = Task.Run(async () => await identityManager.UserServices.GetUserByDisplayName(displayName).ConfigureAwait(false)).Result;
                if (user != null)
                {
                    WriteUser(user);

                    var groups = Task.Run(async () => await identityManager.UserServices.GetGroupMembershipBySignInName(displayName).ConfigureAwait(false)).Result;

                    WriteUserGroupMembership(groups);
                }
                else
                {
                    WriteUserNotFoundMessage(displayName);
                }

                ReadContinue();
            }
            catch (Exception ex)
            {
                WriteException(ex);

                ReadContinue(false);
            }

        }

        internal void GetUserByObjectId()
        {
            try
            {
                var userObjectId = ReadUserObjectId();

                var user = Task.Run(async () => await identityManager.UserServices.GetUserByObjectId(userObjectId).ConfigureAwait(false)).Result;
                if (user != null)
                {
                    WriteUser(user);

                    var userName = identityManager.UserServices.GetUserName(user);
                    if (userName != null)
                    {
                        var groups = Task.Run(async () => await identityManager.UserServices.GetGroupMembershipBySignInName(userName).ConfigureAwait(false)).Result;

                        WriteUserGroupMembership(groups);
                    }
                }
                else
                {
                    WriteUserNotFoundMessage(userObjectId);
                }

                ReadContinue();
            }
            catch (Exception ex)
            {
                WriteException(ex);

                ReadContinue(false);
            }
        }

        internal void GetUsers()
        {
            var limit = ReadLimit();

            try
            {
                var usersList = Task.Run(async () => await identityManager.UserServices.GetUsers(limit).ConfigureAwait(false)).Result;
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
                        var groups = Task.Run(async () => await identityManager.UserServices.GetGroupMembershipBySignInName(userName).ConfigureAwait(false)).Result;

                        WriteUserGroupMembership(groups);
                    }

                    counter++;
                }

                WriteRecordCount(counter);

                ReadContinue();
            }
            catch (Exception ex)
            {
                WriteException(ex);

                ReadContinue(false);
            }
        }

        internal void GetUsersByName()
        {
            var displayName = ReadDisplayName();

            try
            {
                var usersList = Task.Run(async () => await identityManager.UserServices.GetUsersByName(displayName).ConfigureAwait(false)).Result;
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
                        var groups = Task.Run(async () => await identityManager.UserServices.GetGroupMembershipBySignInName(userName).ConfigureAwait(false)).Result;

                        WriteUserGroupMembership(groups);
                    }

                    counter++;
                }

                WriteRecordCount(counter);

                ReadContinue();
            }
            catch (Exception ex)
            {
                WriteException(ex);

                ReadContinue(false);
            }
        }

        internal void CreateUser()
        {
            try
            {
                var userModel = ReadUser();

                var userObjectId = Task.Run(async () => await identityManager.UserServices.CreateUser(userModel).ConfigureAwait(false)).Result;

                WriteUserObjectIdentifier(userObjectId);

                ReadContinue();
            }
            catch (Exception ex)
            {
                var baseException = ex.GetBaseException();
                if (baseException.GetType() == typeof(ServiceException))
                {
                    var se = (ServiceException)baseException;

                    if (se.StatusCode != HttpStatusCode.OK)
                    {
                        if (se.StatusCode == HttpStatusCode.BadRequest)
                        {
                            if (se.Error.Message.Contains("userPrincipalName already exists", StringComparison.CurrentCultureIgnoreCase))
                            {
                                System.Console.WriteLine($"{Environment.NewLine}error-> username already exists (choose another username)");
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
                else
                {
                    WriteException(ex);
                }

                ReadContinue(false);
            }
        }

        internal void DeleteUser()
        {
            try
            {
                var userName = ReadUserName();

                var status = Task.Run(async () => await identityManager.UserServices.DeleteUser(userName).ConfigureAwait(false)).Result;

                ReadContinue(status);
            }
            catch (Exception ex)
            {
                WriteException(ex);

                ReadContinue(false);
            }
        }

        internal void SetUserPassword()
        {
            try
            {
                var userName = ReadUserName();

                var replacementPassword = ReadReplacementPassword();

                var status = Task.Run(async () => await identityManager.UserServices.SetUserPasswordByObjectId(userName, replacementPassword).ConfigureAwait(false)).Result;

                ReadContinue(status);
            }
            catch (Exception ex)
            {
                WriteException(ex);

                ReadContinue(false);
            }
        }

        private static void WriteUserNotFoundMessage(string identifier)
        {
            System.Console.WriteLine($"{Environment.NewLine}information-> unable to locate user using: {identifier}");
        }

        private static void WriteUser(UserModel userModel)
        {
            if (userModel == null)
            {
                System.Console.WriteLine($"{Environment.NewLine}information-> unable to locate user");
                return;
            }

            System.Console.WriteLine("");
            System.Console.WriteLine("[ USER ]");
            System.Console.WriteLine($"- object ID            = {userModel.Id}");
            System.Console.WriteLine($"- assigned ID          = {userModel.AssignedId}");
            System.Console.WriteLine($"- account enabled      = {userModel.AccountEnabled}");
            System.Console.WriteLine($"- created date time    = {userModel.CreatedDateTime}");
            System.Console.WriteLine($"- creation type        = {userModel.CreationType}");
            System.Console.WriteLine($"- deleted date time    = {userModel.DeletedDateTime}");
            System.Console.WriteLine($"- display name         = {userModel.DisplayName}");
            System.Console.WriteLine($"- given name           = {userModel.GivenName}");
            System.Console.WriteLine($"- surname              = {userModel.Surname}");
            System.Console.WriteLine($"- company name         = {userModel.CompanyName}");
            System.Console.WriteLine($"- department           = {userModel.Department}");
            System.Console.WriteLine($"- street address       = {userModel.StreetAddress}");
            System.Console.WriteLine($"- city                 = {userModel.City}");
            System.Console.WriteLine($"- state                = {userModel.State}");
            System.Console.WriteLine($"- postal code          = {userModel.PostalCode}");
            System.Console.WriteLine($"- mail                 = {userModel.Mail}");

            if (userModel.OtherMails != null && userModel.OtherMails.Any())
            {
                System.Console.WriteLine($"- alternate mail       = {userModel.OtherMails.ElementAt(0)}");
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

            System.Console.WriteLine($"                       = identites count         > {identitiesList.Count}");
            foreach (var identity in identitiesList)
            {
                System.Console.WriteLine($"- identity             = signin type             > {identity.SignInType}");
                System.Console.WriteLine($"-                      = issuer                  > {identity.Issuer}");
                System.Console.WriteLine($"-                      = issuer assigned ID      > {identity.IssuerAssignedId}");
            }
        }

        private static void WriteUserGroupMembership(IList<GroupModel> groups)
        {
            if (groups == null)
            {
                return;
            }

            var groupList = groups.ToList();

            System.Console.WriteLine($"                       = group memberships count > {groupList.Count}");
            foreach (var group in groups)
            {
                System.Console.WriteLine($"- group membership     = object ID               > {group.Id}");
                System.Console.WriteLine($"-                      = display name            > {group.DisplayName}");
            }
        }

        private static void WriteUserObjectIdentifier(string userObjectId)
        {
            System.Console.WriteLine("");
            System.Console.WriteLine($"- user object ID       = {userObjectId}");
        }

        private static void WriteRecordCount(int counter)
        {
            System.Console.WriteLine("");
            System.Console.WriteLine($"total records: {counter}");
        }

        private static void WriteException(Exception ex)
        {
            System.Console.WriteLine($"exception-> {ex.Message}");
            System.Console.WriteLine($"inner exception-> {ex.InnerException?.Message}");
            System.Console.WriteLine($"stack trace-> {Environment.NewLine}{ex.StackTrace}");
        }

        private static void WriteServiceException(ServiceException se)
        {
            System.Console.WriteLine($"status code-> {se.StatusCode}");
            System.Console.WriteLine($"error message-> {se.Error.Message}");
            System.Console.WriteLine($"error inner exception-> {se.Error.InnerError}");
            System.Console.WriteLine($"error stack trace-> {Environment.NewLine}{se.StackTrace}");
        }

        private static void ReadContinue()
        {
            System.Console.WriteLine("");
            System.Console.Write("PRESS ENTER TO CONTINUE ->");
            System.Console.ReadKey();
        }

        private static void ReadContinue(bool? status)
        {
            const string message = "PRESS ENTER TO CONTINUE -> ";

            System.Console.WriteLine("");
            System.Console.Write(status != null ? $"{message}({status}) " : "{message}");
            System.Console.ReadKey();
        }

        private static int ReadLimit()
        {
            System.Console.WriteLine("");
            System.Console.Write("ENTER TOP USERS LIMIT: ");

            var line = System.Console.ReadLine();

            return !int.TryParse(line, out var number) ? 0 : number;
        }

        private static string ReadUserName()
        {
            System.Console.WriteLine("");
            System.Console.Write("- user name            : ");

            return System.Console.ReadLine();
        }

        private static string ReadDisplayName()
        {
            System.Console.WriteLine("");
            System.Console.Write("- display name         : ");

            return System.Console.ReadLine();
        }

        private static string ReadUserObjectId()
        {
            System.Console.WriteLine("");
            System.Console.Write("- user object ID       : ");

            return System.Console.ReadLine();
        }

        private static UserModel ReadUser()
        {
            var username      = GetInputField("username", 20);
            var password      = GetInputField("password", 20);

            var userModel = new UserModel
            {
                DisplayName   = GetInputField("display name", 20),
                GivenName     = GetInputField("given name", 20),
                Surname       = GetInputField("surname", 20),
                CompanyName   = GetInputField("company name", 20),
                Department    = GetInputField("department", 20),
                StreetAddress = GetInputField("street address", 20),
                City          = GetInputField("city", 20),
                State         = GetInputField("state", 20),
                PostalCode    = GetInputField("postal code", 20),
                Mail          = GetInputField("mail", 20),
                OtherMails    = GetOtherMails()
            };

            userModel.SetIdentity(username);
            userModel.SetPasswordProfile(password);

            return userModel;
        }

        private static string ReadReplacementPassword()
        {
            System.Console.Write("- replacement password : ");

            return System.Console.ReadLine();
        }

        private static IEnumerable<string> GetOtherMails()
        {
            var alternateMail = GetInputField("alternate mail", 20);

            IEnumerable<string> otherMails = new List<string>() { alternateMail };

            return otherMails;
        }

        private static string GetInputField(string fieldLabel, int fieldLabelWidth)
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

            return System.Console.ReadLine();
        }
    }
}