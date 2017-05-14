using System;
using System.Linq;
using System.Text.RegularExpressions;
using MyOS.Common;

namespace copy
{
    public class Application : KernelProcess
    {
        public override string Name => "copy";

        public override void Run(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Usage: copy FROM TO");
                Exit();
                return;
            }

            string fromPath = args[0];
            string toPath = args[1];

            Regex regex = new Regex(@"\$(.*)\/(.*)");
            Match fromMatches = regex.Match(fromPath);

            foreach (Group matchesGroup in fromMatches.Groups)
            {
                Console.WriteLine("Match {0}", matchesGroup.Value);
            }

            string driveName = "system";
            if (!string.IsNullOrEmpty(fromMatches.Groups[1].Value))
            {
                driveName = fromMatches.Groups[1].Value;
            }

            string fileName = fromMatches.Groups[2].Value;

            //IDiskDrive fromDrive = Kernel.Drives.First(x => x.Name.ToLower() == driveName);

            //var file = fromDrive.Fetch(fileName);
            //Console.WriteLine("File is {0} size", file.Length);

            //Match toMatches = regex.Match(toPath);
            //driveName = toMatches.Groups[1].Value;
            //fileName = toMatches.Groups[2].Value;

            //IDiskDrive toDrive = Kernel.Drives.First(x => x.Name.ToLower() == driveName);
            //toDrive.Push(fileName, file);
            //toDrive.Flush();

            Exit();
        }
    }
}
