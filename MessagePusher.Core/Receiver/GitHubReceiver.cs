using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MessagePusher.Core.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace MessagePusher.Core.Receiver
{
    public class GitHubReceiver : MessageReceiver, IMessageReceiver
    {
        private const string Sha1Prefix = "sha1=";

        private string _payload;
        private string _eventName;
        private string _signature;
        private JObject _json;

        public string Name => "GitHub";

        public string Method => "Post";

        public async Task Init(HttpRequest request)
        {
            _payload = await request.ReadBodyAsync();
            _eventName = request.Headers["X-GitHub-Event"];
            _signature = request.Headers["X-Hub-Signature"];

            _json = _payload.ToJson();
        }

        public bool Verify()
        {
            if (_signature.StartsWith(Sha1Prefix, StringComparison.OrdinalIgnoreCase))
            {
                var signature = _signature.Substring(Sha1Prefix.Length);
                var secret = Encoding.ASCII.GetBytes(Config["Token"].ToString());
                var payloadBytes = Encoding.ASCII.GetBytes(_payload);

                using (var hmSha1 = new HMACSHA1(secret))
                {
                    var hash = hmSha1.ComputeHash(payloadBytes);

                    var hashString = ToHexString(hash);

                    if (hashString.Equals(signature))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public List<Message> Receive()
        {
            var list = new List<Message>();
            if (_eventName.Equals("ping", StringComparison.OrdinalIgnoreCase))
            {
                list.Add(new Message
                {
                    Title = $"{_json["sender"]["login"]} pinged {_json["repository"]["name"]}",
                    Desc = "success"
                });
                return list;
            }
            if (_eventName.Equals("push", StringComparison.OrdinalIgnoreCase))
            {
                var forced = _json["forced"].ToObject<bool>() ? "forced" : "";
                list.Add(new Message
                {
                    Title = $"{_json["head_commit"]["committer"]["name"]} {forced} pushed a commit to {_json["repository"]["name"]}",
                    Desc = $"{_json["head_commit"]["message"]}, {_json["head_commit"]["url"]}"
                });
                return list;
            }
            throw new NotImplementedException("Only push event implemented now.");
        }

        private static string ToHexString(byte[] bytes)
        {
            var builder = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes)
            {
                builder.AppendFormat("{0:x2}", b);
            }
            return builder.ToString();
        }
    }
}
