using System;

namespace MyOS.Common
{
    public abstract class Process : IProcess
    {
        protected IMyLib MyLib;

        protected Process()
        {
            ProcessId = Guid.NewGuid();
        }

        public Guid ProcessId { get; }

        public abstract string Name { get; }
        public abstract ProcessType ProcessType { get; }
        public abstract void End();
        public abstract void Run(string[] args);
        public abstract void Exit(int code = 0);

        public void PublishEvent<T>(string key, T message)
            where T : EventMessageBase
        {
            EventSubscription.Publish(key, message);
        }

        public void SubscribeToEvent<T>(string key, Action<T> action)
            where T : EventMessageBase
        {
            EventSubscription.Subscribe(key, action);
        }
    }
}

