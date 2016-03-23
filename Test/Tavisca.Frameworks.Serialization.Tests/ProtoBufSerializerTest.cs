using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavisca.Frameworks.Serialization.Configuration;
using Tavisca.Frameworks.Serialization.Tests.Data;
using ProtoBuf;
using ProtoBuf.Meta;
using System.IO;
using Tavisca.Frameworks.Serialization.Exceptions;

namespace Tavisca.Frameworks.Serialization.Tests
{
    [TestClass]
    public class ProtoBufSerializerTest
    {
        [TestMethod]
        public void SerializeTest()
        {
            var serializer = new SerializationFactory().GetSerializationFacade(SerializerType.ProtoBuf, Compression.CompressionTypeOptions.None);

            var expected = FlightItineraryObject.GetFlightItineraryObject();
            var serializedData = serializer.Serialize(expected);

            var deserializer = TypeModel.Create();

            var actual = deserializer.Deserialize(new MemoryStream(serializedData), null, typeof(FlightItinerary));

            Assert.IsNotNull(actual);
            Assert.IsTrue(expected.Equals(actual));
        }

        [TestMethod]
        public void DeserializeTest()
        {
            var serializer = TypeModel.Create();

            var expected = FlightItineraryObject.GetFlightItineraryObject();
            var memStream = new MemoryStream();
            serializer.Serialize(memStream, expected);
            var serializedData = memStream.ToArray();

            var deserializer = new SerializationFactory().GetSerializationFacade(SerializerType.ProtoBuf, Compression.CompressionTypeOptions.None);

            var actual = deserializer.Deserialize<FlightItinerary>(serializedData);

            Assert.IsNotNull(actual);
            Assert.IsTrue(expected.Equals(actual));
        }

        [TestMethod]
        [ExpectedException(typeof(SerializationException))]
        public void SerializeWithoutSerializationAttributeTest()
        {
            var serializer = new SerializationFactory().GetSerializationFacade(SerializerType.ProtoBuf, Compression.CompressionTypeOptions.None);

            var expected = FlightItineraryObject.GetFlightItineraryObjectWithoutSerializationAttribute();
            var serializedData = serializer.Serialize(expected);

            var deserializer = TypeModel.Create();

            var actual = deserializer.Deserialize(new MemoryStream(serializedData), null, typeof(FlightItineraryWithoutSerializationAttribute));

            Assert.IsNotNull(actual);
            Assert.IsTrue(expected.Equals(actual));
        }
    }
}