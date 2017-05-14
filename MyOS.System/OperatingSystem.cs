using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using MyOS.ApplicationRuntime;
using MyOS.Common;
using SharpFileSystem;
using SharpFileSystem.IO;

namespace MyOS.System
{
    public class OperatingSystem : IMyOS
    {
        private IKernel _kernel;
        private IProcessManager _processManager;
        private IFileSystem _currentDrive;
        private IMyLib _myLib;

        public void Boot(IKernel kernel, IProcessManager processManager)
        {
            _kernel = kernel;
            _processManager = processManager;
            _currentDrive = kernel.Drives.First().Value;
            _myLib = new MyLib(_kernel, _currentDrive)
            {
                CurrentDirectory = "/"
            };

            Console.WriteLine("Checking for boot.ini");

            if (_currentDrive.Exists(FileSystemPath.Parse("/system/boot.ini")))
            {
                using (Stream bootStream = _currentDrive.OpenFile(FileSystemPath.Parse("/system/boot.ini"), FileAccess.Read))
                using (StreamReader streamReader = new StreamReader(bootStream))
                {
                    ExecuteCommand("cd system");

                    while (!streamReader.EndOfStream)
                    {
                        ExecuteCommand(streamReader.ReadLine());
                    }

                    ExecuteCommand("cd ..");
                }
            }
            else
            {
                Console.WriteLine("No boot.ini file found, moving on");
            }

            while (true)
            {
                Console.Write("#" + _myLib.CurrentDirectory + "> ");
                ExecuteCommand(Console.ReadLine());
            }
        }

        private void ExecuteCommand(string command)
        {
            string[] commands = command.Split(' ');

            switch (commands[0])
            {
                case "ls":
                    ListFiles();
                    break;

                case "clear":
                    Console.Clear();
                    break;

                case "touch":
                    CreateBlankFile(commands[1]);
                    break;

                case "cd":
                    if (commands[1] == "..")
                    {
                        var bits = _myLib.CurrentDirectory.Split('/');
                        _myLib.CurrentDirectory = "/";

                        if (bits.Length == 3)
                        {
                            _myLib.CurrentDirectory = "/";
                            return;
                        }

                        for (var i = 0; i < bits.Length - 2; i++)
                        {
                            _myLib.CurrentDirectory += bits[i];
                        }

                        _myLib.CurrentDirectory += "/";

                        return;
                    }

                    if (_currentDrive.Exists(FileSystemPath.Parse(_myLib.CurrentDirectory + commands[1] + "/")))
                    {
                        _myLib.CurrentDirectory = _myLib.CurrentDirectory + commands[1] + "/";
                        _myLib.CurrentDirectory = _myLib.CurrentDirectory;
                    }
                    else
                    {
                        Console.WriteLine("Cannot find {0}", commands[1]);
                    }
                    break;

                default:
                    if (commands[0].StartsWith("./"))
                    {
                        string path = commands[0].Substring(2);
                        RuntimeEngine myaProcess = new RuntimeEngine(path, _kernel, _myLib);
                        myaProcess.Initialize(_processManager, _myLib);
                        _processManager.Register(myaProcess);
                        myaProcess.Run(commands.Skip(1).ToArray());
                        break;
                    }

                    RunApplication(commands[0], commands.Skip(1).ToArray());
                    break;
            }
        }

        private void CreateBlankFile(string fileName)
        {
            using (Stream stream = _currentDrive.CreateFile(FileSystemPath.Parse(fileName)))
            {

            }
        }

        private void ListFiles()
        {
            ICollection<FileSystemPath> metaData = _currentDrive.GetEntities(FileSystemPath.Parse(_myLib.CurrentDirectory));

            foreach (FileSystemPath fileEntry in metaData)
            {
                if (fileEntry.IsDirectory)
                {
                    Console.WriteLine("   + {0}", fileEntry.EntityName);
                }
                else
                {
                    Console.WriteLine("     {0}", fileEntry.EntityName);
                }

                //Console.WriteLine("|{0,32}", fileEntry.EntityName, fileEntry.IsDirectory, fileEntry.IsFile);
            }
        }

        private void RunApplication(string appToOpen, string[] args)
        {
            byte[] application;
            string pathToOpen = _myLib.CurrentDirectory + appToOpen + ".mye";

            if (!_currentDrive.Exists(FileSystemPath.Parse(pathToOpen)))
            {
                if (_currentDrive.Exists(FileSystemPath.Parse("/system/" + appToOpen + ".mye")))
                {
                    pathToOpen = "/system/" + appToOpen + ".mye";
                }
                else
                {
                    Console.WriteLine("Application not found");
                    return;
                }
            }

            using (Stream applicationStream = _currentDrive.OpenFile(FileSystemPath.Parse(pathToOpen), FileAccess.Read))
            {
                application = applicationStream.ReadAllBytes();
            }

            try
            {
                Assembly assembly = Assembly.Load(application);
                IEnumerable<Type> osType = from t in assembly.GetTypes() where t.Name == "Application" select t;

                if (osType.Count() > 1)
                {
                    throw new Exception("Too many applications");
                }

                IProcess applicationItem = (IProcess)Activator.CreateInstance(osType.First());

                switch (applicationItem.ProcessType)
                {
                    case ProcessType.KernelApplication:
                        KernelProcess kernelProcess = (KernelProcess)applicationItem;
                        kernelProcess.Initialize(_kernel, _processManager, _myLib);
                        _processManager.Register(kernelProcess);
                        kernelProcess.Run(args);
                        break;

                    case ProcessType.UserApplication:
                        UserProcess userProcess = (UserProcess)applicationItem;
                        userProcess.Initialize(_processManager, _myLib);
                        _processManager.Register(userProcess);
                        userProcess.Run(args);
                        break;

                    case ProcessType.KernelService:
                        KernelService kernelService = (KernelService)applicationItem;
                        kernelService.Initialize(_kernel, _processManager, _myLib);
                        _processManager.Register(kernelService);
                        kernelService.Run(args);
                        break;

                    default:
                        Console.WriteLine("Don't know how to start {0}", applicationItem.ProcessType);
                        break;
                }
            }
            catch (ReflectionTypeLoadException error)
            {
                Console.WriteLine("Can't load {0}:", appToOpen);
                error.LoaderExceptions.ToList().ForEach(x => Console.WriteLine("   {0}", x.Message));
            }
            catch (Exception e)
            {
                Console.WriteLine("FATAL ERROR: {0}\n{1}", e.Message, e.StackTrace);
            }
        }
    }
}
