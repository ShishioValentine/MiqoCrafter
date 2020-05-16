using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiqoCraftCore
{
    public class FFXIVAetheryte
    {
        public FFXIVPosition Position;

        public string Region = "Unknown";
        public string Zone = "Unknown";
        public string Name = "Unknown";

        public override string ToString()
        {
            string position = "unknown";
            if (null != Position) position = Position.ToString();
            return Region + " - " + Zone + " - " + Name + " - " + position;
        }
    }
}
