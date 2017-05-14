using MyOS.Common;
using SharpFileSystem;

namespace MyOS.System
{
    public class MyLib : IMyLib
    {
        private readonly IKernel _kernel;

        public string CurrentDirectory { get; set; }
        public IStorage Storage { get; }

        public MyLib(IKernel kernel, IFileSystem fileSystem)
        {
            _kernel = kernel;
            Storage = new StorageService(fileSystem);
        }
    }
}