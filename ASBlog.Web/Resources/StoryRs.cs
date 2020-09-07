using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASBlog.Web.Resources
{
    public class StoryRs
    {
        public StoryRs()
        {
            comments = new List<Comment>();
        }
        public int id { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string headerImage { get; set; }
        public int status { get; set; }
        public DateTime createdDate { get; set; }
        public string user { get; set; }
        public string userEmail { get; set; }
        public string updateUser { get; set; }
        public List<Comment> comments { get; set; }
    }

    public class Comment
    {
        public string content { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public DateTime createdDate { get; set; }
    }

}
