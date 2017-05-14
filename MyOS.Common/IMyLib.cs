using System.IO;

namespace MyOS.Common
{
    public interface IMyLib
    {
        string CurrentDirectory { get; set; }
        IStorage Storage { get; }
    }

    public interface IStorage
    {
        string ReadFileAsString(string path);
        void WriteFile(string path, string fileContent);
        bool Exists(string path);
        bool CreateDirectory(string path);
        MemoryStream ReadFileAsStream(string path);
    }
}
