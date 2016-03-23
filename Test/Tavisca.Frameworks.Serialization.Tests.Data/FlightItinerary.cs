using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace Tavisca.Frameworks.Serialization.Tests.Data
{
    [Serializable]
    [ProtoContract(ImplicitFields=ImplicitFields.AllFields)]
    public class FlightItinerary
    {
        public string DepartureAirport { get; set; }

        public string ArrivalAirport { get; set; }

        public int NumberOfPassengers { get; set; }

        public List<Passenger> PassengersInformation { get; set; }

        public override bool Equals(object obj)
        {
            FlightItinerary itinerary = obj as FlightItinerary;
            if (itinerary != null && this.DepartureAirport == itinerary.DepartureAirport && this.ArrivalAirport == itinerary.ArrivalAirport && this.NumberOfPassengers == itinerary.NumberOfPassengers
                && this.PassengersInformation.SequenceEqual(itinerary.PassengersInformation))
                return true;
            else
                return false;
        }
    }

    [Serializable]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public enum PassengerType
    {
        Adult,
        Child
    }
}
