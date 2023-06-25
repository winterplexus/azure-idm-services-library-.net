//
//  IGroupManagement.cs
//
//  Wiregrass Code Technology 2020-2023
//
namespace IdentityManagement.Services
{
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
}