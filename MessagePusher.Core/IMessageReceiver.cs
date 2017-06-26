using System.Collections.Generic;
using System.Threading.Tasks;
using MessagePusher.Core.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace MessagePusher.Core
{
    public interface IMessageReceiver : IMessager
    {
        string Method { get; }

        List<string> SendTo { get; }

        Task Init(HttpRequest request, JToken config);

        bool Verify();

        List<Message> Receive();
    }
}
