using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eu4Importer
{
    internal class Test
    {
        public static void PrintOuterEntities(Program program)
        {
            List<string> strings = new();
            foreach (GameFile gameFile in program.gameFiles)
            {
                strings.Add(gameFile.ToString());
            }
            foreach (string String in strings)
            {
                File.WriteAllText("test.txt", String);
            }
        }
    }
}
