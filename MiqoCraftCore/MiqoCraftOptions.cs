﻿using System.Collections.Generic;
using VPL.Application.Data;

namespace MiqoCraftCore
{
    public class MiqoCraftOptions : VPOptions
    {
        public bool Collectable = false;
        public MiqoCraftCore.RepairMode RepairMoveValue = MiqoCraftCore.RepairMode.None;
        public bool QuickCraft = false;
        public bool IgnoreCatalysts = false;
        public string GatheringRotation = "HQ +10%";
        public string CraftPreset = "recommended";
        public string NQHQPreset = "balanced";
        public string CustomTeleport = "";
        public int NbPerNode = 1;
        public List<FFXIVSearchItem> LastSearchResult = new List<FFXIVSearchItem>();
        public List<FFXIVCraftingOptions> ListItemOptions = new List<FFXIVCraftingOptions>();
        public string MiqoPresetPath = "";
        public List<string> ListGatherableItems = new List<string>();
        public List<FFXIVSearchItem> ListCraftableItems = new List<FFXIVSearchItem>();
        public List<string> ListGridOKItems = new List<string>();

        protected override void CustomLoad(VPOptions xmlOptions)
        {
            if (null == xmlOptions) return;
            if (!(xmlOptions is MiqoCraftOptions)) return;

            MiqoCraftOptions options = xmlOptions as MiqoCraftOptions;
            if (null == options) return;

            Collectable = options.Collectable;
            RepairMoveValue = options.RepairMoveValue;
            IgnoreCatalysts = options.IgnoreCatalysts;
            QuickCraft = options.QuickCraft;
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
