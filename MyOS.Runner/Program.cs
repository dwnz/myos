using System;
using System.Linq;
using MyOS.Common;

namespace MyOS
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!args.Any())
            {
                Environment.Exit(0);
            }

            Console.WriteLine("Starting {0}...", args[0]);

            IKernel kernel = new MyOSKernel(args[0]);
            kernel.Boot();

            Console.ReadKey();
        }
    }
}
