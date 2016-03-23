using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Tavisca.Frameworks.Serialization.Configuration;
using Tavisca.Frameworks.Serialization.Tests.Data;


namespace Tavisca.Frameworks.Serialization.Tests
{
    [TestClass]
    public class DataContractSerializerTest
    {
        [TestMethod]
        public void SerializeTest()
        {
            var serializer = new SerializationFactory().GetSerializationFacade(SerializerType.DataContractSerializer, Compression.CompressionTypeOptions.None);

            var expected = FlightItineraryObject.GetFlightItineraryObject();
            var serializedData = serializer.Serialize(expected);

            var deserializer = new DataContractSerializer(typeof(FlightItinerary));

            var actual = deserializer.ReadObject(new MemoryStream(serializedData));

            Assert.IsNotNull(actual);
            Assert.IsTrue(expected.Equals(actual));
        }

        [TestMethod]
        public void DeserializeTest()
        {
            var serializer = new DataContractSerializer(typeof(FlightItinerary));

            var expected = FlightItineraryObject.GetFlightItineraryObject();
            var memoryStream = new MemoryStream();
            serializer.WriteObject(memoryStream, expected);
            var serializedData = new byte[memoryStream.Length];

            Array.Copy(memoryStream.GetBuffer(), serializedData, serializedData.Length);

            var deserializer = new SerializationFactory().GetSerializationFacade(SerializerType.DataContractSerializer, Compression.CompressionTypeOptions.None);

            var actual = deserializer.Deserialize<FlightItinerary>(serializedData);

            Assert.IsNotNull(actual);
            Assert.IsTrue(expected.Equals(actual));
        }
    }
}