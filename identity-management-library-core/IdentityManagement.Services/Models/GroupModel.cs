﻿//
//  GroupModel.cs
//
//  Wiregrass Code Technology 2020-2023
//
using Microsoft.Graph;
using Newtonsoft.Json;

namespace IdentityManagement.Services
{
    public class GroupModel : Group
    {
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}