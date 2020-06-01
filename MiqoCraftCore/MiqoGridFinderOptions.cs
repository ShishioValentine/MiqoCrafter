using System.Collections.Generic;
using VPL.Application.Data;

namespace MiqoCraftCore
{
    public class MiqoGridFinderOptions : VPOptions
    {
        public List<FFXIVGatheringNode> ListGatheringNodes = new List<FFXIVGatheringNode>();
        public List<FFXIVAetheryte> ListAetherytes = new List<FFXIVAetheryte>();
        public List<FFXIVSearchItem> ListAllGatheredItems = new List<FFXIVSearchItem>();

        protected override void CustomLoad(VPOptions xmlOptions)
        {
            if (null == xmlOptions) return;
            if (!(xmlOptions is MiqoGridFinderOptions)) return;

            MiqoGridFinderOptions options = xmlOptions as MiqoGridFinderOptions;
            if (null == options) return;

            ListGatheringNodes = options.ListGatheringNodes;
            ListAetherytes = options.ListAetherytes;
            ListAllGatheredItems = options.ListAllGatheredItems;
        }
    }
}
