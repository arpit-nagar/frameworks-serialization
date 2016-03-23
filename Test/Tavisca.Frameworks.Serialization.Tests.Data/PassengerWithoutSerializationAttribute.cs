using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavisca.Frameworks.Serialization.Tests.Data
{
    public class PassengerWithoutSerializationAttribute
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public AddressWithoutSerializationAttribute Address { get; set; }

        public FlightPassengerTypeWithoutAttribute PassengerType { get; set; }

        public override bool Equals(object obj)
        {
            PassengerWithoutSerializationAttribute pas = obj as PassengerWithoutSerializationAttribute;
            if (pas != null && this.Name == pas.Name && this.Age == pas.Age && this.Address.Equals(pas.Address) && this.PassengerType == pas.PassengerType)
                return true;
            else
                return false;
        }
    }
}
