﻿using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VPL.Application.Data;

namespace MiqoCraft
{
    public static class Miqobot
    {
        /// <summary>
        /// Logs into miqoot forums
        /// </summary>
        /// <returns></returns>
        public static CookieCollection LogInForum()
        {
            CookieCollection iCookies = new CookieCollection();
            CookieCollection oCookies = new CookieCollection();
            HttpStatusCode oCode = HttpStatusCode.NotFound;
            string answer = Service_Misc.GetContentFromRequest("POST https://miqobot.com/forum/wp-login.php HTTP/1.1|Host: miqobot.com|Connection: keep-alive|Content-Length: 128|Cache-Control: max-age=0|Origin: https://miqobot.com|Upgrade-Insecure-Requests: 1|Content-Type: application/x-www-form-urlencoded|User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36|Sec-Fetch-Mode: navigate|Sec-Fetch-User: ?1|Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3|Sec-Fetch-Site: same-origin|Referer: https://miqobot.com/forum/wp-login.php|Accept-Encoding: gzip, deflate, br|Accept-Language: fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7|Cookie: wordpress_test_cookie=WP+Cookie+check; _ga=GA1.2.1771485810.1566089776||",
                iCookies, ref oCookies, ref oCode,
                "log=miqocrafter%40gmail.com&pwd=.f%2Fg%24%5D%21zzNb4&wp-submit=Log+In&redirect_to=https%3A%2F%2Fmiqobot.com%2Fforum&testcookie=1");
            if (answer != "0" && oCode != HttpStatusCode.OK) return null;
            return oCookies;
        }

        public static List<MiqoItemPage> GetURLItem(string iItemName, CookieCollection iLogCookies)
        {
            List<MiqoItemPage> result = new List<MiqoItemPage>();

            CookieCollection oCookies = new CookieCollection();
            HttpStatusCode oCode = HttpStatusCode.NotFound;
            HtmlDocument answer = Service_Misc.GetWebPageFromRequest("GET https://miqobot.com/forum/forums/topic/index-gathering-grids/ HTTP/1.1|Host: miqobot.com|Connection: keep-alive|Cache-Control: max-age=0|Upgrade-Insecure-Requests: 1|User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36|Sec-Fetch-Mode: navigate|Sec-Fetch-User: ?1|Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3|Sec-Fetch-Site: same-origin|Referer: https://miqobot.com/forum/forums/forum/grids-and-presets/|Accept-Encoding: gzip, deflate, br|Accept-Language: fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7|Cookie: wordpress_test_cookie=WP+Cookie+check; _ga=GA1.2.1771485810.1566089776||",
                iLogCookies, ref oCookies, ref oCode);
            if (null == answer) return result;

            HtmlNode firstAnswerNode = Service_Misc.GetFirstChildNode(answer.DocumentNode, "div", "topic-tag-gathering");

            List<HtmlNode> listItemNodes = firstAnswerNode.Descendants("li").ToList();


            foreach (HtmlNode node in listItemNodes)
            {
                if (null == node) continue;

                string nodeInnerTextLower = node.InnerText.ToLower();
                if (nodeInnerTextLower.Contains("by") && nodeInnerTextLower.Contains(iItemName.ToLower()))
                {
                    //Found node !

                    List<HtmlNode> listLinks = node.Descendants("a").ToList();
                    foreach (HtmlNode link in listLinks)
                    {
                        MiqoItemPage itempage = new MiqoItemPage();
                        itempage.URL = link.GetAttributeValue("href", "");
                        itempage.Contributor = link.InnerText;
                        result.Add(itempage);
                    }
                }
            }

            return result;
        }

        public static bool HasGrid(string iItemName)
        {
            string gridItemName = iItemName + " Grid";

            //Looking into cache directory
            DirectoryInfo exeDirectory = new DirectoryInfo(Service_Misc.GetExecutionPath());
            DirectoryInfo cacheDirectory = new DirectoryInfo(Path.Combine(exeDirectory.FullName, "CacheGrid"));
            if (!cacheDirectory.Exists) cacheDirectory.Create();

            FileInfo cacheGridFile = new FileInfo(Path.Combine(cacheDirectory.FullName, gridItemName + ".txt"));
            if (cacheGridFile.Exists)
            {
                return true;
            }
            return false;
        }

        public static string GetGrid(string iItemName, CookieCollection iLogCookies, MiqoItemPage iPage, out string oGridName)
        {
            string gridItemName = iItemName + " Grid";
            oGridName = "";
            CookieCollection oCookies = new CookieCollection();
            HttpStatusCode oCode = HttpStatusCode.NotFound;

            //Looking into cache directory
            DirectoryInfo exeDirectory = new DirectoryInfo(Service_Misc.GetExecutionPath());
            DirectoryInfo cacheDirectory = new DirectoryInfo(Path.Combine(exeDirectory.FullName, "CacheGrid"));
            if (!cacheDirectory.Exists) cacheDirectory.Create();

            string gridRawContent = "";
            FileInfo cacheGridFile = new FileInfo(Path.Combine(cacheDirectory.FullName, gridItemName + ".txt"));
            if (cacheGridFile.Exists)
            {
                oGridName = gridItemName;
                gridRawContent = System.IO.File.ReadAllText(cacheGridFile.FullName);
            }
            else
            {
                HtmlDocument answer = Service_Misc.GetWebPageFromRequest("GET " + iPage.URL + " HTTP/1.1|Host: miqobot.com|Connection: keep-alive|Cache-Control: max-age=0|Upgrade-Insecure-Requests: 1|User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36|Sec-Fetch-Mode: navigate|Sec-Fetch-User: ?1|Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3|Sec-Fetch-Site: same-origin|Referer: https://miqobot.com/forum/wp-login.php?redirect_to=https%3A%2F%2Fmiqobot.com%2Fforum%2Fforums%2Ftopic%2Fgrade-1-carbonized-matter-min-lv20%2F|Accept-Encoding: gzip, deflate, br|Accept-Language: fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7|",
                    iLogCookies, ref oCookies, ref oCode);
                if (null == answer) return null;

                string lastUrlAttachment = "";
                List<HtmlNode> listAllAttachmentsNode = answer.DocumentNode.Descendants("div").ToList();
                foreach (HtmlNode node in listAllAttachmentsNode)
                {
                    if (node.GetAttributeValue("class", "") == "bbp-attachments")
                    {
                        lastUrlAttachment = Service_Misc.ExtractLink(node);
                    }
                }

                gridRawContent = Service_Misc.GetContentFromRequest("GET " + lastUrlAttachment + " HTTP/1.1|Host: miqobot.com|Connection: keep-alive|Upgrade-Insecure-Requests: 1|User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36|Sec-Fetch-Mode: navigate|Sec-Fetch-User: ?1|Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3|Sec-Fetch-Site: same-origin|Referer: https://miqobot.com/forum/forums/topic/grade-1-carbonized-matter-min-lv20/|Accept-Encoding: gzip, deflate, br|Accept-Language: fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7|",
                    iLogCookies, ref oCookies, ref oCode);
            }


            if (null == gridRawContent || "" == gridRawContent || !gridRawContent.Contains("{")) return null;

            //Need to parse it further !
            Dictionary<string, JToken> listGatherPreset = new Dictionary<string, JToken>();
            Dictionary<string, string> listGrids = new Dictionary<string, string>();
            List<string> lines = gridRawContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            for (int i = 0; i < lines.Count - 1; i += 2)
            {
                string lineHeader = lines[i];
                string lineContent = lines[i + 1];

                if (lineHeader.Contains("gatherpreset."))
                {
                    JToken mainToken = JObject.Parse(lineContent);
                    if (null == mainToken)
                    {
                        continue;
                    }
                    listGatherPreset.Add(lineHeader.Replace("gatherpreset.", ""), mainToken);
                }
                else if (lineHeader.Contains("grid."))
                {
                    listGrids.Add(lineHeader.Replace("grid.", ""), lineContent);
                }
            }

            if (listGatherPreset.Count > 0)
            {
                KeyValuePair<string, JToken> myItemPair = listGatherPreset.FirstOrDefault(x => x.Key.ToLower().Contains(iItemName.ToLower()) || x.Key.ToLower().Contains(iItemName.ToLower().Replace(" ", "")));
                if (null == myItemPair.Value)
                {
                    if (listGatherPreset.Count == 1)
                    {
                        myItemPair = listGatherPreset.First();
                    }
                }
                if (null != myItemPair.Value)
                {
                    JToken gridNameToken = myItemPair.Value["gridname"];
                    if (null != gridNameToken)
                    {
                        string gridName = gridNameToken.Value<string>();
                        oGridName = gridName;

                        if (listGrids.ContainsKey(gridName))
                        {
                            string gridData = listGrids[gridName];
                            if (gridData.Contains("{"))
                            {
                                string fullGrid = "grid." + gridItemName + Environment.NewLine + gridData.Split(new string[] { "grid." }, StringSplitOptions.None)[0].Replace(Environment.NewLine, "");

                                return fullGrid;
                            }
                        }
                    }
                }

            }
            if (listGrids.Count > 0)
            {
                //Need to rely on grids
                KeyValuePair<string, string> myItemPair = listGrids.FirstOrDefault(x => x.Key.ToLower().Contains(iItemName.ToLower()) || x.Key.ToLower().Contains(iItemName.ToLower().Replace(" ", "")));
                if (null == myItemPair.Value)
                {
                    if (listGrids.Count == 1)
                    {
                        myItemPair = listGrids.First();
                    }
                }
                if (null == myItemPair.Value)
                {
                    myItemPair = listGrids.FirstOrDefault(x => x.Value.ToLower().Contains(iItemName.ToLower()) || x.Value.ToLower().Contains(iItemName.ToLower().Replace(" ", "")));
                }
                if (null == myItemPair.Value)
                {
                    return null;
                }

                string gridData = myItemPair.Value;
                if (!gridData.Contains("{")) return null;
                oGridName = myItemPair.Key;
                string fullGrid = "grid." + gridItemName + Environment.NewLine + gridData.Split(new string[] { "grid." }, StringSplitOptions.None)[0].Replace(Environment.NewLine, "");

                return fullGrid;
            }
            return null;
        }

        public static string GetCacheRotations()
        {
            //Looking into cache directory
            DirectoryInfo exeDirectory = new DirectoryInfo(Service_Misc.GetExecutionPath());
            DirectoryInfo cacheDirectory = new DirectoryInfo(Path.Combine(exeDirectory.FullName, "CacheRotations"));
            if (!cacheDirectory.Exists) cacheDirectory.Create();

            string result = "";
            foreach (FileInfo rotationFile in cacheDirectory.GetFiles())
            {
                result += System.IO.File.ReadAllText(rotationFile.FullName);
            }
            return result;
        }

        public static string GetCacheSolverPresets()
        {
            //Looking into cache directory
            DirectoryInfo exeDirectory = new DirectoryInfo(Service_Misc.GetExecutionPath());
            DirectoryInfo cacheDirectory = new DirectoryInfo(Path.Combine(exeDirectory.FullName, "CacheSolverPresets"));
            if (!cacheDirectory.Exists) cacheDirectory.Create();

            string result = "";
            foreach (FileInfo SolverPresetFile in cacheDirectory.GetFiles())
            {
                result += System.IO.File.ReadAllText(SolverPresetFile.FullName);
            }
            return result;
        }

        public static string GetCraftingPreset(string gatheringType, string iSlot, FFXIVGatheredItem iGatheredItem, string iRotation, string iGridName, string iTime)
        {
            if (null == iGatheredItem) return "";

            //Looking into cache directory
            string presetItemName = iGatheredItem.Name + " preset";
            DirectoryInfo exeDirectory = new DirectoryInfo(Service_Misc.GetExecutionPath());
            DirectoryInfo cacheDirectory = new DirectoryInfo(Path.Combine(exeDirectory.FullName, "CacheGatheringPreset"));
            if (!cacheDirectory.Exists) cacheDirectory.Create();

            FileInfo cacheGridFile = new FileInfo(Path.Combine(cacheDirectory.FullName, presetItemName + ".txt"));
            string rawPreset = "";
            if (cacheGridFile.Exists)
            {
                rawPreset = System.IO.File.ReadAllText(cacheGridFile.FullName);
            }
            else
            {
                string useCompass = "true";
                if (gatheringType == "1" || gatheringType == "3") useCompass = "false";
                string useCompass2 = "false";
                if (gatheringType == "1" || gatheringType == "3") useCompass2 = "true";
                rawPreset = "gatherpreset." + presetItemName + Environment.NewLine;
                string nodeName = "";
                string slot = "";
                string useTruth = "false";
                if (iTime != "")
                {
                    useTruth = "true";
                    //fullScenario += "afkUntil(" + time.Split(',')[0] + ":00et)" + Environment.NewLine;
                }
                {
                    slot = "0";
                    nodeName = iGatheredItem.Name;

                    {
                        nodeName = nodeName + "\",\"Crystal\",\"Shard";
                    }
                }
                rawPreset += "{\"owntab\":0,\"assistmode\":false,\"nodename\":\"\",\"slot\":" + slot + ",\"maxcount\":32,\"usecompass\":" + useCompass + ",\"usecompass2\":" + useCompass2 + ",\"usetruth\":" + useTruth + ",\"userotation\":true,\"usemacro\":false,\"macro\":\"\",\"speargig\":0,\"spearshadows\":0,\"usecordials\":true,\"usefavors\":false,\"spearcollect\":true,\"spearcollectability\":1,\"byname\":[\"" + nodeName + "\"],\"veterantradebyname\":[],\"gridname\":\"" + iGridName + "\",\"rotationname\":\"" + "" + "\"}" + Environment.NewLine;

            }

            //System.IO.File.WriteAllText(cacheGridFile.FullName, rawPreset);

            rawPreset = rawPreset.Replace("ROTATION_TO_USE", iRotation);

            return rawPreset;
        }

        /// <summary>
        /// Create list of items to display
        /// </summary>
        /// <param name="iItem"></param>
        /// <param name="iQuantity"></param>
        /// <param name="allItems"></param>
        /// <param name="allItemsQuantity"></param>
        public static void RecFindItems(FFXIVItem iItem, int iQuantity, ref List<FFXIVItem> allItems, ref List<int> allItemsQuantity, Dictionary<string, int> CustomQuantities)
        {
            if (null == iItem) return;
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

            int quantity = iQuantity * iItem.Quantity;

            bool hasFixedQuantity = false;
            if(CustomQuantities.ContainsKey(iItem.ID))
            {
                hasFixedQuantity = true;
                quantity = CustomQuantities[iItem.ID];
            }
            //if (quantity <= 0) return;

            //Correcting quantity by yield value
            double currentQuantity = (double)quantity;
            double yieldQuantityD = (double)1;
            if (null != craftedItem)
            {
                yieldQuantityD = (double)craftedItem.RecipeQuantity;
            }
            double correctedQuantity = currentQuantity / yieldQuantityD + (yieldQuantityD - 0.95 /*For numeric error*/) / yieldQuantityD;
            quantity = (int)correctedQuantity;

            if (null != craftedItem)
            {
                foreach (FFXIVItem ingredient in craftedItem.ListNeededItems)
                {
                    RecFindItems(ingredient, quantity, ref allItems, ref allItemsQuantity, CustomQuantities);
                }
            }
            if (null != reducedItem)
            {
                RecFindItems(reducedItem.ReducedFrom, quantity, ref allItems, ref allItemsQuantity, CustomQuantities);
            }

            FFXIVItem existingItem = allItems.Find(x => x != null && x.ID == iItem.ID);
            if (null != existingItem)
            {
                int index = allItems.IndexOf(existingItem);
                if (index >= 0 && index < allItemsQuantity.Count)
                {
                    if (hasFixedQuantity)
                    {
                        allItemsQuantity[index] = quantity;
                    }
                    else
                    {
                        allItemsQuantity[index] = allItemsQuantity[index] + (quantity);
                    }
                }
            }
            else
            {
                allItems.Add(iItem);
                allItemsQuantity.Add(quantity);
            }
        }

        public static List<string> GetCatalysts()
        {
            List<string> result = new List<string>();

            //Shards
            result.Add("Fire Shard");
            result.Add("Ice Shard");
            result.Add("Wind Shard");
            result.Add("Earth Shard");
            result.Add("Lightning Shard");
            result.Add("Water Shard");

            //Crystals
            result.Add("Fire Crystal");
            result.Add("Ice Crystal");
            result.Add("Wind Crystal");
            result.Add("Earth Crystal");
            result.Add("Lightning Crystal");
            result.Add("Water Crystal");

            //Clusters
            result.Add("Fire Cluster");
            result.Add("Ice Cluster");
            result.Add("Wind Cluster");
            result.Add("Earth Cluster");
            result.Add("Lightning Cluster");
            result.Add("Water Cluster");

            return result;
        }

        public class MiqobotScenarioOption
        {
            public int Quantity = 1;
            public string GatheringRotation = "";
            public string CraftPreset = "";
            public string NQHQPreset = "";
            public string CustomTeleport = "";
            public bool IgnoreCatalysts = false;
            public bool Collectable = false;
            public int NbPerNode = 1;
            public string MiqoPresetPath = "";
            public Dictionary<string, int> CustomQuantities = new Dictionary<string, int>();
        }

        public static string GenerateScenario(FFXIVItem iItemToGenerate, MiqobotScenarioOption iOptions, System.Windows.Forms.TextBox iLogBox, out string fullScenario)
        {
            List<FFXIVItem> listScenarios = new List<FFXIVItem>();
            listScenarios.Add(iItemToGenerate);
            return GenerateScenario(listScenarios, iOptions, iLogBox, out fullScenario);
        }

        public static string GenerateScenario(List<FFXIVItem> iItemToGenerate, MiqobotScenarioOption iOptions, System.Windows.Forms.TextBox iLogBox, out string fullScenario)
        {
            fullScenario = "";
            if (null == iOptions)
            {
                Service_Misc.LogText(iLogBox, "Whoooops wrong options selected...");
                return null;
            }
            Service_Misc.LogText(iLogBox, "Let's whisper to miqobot ears...");

            MiqoCraftOptions options = new MiqoCraftOptions();
            options.Load(OptionLocation.UserOption);

            //Login to miqobot forums
            CookieCollection logMiqobotCookies = Miqobot.LogInForum();
            if (null != logMiqobotCookies) Service_Misc.LogText(iLogBox, "I'm logged into miqobot forum !");
            else Service_Misc.LogText(iLogBox, "Failed to log into miqobot forum... well you'll have to manually gather stuff, sorry !");

            //Listing items
            List<FFXIVItem> allItems = new List<FFXIVItem>();
            List<int> allItemsQuantity = new List<int>();
            string scenarioName = "";
            foreach (FFXIVItem iItem in iItemToGenerate)
            {
                if (null == iItem) continue;
                if (scenarioName != "") scenarioName += ",";
                scenarioName += iItem.Name;
                RecFindItems(iItem, iOptions.Quantity, ref allItems, ref allItemsQuantity, iOptions.CustomQuantities);
            }
            if (iItemToGenerate.Count > 5)
            {
                scenarioName = iItemToGenerate.Count + " Items";
            }

            fullScenario = "";
            string allGrids = "";
            string allRotations = "";
            string allPreset = "";

            List<string> catalysts = GetCatalysts();
            if (!iOptions.IgnoreCatalysts) catalysts.Clear();

            //Creating rotations
            allRotations += "gatherrotation.Collect Gathering +15%" + Environment.NewLine;
            allRotations += "[31,9,22,26,25,[34,[35,29,36,26]],25,[34,[35,29,36,26]],23,1,32]" + Environment.NewLine;
            allRotations += "gatherrotation.Collect Gathering +5%" + Environment.NewLine;
            allRotations += "[31,9,22,26,25,[34,[35,29,36,26]],25,[34,[35,29,36,26]],23,0]" + Environment.NewLine;
            allRotations += "gatherrotation.Gathering +15%/HQ +10%" + Environment.NewLine;
            allRotations += "[31,1,3]" + Environment.NewLine;
            allRotations += "gatherrotation.HQ +10%" + Environment.NewLine;
            allRotations += "[31,3]" + Environment.NewLine;
            allRotations += "gatherrotation.Gathering +5%/HQ +10%" + Environment.NewLine;
            allRotations += "[31,0,3]" + Environment.NewLine;
            allRotations += Miqobot.GetCacheRotations() + Environment.NewLine; ;

            //Creating default preset
            allPreset += "solverpreset.recommended" + Environment.NewLine;
            allPreset += "{\"cpchunk\":4,\"skillinnovation\":true,\"skillmanipulation\":false,\"skillwastenot1\":false,\"skillwastenot2\":false,\"skillwhistle\":false,\"progresssolver\":true,\"enforcebb100\":true,\"enforcepbp100\":true,\"ignorequality\":false,\"reclaimhqon\":false,\"reclaimhqvalue\":85,\"reclaimqualityon\":false,\"reclaimqualityvalue\":4000}" + Environment.NewLine;
            allPreset += Miqobot.GetCacheSolverPresets() + Environment.NewLine; ;

            //Header
            Service_Misc.LogText(iLogBox, "Creating scenario header...");
            fullScenario += "\"";
            fullScenario += "//--------------------------------------------------------------" + Environment.NewLine;
            fullScenario += "// " + scenarioName + Environment.NewLine;
            fullScenario += "// Script Generated by MiqoCrafter" + Environment.NewLine;
            fullScenario += "//--------------------------------------------------------------" + Environment.NewLine;
            fullScenario += "// Copyright 2019 - Shishio Valentine" + Environment.NewLine;
            fullScenario += "// shishio.valentine@gmail.com" + Environment.NewLine;
            fullScenario += "// http://patreon.com/miqocrafter" + Environment.NewLine;
            fullScenario += "//--------------------------------------------------------------" + Environment.NewLine;

            //Prerequisite
            fullScenario += Environment.NewLine;
            fullScenario += Environment.NewLine;
            fullScenario += "// Prerequisite" + Environment.NewLine;
            fullScenario += "//--------------------------------------------------------------" + Environment.NewLine;
            fullScenario += "//" + Environment.NewLine;
            if (iOptions.IgnoreCatalysts) fullScenario += "// You will need to buy or obtain those items using external means, cause Miqocrafter can't automate it [Yet], or they are ignored catalysts" + Environment.NewLine;
            else fullScenario += "// You will need to buy or obtain those items using external means, cause Miqocrafter can't automate it [Yet]" + Environment.NewLine;
            for (int i = 0; i < allItems.Count && i < allItemsQuantity.Count; i++)
            {
                FFXIVItem iItem = allItems[i];
                if (null == iItem) continue;
                int quantity = allItemsQuantity[i];

                FFXIVCraftingOptions itemOptions = options.GetOption(iItem);

                if (iItem.Type == FFXIVItem.TypeItem.NPC || iItem.Type == FFXIVItem.TypeItem.Unkwown || null != catalysts.Find(x => x != null && x.ToLower() == iItem.Name.ToLower()) || (null != itemOptions && itemOptions.IgnoreItem))
                {
                    fullScenario += "//    - " + quantity + "x " + iItem.Name + " (see " + iItem.UrlGarland + ")" + Environment.NewLine;
                }
            }

            //Reduced
            fullScenario += Environment.NewLine;
            fullScenario += Environment.NewLine;
            fullScenario += "// Reduced Items" + Environment.NewLine;
            fullScenario += "//--------------------------------------------------------------" + Environment.NewLine;
            fullScenario += "//" + Environment.NewLine;
            fullScenario += "// You will need to manually reduce those items, cause Miqocrafter can't automate it [Yet]" + Environment.NewLine;
            fullScenario += "// However Miqocrafter will try to gather/craft/retrieve the items that need to be reduced." + Environment.NewLine;
            for (int i = 0; i < allItems.Count && i < allItemsQuantity.Count; i++)
            {
                FFXIVItem iItem = allItems[i];
                if (null == iItem) continue;
                int quantity = allItemsQuantity[i];
                FFXIVReducedItem reducedItem = null;
                if (iItem is FFXIVReducedItem)
                {
                    reducedItem = iItem as FFXIVReducedItem;
                }
                if (null != reducedItem)
                {
                    if (null != reducedItem.ReducedFrom) fullScenario += "//    - " + iItem.Name + " (Reduced from " + reducedItem.ReducedFrom + " - " + iItem.UrlGarland + ")" + Environment.NewLine;
                    else fullScenario += "//    - " + quantity + "x " + iItem.Name + " (see " + iItem.UrlGarland + ")" + Environment.NewLine;
                }
            }

            //Errors
            fullScenario += "ERRORS_ITEMS_DUMMY";

            fullScenario += "\",";

            //Getting miqo preset content
            string miqoPresetContent = "";
            if (null != iOptions.MiqoPresetPath && "" != iOptions.MiqoPresetPath)
            {
                FileInfo presetFile = new FileInfo(Path.Combine(iOptions.MiqoPresetPath, "presets.miqo"));
                if (presetFile.Exists)
                {
                    miqoPresetContent = File.ReadAllText(presetFile.FullName);
                }
            }
            if (miqoPresetContent == "" && null != options.MiqoPresetPath && "" != options.MiqoPresetPath)
            {
                FileInfo presetFile = new FileInfo(Path.Combine(options.MiqoPresetPath, "presets.miqo"));
                if (presetFile.Exists)
                {
                    miqoPresetContent = File.ReadAllText(presetFile.FullName);
                }
            }

            //Gathering stuff
            string lastTeleport = "";
            string errorContent = "";
            Service_Misc.LogText(iLogBox, "Creating gathering grids...");
            fullScenario += "\"";
            fullScenario += Environment.NewLine;
            fullScenario += Environment.NewLine;
            fullScenario += "// Gathered Items" + Environment.NewLine;
            fullScenario += "//--------------------------------------------------------------" + Environment.NewLine;
            fullScenario += "//" + Environment.NewLine;
            fullScenario += "// Miqocrafter will use the gathering scenario from https://miqobot.com/forum/forums/topic/index-gathering-grids/ to retrieve those." + Environment.NewLine;
            for (int i = 0; i < allItems.Count && i < allItemsQuantity.Count; i++)
            {
                FFXIVItem iItem = allItems[i];
                if (null == iItem) continue;
                int quantity = allItemsQuantity[i] / iOptions.NbPerNode + 1;

                if (!(iItem is FFXIVGatheredItem)) continue;
                FFXIVGatheredItem gatheredItem = iItem as FFXIVGatheredItem;

                if (null != catalysts.Find(x => x != null && x.ToLower() == iItem.Name.ToLower())) continue;
                FFXIVCraftingOptions itemOptions = options.GetOption(iItem);
                if (null != itemOptions && itemOptions.IgnoreItem) continue;

                if (null != gatheredItem)
                {
                    Service_Misc.LogText(iLogBox, "Searching for item grid : " + iItem + "");
                    if (gatheredItem.Slot.Count <= 0)
                    {
                        errorContent += "//    - " + quantity + "x " + iItem.Name + " (see " + iItem.UrlGarland + ")" + Environment.NewLine;
                        fullScenario += "// Failed to retrieve item gathering slot from FFXIVGarlandTool, can't gather this item " + iItem.Name + " (see " + iItem.UrlGarland + ")" + Environment.NewLine;
                        continue;
                    }
                    if (null != logMiqobotCookies)
                    {
                        List<MiqoItemPage> listItemPage = Miqobot.GetURLItem(iItem.Name, logMiqobotCookies);
                        if (listItemPage.Count <= 0)
                        {
                            errorContent += "//    - " + quantity + "x " + iItem.Name + " (see " + iItem.UrlGarland + ")" + Environment.NewLine;
                            fullScenario += "// Failed to retrieve item gathering scenario from miqobot forums, can't gather this item " + iItem.Name + " (see " + iItem.UrlGarland + ")" + Environment.NewLine;
                        }
                        else
                        {
                            string initName = "";
                            string grid = null;
                            for (int j = 0; j < listItemPage.Count && null == grid; j++)
                            {
                                grid = Miqobot.GetGrid(iItem.Name, logMiqobotCookies, listItemPage[j], out initName);
                            }
                            if (null != grid)
                            {
                                //Finding right zone
                                string zone = "";
                                string type = "";
                                string slot = "";
                                string time = "";
                                string gatheringType = "";

                                if (gatheredItem.Slot.Count == 1)
                                {
                                    //No ambiguity
                                    zone = gatheredItem.Zones[0];
                                    type = gatheredItem.Types[0];
                                    slot = gatheredItem.Slot[0];
                                    time = gatheredItem.Times[0];
                                    gatheringType = gatheredItem.GatheringTypes[0];
                                }
                                else
                                {
                                    if (zone == "")
                                    {
                                        foreach (string zoneName in gatheredItem.Zones)
                                        {
                                            string initNameCorrected = initName.Replace(" ", "").ToLower();
                                            string gridDataCorrected = grid.Replace(" ", "").ToLower();
                                            string zoneNameCorrected = zoneName.ToLower().Replace("the ", "").Replace(" ", "").Trim();
                                            if (initNameCorrected.Contains(zoneNameCorrected))
                                            {
                                                int index = gatheredItem.Zones.IndexOf(zoneName);

                                                zone = gatheredItem.Zones[index];
                                                type = gatheredItem.Types[index];
                                                slot = gatheredItem.Slot[index];
                                                time = gatheredItem.Times[index];
                                                gatheringType = gatheredItem.GatheringTypes[index];
                                                break;
                                            }
                                            if (gridDataCorrected.Contains(zoneNameCorrected))
                                            {
                                                int index = gatheredItem.Zones.IndexOf(zoneName);

                                                zone = gatheredItem.Zones[index];
                                                type = gatheredItem.Types[index];
                                                slot = gatheredItem.Slot[index];
                                                time = gatheredItem.Times[index];
                                                gatheringType = gatheredItem.GatheringTypes[index];
                                                break;
                                            }
                                        }
                                    }
                                }
                                if (zone == "")
                                {
                                    errorContent += "//    - " + quantity + "x " + iItem.Name + " (see " + iItem.UrlGarland + ")" + Environment.NewLine;
                                    fullScenario += "// Failed to retrieve item gathering zone from miqobot grid, can't gather this item " + iItem.Name + " (see " + iItem.UrlGarland + ")" + Environment.NewLine;
                                    continue;
                                }

                                //Find teleport item
                                string teleportTo = FXIVGarlandTool.GetTeleportName(zone, grid, gatheredItem);

                                //Compute grid name from miqo presets, to avoid duplicates
                                string gridName = iItem.Name + " Grid";
                                int indexGrid = 1;
                                while (miqoPresetContent.Contains("grid." + gridName + Environment.NewLine))
                                {
                                    string oldGridName = gridName;
                                    gridName = iItem.Name + " Grid" + indexGrid;
                                    grid = grid.Replace("grid." + oldGridName + Environment.NewLine, "grid." + gridName + Environment.NewLine);
                                    indexGrid++;
                                }

                                //Embed grid
                                allGrids += grid + Environment.NewLine;

                                //Get preset
                                string preset = Miqobot.GetCraftingPreset(gatheringType, slot, gatheredItem, iOptions.GatheringRotation, gridName, time);

                                //Compute preset name from miqo presets, to avoid duplicates
                                string presetName = iItem.Name + " preset";
                                int indexpreset = 1;
                                while (miqoPresetContent.Contains("gatherpreset." + presetName + Environment.NewLine))
                                {
                                    string oldpresetName = presetName;
                                    presetName = iItem.Name + " preset" + indexpreset;
                                    preset = preset.Replace("gatherpreset." + oldpresetName + Environment.NewLine, "gatherpreset." + presetName + Environment.NewLine);
                                    indexpreset++;
                                }

                                //Embed a new preset
                                allPreset += preset;

                                //Add gathering rotation to scenario
                                //teleportIf(Black Brush Station)\r\nunstealth()\r\nchangeJob(Miner)\r\nselectGrid(Min5-Copper Ore)\r\nselectGatherPreset(Metal Worm Jar- Copper Ore)\r\nstartGathering(4)
                                //teleportIfNotThere
                                fullScenario += "// Gathering " + iItem.Name + Environment.NewLine;
                                if (lastTeleport == "" || lastTeleport == teleportTo)
                                {
                                    fullScenario += "teleport(" + teleportTo + ")" + Environment.NewLine;
                                }
                                else
                                {
                                    fullScenario += "teleportIfNotThere(" + teleportTo + ")" + Environment.NewLine;
                                }
                                lastTeleport = teleportTo;

                                //Adding custom scenario after teleport
                                DirectoryInfo customDirectory = new DirectoryInfo(Path.Combine(Service_Misc.GetExecutionPath(), "CustomTeleport"));
                                if (!customDirectory.Exists)
                                {
                                    customDirectory.Create();
                                }
                                FileInfo customTeleportScenarioFile = new FileInfo(Path.Combine(customDirectory.FullName, teleportTo + " Scenario.txt"));
                                if (customTeleportScenarioFile.Exists)
                                {
                                    fullScenario += File.ReadAllText(customTeleportScenarioFile.FullName) + Environment.NewLine;
                                }

                                FileInfo customTeleportGridFile = new FileInfo(Path.Combine(customDirectory.FullName, teleportTo + " Grid.txt"));
                                if (customTeleportGridFile.Exists)
                                {
                                    allGrids += File.ReadAllText(customTeleportGridFile.FullName) + Environment.NewLine;
                                }


                                //fullScenario += "unstealth()" + Environment.NewLine;
                                fullScenario += "changeJob(" + FXIVGarlandTool.GetGatheringJobName(gatheringType, iItem) + ")" + Environment.NewLine;
                                fullScenario += "selectGrid(" + gridName + ")" + Environment.NewLine;
                                fullScenario += "selectGatherPreset(" + presetName + ")" + Environment.NewLine;
                                if (null != gatheredItem && gatheredItem.AsCollectable)
                                {
                                    fullScenario += "rotationIfGP(470 Collect 5% Gathering)" + Environment.NewLine;
                                    fullScenario += "rotationIfGP(470 Collect 15% Gathering)" + Environment.NewLine;
                                }
                                else
                                {
                                    fullScenario += "rotationIfGP(" + iOptions.GatheringRotation + ")" + Environment.NewLine;
                                }
                                fullScenario += "startGathering(" + quantity + ")" + Environment.NewLine;
                                fullScenario += Environment.NewLine;
                            }
                            else
                            {
                                errorContent += "//    - " + quantity + "x " + iItem.Name + " (see " + iItem.UrlGarland + ")" + Environment.NewLine;
                                fullScenario += "// Failed to retrieve the grid from miqobot forums, can't gather this item " + iItem.Name + " (see " + iItem.UrlGarland + ")" + Environment.NewLine;
                            }
                        }
                    }
                    else
                    {
                        errorContent += "//    - " + quantity + "x " + iItem.Name + " (see " + iItem.UrlGarland + ")" + Environment.NewLine;
                        fullScenario += "// Failed to log into miqobot forums, can't gather this item " + iItem.Name + " (see " + iItem.UrlGarland + ")" + Environment.NewLine;
                    }
                }
            }
            fullScenario += "\",";

            //Crafting stuff
            Service_Misc.LogText(iLogBox, "Generating craft scenario...");
            fullScenario += "\"";
            fullScenario += Environment.NewLine;
            fullScenario += Environment.NewLine;
            fullScenario += "// Crafted Items" + Environment.NewLine;
            fullScenario += "//--------------------------------------------------------------" + Environment.NewLine;
            fullScenario += "//" + Environment.NewLine;
            fullScenario += "solverPreset(" + iOptions.CraftPreset + ")" + Environment.NewLine;
            fullScenario += "nqhq(" + iOptions.NQHQPreset + ")" + Environment.NewLine;
            fullScenario += "reclaimOff()" + Environment.NewLine;

            string teleportCraft = iOptions.CustomTeleport;
            if (teleportCraft != "")
            {
                fullScenario += "teleport(" + teleportCraft + ")" + Environment.NewLine;
            }

            List<FFXIVCraftedItem> listAllCraftedItems = new List<FFXIVCraftedItem>();
            for (int i = 0; i < allItems.Count && i < allItemsQuantity.Count; i++)
            {
                FFXIVItem iItem = allItems[i];
                if (null == iItem) continue;
                int quantity = allItemsQuantity[i];
                FFXIVCraftedItem craftedItem = null;
                if (iItem is FFXIVCraftedItem)
                {
                    craftedItem = iItem as FFXIVCraftedItem;
                }
                if (null != craftedItem)
                {
                    listAllCraftedItems.Add(craftedItem);
                }
            }
            for (int i = 0; i < allItems.Count && i < allItemsQuantity.Count; i++)
            {
                FFXIVItem iItem = allItems[i];
                if (null == iItem) continue;
                int quantity = allItemsQuantity[i];
                FFXIVCraftedItem craftedItem = null;
                if (iItem is FFXIVCraftedItem)
                {
                    craftedItem = iItem as FFXIVCraftedItem;
                }
                FFXIVCraftingOptions itemOptions = options.GetOption(iItem);
                if (null != itemOptions && itemOptions.IgnoreItem) continue;

                if (null != craftedItem)
                {
                    fullScenario += "// " + craftedItem + Environment.NewLine;

                    int indexCraftedItem = listAllCraftedItems.IndexOf(craftedItem);
                    if (listAllCraftedItems.IndexOf(craftedItem) >= listAllCraftedItems.Count - 1 && iOptions.Collectable)
                    {
                        fullScenario += "setCraftCollect(on)" + Environment.NewLine;
                    }
                    else
                    {
                        fullScenario += "setCraftCollect(off)" + Environment.NewLine;
                    }

                    fullScenario += "job(" + craftedItem.Class + ")" + Environment.NewLine;
                    fullScenario += "recipe(" + craftedItem.Name + ")" + Environment.NewLine;

                    if (null != itemOptions && itemOptions.CustomCraftingMacro != "")
                    {
                        fullScenario += "selectCraftMacro(" + itemOptions.CustomCraftingMacro + ")" + Environment.NewLine;
                    }

                    fullScenario += "craft(" + quantity + ")" + Environment.NewLine;

                    if (listAllCraftedItems.IndexOf(craftedItem) >= listAllCraftedItems.Count - 1 && iOptions.Collectable)
                    {
                        fullScenario += "setCraftCollect(off)" + Environment.NewLine;
                    }

                    if (null != itemOptions && itemOptions.CustomCraftingMacro != "")
                    {
                        fullScenario += "solverPreset(" + iOptions.CraftPreset + ")" + Environment.NewLine;
                    }
                    fullScenario += Environment.NewLine;
                }
            }
            fullScenario += "\"";

            if (errorContent == "")
            {
                fullScenario = fullScenario.Replace("ERRORS_ITEMS_DUMMY", "");
            }
            else
            {
                string scenarioError = "";
                scenarioError += Environment.NewLine;
                scenarioError += Environment.NewLine;
                scenarioError += "// Items without grid / Miqocrafter couldn't automate" + Environment.NewLine;
                scenarioError += "//--------------------------------------------------------------" + Environment.NewLine;
                scenarioError += "//" + Environment.NewLine;
                scenarioError += "// You will need to buy or obtain those items using external means, cause Miqocrafter can't automate it [Yet]" + Environment.NewLine;
                scenarioError += errorContent;
                scenarioError += Environment.NewLine;
                fullScenario = fullScenario.Replace("ERRORS_ITEMS_DUMMY", scenarioError);
            }

            string textFileContent = "scenario.Craft " + scenarioName + Environment.NewLine;
            textFileContent += "{ \"chapters\":[";
            textFileContent += fullScenario.Replace(Environment.NewLine, "\\r\\n").Replace("/", "\\/");
            textFileContent += "]}" + Environment.NewLine;

            textFileContent += allRotations + Environment.NewLine;
            textFileContent += allGrids + Environment.NewLine;
            textFileContent += allPreset + Environment.NewLine;

            while (textFileContent.Contains(Environment.NewLine + Environment.NewLine))
            {
                textFileContent = textFileContent.Replace(Environment.NewLine + Environment.NewLine, Environment.NewLine);
            }

            return textFileContent;
        }

        static string[] Scopes = { DriveService.Scope.DriveReadonly };
        //220680762635-qg38qgp1g1bbmkdjma1o50df199sbsjm.apps.googleusercontent.com
        //3Iub0pwzrs3-tCB4UyP4dgig
        //220680762635-qg38qgp1g1bbmkdjma1o50df199sbsjm.apps.googleusercontent.com
        public static void DownloadGrids()
        {
            FileStream fileStream = null;
            try
            {
                UserCredential credential;

                using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
                {
                    // The file token.json stores the user's access and refresh tokens, and is created
                    // automatically when the authorization flow completes for the first time.
                    string credPath = "token.json";
                    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        Scopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true)).Result;
                }

                // Create Drive API service.
                var service = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "miqocrafter",
                });

                // Define parameters of request.
                FilesResource.ListRequest listRequest = service.Files.List();
                listRequest.PageSize = 10;
                listRequest.Fields = "nextPageToken, files(id, name)";

                // List files.
                IList<Google.Apis.Drive.v3.Data.File> files = listRequest.Execute().Files;

                if (files != null && files.Count > 0)
                {
                    foreach (var file in files)
                    {
                        if (file.Name == "Release.zip")
                        {
                            //Delete old zip if any
                            FileInfo zipPath = new FileInfo(Path.Combine(Service_Misc.GetExecutionPath(), "Release.zip"));
                            if (zipPath.Exists)
                            {
                                zipPath.Delete();
                            }

                            //Download new zip
                            String fileId = file.Id;
                            fileStream = File.Create(zipPath.FullName);
                            Stream stream = fileStream;
                            service.Files.Get(fileId).Download(stream);
                            fileStream.Close();
                            fileStream = null;

                            //Unzip
                            fileStream = File.OpenRead(zipPath.FullName);
                            using (ZipArchive archive = new ZipArchive(fileStream))
                            {
                                archive.ExtractToDirectory(Service_Misc.GetExecutionPath(), true);
                            }
                            fileStream.Close();
                            fileStream = null;
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            if(null != fileStream)
            {
                fileStream.Close();
            }
        }
    }
}

