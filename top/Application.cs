using System;
using System.Reflection;
using MyOS.Common;

namespace top
{
    public class Application : KernelProcess
    {
        public override string Name => "top";

        public override void Run(string[] args)
        {
            foreach (IProcess process in ProcessManager.List())
            {
                Console.WriteLine("{0} {1} {2}", process.ProcessId, process.ProcessType, process.Name);
            }

            Exit();
        }
    }
}
