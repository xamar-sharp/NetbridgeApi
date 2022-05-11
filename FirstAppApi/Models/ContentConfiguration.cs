using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace FirstAppApi.Models
{
    public class ContentConfiguration:IEntityTypeConfiguration<Content>
    {
        public void Configure(EntityTypeBuilder<Content> builder)
        {
            builder.Property(content => content.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            builder.HasOne(content => content.Owner).WithMany(user => user.Contents).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(content => content.Friend).WithMany(friend => friend.Contents).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
