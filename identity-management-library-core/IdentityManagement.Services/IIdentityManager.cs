//
//  IIdentityManager.cs
//
//  Copyright (c) Wiregrass Code Technology 2020
//
namespace IdentityManagement.Services
{
    public interface IIdentityManager
    {
        string Domain { get; }
        IUserServices UserServices { get; }
        IGroupServices GroupServices { get; }
    }
}