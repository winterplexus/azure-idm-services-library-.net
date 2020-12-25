Azure IDM Services Library for .NET Core
========================================

Azure (IDM) identity management services library based on .NET core platform using Microsoft Graph API and Microsoft Identity Client.

The IDM services library can provision and manage Azure B2C users and groups and the primary interface is:

* IIdentityManager interface:

```
  public interface IIdentityManager
  {
      string Domain { get; }
      IUserServices UserServices { get; }
      IGroupServices GroupServices { get; }
  }
```

where:

* IUserServices interface:

```
  public interface IUserServices
  {
      Task<UserModel> GetUserBySignInName(string signInName);
      Task<UserModel> GetUserByDisplayName(string displayName);
      Task<UserModel> GetUserByObjectId(string userObjectId);
      Task<IList<UserModel>> GetUsers(int limit);
      Task<IList<UserModel>> GetUsersByName(string displayName);
      Task<IList<GroupModel>> GetGroupMembershipBySignInName(string userName);
      Task<bool> SetUserPasswordByObjectId(string userName, string replacementPassword);
      Task<string> CreateUser(UserModel userModel);
      Task<bool> DeleteUser(string userName);
      string GetUserName(UserModel userModel);
  }
```

* IGroupServices interface:

```
  public interface IGroupServices
  {
      Task<GroupModel> GetGroupByGroupName(string groupName);
      Task<GroupModel> GetGroupByObjectId(string groupObjectId);
      Task<IList<GroupModel>> GetGroups(int limit);
      Task<IList<GroupModel>> GetGroupsByName(string groupName);
      Task<string> CreateGroup(GroupModel groupModel);
      Task<bool> DeleteGroup(string groupName);
      Task<bool> AddMemberToGroup(string groupName, string userName);
      Task<bool> RemoveMemberFromGroup(string groupName, string userName);
      Task<bool> AddOwnerToGroup(string groupName, string userName);
      Task<bool> RemoveOwnerFromGroup(string groupObjectId, string userName);
      Task<IList<UserModel>> GetGroupMembers(string groupName);
      Task<IList<UserModel>> GetGroupOwners(string groupName);
  }
```
