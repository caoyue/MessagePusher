using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MessagePusher.Core.Extensions;
using MessagePusher.Core.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace MessagePusher.Core.Receiver
{
    public class TextReceiver : MessageReceiver, IMessageReceiver
    {
        private JToken _config;
        private string _text = string.Empty;

        public string Method => "Post";

        public List<string> SendTo => _config["SendTo"].ToObject<List<string>>();

        public async Task Init(HttpRequest request, JToken config)
        {
            _config = config;
            _text = await request.ReadBodyAsync();
        }

        public List<Message> Receive()
        {
            return new List<Message>
            {
                new Message
                {
                    Title = _text
                }
            };
        }

        public bool Verify()
        {
            return !_text.IsNullOrWhiteSpace();
        }
    }
}
