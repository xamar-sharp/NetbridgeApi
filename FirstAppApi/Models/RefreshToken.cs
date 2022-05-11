using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using FirstAppApi.ViewModels;
namespace FirstAppApi.Models
{
    //[Index("Id",IsUnique =true,Name ="IDX_Tokens")]
    public class RefreshToken
    {
        [Key]
        public virtual long Id { get; set; }
        public virtual DateTime JwtExpired { get; set; }
        public virtual bool WasUsed { get; set; }
        public virtual User Owner { get; set; }
    }
}
