using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IO;

namespace Tavisca.Frameworks.Serialization.Binary
{
    public sealed class BinarySerializationFacade : ISerializationFacade
    {
        private static readonly RecyclableMemoryStreamManager StreamManager = new RecyclableMemoryStreamManager();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public byte[] Serialize(object obj, object serializationSetting = null)
        {
            if (obj == null)
                return null;

            byte[] buffer;
            using (var stream = StreamManager.GetStream())
            {
                //Creating binary formatter to serialize object.
                var formatter = new BinaryFormatter();
                //Serializing objectToSeralize. 
                formatter.Serialize(stream, obj);

                buffer = stream.ToArray();
            }

            return buffer;
        }

        public T Deserialize<T>(byte[] data, object deserializationSetting = null)
        {
            if (data == null)
                return default(T);

            object serializedObject;
            var bytes = data.ToArray();
            using (var stream = StreamManager.GetStream("DeSerialization.Binary", bytes, 0, bytes.Length))
            {
                //Creating binary formatter to De-Serialize string.
                var formatter = new BinaryFormatter();
                //De-Serializing.
                serializedObject = formatter.Deserialize(stream);
            }

            return (T)serializedObject;
        }

        public T DeepClone<T>(T obj)
        {
            using (var stream = StreamManager.GetStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }
    }
}
