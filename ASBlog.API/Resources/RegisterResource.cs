using System;
using System.Collections.Generic;
using System.Text;

namespace ASBlog.API.Resources
{
    public class RegisterResource
    {
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Password { get; set; }
    }
}
