using System;
using MyOS.Common;

namespace MyOS.Apps.Echo
{
    public class Application : UserProcess
    {
        public override string Name => "echo";

        public override void Run(string[] args)
        {
            if (args[0] == ">" && args.Length < 3)
            {
                if (!args[1].StartsWith("/"))
                {
                    args[1] = MyLib.CurrentDirectory + args[1];
                }

                string file = MyLib.Storage.ReadFileAsString(args[1]);
                Console.WriteLine(file);
                Exit();
                return;
            }

            if (args[0] == ">" && args[2] == ">")
            {
                if (!args[3].StartsWith("/"))
                {
                    args[3] = MyLib.CurrentDirectory + args[1];
                }

                MyLib.Storage.WriteFile(args[3], args[1]);
                Exit();
                return;
            }

            Console.WriteLine("Echo: {0}", string.Join(" ", args));
            Exit();
        }
    }
}
