using System.Collections.Generic;

namespace MiqoCraftCore
{
    public class FFXIVGatheredItem : FFXIVItem
    {
        public List<string> Zones = new List<string>();

        public List<string> Slot = new List<string>();

        public List<string> NodeType = new List<string>();

        public List<string> Times = new List<string>();

        public List<string> Types = new List<string>();

        public List<string> GatheringTypes = new List<string>();

        public bool AsCollectable = false;
    }
}
