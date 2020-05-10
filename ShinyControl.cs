using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VPL.Threading.Modeler;
using System.Threading;
using System.Net;
using VPL.Application.Data;
using System.IO;

namespace MiqoCraft
{
    public partial class ShinyControl : UserControl
    {
        /*
            _gatheredListViewGroup = listViewGroup1;
            _craftedListViewGroup = listViewGroup2;
            _otherListViewGroup = listViewGroup3;
            _npcListViewGroup = listViewGroup4;
            _reducedListViewGroup = listViewGroup5;
*/

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

        FFXIVCraftingSearchItem _item = null;

        FFXIVItem _resultItem = null;

        public TextBox LogTextBox = null;

        public int Quantity = 1;

        public TabPage ItemTabPage;

        public TabControl ItemTabControl;

        public ShinyControl()
        {
            InitializeComponent();

            //Groups
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Gathered", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("Crafted (Prerequisite)", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("Other", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup("Bought", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup5 = new System.Windows.Forms.ListViewGroup("Reduced (From other item)", System.Windows.Forms.HorizontalAlignment.Left);

            _gatheredListViewGroup = listViewGroup1;
            _craftedListViewGroup = listViewGroup2;
            _otherListViewGroup = listViewGroup3;
            _npcListViewGroup = listViewGroup4;
            _reducedListViewGroup = listViewGroup5;

            listViewGroup1.Header = "Gathered";
            listViewGroup1.Name = "_gatheredListViewGroup";
            listViewGroup2.Header = "Crafted (Prerequisite)";
            listViewGroup2.Name = "_craftedListViewGroup";
            listViewGroup3.Header = "Other";
            listViewGroup3.Name = "_otherListViewGroup";
            listViewGroup4.Header = "Bought";
            listViewGroup4.Name = "_npcListViewGroup";
            listViewGroup5.Header = "Reduced (From other item)";
            listViewGroup5.Name = "_reducedListViewGroup";
            this._ingredientsListView.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3,
            listViewGroup4,
            listViewGroup5});

            //Saved options
            MiqoCraftOptions options = new MiqoCraftOptions();
            options.Load(OptionLocation.UserOption);

            _ignoreShardCheckBox.Checked = options.IgnoreCatalysts;
            _rotationTextBox.Text = options.GatheringRotation;
            _craftingpresetTextBox.Text = options.CraftPreset;
            _nghqTextBox.Text = options.NQHQPreset;
            _teleportTextBox.Text = options.CustomTeleport;
            _collectableCheckBox.Checked = options.Collectable;
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


        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {

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

                    VPThreading.SetSubItemTextFromTag(_ingredientsListView, "No option set. Double click on item name to set options.", item, 3);

                    FFXIVCraftingOptions itemOption = options.GetOption(item);
                    if (null != itemOption)
                    {
                        VPThreading.SetSubItemTextFromTag(_ingredientsListView, itemOption.ToString(), item, 3);
                    }
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// Sets the associated item and start displaying the crafting list
        /// </summary>
        /// <param name="iItem"></param>
        public void SetItem(FFXIVCraftingSearchItem iItem)
        {
            _item = iItem;
            VPThreading.Abort(_craftThread);
            ThreadStart starter = new ThreadStart(CraftThread);
            _craftThread = new Thread(starter);
            _craftThread.Start();
        }

        /// <summary>
        /// Search an element on FFXIVCrafting
        /// </summary>
        private void CraftThread()
        {
            try
            {
                //GetInfo
                VPThreading.SetTooltip(_toolTip, _jobPictureBox, _item.Class);
                VPThreading.SetText(_shinyLabel, _item.Name);
                VPThreading.SetImageFromURL(_shinyPictureBox, _item.UrlImage);
                VPThreading.SetImageFromURL(_jobPictureBox, _item.UrlClass);
                VPThreading.SetText(_infoLabel, "Loading ingredients...");

                FFXIVItem item = FXIVGarlandTool.RecBuildCraftingTree(LogTextBox, _item.ID);
                Service_Misc.LogText(LogTextBox, "Alright, end of the day !");

                //Display
                VPThreading.ClearItems(_ingredientsListView);
                Service_Misc.LogText(LogTextBox, "Displaying list...");
                _resultItem = item;
                List<FFXIVItem> allItems = new List<FFXIVItem>();
                List<int> allItemsQuantity = new List<int>();
                RecDisplayItems(item, Quantity, ref allItems, ref allItemsQuantity);

                for (int i = 0; i < allItems.Count && i < allItemsQuantity.Count; i++)
                {
                    FFXIVItem iItem = allItems[i];
                    if (null == iItem) return;
                    int quantity = allItemsQuantity[i];
                    ListViewItem listViewItem = new ListViewItem();
                    listViewItem.Tag = iItem;
                    listViewItem.Text = iItem.Name;
                    ListViewGroup group = _npcListViewGroup;

                    FFXIVCraftedItem craftedItem = null;
                    if (iItem is FFXIVCraftedItem)
                    {
                        craftedItem = iItem as FFXIVCraftedItem;
                    }
                    if (null != craftedItem) group = _craftedListViewGroup;

                    FFXIVReducedItem reducedItem = null;
                    if (iItem is FFXIVReducedItem)
                    {
                        reducedItem = iItem as FFXIVReducedItem;
                    }
                    if (null != reducedItem) group = _reducedListViewGroup;

                    FFXIVGatheredItem gatheredItem = null;
                    if (iItem is FFXIVGatheredItem)
                    {
                        gatheredItem = iItem as FFXIVGatheredItem;
                    }
                    if (null != gatheredItem) group = _gatheredListViewGroup;

                    if (null == craftedItem) listViewItem.SubItems.Add("-");
                    else listViewItem.SubItems.Add(craftedItem.Class + " lvl" + craftedItem.Level);

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
                    catch (Exception exc)
                    {
                        Service_Misc.LogText(LogTextBox, "Snap, couldn't get some pictures : " + iItem.Name);
                        Service_Misc.LogText(LogTextBox, exc.Message);
                    }

                    listViewItem.Group = group;

                    VPThreading.AddItem(_ingredientsListView, listViewItem);

                }

                VPThreading.SetText(_infoLabel, _item.Class + " level " + _item.Level);
            }
            catch (Exception exc)
            {

            }
            UpdateOptionsThread();
        }

        /// <summary>
        /// Create list of items to display
        /// </summary>
        /// <param name="iItem"></param>
        /// <param name="iQuantity"></param>
        /// <param name="allItems"></param>
        /// <param name="allItemsQuantity"></param>
        private void RecDisplayItems(FFXIVItem iItem, int iQuantity, ref List<FFXIVItem> allItems, ref List<int> allItemsQuantity)
        {
            Dictionary<string, int> dummyCustomQuantities = new Dictionary<string, int>();
            Miqobot.RecFindItems(iItem, iQuantity, ref allItems, ref allItemsQuantity, dummyCustomQuantities);
        }

        /// <summary>
        /// Generate script button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _generateButton_Click(object sender, EventArgs e)
        {
            if (null == _resultItem)
            {
                Service_Misc.LogText(LogTextBox, "Well I couldn't retrieve the item to craft...");
                return;
            }
            VPThreading.Abort(_generateThread);
            ThreadStart starter = new ThreadStart(GenerateThread);
            _generateThread = new Thread(starter);
            _generateThread.SetApartmentState(ApartmentState.STA);
            _generateThread.Start();
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
                Miqobot.MiqobotScenarioOption options = new Miqobot.MiqobotScenarioOption();
                options.GatheringRotation = VPThreading.GetText(_rotationTextBox);
                options.CraftPreset = VPThreading.GetText(_craftingpresetTextBox);
                options.NQHQPreset = VPThreading.GetText(_nghqTextBox);
                options.CustomTeleport = VPThreading.GetText(_teleportTextBox);
                options.Quantity = Quantity;
                options.IgnoreCatalysts = VPThreading.GetChecked(_ignoreShardCheckBox);
                options.Collectable = VPThreading.GetChecked(_collectableCheckBox);
                options.NbPerNode = (int)VPThreading.GetValue(_quantityPerNodeNumericUpDown);
                options.MiqoPresetPath = VPThreading.GetText(_miqoPathTextBox);

                string fullScenario = "";
                string textFileContent = Miqobot.GenerateScenario(_resultItem,
                    options,
                    LogTextBox,
                    out fullScenario);

                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Title = "Save Miqobot Scenario";
                dialog.Filter = "Miqobot Scenario|*.txt";
                dialog.FileName = _resultItem.Name;

                if (dialog.ShowDialog() == DialogResult.OK || dialog.ShowDialog() == DialogResult.Yes)
                {
                    System.IO.File.WriteAllText(dialog.FileName, textFileContent);
                }

                Clipboard.SetText(fullScenario);
                Service_Misc.LogText(LogTextBox, "All done ! Save file somewhere and time for miqobot to work its magic !");
            }
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
                FFXIVCraftingOptionsForm optionForm = new FFXIVCraftingOptionsForm();
                optionForm.Item = item;
                optionForm.ShowDialog();
                UpdateOptionsInfo();
            }
            catch
            {

            }
        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void _closeButton_Click(object sender, EventArgs e)
        {
            if (null != ItemTabPage && null != ItemTabControl)
            {
                ItemTabControl.TabPages.Remove(ItemTabPage);
            }
        }

        private void _toolTip_Popup(object sender, PopupEventArgs e)
        {

        }

        private void _openMiqoPresetButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Select miqobot installation path";

            if(_miqoPathTextBox.Text != "")
            {
                DirectoryInfo directory = new DirectoryInfo(_miqoPathTextBox.Text);
                if(directory.Exists)
                {
                    dialog.SelectedPath = directory.FullName;
                }
            }

            if(dialog.ShowDialog() == DialogResult.OK)
            {
                _miqoPathTextBox.Text = dialog.SelectedPath;
                SaveOptions();
            }
        }
    }
}
