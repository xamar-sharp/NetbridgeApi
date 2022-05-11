using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using FirstAppApi.Models;
namespace FirstAppApi.ViewModels
{
    public class AuthViewModel
    {
        public string JwtToken { get; set; }
        public RefreshToken RefreshToken { get; set; }
    }
}
