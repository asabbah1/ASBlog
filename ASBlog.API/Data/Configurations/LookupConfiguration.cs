using System;
using System.Security.Cryptography.X509Certificates;
using ASBlog.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ASBlog.API.Data.Configurations
{
    public class LookupConfiguration : IEntityTypeConfiguration<Lookup>
    {
        public void Configure(EntityTypeBuilder<Lookup> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .UseIdentityColumn();

            builder.Property(x => x.Major)
                   .IsRequired()
                   .HasMaxLength(6);

            builder.HasIndex(x => new { x.Major, x.Key })
                   .IsUnique();

            builder.ToTable("Lookups");
        }
    }
}
