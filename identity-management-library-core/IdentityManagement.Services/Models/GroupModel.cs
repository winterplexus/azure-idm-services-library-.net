//
//  GroupModel.cs
//
//  Copyright (c) Wiregrass Code Technology 2020-2021
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