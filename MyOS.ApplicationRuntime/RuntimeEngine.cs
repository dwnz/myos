using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using MyOS.Common;
using SharpFileSystem;
using SharpFileSystem.SharpZipLib;

namespace MyOS.ApplicationRuntime
{
    public class RuntimeEngine : UserProcess
    {
        private readonly string _path;
        private readonly IKernel _kernel;
        private readonly IMyLib _myLib;
        private readonly IFileSystem _package;
        private readonly IStorage _storage;

        public override string Name { get; }

        public RuntimeEngine(string path, IKernel kernel, IMyLib myLib)
        {
            _path = path;
            _kernel = kernel;
            _myLib = myLib;

            _package = SharpZipLibFileSystem.Open(_myLib.Storage.ReadFileAsStream(Path.Combine(_myLib.CurrentDirectory, path)));
            kernel.MountDisk(_package, "/mnt/" + path);

            _storage = new StorageService();

            Name = "Test";
        }

        public override void Run(string[] args)
        {
            Console.WriteLine(_myLib.CurrentDirectory);
        }

        public override void End()
        {
            base.End();
        }
    }

    public class ApplicationManifest
    {
        public string Name { get; set; }
    }
}
