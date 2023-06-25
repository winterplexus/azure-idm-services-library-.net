//
//  IUserManagement.cs
//
//  Wiregrass Code Technology 2020-2023
//
namespace IdentityManagement.Services
{
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
}