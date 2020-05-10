using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiqoCraft
{
    public class FFXIVCraftingOptions
    {
        public string CustomCraftingMacro = "";
        public bool IgnoreItem = false;
        public string ItemID = "";

        public override string ToString()
        {
            string result = "";

            if (IgnoreItem) result += "Ignored";
            else
            {
                if(CustomCraftingMacro != "")
                {
                    if (result != "") result += ";";
                    result += "Crafting Macro : " + CustomCraftingMacro + ".";
                }
            }

            //result += Environment.NewLine + "Double click on item name to edit options.";

            return result;
        }
    }
}
