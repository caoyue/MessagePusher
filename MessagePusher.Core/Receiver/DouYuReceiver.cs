using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MessagePusher.Core.Extensions;
using MessagePusher.Core.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace MessagePusher.Core.Receiver
{
    public class DouYuReceiver : MessageReceiver, IMessageReceiver
    {
        private const string DouYuAPi = "http://open.douyucdn.cn/api/RoomApi/room/{0}";
        private static readonly List<string> OnlineList = new List<string>();

        private List<string> _roomIdList = new List<string>();
        private readonly List<Message> _messages = new List<Message>();
        private JToken _config;

        private static readonly HttpClient Client = new HttpClient();

        public string Method => "Get";

        public List<string> SendTo => _config["SendTo"].ToObject<List<string>>();

        public async Task Init(HttpRequest request, JToken config)
        {
            _config = config;
            _roomIdList = _config["Rooms"].ToObject<List<string>>();

            if (_roomIdList == null || !_roomIdList.Any())
            {
                return;
            }

            foreach (var rId in _roomIdList)
            {
                var response = await Client.GetAsync(string.Format(DouYuAPi, rId));
                if (!response.IsSuccessStatusCode)
                {
                    continue;
                }

                var responseStr = await response.Content.ReadAsStringAsync();
                var json = responseStr.ToJson();
                if (json["data"]["room_status"].ToString() == "1")
                {
                    if (!OnlineList.Contains(rId))
                    {
                        var message = new Message
                        {
                            Title = $"{json["data"]["owner_name"]} start streaming",
                            Desc = $"{json["data"]["room_name"]}",
                            From = Name
                        };
                        _messages.Add(message);
                        OnlineList.Add(rId);
                    }
                }
                else
                {
                    OnlineList.Remove(rId);
                }
            }
        }

        public bool Verify()
        {
            return _messages != null && _messages.Any();
        }

        public List<Message> Receive()
        {
            return _messages;
        }

        public void Config(JToken config)
        {
            throw new NotImplementedException();
        }
    }
}
