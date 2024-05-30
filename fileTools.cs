
using System;
using System.IO;

namespace Tool
{
    public static class fileLoader
    {

        public static readonly string pathToDirectory;

        static fileLoader()
        {
            string relativePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..");
            string fullPath = Path.GetFullPath(relativePath);
            Console.WriteLine(fullPath);
            Console.WriteLine(AppContext.BaseDirectory);
            pathToDirectory = fullPath;
        }

    }
}


