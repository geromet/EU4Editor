using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eu4Importer
{
    internal record GameFile
    {
        public List<string> UnprocessedOuterEntites = new();
        public string Name;
        public string Path;
        public List<OuterEntity> OuterEntities = new();
        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine(Name);
            sb.AppendLine(Path);
            foreach(OuterEntity entity in OuterEntities)
            {
                sb.AppendLine(entity.ToString());
            }
            foreach(string entity in UnprocessedOuterEntites)
            {
                sb.AppendLine(entity);
            }
            return sb.ToString();
        }
    }
}
