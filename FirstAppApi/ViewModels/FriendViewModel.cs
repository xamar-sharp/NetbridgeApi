using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirstAppApi.Models;
using System.ComponentModel.DataAnnotations;
namespace FirstAppApi.ViewModels
{
    public class FriendViewModel
    {
        [Required]
        public string To { get; set; }
    }
}
