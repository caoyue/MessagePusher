using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MessagePusher.Core.Models;

namespace MessagePusher.Core.MessageSender
{
    public class ServerJiangSender : MessageConfig, IMessageSender
    {
        public string Name => "ServerJiang";

        public async Task<Result> Send(Message item)
        {
            var token = Config["Token"].ToString();
            var response = await new HttpClient()
                .GetAsync($"http://sc.ftqq.com/{token}.send?text=" +
                          $"{WebUtility.UrlEncode(item.Title)}&desp={WebUtility.UrlEncode(item.Desc)}");
            var result = new Result
            {
                Success = response.IsSuccessStatusCode,
                Message = response.IsSuccessStatusCode
                    ? "OK"
                    : $"Error: Http {response.StatusCode}"
            };
            return result;
        }
    }
}