using System;
using System.Collections.Generic;
using MyOS.Common;
using SharpFileSystem;

namespace fdisk
{
    public class Application : KernelProcess
    {
        public override string Name => "fdisk";

        public override void Run(string[] args)
        {
            bool run = true;

            while (run)
            {
                Console.Write("fdisk #> ");
                string[] command = Console.ReadLine().Split(' ');

                switch (command[0])
                {
                    case "help":
                        Console.WriteLine("Commands:");
                        Console.WriteLine("   list - lists all active disks");
                        Console.WriteLine("   new size name - creates a new drive");
                        Console.WriteLine("   exit - exists application");
                        break;

                    case "list":
                        foreach (KeyValuePair<string, IFileSystem> mount in Kernel.Drives)
                        {
                            Console.WriteLine("{0} {1}", mount.Key, "");
                        }
                        break;

                    //case "new":
                    //    IDiskDrive newDisk = new MyFS("../../../" + command[2] + ".dat", long.Parse(command[1]), command[2]);
                    //    newDisk.Flush();
                    //    Console.WriteLine("Created disk {0}", command[2]);
                    //    break;

                    case "exit":
                        run = false;
                        Exit();
                        break;
                }
            }

        }
    }
}
