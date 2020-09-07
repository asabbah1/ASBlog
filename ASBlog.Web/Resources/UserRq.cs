using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASBlog.Web.Resources
{
    public class LoginRq
    {
        public string Email { get; set; }
        public string Password { get; set; }

    }

    public class RegisterRq
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Please Enter Valid Email Address")]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Password Should be at least 8 characters")]
        public string Password { get; set; }
    }
}
