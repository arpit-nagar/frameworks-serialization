using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Tavisca.Frameworks.Serialization.Configuration;
using Tavisca.Frameworks.Serialization.Tests.Data;
using Tavisca.Frameworks.Serialization.Binary;


namespace Tavisca.Frameworks.Serialization.Tests
{
    [TestClass]
    public class JsonNetSerializerTest
    {
        [TestMethod]
        public void SerializeTest()
        {
            var serializer = new SerializationFactory().GetSerializationFacade(SerializerType.NewtonsoftJsonNetSerializer, Compression.CompressionTypeOptions.None);

            var expected = FlightItineraryObject.GetFlightItineraryObject();
            var serializedData = serializer.Serialize(expected);

            var actual = JsonConvert.DeserializeObject<FlightItinerary>(serializedData.FromBytes());

            Assert.IsNotNull(actual);
            Assert.IsTrue(expected.Equals(actual));
        }

        [TestMethod]
        public void DeserializeTest()
        {
            var expected = FlightItineraryObject.GetFlightItineraryObject();
            var serializedData = JsonConvert.SerializeObject(expected).ToBytes();

            var deserializer = new SerializationFactory().GetSerializationFacade(SerializerType.NewtonsoftJsonNetSerializer, Compression.CompressionTypeOptions.None);

            var actual = deserializer.Deserialize<FlightItinerary>(serializedData);

            Assert.IsNotNull(actual);
            Assert.IsTrue(expected.Equals(actual));
        }
    }
}