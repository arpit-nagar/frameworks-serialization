using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tavisca.Frameworks.Serialization.Configuration;

using System.Collections.Generic;
using Tavisca.Frameworks.Serialization.Compression;
using Tavisca.Frameworks.Serialization.Exceptions;
using Tavisca.Frameworks.Serialization.Tests.Configuration;
using Tavisca.Frameworks.Serialization.Tests.Data;

namespace Tavisca.Frameworks.Serialization.Tests
{
    [TestClass]
    public class SerializationFactoryConfigDrivenTest
    {
        [TestMethod]
        public void SerializationWithDefaultProvider()
        {
            MockApplicationSerializationConfiguration config = new MockApplicationSerializationConfiguration();
            config.DefaultSerializationProvider = SerializerType.ProtoBuf;
            SerializationConfigurationManager.SetConfiguration(config);

            var serializer = new SerializationFactory().GetSerializationFacade();

            var expected = new Airport()
            {
                AirportCode = "IGI",
                AirportName = "Indira Gandhi International Airport",
                Terminal = new Terminal() { TerminalId = 12345, TerminalName = "T1" }
            };

            var serializedData = serializer.Serialize(expected);

            var actual = serializer.Deserialize<Airport>(serializedData);

            Assert.IsNotNull(actual);
            Assert.IsTrue(expected.Equals(actual));
        }

        [TestMethod]
        public void SerializationWithSpecificProvider()
        {
            MockApplicationSerializationConfiguration config = new MockApplicationSerializationConfiguration();
            config.DefaultSerializationProvider = SerializerType.Binary;
            config.TypeElements.Add(new MockTypeElement()
                    {
                        Type = "Tavisca.Frameworks.Serialization.Tests.Data.Airport",
                        SerializationProvider = SerializerType.ProtoBuf,
                        CompressionOptions = CompressionTypeOptions.Deflate,
                        LookupAssemblies = new List<IAssemblyElement>()
                        {
                            new MockAssemblyElement(){ Name = "Tavisca.Frameworks.Serialization.Tests.Data" }
                        }
                    });
            // Even if the default SerializationProvider is Binary, still ProtoBuf serializer is used because for the Airport type ProtoBuf SerializationProvider
            // is specified in the config
            SerializationConfigurationManager.SetConfiguration(config);

            var expected = new Airport()
            {
                AirportCode = "IGI",
                AirportName = "Indira Gandhi International Airport",
                Terminal = new Terminal() { TerminalId = 12345, TerminalName = "T1" }
            };

            var serializer = new SerializationFactory().GetSerializationFacade();
            var serializedData = serializer.Serialize(expected);

            var deserializer = new SerializationFactory().GetSerializationFacade(SerializerType.ProtoBuf);
            var actual = deserializer.Deserialize<Airport>(serializedData);

            Assert.IsNotNull(actual);
            Assert.IsTrue(expected.Equals(actual));
        }

        [TestMethod]
        public void SerializationWithDifferentSerializersInConfig()
        {
            MockApplicationSerializationConfiguration config = new MockApplicationSerializationConfiguration();
            config.DefaultSerializationProvider = SerializerType.Binary;
            config.TypeElements.Add(new MockTypeElement()
            {
                Type = "Tavisca.Frameworks.Serialization.Tests.Data.Airport",
                SerializationProvider = SerializerType.ProtoBuf,
                CompressionOptions = CompressionTypeOptions.Deflate,
                LookupAssemblies = new List<IAssemblyElement>()
                        {
                            new MockAssemblyElement(){ Name = "Tavisca.Frameworks.Serialization.Tests.Data" }
                        }
            });

            config.TypeElements.Add(new MockTypeElement()
            {
                Type = "Tavisca.Frameworks.Serialization.Tests.Data.Terminal",
                SerializationProvider = SerializerType.Binary,
                CompressionOptions = CompressionTypeOptions.Deflate,
                LookupAssemblies = new List<IAssemblyElement>()
                        {
                            new MockAssemblyElement(){ Name = "Tavisca.Frameworks.Serialization.Tests.Data" }
                        }
            });

            SerializationConfigurationManager.SetConfiguration(config);

            var serializer = new SerializationFactory().GetSerializationFacade();

            var expected = new Airport()
            {
                AirportCode = "IGI",
                AirportName = "Indira Gandhi International Airport",
                Terminal = new Terminal() { TerminalId = 12345, TerminalName = "T1" }
            };

            var serializedData = serializer.Serialize(expected);

            var deserializer = new SerializationFactory().GetSerializationFacade(SerializerType.ProtoBuf);
            var actual = deserializer.Deserialize<Airport>(serializedData);
            Assert.IsNotNull(actual);
            Assert.IsTrue(expected.Equals(actual));
        }

        [TestMethod]
        public void SerializationOfTypesInMultipleAssemblies()
        {
            MockApplicationSerializationConfiguration config = new MockApplicationSerializationConfiguration();
            config.DefaultSerializationProvider = SerializerType.ProtoBuf;
            config.TypeElements.Add(new MockTypeElement()
            {
                Type = "Tavisca.Frameworks.Serialization.Tests.Data.FlightItinerary",
                SerializationProvider = SerializerType.ProtoBuf,
                CompressionOptions = CompressionTypeOptions.Deflate,
                LookupAssemblies = new List<IAssemblyElement>()
                        {
                            new MockAssemblyElement(){ Name = "Tavisca.Frameworks.Serialization.Tests.Data" }
                        }
            });

            config.TypeElements.Add(new MockTypeElement()
            {
                Type = "Tavisca.Frameworks.Serialization.Tests.Data.Address",
                SerializationProvider = SerializerType.ProtoBuf,
                CompressionOptions = CompressionTypeOptions.Deflate,
                LookupAssemblies = new List<IAssemblyElement>()
                        {
                            new MockAssemblyElement(){ Name = "Tavisca.Frameworks.Serialization.Tests.Data" }
                        }
            });

            SerializationConfigurationManager.SetConfiguration(config);

            var serializer = new SerializationFactory().GetSerializationFacade();

            var expected = FlightItineraryObject.GetFlightItineraryObject(); 

            var serializedData = serializer.Serialize(expected);

            var actual = serializer.Deserialize<FlightItinerary>(serializedData);

            Assert.IsNotNull(actual);
            Assert.IsTrue(expected.Equals(actual));
        }
    }
}