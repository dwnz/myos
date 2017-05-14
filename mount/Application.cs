using System;
using MyOS.Common;

namespace mount
{
    public class Application : KernelProcess
    {
        public override string Name => "mount";

        public override void Run(string[] args)
        {
            if (args[0] == "help")
            {
                Console.WriteLine("mount help");
                Console.WriteLine("   path - mounts a disk from the path");
                Console.WriteLine("   -u name - unmounts a disk with name");
                return;
            }

            if (args[0] == "-u")
            {
                Kernel.UnmountDisk(args[1]);
                Console.WriteLine("Unmounted {0}", args[1]);
            }
            else
            {
                Kernel.MountDisk("", args[0]);
            }

            Exit();
        }
    }
}