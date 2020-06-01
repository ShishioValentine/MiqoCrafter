using System.Collections.Generic;

namespace MiqoCraftCore
{
    public class FFXIVCraftedItem : FFXIVItem
    {
        /// <summary>
        /// List of items needed to craft this item
        /// </summary>
        public List<FFXIVItem> ListNeededItems = new List<FFXIVItem>();

        /// <summary>
        /// Item associated job
        /// </summary>
        public string Class = "";

        /// <summary>
        /// Item associated job image URL
        /// </summary>
        public string UrlClass = "";

        /// <summary>
        /// Item needed level
        /// </summary>
        public string Level = "";

        /// <summary>
        /// Number of item crafted by round
        /// </summary>
        public int RecipeQuantity = 1;

        /// <summary>
        /// Display this item info
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Class == "")
            {
                return Name;
            }
            return Name + " [" + Type.ToString() + "]" + " - " + Class + " - Level " + Level;
        }
    }
}
