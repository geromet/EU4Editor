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
        string[] paths = Directory.GetFiles("Source", "*.txt", SearchOption.AllDirectories);
        foreach (string path in paths)
        {
            Console.WriteLine(File.ReadAllText(path));
            parser.ParseString(RemoveComments(File.ReadAllText(path)));
        }

    } 

    private static string RemoveComments(string input)
    {
        string[] strings = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
        StringBuilder output = new StringBuilder();
        foreach(string s in strings)
        {
            output.Append(s.Split('#')[0]);
        }
        return output.ToString().Replace("\n", "").Replace("\r", "");
    }
}
