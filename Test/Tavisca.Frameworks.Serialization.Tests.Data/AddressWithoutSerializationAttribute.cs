using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tavisca.Frameworks.Serialization.Tests.Data
{
    public class AddressWithoutSerializationAttribute
    {
        public int ApartmentNumber { get; set; }

        public string StreetName { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public override bool Equals(object obj)
        {
            AddressWithoutSerializationAttribute address = obj as AddressWithoutSerializationAttribute;
            if (address != null && this.ApartmentNumber == address.ApartmentNumber && this.StreetName == address.StreetName && this.City == address.City && this.Country == address.Country)
                return true;
            else
                return false;
        }
    }
}
