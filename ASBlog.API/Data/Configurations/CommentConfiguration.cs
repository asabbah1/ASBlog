using ASBlog.API.Models.Blog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASBlog.API.Data.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .UseIdentityColumn();

            builder.Property(x => x.Content)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(x => x.Name)
              .IsRequired()
              .HasMaxLength(80);

            builder.Property(x => x.Email)
              .IsRequired()
              .HasMaxLength(150);

            builder.HasOne(x => x.Post)
                   .WithMany(m => m.Comments)
                   .HasForeignKey(m => m.PostId);


            builder.ToTable("Comments");
        }
    }
}
