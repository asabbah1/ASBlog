using ASBlog.API.Data;
using ASBlog.API.Models.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASBlog.API.Repositories
{
    public class CommentRepository : BaseData<Comment>, ICommentRepository
    {
        public CommentRepository(ASBlogDbContext context) : base(context)
        {

        }
    }
}
