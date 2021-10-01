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
        IUserManagement UserServices { get; }
        IGroupManagement GroupServices { get; }
    }
}