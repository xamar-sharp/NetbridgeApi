using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirstAppApi.Models;
using System.ComponentModel.DataAnnotations;
namespace FirstAppApi.ViewModels
{
    public class ContentViewModel
    {
        [Required]
        public UnitOfChat Type { get; set; }
        [MaxLength(int.MaxValue)]
        [Required]
        public byte[] Data { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        public Friend To { get; set; }
    }
}
