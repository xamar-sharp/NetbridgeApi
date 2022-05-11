using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
namespace FirstAppApi.Models
{
    public class MobileContextFactory:IDesignTimeDbContextFactory<MobileContext>
    {
        public MobileContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<MobileContext> builder = new DbContextOptionsBuilder<MobileContext>();
            builder.UseSqlServer(SecureInfo.GetConnectionString()).UseLazyLoadingProxies().LogTo(LogWorker.WriteLog);
            return new MobileContext(builder.Options);
        }
    }
}
