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
	static async void sendString(String input, HttpListenerResponse response)
	{
		byte[] buffer = Encoding.UTF8.GetBytes(input);
		response.ContentLength64 = buffer.Length;
		var output = response.OutputStream;
		await output.WriteAsync(buffer, 0, buffer.Length);
		response.Close();
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
		else
		{
			sendString("<html><body> Go home yankee </body></html>", response);
			Console.WriteLine("my home has destroyed");
		}
	}

	public static async Task Main(String[] args)
	{

		string html = projectFileLoader.getTextFromFile("page/1.html");

		string[] str = Formatter.getFormat(html);

		foreach(string s in str)
		{
			Console.WriteLine(s);
		}

		/*HttpListener listener = new HttpListener();
        //listener.Prefixes.Add("http://127.0.0.1:5000/");
		listener.Prefixes.Add("http://127.0.0.1:5000/");
        listener.Start();
		Console.WriteLine("I am sleepy");
		Console.WriteLine("I am on : http://127.0.0.1:5000/page/1");


		while(true)
		{
			HttpListenerContext context = listener.GetContext();
			await handleRequest(context);
		}*/
	}
	

}
