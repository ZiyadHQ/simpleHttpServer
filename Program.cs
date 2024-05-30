using Tools;
public class Server
{

	public static void Main(String[] args)
	{

		Console.WriteLine("hello world!!");
		String st = projectFileLoader.pathToDirectory;

		Console.WriteLine(st);

		Console.WriteLine(projectFileLoader.getTextFromFile("page/1.html"));

	}

}
