using System.Collections.Generic;
using System.Threading.Tasks;
using MessagePusher.Core.Models;
using Newtonsoft.Json.Linq;

namespace MessagePusher.Core
{
    public interface IMessageSender : IMessager
    {
        void Config(JToken config);

        Task<Result> Send(List<Message> message);
    }
}
