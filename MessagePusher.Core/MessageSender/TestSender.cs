using System.Threading.Tasks;
using MessagePusher.Core.Models;

namespace MessagePusher.Core.MessageSender
{
    public class TestSender : MessageConfig, IMessageSender
    {
        public string Name => "Test";

        public Task<Result> Send(Message message)
        {
            var config = Config["test_config"];
            return Task.FromResult<Result>(new Result
            {
                Success = false,
                Message = $"This is a test! config: {config}"
            });
        }
    }
}
