using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tavisca.Frameworks.Serialization.Configuration;
using Tavisca.Frameworks.Serialization.Tests.Data;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Reflection;


namespace Tavisca.Frameworks.Serialization.Tests
{
    [TestClass]
    public class DataContractJsonSerializerTest
    {
        [TestMethod]
        public void SerializeTest()
        {
            var serializer = new SerializationFactory().GetSerializationFacade(SerializerType.DataContractJsonSerializer, Compression.CompressionTypeOptions.None);

            var expected = FlightItineraryObject.GetFlightItineraryObject();
            var serializedData = serializer.Serialize(expected);

            var deserializer = new DataContractJsonSerializer(typeof(FlightItinerary));

            var actual = deserializer.ReadObject(new MemoryStream(serializedData));

            Assert.IsNotNull(actual);
            Assert.IsTrue(expected.Equals(actual));
        }

        [TestMethod]
        public void DeserializeTest()
        {
            var serializer = new DataContractJsonSerializer(typeof(FlightItinerary));

            var expected = FlightItineraryObject.GetFlightItineraryObject();
            var memStream = new MemoryStream();
            serializer.WriteObject(memStream, expected);
            var serializedData = memStream.ToArray();

            var deserializer = new SerializationFactory().GetSerializationFacade(SerializerType.DataContractJsonSerializer, Compression.CompressionTypeOptions.None);

            var actual = deserializer.Deserialize<FlightItinerary>(serializedData);

            Assert.IsNotNull(actual);
            Assert.IsTrue(expected.Equals(actual));
        }
    }
}