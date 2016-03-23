using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Tavisca.Frameworks.Serialization.Binary
{
    public sealed class XmlSerializerFacade : ISerializationFacade
    {
        public byte[] Serialize(object obj)
        {
            if (obj == null)
                return null;

            var reqSerializer = GetXmlSerializer(obj.GetType());

            using (TextWriter reqWriter = new StringWriter())
            {
                reqSerializer.Serialize(reqWriter, obj);

                reqWriter.Flush();

                return reqWriter.ToString().ToBytes();
            }
        }

        public T Deserialize<T>(byte[] data)
        {
            if (data == null)
                return default(T);

            var type = typeof(T);

            var reqSerializer = GetXmlSerializer(type);

            var xml = data.FromBytes();
            using (TextReader reader = new StringReader(xml))
            {
                return (T)reqSerializer.Deserialize(reader);
            }
        }

        public T DeepClone<T>(T obj)
        {
            var serialized = Serialize(obj);

            var copy = Deserialize<T>(serialized);

            return copy;
        }

        private static readonly Dictionary<Type, XmlSerializer> XMLSerializerCache = new Dictionary<Type, XmlSerializer>();
        private static readonly ReaderWriterLockSlim SyncXmlObject = new ReaderWriterLockSlim();

        private static XmlSerializer GetXmlSerializer(Type type)
        {
            SyncXmlObject.EnterReadLock();
            try
            {
                if (XMLSerializerCache.ContainsKey(type))
                {
                    return XMLSerializerCache[type];
                }
            }
            finally
            {
                SyncXmlObject.ExitReadLock();
            }

            var serializer = new XmlSerializer(type);
            SyncXmlObject.EnterWriteLock();
            try
            {

                if (!XMLSerializerCache.ContainsKey(type))
                {
                    XMLSerializerCache.Add(type, serializer);
                }
                return serializer;
            }
            finally
            {
                SyncXmlObject.ExitWriteLock();
            }
        }
    }
}