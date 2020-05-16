using MiqoCraftCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VPL.Application.Data;
using VPL.Threading.Modeler;

namespace MiqoCraft
{
    public partial class FFXIVCraftingOptionsForm : Form
    {
        FFXIVItem _item;
        FFXIVCraftingOptions _options;

        public FFXIVCraftingOptionsForm()
        {
            InitializeComponent();
        }

        public FFXIVItem Item
        {
            get => _item;
            set
            {
                _item = value;
                UpdateValues();
            }
        }

        public FFXIVCraftingOptions Options { get => _options; set => _options = value; }

        public void UpdateValues()
        {
            _titleLabel.Text = "Item Options";
            _itemPictureBox.BackgroundImage = null;
            Options = null;
            if (null == _item) return;


            MiqoCraftOptions options = new MiqoCraftOptions();
            options.Load(OptionLocation.UserOption);

            Options = options.GetOption(_item);

            //Creating option if need be
            if(null == Options)
            {
                Options = new FFXIVCraftingOptions();
                Options.ItemID = _item.ID;
                options.ListItemOptions.Append(Options);
                options.Save();
            }

            _titleLabel.Text = _item.Name + " Options";
            VPThreading.SetImageFromURL(_itemPictureBox, _item.UrlImage);
            _itemPictureBox.BackgroundImageLayout = ImageLayout.Zoom;
            _craftTextBox.Text = Options.CustomCraftingMacro;
            _ignoreCheckBox.Checked = Options.IgnoreItem;
        }

        private void _okButton_Click(object sender, EventArgs e)
        {
            if (null == Options) return;

            Options.IgnoreItem = _ignoreCheckBox.Checked;
            Options.CustomCraftingMacro = _craftTextBox.Text;

            MiqoCraftOptions options = new MiqoCraftOptions();
            options.Load(OptionLocation.UserOption);

            FFXIVCraftingOptions existingOption = options.GetOption(Options.ItemID);
            while(null != existingOption)
            {
                options.ListItemOptions.Remove(existingOption);
                existingOption = options.GetOption(Options.ItemID);
            }
            options.ListItemOptions.Add(Options);
            options.Save();

            Close();
        }
    }
}
