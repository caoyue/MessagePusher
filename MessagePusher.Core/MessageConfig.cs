using MessagePusher.Core.Models;
using Newtonsoft.Json.Linq;

namespace MessagePusher.Core
{
    public abstract class MessageConfig
    {
        public virtual JToken Config
        {
            get {
                var name = GetType().Name.Replace("Receiver", "").Replace("Sender", "");
                return ConfigLoader.Config[name];
            }
        }
    }
}
