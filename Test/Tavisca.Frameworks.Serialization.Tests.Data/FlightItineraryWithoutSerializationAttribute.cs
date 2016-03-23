using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavisca.Frameworks.Serialization.Tests.Data
{
    public class FlightItineraryWithoutSerializationAttribute
    {
        public string DepartureAirport { get; set; }

        public string ArrivalAirport { get; set; }

        public int NumberOfPassengers { get; set; }

        public List<PassengerWithoutSerializationAttribute> PassengersInformation { get; set; }

        public override bool Equals(object obj)
        {
            FlightItineraryWithoutSerializationAttribute itinerary = obj as FlightItineraryWithoutSerializationAttribute;
            if (itinerary != null && this.DepartureAirport == itinerary.DepartureAirport && this.ArrivalAirport == itinerary.ArrivalAirport && this.NumberOfPassengers == itinerary.NumberOfPassengers
                && this.PassengersInformation.SequenceEqual(itinerary.PassengersInformation))
                return true;
            else
                return false;
        }
    }

    public enum FlightPassengerTypeWithoutAttribute
    {
        Adult,
        Child
    }
}
