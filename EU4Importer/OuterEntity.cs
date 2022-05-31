using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eu4Importer
{
    internal class OuterEntity
    {
        public List<string> UnprocessedInnerEntities = new();
        public string? Name = "";
        public List<InnerEntity> InnerEntities = new();
        public void Clear()
        {
            this.UnprocessedInnerEntities.Clear();
            this.InnerEntities.Clear();
            this.Name = null;
        }
        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append(Name);
            foreach (InnerEntity entity in InnerEntities)
            {
                sb.Append(entity.ToString());
            }
            foreach(string entity in UnprocessedInnerEntities)
            {
                sb.Append(entity);
            }
            return sb.ToString();
        }
    }
}
