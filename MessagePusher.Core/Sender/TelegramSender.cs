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
        private static readonly HttpClient Client = new HttpClient();

        public async Task<Result> Send(List<Message> messages)
        {
            var token = Config["Token"].ToString();
            var chatId = Config["ChatId"].ToString();
            var result = new Result
            {
                Success = true,
                Message = "Success"
            };

            if (messages == null || !messages.Any())
            {
                return result;
            }

            foreach (var message in messages)
            {
                var desc = string.IsNullOrWhiteSpace(message.Desc) ? "" : $": {message.Desc}";
                var mStr = WebUtility.HtmlEncode($"{ message.Title}{desc}");
                var response = await Client.GetAsync($"https://api.telegram.org/bot{token}/sendMessage?" +
                              $"chat_id={chatId}&text={mStr}");
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
