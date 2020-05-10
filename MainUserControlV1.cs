using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VPL.Application.Data;
using VPL.Threading.Modeler;

namespace MiqoCraft
{
    public partial class MainUserControlV1 : UserControl
    {
        public MainUserControlV1()
        {
            InitializeComponent();

            Service_Misc.LogText(_logTextBox, "Hey There ! I am MiqoCrafter, nice to met you.");
            Service_Misc.LogText(_logTextBox, "What can I do for you today ?");

            //Loading last search result
            MiqoCraftOptions options = new MiqoCraftOptions();
            options.Load(OptionLocation.UserOption);
            VPThreading.ClearItems(_shinyComboBox);
            foreach (FFXIVCraftingSearchItem item in options.LastSearchResult)
            {
                if (null == item) continue;
                VPThreading.AddItem(_shinyComboBox, item);
            }
            if (_shinyComboBox.Items.Count > 0) _shinyComboBox.SelectedIndex = 0;
        }

        /// <summary>
        /// Search item thread
        /// </summary>
        Thread _searchThread = null;

        TabControl _itemsTabControl = null;

        ShinyControl _currentControl = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            VPThreading.Abort(_searchThread);
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Starts search thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _searchButton_Click(object sender, EventArgs e)
        {
            VPThreading.Abort(_searchThread);
            ThreadStart starter = new ThreadStart(SearchThread);
            _searchThread = new Thread(starter);
            _searchThread.Start();
        }

        /// <summary>
        /// Search an element on FFXIVCrafting
        /// </summary>
        private void SearchThread()
        {
            string elemToSearch = VPThreading.GetText(_searchTextBox);
            Service_Misc.LogText(_logTextBox, "Alright, let's look for " + elemToSearch);
            List<FFXIVCraftingSearchItem> listResults = FXIVGarlandTool.Search(elemToSearch, _logTextBox);

            VPThreading.ClearItems(_shinyComboBox);
            foreach (FFXIVCraftingSearchItem item in listResults)
            {
                if (null == item) continue;
                VPThreading.AddItem(_shinyComboBox, item);
            }
            VPThreading.SetCurrentIndex(_shinyComboBox, 0);

            //Storing in options
            MiqoCraftOptions options = new MiqoCraftOptions();
            options.Load(OptionLocation.UserOption);
            options.LastSearchResult = listResults;
            options.Save();

            if (listResults.Count > 0) Service_Misc.LogText(_logTextBox, "All done ! Found " + listResults.Count + " items!");
            else Service_Misc.LogText(_logTextBox, "All done ! But I couldn't find your item...");
        }

        private void _craftButton_Click(object sender, EventArgs e)
        {
            if (null == _itemsTabControl)
            {
                _itemsTabControl = new TabControl();
                _itemsTabControl.Dock = DockStyle.Fill;
                _itemsTabControl.Name = "ItemsDocControl";

                _statusPanel.Controls.Add(_itemsTabControl);
            }

            object searchItemObj = VPThreading.GetSelectedItem(_shinyComboBox);
            if (null == searchItemObj)
            {
                Service_Misc.LogText(_logTextBox, "Hey, you have to select an item in the combo box first !");
                return;
            }
            if (!(searchItemObj is FFXIVCraftingSearchItem))
            {
                Service_Misc.LogText(_logTextBox, "Hey, you have to select an item in the combo box first !");
                return;
            }
            FFXIVCraftingSearchItem searchItem = searchItemObj as FFXIVCraftingSearchItem;
            if (null == searchItem)
            {
                Service_Misc.LogText(_logTextBox, "Hey, you have to select an item in the combo box first !");
                return;
            }

            TabPage tabPage = new TabPage();
            tabPage.Name = searchItem.Name;
            tabPage.Text = searchItem.Name;
            _itemsTabControl.TabPages.Add(tabPage);
            _itemsTabControl.SelectedTab = tabPage;

            _currentControl = new ShinyControl();
            _currentControl.ItemTabPage = tabPage;
            _currentControl.ItemTabControl = _itemsTabControl;
            _currentControl.Dock = DockStyle.Fill;
            tabPage.Controls.Add(_currentControl);
            _currentControl.Quantity = (int)_quantityNumericUpDown.Value;
            _currentControl.SetItem(searchItem);
            _currentControl.LogTextBox = _logTextBox;
        }

        private void _searchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _searchButton_Click(sender, e);
            }
        }
    }
}
