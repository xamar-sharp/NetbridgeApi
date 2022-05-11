using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using io = System.IO;
using System.IO;
namespace FirstAppApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FileController : ControllerBase
    {
        [HttpGet("{email}/{type}/{fileName}")]
        public IActionResult GetFile(string email, string type, string fileName)
        {
            string path = Path.Combine("C:\\FirstAppServer\\Users", email.Replace("@", "."));
            switch (type)
            {
                case "UserIcon":
                    path = Path.Combine(path, "Icons");
                    break;
                case "ImageContent":
                    path = Path.Combine(path, "Content\\Images");
                    break;
                case "VideoContent":
                    path = Path.Combine(path, "Content\\Videos");
                    break;
                default:
                    path = Path.Combine(path, "none.jpg");
                    return PhysicalFile(path, "image/jpeg");
            }
            return PhysicalFile(Path.Combine(path, fileName), "application/octet-stream");
        }
        [NonAction]
        public static async Task<string> StoreIconForUser(string email, byte[] data)
        {
            InitDirectory(email);
            string res = $"{ Guid.NewGuid() }{ DateTime.UtcNow.ToString("s").Replace(":", "-")}.jpg";
            await using (FileStream stream = io.File.Create($"C:\\FirstAppServer\\Users\\{email.Replace("@",".")}\\Icons\\{res}"))
            {
                await stream.WriteAsync(data);
            }
            return $"http://{SecureInfo.IP}:5000/file/{email}/UserIcon/{res}";
        }
        [NonAction]
        public static async Task<string> StoreVideoContent(string email, byte[] data)
        {
            InitDirectory(email);
            string res = $"{ Guid.NewGuid() }{ DateTime.UtcNow.ToString("s").Replace(":", "-")}.mp4";
            await using (FileStream stream = io.File.Create($"C:\\FirstAppServer\\Users\\{email.Replace("@", ".")}\\Content\\Videos\\{res}"))
            {
                await stream.WriteAsync(data);
            }
            return $"http://{SecureInfo.IP}:5000/file/{email}/VideoContent/{res}";

        }
        [NonAction]
        public static async Task<string> StoreImageContent(string email, byte[] data)
        {
            InitDirectory(email);
            string res = $"{ Guid.NewGuid() }{ DateTime.UtcNow.ToString("s").Replace(":", "-")}.jpg";
            await using (FileStream stream = io.File.Create($"C:\\FirstAppServer\\Users\\{email.Replace("@", ".")}\\Content\\Images\\{res}"))
            {
                await stream.WriteAsync(data);
            }
            return $"http://{SecureInfo.IP}:5000/file/{email}/ImageContent/{res}";
        }
        [NonAction]
        public static void InitDirectory(string email)
        {
            string targetPath = Path.Combine("C:\\FirstAppServer\\Users", email.Replace("@", "."));
            if (io.Directory.Exists(targetPath))
            {
                return;
            }
            else
            {
                DirectoryInfo root=Directory.CreateDirectory(targetPath);
                root.CreateSubdirectory("Icons");
                DirectoryInfo content = root.CreateSubdirectory("Content");
                content.CreateSubdirectory("Videos");
                content.CreateSubdirectory("Images");
                string emptyPath = Path.Combine(root.FullName, "none.jpg");
                io.File.Create(emptyPath).Close();
                io.File.WriteAllBytes(emptyPath, io.File.ReadAllBytes("C:\\emptyimage.jpg"));
            }
        }
    }
}
