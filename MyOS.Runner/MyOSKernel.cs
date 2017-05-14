using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using MyOS.Common;
using SharpFileSystem;
using SharpFileSystem.FileSystems;
using SharpFileSystem.IO;

namespace MyOS
{
    public class MyOSKernel : IKernel
    {
        public Dictionary<string, IFileSystem> Drives { get; set; }

        public MyOSKernel(string diskName)
        {
            Drives = new Dictionary<string, IFileSystem>
            {
                { "/",new PhysicalFileSystem(diskName)}
            };
        }

        public void MountDisk(string path, string mountPath)
        {
            IFileSystem drive = new PhysicalFileSystem(path);
            Drives.Add(mountPath, drive);
        }

        public void Boot()
        {
            byte[] bootFile;

            using (Stream systemStream = Drives.First().Value.OpenFile(FileSystemPath.Parse("/system/system.mye"), FileAccess.Read))
            {
                bootFile = systemStream.ReadAllBytes();
            }

            if (bootFile == null)
            {
                throw new Exception("No operating system found");
            }

            Assembly assembly = Assembly.Load(bootFile);

            IEnumerable<Type> osType = from t in assembly.GetTypes() where t.Name == "OperatingSystem" select t;

            if (osType.Count() > 1)
            {
                throw new Exception("Too many OSes");
            }

            IMyOS osItem = (IMyOS)Activator.CreateInstance(osType.First());
            IProcessManager processManager = new ProcessManager();
            osItem.Boot(this, processManager);
        }

        public void UnmountDisk(string mountPath)
        {
            var fileSystem = Drives.FirstOrDefault(x => x.Key == mountPath).Value;
            fileSystem.Dispose();
            Drives.Remove(mountPath);
        }

        public IFileSystem Drive(string mountPath)
        {
            return Drives.FirstOrDefault(x => x.Key == mountPath).Value;
        }

        public void MountDisk(IFileSystem package, string mountPath)
        {
            Drives.Add(mountPath, package);
        }
    }
}