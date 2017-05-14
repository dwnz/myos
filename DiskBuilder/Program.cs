using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Timers;
using SharpFileSystem;
using SharpFileSystem.FileSystems;
using Directory = System.IO.Directory;
using File = System.IO.File;
using Process = System.Diagnostics.Process;

namespace DiskBuilder
{
    class Program
    {
        private static Timer _timer;
        private static int ticks;
        private static Process process;

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                args = new[] { "none" };
            }

            Console.WriteLine("Mode: {0}", args[0]);

            FileSystemWatcher watcher = new FileSystemWatcher("../../../Core");
            watcher.Changed += WatcherOnChanged;
            watcher.EnableRaisingEvents = true;

            Bundle(false);

            if (args[0] == "watch")
            {
                process = new Process
                {
                    StartInfo =
                    {
                        FileName = @"C:\Users\danie\Documents\visual studio 2017\Projects\MyOS\MyOS.Runner\bin\Debug\MyOS.exe",
                        Arguments = "../../../dev",
                        WindowStyle = ProcessWindowStyle.Normal
                    }
                };
               // process.Start();
            }

            Console.ReadKey();
        }

        private static void WatcherOnChanged(object sender, FileSystemEventArgs fileSystemEventArgs)
        {
            if (_timer == null)
            {
                _timer = new Timer(1000);
                _timer.Elapsed += (o, args) =>
                {
                    if (ticks < 3)
                    {
                        ticks++;
                    }
                    else
                    {
                        ticks = 0;
                        _timer.Stop();
                        _timer = null;
                        Bundle();
                    }
                };
                _timer.Start();
            }
        }

        static void Bundle(bool run = true)
        {
            List<string> files = Directory.GetFiles("../../../Core", "*.dll").ToList();
            files.AddRange(Directory.GetFiles("./", "*.txt"));


            PhysicalFileSystem fileSystem = new PhysicalFileSystem("../../../dev/");

            fileSystem.CreateDirectory(FileSystemPath.Parse("/system/"));

            foreach (string file in files)
            {
                if (!file.Contains("MyOS."))
                {
                    using (Stream fileStream = fileSystem.CreateFile(FileSystemPath.Parse("/system/" + new FileInfo(file).Name.Replace(".dll", ".mye").Replace(".txt", ".ini"))))
                    {
                        byte[] fileBytes = File.ReadAllBytes(file);
                        fileStream.Write(fileBytes, 0, fileBytes.Length);
                    }

                    Console.WriteLine("Written {0}", new FileInfo(file).Name.Replace(".dll", ".mye").Replace(".txt", ".ini"));
                }
            }

            Console.WriteLine("Done");

            if (run)
            {
                //process.Kill();
                //process.Start();
            }
        }
    }
}
