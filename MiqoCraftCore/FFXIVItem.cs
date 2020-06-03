using System;

namespace MiqoCraftCore
{
    public class FFXIVItem : IComparable
    {
        /// <summary>
        /// Item Name
        /// </summary>
        public string Name = "Unkown Item";

        /// <summary>
        /// Item URL on GarlandTool
        /// </summary>
        public string UrlGarland = "";

        /// <summary>
        /// Item Image URL
        /// </summary>
        public string UrlImage = "";

        /// <summary>
        /// Item ID
        /// </summary>
        public string ID = "";

        /// <summary>
        /// Item quantity
        /// </summary>
        private int _quantity = 1;

        public enum TypeItem
        {
            Unkwown,
            Crafted,
            Gathered,
            Reduced,
            NPC
        }

        /// <summary>
        /// Item type
        /// </summary>
        public TypeItem Type = TypeItem.Unkwown;

        /// <summary>
        /// Defines the items quantity for a recipe.
        /// Does not take into account custom quantities, it refers to the standard recipe quantity
        /// </summary>
        public int Quantity { get => _quantity; set => _quantity = value; }

        public int CompareTo(object obj)
        {
            if (null != obj && (obj is FFXIVSearchItem))
            {
                FFXIVSearchItem other = obj as FFXIVSearchItem;
                if (null != other) return Name.CompareTo(other.Name);
            }
            return Name.CompareTo(obj.ToString());
        }

        /// <summary>
        /// Display this item info
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Name + " [" + Type.ToString() + "]";
        }
    }
}
