using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavisca.Frameworks.Serialization.Binary
{
    public static class StringByteConverter
    {
        public static byte[] ToBytes(this string data)
        {
            return Encoding.UTF8.GetBytes(data);
        }

        public static string FromBytes(this byte[] data)
        {
            return Encoding.UTF8.GetString(data);
        }
    }
}
