Azure IDM Services Library for .NET
===================================

Azure IDM (identity management) services library based on .NET 6 platform using Microsoft Identity Client library nd Microsoft Graph API.

IDM services library can provision and manage Azure B2C users and groups using the primary interface:

* IIdentityManager interface:

```
    public interface IIdentityManager
    {
        string? Tenant { get; }
        IUserManagement? UserServices { get; }
        IGroupManagement? GroupServices { get; }
    }
```

where:

* Domain is the name of the B2C domain

* IUserManagement interface

```
    public interface IUserManagement
    {
        Task<string?> CreateUser(UserModel userModel);
        Task<bool> DeleteUser(string userName);
        Task<UserModel?> GetUserBySignInName(string signInName);
        Task<UserModel?> GetUserByDisplayName(string displayName);
        Task<UserModel?> GetUserByObjectId(string userObjectId);
        Task<IList<UserModel>> GetUsers(int limit);
        Task<IList<UserModel>> GetUsersByDisplayName(string displayName);
        Task<IList<GroupModel>?> GetGroupMembershipBySignInName(string signInName);
        string? GetUserName(UserModel userModel);
        Task<bool> SetUserPasswordByObjectId(string userName, string replacementPassword);
    }
```

* IGroupManagement interface

```
    public interface IGroupManagement
    {
        Task<string> CreateGroup(GroupModel groupModel);
        Task<bool> DeleteGroup(string groupName);
        Task<GroupModel?> GetGroupByGroupName(string groupName);
        Task<GroupModel?> GetGroupByObjectId(string groupObjectId);
        Task<IList<GroupModel>> GetGroups(int limit);
        Task<IList<GroupModel>> GetGroupsByName(string groupName);
        Task<IList<UserModel?>?> GetGroupOwners(string groupName);
        Task<IList<UserModel?>?> GetGroupMembers(string groupName);
        Task<bool> AddOwnerToGroup(string groupName, string userName);
        Task<bool> RemoveOwnerFromGroup(string groupName, string userName);
        Task<bool> AddMemberToGroup(string groupName, string userName);
        Task<bool> RemoveMemberFromGroup(string groupName, string userName);
    }
```
