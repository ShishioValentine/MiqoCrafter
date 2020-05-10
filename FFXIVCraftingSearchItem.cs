using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiqoCraft
{
    public class FFXIVCraftingSearchItem : FFXIVCraftedItem
    {
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
            return Name + " - " + Class + " - Level " + Level;
        }
    }
}
