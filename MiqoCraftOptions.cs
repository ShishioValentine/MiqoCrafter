using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPL.Application.Data;

namespace MiqoCraft
{
    public class MiqoCraftOptions : VPOptions
    {
        public bool Collectable = false;
        public bool IgnoreCatalysts = false;
        public string GatheringRotation = "HQ +10%";
        public string CraftPreset = "recommended";
        public string NQHQPreset = "balanced";
        public string CustomTeleport = "";
        public int NbPerNode = 1;
        public List<FFXIVCraftingSearchItem> LastSearchResult = new List<FFXIVCraftingSearchItem>();
        public List<FFXIVCraftingOptions> ListItemOptions = new List<FFXIVCraftingOptions>();
        public string MiqoPresetPath = "";
        public List<string> ListGatherableItems = new List<string>();
        public List<FFXIVCraftingSearchItem> ListCraftableItems = new List<FFXIVCraftingSearchItem>();
        public List<string> ListGridOKItems = new List<string>();

        protected override void CustomLoad(VPOptions xmlOptions)
        {
            if (null == xmlOptions) return;
            if (!(xmlOptions is MiqoCraftOptions)) return;

            MiqoCraftOptions options = xmlOptions as MiqoCraftOptions;
            if (null == options) return;

            Collectable = options.Collectable;
            IgnoreCatalysts = options.IgnoreCatalysts;
            GatheringRotation = options.GatheringRotation;
            CraftPreset = options.CraftPreset;
            NQHQPreset = options.NQHQPreset;
            CustomTeleport = options.CustomTeleport;
            NbPerNode = options.NbPerNode;
            LastSearchResult = options.LastSearchResult;
            ListItemOptions = options.ListItemOptions;
            MiqoPresetPath = options.MiqoPresetPath;
            ListGatherableItems = options.ListGatherableItems;
            ListGridOKItems = options.ListGridOKItems;
            ListCraftableItems = options.ListCraftableItems;
        }

        /// <summary>
        /// Retrieve an item option
        /// </summary>
        /// <param name="iItem"></param>
        /// <returns></returns>
        public FFXIVCraftingOptions GetOption(FFXIVItem iItem)
        {
            return GetOption(iItem.ID);
        }

        /// <summary>
        /// Retrieve an item option
        /// </summary>
        /// <param name="iItem"></param>
        /// <returns></returns>
        public FFXIVCraftingOptions GetOption(string iItemID)
        {
            foreach (FFXIVCraftingOptions option in ListItemOptions)
            {
                if (null != option && option.ItemID == iItemID)
                {
                    return option;
                }
            }
            return null;
        }
    }
}
