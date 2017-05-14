using System.IO;
using MyOS.Common;
using SharpFileSystem;
using SharpFileSystem.IO;

namespace MyOS.System
{
    public class StorageService : IStorage
    {
        private readonly IFileSystem _fileSystem;

        public StorageService(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public string ReadFileAsString(string path)
        {
            using (Stream bootStream = _fileSystem.OpenFile(FileSystemPath.Parse(path), FileAccess.Read))
            using (StreamReader streamReader = new StreamReader(bootStream))
            {
                return streamReader.ReadToEnd();
            }
        }

        public MemoryStream ReadFileAsStream(string path)
        {
            MemoryStream memoryStream;

            using (Stream stream = _fileSystem.OpenFile(FileSystemPath.Parse(path), FileAccess.Read))
            {
                memoryStream = new MemoryStream(stream.ReadAllBytes());
            }

            return memoryStream;
        }

        public void WriteFile(string path, string fileContent)
        {
            FileSystemPath fileSystemPath = FileSystemPath.Parse(path);

            if (_fileSystem.Exists(fileSystemPath))
            {
                using (Stream stream = _fileSystem.OpenFile(fileSystemPath, FileAccess.Write))
                using (StreamWriter streamWriter = new StreamWriter(stream))
                {
                    streamWriter.Write(fileContent);
                }
            }
            else
            {
                using (Stream stream = _fileSystem.CreateFile(fileSystemPath))
                using (StreamWriter streamWriter = new StreamWriter(stream))
                {
                    streamWriter.Write(fileContent);
                }
            }
        }

        public bool Exists(string path)
        {
            return _fileSystem.Exists(FileSystemPath.Parse(path));
        }

        public bool CreateDirectory(string path)
        {
            if (!path.EndsWith("/"))
            {
                path = path + "/";
            }

            _fileSystem.CreateDirectoryRecursive(FileSystemPath.Parse(path));

            return true;
        }
    }
}
