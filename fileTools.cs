
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Tools;

//class concerned with file loading and formatting
public static class projectFileLoader
{

    public static readonly string pathToDirectory;

    static projectFileLoader()
    {
        string relativePath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..");
        string fullPath = Path.GetFullPath(relativePath);
        pathToDirectory = fullPath;
    }

    /// <summary>
    /// retrieve path relative to project root
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string pathToFile(string path)
    {
        return Path.Combine(pathToDirectory, path);
    }

    /// <summary>
    /// recieves path relative to project root, returns the text content of that file if found, else crashes thread
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string getTextFromFile(string path)
    {
        try
        {
            return File.ReadAllText(pathToFile(path));
        }
        catch (System.Exception)
        {
            
            return "<html><h1>404</h1><h2>page not found!</h2></html>";
        }
    }

}


public static class Formatter
{

    ///function takes format parametrs: "@{targetString}" and source parameters: "{string to replace original string}"
    //e.g input = "{A} and {B}", parameters[] = ["@{A}", "@{B}", "ahmad", "khaled"], the input will become: "ahmad and khaled"
    //must include "@" at the start of the format string to differentate between it and non format strings
    public static void formatString(string input, string[] parameters)
    {
        int count = 0;
        //match each format string with a source string
        foreach(string str in parameters)
        {
            if(str.StartsWith('@'))
            {
                count++;
            }else
            {
                count--;
            }
        }
        
        if(count != 0)
        {
            if(count > 0)
            {
                throw new Exception("Too many format strings!");
            }else if(count < 0)
            {
                throw new Exception("Too many source strings!");
            }
        }

        StringBuilder sb = new StringBuilder(input);

        int numOfPairs = parameters.Length/2;
        for(int i=0; i<numOfPairs; i++)
        {
            sb.Replace(parameters[i][1..], parameters[i + numOfPairs]);
        }

        input = sb.ToString();

    }

    //returns the format strings from the begening of the html file, then returns all the format strings found in an array
    public static String[] getFormat(String input)
    {

        string pattern = @"<!--(.*?)-->";

        Match match = Regex.Match(input, pattern, RegexOptions.Singleline);

        string formatString = match.Groups[1].Value.Trim(); 

        if(!match.Success)
        {
            throw new Exception("couldn't get format line");
        }

        StringBuilder sb = new StringBuilder(formatString);

        formatString = sb.ToString();

        string[] list = formatString.Split(',', StringSplitOptions.TrimEntries);

        return list;

    }
}


//tracks basics metrics of the server, allows for async requests and changes
public class ServerMetricsTracker
{
    //tracks the total amount of http requests done since the start of the server, can also be loaded from a file
    long httpRequestsDone;

    //copies the value of the httpRequestsDone each set interval in updateInterval 
    long lastHttpRequestsDone;

    //in milliseconds, the amount of time between updates
    int updateInterval;

    //determines if the thread updater is runnig or not
    bool running = false;

    public void run()
    {
        running = true;
        while(true)
        {

            while(running)
            {
                //code for the update logic
            }

        }

    }

    public long IncrementRequestCountAsync()
    {
        return Interlocked.Increment(ref httpRequestsDone);
    }

    public long GetRequestCount()
    {
        return Interlocked.Read(ref httpRequestsDone);
    }

    public async Task PauseThread()
    {
        await Task.Run(() => running = false);
    }

}

