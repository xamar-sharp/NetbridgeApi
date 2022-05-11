using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
namespace FirstAppApi
{
    public static class LogWorker
    {
        public static async void WriteLog(string log)
        {
            await using(StreamWriter writer = File.AppendText(Path.Combine(Environment.CurrentDirectory, "logEFCore.txt")))
            {
                using(JsonTextWriter jsonTextWriter=new JsonTextWriter(writer))
                {
                    await jsonTextWriter.WriteStartObjectAsync();
                    await jsonTextWriter.WritePropertyNameAsync($"{DateTime.Now}!{Guid.NewGuid()}");
                    await jsonTextWriter.WriteValueAsync(log);
                    await jsonTextWriter.WriteEndObjectAsync();
                }
            }
        }
    }
}
