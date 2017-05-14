using System;
using System.Linq;
using System.Threading;
using MyOS.Common;

namespace events
{
    public class Application : KernelService
    {
        public override string Name => "events";

        public override void Loop(string[] args)
        {
            if (EventSubscription.Queue.Any())
            {
                EventMessageBase item = EventSubscription.Queue.Dequeue();

                foreach (Action<EventMessageBase> action in EventSubscription.Subscriptions[item.Key])
                {
                    action(item);
                }
            }
            else
            {
                Thread.Sleep(500);
            }
        }
    }
}