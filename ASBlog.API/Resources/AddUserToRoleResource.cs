using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASBlog.API.Resources
{
    public class AddUserToRoleResource
    {
        public string UserEmail { get; set; }
        public string RoleName { get; set; }
    }
}
