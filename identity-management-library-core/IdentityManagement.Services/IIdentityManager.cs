//
//  IIdentityManager.cs
//
//  Wiregrass Code Technology 2020-2021
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