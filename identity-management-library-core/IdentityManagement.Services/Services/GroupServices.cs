//
//  GroupServices.cs
//
//  Wiregrass Code Technology 2020-2021
//
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Graph;

namespace IdentityManagement.Services
{
    public class GroupServices : IGroupServices
    {
        private readonly GraphServiceClient client;
        private readonly IUserServices userServices;

        public GroupServices(GraphServiceClient graphServiceClient, IUserServices userServices)
        {
            client = graphServiceClient;
            this.userServices = userServices;
        }

        public async Task<GroupModel> GetGroupByGroupName(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                throw new ArgumentNullException(nameof(groupName));
            }

            var result = await client.Groups
                .Request()
                .Filter($"displayName eq '{groupName}'")
                .Select(g => new
                {
                    g.Id,
                    g.SecurityEnabled,
                    g.CreatedDateTime,
                    g.DisplayName,
                    g.Description
                })
                .GetAsync();

            return result.CurrentPage.Select(CreateGroupModel).FirstOrDefault();
        }

        public async Task<GroupModel> GetGroupByObjectId(string groupObjectId)
        {
            if (string.IsNullOrEmpty(groupObjectId))
            {
                throw new ArgumentNullException(nameof(groupObjectId));
            }

            var result = await client.Groups[groupObjectId]
                .Request()
                .Select(g => new
                {
                    g.Id,
                    g.SecurityEnabled,
                    g.CreatedDateTime,
                    g.DisplayName,
                    g.Description
                })
                .GetAsync();

            return result != null ? CreateGroupModel(result) : null;
        }

        public async Task<IList<GroupModel>> GetGroups(int limit)
        {
            if (limit < 1)
            {
                throw new ArgumentException("limit must be greater than 0");
            }

            var groups = await client.Groups
                .Request()
                .Top(limit)
                .Select(g => new
                {
                    g.Id,
                    g.SecurityEnabled,
                    g.CreatedDateTime,
                    g.DisplayName,
                    g.Description,
                })
                .GetAsync();

            var groupsList = groups.Select(group => (CreateGroupModel(group))).ToList();
            if (groupsList.Count >= limit)
            {
                return groupsList;
            }

            do
            {
                if (groups.NextPageRequest != null)
                {
                    groups = await groups.NextPageRequest.GetAsync();
                    groupsList.AddRange(groups.Select(group => (CreateGroupModel(group))));
                }
                else
                {
                    groups = null;
                }
            } while (groups != null);

            return groupsList;
        }

        public async Task<IList<GroupModel>> GetGroupsByName(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                throw new ArgumentNullException(nameof(groupName));
            }

            var groups = await client.Groups
                .Request()
                .Filter($"startswith(displayName, '{groupName}')")
                .Select(g => new
                {
                    g.Id,
                    g.SecurityEnabled,
                    g.CreatedDateTime,
                    g.DisplayName,
                    g.Description,
                })
                .GetAsync();

            var groupsList = groups.Select(group => (CreateGroupModel(group))).ToList();

            do
            {
                if (groups.NextPageRequest != null)
                {
                    groups = await groups.NextPageRequest.GetAsync();
                    groupsList.AddRange(groups.Select(group => (CreateGroupModel(group))));
                }
                else
                {
                    groups = null;
                }
            } while (groups != null);

            return groupsList;
        }

        public async Task<string> CreateGroup(GroupModel groupModel)
        {
            if (groupModel == null)
            {
                throw new ArgumentNullException(nameof(groupModel));
            }

            var group = await client.Groups
                .Request()
                .AddAsync(new Group
                {
                    DisplayName = groupModel.DisplayName,
                    Description = groupModel.Description,
                    MailEnabled = false,
                    MailNickname = groupModel.DisplayName,
                    SecurityEnabled = true
                });

            return group.Id;
        }

        public async Task<bool> DeleteGroup(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                throw new ArgumentNullException(nameof(groupName));
            }

            var groupObjectId = GetGroupObjectId(groupName);
            if (groupObjectId == null)
            {
                return false;
            }

            await client.Groups[groupObjectId]
                .Request()
                .DeleteAsync();

            return true;
        }

        public async Task<bool> AddMemberToGroup(string groupName, string userName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                throw new ArgumentNullException(nameof(groupName));
            }
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            var groupObjectId = GetGroupObjectId(groupName);
            if (groupObjectId == null)
            {
                return false;
            }
            var userObjectId = GetUserObjectId(userName);
            if (userObjectId == null)
            {
                return false;
            }

            var userDirectoryObject = new DirectoryObject
            {
                Id = userObjectId
            };

            await client.Groups[groupObjectId].Members.References
                .Request()
                .AddAsync(userDirectoryObject);

            return true;
        }

        public async Task<bool> RemoveMemberFromGroup(string groupName, string userName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                throw new ArgumentNullException(nameof(groupName));
            }
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            var groupObjectId = GetGroupObjectId(groupName);
            if (groupObjectId == null)
            {
                return false;
            }
            var userObjectId = GetUserObjectId(userName);
            if (userObjectId == null)
            {
                return false;
            }

            await client.Groups[groupObjectId].Members[userObjectId].Reference
                .Request()
                .DeleteAsync();

            return true;
        }

        public async Task<bool> AddOwnerToGroup(string groupName, string userName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                throw new ArgumentNullException(nameof(groupName));
            }
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            var groupObjectId = GetGroupObjectId(groupName);
            if (groupObjectId == null)
            {
                return false;
            }
            var userObjectId = GetUserObjectId(userName);
            if (userObjectId == null)
            {
                return false;
            }

            var userDirectoryObject = new DirectoryObject
            {
                Id = userObjectId
            };

            await client.Groups[groupObjectId].Owners.References
                .Request()
                .AddAsync(userDirectoryObject);

            return true;
        }

        public async Task<bool> RemoveOwnerFromGroup(string groupName, string userName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                throw new ArgumentNullException(nameof(groupName));
            }
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException(nameof(userName));
            }

            var groupObjectId = GetGroupObjectId(groupName);
            if (groupObjectId == null)
            {
                return false;
            }
            var userObjectId = GetUserObjectId(userName);
            if (userObjectId == null)
            {
                return false;
            }

            await client.Groups[groupObjectId].Owners[userObjectId].Reference
                .Request()
                .DeleteAsync();

            return true;
        }

        public async Task<IList<UserModel>> GetGroupMembers(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                throw new ArgumentNullException(nameof(groupName));
            }

            var groupObjectId = GetGroupObjectId(groupName);
            if (groupObjectId == null)
            {
                return null;
            }

            var group = await client.Groups[groupObjectId]
                .Request()
                .Expand("members")
                .GetAsync();

            var members = group.Members;

            var membersCount = members.Count;
            if (membersCount < 1)
            {
                return null;
            }

            var groupMembers = new List<UserModel>();

            do
            {
                groupMembers.AddRange(members.Select(member => userServices.GetUserByObjectId(member.Id).Result));
            }
            while (members.NextPageRequest != null && (members = await members.NextPageRequest.GetAsync()).Count > 0);

            return groupMembers;
        }

        public async Task<IList<UserModel>> GetGroupOwners(string groupName)
        {
            if (string.IsNullOrEmpty(groupName))
            {
                throw new ArgumentNullException(nameof(groupName));
            }

            var groupObjectId = GetGroupObjectId(groupName);
            if (groupObjectId == null)
            {
                return null;
            }

            var group = await client.Groups[groupObjectId]
                .Request()
                .Expand("owners")
                .GetAsync();

            var owners = group.Owners;

            var ownersCount = owners.Count;
            if (ownersCount < 1)
            {
                return null;
            }

            var groupMembers = new List<UserModel>();

            do
            {
                groupMembers.AddRange(owners.Select(owner => userServices.GetUserByObjectId(owner.Id).Result));
            }
            while (owners.NextPageRequest != null && (owners = await owners.NextPageRequest.GetAsync()).Count > 0);

            return groupMembers;
        }

        private string GetGroupObjectId(string groupName)
        {
            var group = Task.Run(async () => await GetGroupByGroupName(groupName)).Result;

            return group?.Id;
        }

        private string GetUserObjectId(string userName)
        {
            var user = Task.Run(async () => await userServices.GetUserBySignInName(userName)).Result;

            return user?.Id;
        }

        private static GroupModel CreateGroupModel(Group group)
        {
            var groupModel = new GroupModel
            {
                Id = group.Id,
                SecurityEnabled = group.SecurityEnabled,
                CreatedDateTime = group.CreatedDateTime,
                DisplayName = group.DisplayName,
                Description = group.Description
            };
            return groupModel;
        }
    }
}