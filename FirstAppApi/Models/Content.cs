using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace FirstAppApi.Models
{
    [Table("Content")]
    public class Content
    {
        [Key]
        public virtual long Id { get; set; }
        [Required]
        public virtual UnitOfChat Type { get; set; }
        /// <summary>
        /// Data or uri to data
        /// <list type="bullet">
        /// <item>Text - Text</item>
        /// <item>Video - VideoUri</item>
        /// <item>Image - ImageUri</item>
        /// <item>Audio - AudioUri</item>
        /// </list>
        /// </summary>
        public virtual string DataUri { get; set; }
        public virtual DateTime CreatedAt { get; set; }
        public virtual User Owner { get; set; }
        public virtual Friend Friend { get; set; }
    }
}
