using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace FirstAppApi.ViewModels
{
    public class WebCredential
    {
        [MaxLength(20)]
        public string Name { get; set; }
        [Required]
        [MaxLength(15)]
        public string Password { get; set; }
        [Required]
        [MaxLength(50)]
        [EmailAddress]
        public string Email { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        [MaxLength(int.MaxValue)]
        public byte[] Data { get; set; }
    }
}
