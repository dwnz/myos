using System;
using System.IO;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace Packager
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Application path:");
            var applicationPath = args.Length == 0 ? Console.ReadLine() : args[0];

            if (args.Length == 1)
            {
                Console.WriteLine(applicationPath);
            }

            if (!applicationPath.EndsWith("/"))
            {
                applicationPath = applicationPath + "/";
            }

            DirectoryInfo directoryInfo = new DirectoryInfo(applicationPath);

            using (FileStream fileStream = new FileStream(Path.Combine(applicationPath, "../", directoryInfo.Name + ".mya"), FileMode.Create))
            using (ZipOutputStream zipFile = new ZipOutputStream(fileStream))
            {
                RecursiveAdd(applicationPath, applicationPath, zipFile);
                zipFile.Finish();
            }

            File.Copy(Path.Combine(applicationPath, "../", directoryInfo.Name + ".mya"), Path.Combine(applicationPath, "../../dev/apps", directoryInfo.Name + ".mya"), true);

            Console.WriteLine("Done. Press any key to exit");
            Console.ReadKey();
        }

        private static void RecursiveAdd(string currentPath, string baseDirectory, ZipOutputStream zipFile)
        {
            foreach (var directory in Directory.GetDirectories(currentPath))
            {
                DirectoryInfo info = new DirectoryInfo(directory);
                RecursiveAdd(Path.Combine(currentPath, info.Name), baseDirectory, zipFile);
            }

            foreach (string file in Directory.GetFiles(currentPath))
            {
                FileInfo fileInfo = new FileInfo(file);
                ZipEntry zipEntry = new ZipEntry(fileInfo.Name);
                zipFile.PutNextEntry(zipEntry);

                byte[] buffer = new byte[4096];
                using (FileStream streamReader = File.OpenRead(file))
                {
                    StreamUtils.Copy(streamReader, zipFile, buffer);
                }

                zipFile.CloseEntry();
            }
        }
    }
}
