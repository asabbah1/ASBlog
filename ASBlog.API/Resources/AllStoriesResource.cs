using ASBlog.API.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASBlog.API.Resources
{
    public class AllStoriesResource
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string HeaderImage { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string User { get; set; }
    }
}
