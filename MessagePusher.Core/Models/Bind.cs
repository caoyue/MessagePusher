﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MessagePusher.Core.Models
{
    public static class Bind
    {
        public static IMessageReceiver GetReceiver(string resource, string method,
            IEnumerable<IMessageReceiver> receivers)
        {
            return receivers.FirstOrDefault(t => t.Name.Equals(resource, StringComparison.OrdinalIgnoreCase)
                && t.Method.Equals(method, StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<IMessageSender> GetSenders(List<string> sendTo,
            IEnumerable<IMessageSender> senders)
        {
            var result = new List<IMessageSender>();
            foreach (var st in sendTo)
            {
                var temp = senders.FirstOrDefault(t => t.Name.Equals(st, StringComparison.OrdinalIgnoreCase));
                if (temp != null)
                {
                    result.Add(temp);
                }
            }
            return result;
        }

        public static IEnumerable<IMessageReceiver> GetAllReceivers()
        {
            return GetAllImps<IMessageReceiver>();
        }

        public static IEnumerable<IMessageSender> GetAllSenders()
        {
            return GetAllImps<IMessageSender>();
        }

        private static IEnumerable<T> GetAllImps<T>() where T : class
        {
            return typeof(T).GetTypeInfo()
                .Assembly.GetTypes()
                .Where(t => typeof(T).IsAssignableFrom(t) && t.GetTypeInfo().IsClass)
                .Select(i => Activator.CreateInstance(i) as T);
        }
    }
}
