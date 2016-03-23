using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tavisca.Frameworks.Serialization.Configuration;
using Tavisca.Frameworks.Serialization.Tests.Data;
using Tavisca.Frameworks.Serialization.Binary;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;
using Tavisca.Frameworks.Serialization.Exceptions;


namespace Tavisca.Frameworks.Serialization.Tests
{
    [TestClass]
    public class BinarySerializerTest
    {
        [TestMethod]
        public void SerializeTest()
        {
            var serializer = new SerializationFactory().GetSerializationFacade(SerializerType.Binary, Compression.CompressionTypeOptions.None);

            var expected = FlightItineraryObject.GetFlightItineraryObject();
            var serializedData = serializer.Serialize(expected);

            var deserializer = new BinaryFormatter();
            
            var actual = deserializer.Deserialize(new MemoryStream(serializedData.ToArray()));

            Assert.IsNotNull(actual);
            Assert.IsTrue(expected.Equals(actual));
        }

        [TestMethod]
        public void DeserializeTest()
        {
            var serializer = new BinaryFormatter();

            var expected = FlightItineraryObject.GetFlightItineraryObject();
            var stream = new MemoryStream();
            serializer.Serialize(stream, expected);
            var serializedData = stream.ToArray();

            var deserializer = new SerializationFactory().GetSerializationFacade(SerializerType.Binary, Compression.CompressionTypeOptions.None);

            var actual = deserializer.Deserialize<FlightItinerary>(serializedData);

            Assert.IsNotNull(actual);
            Assert.IsTrue(expected.Equals(actual));
        }

        [TestMethod]
        [ExpectedException(typeof(SerializationException))]
        public void SerializeWithoutSerializationAttributeTest()
        {
            var serializer = new SerializationFactory().GetSerializationFacade(SerializerType.Binary, Compression.CompressionTypeOptions.None);

            var expected = FlightItineraryObject.GetFlightItineraryObjectWithoutSerializationAttribute();
            var serializedData = serializer.Serialize(expected);

            var deserializer = new BinaryFormatter();

            var actual = deserializer.Deserialize(new MemoryStream(serializedData.ToArray()));

            Assert.IsNotNull(actual);
            Assert.IsTrue(expected.Equals(actual));
        }
    }
}