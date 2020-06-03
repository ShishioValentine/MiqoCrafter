using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;

namespace MiqoCraftCore
{
    public static class ConsoleGamesWiki
    {
        public static List<FFXIVGatheringNode> GetFFXIVGatheringNodes()
        {
            List<FFXIVGatheringNode> result = new List<FFXIVGatheringNode>();

            //MIN Node
            CookieCollection iCookies = new CookieCollection();
            CookieCollection oCookies = new CookieCollection();
            HttpStatusCode oCode = HttpStatusCode.NotFound;

            HtmlDocument answer = Service_Misc.GetWebPageFromRequest("GET https://ffxiv.consolegameswiki.com/wiki/Mining_Node_Locations HTTP/1.1|Host: ffxiv.consolegameswiki.com|Connection: keep-alive|Pragma: no-cache|Cache-Control: no-cache|Upgrade-Insecure-Requests: 1|User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.88 Safari/537.36|Sec-Fetch-User: ?1|Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9|Sec-Fetch-Site: same-origin|Sec-Fetch-Mode: navigate|Referer: https://ffxiv.consolegameswiki.com/wiki/FF14_Wiki|Accept-Encoding: gzip, deflate|Accept-Language: fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7|Cookie: __cfduid=d64998d831d683525635413f445baf9671577745615; _ga=GA1.2.147161069.1577745617; _gid=GA1.2.132489683.1577745617; trc_cookie_storage=taboola%2520global%253Auser-id%3D8ee6f8c7-39f8-4db7-b620-5868e971d900-tuct4ec3105||",
                iCookies, ref oCookies, ref oCode);
            if (null != answer)
            {
                foreach (HtmlNode node in answer.DocumentNode.Descendants("table"))
                {
                    result.AddRange(GetFFXIVGatheringNodesFromTable(node, "MIN", FFXIVGatheringNode.NodeType.Standard));
                }
            }
            answer = Service_Misc.GetWebPageFromRequest("GET https://ffxiv.consolegameswiki.com/wiki/Unspoiled_Mining_Nodes HTTP/1.1|Host: ffxiv.consolegameswiki.com|Connection: keep-alive|Pragma: no-cache|Cache-Control: no-cache|Upgrade-Insecure-Requests: 1|User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.88 Safari/537.36|Sec-Fetch-User: ?1|Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9|Sec-Fetch-Site: same-origin|Sec-Fetch-Mode: navigate|Referer: https://ffxiv.consolegameswiki.com/wiki/FF14_Wiki|Accept-Encoding: gzip, deflate|Accept-Language: fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7|Cookie: __cfduid=d64998d831d683525635413f445baf9671577745615; _ga=GA1.2.147161069.1577745617; _gid=GA1.2.132489683.1577745617; trc_cookie_storage=taboola%2520global%253Auser-id%3D8ee6f8c7-39f8-4db7-b620-5868e971d900-tuct4ec3105||",
                iCookies, ref oCookies, ref oCode);
            if (null != answer)
            {
                foreach (HtmlNode node in answer.DocumentNode.Descendants("table"))
                {
                    result.AddRange(GetFFXIVGatheringNodesFromTable(node, "MIN", FFXIVGatheringNode.NodeType.Unspoiled));
                }
            }

            //BTN Node
            answer = Service_Misc.GetWebPageFromRequest("GET https://ffxiv.consolegameswiki.com/wiki/Botanist_Node_Locations HTTP/1.1|Host: ffxiv.consolegameswiki.com|Connection: keep-alive|Pragma: no-cache|Cache-Control: no-cache|Upgrade-Insecure-Requests: 1|User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.88 Safari/537.36|Sec-Fetch-User: ?1|Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9|Sec-Fetch-Site: same-origin|Sec-Fetch-Mode: navigate|Referer: https://ffxiv.consolegameswiki.com/wiki/FF14_Wiki|Accept-Encoding: gzip, deflate|Accept-Language: fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7|Cookie: __cfduid=d64998d831d683525635413f445baf9671577745615; _ga=GA1.2.147161069.1577745617; _gid=GA1.2.132489683.1577745617; trc_cookie_storage=taboola%2520global%253Auser-id%3D8ee6f8c7-39f8-4db7-b620-5868e971d900-tuct4ec3105||",
                            iCookies, ref oCookies, ref oCode);
            if (null != answer)
            {
                foreach (HtmlNode node in answer.DocumentNode.Descendants("table"))
                {
                    result.AddRange(GetFFXIVGatheringNodesFromTable(node, "BTN", FFXIVGatheringNode.NodeType.Standard));
                }
            }
            answer = Service_Misc.GetWebPageFromRequest("GET https://ffxiv.consolegameswiki.com/wiki/Unspoiled_Botanist_Nodes HTTP/1.1|Host: ffxiv.consolegameswiki.com|Connection: keep-alive|Pragma: no-cache|Cache-Control: no-cache|Upgrade-Insecure-Requests: 1|User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.88 Safari/537.36|Sec-Fetch-User: ?1|Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9|Sec-Fetch-Site: same-origin|Sec-Fetch-Mode: navigate|Referer: https://ffxiv.consolegameswiki.com/wiki/FF14_Wiki|Accept-Encoding: gzip, deflate|Accept-Language: fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7|Cookie: __cfduid=d64998d831d683525635413f445baf9671577745615; _ga=GA1.2.147161069.1577745617; _gid=GA1.2.132489683.1577745617; trc_cookie_storage=taboola%2520global%253Auser-id%3D8ee6f8c7-39f8-4db7-b620-5868e971d900-tuct4ec3105||",
                iCookies, ref oCookies, ref oCode);
            if (null != answer)
            {
                foreach (HtmlNode node in answer.DocumentNode.Descendants("table"))
                {
                    result.AddRange(GetFFXIVGatheringNodesFromTable(node, "BTN", FFXIVGatheringNode.NodeType.Unspoiled));
                }
            }

            //FSH Node
            answer = Service_Misc.GetWebPageFromRequest("GET https://ffxiv.consolegameswiki.com/wiki/Heavensward_Fishing_Locations HTTP/1.1|Host: ffxiv.consolegameswiki.com|Connection: keep-alive|Pragma: no-cache|Cache-Control: no-cache|Upgrade-Insecure-Requests: 1|User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.88 Safari/537.36|Sec-Fetch-User: ?1|Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9|Sec-Fetch-Site: same-origin|Sec-Fetch-Mode: navigate|Referer: https://ffxiv.consolegameswiki.com/wiki/FF14_Wiki|Accept-Encoding: gzip, deflate|Accept-Language: fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7|Cookie: __cfduid=d64998d831d683525635413f445baf9671577745615; _ga=GA1.2.147161069.1577745617; _gid=GA1.2.132489683.1577745617; trc_cookie_storage=taboola%2520global%253Auser-id%3D8ee6f8c7-39f8-4db7-b620-5868e971d900-tuct4ec3105||",
                            iCookies, ref oCookies, ref oCode);
            if (null != answer)
            {
                foreach (HtmlNode node in answer.DocumentNode.Descendants("table"))
                {
                    result.AddRange(GetFFXIVGatheringNodesFromTable(node, "FSH", FFXIVGatheringNode.NodeType.Standard));
                }
            }
            answer = Service_Misc.GetWebPageFromRequest("GET https://ffxiv.consolegameswiki.com/wiki/Stormblood_Spearfishing_Locations HTTP/1.1|Host: ffxiv.consolegameswiki.com|Connection: keep-alive|Pragma: no-cache|Cache-Control: no-cache|Upgrade-Insecure-Requests: 1|User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.88 Safari/537.36|Sec-Fetch-User: ?1|Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9|Sec-Fetch-Site: same-origin|Sec-Fetch-Mode: navigate|Referer: https://ffxiv.consolegameswiki.com/wiki/FF14_Wiki|Accept-Encoding: gzip, deflate|Accept-Language: fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7|Cookie: __cfduid=d64998d831d683525635413f445baf9671577745615; _ga=GA1.2.147161069.1577745617; _gid=GA1.2.132489683.1577745617; trc_cookie_storage=taboola%2520global%253Auser-id%3D8ee6f8c7-39f8-4db7-b620-5868e971d900-tuct4ec3105||",
                            iCookies, ref oCookies, ref oCode);
            if (null != answer)
            {
                foreach (HtmlNode node in answer.DocumentNode.Descendants("table"))
                {
                    result.AddRange(GetFFXIVGatheringNodesFromTable(node, "FSH", FFXIVGatheringNode.NodeType.Standard));
                }
            }
            answer = Service_Misc.GetWebPageFromRequest("GET https://ffxiv.consolegameswiki.com/wiki/Stormblood_Fishing_Locations HTTP/1.1|Host: ffxiv.consolegameswiki.com|Connection: keep-alive|Pragma: no-cache|Cache-Control: no-cache|Upgrade-Insecure-Requests: 1|User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.88 Safari/537.36|Sec-Fetch-User: ?1|Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9|Sec-Fetch-Site: same-origin|Sec-Fetch-Mode: navigate|Referer: https://ffxiv.consolegameswiki.com/wiki/FF14_Wiki|Accept-Encoding: gzip, deflate|Accept-Language: fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7|Cookie: __cfduid=d64998d831d683525635413f445baf9671577745615; _ga=GA1.2.147161069.1577745617; _gid=GA1.2.132489683.1577745617; trc_cookie_storage=taboola%2520global%253Auser-id%3D8ee6f8c7-39f8-4db7-b620-5868e971d900-tuct4ec3105||",
                            iCookies, ref oCookies, ref oCode);
            if (null != answer)
            {
                foreach (HtmlNode node in answer.DocumentNode.Descendants("table"))
                {
                    result.AddRange(GetFFXIVGatheringNodesFromTable(node, "FSH", FFXIVGatheringNode.NodeType.Standard));
                }
            }
            answer = Service_Misc.GetWebPageFromRequest("GET https://ffxiv.consolegameswiki.com/wiki/Shadowbringers_Spearfishing_Locations HTTP/1.1|Host: ffxiv.consolegameswiki.com|Connection: keep-alive|Pragma: no-cache|Cache-Control: no-cache|Upgrade-Insecure-Requests: 1|User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.88 Safari/537.36|Sec-Fetch-User: ?1|Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9|Sec-Fetch-Site: same-origin|Sec-Fetch-Mode: navigate|Referer: https://ffxiv.consolegameswiki.com/wiki/FF14_Wiki|Accept-Encoding: gzip, deflate|Accept-Language: fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7|Cookie: __cfduid=d64998d831d683525635413f445baf9671577745615; _ga=GA1.2.147161069.1577745617; _gid=GA1.2.132489683.1577745617; trc_cookie_storage=taboola%2520global%253Auser-id%3D8ee6f8c7-39f8-4db7-b620-5868e971d900-tuct4ec3105||",
                            iCookies, ref oCookies, ref oCode);
            if (null != answer)
            {
                foreach (HtmlNode node in answer.DocumentNode.Descendants("table"))
                {
                    result.AddRange(GetFFXIVGatheringNodesFromTable(node, "FSH", FFXIVGatheringNode.NodeType.Standard));
                }
            }
            answer = Service_Misc.GetWebPageFromRequest("GET https://ffxiv.consolegameswiki.com/wiki/Shadowbringers_Fishing_Locations HTTP/1.1|Host: ffxiv.consolegameswiki.com|Connection: keep-alive|Pragma: no-cache|Cache-Control: no-cache|Upgrade-Insecure-Requests: 1|User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.88 Safari/537.36|Sec-Fetch-User: ?1|Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9|Sec-Fetch-Site: same-origin|Sec-Fetch-Mode: navigate|Referer: https://ffxiv.consolegameswiki.com/wiki/FF14_Wiki|Accept-Encoding: gzip, deflate|Accept-Language: fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7|Cookie: __cfduid=d64998d831d683525635413f445baf9671577745615; _ga=GA1.2.147161069.1577745617; _gid=GA1.2.132489683.1577745617; trc_cookie_storage=taboola%2520global%253Auser-id%3D8ee6f8c7-39f8-4db7-b620-5868e971d900-tuct4ec3105||",
                            iCookies, ref oCookies, ref oCode);
            if (null != answer)
            {
                foreach (HtmlNode node in answer.DocumentNode.Descendants("table"))
                {
                    result.AddRange(GetFFXIVGatheringNodesFromTable(node, "FSH", FFXIVGatheringNode.NodeType.Standard));
                }
            }

            //Ephemeral Node
            answer = Service_Misc.GetWebPageFromRequest("GET https://ffxiv.consolegameswiki.com/wiki/Ephemeral_Nodes HTTP/1.1|Host: ffxiv.consolegameswiki.com|Connection: keep-alive|Pragma: no-cache|Cache-Control: no-cache|Upgrade-Insecure-Requests: 1|User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.88 Safari/537.36|Sec-Fetch-User: ?1|Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9|Sec-Fetch-Site: same-origin|Sec-Fetch-Mode: navigate|Referer: https://ffxiv.consolegameswiki.com/wiki/FF14_Wiki|Accept-Encoding: gzip, deflate|Accept-Language: fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7|Cookie: __cfduid=d64998d831d683525635413f445baf9671577745615; _ga=GA1.2.147161069.1577745617; _gid=GA1.2.132489683.1577745617; trc_cookie_storage=taboola%2520global%253Auser-id%3D8ee6f8c7-39f8-4db7-b620-5868e971d900-tuct4ec3105||",
                iCookies, ref oCookies, ref oCode);
            if (null != answer)
            {
                HtmlNode contentNode = answer.GetElementbyId("mw-content-text");
                if (null == contentNode) return result;

                string currentRegion = "Unknown";
                foreach (HtmlNode childNode in contentNode.ChildNodes)
                {
                    if (childNode.Name == "h3")
                    {
                        currentRegion = childNode.InnerText.Trim();
                        if (currentRegion.ToLower().Contains("fish")) currentRegion = "FSH";
                        else if (currentRegion.ToLower().Contains("min")) currentRegion = "MIN";
                        else if (currentRegion.ToLower().Contains("bot") || currentRegion.ToLower().Contains("btn")) currentRegion = "BTN";
                    }
                    else if (childNode.Name == "table" && (currentRegion == "BTN" || currentRegion == "MIN" || currentRegion == "FSH"))
                    {
                        result.AddRange(GetFFXIVGatheringNodesFromTable(childNode, currentRegion, FFXIVGatheringNode.NodeType.Ephemeral));
                    }
                }
            }

            //Folklore Node
            answer = Service_Misc.GetWebPageFromRequest("GET https://ffxiv.consolegameswiki.com/wiki/Folklore_Nodes HTTP/1.1|Host: ffxiv.consolegameswiki.com|Connection: keep-alive|Pragma: no-cache|Cache-Control: no-cache|Upgrade-Insecure-Requests: 1|User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.88 Safari/537.36|Sec-Fetch-User: ?1|Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9|Sec-Fetch-Site: same-origin|Sec-Fetch-Mode: navigate|Referer: https://ffxiv.consolegameswiki.com/wiki/FF14_Wiki|Accept-Encoding: gzip, deflate|Accept-Language: fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7|Cookie: __cfduid=d64998d831d683525635413f445baf9671577745615; _ga=GA1.2.147161069.1577745617; _gid=GA1.2.132489683.1577745617; trc_cookie_storage=taboola%2520global%253Auser-id%3D8ee6f8c7-39f8-4db7-b620-5868e971d900-tuct4ec3105||",
                iCookies, ref oCookies, ref oCode);
            if (null != answer)
            {
                HtmlNode contentNode = answer.GetElementbyId("mw-content-text");
                if (null == contentNode) return result;

                string currentRegion = "Unknown";
                foreach (HtmlNode childNode in contentNode.ChildNodes)
                {
                    if (childNode.Name == "h2")
                    {
                        currentRegion = childNode.InnerText.Trim();
                        if (currentRegion.ToLower().Contains("fish")) currentRegion = "FSH";
                        else if (currentRegion.ToLower().Contains("min")) currentRegion = "MIN";
                        else if (currentRegion.ToLower().Contains("bot") || currentRegion.ToLower().Contains("btn")) currentRegion = "BTN";
                    }
                    else if (childNode.Name == "table" && (currentRegion == "BTN" || currentRegion == "MIN" || currentRegion == "FSH"))
                    {
                        result.AddRange(GetFFXIVGatheringNodesFromTable(childNode, currentRegion, FFXIVGatheringNode.NodeType.Folklore));
                    }
                }
            }


            return result;
        }

        public static List<FFXIVGatheringNode> GetFFXIVGatheringNodesFromTable(HtmlNode iNode, string iJobName, FFXIVGatheringNode.NodeType iType = FFXIVGatheringNode.NodeType.Standard)
        {
            List<FFXIVGatheringNode> result = new List<FFXIVGatheringNode>();
            if (null == iNode) return result;

            DirectoryInfo exeDirectory = new DirectoryInfo(Service_Misc.GetExecutionPath());
            DirectoryInfo metadataDirectory = new DirectoryInfo(Path.Combine(exeDirectory.FullName, "Metadata"));


            int indexItems = -1;
            int indexLocation = -1;
            int indexName = -1;

            List<HtmlNode> listthNodes = iNode.Descendants("th").ToList();
            for (int i = 0; i < listthNodes.Count; i++)
            {
                HtmlNode thNode = listthNodes[i];
                if (null == thNode) continue;

                string thText = thNode.InnerText.Trim().ToLower();
                if (thText == "item" || thText == "items" || thText == "fish name" || thText == "fish")
                {
                    indexItems = i;
                }
                else if (thText == "coordinate" || thText == "coordinates")
                {
                    indexLocation = i;
                }
                else if (thText == "location" || thText == "zone")
                {
                    indexName = i;
                }
            }

            if (indexItems < 0) return result;
            if (indexName < 0) return result;
            if (indexLocation < 0) return result;

            foreach (HtmlNode trNode in iNode.Descendants("tr"))
            {
                List<HtmlNode> listtdNodes = trNode.Descendants("td").ToList();
                if (listtdNodes.Count <= indexItems) continue;
                if (listtdNodes.Count <= indexLocation) continue;
                if (listtdNodes.Count <= indexName) continue;
                string coordinates = listtdNodes[indexLocation].InnerText.Trim().ToLower().Replace(":", "").Replace("x", "").Replace("y", "");
                string nameZone = listtdNodes[indexName].InnerText.Trim();
                List<string> items = listtdNodes[indexItems].InnerText.Trim().Split(new string[] { Environment.NewLine, ",", "\n" }, StringSplitOptions.None).ToList();

                double x = 0, y = 0;
                if (coordinates == "(shore)")
                {

                }
                else
                {
                    if (!coordinates.Contains("(")) coordinates = "(" + coordinates;
                    if (!coordinates.Contains(")")) coordinates = coordinates + ")";
                    if (!coordinates.Contains(",")) coordinates = coordinates.Replace(" ", ",");

                    string xText = coordinates.Split(new string[] { "(" }, StringSplitOptions.None)[1].Split(',')[0].Trim();
                    string yText = coordinates.Split(new string[] { "," }, StringSplitOptions.None)[1].Split(')')[0].Trim();
                    double.TryParse(xText, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out x);
                    double.TryParse(yText, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out y);
                }

                string nameNode = nameZone.Replace(" ", "_") + "_" + x + "_" + y;

                FFXIVGatheringNode gatheringNode = null;
                foreach (FFXIVGatheringNode node in result)
                {
                    if (node.Name == nameNode)
                    {
                        gatheringNode = node;
                    }
                }
                if (null == gatheringNode)
                {
                    gatheringNode = new FFXIVGatheringNode();
                    gatheringNode.Name = nameNode;

                    //Conversion aetheryte position to map position
                    double offsetX = 0;
                    double offsetY = 0;
                    if (nameZone == "Greytail Falls") nameZone = "Coerthas Western Highlands";
                    if (nameZone == "South Banepool") nameZone = "Coerthas Western Highlands";
                    if (nameZone == "West Banepool") nameZone = "Coerthas Western Highlands";
                    if (nameZone == "Unfrozen Pond") nameZone = "Coerthas Western Highlands";
                    if (nameZone == "Ashpool") nameZone = "Coerthas Western Highlands";
                    if (nameZone == "Riversmeet") nameZone = "Coerthas Western Highlands";
                    if (nameZone == "Dragonspit") nameZone = "Coerthas Western Highlands";
                    if (nameZone == "Clearpool") nameZone = "Coerthas Western Highlands";

                    if (nameZone == "Voor Sian Siran") nameZone = "The Sea of Clouds";
                    if (nameZone == "Mok Oogl Island") nameZone = "The Sea of Clouds";
                    if (nameZone == "Cloudtop") nameZone = "The Sea of Clouds";
                    if (nameZone == "The Blue Window") nameZone = "The Sea of Clouds";
                    if (nameZone == "The Eddies") nameZone = "The Sea of Clouds";

                    if (nameZone == "Riversmeet") nameZone = "Coerthas Western Highlands";
                    if (nameZone == "Riversmeet") nameZone = "Coerthas Western Highlands";
                    if (nameZone == "Riversmeet") nameZone = "Coerthas Western Highlands";
                    FileInfo metadataFile = new FileInfo(Path.Combine(metadataDirectory.FullName, nameZone + ".txt"));
                    if (!metadataFile.Exists)
                    {
                        metadataFile = new FileInfo(Path.Combine(metadataDirectory.FullName, "The " + nameZone + ".txt"));
                        if (metadataFile.Exists) nameZone = "The " + nameZone;
                    }
                    if (!metadataFile.Exists)
                    {
                        offsetX = 0;
                        offsetY = 0;
                        //File.WriteAllText(metadataFile.FullName, "21.4;21.4");
                    }
                    else
                    {
                        string metadata = File.ReadAllText(metadataFile.FullName);
                        List<string> coordinatesMetadata = metadata.Replace(",", ".").Split(';').ToList();
                        double.TryParse(coordinatesMetadata[0], System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out offsetX);
                        double.TryParse(coordinatesMetadata[1], System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out offsetY);
                    }
                    gatheringNode.Zone = nameZone;
                    gatheringNode.Position = new FFXIVPosition((x - offsetX) * 50, (y - offsetY) * 50);
                    gatheringNode.JobTrigram = iJobName;
                    gatheringNode.Type = iType;
                    result.Add(gatheringNode);
                }
                gatheringNode.NodeItems.AddRange(items);
            }


            return result;
        }

        /// <summary>
        /// Retrieves all aetheryte names and positions from https://ffxiv.consolegameswiki.com/wiki/Zone
        /// </summary>
        /// <returns></returns>
        public static List<FFXIVAetheryte> GetAetherytes()
        {
            List<FFXIVAetheryte> result = new List<FFXIVAetheryte>();

            //Main zone page
            CookieCollection oCookies = new CookieCollection();
            HttpStatusCode oCode = HttpStatusCode.NotFound;
            HtmlDocument answer = Service_Misc.GetWebPageFromRequest("GET https://ffxiv.consolegameswiki.com/wiki/Zone HTTP/1.1|Host: ffxiv.consolegameswiki.com|Connection: keep-alive|Pragma: no-cache|Cache-Control: no-cache|Upgrade-Insecure-Requests: 1|User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.88 Safari/537.36|Sec-Fetch-User: ?1|Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9|Sec-Fetch-Site: same-origin|Sec-Fetch-Mode: navigate|Referer: https://ffxiv.consolegameswiki.com/wiki/FF14_Wiki|Accept-Encoding: gzip, deflate|Accept-Language: fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7|Cookie: __cfduid=d64998d831d683525635413f445baf9671577745615; _ga=GA1.2.147161069.1577745617; _gid=GA1.2.132489683.1577745617; trc_cookie_storage=taboola%2520global%253Auser-id%3D8ee6f8c7-39f8-4db7-b620-5868e971d900-tuct4ec3105||",
                oCookies, ref oCookies, ref oCode);
            if (null == answer) return result;

            HtmlNode contentNode = answer.GetElementbyId("mw-content-text");
            if (null == contentNode) return result;

            string currentRegion = "Unknown";
            foreach (HtmlNode childNode in contentNode.ChildNodes)
            {
                if (childNode.Name == "h3")
                {
                    currentRegion = childNode.InnerText.Trim();
                }
                else if (childNode.Name == "ul")
                {
                    foreach (HtmlNode liNode in childNode.ChildNodes)
                    {
                        foreach (HtmlNode aNode in liNode.ChildNodes)
                        {
                            if (aNode.Name != "a") continue;

                            string link = Service_Misc.ExtractLink(aNode);
                            result.AddRange(GetAetherytesFromRegion(currentRegion, aNode.InnerText.Trim(), link));
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Retrieves all aetheryte names and positions from https://ffxiv.consolegameswiki.com/wiki/Zone
        /// </summary>
        /// <returns></returns>
        public static List<FFXIVAetheryte> GetAetherytesFromRegion(string iRegion, string iZoneName, string iPageLink)
        {
            List<FFXIVAetheryte> result = new List<FFXIVAetheryte>();

            DirectoryInfo exeDirectory = new DirectoryInfo(Service_Misc.GetExecutionPath());
            DirectoryInfo metadataDirectory = new DirectoryInfo(Path.Combine(exeDirectory.FullName, "Metadata"));

            //Main zone page
            CookieCollection oCookies = new CookieCollection();
            HttpStatusCode oCode = HttpStatusCode.NotFound;
            HtmlDocument answer = Service_Misc.GetWebPageFromRequest("GET https://ffxiv.consolegameswiki.com" + iPageLink + " HTTP/1.1|Host: ffxiv.consolegameswiki.com|Connection: keep-alive|Pragma: no-cache|Cache-Control: no-cache|Upgrade-Insecure-Requests: 1|User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.88 Safari/537.36|Sec-Fetch-User: ?1|Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9|Sec-Fetch-Site: same-origin|Sec-Fetch-Mode: navigate|Referer: https://ffxiv.consolegameswiki.com/wiki/FF14_Wiki|Accept-Encoding: gzip, deflate|Accept-Language: fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7|Cookie: __cfduid=d64998d831d683525635413f445baf9671577745615; _ga=GA1.2.147161069.1577745617; _gid=GA1.2.132489683.1577745617; trc_cookie_storage=taboola%2520global%253Auser-id%3D8ee6f8c7-39f8-4db7-b620-5868e971d900-tuct4ec3105||",
                oCookies, ref oCookies, ref oCode);
            if (null == answer) return result;

            HtmlNode nodeInfobox = Service_Misc.GetFirstChildNode(answer.DocumentNode, "div", "infobox-n area");
            if (null == nodeInfobox) return result;

            List<HtmlNode> dlNodes = nodeInfobox.Descendants("dl").ToList();
            if (dlNodes.Count != 1) return result;

            foreach (HtmlNode aNode in dlNodes[0].ChildNodes)
            {
                if (aNode.Name != "dd") continue;
                string text = aNode.InnerText;
                List<string> listAetherytes = text.Split(')').ToList();
                foreach (string aetheryteText in listAetherytes)
                {
                    if (aetheryteText.Contains("(X:") && aetheryteText.Contains("Y:"))
                    {
                        string xText = aetheryteText.Split(new string[] { "(X:" }, StringSplitOptions.None)[1].Split(',')[0].Trim();
                        string yText = aetheryteText.Split(new string[] { "Y:" }, StringSplitOptions.None)[1].Split(')')[0].Split(',')[0].Trim();
                        string nameText = aetheryteText.Split(new string[] { "(X:" }, StringSplitOptions.None)[0];
                        double x = 0, y = 0;
                        double.TryParse(xText, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out x);
                        double.TryParse(yText, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out y);

                        FFXIVAetheryte aetheryte = new FFXIVAetheryte();
                        aetheryte.Region = iRegion;
                        aetheryte.Zone = iZoneName;
                        aetheryte.Name = nameText;

                        //Conversion aetheryte position to map position
                        double offsetX = 0;
                        double offsetY = 0;
                        FileInfo metadataFile = new FileInfo(Path.Combine(metadataDirectory.FullName, aetheryte.Zone + ".txt"));
                        if (!metadataFile.Exists)
                        {
                            offsetX = 0;
                            offsetY = 0;
                            File.WriteAllText(metadataFile.FullName, "21.4;21.4");
                        }
                        else
                        {
                            string metadata = File.ReadAllText(metadataFile.FullName);
                            List<string> coordinatesMetadata = metadata.Replace(",", ".").Split(';').ToList();
                            double.TryParse(coordinatesMetadata[0], System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out offsetX);
                            double.TryParse(coordinatesMetadata[1], System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out offsetY);
                        }

                        aetheryte.Position = new FFXIVPosition((x - offsetX) * 50, (y - offsetY) * 50);
                        result.Add(aetheryte);
                    }
                }
            }

            return result;
        }
    }
}
