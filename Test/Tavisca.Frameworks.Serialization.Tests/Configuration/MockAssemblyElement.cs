using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tavisca.Frameworks.Serialization.Configuration;

namespace Tavisca.Frameworks.Serialization.Tests.Configuration
{
    public class MockAssemblyElement : IAssemblyElement
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}