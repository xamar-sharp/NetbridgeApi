using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FirstAppApi.Models;
using FirstAppApi.ViewModels;
using Microsoft.EntityFrameworkCore;
namespace FirstAppApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "USER, ADMIN")]
    public class FriendController : ControllerBase
    {
        private readonly MobileContext _ctx;
        public FriendController(MobileContext ctx)
        {
            _ctx = ctx;
        }
        [HttpPost]
        public async Task<IActionResult> AddFriend([FromBody] FriendViewModel model)
        {
            if (ModelState.IsValid && (await _ctx.Users.FirstOrDefaultAsync(user => user.Email == model.To)) is User addUser && addUser.IsAlive)
            {
                User currentUser = await _ctx.Users.FirstOrDefaultAsync(user => user.Email == User.Identity.Name && user.IsAlive);
                Friend friend = new Friend() { IsActive = true, Users = new List<User>(2) };
                friend.Users.Add(currentUser);
                friend.Users.Add(addUser);
                try
                {
                    await _ctx.Friends.AddAsync(friend);
                    await _ctx.SaveChangesAsync();
                    return Ok();
                }
                catch
                {

                }
            }
            return BadRequest();
        }
        [HttpGet]
        public async Task<IActionResult> GetFriends()
        {
            User currentUser = await _ctx.Users.FirstOrDefaultAsync(user => user.Email == User.Identity.Name && user.IsAlive);
            return new ObjectResult(currentUser.Friends.Where(friend => friend.IsActive).ToList());
        }
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DropFriend(long id)
        {
            User currentUser = await _ctx.Users.FirstOrDefaultAsync(user => user.Email == User.Identity.Name && user.IsAlive);
            Friend currentFriend = await _ctx.Friends.FirstOrDefaultAsync(friend => friend.Id == id && friend.IsActive);
            try
            {
                currentFriend.IsActive = false;
                await _ctx.SaveChangesAsync();
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
