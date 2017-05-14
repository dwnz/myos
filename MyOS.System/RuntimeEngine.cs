using System;
using System.IO;
using Jurassic;
using MyOS.Common;
using MyOS.System;
using MyOS.System.RuntimeModules;
using Newtonsoft.Json;
using SharpFileSystem;
using SharpFileSystem.SharpZipLib;

namespace MyOS.ApplicationRuntime
{
    public class RuntimeEngine : UserProcess
    {
        private readonly string _path;
        private readonly IKernel _kernel;
        private readonly IFileSystem _package;
        private readonly IStorage _storage;
        private readonly ApplicationManifest _manifest;

        public override string Name { get; }

        public RuntimeEngine(string path, IKernel kernel, IMyLib myLib)
        {
            _path = path;
            _kernel = kernel;

            _package = SharpZipLibFileSystem.Open(myLib.Storage.ReadFileAsStream(Path.Combine(myLib.CurrentDirectory, path)));
            kernel.MountDisk(_package, "/mnt/" + path);

            _storage = new StorageService(kernel.Drive("/mnt/" + path));

            string data = _storage.ReadFileAsString("/app.json");
            _manifest = JsonConvert.DeserializeObject<ApplicationManifest>(data);
            Name = "app." + _manifest.Name;
        }

        public override void Run(string[] args)
        {
            ScriptEngine engine = new ScriptEngine();
            engine.SetGlobalValue("console", new ConsoleObject(engine));
            engine.Execute(_storage.ReadFileAsString("/index.js"));

            Exit();
        }

        public override void End()
        {
            _kernel.UnmountDisk("/mnt/" + _path);
            _package.Dispose();
            base.End();
        }
    }

    public class ApplicationManifest
    {
        public string Name { get; set; }
    }
}
