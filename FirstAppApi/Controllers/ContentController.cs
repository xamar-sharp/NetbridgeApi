using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FirstAppApi.Models;
using Microsoft.EntityFrameworkCore;
using FirstAppApi.ViewModels;
using System.Text;
using static FirstAppApi.Controllers.FileController;
namespace FirstAppApi.Controllers
{
    [Authorize(Roles = "USER, ADMIN")]
    [ApiController]
    [Route("[controller]")]
    public class ContentController : ControllerBase
    {
        private readonly MobileContext _ctx;
        public ContentController(MobileContext ctx)
        {
            _ctx = ctx;
        }
        [HttpPost]
        public async Task<IActionResult> PostContent([FromBody] PostContentModel model)
        {
            Friend friend = _ctx.Friends.Find(model.ToId);
            if (ModelState.IsValid && friend.IsActive)
            {
                User currentUser = friend.Users[0].Email == User.Identity.Name ? friend.Users[0] : friend.Users[1];
                string dataUri = null;
                Models.Content content = new Content()
                {
                    CreatedAt = model.CreatedAt,
                    Friend = friend
                };
                if (friend.Users[0].Email == currentUser.Email)
                {
                    content.Owner =friend.Users[0];
                }
                else if (friend.Users[1].Email == currentUser.Email)
                {
                    content.Owner = friend.Users[1];
                }
                else
                {
                    return BadRequest();
                }
                switch (model.Type)
                {
                    case UnitOfChat.Text:
                        dataUri = Encoding.Default.GetString(model.Data);//Text in Unicode format!!!
                        break;
                    case UnitOfChat.Video:
                        dataUri = await StoreVideoContent(User.Identity.Name, model.Data);
                        break;
                    case UnitOfChat.Image:
                        dataUri = await StoreImageContent(User.Identity.Name, model.Data);
                        break;
                    case UnitOfChat.Audio:
                        throw new NotImplementedException();
                    default:
                        return BadRequest();
                }
                content.DataUri = dataUri;
                content.Type = model.Type;
                try
                {
                    await _ctx.Contents.AddAsync(content);
                    await _ctx.SaveChangesAsync();
                    return Ok();
                }
                catch (Exception ex)
                {

                }
            }
            return BadRequest();
        }
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DropContent(long id)
        {
            Content content = await _ctx.Contents.FindAsync(id);
            _ctx.Contents.Remove(content);
            try
            {
                await _ctx.SaveChangesAsync();
                return Ok();
            }
            catch
            {

            }
            return BadRequest();
        }
        [HttpGet("{useSoft:bool}/{friendId:long}")]
        public async Task<IActionResult> GetContent(bool useSoft, long friendId)
        {
            User currentUser = await _ctx.Users.FirstOrDefaultAsync(user => user.Email == User.Identity.Name && user.IsAlive);
            if (useSoft)
                return new ObjectResult(currentUser.Contents.Where(content => content.Friend.IsActive && content.Friend.Contents.FirstOrDefault().Id == content.Id).OrderBy(content => content.CreatedAt.Ticks).ToList());
            else
            {
                var list = _ctx.Contents.ToList().Where(content => content.Friend.IsActive && content.Friend.Id == friendId && (content.Friend.Users[0].Email==User.Identity.Name || content.Friend.Users[1].Email == User.Identity.Name)).OrderBy(content => content.CreatedAt.Ticks).ToList();
                return new ObjectResult(list);
            }
        }
    }
}
