using Eu4Importer;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

internal class Program
{
    private static Program program = new();
    public List<GameFile> gameFiles = new();
    public static void Main()
    {
        program.Run();
    }
    private void Run()
    {
        string[] paths = Directory.GetFiles("Source", "*.txt", SearchOption.AllDirectories);
        foreach (string path in paths)
        {
            Process.GetOuterEntities(path,this);
        }
        Process.ProcessOuterEntities(this);
        //Process.ProcessInnerEntities(this);
        Test.PrintOuterEntities(this);
    } 
}
