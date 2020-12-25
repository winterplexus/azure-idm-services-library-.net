//
//  UserModel.cs
//
//  Copyright (c) Wiregrass Code Technology 2020
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
                SignInType = "userName",
                IssuerAssignedId = username
            };

            IList<ObjectIdentity> identities = new List<ObjectIdentity>
            {
                identity
            };

            Identities = identities;
        }

        public void SetPasswordProfile(string password)
        {
            PasswordPolicies = "DisablePasswordExpiration,DisableStrongPassword";

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