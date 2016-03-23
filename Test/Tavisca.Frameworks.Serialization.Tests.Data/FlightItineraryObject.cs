using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavisca.Frameworks.Serialization.Tests.Data;

namespace Tavisca.Frameworks.Serialization.Tests.Data
{
    public class FlightItineraryObject
    {
        public static FlightItinerary GetFlightItineraryObject()
        {
            FlightItinerary flightItinerary = new FlightItinerary()
            {
                DepartureAirport = "Los Angeles",
                ArrivalAirport = "New York",
                NumberOfPassengers = 2,
                PassengersInformation = new List<Passenger>()
            };
            flightItinerary.PassengersInformation.Add(new Passenger()
            {
                Name = "John",
                Age = 20,
                Address = new Address() { ApartmentNumber = 5, StreetName = "Street", City = "Pune", Country = "India" },
                PassengerType = PassengerType.Adult
            });
            flightItinerary.PassengersInformation.Add(new Passenger()
            {
                Name = "Marie",
                Age = 10,
                Address = new Address() { ApartmentNumber = 5, StreetName = "Street", City = "Pune", Country = "India" },
                PassengerType = PassengerType.Child
            });

            return flightItinerary;
        }

        public static FlightItineraryWithoutSerializationAttribute GetFlightItineraryObjectWithoutSerializationAttribute()
        {
            FlightItineraryWithoutSerializationAttribute flightItinerary = new FlightItineraryWithoutSerializationAttribute()
            {
                DepartureAirport = "Los Angeles",
                ArrivalAirport = "New York",
                NumberOfPassengers = 2,
                PassengersInformation = new List<PassengerWithoutSerializationAttribute>()
            };
            flightItinerary.PassengersInformation.Add(new PassengerWithoutSerializationAttribute()
            {
                Name = "John",
                Age = 20,
                Address = new AddressWithoutSerializationAttribute() { ApartmentNumber = 5, StreetName = "Street", City = "Pune", Country = "India" },
                PassengerType = FlightPassengerTypeWithoutAttribute.Adult
            });
            flightItinerary.PassengersInformation.Add(new PassengerWithoutSerializationAttribute()
            {
                Name = "Marie",
                Age = 10,
                Address = new AddressWithoutSerializationAttribute() { ApartmentNumber = 5, StreetName = "Street", City = "Pune", Country = "India" },
                PassengerType = FlightPassengerTypeWithoutAttribute.Child
            });

            return flightItinerary;
        }
    }
}
