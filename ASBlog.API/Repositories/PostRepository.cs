using ASBlog.API.Data;
using ASBlog.API.Models.Blog;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASBlog.API.Repositories
{
    public class PostRepository : BaseData<Post>, IPostRepository
    {
        private ASBlogDbContext ASBlogDbContext
        {
            get { return Context as ASBlogDbContext; }
        }
        public PostRepository(ASBlogDbContext context) : base(context) { }
        
        public async Task<IEnumerable<Post>> GetAllStoriesByStatus(int Status)
        {
            return await ASBlogDbContext.Posts
                .Include(m=> m.User)
                .Include(m=> m.UpdateUser)
                .Where(x => x.Status == Status)
                .ToListAsync();
        }

        public async Task<Post> GetStoryWithComments(int Id)
        {
            return await ASBlogDbContext.Posts
                .Include(m => m.Comments)
                .Include(m => m.User)
                .Include(m => m.UpdateUser)
                .SingleOrDefaultAsync(m => m.Id == Id);
        }



    }
}
