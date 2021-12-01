//
//  UserManagement.cs
//
//  Wiregrass Code Technology 2020-2022
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;

namespace IdentityManagement.Services
{
    public class UserManagement : IUserManagement
    {
        private readonly GraphServiceClient client;
        private readonly IConfigurationSection configuration;

        public UserManagement(GraphServiceClient graphServiceClient, IConfigurationSection configurationSection)
        {
            client = graphServiceClient;
            configuration = configurationSection;
        }

        public async Task<UserModel> GetUserBySignInName(string signInName)
        {
            if (string.IsNullOrEmpty(signInName))
            {
                throw new ArgumentNullException(nameof(signInName));
            }

            var user = await client.Users
                .Request()
                .Filter($"identities/any(c:c/issuerAssignedId eq '{signInName}' and c/issuer eq '{configuration["Tenant"]}')")
                .Select(u => new
                {
                    u.Id,
                    u.Identities,
                    u.AccountEnabled,
                    u.CreatedDateTime,
                    u.CreationType,
                    u.DeletedDateTime,
                    u.DisplayName,
                    u.GivenName,
                    u.Surname,
                    u.CompanyName,
                    u.Department,
                    u.StreetAddress,
                    u.City,
                    u.State,
                    u.PostalCode,
                    u.Mail,
                    u.OtherMails
                })
                .GetAsync();

            return user.CurrentPage.Select(CreateUserModel).FirstOrDefault();
        }

        public async Task<UserModel> GetUserByDisplayName(string displayName)
        {
            if (string.IsNullOrEmpty(displayName))
            {
                throw new ArgumentNullException(nameof(displayName));
            }

            var user = await client.Users
                .Request()
                .Filter($"displayName eq '{displayName}'")
                .Select(u => new
                {
                    u.Id,
                    u.Identities,
                    u.AccountEnabled,
                    u.CreatedDateTime,
                    u.CreationType,
                    u.DeletedDateTime,
                    u.DisplayName,
                    u.GivenName,
                    u.Surname,
                    u.CompanyName,
                    u.Department,
                    u.StreetAddress,
                    u.City,
                    u.State,
                    u.PostalCode,
                    u.Mail,
                    u.OtherMails
                })
                .GetAsync();

            return user.CurrentPage.Select(CreateUserModel).FirstOrDefault();
        }

        public async Task<UserModel> GetUserByObjectId(string userObjectId)
        {
            if (string.IsNullOrEmpty(userObjectId))
            {
                throw new ArgumentNullException(nameof(userObjectId));
            }

            try
            {
                var user = await client.Users[userObjectId]
                    .Request()
                    .Select(u => new
                    {
                        u.Id,
                        u.Identities,
                        u.AccountEnabled,
                        u.CreatedDateTime,
                        u.CreationType,
                        u.DeletedDateTime,
                        u.DisplayName,
                        u.GivenName,
                        u.Surname,
                        u.CompanyName,
                        u.Department,
                        u.StreetAddress,
                        u.City,
                        u.State,
                        u.PostalCode,
                        u.Mail,
                        u.OtherMails
                    })
                    .GetAsync();

                return user != null ? CreateUserModel(user) : null;
            }
            catch (ServiceException se)
            {
                if (se.Error.Code.Equals("Request_ResourceNotFound", StringComparison.CurrentCulture))
                {
                    return null;
                }
                throw;
            }
        }

        public async Task<string> CreateUser(UserModel userModel)
        {
            if (userModel == null)
            {
                throw new ArgumentNullException(nameof(userModel));
            }

            SetIdentityIssuer(userModel);

            var user = await client.Users
                .Request()
                .AddAsync(userModel);

            return user.Id;
        }

        public async Task<bool> DeleteUser(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            var userObjectId = GetUserObjectId(userName);
            if (userObjectId == null)
            {
                return false;
            }

            await client.Users[userObjectId]
                .Request()
                .DeleteAsync();

            return true;
        }

        public async Task<bool> SetUserPasswordByObjectId(string userName, string replacementPassword)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            if (string.IsNullOrEmpty(replacementPassword))
            {
                throw new ArgumentNullException(nameof(replacementPassword));
            }

            var userObjectId = GetUserObjectId(userName);
            if (userObjectId == null)
            {
                return false;
            }

            var user = new User
            {
                PasswordPolicies = "DisablePasswordExpiration",
                PasswordProfile = new PasswordProfile
                {
                    ForceChangePasswordNextSignIn = false,
                    Password = replacementPassword
                }
            };

            await client.Users[userObjectId]
                .Request()
                .UpdateAsync(user);

            return true;
        }

        public async Task<IList<UserModel>> GetUsers(int limit)
        {
            if (limit < 1)
            {
                throw new ArgumentException("limit must be greater than 0");
            }

            var users = await client.Users
                .Request()
                .Top(limit)
                .Select(u => new
                {
                    u.Id,
                    u.Identities,
                    u.AccountEnabled,
                    u.CreatedDateTime,
                    u.CreationType,
                    u.DeletedDateTime,
                    u.DisplayName,
                    u.GivenName,
                    u.Surname,
                    u.CompanyName,
                    u.Department,
                    u.StreetAddress,
                    u.City,
                    u.State,
                    u.PostalCode,
                    u.Mail,
                    u.OtherMails
                })
                .GetAsync();

            var usersList = users.Select(user => (CreateUserModel(user))).ToList();
            if (usersList.Count >= limit)
            {
                return usersList;
            }

            do
            {
                if (users.NextPageRequest != null)
                {
                    users = await users.NextPageRequest.GetAsync();
                    usersList.AddRange(users.Select(user => (CreateUserModel(user))));
                }
                else
                {
                    users = null;
                }
            } while (users != null);

            return usersList;
        }

        public async Task<IList<UserModel>> GetUsersByName(string displayName)
        {
            if (string.IsNullOrEmpty(displayName))
            {
                throw new ArgumentNullException(nameof(displayName));
            }

            var users = await client.Users
                .Request()
                .Filter($"startswith(displayName, '{displayName}')")
                .Select(u => new
                {
                    u.Id,
                    u.Identities,
                    u.AccountEnabled,
                    u.CreatedDateTime,
                    u.CreationType,
                    u.DeletedDateTime,
                    u.DisplayName,
                    u.GivenName,
                    u.Surname,
                    u.CompanyName,
                    u.Department,
                    u.StreetAddress,
                    u.City,
                    u.State,
                    u.PostalCode,
                    u.Mail,
                    u.OtherMails
                })
                .GetAsync();

            var usersList = users.Select(user => (CreateUserModel(user))).ToList();

            do
            {
                if (users.NextPageRequest != null)
                {
                    users = await users.NextPageRequest.GetAsync();
                    usersList.AddRange(users.Select(user => (CreateUserModel(user))));
                }
                else
                {
                    users = null;
                }
            } while (users != null);

            return usersList;
        }

        public async Task<IList<GroupModel>> GetGroupMembershipBySignInName(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            var userObjectId = GetUserObjectId(userName);
            if (userObjectId == null)
            {
                return null;
            }

            var memberOf = await client.Users[userObjectId].MemberOf
                .Request()
                .GetAsync();

            var groupMembership = memberOf.Select(member => new GroupModel
            {
                Id = member.Id,
                DisplayName = GetGroupDisplayName(member.Id).Result.DisplayName
            }).ToList();

            do
            {
                if (memberOf.NextPageRequest != null)
                {
                    memberOf = await memberOf.NextPageRequest.GetAsync();
                    groupMembership.AddRange(memberOf.Select(member => new GroupModel
                    {
                        Id = member.Id,
                        DisplayName = GetGroupDisplayName(member.Id).Result.DisplayName
                    }));
                }
                else
                {
                    memberOf = null;
                }
            } while (memberOf != null);

            return groupMembership;
        }

        public string GetUserName(UserModel userModel)
        {
            if (userModel == null)
            {
                throw new ArgumentNullException(nameof(userModel));
            }

            var username = (from identity in userModel.Identities where !string.IsNullOrWhiteSpace(identity.SignInType)
                                where  identity.SignInType == "emailAddress" ||
                                       identity.SignInType == "userName"
                                select identity.IssuerAssignedId).FirstOrDefault();

            return username;
        }

        private string GetUserObjectId(string userName)
        {
            var user = Task.Run(async () => await GetUserBySignInName(userName)).Result;

            return user?.Id;
        }

        private static UserModel CreateUserModel(User user)
        {
            var userModel = new UserModel
            {
                Id = user.Id,
                AccountEnabled = user.AccountEnabled,
                CreatedDateTime = user.CreatedDateTime,
                CreationType = user.CreationType,
                DeletedDateTime = user.DeletedDateTime,
                DisplayName = user.DisplayName,
                GivenName = user.GivenName,
                Surname = user.Surname,
                CompanyName = user.CompanyName,
                Department = user.Department,
                StreetAddress = user.StreetAddress,
                City = user.City,
                State = user.State,
                PostalCode = user.PostalCode,
                Mail = user.Mail,
                OtherMails = user.OtherMails
            };

            if (user.Identities != null)
            {
                IList<ObjectIdentity> identities = user.Identities.Select(identity => new ObjectIdentity
                {
                    SignInType = identity.SignInType,
                    Issuer = identity.Issuer,
                    IssuerAssignedId = identity.IssuerAssignedId
                }).ToList();

                userModel.AssignedId = (from identity in user.Identities where !string.IsNullOrWhiteSpace(identity.SignInType)
                                            where  identity.SignInType == "emailAddress" ||
                                                   identity.SignInType == "userName"
                                            select identity.IssuerAssignedId).FirstOrDefault();

                userModel.Identities = identities;
            }

            return userModel;
        }

        private void SetIdentityIssuer(UserModel userModel)
        {
            if (userModel.Identities != null)
            {
                foreach (var identity in userModel.Identities)
                {
                    if (identity.SignInType == "userName")
                    {
                        identity.Issuer = configuration["Tenant"];
                    }
                }
            }
        }

        private async Task<Group> GetGroupDisplayName(string groupObjectId)
        {
            var group = await client.Groups[groupObjectId]
                .Request()
                .Select(g => new
                {
                    g.DisplayName
                })
                .GetAsync();

            return group;
        }
    }
}