using System;
using MyOS.Common;
using Timer = System.Timers.Timer;

namespace ping
{
    public class Application : KernelService
    {
        private Timer _timer;
        private DateTime _lastPing;

        public override string Name => "ping";

        public override void End()
        {
            _timer.Stop();
            base.End();
        }

        public override void Loop(string[] args)
        {
            _timer = new Timer();
            _timer.Elapsed += (sender, eventArgs) =>
            {
                _lastPing = DateTime.Now;
            };
            _timer.Start();
        }
    }
}
