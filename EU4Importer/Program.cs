using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public class Program
{
    private static Program program = new Program();
    public static void Main()
    {
        program.Run();
    }
    private void Run()
    {
        string[] paths = Directory.GetFiles("Source", "*.txt", SearchOption.AllDirectories);
        /*foreach(string path in paths)
        {
            program.ReadFile(path);
        }*/
        paths = Directory.GetFiles("ProcessedSource", "*.txt", SearchOption.AllDirectories);
        ReprocessAsync(paths);
    }
    private void ReprocessAsync(string[] paths)
    {
        foreach (string path in paths)
        {
            //program.Reprocess(path);
            Parallel.ForEach(paths, new ParallelOptions { MaxDegreeOfParallelism = 1 }, path =>
             {
                 program.Reprocess(path);
             });
        }       
    }
    private void Reprocess(string path)
    {
            string[] lines = System.IO.File.ReadAllLines(path);
            var sb = new StringBuilder();
            foreach (string line in lines)
            {
                OuterEntity outerEntity = program.GetOuterEntity(line);
                sb.Append(outerEntity.ToString() + "\n");
            }
            string outer = sb.ToString();
            string resultString = Regex.Replace(outer, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
            File.WriteAllText("Re" + path, resultString);  
    }
    private OuterEntity GetOuterEntity(string line)
    {
        InnerEntityCollection innerEntityCollection = new InnerEntityCollection();
        var sb = new StringBuilder();
        int currentPos = 0;
        foreach (char c in line)
        {
            if (c == '{')
            {
                for (int i = 0; i <= currentPos; i++)
                {
                    sb.Append(line[i]);
                }
                OuterEntity outerEntity = new();
                outerEntity.Name = sb.ToString();
                sb.Clear();
                outerEntity.InnerEntities = innerEntityCollection.GetInnerEntities(line.Substring(currentPos + 1));
                return outerEntity;
            }
            currentPos++;
        }
        return new OuterEntity { Name = "No OuterEntity found" };
    }
    
    public class InnerEntityCollection
    {
        private List<InnerEntity> innerEntities = new();
        public List<InnerEntity> GetInnerEntities(string line)
        {
            var sb = new StringBuilder();
            InnerEntity valueEntity = new InnerEntity();
            int currentPos = 0;
            int lastPos = 0;
            foreach (char c in line)
            {
                if (c == '{' && valueEntity.Name!="")
                {
                    InnerEntity nestedEntity = new();
                    nestedEntity.Name = valueEntity.Name;
                    nestedEntity.InnerEntities = GetInnerEntities(line.Substring(currentPos + 1));
                    lastPos = currentPos + 1;
                    innerEntities.Add(nestedEntity);
                    sb.Clear();
                }
                else if (c == '=')
                {
                    if (valueEntity.Name == "")
                    {
                        for (int i = lastPos; i <= currentPos; i++)
                        {
                            sb.Append(line[i]);
                        }
                        valueEntity.Name = sb.ToString();
                        lastPos = currentPos + 1;
                        sb.Clear();
                    }                  
                }
                else if (c==' ' && valueEntity.Name != "")
                {
                    for (int i = lastPos; i <= currentPos; i++)
                    {
                        if(line[i] != ' ')
                        {
                            sb.Append(line[i]);
                        } 
                    }
                    valueEntity.Value = sb.ToString();
                    lastPos = currentPos + 1;
                    sb.Clear();
                    innerEntities.Add(valueEntity);
                    valueEntity = new();
                }
                currentPos++;
            }
            return innerEntities;
        }
    }
    private void ReadFile(string path) //Returns only last OuterEntity?
    {
        string[] lines = System.IO.File.ReadAllLines(path);
        string currentLine;
        var sb = new StringBuilder();
        foreach (string line in lines)
        {
            if (line.Contains("#"))
            {
                currentLine = line.Split("#")[0];
                currentLine = Regex.Replace(currentLine, @"\n", "");
                sb.Append(currentLine.ToString());  
            }
            else
            {
                currentLine = line;
                currentLine = Regex.Replace(currentLine, @"\n", "");
                sb.Append(currentLine);
            }

        }   
        int currentPos = 0;
        int lastPos = 0;
        int brackets = 0;
        bool inBrackets = false;
        string st = sb.ToString();
        foreach (char c in st)
        {
            if (c == '{')
            {
                inBrackets = true;
                brackets++;
            }
            else if (c == '}')
            {
                brackets--;
            }
            if (brackets == 0 && inBrackets)
            {
                sb.Clear();
                inBrackets = false;
                for (int i = lastPos; i <= currentPos; i++)
                {
                    sb.Append(st[i]);
                }
                sb.Append('\n');
                lastPos = currentPos + 1;
            }
            currentPos++;
        }
        lines = sb.ToString().Split(Environment.NewLine.ToCharArray());
        File.WriteAllLines("Processed"+path, lines); ;
    }
    public class OuterEntity
    {
        public string Name ="";
        public List<InnerEntity> InnerEntities = new();
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Name);
            foreach (InnerEntity entity in InnerEntities)
            {
                sb.Append(entity.ToString());
            }
            return sb.ToString();
        }
    }
    public class InnerEntity
    {
        public string Name;
        public string Value;
        public List<InnerEntity> InnerEntities = new();
        private StringBuilder sb = new StringBuilder();

        public override string ToString()
        {
            sb.AppendLine(Name);
            if(InnerEntities.Count >0)
            {
                foreach(InnerEntity entity in InnerEntities)
                {
                    //sb.Append(entity.ToString());
                }
            }
            else
            {
                sb.AppendLine(Value);
            }
            return sb.ToString();
        }

    }
}
