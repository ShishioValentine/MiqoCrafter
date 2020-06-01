using MiqoCraftCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using VPL.Threading.Modeler;

namespace MiqoGridFinder
{
    public partial class MiqoGridFinder : Form
    {
        Thread _thread = null;
        List<FFXIVGatheringNode> ListGatheringNodes = new List<FFXIVGatheringNode>();
        List<FFXIVAetheryte> ListAetherytes = new List<FFXIVAetheryte>();
        List<FFXIVItemGridF> ListGridF = new List<FFXIVItemGridF>();

        public MiqoGridFinder()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            VPThreading.Abort(_thread);
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void MiqoGridFinder_Load(object sender, EventArgs e)
        {
            DirectoryInfo exeDirectory = new DirectoryInfo(Service_Misc.GetExecutionPath());
            DirectoryInfo newGridDirectory = new DirectoryInfo(Path.Combine(exeDirectory.FullName, "NewGrids"));
            DirectoryInfo newGridBitmapDirectory = new DirectoryInfo(Path.Combine(exeDirectory.FullName, "NewGridsBitmap"));
            if (!newGridDirectory.Exists)
            {
                newGridDirectory.Create();
            }
            if (!newGridBitmapDirectory.Exists)
            {
                newGridBitmapDirectory.Create();
            }
            foreach (FileInfo file in newGridDirectory.GetFiles())
            {
                file.Delete();
            }
            foreach (FileInfo file in newGridBitmapDirectory.GetFiles())
            {
                file.Delete();
            }
        }

        private void MainThread()
        {
            MiqoGridFinderOptions options = new MiqoGridFinderOptions();
            options.Load(VPL.Application.Data.OptionLocation.GlobalOption);
            ListGatheringNodes = options.ListGatheringNodes;
            if (ListGatheringNodes.Count <= 0)
            {
                VPThreading.SetText(_progressLabel, "Downloading all gathering nodes...");
                ListGatheringNodes = MiqoCraftCore.ConsoleGamesWiki.GetFFXIVGatheringNodes();
            }

            ListAetherytes = options.ListAetherytes;
            if (ListAetherytes.Count <= 0)
            {
                VPThreading.SetText(_progressLabel, "Downloading all aetherytes...");
                ListAetherytes = MiqoCraftCore.ConsoleGamesWiki.GetAetherytes();
            }

            options.ListGatheringNodes = ListGatheringNodes;
            options.ListAetherytes = ListAetherytes;
            options.Save();

            AnalyzeGridThread();
        }

        private void AnalyzeGridThread()
        {
            VPThreading.ClearItems(_gridListView);

            DirectoryInfo exeDirectory = new DirectoryInfo(Service_Misc.GetExecutionPath());
            DirectoryInfo cacheDirectory = new DirectoryInfo(Path.Combine(exeDirectory.FullName, "CacheGrid"));
            DirectoryInfo analyzeDirectory = new DirectoryInfo(Path.Combine(exeDirectory.FullName, "DownloadedGrids"));
            if (!analyzeDirectory.Exists)
            {
                DownloadMissingItemGrids();
            }
            if (!analyzeDirectory.Exists) return;
            Dictionary<string, double> dictionaryClosestAetherytes = new Dictionary<string, double>();
            FileInfo[] files = analyzeDirectory.GetFiles();
            int index = 0;
            foreach (FileInfo file in files)
            {
                index++;
                double percentage = (double)index / (double)files.Count() * 100.0;

                VPThreading.SetText(_progressLabel, "Analyzing Grids..." + percentage + "%");
                if (null == file) continue;
                try
                {
                    string itemName = file.Name.Split(new string[] { " Grid---" }, StringSplitOptions.None)[0];

                    //Looking into cache directory
                    string gridItemName = itemName + " Grid";
                    FileInfo cacheGridFile = new FileInfo(Path.Combine(cacheDirectory.FullName, gridItemName + ".txt"));
                    if (cacheGridFile.Exists)
                    {
                        VPThreading.SetText(_progressLabel, "Item already has a grid.");
                        continue;
                    }

                    FFXIVItemGridF gridF = new FFXIVItemGridF();
                    gridF.ItemName = itemName;
                    gridF.GridFile = new FileInfo(file.FullName);
                    gridF.Analyze(ListAetherytes, ListGatheringNodes, ref dictionaryClosestAetherytes);
                    ListGridF.Add(gridF);

                    if (gridF.IsValid)
                    {
                        gridF.SaveAsGrids();
                    }

                    ListViewItem item = new ListViewItem();
                    item.Text = file.Name;
                    item.SubItems.Add(gridF.Status);
                    item.Tag = gridF;
                    VPThreading.AddItem(_gridListView, item);
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
            }

        }

        private void DownloadMissingItemGridsThread()
        {
            VPThreading.SetText(_progressLabel, "Downloading all grids from miqobot forum index...");
            DownloadMissingItemGrids();
        }

        private void DownloadMissingItemGrids()
        {

            List<MiqoItemPage> result = new List<MiqoItemPage>();
            CookieCollection logMiqobotCookies = Miqobot.LogInForum();

            CookieCollection oCookies = new CookieCollection();
            HttpStatusCode oCode = HttpStatusCode.NotFound;
            HtmlAgilityPack.HtmlDocument answer = Service_Misc.GetWebPageFromRequest("GET https://miqobot.com/forum/forums/topic/index-gathering-grids/ HTTP/1.1|Host: miqobot.com|Connection: keep-alive|Cache-Control: max-age=0|Upgrade-Insecure-Requests: 1|User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36|Sec-Fetch-Mode: navigate|Sec-Fetch-User: ?1|Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3|Sec-Fetch-Site: same-origin|Referer: https://miqobot.com/forum/forums/forum/grids-and-presets/|Accept-Encoding: gzip, deflate, br|Accept-Language: fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7|Cookie: wordpress_test_cookie=WP+Cookie+check; _ga=GA1.2.1771485810.1566089776||",
                logMiqobotCookies, ref oCookies, ref oCode);
            if (null == answer)
            {
                VPThreading.SetText(_progressLabel, "Failed to update database. No answer from miqobot forum.");
                return;
            }

            HtmlAgilityPack.HtmlNode firstAnswerNode = Service_Misc.GetFirstChildNode(answer.DocumentNode, "div", "topic-tag-gathering");

            List<HtmlAgilityPack.HtmlNode> listItemNodes = firstAnswerNode.Descendants("li").ToList();

            DirectoryInfo exeDirectory = new DirectoryInfo(Service_Misc.GetExecutionPath());
            DirectoryInfo cacheDirectory = new DirectoryInfo(Path.Combine(exeDirectory.FullName, "CacheGrid"));
            DirectoryInfo analyzeDirectory = new DirectoryInfo(Path.Combine(exeDirectory.FullName, "DownloadedGrids"));
            if (!analyzeDirectory.Exists)
            {
                analyzeDirectory.Create();
            }
            if (!cacheDirectory.Exists)
            {
                VPThreading.SetText(_progressLabel, "Failed to compute database status, CacheGrid directory does not exist.");
                return;
            }
            int indexProg = 0;
            foreach (HtmlAgilityPack.HtmlNode node in listItemNodes)
            {
                indexProg++;

                VPThreading.SetProgress(_progressBar, indexProg * 100 / listItemNodes.Count);
                if (null == node) continue;

                string nodeInnerTextLower = node.InnerText.ToLower();
                if (nodeInnerTextLower.Contains("Mining Your Own Business".ToLower())) break;
                if (nodeInnerTextLower.Contains("lv."))
                {
                    //Found node !
                    string level = nodeInnerTextLower.Split(new string[] { "  " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                    string itemName = nodeInnerTextLower.Split(new string[] { "  " }, StringSplitOptions.RemoveEmptyEntries)[1].Trim().Replace("(hidden)", "").Trim();

                    //Check if grid exist
                    string gridItemName = itemName + " Grid";

                    //Looking into cache directory
                    FileInfo cacheGridFile = new FileInfo(Path.Combine(cacheDirectory.FullName, gridItemName + ".txt"));
                    if (cacheGridFile.Exists)
                    {
                        VPThreading.SetText(_progressLabel, "Item already has a grid.");
                        continue;
                    }
                    VPThreading.SetText(_progressLabel, "Trying to find a new grid : " + itemName);

                    //Looking for all links
                    List<MiqoItemPage> listLinks = Miqobot.GetURLItem(itemName, logMiqobotCookies, answer);
                    foreach (MiqoItemPage page in listLinks)
                    {
                        //List<string> ListGrids = Miqobot.GetAllGridsFromForum(itemName, logMiqobotCookies, page);
                        //if (null == ListGrids) continue;

                        //foreach(string grid in ListGrids)
                        //{
                        //    try
                        //    {
                        //        string pathGrid = Path.Combine(analyzeDirectory.FullName, gridItemName + "---" + gridIndex + ".txt");
                        //        if (File.Exists(pathGrid)) File.Delete(pathGrid);

                        //        File.WriteAllText(pathGrid, grid);
                        //        gridIndex++;
                        //    }
                        //    catch
                        //    {

                        //    }
                        //}
                        DownloadFromURL(page.URL, itemName, logMiqobotCookies);
                    }


                }
            }
            VPThreading.SetProgress(_progressBar, 100);
            VPThreading.SetText(_progressLabel, "Done!");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ThreadStart starter = new ThreadStart(DownloadMissingItemGridsThread);
            _thread = new Thread(starter);
            _thread.Start();
        }

        private void _gridListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (null == _gridListView) return;
            if (null == _gridListView.SelectedItems) return;
            if (0 >= _gridListView.SelectedItems.Count) return;

            ListViewItem selectedItem = _gridListView.SelectedItems[0];
            if (null == selectedItem) return;
            if (null == selectedItem.Tag) return;

            try
            {
                FFXIVItemGridF gridF = selectedItem.Tag as FFXIVItemGridF;
                DisplayGridDetails(gridF);
            }
            catch
            {

            }

        }

        private void DisplayGridDetails(FFXIVItemGridF iGrid)
        {
            _itemLabel.Text = "";
            _statusLabel.Text = "";
            _infoTextBox.Text = "";
            _nodeListView.Items.Clear();
            _aetheryteListView.Items.Clear();
            _closestLabel.Text = "";
            _computedDescriptionLabel.Text = "";
            _validationLabel.Text = "";
            if (null == iGrid) return;


            _itemLabel.Text = iGrid.ItemName;
            _statusLabel.Text = iGrid.Status;
            _infoTextBox.Text = iGrid.Description;
            _closestLabel.Text = "Closest Gathering Node : " + iGrid.ClosestNode;
            _computedDescriptionLabel.Text = "Computed Description : " + iGrid.ComputedDescription;
            _validationLabel.Text = "Validation Status : " + iGrid.IsValid;

            foreach (FFXIVAetheryte aetheryte in iGrid.ZoneAetherytes)
            {
                ListViewItem item = new ListViewItem();
                item.Text = aetheryte.ToString();
                item.SubItems.Add(aetheryte.Zone);
                try
                {
                    item.SubItems.Add(iGrid.ZoneAetherytesDistance[iGrid.ZoneAetherytes.IndexOf(aetheryte)].ToString());
                }
                catch
                {
                    item.SubItems.Add("-");
                }
                _aetheryteListView.Items.Add(item);
            }

            foreach (FFXIVGatheringNode node in iGrid.AllNodes)
            {
                ListViewItem item = new ListViewItem();
                item.Text = node.ToString();
                item.SubItems.Add(node.Zone);
                try
                {
                    item.SubItems.Add(iGrid.AllNodesDistance[iGrid.AllNodes.IndexOf(node)].ToString());
                }
                catch
                {
                    item.SubItems.Add("-");
                }
                _nodeListView.Items.Add(item);
            }
        }

        private void _displayGridButton_Click(object sender, EventArgs e)
        {
            if (null == _gridListView) return;
            if (null == _gridListView.SelectedItems) return;
            if (0 >= _gridListView.SelectedItems.Count) return;

            ListViewItem selectedItem = _gridListView.SelectedItems[0];
            if (null == selectedItem) return;
            if (null == selectedItem.Tag) return;

            try
            {
                FFXIVItemGridF gridF = selectedItem.Tag as FFXIVItemGridF;
                gridF.BuildPicture();
                Form form = new Form();
                form.Text = "Grid Bitmap";

                PictureBox pictureBox = new PictureBox();
                pictureBox.BackgroundImageLayout = ImageLayout.Zoom;
                pictureBox.Dock = DockStyle.Fill;
                pictureBox.BackgroundImage = gridF.Picture;

                form.Controls.Add(pictureBox);
                form.Show();
                form.WindowState = FormWindowState.Maximized;
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        private void _downloadFromURLButton_Click(object sender, EventArgs e)
        {
            ThreadStart starter = new ThreadStart(DownloadFromURLThread);
            VPThreading.Abort(_thread);
            _thread = new Thread(starter);
            _thread.Start();
        }

        private void DownloadFromURLThread()
        {
            DownloadFromURL(VPThreading.GetText(_urlTextBox));
        }

        private void DownloadFromURL(string iURL, string iItemName = "", CookieCollection iCookies = null)
        {
            VPThreading.SetText(_progressLabel, "Downloading grid from given URL...");
            if (null == iCookies) iCookies = Miqobot.LogInForum();
            List<string> listGridRawContent = Miqobot.GetAllGridsRawContentFromForum(iURL, iCookies);
            List<MiqobotGrid> listGrids = new List<MiqobotGrid>();

            MiqoGridFinderOptions options = new MiqoGridFinderOptions();
            options.Load(VPL.Application.Data.OptionLocation.GlobalOption);
            List<FFXIVSearchItem> listAllGatheredItems = options.ListAllGatheredItems;
            if (listAllGatheredItems.Count <= 0)
            {
                VPThreading.SetText(_progressLabel, "Downloading all item names...");
                listAllGatheredItems = GarlandTool.Search("", null, FFXIVItem.TypeItem.Gathered);
            }
            options.ListAllGatheredItems = listAllGatheredItems;
            options.Save();


            VPThreading.SetText(_progressLabel, "Reading grids...");
            foreach (string rawContent in listGridRawContent)
            {
                listGrids.AddRange(Miqobot.GetAllGridsFromContent(rawContent));
            }

            DirectoryInfo exeDirectory = new DirectoryInfo(Service_Misc.GetExecutionPath());
            DirectoryInfo cacheDirectory = new DirectoryInfo(Path.Combine(exeDirectory.FullName, "CacheGrid"));
            DirectoryInfo analyzeDirectory = new DirectoryInfo(Path.Combine(exeDirectory.FullName, "DownloadedGrids"));
            if (!analyzeDirectory.Exists)
            {
                analyzeDirectory.Create();
            }
            if (!cacheDirectory.Exists)
            {
                VPThreading.SetText(_progressLabel, "Failed to compute database status, CacheGrid directory does not exist.");
                return;
            }

            VPThreading.SetText(_progressLabel, "Matching grid to item list...");
            int gridIndex = 1;
            foreach (MiqobotGrid grid in listGrids)
            {
                List<string> listCorrespondingItemNames = new List<string>();
                foreach (FFXIVSearchItem item in listAllGatheredItems)
                {
                    if (null == grid.Description) continue;
                    if (grid.Description.ToLower().Contains(item.Name.ToLower()))
                    {
                        listCorrespondingItemNames.Add(item.Name);
                    }
                    if (grid.Header.ToLower().Contains(item.Name.ToLower()))
                    {
                        listCorrespondingItemNames.Add(item.Name);
                    }
                }

                if (listCorrespondingItemNames.Count <= 0 && iItemName != "")
                {
                    listCorrespondingItemNames.Add(iItemName);
                }
                List<string> listFilteredCorrespondingItemNames = new List<string>();
                foreach (string itemName in listCorrespondingItemNames)
                {
                    bool hasBigger = false;
                    foreach (string itemName2 in listCorrespondingItemNames)
                    {
                        if (itemName2 != itemName && itemName2.Contains(itemName))
                        {
                            hasBigger = true;
                            break;
                        }
                    }
                    if (!hasBigger)
                    {
                        listFilteredCorrespondingItemNames.Add(itemName);
                    }
                }
                foreach (string itemName in listFilteredCorrespondingItemNames)
                {
                    //Check if grid exist
                    string gridItemName = itemName + " Grid";

                    //Looking into cache directory
                    FileInfo cacheGridFile = new FileInfo(Path.Combine(cacheDirectory.FullName, gridItemName + ".txt"));
                    if (cacheGridFile.Exists)
                    {
                        continue;
                    }

                    //Saving grid
                    string pathGrid = Path.Combine(analyzeDirectory.FullName, gridItemName + "---" + gridIndex + ".txt");
                    if (File.Exists(pathGrid)) File.Delete(pathGrid);

                    File.WriteAllText(pathGrid, "grid." + grid.Header + Environment.NewLine + grid.Content);
                    gridIndex++;
                }
            }
        }

        private void _analyzeButton_Click(object sender, EventArgs e)
        {
            ThreadStart starter = new ThreadStart(MainThread);
            _thread = new Thread(starter);
            _thread.Start();
        }
    }
}
