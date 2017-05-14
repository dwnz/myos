using System;
using System.Collections.Generic;
using System.IO;
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

            Console.ReadKey();
        }
    }

    public class MyOSKernel : IKernel
    {
        public List<IDiskDrive> Drives { get; set; }

        public MyOSKernel(string diskName)
        {
            Drives = new List<IDiskDrive>();

            if (!File.Exists(diskName + ".dat"))
            {
                Console.WriteLine("Drive doesn't exist...");

                Console.Write("Max size: ");
                long maxSize = long.Parse(Console.ReadLine());

                Console.Write("Name: ");
                string name = Console.ReadLine();

                Console.WriteLine("Creating disk");

                IDiskDrive drive = new MyFS(maxSize, name);
                drive.Flush();
            }

            Drives.Add(new MyFS(diskName + ".dat"));
        }
    }

    public class MyFS : IDiskDrive
    {
        private readonly string _path;
        private readonly byte[] _drive;

        public long Size { get; set; }
        public long Free { get; set; }
        public string Name { get; set; }

        public void Flush()
        {
            File.WriteAllBytes(_path, _drive);
        }

        public MyFS(string path)
        {
            _path = path;
            _drive = File.ReadAllBytes(path);
        }

        public MyFS(long size, string name)
        {
            if (size < 2048)
            {
                throw new Exception("Drive too small");
            }

            _drive = new byte[size];

            string header = string.Format("{0:00000000}{1}", size, name.PadRight(64));

            Console.WriteLine(header);

        }
    }
}