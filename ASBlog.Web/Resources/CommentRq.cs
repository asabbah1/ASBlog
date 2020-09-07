using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASBlog.Web.Resources
{
    public class CommentRq
    {
        public int postId { get; set; }
        public string content { get; set; }
        public string name { get; set; }
        public string email { get; set; }
    }

}
