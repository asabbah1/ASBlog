using ASBlog.API.Data;
using ASBlog.API.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASBlog.API.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ASBlogDbContext _context;
        private LookupRepository _lookupRepository;
        private PostRepository _postRepository;
        private CommentRepository _commentRepository;
        public UnitOfWork(ASBlogDbContext context)
        {
            _context = context;
        }
        public ILookupRepository Lookups => _lookupRepository ??= new LookupRepository(_context);

        public IPostRepository Posts => _postRepository ??= new PostRepository(_context);

        public ICommentRepository Comments => _commentRepository ??= new CommentRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
