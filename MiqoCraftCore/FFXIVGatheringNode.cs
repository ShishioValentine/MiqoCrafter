using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiqoCraftCore
{
    public class FFXIVGatheringNode
    {
        public string Name = "";
        public string Zone = "";
        public FFXIVPosition Position;
        public List<string> NodeItems = new List<string>();
        public string JobTrigram = "";

        public enum NodeType
        {
            Standard,
            Unspoiled,
            Ephemeral,
            Folklore
        }

        public NodeType Type = NodeType.Standard;

        public override string ToString()
        {
            return Type.ToString() + " - " + JobTrigram + " - " + Name + " - " + String.Join(", ", NodeItems.ToArray());
        }
    }
}
