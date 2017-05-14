using System.Collections.Generic;

namespace MyOS.Common
{
    public interface IDiskDrive
    {
        long Size { get; set; }
        long Free { get; }
        string Name { get; set; }

        void Flush();
        byte[] Fetch(string fileName);
        void Push(string name, byte[] file);
        List<IFileEntry> Meta();
    }
}