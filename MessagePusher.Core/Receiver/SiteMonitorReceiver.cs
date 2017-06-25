using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using MessagePusher.Core.Models;
using Microsoft.AspNetCore.Http;

namespace MessagePusher.Core.Receiver
{
    public class SiteMonitorReceiver : MessageReceiver, IMessageReceiver
    {
        public string Method => "Get";

        private List<string> Sites => Config["Sites"].ToObject<List<string>>();

        private readonly List<Message> _messages = new List<Message>();
        private static readonly Dictionary<string, DateTime> DownTime = new Dictionary<string, DateTime>();

        public async Task Init(HttpRequest request)
        {
            var now = DateTime.Now;
            foreach (var site in Sites)
            {
                var r = new HttpResponseMessage();
                var success = true;
                try
                {
                    r = await new HttpClient().GetAsync(site);
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

        public List<Message> Receive()
        {
            return _messages;
        }

        public bool Verify()
        {
            return _messages != null && _messages.Any();
        }
    }
}
