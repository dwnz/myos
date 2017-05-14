using System.Collections.Generic;
using SharpFileSystem;

namespace MyOS.Common
{
    public interface IKernel
    {
        Dictionary<string, IFileSystem> Drives { get; set; }
        void MountDisk(string path, string mountPath);
        void Boot();
        void UnmountDisk(string mountPath);
        IFileSystem Drive(string mountPath);
        void MountDisk(IFileSystem package, string mountPath);
    }
}