using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MessagePusher.Core.Models;

namespace MessagePusher.Core.Sender
{
    public class TelegramSender : MessageConfig, IMessageSender
    {
        public string Name => "Telegram";

        public async Task<Result> Send(Message message)
        {
            var token = Config["Token"].ToString();
            var chatId = Config["ChatId"].ToString();
            var mStr = WebUtility.HtmlEncode($"{ message.Title}: { message.Desc}");
            var response = await new HttpClient()
                .GetAsync($"https://api.telegram.org/bot{token}/sendMessage?" +
                    $"chat_id={chatId}&text={mStr}");
            var result = new Result
            {
                Success = response.IsSuccessStatusCode,
                Message = response.IsSuccessStatusCode
                    ? "OK"
                    : $"Status: {response.StatusCode}, { await response.Content.ReadAsStringAsync()}"
            };
            return result;
        }
    }
}
