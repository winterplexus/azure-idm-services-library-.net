//
//  IUserManagement.cs
//
//  Copyright (c) Wiregrass Code Technology 2020-2021
//
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityManagement.Services
{
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
}