using System.Collections.Generic;
using System.Threading.Tasks;
using MessagePusher.Core.Models;

namespace MessagePusher.Core
{
    public interface IMessageSender : IMessager
    {
        Task<Result> Send(List<Message> message);
    }
}
