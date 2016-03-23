using System.Collections.Generic;

namespace Tavisca.Frameworks.Serialization.Specialized
{
    public sealed class InfluxDbTable
    {
        public string Name { get; set; }
        public ICollection<string> Columns { get; set; }
        public ICollection<InfluxDbTableRow> Rows { get; set; }
    }
}