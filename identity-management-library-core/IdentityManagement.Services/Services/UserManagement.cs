//
//  UserManagement.cs
//
//  Wiregrass Code Technology 2020-2023
//
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

        public async Task<string?> CreateUser(UserModel userModel)
        {
            if (userModel == null)
            {
                throw new ArgumentNullException(nameof(userModel));
            }

            SetIdentityIssuer(userModel);

            var user = await client.Users.Request().AddAsync(userModel).ConfigureAwait(false);

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

            await client.Users[userObjectId].Request().DeleteAsync().ConfigureAwait(false);

            return true;
        }

        public async Task<UserModel?> GetUserBySignInName(string signInName)
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
                .GetAsync()
                .ConfigureAwait(false);

            var result = user.CurrentPage.Select(CreateUserModel).FirstOrDefault();

            return result;
        }

        public async Task<UserModel?> GetUserByDisplayName(string displayName)
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
                .GetAsync()
                .ConfigureAwait(false);

            var result = user.CurrentPage.Select(CreateUserModel).FirstOrDefault();

            return result;
        }

        public async Task<UserModel?> GetUserByObjectId(string userObjectId)
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
                    .GetAsync()
                    .ConfigureAwait(false);

                var result = user != null ? CreateUserModel(user) : null;

                return result;
            }
            catch (ServiceException se)
            {
                if (se.Error.Code.Equals("Request_ResourceNotFound", StringComparison.Ordinal))
                {
                    return null;
                }
                throw;
            }
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
                .GetAsync()
                .ConfigureAwait(false);

            var usersList = users.Select(user => (CreateUserModel(user))).ToList();
            if (usersList.Count >= limit)
            {
                return usersList;
            }

            do
            {
                if (users.NextPageRequest != null)
                {
                    users = await users.NextPageRequest.GetAsync().ConfigureAwait(false);
                    usersList.AddRange(users.Select(user => (CreateUserModel(user))));
                }
                else
                {
                    users = null;
                }
            } while (users != null);

            return usersList;
        }

        public async Task<IList<UserModel>> GetUsersByDisplayName(string displayName)
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
                .GetAsync()
                .ConfigureAwait(false);

            var usersList = users.Select(user => (CreateUserModel(user))).ToList();

            do
            {
                if (users.NextPageRequest != null)
                {
                    users = await users.NextPageRequest.GetAsync().ConfigureAwait(false);
                    usersList.AddRange(users.Select(user => (CreateUserModel(user))));
                }
                else
                {
                    users = null;
                }
            } while (users != null);

            return usersList;
        }

        public async Task<IList<GroupModel>?> GetGroupMembershipBySignInName(string signInName)
        {
            if (string.IsNullOrEmpty(signInName))
            {
                throw new ArgumentNullException(nameof(signInName));
            }

            var userObjectId = GetUserObjectId(signInName);
            if (userObjectId == null)
            {
                return null;
            }

            var memberOf = await client.Users[userObjectId].MemberOf.Request().GetAsync().ConfigureAwait(false);
            var groupMembership = memberOf.Select(member => new GroupModel
            {
                Id = member.Id,
                DisplayName = GetGroupDisplayName(member.Id).Result.DisplayName
            })
            .ToList();

            do
            {
                if (memberOf.NextPageRequest != null)
                {
                    memberOf = await memberOf.NextPageRequest.GetAsync().ConfigureAwait(false);
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

        public string? GetUserName(UserModel userModel)
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
                    Password = replacementPassword,
                    ForceChangePasswordNextSignIn = false
                }
            };

            await client.Users[userObjectId].Request().UpdateAsync(user).ConfigureAwait(false);

            return true;
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
                })
                .ToList();

                userModel.AssignedId = (from identity in user.Identities where !string.IsNullOrWhiteSpace(identity.SignInType)
                                            where  identity.SignInType == "emailAddress" || 
                                                   identity.SignInType == "userName"
                                            select identity.IssuerAssignedId).FirstOrDefault();

                userModel.Identities = identities;
            }

            return userModel;
        }

        private string? GetUserObjectId(string userName)
        {
            var user = Task.Run(async () => await GetUserBySignInName(userName).ConfigureAwait(false)).Result;
            return user?.Id;
        }

        private async Task<Group> GetGroupDisplayName(string groupObjectId)
        {
            var group = await client.Groups[groupObjectId]
                .Request()
                .Select(g => new
                {
                    g.DisplayName
                })
                .GetAsync()
                .ConfigureAwait(false);

            return group;
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
    }
}