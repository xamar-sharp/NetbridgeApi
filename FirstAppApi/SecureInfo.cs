using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstAppApi
{
    public static class SecureInfo
    {
        public static readonly string IP = "192.168.43.179";
        public static string GetConnectionString()
        {
            return "TrustServerCertificate=True;Server=DESKTOP-LM4Q538\\SECURESERVER,1433;Database=FirstAppBase;User ID=sa;Password=NullForMeCable";
        }
    }
}
