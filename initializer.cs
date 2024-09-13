using System.Text;
using System.Text.Json.Nodes;
using Newtonsoft;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Tools;
using System.Numerics;

public static class Initializer
{

    public static Dictionary<String, Object> getConfigsFromFile(String fileName)
    {
        projectFileLoader.pathToFile(fileName);
        FileStream file = File.Open(fileName, FileMode.Open, FileAccess.Read);
        var Dic = JsonSerializer.Deserialize<Dictionary<String, Object>>(file);
        file.Close();
        return Dic;
    }

}

public struct Configs
{

    public Configs(Dictionary<String, Object> json)
    {
        name = json["name"] as string;
        version = json["version"] as string;
        ip = json["ip"] as string;
        domain = json["domain"] as string;
        port = (int)json["port"];
        nThreads = (int)json["nThreads"];
    }

    public string name;
    public string version;
    public string ip;
    /// <summary>
    /// domain name must be in format {subdomain}.{top-level domain} e.g: website.net.
    /// can be left empty.
    /// </summary>
    public string domain;
    public int port;
    /// <summary>
    /// determines the number of threads spawned to handle traffic, no guarantee of increased throughput.
    /// </summary>
    public int nThreads; 
}