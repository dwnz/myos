using System;
using MyOS.Common;

namespace eventflow
{
    public class Application : KernelService
    {
        public override string Name => "eventflow";

        public override void Loop(string[] args)
        {
            SubscribeToEvent<EventMessageBase>("mkdir.created", message =>
            {
                Console.WriteLine(message.Key);
            });
        }
    }
}