using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace Tavisca.Frameworks.Serialization.Tests.Data
{
    [Serializable]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class Airport
    {
        public string AirportName { get; set; }

        public string AirportCode { get; set; }

        public Terminal Terminal { get; set; }

        public override bool Equals(object obj)
        {
            Airport airport = obj as Airport;

            if (airport != null && this.AirportName == airport.AirportName && this.AirportCode == airport.AirportCode && this.Terminal.Equals(airport.Terminal))
                return true;
            else
                return false;
        }
    }
}
