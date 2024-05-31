using Tools;
using System;
using System.Data;
using System.Diagnostics.Metrics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
public class Server
{

	public static void Main(String[] args)
	{

		HttpListener listener = new HttpListener();
        listener.Prefixes.Add("http://*:80/");
        listener.Start();

		Console.WriteLine("hello world!!");
		String st = projectFileLoader.pathToDirectory;

		Console.WriteLine(st);

		Console.WriteLine(projectFileLoader.getTextFromFile("page/1.html"));

	}

}
