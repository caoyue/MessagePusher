using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MessagePusher.Core.Models
{
    public static class RequestExtensions
    {
        public static async Task<string> ReadBodyAsync(this HttpRequest request)
        {
            using (var r = new StreamReader(request.Body))
            {
                return await r.ReadToEndAsync();
            }
        }
    }
}
