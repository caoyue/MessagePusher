using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MessagePusher.Core.Models;

namespace MessagePusher.Core.Sender
{
    public class ServerJiangSender : MessageConfig, IMessageSender
    {
        public string Name => "ServerJiang";

        public async Task<Result> Send(List<Message> messages)
        {
            var token = Config["Token"].ToString();
            var result = new Result
            {
                Success = true,
                Message = "Success"
            };

            if (messages != null && messages.Any())
            {
                foreach (var message in messages)
                {
                    var response = await new HttpClient()
                        .GetAsync($"http://sc.ftqq.com/{token}.send?text=" +
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