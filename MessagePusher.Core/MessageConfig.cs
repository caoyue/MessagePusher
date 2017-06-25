using MessagePusher.Core.Models;
using Newtonsoft.Json.Linq;

namespace MessagePusher.Core
{
    public abstract class MessageConfig
    {
        public virtual JToken Config => ConfigLoader.Config[Name];

        public virtual string Name => GetType().Name.Replace("Receiver", "").Replace("Sender", "");
    }
}
