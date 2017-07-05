using System;
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
    public class TelegramSender : MessageConfig, IMessageSender
    {
        private static readonly HttpClient Client = new HttpClient();
        private string _token;
        private string _chatId;

        public void Config(JToken config)
        {
            _token = config["Token"].ToString();
            _chatId = config["ChatId"].ToString();
        }

        public async Task<Result> Send(List<Message> messages)
        {
            var result = new Result
            {
                Success = true,
                Message = "Success"
            };

            if (_token.IsNullOrWhiteSpace() || _chatId.IsNullOrWhiteSpace())
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
                var mStr = WebUtility.UrlEncode($"{ message.Title}\n{message.Desc} #{message.From}#");
                var response = await Client.GetAsync($"https://api.telegram.org/bot{_token}/sendMessage?" +
                              $"chat_id={_chatId}&text={mStr}");
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
