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
using simpleHttpServer;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Text.Json;
using ScottPlot.DataViews;
public class Server
{
	static async void sendString(String input, HttpListenerResponse response)
	{
		byte[] buffer = Encoding.UTF8.GetBytes(input);
		response.ContentLength64 = buffer.Length;
		var output = response.OutputStream;
		await output.WriteAsync(buffer, 0, buffer.Length);
		response.Close();
	}

	static async void sendBytes(Byte[] bytes, HttpListenerResponse response)
	{
		var output = response.OutputStream;
		response.ContentLength64 = bytes.Length;
		output.Write(bytes, 0, bytes.Length);
		response.Close();
	}

	static async void sendImage(String imagePath, HttpListenerResponse response)
	{
		imagePath = projectFileLoader.pathToFile(imagePath.Substring(1));
		Console.WriteLine(imagePath);
		Byte[] bytes = File.ReadAllBytes(imagePath);
		response.ContentLength64 = bytes.Length;
		var output = response.OutputStream;
		output.Write(bytes, 0, bytes.Length);
	}

	static async Task handleRequest(HttpListenerContext context)
	{
		var request = context.Request;
		var response = context.Response;
		
		if(request.HttpMethod == "GET" && request.Url.AbsolutePath.ToString().Contains("page"))
		{
			String htmlText = projectFileLoader.getTextFromFile(request.Url.AbsolutePath.Substring(1) + ".html");

			sendString(htmlText, response);
			
		}
		else if(request.HttpMethod == "GET" && (request.Url.AbsolutePath.ToString().EndsWith(".jpg") || request.Url.AbsolutePath.ToString().EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) || request.Url.AbsolutePath.ToString().EndsWith(".png")))
		{
			Console.WriteLine("image sent?");
			sendImage(request.Url.AbsolutePath.ToString() ,response);
		}
		else if(request.HttpMethod == "GET" && request.Url.AbsolutePath.EndsWith(".jpg"))
		{
			String scriptPath = projectFileLoader.pathToFile(request.Url.AbsolutePath.Substring(1));
			Byte[] bytes = File.ReadAllBytes(scriptPath);
			sendBytes(bytes, response);
		}
		else
		{
			sendString("<html><body> Go home yankee </body></html>", response);
			Console.WriteLine("my home has destroyed");
		}
	}

	public static async Task Main(String[] args)
	{

		Configs config = new();
		config.domain = "TEST.DOMAIN";
		config.ip = "IP";
		config.name = "SERVER NAME";
		config.nThreads = 8;
		config.port = 80;
		config.version = "V1";

		Console.WriteLine(JsonSerializer.Serialize(config));

		string html = projectFileLoader.getTextFromFile("page/1.html");

		string[] str = Formatter.getFormat(html);

		foreach(string s in str)
		{
			Console.WriteLine(s);
		}

		HTMLCache Cache = new HTMLCache(TimeSpan.FromMilliseconds(10000) , 100);
		// List<float> Y1 = new();
		// List<float> Y2 = new();
		// List<float> X = new();
		// int testSize = 50;
		// Stopwatch stopwatch = new();
		// for(int i=0; i<testSize; i++)
		// {
		// 	stopwatch.Start();
		// 	for(int j=0; j<i*10; j++)
		// 	{
		// 		string page = (i%3 + 1).ToString();
		// 		string HTML = Cache.getHTML("page/" + page + ".html");
		// 	}
		// 	Y1.Add(stopwatch.ElapsedMilliseconds);
		// 	X.Add(i*10);
		// 	stopwatch.Reset();
		// }
		// for(int i=0; i<testSize; i++)
		// {
		// 	stopwatch.Start();
		// 	for(int j=0; j<i*10; j++)
		// 	{
		// 		string page = (i%3 + 1).ToString();
		// 		string HTML = projectFileLoader.getTextFromFile("page/" + page + ".html");
		// 	}
		// 	Y2.Add(stopwatch.ElapsedMilliseconds);
		// 	stopwatch.Reset();
		// }

		// PlotSuite.plotToFile(X.ToArray(), Y1.ToArray(), "Plot1.png");
		// PlotSuite.plotToFile(X.ToArray(), Y2.ToArray(), "Plot2.png");


		HttpListener listener = new HttpListener();
        //listener.Prefixes.Add("http://127.0.0.1:5000/");
		listener.Prefixes.Add("http://127.0.0.1:5000/");
        listener.Start();
		Console.WriteLine("I am sleepy");
		Console.WriteLine("I am on : http://127.0.0.1:5000/page/1");


		while(true)
		{
			HttpListenerContext context = listener.GetContext();
			await handleRequest(context);
		}
	}
	

}
