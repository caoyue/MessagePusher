using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MessagePusher.Core.Extensions
{
    public static class JsonExtensions
    {
        public static JObject ToJson(this string str)
        {
            return JsonConvert.DeserializeObject(str) as JObject;
        }
    }
}
