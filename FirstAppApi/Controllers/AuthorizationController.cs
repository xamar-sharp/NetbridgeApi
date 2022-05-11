using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using FirstAppApi.Models;
using FirstAppApi.ViewModels;
using System.IO;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using static FirstAppApi.Controllers.FileController;
namespace FirstAppApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly MobileContext _ctx;
        public AuthorizationController(MobileContext ctx)
        {
            _ctx = ctx;
        }

        [HttpPost("{isSignIn:bool}")]
        public async Task<IActionResult> Authorize([FromBody] WebCredential credential, bool isSignIn)
        {
            if (ModelState.IsValid)
            {
                string jwt = null;
                RefreshToken token = null;
                if (isSignIn)
                {
                    if ((await _ctx.Users.FirstOrDefaultAsync(user => user.Email == credential.Email && user.Password == credential.Password && user.IsAlive)) is User user)
                    {
                        jwt = await Authenticate(credential.Email);
                        token = new RefreshToken() { Owner = user, JwtExpired = DateTime.UtcNow.AddDays(5) };
                        try
                        {
                            await _ctx.Tokens.AddAsync(token);
                            await _ctx.SaveChangesAsync();
                            return Ok(new AuthViewModel() { JwtToken = jwt, RefreshToken = token });
                        }
                        catch
                        {

                        }
                    }
                }
                else
                {
                    if (await _ctx.Users.FirstOrDefaultAsync(user => user.Email == credential.Email && user.Password == credential.Password && user.IsAlive) is null)
                    {
                        User newUser = new User()
                        {
                            Description = "none",
                            Email = credential.Email,
                            IconUri = await StoreIconForUser(credential.Email, credential.Data is null ? System.IO.File.ReadAllBytes("C:\\ImagesForApp\\empty.png") : credential.Data),
                            Password = credential.Password,
                            UserName = credential.Name,
                            IsAlive = true
                        };
                        try
                        {
                            await _ctx.Users.AddAsync(newUser);
                            await _ctx.SaveChangesAsync();
                            jwt = await Authenticate(credential.Email);
                            token = new RefreshToken() { Owner = newUser, JwtExpired = DateTime.UtcNow.AddDays(5) };
                            await _ctx.Tokens.AddAsync(token);
                            await _ctx.SaveChangesAsync();
                            return Ok(new AuthViewModel() { JwtToken = jwt, RefreshToken = token });
                        }
                        catch (Exception e)
                        {

                        }
                    }
                }
            }
            return BadRequest();
        }
        [HttpPut]
        [Authorize(Roles = "USER, ADMIN")]
        public async Task<IActionResult> UpdateProfile([FromBody] WebCredential credential)
        {
            if ((await _ctx.Users.FirstOrDefaultAsync(user => user.Email == User.Identity.Name && user.IsAlive)) is User user)
            {
                if (credential.Data is not null)
                    user.IconUri = await StoreIconForUser(user.Email, credential.Data);
                user.Description = credential.Description;
                user.Password = credential.Password;
                user.UserName = credential.Name;
                user.Email = credential.Email;
                try
                {
                    await _ctx.SaveChangesAsync();
                    string jwt = await Authenticate(user.Email);
                    RefreshToken newToken = new RefreshToken() { Owner = user, JwtExpired = DateTime.UtcNow.AddDays(5) };
                    await _ctx.Tokens.AddAsync(newToken);
                    await _ctx.SaveChangesAsync();
                    return Ok(new AuthViewModel() { JwtToken = jwt, RefreshToken = newToken });
                }
                catch
                {

                }
            }
            return BadRequest();
        }
        [Authorize(Roles = "USER, ADMIN")]
        [HttpDelete("{email}/{password}")]
        public async Task<IActionResult> DeleteProfile(string email, string password)
        {
            if ((await _ctx.Users.FirstOrDefaultAsync(user => user.Email == email && user.Password == password && user.IsAlive)) is User user)
            {
                user.IsAlive = false;
                try
                {
                    await _ctx.SaveChangesAsync();
                    return Ok();
                }
                catch
                {

                }
            }
            return BadRequest();
        }
        [HttpGet("{id}/{email}/{password}")]
        public async Task<IActionResult> UseRefreshToken(long id, string email, string password)
        {
            if ((await _ctx.Tokens.FindAsync(id)) is RefreshToken token && token.Owner.Email == email && token.Owner.Password == password && token.Owner.IsAlive)
            {
                string jwt = await Authenticate(email);
                RefreshToken newToken = new RefreshToken() { Owner = token.Owner, JwtExpired = DateTime.UtcNow.AddDays(1) };
                try
                {
                    await _ctx.Tokens.AddAsync(newToken);
                    await _ctx.SaveChangesAsync();
                    return Ok(new AuthViewModel() { JwtToken = jwt, RefreshToken = newToken });
                }
                catch
                {

                }
            }
            return BadRequest();
        }
        internal static async Task<byte[]> GetSecurityKey()
        {
            return Encoding.UTF8.GetBytes(await System.IO.File.ReadAllTextAsync("C:\\securityKey.txt"));
        }
        internal static async Task<string> GetEmail()
        {
            return await System.IO.File.ReadAllTextAsync("C:\\securityEmail.txt");
        }
        [NonAction]
        internal async Task<IEnumerable<Claim>> GetClaims(string email)
        {
            IEnumerable<Claim> claims = new Claim[] {
                new Claim(ClaimsIdentity.DefaultNameClaimType,email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType,email == (await GetEmail())?"ADMIN":"USER")
            };
            ClaimsIdentity identity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            return identity.Claims;
        }
        [NonAction]
        internal async Task<string> Authenticate(string email)
        {
            IEnumerable<Claim> claims = await GetClaims(email);
            SigningCredentials credentials = new SigningCredentials(new SymmetricSecurityKey(await GetSecurityKey()), "HS256");
            JwtSecurityToken token = new JwtSecurityToken("ASP", "APP", claims, DateTime.UtcNow, DateTime.UtcNow.AddDays(5), credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
