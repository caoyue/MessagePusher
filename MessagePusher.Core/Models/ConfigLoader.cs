using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MessagePusher.Core.Models
{
    public static class ConfigLoader
    {
        public static JObject Config = Load();

        public static void Reload()
        {
            Config = Load();
        }

        private static JObject Load()
        {
            var str = File.ReadAllText("config.json");
            return JsonConvert.DeserializeObject(str) as JObject;
        }


    }
}
