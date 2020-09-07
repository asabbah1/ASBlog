using ASBlog.API.Models.Auth;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace ASBlog.API.Models.Blog
{
    public class Post
    {
        public Post()
        {
            Comments = new Collection<Comment>();
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string HeaderImage { get; set; }
        public int Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public User UpdateUser { get; set; }
        public User User { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
