using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavisca.Frameworks.Serialization.Compression
{
    public enum CompressionTypeOptions : int
    {
        None = 0,
        Zip = 1,
        Deflate = 2,
        Lz4 = 3
    }
}
