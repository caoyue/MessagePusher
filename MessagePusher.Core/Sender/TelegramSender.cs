using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MessagePusher.Core.Models;

namespace MessagePusher.Core.Sender
{
    public class TelegramSender : MessageConfig, IMessageSender
    {
        public string Name => "Telegram";

        public async Task<Result> Send(List<Message> messages)
        {
            var token = Config["Token"].ToString();
            var chatId = Config["ChatId"].ToString();
            var result = new Result
            {
                Success = true,
                Message = "Success"
            };

            if (messages != null && messages.Any())
            {
                foreach (var message in messages)
                {
                    var mStr = WebUtility.HtmlEncode($"{ message.Title}: { message.Desc}");
                    var response = await new HttpClient()
                        .GetAsync($"https://api.telegram.org/bot{token}/sendMessage?" +
                                  $"chat_id={chatId}&text={mStr}");
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
