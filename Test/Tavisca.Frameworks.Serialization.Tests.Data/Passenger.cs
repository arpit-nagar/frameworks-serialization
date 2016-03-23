using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavisca.Frameworks.Serialization.Tests.Data;

namespace Tavisca.Frameworks.Serialization.Tests.Data
{
    [Serializable]
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class Passenger
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public Address Address { get; set; }

        public PassengerType PassengerType { get; set; }

        public override bool Equals(object obj)
        {
            Passenger pas = obj as Passenger;
            if (pas != null && this.Name == pas.Name && this.Age == pas.Age && this.Address.Equals(pas.Address) && this.PassengerType == pas.PassengerType)
                return true;
            else
                return false;
        }
    }
}
