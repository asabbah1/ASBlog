using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASBlog.Web.Resources
{
    public class LoginRs
    {
        public int status { get; set; }
        public string accessToken { get; set; }
        public string errorMessage { get; set; }
    }

    public class UserRs
    {
        public string id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public List<string> roles { get; set; }

    }

}
