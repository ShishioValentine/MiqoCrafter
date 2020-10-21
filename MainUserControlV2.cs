using MiqoCraftCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using VPL.Application.Data;
using VPL.Threading.Modeler;

namespace MiqoCraft
{
    public partial class MainUserControlV2 : UserControl
    {
        object _locker = new object();
        List<FFXIVSearchItem> _itemsToCraft = new List<FFXIVSearchItem>();

        List<string> _jobFilter = new List<string>();
        List<FFXIVSearchItem> _lastDisplayedItems = new List<FFXIVSearchItem>();

        Thread _updateBDDThread;
        Thread _updateResultPictureThread;

        public MainUserControlV2()
        {
            InitializeComponent();
            Service_Misc.LogText(_logTextBox, "Hey There ! I am MiqoCrafter, nice to met you.");
            Service_Misc.LogText(_logTextBox, "What can I do for you today ?");

            //Loading last search result
            MiqoCraftOptions options = new MiqoCraftOptions();
            options.Load(OptionLocation.UserOption);
            DisplayItemList(options.LastSearchResult);

            UpdateBDDStatusFromOptions();
        }

        /// <summary>
        /// Search item thread
        /// </summary>
        Thread _searchThread = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            VPThreading.Abort(_searchThread);
            VPThreading.Abort(_updateBDDThread);
            VPThreading.Abort(_updateResultPictureThread);
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
            int minLevel = (int)VPThreading.GetValue(_minLevelNumericUpDown);
            int maxLevel = (int)VPThreading.GetValue(_maxLevelNumericUpDown);
            List<string> jobs = new List<string>();
            lock (_locker)
            {
                jobs = _jobFilter;
            }
            Service_Misc.LogText(_logTextBox, "Alright, let's look for " + elemToSearch);
            List<FFXIVSearchItem> listResults = GarlandTool.Search(elemToSearch, _logTextBox, FFXIVItem.TypeItem.Crafted, jobs, minLevel, maxLevel);

            //Storing in options
            MiqoCraftOptions options = new MiqoCraftOptions();
            options.Load(OptionLocation.UserOption);
            options.LastSearchResult = listResults;
            options.Save();

            if (listResults.Count > 0) Service_Misc.LogText(_logTextBox, "All done ! Found " + listResults.Count + " items!");
            else Service_Misc.LogText(_logTextBox, "All done ! But I couldn't find your item...");

            DisplayItemList(listResults);
        }

        private void DisplayItemList(List<FFXIVSearchItem> iItems)
        {
            VPThreading.ClearItems(_resultListView);
            _lastDisplayedItems = iItems;

            Service_Misc.LogText(_logTextBox, "Updating list...");
            foreach (FFXIVSearchItem item in iItems)
            {
                if (null == item) continue;

                ListViewItem listViewItem = new ListViewItem();
                listViewItem.Tag = item;
                listViewItem.Text = item.Quantity + "x " + item.Name;
                listViewItem.SubItems.Add(item.Class + " lvl" + item.Level);
                listViewItem.ToolTipText = item.Class + " lvl" + item.Level;

                VPThreading.AddItem(_resultListView, listViewItem);
            }
            VPThreading.SetText(_nbResultItemLabel, iItems.Count + " Items found");

            UpdateResultPictures();
        }

        private void _craftButton_Click(object sender, EventArgs e)
        {
            ScenarioForm scenarioForm = new ScenarioForm();

            scenarioForm.ListItemsToCraft = _itemsToCraft;
            if(scenarioForm.ListItemsToCraft.Count <= 0)
            {
                _addToCraftingListButton_Click(sender, e);
                scenarioForm.ListItemsToCraft = _itemsToCraft;
            }
            scenarioForm.ShowDialog();
        }

        private void _searchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                _searchButton_Click(sender, e);
            }
        }

        private void _alcButton_Click(object sender, EventArgs e)
        {
            try
            {
                Button jobButton = sender as Button;
                string jobName = jobButton.Name.Replace("_", "").Substring(0, 3).ToUpper();
                lock (_locker)
                {
                    if (_jobFilter.Contains(jobName))
                    {
                        _jobFilter.Remove(jobName);
                    }
                    else
                    {
                        _jobFilter.Add(jobName);
                    }
                }
                ActionUpdateUI();
            }
            catch
            {

            }
        }

        private void ActionUpdateUI()
        {
            UpdateJobButtonStatus(_alcButton);
            UpdateJobButtonStatus(_wvrButton);
            UpdateJobButtonStatus(_culButton);
            UpdateJobButtonStatus(_gsmButton);
            UpdateJobButtonStatus(_armButton);
            UpdateJobButtonStatus(_bsmButton);
            UpdateJobButtonStatus(_crpButton);
            UpdateJobButtonStatus(_ltwButton);
        }

        private void UpdateJobButtonStatus(Button iButton)
        {
            if (null == iButton) return;

            string jobName = iButton.Name.Replace("_", "").Substring(0, 3).ToUpper();

            lock (_locker)
            {
                if (_jobFilter.Contains(jobName))
                {
                    iButton.FlatAppearance.BorderSize = 2;
                }
                else
                {
                    iButton.FlatAppearance.BorderSize = 0;
                }
            }
        }

        private void UpdateBDDStatusFromOptions()
        {
            try
            {
                MiqoCraftOptions options = new MiqoCraftOptions();
                options.Load(OptionLocation.GlobalOption);

                int percentage = options.ListGridOKItems.Count * 100 / options.ListGatherableItems.Count;

                int nbOK = 0;
                string OKList = "";
                string KOList = "";
                foreach (string item in options.ListGatherableItems)
                {
                    if (options.ListGridOKItems.Contains(item))
                    {
                        nbOK++;
                        OKList += item + Environment.NewLine;
                    }
                    else
                    {
                        KOList += item + Environment.NewLine;
                    }
                }
                Console.WriteLine("--- OK Items");
                Console.WriteLine(OKList);
                Console.WriteLine("--- KO Items");
                Console.WriteLine(KOList);
            }
            catch
            {

            }
        }

        private void UpdateGridBDDThread()
        {
            List<string> _gatherableItemNames = new List<string>();
            List<string> _gridOKItemNames = new List<string>();

            Service_Misc.LogText(_logTextBox, "Updating database...");

            MiqoCraftCore.MiqoCraftCore.DownloadGrids();

            try
            {
                List<MiqoItemPage> result = new List<MiqoItemPage>();
                CookieCollection logMiqobotCookies = Miqobot.LogInForum();

                CookieCollection oCookies = new CookieCollection();
                HttpStatusCode oCode = HttpStatusCode.NotFound;
                HtmlAgilityPack.HtmlDocument answer = Service_Misc.GetWebPageFromRequest("GET https://miqobot.com/forum/forums/topic/index-gathering-grids/ HTTP/1.1|Host: miqobot.com|Connection: keep-alive|Cache-Control: max-age=0|Upgrade-Insecure-Requests: 1|User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36|Sec-Fetch-Mode: navigate|Sec-Fetch-User: ?1|Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3|Sec-Fetch-Site: same-origin|Referer: https://miqobot.com/forum/forums/forum/grids-and-presets/|Accept-Encoding: gzip, deflate, br|Accept-Language: fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7|Cookie: wordpress_test_cookie=WP+Cookie+check; _ga=GA1.2.1771485810.1566089776||",
                    logMiqobotCookies, ref oCookies, ref oCode);
                if (null == answer)
                {
                    Service_Misc.LogText(_logTextBox, "Failed to update database. No answer from miqobot forum.");
                    return;
                }

                HtmlAgilityPack.HtmlNode firstAnswerNode = Service_Misc.GetFirstChildNode(answer.DocumentNode, "div", "topic-tag-gathering");

                List<HtmlAgilityPack.HtmlNode> listItemNodes = firstAnswerNode.Descendants("li").ToList();

                DirectoryInfo exeDirectory = new DirectoryInfo(Service_Misc.GetExecutionPath());
                DirectoryInfo cacheDirectory = new DirectoryInfo(Path.Combine(exeDirectory.FullName, "CacheGrid"));
                if (!cacheDirectory.Exists)
                {
                    Service_Misc.LogText(_logTextBox, "Failed to compute database status, CacheGrid directory does not exist.");
                    return;
                }

                foreach (HtmlAgilityPack.HtmlNode node in listItemNodes)
                {
                    if (null == node) continue;

                    string nodeInnerTextLower = node.InnerText.ToLower();
                    if (nodeInnerTextLower.Contains("Mining Your Own Business".ToLower())) break;
                    if (nodeInnerTextLower.Contains("lv."))
                    {
                        //Found node !
                        string level = nodeInnerTextLower.Split(new string[] { "  " }, StringSplitOptions.RemoveEmptyEntries)[0].Trim();
                        string itemName = nodeInnerTextLower.Split(new string[] { "  " }, StringSplitOptions.RemoveEmptyEntries)[1].Trim().Replace("(hidden)", "").Trim();

                        _gatherableItemNames.Add(itemName);
                        if (File.Exists(Path.Combine(cacheDirectory.FullName, itemName + " Grid.txt")))
                        {
                            _gridOKItemNames.Add(itemName);
                        }
                    }
                }

                lock (_locker)
                {
                    MiqoCraftOptions options = new MiqoCraftOptions();
                    options.Load(OptionLocation.GlobalOption);
                    options.ListGatherableItems = _gatherableItemNames;
                    options.ListGridOKItems = _gridOKItemNames;
                    options.Save();
                }

                UpdateBDDStatusFromOptions();

                {
                    Service_Misc.LogText(_logTextBox, "Database updated !");
                    return;
                }
            }
            catch
            {
                Service_Misc.LogText(_logTextBox, "Failed to compute database status, CacheGrid directory does not exist.");
                return;
            }

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            VPThreading.Abort(_updateBDDThread);
            ThreadStart starter = new ThreadStart(UpdateGridBDDThread);
            _updateBDDThread = new Thread(starter);
            _updateBDDThread.Start();
        }

        private void _selectAllButton_Click(object sender, EventArgs e)
        {
            _resultListView.SelectedItems.Clear();
            foreach (ListViewItem item in _resultListView.Items)
            {
                item.Selected = true;
            }
        }
        private void listView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A && e.Control)
            {
                _selectAllButton_Click(sender, e);
            }
        }

        private void UpdateResultPictures()
        {
            VPThreading.Abort(_updateResultPictureThread);
            ThreadStart starter = new ThreadStart(UpdateResultPicturesThread);
            _updateResultPictureThread = new Thread(starter);
            _updateResultPictureThread.Start();
        }

        private void UpdateResultPicturesThread()
        {
            List<ListViewItem> iItems = VPThreading.GetItems(_resultListView);

            Service_Misc.LogText(_logTextBox, "Updating Item Pictures...");
            foreach (ListViewItem listViewItem in iItems)
            {
                try
                {
                    FFXIVSearchItem item = VPThreading.GetTag(listViewItem) as FFXIVSearchItem;
                    if (!_prerequisiteImageList.Images.ContainsKey(item.ID))
                    {
                        System.Net.WebRequest request = System.Net.WebRequest.Create(item.UrlImage);
                        System.Net.WebResponse resp = request.GetResponse();
                        System.IO.Stream respStream = resp.GetResponseStream();
                        Bitmap bmp = new Bitmap(respStream);
                        respStream.Dispose();

                        VPThreading.AddImage(_resultListView, _prerequisiteImageList, item.ID, bmp);
                    }
                    VPThreading.SetImageKey(listViewItem, item.ID);
                }
                catch (Exception)
                {
                }
            }
        }

        private void _resultListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            _nbSelectedItemLabel.Text = _resultListView.SelectedItems.Count + " Items selected";
        }

        private void _quantityNumericUpDown_ValueChanged(object sender, EventArgs e)
        {

        }

        private void _quantityNumericUpDown_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ListView.SelectedListViewItemCollection iItems = _resultListView.SelectedItems;

                Service_Misc.LogText(_logTextBox, "Updating Item Pictures...");
                foreach (ListViewItem listViewItem in iItems)
                {
                    try
                    {
                        FFXIVSearchItem item = VPThreading.GetTag(listViewItem) as FFXIVSearchItem;
                        //if (null != item) item.Quantity = (int)VPThreading.GetValue(_quantityNumericUpDown);
                        DisplayItemList(_lastDisplayedItems);
                    }
                    catch
                    {

                    }
                }
            }
        }

        private void _resultListView_ItemActivate(object sender, EventArgs e)
        {
            if (_resultListView.SelectedItems.Count != 1) return;
            ListViewItem currentItem = _resultListView.SelectedItems[0];
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

        private void _addToCraftingListButton_Click(object sender, EventArgs e)
        {
            bool checkedItems = false;
            foreach (ListViewItem listViewItem in _resultListView.CheckedItems)
            {
                try
                {
                    FFXIVSearchItem item = VPThreading.GetTag(listViewItem) as FFXIVSearchItem;
                    if (null != item)
                    {
                        _itemsToCraft.Add(item);
                        checkedItems = true;
                    }
                }
                catch
                {

                }
            }

            if(!checkedItems)
            {
                foreach (ListViewItem listViewItem in _resultListView.SelectedItems)
                {
                    try
                    {
                        FFXIVSearchItem item = VPThreading.GetTag(listViewItem) as FFXIVSearchItem;
                        if (null != item)
                        {
                            _itemsToCraft.Add(item);
                            checkedItems = true;
                        }
                    }
                    catch
                    {

                    }
                }
            }
            UpdateCraftingList();
        }

        private void UpdateCraftingList()
        {
            _craftingListPanel.Controls.Clear();

            foreach (FFXIVSearchItem item in _itemsToCraft)
            {
                FFXIVCraftingListItem control = new FFXIVCraftingListItem();
                control.Item = item;
                control.Dock = DockStyle.Top;
                _craftingListPanel.Controls.Add(control);
            }
        }

        private void _clearButton_Click(object sender, EventArgs e)
        {
            _itemsToCraft.Clear();
            UpdateCraftingList();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
