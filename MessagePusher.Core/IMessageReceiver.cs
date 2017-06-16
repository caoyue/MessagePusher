using System.Collections.Generic;
using System.Threading.Tasks;
using MessagePusher.Core.Models;
using Microsoft.AspNetCore.Http;

namespace MessagePusher.Core
{
    public interface IMessageReceiver : IMessager
    {
        string Method { get; }

        List<string> SendTo { get; }

        Task Init(HttpRequest request);

        bool Verify();

        Message Receive();
    }
}