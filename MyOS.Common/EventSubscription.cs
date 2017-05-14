using System;
using System.Collections.Generic;

namespace MyOS.Common
{
    public static class EventSubscription
    {
        public static Queue<EventMessageBase> Queue = new Queue<EventMessageBase>();
        public static Dictionary<string, List<Action<EventMessageBase>>> Subscriptions = new Dictionary<string, List<Action<EventMessageBase>>>();

        public static void Publish<T>(string key, T message)
            where T : EventMessageBase
        {
            message.Key = key;
            Queue.Enqueue(message);
        }

        public static void Subscribe<T>(string key, Action<T> action)
            where T : EventMessageBase
        {
            if (!Subscriptions.ContainsKey(key))
            {
                Subscriptions.Add(key, new List<Action<EventMessageBase>>());
            }

            Subscriptions[key].Add((Action<EventMessageBase>)action);
        }
    }

    public class EventMessageBase
    {
        public string Key { get; set; }
    }
}
