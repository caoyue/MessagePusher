using System.Collections.Generic;
using MessagePusher.Core.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace MessagePusher.Core
{
    public interface IMessageReceiver : IMessager
    {
        string Method { get; }

        List<string> SendTo { get; }

        bool Verify(IQueryCollection query, IHeaderDictionary headers, JObject json);

        Result<Message> Receive(IQueryCollection query, IHeaderDictionary headers, JObject json);
    }
}