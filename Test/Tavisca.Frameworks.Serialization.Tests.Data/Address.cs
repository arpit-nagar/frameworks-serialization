using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavisca.Frameworks.Serialization.Tests.Data
{
    [Serializable]
    [ProtoContract(ImplicitFields=ImplicitFields.AllFields)]
    public class Address
    {
        public int ApartmentNumber { get; set; }

        public string StreetName { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public override bool Equals(object obj)
        {
            Address address = obj as Address;
            if (address != null && this.ApartmentNumber == address.ApartmentNumber && this.StreetName == address.StreetName && this.City == address.City && this.Country == address.Country)
                return true;
            else
                return false;
        }
    }
}
