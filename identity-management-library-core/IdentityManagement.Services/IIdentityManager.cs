﻿//
//  IIdentityManager.cs
//
//  Wiregrass Code Technology 2020-2022
//
namespace IdentityManagement.Services
{
    public interface IIdentityManager
    {
        string Tenant { get; }
        IUserManagement UserServices { get; }
        IGroupManagement GroupServices { get; }
    }
}