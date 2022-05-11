using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace FirstAppApi.Models
{
    //[Index("Email",IsUnique =true,Name ="IDX_Users")]
    public class User
    {
        [Key]
        public virtual long Id { get; set; }
        [MaxLength(20)]
        [Required]
        public virtual string UserName { get; set; }
        [Required]
        public virtual bool IsAlive { get; set; }
        [MaxLength(50)]
        [Required]
        [EmailAddress]
        public virtual string Email { get; set; }
        [MaxLength(15)]
        [Required]
        public virtual string Password { get; set; }
        public virtual string IconUri { get; set; }
        [MaxLength(200)]
        public virtual string Description { get; set; }
        public virtual ICollection<Friend> Friends { get; set; }
        public virtual ICollection<Content> Contents { get; set; }
        public virtual ICollection<RefreshToken> Tokens { get; set; }
    }
}
