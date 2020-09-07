using ASBlog.API.Models.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASBlog.API.Repositories
{
    public interface IPostRepository : IData<Post>
    {
        Task<IEnumerable<Post>> GetAllStoriesByStatus(int Status);
        Task<Post> GetStoryWithComments(int Id);
    }
}
