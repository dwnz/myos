using System;
using System.IO;

namespace Text
{
    static class FileInfo
    {
        public static String Path = Directory.GetCurrentDirectory();
        public static String Filename = "Untitled.txt";

        public static Boolean HasChanged;
    }
}
