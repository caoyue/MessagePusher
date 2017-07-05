using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MessagePusher.Core.Extensions;
using MessagePusher.Core.Models;
using Newtonsoft.Json.Linq;

namespace MessagePusher.Core.Sender
{
    public class SlackSender : MessageConfig, IMessageSender
    {
        private static readonly HttpClient Client = new HttpClient();
        private string _webhook;

        public void Config(JToken config)
        {
            _webhook = config["Webhook"].ToString();
        }

        public async Task<Result> Send(List<Message> messages)
        {
            var result = new Result
            {
                Success = true,
                Message = "Success"
            };

            if (_webhook.IsNullOrWhiteSpace())
            {
                result.Success = false;
                result.Message = "config not correct";
                return result;
            }

            if (messages == null || !messages.Any())
            {
                return result;
            }

            foreach (var message in messages)
            {
                var mStr = $"{message.Title}\n{message.Desc}";
                var response = await Client.PostAsync(_webhook,
                    new StringContent($"{{\"text\":\"{mStr}\"}}", Encoding.UTF8, "application/json"));
                if (!response.IsSuccessStatusCode)
                {
                    result.Success = false;
                    result.Message = $"Status: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}";
                }
            }
            return result;
        }
    }
}
