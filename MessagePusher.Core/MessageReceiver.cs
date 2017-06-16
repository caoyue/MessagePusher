using System;
using System.Collections.Generic;
using System.Text;

namespace MessagePusher.Core
{
    public abstract class MessageReceiver : MessageConfig
    {
        public virtual List<string> SendTo => Config["SendTo"].ToObject<List<string>>();
    }
}
