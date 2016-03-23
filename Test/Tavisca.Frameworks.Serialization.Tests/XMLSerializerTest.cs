using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tavisca.Frameworks.Serialization.Configuration;
using Tavisca.Frameworks.Serialization.Tests.Data;
using System.Xml.Serialization;
using System.IO;
using Tavisca.Frameworks.Serialization.Binary;
using Tavisca.Frameworks.Serialization.Exceptions;

namespace Tavisca.Frameworks.Serialization.Tests
{
    [TestClass]
    public class XMLSerializerTest
    {
        [TestMethod]
        public void SerializeTest()
        {
            var serializer = new SerializationFactory().GetSerializationFacade(SerializerType.XmlSerializer, Compression.CompressionTypeOptions.None);

            var expected = FlightItineraryObject.GetFlightItineraryObject();
            var serializedData = serializer.Serialize(expected);

            var deserializer = new XmlSerializer(typeof(FlightItinerary));

            var actual = deserializer.Deserialize(new StringReader(serializedData.FromBytes()));

            Assert.IsNotNull(actual);
            Assert.IsTrue(expected.Equals(actual));
        }

        [TestMethod]
        public void DeserializeTest()
        {
            var serializer = new XmlSerializer(typeof(FlightItinerary));

            var expected = FlightItineraryObject.GetFlightItineraryObject();
            var reqWriter = new StringWriter();
            serializer.Serialize(reqWriter, expected);
            reqWriter.Flush();
            var serializedData = reqWriter.ToString().ToBytes();

            var deserializer = new SerializationFactory().GetSerializationFacade(SerializerType.XmlSerializer, Compression.CompressionTypeOptions.None);

            var actual = deserializer.Deserialize<FlightItinerary>(serializedData);

            Assert.IsNotNull(actual);
            Assert.IsTrue(expected.Equals(actual));
        }
    }
}