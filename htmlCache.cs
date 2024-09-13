using System.Text;
using ScottPlot.Plottables;
using Tools;

namespace simpleHttpServer;

public class HTMLCache
{

    Dictionary<string, Byte[]> Cache;
    Dictionary<string, DateTime> expirationMap;
    
    //the time an entry is allowed to exist without being accessed
    TimeSpan expirationTime;

    //how many times the Cache is accessed before the Cache gets cleaned
    int cleanUpInterval;
    int currentCleanUpInterval;

    public HTMLCache(TimeSpan expirationTime, int cleanUpInterval)
    {
        this.expirationTime = expirationTime;
        this.cleanUpInterval = cleanUpInterval;
        this.currentCleanUpInterval = cleanUpInterval;
        Cache = new Dictionary<string, Byte[]>();
        expirationMap = new Dictionary<string, DateTime>();
    }

    private void cleanUpExpiredEntries()
    {
            foreach(var keyValuePair in expirationMap)
            {
                if(keyValuePair.Value < DateTime.Now)
                {
                    Delete(keyValuePair.Key);
                }
            }
    }

    public void Add(String key, Byte[] value)
    {
        Cache[key] = value;
        expirationMap[key] = DateTime.Now.Add(expirationTime);
    }

    public void Delete(String key)
    {
        Cache.Remove(key);
        expirationMap.Remove(key);
    }

    //the key will be the processed html page request path
    public Byte[] getBytes(string key)
    {
        Byte[] toReturn;
        if(expirationMap.ContainsKey(key) == false)
        {
            if(key.EndsWith(".html"))
            {
                String content = projectFileLoader.getTextFromFile(key);
                toReturn = Encoding.UTF8.GetBytes(content);
            }
            else if(key.EndsWith(".jpg") || key.EndsWith(".jpeg") || key.EndsWith(".png")) 
            {
                    Byte[] bytes = File.ReadAllBytes(projectFileLoader.pathToFile(key));
                    toReturn = bytes;
            }
            else
            {
                throw new Exception("couldn't store this file to cache");
            }
            Add(key, toReturn);
        }else
        {
            toReturn  = Cache[key];
        }
        if(currentCleanUpInterval-- == 0)
        {
            cleanUpExpiredEntries();
            currentCleanUpInterval = cleanUpInterval;
        }
        return toReturn;
    }

}
