using Microsoft.AspNetCore.Identity;
using System;

namespace ASBlog.API.Models.Auth
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
