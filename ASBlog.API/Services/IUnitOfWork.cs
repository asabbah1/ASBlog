using ASBlog.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASBlog.API.Services
{
    public interface IUnitOfWork : IDisposable
    {
        ILookupRepository Lookups { get; }
        IPostRepository Posts { get; }
        ICommentRepository Comments { get; }
        Task<int> CommitAsync();
    }
}
