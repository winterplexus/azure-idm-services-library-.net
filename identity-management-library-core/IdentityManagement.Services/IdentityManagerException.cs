//
//  IdentityManagerException.cs
//
//  Wiregrass Code Technology 2020-2023
//
namespace IdentityManagement.Services
{
    public class IdentityManagerException : Exception
    {
        public IdentityManagerException()
        {
        }

        public IdentityManagerException(string message)
            : base(message)
        {
        }

        public IdentityManagerException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}