using MiqoCraftCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MiqoCraftMapViewer
{
    public partial class FormViewer : Form
    {
        List<FFXIVGatheringNode> ListGatheringNodes = new List<FFXIVGatheringNode>();
        List<FFXIVAetheryte> ListAetherytes = new List<FFXIVAetheryte>();
        FFXIVItemGridF gridF;

        public FormViewer()
        {
            InitializeComponent();

            try
            {
                MiqoGridFinderOptions options = new MiqoGridFinderOptions();
                options.Load(VPL.Application.Data.OptionLocation.GlobalOption);
                ListGatheringNodes = options.ListGatheringNodes;
                if (ListGatheringNodes.Count <= 0)
                {
                    ListGatheringNodes = MiqoCraftCore.ConsoleGamesWiki.GetFFXIVGatheringNodes();
                }

                ListAetherytes = options.ListAetherytes;
                if (ListAetherytes.Count <= 0)
                {
                    ListAetherytes = MiqoCraftCore.ConsoleGamesWiki.GetAetherytes();
                }
                _aetheryteComboBox.Items.Clear();
                foreach (FFXIVAetheryte aetherythe in ListAetherytes)
                {
                    _aetheryteComboBox.Items.Add(aetherythe);
                }

                options.ListGatheringNodes = ListGatheringNodes;
                options.ListAetherytes = ListAetherytes;
                options.Save();
            }
            catch
            {

            }
        }

        private void FormViewer_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Count() > 0)
            {
                try
                {
                    string filePath = files[0];
                    FileInfo file = new FileInfo(filePath);
                    if (!file.Exists) return;

                    List<MiqobotGrid> grids = Miqobot.GetAllGridsFromFile(file);
                    if (grids.Count <= 0) return;

                    Dictionary<string, double> dictionaryClosestAetherytes = new Dictionary<string, double>();

                    gridF = new FFXIVItemGridF();
                    gridF.GridFile = new FileInfo(file.FullName);
                    gridF.Analyze(ListAetherytes, ListGatheringNodes, ref dictionaryClosestAetherytes);

                    gridF.BuildPicture();

                    pictureBox1.BackColor = Color.White;
                    pictureBox1.BackgroundImage = gridF.Picture;
                    pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
            }
        }

        private void FormViewer_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void _aetheryteComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string gridName = _aetheryteComboBox.Text;
            if (null == gridF) return;

            foreach (FFXIVAetheryte aetheryte in ListAetherytes)
            {
                if (null != aetheryte && aetheryte.ToString() == gridName)
                {
                    gridF.ClosestAetheryte = aetheryte;
                    break;
                }
            }
            gridF.BuildPicture();

            pictureBox1.BackColor = Color.White;
            pictureBox1.BackgroundImage = gridF.Picture;
            pictureBox1.BackgroundImageLayout = ImageLayout.Zoom;
        }
    }
}
