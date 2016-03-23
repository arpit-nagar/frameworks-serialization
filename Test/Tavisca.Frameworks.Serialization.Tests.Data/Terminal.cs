using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ProtoBuf;

namespace Tavisca.Frameworks.Serialization.Tests.Data
{
    [Serializable]
    [ProtoContract(ImplicitFields=ImplicitFields.AllFields)]
    public class Terminal
    {
        public string TerminalName { get; set; }

        public int TerminalId { get; set; }

        public override bool Equals(object obj)
        {
            Terminal terminal = obj as Terminal;
            if (terminal != null && this.TerminalName == terminal.TerminalName && this.TerminalId == terminal.TerminalId)
                return true;
            else
                return false;
        }
    }
}
