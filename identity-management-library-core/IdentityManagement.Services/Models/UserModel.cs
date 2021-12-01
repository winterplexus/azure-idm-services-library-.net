//
//  UserModel.cs
//
//  Wiregrass Code Technology 2020-2022
//
using System.Collections.Generic;
using Microsoft.Graph;
using Newtonsoft.Json;

namespace IdentityManagement.Services
{
    public class UserModel : User
    {
        public string AssignedId { get; set; }

        public void SetIdentity(string username)
        {
            var identity = new ObjectIdentity
            {
                IssuerAssignedId = username,
                SignInType = "userName"
            };

            IList<ObjectIdentity> identities = new List<ObjectIdentity>
            {
                identity
            };

            Identities = identities;
        }

        public void SetPasswordProfile(string password)
        {
            PasswordPolicies = "DisablePasswordExpiration";

            PasswordProfile = new PasswordProfile
            {
                ForceChangePasswordNextSignIn = false,
                ODataType = null,
                Password = password
            };
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}