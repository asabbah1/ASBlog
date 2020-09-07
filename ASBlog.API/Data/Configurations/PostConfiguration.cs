using ASBlog.API.Models.Blog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASBlog.API.Data.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .UseIdentityColumn();

            builder.Property(x=> x.Title)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.ToTable("Posts");
        }
    }
}
