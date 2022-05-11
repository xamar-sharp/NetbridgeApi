using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace FirstAppApi.Models
{
    public class MobileContext:DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Content> Contents { get; set;}
        public virtual DbSet<Friend> Friends { get; set; }
        public virtual DbSet<RefreshToken> Tokens { get; set; }
        public MobileContext(DbContextOptions<MobileContext> opt) : base(opt)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new FriendConfiguration());
            builder.ApplyConfiguration(new ContentConfiguration());
            builder.ApplyConfiguration(new RefreshTokenConfiguration());
        }
    }
}
