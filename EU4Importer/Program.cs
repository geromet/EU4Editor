using Eu4Importer;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

internal class Program
{
    static Parser parser = new();
    public static void Main()
    {
        Run();
    }
    private static void Run()
    {
        int counter = 0;
        string[] paths = Directory.GetFiles("Source", "*.txt", SearchOption.AllDirectories);
        foreach (string path in paths)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine("parsing #"+counter+":         " + path +"\n\n\n");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
            parser.ParsePath(path);
            counter++;
        }

    } 

    private static string RemoveComments(string input)
    {
        string[] strings = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        StringBuilder output = new StringBuilder();
        foreach(string s in strings)
        {
            output.Append(s.Split('#')[0]).Replace("#", "");
        }
        return output.ToString().Replace("\n", "").Replace("\r", "").Replace("="," = ");
    }
}
