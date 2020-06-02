using MiqoCraftCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using VPL.Application.Data;
using VPL.Threading.Modeler;

namespace MiqoCraft
{
    public partial class ScenarioForm : Form
    {

        /// <summary>
        /// Craft item thread
        /// </summary>
        Thread _craftThread = null;

        /// <summary>
        /// Updates option display
        /// </summary>
        Thread _optionThread = null;

        /// <summary>
        /// Generate script thread
        /// </summary>
        Thread _generateThread = null;

        /// <summary>
        /// Display list ingredient thread
        /// </summary>
        Thread _displayThread = null;

        /// <summary>
        /// List of items to craft, from the search
        /// </summary>
        List<FFXIVSearchItem> _listItemsToCraft = null;

        Dictionary<string, int> _itemsQuantity = new Dictionary<string, int>();

        /// <summary>
        /// List of items to craft, from the search
        /// </summary>
        public List<FFXIVSearchItem> ListItemsToCraft
        {
            get => _listItemsToCraft;
            set
            {
                _listItemsToCraft = value;

                SetProgressStatus(-1, "Defining list of items...");

                //Update display
                VPThreading.Abort(_craftThread);
                ThreadStart starter = new ThreadStart(BuildCraftingTreeThread);
                _craftThread = new Thread(starter);
                _craftThread.Start();
            }
        }

        /// <summary>
        /// List of final items to craft, including crafting tree
        /// </summary>
        List<FFXIVItem> _listResultItems = null;

        /// <summary>
        /// List of final items to craft, including crafting tree
        /// </summary>
        public List<FFXIVItem> ListResultItems { get => _listResultItems; set => _listResultItems = value; }

        public ScenarioForm()
        {
            InitializeComponent();

            //Load default options
            MiqoCraftOptions options = new MiqoCraftOptions();
            options.Load(OptionLocation.UserOption);

            _ignoreShardCheckBox.Checked = options.IgnoreCatalysts;
            _rotationTextBox.Text = options.GatheringRotation;
            _craftingpresetTextBox.Text = options.CraftPreset;
            _nghqTextBox.Text = options.NQHQPreset;
            _teleportTextBox.Text = options.CustomTeleport;
            _collectableCheckBox.Checked = options.Collectable;
            _RMenderEulmoreCheckBox.Checked = options.RMenderEulmore;
            _quantityPerNodeNumericUpDown.Value = options.NbPerNode;
            _miqoPathTextBox.Text = options.MiqoPresetPath;
        }

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {

            VPThreading.Abort(_craftThread);
            VPThreading.Abort(_generateThread);
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        /// <summary>
        /// Start updtae options display thread
        /// </summary>
        public void UpdateOptionsInfo()
        {
            VPThreading.Abort(_optionThread);
            ThreadStart starter = new ThreadStart(UpdateOptionsThread);
            _optionThread = new Thread(starter);
            _optionThread.Start();
        }

        /// <summary>
        /// Updates option info on list view items
        /// </summary>
        private void UpdateOptionsThread()
        {
            try
            {
                MiqoCraftOptions options = new MiqoCraftOptions();
                options.Load(OptionLocation.UserOption);

                List<ListViewItem> listItems = VPThreading.GetItems(_ingredientsListView);
                foreach (ListViewItem listViewItem in listItems)
                {
                    if (null == listViewItem) continue;
                    if (null == listViewItem.Tag) continue;
                    if (!(listViewItem.Tag is FFXIVItem)) continue;

                    FFXIVItem item = listViewItem.Tag as FFXIVItem;
                    if (null == item) continue;

                    VPThreading.SetText(listViewItem, VPThreading.GetText(listViewItem).Split('{')[0]);
                    FFXIVCraftingOptions itemOption = options.GetOption(item);
                    if (null != itemOption)
                    {
                        string detailsOption = itemOption.ToString();
                        if (detailsOption != "") VPThreading.SetText(listViewItem, VPThreading.GetText(listViewItem).Split('{')[0] + Environment.NewLine + "{" + detailsOption + "}");
                    }
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// Search an element on FFXIVCrafting
        /// </summary>
        private void BuildCraftingTreeThread()
        {
            try
            {
                SetProgressStatus(-1, "Building crafting tree..");

                //GetInfo
                ListResultItems = new List<FFXIVItem>();
                for (int i = 0; i < ListItemsToCraft.Count; i++)
                {
                    FFXIVSearchItem itemToCraft = ListItemsToCraft[i];
                    if (null == itemToCraft) continue;

                    if (i == 0)
                    {
                        SetProgressStatus(-1, "Building " + itemToCraft.Name + " crafting tree..");
                    }
                    else
                    {
                        SetProgressStatus((i + 1) * 100 / ListItemsToCraft.Count, "Building " + itemToCraft.Name + " crafting tree..");
                    }

                    FFXIVItem item = GarlandTool.RecBuildCraftingTree(null, itemToCraft.ID, itemToCraft.Quantity);
                    if (null == item) continue;
                    //item.Quantity = itemToCraft.Quantity;
                    if (!_itemsQuantity.ContainsKey(item.ID)) _itemsQuantity.Add(item.ID, itemToCraft.Quantity);

                    ListResultItems.Add(item);
                }
                UpdateList();
            }
            catch
            { }
        }

        private void UpdateList()
        {
            VPThreading.Abort(_displayThread);
            ThreadStart starter = new ThreadStart(DisplayListResultThread);
            _displayThread = new Thread(starter);
            _displayThread.Start();
        }

        private void DisplayListResultThread()
        {
            try
            {
                //Display
                VPThreading.ClearItems(_ingredientsListView);
                SetProgressStatus(-1, "Updating Quantities..");
                List<FFXIVItem> allItems = new List<FFXIVItem>();
                List<int> allItemsQuantity = new List<int>();
                for (int i = 0; i < ListResultItems.Count; i++)
                {
                    FFXIVItem item = ListResultItems[i];
                    if (null == item) continue;

                    SetProgressStatus(i * 100 / ListItemsToCraft.Count, "Computing " + item.Name + " quantity..");

                    MiqoCraftCore.MiqoCraftCore.RecFindItems(item, GetItemQuantity(item, 1), ref allItems, ref allItemsQuantity, _itemsQuantity);
                }

                SetProgressStatus(-1, "Displaying Items..");
                for (int i = 0; i < allItems.Count && i < allItemsQuantity.Count; i++)
                {
                    FFXIVItem iItem = allItems[i];
                    if (null == iItem) return;
                    int quantity = allItemsQuantity[i];

                    if (_itemsQuantity.ContainsKey(iItem.ID)) quantity = _itemsQuantity[iItem.ID];

                    ListViewItem listViewItem = new ListViewItem();
                    listViewItem.Tag = iItem;
                    listViewItem.Text = quantity + "x " + iItem.Name;
                    ListViewGroup group = GetItemGroup(iItem);


                    SetProgressStatus(i * 100 / ListItemsToCraft.Count, "Displaying " + iItem.Name + "..");

                    FFXIVCraftedItem craftedItem = null;
                    if (iItem is FFXIVCraftedItem)
                    {
                        craftedItem = iItem as FFXIVCraftedItem;
                    }
                    if (null != craftedItem)
                    {
                        if (craftedItem.RecipeQuantity == 1) listViewItem.Text = "[" + craftedItem.Class + " lvl" + craftedItem.Level + "]" + Environment.NewLine + listViewItem.Text;
                        else listViewItem.Text = "[" + craftedItem.Class + " lvl" + craftedItem.Level + "]" + Environment.NewLine + listViewItem.Text + "[x" + craftedItem.RecipeQuantity + "]";

                        listViewItem.ToolTipText = craftedItem.Class + " lvl" + craftedItem.Level;
                    }

                    listViewItem.SubItems.Add((quantity).ToString());

                    try
                    {
                        if (!_prerequisiteImageList.Images.ContainsKey(iItem.ID))
                        {
                            System.Net.WebRequest request = System.Net.WebRequest.Create(iItem.UrlImage);
                            System.Net.WebResponse resp = request.GetResponse();
                            System.IO.Stream respStream = resp.GetResponseStream();
                            Bitmap bmp = new Bitmap(respStream);
                            respStream.Dispose();

                            VPThreading.AddImage(_ingredientsListView, _prerequisiteImageList, iItem.ID, bmp);
                        }
                        listViewItem.ImageKey = iItem.ID;
                    }
                    catch (Exception)
                    {
                    }

                    listViewItem.Group = group;

                    VPThreading.AddItem(_ingredientsListView, listViewItem);

                }

            }
            catch (Exception)
            {

            }
            UpdateOptionsThread();
            SetProgressStatus(100, "Done. You can generate the scenario.");
        }

        /// <summary>
        /// Action on activate event on item list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _ingredientsListView_ItemActivate(object sender, EventArgs e)
        {
            if (_ingredientsListView.SelectedItems.Count != 1) return;
            ListViewItem currentItem = _ingredientsListView.SelectedItems[0];
            if (null == currentItem || null == currentItem.Tag) return;

            try
            {
                FFXIVItem item = currentItem.Tag as FFXIVItem;
                System.Diagnostics.Process.Start(item.UrlGarland);
            }
            catch
            {

            }
        }

        public void SaveOptions()
        {
            MiqoCraftOptions options = new MiqoCraftOptions();
            options.Load(OptionLocation.UserOption);

            options.IgnoreCatalysts = _ignoreShardCheckBox.Checked;
            options.GatheringRotation = _rotationTextBox.Text;
            options.CraftPreset = _craftingpresetTextBox.Text;
            options.NQHQPreset = _nghqTextBox.Text;
            options.CustomTeleport = _teleportTextBox.Text;
            options.Collectable = VPThreading.GetChecked(_collectableCheckBox);
            options.RMenderEulmore = VPThreading.GetChecked(_RMenderEulmoreCheckBox);
            options.NbPerNode = (int)VPThreading.GetValue(_quantityPerNodeNumericUpDown);
            options.MiqoPresetPath = VPThreading.GetText(_miqoPathTextBox);
            options.Save();
        }

        /// <summary>
        /// Generate miqobot script
        /// </summary>
        private void GenerateThread()
        {
            //Saving options
            {
                SaveOptions();
            }

            //Generating
            {
                SetProgressStatus(-1, "Generating Scenario");
                MiqoCraftCore.MiqoCraftCore.MiqobotScenarioOption options = new MiqoCraftCore.MiqoCraftCore.MiqobotScenarioOption();
                options.GatheringRotation = VPThreading.GetText(_rotationTextBox);
                options.CraftPreset = VPThreading.GetText(_craftingpresetTextBox);
                options.NQHQPreset = VPThreading.GetText(_nghqTextBox);
                options.CustomTeleport = VPThreading.GetText(_teleportTextBox);
                options.IgnoreCatalysts = VPThreading.GetChecked(_ignoreShardCheckBox);
                options.Collectable = VPThreading.GetChecked(_collectableCheckBox);
                options.RMenderEulmore = VPThreading.GetChecked(_RMenderEulmoreCheckBox);
                options.NbPerNode = (int)VPThreading.GetValue(_quantityPerNodeNumericUpDown);
                options.MiqoPresetPath = VPThreading.GetText(_miqoPathTextBox);
                options.CustomQuantities = _itemsQuantity;

                string fullScenario = "";
                string textFileContent = MiqoCraftCore.MiqoCraftCore.GenerateScenario(ListResultItems,
                    options,
                    null,
                    out fullScenario);

                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Title = "Save Miqobot Scenario";
                dialog.Filter = "Miqobot Scenario|*.txt";
                if (ListResultItems.Count > 0 && null != ListResultItems[0]) dialog.FileName = ListResultItems[0].Name;

                if (dialog.ShowDialog() == DialogResult.OK || dialog.ShowDialog() == DialogResult.Yes)
                {
                    System.IO.File.WriteAllText(dialog.FileName, textFileContent);
                }

                Clipboard.SetText(fullScenario);
                SetProgressStatus(100, "Done");
            }
        }

        /// <summary>
        /// Sets the dialog progress status
        /// </summary>
        /// <param name="iProgress"></param>
        /// <param name="iStatus"></param>
        private void SetProgressStatus(int iProgress, string iStatus)
        {
            VPThreading.SetText(_statusLabel, iStatus);
            if (iProgress >= 0) VPThreading.SetProgress(_progressBar, iProgress);
            else VPThreading.SetProgressUnknown(_progressBar);
        }

        /// <summary>
        /// Retrieve an item quantity
        /// </summary>
        /// <param name="iItem"></param>
        /// <param name="iDefaultValue"></param>
        /// <returns></returns>
        private int GetItemQuantity(FFXIVItem iItem, int iDefaultValue)
        {
            if (null != iItem && _itemsQuantity.ContainsKey(iItem.ID))
            {
                return _itemsQuantity[iItem.ID];
            }
            //if (null != iItem && null != ListResultItems.Find(x => x != null && x.ID == iItem.ID))
            //{
            //    return iItem.Quantity;
            //}
            return iDefaultValue;
        }

        /// <summary>
        /// Retrieve an item group
        /// </summary>
        /// <param name="iItem"></param>
        /// <returns></returns>
        private ListViewGroup GetItemGroup(FFXIVItem iItem)
        {
            ListViewGroup resultGroup = null;


            ListViewGroupCollection groups = VPThreading.GetGroups(_ingredientsListView);
            string groupName = "Bought/Others";
            string level = "1";

            FFXIVCraftedItem craftedItem = null;
            if (iItem is FFXIVCraftedItem)
            {
                craftedItem = iItem as FFXIVCraftedItem;
            }
            FFXIVReducedItem reducedItem = null;
            if (iItem is FFXIVReducedItem)
            {
                reducedItem = iItem as FFXIVReducedItem;
            }
            FFXIVGatheredItem gatheredItem = null;
            if (iItem is FFXIVGatheredItem)
            {
                gatheredItem = iItem as FFXIVGatheredItem;
            }

            List<string> catalysts = MiqoCraftCore.MiqoCraftCore.GetCatalysts();
            List<UnspoiledNodes> AllUnspoiledNodes = MiqoCraftCore.MiqoCraftCore.GetAllUnspoiledNodes();

            if (null != catalysts.Find(x => x != null && x.ToLower() == iItem.Name.ToLower()))
            {
                groupName = "Catalysts";
            }
            else if (null != craftedItem)
            {
                //groupName = craftedItem.Class;
                groupName = "Crafted";
                level = craftedItem.Level;
            }
            else if (null != reducedItem)
            {
                groupName = "Reduced";
            }
            else if (null != gatheredItem)
            {
                if (MiqoCraftCore.MiqoCraftCore.IsUnspoiledNode(gatheredItem.Name, AllUnspoiledNodes) != null)
                {
                    groupName = "Gathered Unspoiled Nodes";
                    if (!MiqoCraftCore.MiqoCraftCore.HasGrid(gatheredItem.Name))
                    {
                        groupName = "Unspoiled Nodes - No Grid";
                    }
                }
                else
                {
                    groupName = "Gathered";
                    if (!MiqoCraftCore.MiqoCraftCore.HasGrid(gatheredItem.Name))
                    {
                        groupName = "Gathered - No Grid";
                    }
                }

            }

            foreach (ListViewGroup group in groups)
            {
                if (group.Name == groupName)
                {
                    return group;
                }
            }

            resultGroup = new ListViewGroup();
            resultGroup.Name = groupName;
            resultGroup.Header = groupName;
            VPThreading.AddGroup(_ingredientsListView, resultGroup);

            return resultGroup;
        }

        private void _generateButton_Click(object sender, EventArgs e)
        {
            VPThreading.Abort(_generateThread);
            ThreadStart starter = new ThreadStart(GenerateThread);
            _generateThread = new Thread(starter);
            _generateThread.SetApartmentState(ApartmentState.STA);
            _generateThread.Start();
        }

        private void _ingredientsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            _nbSelectedItemLabel.Text = _ingredientsListView.SelectedItems.Count + " Items selected : ";

            FFXIVItem firstItem = null;
            if (_ingredientsListView.SelectedItems.Count > 0)
            {
                try
                {
                    firstItem = _ingredientsListView.SelectedItems[0].Tag as FFXIVItem;
                }
                catch
                {

                }
            }

            if (null == firstItem)
            {
                _quantityNumericUpDown.Enabled = false;
                //_ignoreCheckBox.Enabled = false;
                _craftTextBox.Enabled = false;
                return;
            }
            else
            {
                _quantityNumericUpDown.Enabled = true;
                //_ignoreCheckBox.Enabled = true;
                _craftTextBox.Enabled = true;
            }

            try
            {
                //Options
                MiqoCraftOptions options = new MiqoCraftOptions();
                options.Load(OptionLocation.UserOption);

                FFXIVCraftingOptions itemOption = options.GetOption(firstItem);
                if (null != itemOption)
                {
                    VPThreading.SetText(_craftTextBox, itemOption.CustomCraftingMacro);
                    //_ignoreCheckBox.Checked = itemOption.IgnoreItem;
                }

                //Quantity
                _quantityNumericUpDown.Value = (decimal)GetItemQuantity(firstItem, 1);
            }
            catch
            {

            }
        }

        private void _quantityNumericUpDown_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ListView.SelectedListViewItemCollection iItems = _ingredientsListView.SelectedItems;

                foreach (ListViewItem listViewItem in iItems)
                {
                    try
                    {
                        FFXIVItem item = VPThreading.GetTag(listViewItem) as FFXIVItem;
                        if (null != item)
                        {
                            if (!_itemsQuantity.ContainsKey(item.ID)) _itemsQuantity.Add(item.ID, (int)_quantityNumericUpDown.Value);
                            else _itemsQuantity[item.ID] = (int)_quantityNumericUpDown.Value;
                        }
                    }
                    catch
                    {

                    }
                }
                UpdateList();
            }
        }

        private void _ignoreCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MiqoCraftOptions options = new MiqoCraftOptions();
            options.Load(OptionLocation.UserOption);

            ListView.SelectedListViewItemCollection iItems = _ingredientsListView.SelectedItems;

            foreach (ListViewItem listViewItem in iItems)
            {
                try
                {
                    FFXIVItem item = VPThreading.GetTag(listViewItem) as FFXIVItem;
                    if (null != item)
                    {
                        FFXIVCraftingOptions Options = new FFXIVCraftingOptions();
                        Options.ItemID = item.ID;
                        //Options.IgnoreItem = _ignoreCheckBox.Checked;
                        Options.CustomCraftingMacro = _craftTextBox.Text;

                        FFXIVCraftingOptions existingOption = options.GetOption(Options.ItemID);
                        while (null != existingOption)
                        {
                            options.ListItemOptions.Remove(existingOption);
                            existingOption = options.GetOption(Options.ItemID);
                        }
                        options.ListItemOptions.Add(Options);
                    }
                }
                catch
                {

                }
            }

            options.Save();

            UpdateOptionsInfo();
        }

        private void _craftTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _ignoreCheckBox_CheckedChanged(sender, e);
            }
        }

        private void _validQuantityButton_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection iItems = _ingredientsListView.SelectedItems;

            foreach (ListViewItem listViewItem in iItems)
            {
                try
                {
                    FFXIVItem item = VPThreading.GetTag(listViewItem) as FFXIVItem;
                    if (null != item)
                    {
                        if (!_itemsQuantity.ContainsKey(item.ID)) _itemsQuantity.Add(item.ID, (int)_quantityNumericUpDown.Value);
                        else _itemsQuantity[item.ID] = (int)_quantityNumericUpDown.Value;
                    }
                }
                catch
                {

                }
            }
            UpdateList();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
