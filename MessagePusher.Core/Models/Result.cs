namespace MessagePusher.Core.Models
{
    public class Result
    {
        public bool Success { get; set; }

        public string Message { get; set; }
    }

    public class Result<T> : Result where T : class
    {
        public T Data { get; set; }
    }
}
