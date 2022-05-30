using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Sources.Common
{
    public class PropertyLayer
    {
        public List<PropertyLayer> InnerPropertyLayers;
        public string Name;
        public List<Properties> Properties;
    }
}
