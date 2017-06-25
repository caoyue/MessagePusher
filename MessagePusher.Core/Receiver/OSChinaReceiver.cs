using System.Collections.Generic;
using System.Threading.Tasks;
using MessagePusher.Core.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace MessagePusher.Core.Receiver
{
    public class OsChinaReceiver : MessageReceiver, IMessageReceiver
    {
        private JObject _json;

        public string Method => "Post";

        public async Task Init(HttpRequest request)
        {
            _json = (await request.ReadBodyAsync()).ToJson();
        }

        public bool Verify()
        {
            var password = _json["password"].ToString().Trim();
            return Config["Password"].ToString().Trim().Equals(password);
        }

        public List<Message> Receive()
        {
            var messages = new List<Message>();
            if (_json["hook_name"].ToString().Equals("push_hooks"))
            {
                messages.Add(new Message
                {
                    Title = $"{_json["user_name"]} pushed a commit to {_json["repository"]["name"]}",
                    Desc = $"{_json["commits"][0]["message"]}, {_json["commits"][0]["url"]}"
                });
            }
            else if (_json["hook_name"].ToString().Equals("merge_request_hooks"))
            {
                messages.Add(new Message
                {
                    Title = $"{_json["author"]} send a pull request"
                });
            }
            return messages;
        }
    }
}
