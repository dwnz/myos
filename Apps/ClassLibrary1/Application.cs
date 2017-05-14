using System;
using MyOS.Common;

namespace MyOS.Apps.Clock
{
    public class Application : UserProcess
    {
        public override string Name => "clock";

        public override void Run(string[] args)
        {
            Console.WriteLine("The time is {0}", DateTime.Now);
            Exit();
        }
    }
}
