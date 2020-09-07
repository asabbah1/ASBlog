using ASBlog.API.Data;
using ASBlog.API.Models;

namespace ASBlog.API.Repositories
{
    public class LookupRepository : BaseData<Lookup>, ILookupRepository
    {
        public LookupRepository(ASBlogDbContext context) : base(context)
        {

        }
    }
}
