using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FirstAppApi.Controllers;
using System.IO;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace FirstAppApi.Models
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");
            builder.HasAlternateKey(user => user.Email);
            builder.Property(user => user.Password).HasDefaultValue("none");
            builder.Property(user => user.UserName).HasDefaultValue("none");
            builder.Property(user => user.IsAlive).HasDefaultValue(true);
        }
    }
}
