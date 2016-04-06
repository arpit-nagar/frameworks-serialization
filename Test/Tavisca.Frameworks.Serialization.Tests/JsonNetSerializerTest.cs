using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Tavisca.Frameworks.Serialization.Configuration;
using Tavisca.Frameworks.Serialization.Tests.Data;
using Tavisca.Frameworks.Serialization.Binary;
using Tavisca.Frameworks.Serialization.Exceptions;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

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

        [TestMethod]
        [ExpectedException(typeof(SerializationException))]
        public void SerailizationWithInvalidSettingShouldThrowSerializationException()
        {

            var serializer = new SerializationFactory().GetSerializationFacade(SerializerType.NewtonsoftJsonNetSerializer, Compression.CompressionTypeOptions.None);

            var expected = FlightItineraryObject.GetFlightItineraryObject();
            var serializedData = serializer.Serialize(expected, new object());
        }

        [TestMethod]
        [ExpectedException(typeof(SerializationException))]
        public void DeserailizationWithInvalidSettingShouldThrowSerializationException()
        {

            var expected = FlightItineraryObject.GetFlightItineraryObject();
            var serializedData = JsonConvert.SerializeObject(expected).ToBytes();

            var deserializer = new SerializationFactory().GetSerializationFacade(SerializerType.NewtonsoftJsonNetSerializer, Compression.CompressionTypeOptions.None);

            var actual = deserializer.Deserialize<FlightItinerary>(serializedData, new object());


        }

        [TestMethod]
        public void SerializationWithValidSettingsShouldSerialize()
        {
            var serializer = new SerializationFactory().GetSerializationFacade(SerializerType.NewtonsoftJsonNetSerializer, Compression.CompressionTypeOptions.None);

            var jsonSerializerSetting = new JsonSerializerSettings()
            {
                Converters = new List<JsonConverter>()
                {
                    new StringEnumConverter(),
                }
            };

            List<StringComparison> stringComparisons = new List<StringComparison>
            {
                StringComparison.CurrentCulture,
                StringComparison.Ordinal
            };

            var serializedData = serializer.Serialize(stringComparisons, jsonSerializerSetting);

            var serializedString = serializedData.FromBytes();

            Assert.IsTrue(serializedString.Contains("CurrentCulture"));
            Assert.IsTrue(serializedString.Contains("Ordinal"));
        }

        [TestMethod]
        public void DeserializationWithValidSettingsShouldDeserialize()
        {
            var serializer = new SerializationFactory().GetSerializationFacade(SerializerType.NewtonsoftJsonNetSerializer, Compression.CompressionTypeOptions.None);


            var jsonSerializerSetting = new JsonSerializerSettings()
            {
                Converters = new List<JsonConverter>()
                {
                    new StringEnumConverter(),
                }
            };

            List<StringComparison> stringComparisons = new List<StringComparison>
            {
                StringComparison.CurrentCulture,
                StringComparison.Ordinal
            };

            var serializedData = JsonConvert.SerializeObject(stringComparisons, jsonSerializerSetting);

            var actual = serializer.Deserialize<List<StringComparison>>(serializedData.ToBytes(), jsonSerializerSetting);
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Exists(x=>x == StringComparison.CurrentCulture));
            Assert.IsTrue(actual.Exists(x =>x== StringComparison.Ordinal));
        }

    }
}