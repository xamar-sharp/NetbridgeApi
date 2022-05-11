using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace FirstAppApi.Models
{
    public class FriendConfiguration:IEntityTypeConfiguration<Friend>
    {
        public void Configure(EntityTypeBuilder<Friend> builder)
        {
            builder.HasMany(friend => friend.Users).WithMany(user => user.Friends);
            builder.Property(friend => friend.IsActive).HasDefaultValue(true);
        }
    }
}
