using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace FirstAppApi.Models
{
    public class RefreshTokenConfiguration:IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasOne(token => token.Owner).WithMany(owner => owner.Tokens).OnDelete(DeleteBehavior.Cascade);
            builder.Property(token => token.JwtExpired).HasDefaultValueSql("DATEADD(day,1,GETUTCDATE())");
            builder.Property(token => token.WasUsed).HasDefaultValue(false);
        }
    }
}
