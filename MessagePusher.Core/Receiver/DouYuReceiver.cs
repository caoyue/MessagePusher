using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MessagePusher.Core.Models;
using Microsoft.AspNetCore.Http;

namespace MessagePusher.Core.Receiver
{
    public class DouYuReceiver : MessageReceiver, IMessageReceiver
    {
        private const string DouYuAPi = "http://open.douyucdn.cn/api/RoomApi/room/{0}";
        private static readonly List<string> OnlineList = new List<string>();

        private List<string> _roomIdList = new List<string>();
        private readonly List<Message> _messages = new List<Message>();

        public string Name => "DouYu";
        public string Method => "Get";

        public async Task Init(HttpRequest request)
        {
            _roomIdList = Config["Rooms"].ToObject<List<string>>();
            if (_roomIdList != null && _roomIdList.Any())
            {
                foreach (var rId in _roomIdList)
                {
                    var response = await new HttpClient()
                        .GetAsync(string.Format(DouYuAPi, rId));
                    if (response.IsSuccessStatusCode)
                    {
                        var responseStr = await response.Content.ReadAsStringAsync();
                        var json = responseStr.ToJson();
                        if (json["data"]["room_status"].ToString() == "1")
                        {

                            if (!OnlineList.Contains(rId))
                            {
                                var message = new Message
                                {
                                    Title = $"{json["data"]["owner_name"]} start streaming!",
                                    Desc = $"{json["data"]["room_name"]}"
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
    }
}
