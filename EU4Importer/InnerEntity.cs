using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eu4Importer
{
    internal class InnerEntity
    {
        public List<string> UnprocessedInnerEntities = new();
        public string? Name;
        public string? Value;
        public List<InnerEntity> InnerEntities = new();
        public void Clear()
        {
            this.Name = null;
            this.Value = null;
            this.InnerEntities = new();
            this.UnprocessedInnerEntities = new();
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Name);
            if (InnerEntities.Count > 0)
            {
                foreach (InnerEntity entity in InnerEntities)
                {
                    sb.Append(entity.ToString());
                }
            }
            else
            {
                sb.AppendLine(Value);
            }
            foreach (string String in UnprocessedInnerEntities)
            {
                sb.AppendLine(String);
            }
            return sb.ToString();
        }
    }
}
