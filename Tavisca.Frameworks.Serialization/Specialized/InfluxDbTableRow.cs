using System.Collections.Generic;

namespace Tavisca.Frameworks.Serialization.Specialized
{
    public sealed class InfluxDbTableRow
    {
        public ICollection<InfluxDbTableRowItem> Items { get; set; }
    }
}