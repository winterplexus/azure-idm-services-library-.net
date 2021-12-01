Azure IDM Services Library for .NET
===================================

Azure IDM (identity management) services library based on .NET 6 platform using Microsoft Identity Client and Microsoft Graph API.

IDM services library can provision and manage Azure B2C users and groups using the primary interface:

* IIdentityManager interface:

```
    public interface IIdentityManager
    {
        string Domain { get; }
        IUserManagement UserServices { get; }
        IGroupManagement GroupServices { get; }
    }
```

where:

* Domain is the name of the B2C domain

* IUserManagement interface

```
    public interface IUserManagement
    {
        Task<UserModel> GetUserBySignInName(string signInName);
        Task<UserModel> GetUserByDisplayName(string displayName);
        Task<UserModel> GetUserByObjectId(string userObjectId);
        Task<string> CreateUser(UserModel userModel);
        Task<bool> DeleteUser(string userName);
        Task<bool> SetUserPasswordByObjectId(string userName, string replacementPassword);
        Task<IList<UserModel>> GetUsers(int limit);
        Task<IList<UserModel>> GetUsersByName(string displayName);
        Task<IList<GroupModel>> GetGroupMembershipBySignInName(string userName);
        string GetUserName(UserModel userModel);
    }
```

* IGroupManagement interface

```
    public interface IGroupManagement
    {
        Task<GroupModel> GetGroupByGroupName(string groupName);
        Task<GroupModel> GetGroupByObjectId(string groupObjectId);
        Task<IList<GroupModel>> GetGroups(int limit);
        Task<IList<GroupModel>> GetGroupsByName(string groupName);
        Task<string> CreateGroup(GroupModel groupModel);
        Task<bool> DeleteGroup(string groupName);
        Task<IList<UserModel>> GetGroupOwners(string groupName);
        Task<IList<UserModel>> GetGroupMembers(string groupName);
        Task<bool> AddOwnerToGroup(string groupName, string userName);
        Task<bool> RemoveOwnerFromGroup(string groupObjectId, string userName);
        Task<bool> AddMemberToGroup(string groupName, string userName);
        Task<bool> RemoveMemberFromGroup(string groupName, string userName);
    }
```
