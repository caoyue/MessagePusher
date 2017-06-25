using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MessagePusher.Core.Models;
using Microsoft.AspNetCore.Http;

namespace MessagePusher.Core.Receiver
{
    public class TwitchReceiver : MessageReceiver, IMessageReceiver
    {
        private const string TwitchApi = "https://api.twitch.tv/kraken/streams/{0}";
        private static readonly List<string> OnlineList = new List<string>();

        private List<string> _channelList = new List<string>();
        private readonly List<Message> _messages = new List<Message>();

        private static readonly HttpClient Client = new HttpClient();

        public string Method => "Get";

        public async Task Init(HttpRequest request)
        {
            Client.DefaultRequestHeaders.Add("Client-ID", Config["ClientId"].ToString());
            _channelList = Config["Channels"].ToObject<List<string>>();
            if (_channelList != null && _channelList.Any())
            {
                foreach (var cId in _channelList)
                {
                    var response = await Client.GetAsync(string.Format(TwitchApi, cId));
                    if (response.IsSuccessStatusCode)
                    {
                        var responseStr = await response.Content.ReadAsStringAsync();
                        var json = responseStr.ToJson();
                        if (json["stream"] != null)
                        {

                            if (!OnlineList.Contains(cId))
                            {
                                var message = new Message
                                {
                                    Title = $"{cId}({json["stream"]["channel"]["display_name"]}) start streaming",
                                    Desc = $"{json["stream"]["channel"]["status"]}, {json["stream"]["channel"]["url"]}"
                                };
                                _messages.Add(message);
                                OnlineList.Add(cId);
                            }
                        }
                        else
                        {
                            OnlineList.Remove(cId);
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
