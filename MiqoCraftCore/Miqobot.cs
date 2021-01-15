using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace MiqoCraftCore
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

        public static List<MiqoItemPage> GetURLItem(string iItemName, CookieCollection iLogCookies, HtmlDocument iDocument)
        {
            List<MiqoItemPage> result = new List<MiqoItemPage>();

            CookieCollection oCookies = new CookieCollection();
            HttpStatusCode oCode = HttpStatusCode.NotFound;
            HtmlDocument answer = iDocument;
            if (null == answer) answer = Service_Misc.GetWebPageFromRequest("GET https://miqobot.com/forum/forums/topic/index-gathering-grids/ HTTP/1.1|Host: miqobot.com|Connection: keep-alive|Cache-Control: max-age=0|Upgrade-Insecure-Requests: 1|User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36|Sec-Fetch-Mode: navigate|Sec-Fetch-User: ?1|Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3|Sec-Fetch-Site: same-origin|Referer: https://miqobot.com/forum/forums/forum/grids-and-presets/|Accept-Encoding: gzip, deflate, br|Accept-Language: fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7|Cookie: wordpress_test_cookie=WP+Cookie+check; _ga=GA1.2.1771485810.1566089776||",
                 iLogCookies, ref oCookies, ref oCode);
            if (null == answer) return result;

            HtmlNode firstAnswerNode = Service_Misc.GetChildNodeByID(answer.DocumentNode, "d4p-bbp-quote-1116");
            if (null == firstAnswerNode) return new List<MiqoItemPage>();

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

        public static List<string> GetAllGridsFromForum(string iItemName, CookieCollection iLogCookies, MiqoItemPage iPage)
        {
            string gridItemName = iItemName + " Grid";
            CookieCollection oCookies = new CookieCollection();
            HttpStatusCode oCode = HttpStatusCode.NotFound;

            HtmlDocument answer = Service_Misc.GetWebPageFromRequest("GET " + iPage.URL + " HTTP/1.1|Host: miqobot.com|Connection: keep-alive|Cache-Control: max-age=0|Upgrade-Insecure-Requests: 1|User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36|Sec-Fetch-Mode: navigate|Sec-Fetch-User: ?1|Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3|Sec-Fetch-Site: same-origin|Referer: https://miqobot.com/forum/wp-login.php?redirect_to=https%3A%2F%2Fmiqobot.com%2Fforum%2Fforums%2Ftopic%2Fgrade-1-carbonized-matter-min-lv20%2F|Accept-Encoding: gzip, deflate, br|Accept-Language: fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7|",
                iLogCookies, ref oCookies, ref oCode);
            if (null == answer) return null;

            List<string> attachmentsList = new List<string>();
            List<HtmlNode> listAllAttachmentsNode = answer.DocumentNode.Descendants("div").ToList();
            foreach (HtmlNode node in listAllAttachmentsNode)
            {
                if (node.GetAttributeValue("class", "") == "bbp-attachments")
                {
                    attachmentsList.Add(Service_Misc.ExtractLink(node));
                }
            }

            List<string> listGrids = new List<string>();
            foreach (string attachmentLink in attachmentsList)
            {
                string gridRawContent = Service_Misc.GetContentFromRequest("GET " + attachmentLink + " HTTP/1.1|Host: miqobot.com|Connection: keep-alive|Upgrade-Insecure-Requests: 1|User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36|Sec-Fetch-Mode: navigate|Sec-Fetch-User: ?1|Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3|Sec-Fetch-Site: same-origin|Referer: https://miqobot.com/forum/forums/topic/grade-1-carbonized-matter-min-lv20/|Accept-Encoding: gzip, deflate, br|Accept-Language: fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7|",
                    iLogCookies, ref oCookies, ref oCode);

                string oGridName = "";
                try
                {
                    listGrids.Add(GetGridFromRawContent(iItemName, gridRawContent, out oGridName));
                }
                catch
                {

                }
            }
            return listGrids;
        }

        public static string GetLastGrid(string iItemName, CookieCollection iLogCookies, MiqoItemPage iPage, out string oGridName)
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

            return GetGridFromRawContent(iItemName, gridRawContent, out oGridName);
        }

        /// <summary>
        /// Parse a grid raw content and return a formatted grid content
        /// </summary>
        /// <param name="iRawContent"></param>
        /// <returns></returns>
        public static string GetGridFromRawContent(string iItemName, string iRawContent, out string oGridName)
        {
            string gridItemName = iItemName + " Grid";
            oGridName = "";
            string gridRawContent = iRawContent;
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

        public static List<MiqobotGrid> GetAllGridsFromFile(FileInfo iFile)
        {
            string gridRawContent = File.ReadAllText(iFile.FullName);
            return GetAllGridsFromContent(gridRawContent);
        }
        public static List<MiqobotGrid> GetAllGridsFromContent(string iContent)
        {
            List<MiqobotGrid> grids = new List<MiqobotGrid>();
            if (null == iContent) return grids;

            string gridRawContent = iContent;
            if (null == gridRawContent || "" == gridRawContent || !gridRawContent.Contains("{")) return grids;

            //Need to parse it further !
            Dictionary<string, JToken> listGatherPreset = new Dictionary<string, JToken>();
            Dictionary<string, string> listGrids = new Dictionary<string, string>();
            List<string> lines = gridRawContent.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            for (int i = 0; i < lines.Count - 1; i += 2)
            {
                string lineHeader = lines[i];
                string lineContent = lines[i + 1];

                try
                {
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
                catch
                {

                }
            }

            if (listGrids.Count > 0)
            {
                foreach (KeyValuePair<string, string> pair in listGrids)
                {
                    MiqobotGrid grid = new MiqobotGrid();
                    grid.Header = pair.Key;
                    grid.ParseFromLine(pair.Value);
                    grids.Add(grid);
                }
            }

            return grids;
        }


        public static List<string> GetAllGridsRawContentFromForum(string iURL, CookieCollection iLogCookies)
        {
            CookieCollection oCookies = new CookieCollection();
            HttpStatusCode oCode = HttpStatusCode.NotFound;

            HtmlDocument answer = Service_Misc.GetWebPageFromRequest("GET " + iURL + " HTTP/1.1|Host: miqobot.com|Connection: keep-alive|Cache-Control: max-age=0|Upgrade-Insecure-Requests: 1|User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36|Sec-Fetch-Mode: navigate|Sec-Fetch-User: ?1|Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3|Sec-Fetch-Site: same-origin|Referer: https://miqobot.com/forum/wp-login.php?redirect_to=https%3A%2F%2Fmiqobot.com%2Fforum%2Fforums%2Ftopic%2Fgrade-1-carbonized-matter-min-lv20%2F|Accept-Encoding: gzip, deflate, br|Accept-Language: fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7|",
                iLogCookies, ref oCookies, ref oCode);
            if (null == answer) return null;

            List<string> attachmentsList = new List<string>();
            List<HtmlNode> listAllAttachmentsNode = answer.DocumentNode.Descendants("div").ToList();
            foreach (HtmlNode node in listAllAttachmentsNode)
            {
                if (node.GetAttributeValue("class", "") == "bbp-attachments")
                {
                    attachmentsList.Add(Service_Misc.ExtractLink(node));
                }
            }

            List<string> listGrids = new List<string>();
            foreach (string attachmentLink in attachmentsList)
            {
                string gridRawContent = Service_Misc.GetContentFromRequest("GET " + attachmentLink + " HTTP/1.1|Host: miqobot.com|Connection: keep-alive|Upgrade-Insecure-Requests: 1|User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36|Sec-Fetch-Mode: navigate|Sec-Fetch-User: ?1|Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3|Sec-Fetch-Site: same-origin|Referer: https://miqobot.com/forum/forums/topic/grade-1-carbonized-matter-min-lv20/|Accept-Encoding: gzip, deflate, br|Accept-Language: fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7|",
                    iLogCookies, ref oCookies, ref oCode);

                listGrids.Add(gridRawContent);
            }
            return listGrids;
        }
    }
}


