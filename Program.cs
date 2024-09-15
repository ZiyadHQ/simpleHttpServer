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
using System.Data.SqlTypes;
using System.IO.Compression;
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

		if (File.Exists(imagePath))
		{
			Byte[] bytes = File.ReadAllBytes(imagePath);
			response.ContentLength64 = bytes.Length;
			var output = response.OutputStream;
			output.Write(bytes, 0, bytes.Length);
		}
		else
		{
			var output = response.OutputStream;
			Byte[] error = File.ReadAllBytes(projectFileLoader.pathToFile("page/1.html"));
			response.ContentLength64 = error.Length;
			response.StatusCode = 404;
			output.Write(error, 0, error.Length);
		}
	}

	public static Byte[] compressBytes(Byte[] bytes)
	{
		using (MemoryStream memStream = new())
		{
			using (GZipStream gZipStream = new(memStream, CompressionLevel.SmallestSize))
			{
				gZipStream.Write(bytes, 0, bytes.Length);
			}
			return memStream.ToArray();
		}
	}

	static async Task handleRequest(HttpListenerContext context, HTMLCache cache, ServerMetricsTracker tracker)
	{
		var request = context.Request;
		var response = context.Response;

		if (request.HttpMethod == "GET")
		{
			String filePath = projectFileLoader.pathToFile(request.Url.AbsolutePath.Substring(1));
			if (File.Exists(filePath))
			{
				Byte[] bytes = cache.getBytes(projectFileLoader.pathToFile(request.Url.AbsolutePath.Substring(1)));

				response.Headers.Add("Content-Encoding", "gzip");
				var output = response.OutputStream;
				response.ContentLength64 = bytes.Length;
				if (request.Url.AbsolutePath.EndsWith(".wasm"))
				{
					response.ContentType = "application/wasm";
				}
				try
				{
					output.Write(bytes, 0, bytes.Length);
					tracker.addBytes(bytes.Length);
					tracker.IncrementRequestCountAsync();
				}
				catch (System.Exception)
				{ }
				response.Close();
			}
			else
			{
				Byte[] bytes = File.ReadAllBytes(projectFileLoader.pathToFile("page/404.html"));
				var output = response.OutputStream;
				response.ContentLength64 = bytes.Length;
				if (request.Url.AbsolutePath.EndsWith(".wasm"))
				{
					response.ContentType = "application/wasm";
				}
				output.Write(bytes, 0, bytes.Length);
				response.Close();
				tracker.addBytes(bytes.Length);
			}
		}
		// if (request.HttpMethod == "GET" && request.Url.AbsolutePath.EndsWith(".html"))
		// {
		// 	String htmlText = projectFileLoader.getTextFromFile(request.Url.AbsolutePath.Substring(1));

		// 	sendString(htmlText, response);

		// }
		// else if (request.HttpMethod == "GET" && (request.Url.AbsolutePath.ToString().EndsWith(".jpg") || request.Url.AbsolutePath.ToString().EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) || request.Url.AbsolutePath.ToString().EndsWith(".png")))
		// {
		// 	Console.WriteLine("image sent?");
		// 	try
		// 	{
		// 		sendImage(request.Url.AbsolutePath.ToString(), response);
		// 	}
		// 	catch (System.Exception e)
		// 	{
		// 		Console.WriteLine($"Error getting image: {e}");
		// 	}
		// }
		// else if (request.HttpMethod == "GET" && request.Url.AbsolutePath.EndsWith(".js"))
		// {
		// 	String scriptPath = projectFileLoader.pathToFile(request.Url.AbsolutePath.Substring(1));
		// 	Byte[] bytes = File.ReadAllBytes(scriptPath);
		// 	sendBytes(bytes, response);
		// }
		else
		{
			response.StatusCode = 404;
			sendString("<html><body>404</body></html>", response);
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

		foreach (string s in str)
		{
			Console.WriteLine(s);
		}

		HttpListener listener = new HttpListener();
		//listener.Prefixes.Add("http://127.0.0.1:5000/");
		listener.Prefixes.Add("http://127.0.0.1:5000/");
		listener.Start();
		Console.WriteLine("I am sleepy");
		Console.WriteLine("I am on : http://127.0.0.1:5000/page/1.html");

		HTMLCache cache = new HTMLCache(TimeSpan.FromMilliseconds(10000), 100);
		ServerMetricsTracker serverMetricsTracker = new();
		serverMetricsTracker.setInterval(10000);
		serverMetricsTracker.run();

		while (true)
		{
			HttpListenerContext context = listener.GetContext();
			await handleRequest(context, cache, serverMetricsTracker);
		}
	}


}
