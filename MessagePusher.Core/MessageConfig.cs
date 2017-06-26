namespace MessagePusher.Core
{
    public abstract class MessageConfig
    {
        public virtual string Name => GetType().Name.Replace("Receiver", "").Replace("Sender", "");
    }
}
