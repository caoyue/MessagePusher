using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MessagePusher.Core.Extensions;
using MessagePusher.Core.Models;
using Newtonsoft.Json.Linq;

namespace MessagePusher.Core.Sender
{
    public class ServerJiangSender : MessageConfig, IMessageSender
    {
        private static readonly HttpClient Client = new HttpClient();
        private string _token;

        public void Config(JToken config)
        {
            _token = config["Token"].ToString();
        }

        public async Task<Result> Send(List<Message> messages)
        {
            var result = new Result
            {
                Success = true,
                Message = "Success"
            };

            if (_token.IsNullOrWhiteSpace())
            {
                result.Success = false;
                result.Message = "config not correct";
                return result;
            }

            if (messages != null && messages.Any())
            {
                foreach (var message in messages)
                {
                    var response = await Client.GetAsync($"http://sc.ftqq.com/{_token}.send?text=" +
                                  $"{WebUtility.UrlEncode(message.Title)}&desp={WebUtility.UrlEncode(message.Desc)}");

                    if (!response.IsSuccessStatusCode)
                    {
                        result.Success = false;
                        result.Message = $"Status: {response.StatusCode}, {await response.Content.ReadAsStringAsync()}";
                    }
                }
            }
            return result;
        }
    }
}
