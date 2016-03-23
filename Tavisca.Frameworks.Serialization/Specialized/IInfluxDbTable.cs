using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavisca.Frameworks.Serialization.Specialized
{
    public interface IInfluxDbData
    {
        ICollection<InfluxDbTable> GetTables();
    }
}