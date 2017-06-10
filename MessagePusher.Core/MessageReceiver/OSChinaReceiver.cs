using System.Collections.Generic;
using System.Linq;
using MessagePusher.Core.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace MessagePusher.Core.MessageReceiver
{
    public class OsChinaReceiver : MessageConfig, IMessageReceiver
    {
        public string Name => "OsChina";

        public string Method => "Post";

        public List<string> SendTo => Config["SendTo"].ToObject<List<string>>();

        public bool Verify(IQueryCollection query, IHeaderDictionary headers, JObject json)
        {
            var password = json["password"].ToString().Trim();
            return Config["Password"].ToString().Trim().Equals(password);
        }

        public Result<Message> Receive(IQueryCollection query, IHeaderDictionary headers, JObject json)
        {
            var result = new Result<Message> { Success = false };
            if (!Verify(query, headers, json))
            {
                result.Message = "Password Invalid";
                return result;
            }

            result.Success = true;
            result.Data = new Message
            {
                Title = $"{json["user_name"]} pushed a commit to {json["repository"]["name"]}",
                Desc = $"{json["commits"][0]["message"]}, {json["commits"][0]["url"]}"
            };
            return result;
        }
    }
}