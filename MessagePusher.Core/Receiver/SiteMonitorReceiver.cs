using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MessagePusher.Core.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace MessagePusher.Core.Receiver
{
    public class SiteMonitorReceiver : MessageReceiver, IMessageReceiver
    {
        public string Method => "Get";
        public List<string> SendTo => _config["SendTo"].ToObject<List<string>>();

        private List<string> Sites => _config["Sites"].ToObject<List<string>>();

        private readonly List<Message> _messages = new List<Message>();
        private static readonly Dictionary<string, DateTime> DownTime = new Dictionary<string, DateTime>();

        private JToken _config;

        private static readonly HttpClient Client = new HttpClient();

        public List<Message> Receive()
        {
            return _messages;
        }

        public async Task Init(HttpRequest request, JToken config)
        {
            _config = config;

            var now = DateTime.Now;
            foreach (var site in Sites)
            {
                var r = new HttpResponseMessage();
                var success = true;
                try
                {
                    r = await Client.GetAsync(site);
                }
                catch (Exception)
                {
                    success = false;
                }

                if (success && r != null && r.IsSuccessStatusCode)
                {
                    if (DownTime.ContainsKey(site))
                    {
                        _messages.Add(new Message
                        {
                            Title = $"site {site} is back online",
                            Desc = $"goes offline for {(now - DownTime[site]).TotalMinutes} minutes"
                        });
                        DownTime.Remove(site);
                    }
                }
                else
                {
                    if (!DownTime.ContainsKey(site))
                    {
                        DownTime.Add(site, now);
                        _messages.Add(new Message
                        {
                            Title = $"site {site} is offline.",
                            Desc = ""
                        });
                    }
                }
            }
        }

        public bool Verify()
        {
            return _messages != null && _messages.Any();
        }
    }
}
