//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using MyOS.Common;

//namespace MyOS.BasicFileSystem
//{
//    /// <summary>
//    /// Header - 128 bytes
//    ///     Size - 64 bytes
//    ///     Name - 64 bytes
//    /// File Index - 129 - 2048 bytes
//    ///     Entry 96 bytes
//    ///         Name 32 bytes
//    ///         Position 32 bytes
//    ///         Length 32 bytes
//    /// File data > 2048
//    /// </summary>
//    public class MyFS : IDiskDrive
//    {
//        private readonly string _path;
//        private readonly byte[] _drive;

//        public long Size { get; set; }
//        public long Free { get; set; }
//        public string Name { get; set; }

//        public void Flush()
//        {
//            File.WriteAllBytes(_path, _drive);
//        }

//        public MyFS(string path)
//        {
//            _path = path;
//            _drive = File.ReadAllBytes(path);

//            byte[] rawHeader = new byte[128];

//            for (var i = 0; i < 128; i++)
//            {
//                rawHeader[i] = _drive[i];
//            }

//            string header = Encoding.UTF8.GetString(rawHeader);

//            if (string.IsNullOrEmpty(header))
//            {
//                throw new Exception("Disk invalid");
//            }

//            Size = long.Parse(header.Substring(0, 64));
//            Name = header.Substring(64).Trim();

//            Console.WriteLine("Disk '{0}' ready to go", Name);
//        }

//        public MyFS(string path, long size, string name)
//        {
//            _path = path;

//            if (size < 2)
//            {
//                throw new Exception("Drive too small");
//            }

//            _drive = new byte[(size * 1024) * 1024];

//            string header = string.Format("{0}{1}", size.ToString().PadLeft(64, '0'), name.PadRight(64, ' '));
//            byte[] headerBytes = Encoding.UTF8.GetBytes(header);

//            for (var i = 0; i < headerBytes.Length; i++)
//            {
//                _drive[i] = headerBytes[i];
//            }
//        }

//        public byte[] Fetch(string fileName)
//        {
//            var fileEntries = ExtractEntries();

//            foreach (byte[] fileEntry in fileEntries)
//            {
//                IFileEntry entryObject = DecodeFileEntry(fileEntry);

//                if (fileName == entryObject.Name)
//                {
//                    byte[] buffer = new byte[entryObject.Length];
//                    int bufferCursor = 0;

//                    for (var i = entryObject.Position; i < (entryObject.Position + entryObject.Length); i++)
//                    {
//                        buffer[bufferCursor] = _drive[i];
//                        bufferCursor++;
//                    }

//                    return buffer;
//                }
//            }

//            return null;
//        }

//        private List<byte[]> ExtractEntries()
//        {
//            List<byte[]> fileEntries = new List<byte[]>();

//            byte[] buffer = new byte[96];
//            int cursorIndex = 0;

//            for (var i = 129; i < 1920; i++)
//            {
//                buffer[cursorIndex] = _drive[i];

//                if (cursorIndex + 1 == 96)
//                {
//                    if (!buffer.All(x => x == 0))
//                    {
//                        fileEntries.Add(buffer);
//                    }

//                    buffer = new byte[96];
//                    cursorIndex = 0;
//                }
//                else
//                {
//                    cursorIndex++;
//                }
//            }

//            return fileEntries;
//        }

//        public void Push(string name, byte[] file)
//        {
//            List<byte[]> fileEntries = ExtractEntries();
//            int toWriteInto = fileEntries.Count;

//            int startByte = 129 + (toWriteInto * 96);
//            if (startByte > 1920)
//            {
//                throw new Exception("Out of space");
//            }

//            long startWriteAt = 2048;
//            if (toWriteInto > 0)
//            {
//                var lastFileHeader = DecodeFileEntry(fileEntries.Last());
//                startWriteAt = lastFileHeader.Position + lastFileHeader.Length;
//            }

//            FileEntry entry = new FileEntry
//            {
//                Name = name,
//                Length = file.Length,
//                Position = startWriteAt
//            };

//            byte[] headerRow = EncodeFileEntry(entry);

//            for (var i = 0; i < 96; i++)
//            {
//                _drive[i + startByte] = headerRow[i];
//            }

//            for (var i = 0; i < entry.Length; i++)
//            {
//                _drive[i + startWriteAt] = file[i];
//            }
//        }

//        public List<IFileEntry> Meta()
//        {
//            List<byte[]> files = ExtractEntries();
//            return files.Select(DecodeFileEntry).ToList();
//        }

//        private IFileEntry DecodeFileEntry(byte[] entryBytes)
//        {
//            string header = Encoding.UTF8.GetString(entryBytes);
//            return new FileEntry
//            {
//                Name = header.Substring(0, 32).Trim(),
//                Position = long.Parse(header.Substring(32, 32)),
//                Length = long.Parse(header.Substring(64, 32))
//            };
//        }

//        private byte[] EncodeFileEntry(IFileEntry fileEntry)
//        {
//            string header = string.Format("{0}{1}{2}", fileEntry.Name.PadRight(32), fileEntry.Position.ToString().PadLeft(32, '0'), fileEntry.Length.ToString().PadLeft(32, '0'));
//            return Encoding.UTF8.GetBytes(header);
//        }
//    }
//}