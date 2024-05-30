
using System;
using System.IO;

namespace Tools;

public static class projectFileLoader
{

    public static readonly string pathToDirectory;

    static projectFileLoader()
    {
        string relativePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..");
        string fullPath = Path.GetFullPath(relativePath);
        pathToDirectory = fullPath;
    }

    //recieves path relative to project root
    public static string pathToFile(string path)
    {
        return Path.Combine(pathToDirectory, path);
    }

    //recieves path relative to project root, returns the text content of that file if found, else crashes thread
    public static string getTextFromFile(string path)
    {
        return File.ReadAllText(pathToFile(path));
    }

}


