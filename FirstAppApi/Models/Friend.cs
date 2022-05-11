using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace FirstAppApi.Models
{
    //[Index("Id",IsUnique =true,Name ="IDX_Friends")]
    public class Friend
    {
        [Key]
        public virtual long Id { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual List<User> Users { get; set; }
        public virtual List<Content> Contents { get; set; }
    }
}
