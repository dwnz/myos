using System;
using MyOS.Common;

namespace mkdir
{
    public class Application : UserProcess
    {
        public override string Name => "mkdir";

        public override void Run(string[] args)
        {
            if (MyLib.Storage.Exists(MyLib.CurrentDirectory + args[0] + "/"))
            {
                Console.WriteLine("File/Directory already exists");
                Exit();
                return;
            }

            MyLib.Storage.CreateDirectory(MyLib.CurrentDirectory + args[0]);
            PublishEvent("mkdir.created", new EventMessageBase());

            Exit();
        }
    }
}
