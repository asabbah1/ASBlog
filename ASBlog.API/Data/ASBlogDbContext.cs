using ASBlog.API.Data.Configurations;
using ASBlog.API.Models;
using ASBlog.API.Models.Auth;
using ASBlog.API.Models.Blog;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASBlog.API.Data
{
    public class ASBlogDbContext : IdentityDbContext<User, Role, Guid>
    {
        public DbSet<Lookup> Lookups { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public ASBlogDbContext(DbContextOptions<ASBlogDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new LookupConfiguration());

            builder.ApplyConfiguration(new PostConfiguration());

            builder.ApplyConfiguration(new CommentConfiguration());

        }
    }
}
