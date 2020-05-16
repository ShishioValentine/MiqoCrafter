using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;

namespace MiqoCraftCore
{
    public static class GarlandTool
    {

        public static List<FFXIVSearchItem> Search(string iElemToSearch, System.Windows.Forms.TextBox iLogBox = null, FFXIVItem.TypeItem iType = FFXIVItem.TypeItem.Crafted, List<string> iJobs = null, int iMinLevel = 0, int iMaxLevel = 0)
        {
            Service_Misc.LogText(iLogBox, "Let me search this item for you : " + iElemToSearch);
            CookieCollection iCookies = new CookieCollection();
            CookieCollection oCookies = new CookieCollection();
            HttpStatusCode oCode = HttpStatusCode.NotFound;

            bool itemAdded = true;
            int pageNb = 0;
            List<FFXIVSearchItem> listItems = new List<FFXIVSearchItem>();
            while (itemAdded)
            {
                itemAdded = false;

                string searchString = iElemToSearch.ToLower().Replace(" ", "+");
                //http://garlandtools.org/api/search.php?lang=en&ilvlMax=5&craftable=1 
                string lvlMinString = "";
                string lvlMaxString = "";
                string category = "";
                //if (iMinLevel > 0) lvlMinString = "&ilvlMin=" + iMinLevel;
                //if (iMaxLevel > 0) lvlMaxString = "&ilvlMax=" + iMaxLevel;
                if (iType == FFXIVItem.TypeItem.Crafted) category = "&craftable=1";
                string searchResultContent = Service_Misc.GetContentFromRequest("GET http://garlandtools.org/api/search.php?text=" + searchString + "&lang=en&page=" + pageNb + lvlMinString + lvlMaxString + category + " HTTP/1.1|Host: garlandtools.org|Connection: keep-alive|Accept: application/json, text/javascript, */*; q=0.01|X-Requested-With: XMLHttpRequest|User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36|Referer: http://garlandtools.org/db/|Accept-Encoding: gzip, deflate|Accept-Language: fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7|",
                    iCookies, ref oCookies, ref oCode);
                pageNb++;

                string innerHTML = "{\"Result\":" + searchResultContent + "}";

                JToken mainToken = null;
                try
                {
                    mainToken = JObject.Parse(innerHTML);
                    if (null == mainToken)
                    {
                        throw new Exception("Impossible de récupérer l'identifiant de connexion à partir du site");
                    }
                }
                catch (Exception)
                {
                    continue;
                }

                JToken listToken = mainToken["Result"];
                if (listToken.Count() > 0)
                {
                    Service_Misc.LogText(iLogBox, "Hey I found some items !");
                    itemAdded = true;
                }
                else
                {
                    Service_Misc.LogText(iLogBox, "Looks like there is no more items to find... (Page " + pageNb + ")");
                    return listItems;
                }
                foreach (JToken childToken in listToken.Children())
                {
                    if (null == childToken) continue;

                    string itemName = "Unknown Item";
                    JToken dataToken = childToken["obj"];
                    if (null != dataToken && null != dataToken["n"])
                    {
                        itemName = dataToken["n"].Value<string>();
                    }
                    string type = childToken["type"].Value<string>();
                    if (type != "item")
                    {
                        Service_Misc.LogText(iLogBox, "Found something but it ain't an item : " + itemName + " - " + type);
                        continue;
                    }

                    try
                    {
                        FFXIVSearchItem item = new FFXIVSearchItem();

                        item.Name = itemName;
                        item.UrlImage = "http://garlandtools.org/files/icons/item/" + dataToken["c"].Value<string>() + ".png";
                        item.UrlGarland = "http://garlandtools.org/db/#item/" + dataToken["i"].Value<string>();

                        if (dataToken["f"] != null && iType == FFXIVItem.TypeItem.Crafted)
                        {
                            string jobID = dataToken["f"][0]["job"].Value<string>();
                            string itemClass = GetJobName(jobID);
                            string itemLevel = dataToken["f"][0]["lvl"].Value<string>();
                            int level = 0;
                            int.TryParse(itemLevel, out level);
                            if (iMinLevel > 0 && level < iMinLevel)
                            {
                                Service_Misc.LogText(iLogBox, "Found something but its level is under the requested min level : " + itemName + " - Level : " + itemLevel);
                                continue;
                            }
                            if (iMaxLevel > 0 && level > iMaxLevel)
                            {
                                Service_Misc.LogText(iLogBox, "Found something but its level is over the requested max level : " + itemName + " - Level : " + itemLevel);
                                continue;
                            }

                            if (null != iJobs && iJobs.Count > 0 && !iJobs.Contains(itemClass.ToUpper()) && !iJobs.Contains(itemClass.ToLower()))
                            {
                                Service_Misc.LogText(iLogBox, "Found something but its job is not in the requested list : " + itemName + " - Job : " + itemClass);
                                continue;
                            }
                            item.Class = itemClass;
                            item.Level = itemLevel;
                        }
                        else if (iType == FFXIVItem.TypeItem.Crafted)
                        {
                            Service_Misc.LogText(iLogBox, "Found something but it ain't for crafting : " + itemName);
                            continue;
                        }

                        item.ID = childToken["id"].Value<string>();

                        Service_Misc.LogText(iLogBox, "Found this item : " + item.ToString());

                        listItems.Add(item);
                        itemAdded = true;
                    }
                    catch (Exception exc)
                    {
                        Service_Misc.LogText(iLogBox, "Oh man, I failed to retrieve an item from the list : ");
                        Service_Misc.LogText(iLogBox, itemName);
                        Service_Misc.LogText(iLogBox, exc.Message);
                    }
                }
            }

            listItems.Sort();
            return listItems;
        }

        /// <summary>
        /// Retrieve a job name from its ID
        /// </summary>
        /// <param name="iId"></param>
        /// <returns></returns>
        public static string GetJobName(string iId)
        {
            if (iId == "0") return "ADV";
            if (iId == "1") return "";
            if (iId == "2") return "";
            if (iId == "3") return "";
            if (iId == "4") return "";
            if (iId == "5") return "";
            if (iId == "6") return "";
            if (iId == "7") return "";
            if (iId == "8") return "CRP";
            if (iId == "9") return "BSM";
            if (iId == "10") return "ARM";
            if (iId == "11") return "GSM";
            if (iId == "12") return "LTW";
            if (iId == "13") return "WVR";
            if (iId == "14") return "ALC";
            if (iId == "15") return "CUL";
            if (iId == "16") return "";
            if (iId == "17") return "";
            if (iId == "18") return "";

            return "";
        }

        /// <summary>
        /// Retrieve a job name from its ID
        /// </summary>
        /// <param name="iId"></param>
        /// <returns></returns>
        public static string GetGatheringJobName(string iGatherType, FFXIVItem iItem)
        {
            if (null != iItem && iItem.Name == "Water Crystal") return "BTN";
            if (iGatherType == "0") return "MIN";
            if (iGatherType == "1") return "MIN";
            if (iGatherType == "2") return "BTN";
            if (iGatherType == "3") return "BTN";

            return "";
        }

        /// <summary>
        /// Retrieve all gathering nodes from an item
        /// </summary>
        /// <param name="iItemID"></param>
        /// <returns></returns>
        public static List<FFXIVGatheringNode> GetGatheringNodesFromItem(string iItemID)
        {
            List<FFXIVGatheringNode> result = new List<FFXIVGatheringNode>();
            if (null == iItemID) return result;

            //https://www.garlandtools.org/db/doc/item/en/3/26498.json

            CookieCollection iCookies = new CookieCollection();
            CookieCollection oCookies = new CookieCollection();
            HttpStatusCode oCode = HttpStatusCode.NotFound;

            string searchResultContent = Service_Misc.GetContentFromRequest("GET https://www.garlandtools.org/db/doc/item/en/3/" + iItemID + ".json HTTP/1.1|Host: www.garlandtools.org|Connection: keep-alive|Pragma: no-cache|Cache-Control: no-cache|Upgrade-Insecure-Requests: 1|User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36|Sec-Fetch-Mode: navigate|Sec-Fetch-User: ?1|Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3|Sec-Fetch-Site: cross-site|Accept-Encoding: gzip, deflate|Accept-Language: fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7|",
                iCookies, ref oCookies, ref oCode);

            try
            {
                string logName2 = Path.Combine(Service_Misc.GetExecutionPath(), "Searchlog.log");
                File.WriteAllText(logName2, searchResultContent);
            }
            catch
            {

            }

            string logName = Path.Combine(Service_Misc.GetExecutionPath(), "GeneralDataBase.log");
            string dataResultContent = "";
            if (File.Exists(logName))
            {
                dataResultContent = File.ReadAllText(logName);
            }

            if (dataResultContent == "")
            {
                dataResultContent = Service_Misc.RemoveIllegalCharacters(Service_Misc.GetContentFromRequest("GET http://garlandtools.org/db/doc/core/en/3/data.json HTTP/1.1|Host: garlandtools.org|Connection: keep-alive|Pragma: no-cache|Cache-Control: no-cache|Accept: application/json, text/javascript, */*; q=0.01|X-Requested-With: XMLHttpRequest|User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36|Referer: http://garlandtools.org/db/|Accept-Encoding: gzip, deflate|Accept-Language: fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7|",
                iCookies, ref oCookies, ref oCode));

                try
                {
                    File.WriteAllText(logName, dataResultContent);
                }
                catch
                {

                }
            }
            try
            {
                JToken mainToken = JObject.Parse(searchResultContent);
                if (null == mainToken) return result;

                JToken dataToken = JObject.Parse(dataResultContent);
                if (null == dataToken) return result;

                JToken itemToken = mainToken["item"];
                if (null == itemToken) return result;

                JToken nodeToken = itemToken["nodes"];
                if (null != nodeToken)
                {
                    return ReadGatheredNodes(iCookies, ref oCookies, ref oCode, dataToken, itemToken, nodeToken);
                }
            }
            catch
            {

            }

            return result;
        }

        /// <summary>
        /// Retrieve an item name from its ID
        /// </summary>
        /// <param name="iItemID"></param>
        /// <returns></returns>
        public static string GetItemName(string iItemID)
        {
            if (null == iItemID) return "";

            //https://www.garlandtools.org/db/doc/item/en/3/26498.json

            CookieCollection iCookies = new CookieCollection();
            CookieCollection oCookies = new CookieCollection();
            HttpStatusCode oCode = HttpStatusCode.NotFound;

            string searchResultContent = Service_Misc.GetContentFromRequest("GET https://www.garlandtools.org/db/doc/item/en/3/" + iItemID + ".json HTTP/1.1|Host: www.garlandtools.org|Connection: keep-alive|Pragma: no-cache|Cache-Control: no-cache|Upgrade-Insecure-Requests: 1|User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36|Sec-Fetch-Mode: navigate|Sec-Fetch-User: ?1|Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3|Sec-Fetch-Site: cross-site|Accept-Encoding: gzip, deflate|Accept-Language: fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7|",
                iCookies, ref oCookies, ref oCode);

            try
            {
                JToken mainToken = JObject.Parse(searchResultContent);
                if (null == mainToken) return "";

                JToken itemToken = mainToken["item"];
                if (null == itemToken) return "";

                return itemToken["name"].Value<string>();
            }
            catch
            {
            }
            return "";
        }

        /// <summary>
        /// Recursively build an item craft tree
        /// </summary>
        /// <param name="iItem"></param>
        public static FFXIVItem RecBuildCraftingTree(System.Windows.Forms.TextBox iLogBox, string iItemID, int quantity = 1)
        {
            if (null == iItemID) return null;

            //https://www.garlandtools.org/db/doc/item/en/3/26498.json

            FFXIVItem item = null;

            CookieCollection iCookies = new CookieCollection();
            CookieCollection oCookies = new CookieCollection();
            HttpStatusCode oCode = HttpStatusCode.NotFound;

            string searchResultContent = Service_Misc.GetContentFromRequest("GET https://www.garlandtools.org/db/doc/item/en/3/" + iItemID + ".json HTTP/1.1|Host: www.garlandtools.org|Connection: keep-alive|Pragma: no-cache|Cache-Control: no-cache|Upgrade-Insecure-Requests: 1|User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36|Sec-Fetch-Mode: navigate|Sec-Fetch-User: ?1|Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3|Sec-Fetch-Site: cross-site|Accept-Encoding: gzip, deflate|Accept-Language: fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7|",
                iCookies, ref oCookies, ref oCode);

            try
            {
                string logName2 = Path.Combine(Service_Misc.GetExecutionPath(), "Searchlog.log");
                File.WriteAllText(logName2, searchResultContent);
            }
            catch
            {

            }

            string logName = Path.Combine(Service_Misc.GetExecutionPath(), "GeneralDataBase.log");
            string dataResultContent = "";
            if (File.Exists(logName))
            {
                dataResultContent = File.ReadAllText(logName);
            }

            if (dataResultContent == "")
            {
                dataResultContent = Service_Misc.RemoveIllegalCharacters(Service_Misc.GetContentFromRequest("GET http://garlandtools.org/db/doc/core/en/3/data.json HTTP/1.1|Host: garlandtools.org|Connection: keep-alive|Pragma: no-cache|Cache-Control: no-cache|Accept: application/json, text/javascript, */*; q=0.01|X-Requested-With: XMLHttpRequest|User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36|Referer: http://garlandtools.org/db/|Accept-Encoding: gzip, deflate|Accept-Language: fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7|",
                iCookies, ref oCookies, ref oCode));

                try
                {
                    File.WriteAllText(logName, dataResultContent);
                }
                catch
                {

                }
            }

            try
            {
                JToken mainToken = JObject.Parse(searchResultContent);
                if (null == mainToken) return item;

                JToken dataToken = JObject.Parse(dataResultContent);
                if (null == dataToken) return item;

                JToken itemToken = mainToken["item"];
                if (null == itemToken) return item;

                JToken mainIngredientToken = mainToken["ingredients"];
                List<JToken> allIngredientsToken = new List<JToken>();
                List<string> allIngredientsIDs = new List<string>();
                if (null != mainIngredientToken)
                {
                    //Listing ingredients nodes and ID

                    foreach (JToken ingredientToken in mainIngredientToken.Children())
                    {
                        if (null == ingredientToken) continue;

                        string ID = ingredientToken["id"].Value<string>();
                        allIngredientsIDs.Add(ID);
                        allIngredientsToken.Add(ingredientToken);
                    }
                }

                item = CreateItemFromNode(iLogBox, iCookies, ref oCookies, ref oCode, itemToken, dataToken, allIngredientsToken, allIngredientsIDs);
                if (null != item) item.Quantity = quantity;
            }
            catch (Exception exc)
            {
                Service_Misc.LogText(iLogBox, "Ouch I failed !");
                Service_Misc.LogText(iLogBox, exc.Message);
            }

            if (null != item) Service_Misc.LogText(iLogBox, "Got this item ! " + item);
            return item;
        }

        private static List<FFXIVGatheringNode> ReadGatheredNodes(CookieCollection iCookies, ref CookieCollection oCookies, ref HttpStatusCode oCode, JToken dataToken, JToken itemToken, JToken nodeToken)
        {
            List<FFXIVGatheringNode> result = new List<FFXIVGatheringNode>();

            //Adding nodes info
            foreach (JToken gatherNodeToken in nodeToken.Children())
            {
                try
                {
                    string nodeSearchContent = Service_Misc.GetContentFromRequest("GET http://garlandtools.org/db/doc/node/en/2/" + gatherNodeToken.Value<string>() + ".json HTTP/1.1|Host: garlandtools.org|Connection: keep-alive|Accept: application/json, text/javascript, */*; q=0.01|X-Requested-With: XMLHttpRequest|User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36|Referer: http://garlandtools.org/db/|Accept-Encoding: gzip, deflate|Accept-Language: fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7|",
                        iCookies, ref oCookies, ref oCode);

                    try
                    {
                        string logName2 = Path.Combine(Service_Misc.GetExecutionPath(), "Node.log");
                        File.WriteAllText(logName2, nodeSearchContent);
                    }
                    catch
                    {

                    }

                    JToken gatherResultToken = JObject.Parse(nodeSearchContent);
                    if (null == gatherResultToken) continue;

                    JToken gatherResultNodeToken = gatherResultToken.Children().ToList()[0].Children().ToList()[0];
                    if (null == gatherResultNodeToken) continue;

                    string zoneID = gatherResultNodeToken["zoneid"].Value<string>();

                    JToken locationToken = dataToken["locationIndex"];
                    if (null == locationToken) continue;

                    JToken zoneIDToken = locationToken[zoneID];
                    if (null == zoneIDToken) continue;

                    JToken zoneNameToken = zoneIDToken["name"];
                    if (null == zoneNameToken) continue;

                    string zone = zoneNameToken.Value<string>();
                    string gatheringType = gatherResultNodeToken["type"].Value<string>();
                    string type = "";
                    if (null != gatherResultNodeToken["limitType"]) type = gatherResultNodeToken["limitType"].Value<string>();
                    string name = gatherResultNodeToken["name"].Value<string>();

                    FFXIVGatheringNode gatheringNode = new FFXIVGatheringNode();
                    gatheringNode.JobTrigram = GetGatheringJobName(gatheringType, null);
                    gatheringNode.Name = name;
                    //gatheringNode.Type = type;
                    gatheringNode.Zone = zone;

                    if(null != gatherResultNodeToken["items"])
                    {
                        foreach (JToken nodeItemToken in gatherResultNodeToken["items"].Children())
                        {
                            if (null == nodeItemToken) continue;
                            gatheringNode.NodeItems.Add(GetItemName(nodeItemToken["id"].Value<string>()));
                        }
                    }
                    result.Add(gatheringNode);
                }
                catch (Exception exc)
                {
                    string msg = exc.Message;
                }
            }

            return result;
        }

        private static FFXIVGatheredItem ReadGatheredItem(CookieCollection iCookies, ref CookieCollection oCookies, ref HttpStatusCode oCode, JToken dataToken, JToken itemToken, JToken nodeToken)
        {
            FFXIVGatheredItem gatheredItem = new FFXIVGatheredItem();

            //Adding nodes info
            foreach (JToken gatherNodeToken in nodeToken.Children())
            {
                try
                {
                    string nodeSearchContent = Service_Misc.GetContentFromRequest("GET http://garlandtools.org/db/doc/node/en/2/" + gatherNodeToken.Value<string>() + ".json HTTP/1.1|Host: garlandtools.org|Connection: keep-alive|Accept: application/json, text/javascript, */*; q=0.01|X-Requested-With: XMLHttpRequest|User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36|Referer: http://garlandtools.org/db/|Accept-Encoding: gzip, deflate|Accept-Language: fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7|",
                        iCookies, ref oCookies, ref oCode);

                    try
                    {
                        string logName2 = Path.Combine(Service_Misc.GetExecutionPath(), "Node.log");
                        File.WriteAllText(logName2, nodeSearchContent);
                    }
                    catch
                    {

                    }

                    JToken gatherResultToken = JObject.Parse(nodeSearchContent);
                    if (null == gatherResultToken) continue;

                    JToken gatherResultNodeToken = gatherResultToken.Children().ToList()[0].Children().ToList()[0];
                    if (null == gatherResultNodeToken) continue;

                    string zoneID = gatherResultNodeToken["zoneid"].Value<string>();

                    JToken locationToken = dataToken["locationIndex"];
                    if (null == locationToken) continue;

                    JToken zoneIDToken = locationToken[zoneID];
                    if (null == zoneIDToken) continue;

                    JToken zoneNameToken = zoneIDToken["name"];
                    if (null == zoneNameToken) continue;

                    string zone = zoneNameToken.Value<string>();
                    string gatheringType = gatherResultNodeToken["type"].Value<string>();
                    string type = "";
                    if (null != gatherResultNodeToken["limitType"]) type = gatherResultNodeToken["limitType"].Value<string>();
                    string slot = "";
                    foreach (JToken slotToken in gatherResultNodeToken["items"])
                    {
                        if (null == slotToken) continue;
                        if (null == slotToken["slot"]) continue;
                        if (null == slotToken["id"]) continue;
                        string id = slotToken["id"].Value<string>();
                        string slotCandidate = slotToken["slot"].Value<string>();
                        if (id == itemToken["id"].Value<string>())
                        {
                            slot = slotCandidate;
                            break;
                        }
                    }
                    string time = "";
                    if (null != gatherResultNodeToken["time"])
                    {
                        foreach (JToken timeToken in gatherResultNodeToken["time"])
                        {
                            if (null == timeToken) continue;
                            if (time != "") time += ",";
                            time += timeToken.Value<string>();
                        }
                    }

                    gatheredItem.Zones.Add(zone);
                    gatheredItem.NodeType.Add("");
                    gatheredItem.Slot.Add(slot);
                    gatheredItem.Times.Add(time);
                    gatheredItem.Types.Add(type);
                    gatheredItem.GatheringTypes.Add(gatheringType);
                }
                catch (Exception exc)
                {
                    string msg = exc.Message;
                }
            }

            return gatheredItem;
        }

        private static FFXIVItem CreateItemFromNode(System.Windows.Forms.TextBox iLogBox, CookieCollection iCookies, ref CookieCollection oCookies, ref HttpStatusCode oCode, JToken iItemNode, JToken iDataNode, List<JToken> iIngredientsToken, List<string> iIngredienstIDs)
        {
            FFXIVItem item = null;
            if (null == iItemNode) return null;

            JToken itemToken = iItemNode;
            if (null == itemToken) return item;

            JToken craftToken = itemToken["craft"];
            JToken nodeToken = itemToken["nodes"];
            JToken reducedToken = itemToken["reducedFrom"];
            JToken tradeShop = itemToken["tradeShops"];
            JToken vendor = itemToken["vendors"];

            if (null != craftToken)
            {
                FFXIVCraftedItem craftedItem = new FFXIVCraftedItem();
                craftedItem.Type = FFXIVItem.TypeItem.Crafted;

                string jobID = craftToken[0]["job"].Value<string>();
                craftedItem.Class = GetJobName(jobID);
                craftedItem.UrlClass = "http://garlandtools.org/files/icons/job/" + craftedItem.Class + ".png";
                craftedItem.Level = craftToken[0]["lvl"].Value<string>();

                int yieldQuantity = 1;
                JToken yieldToken = craftToken[0]["yield"];
                if (null != yieldToken)
                {
                    yieldQuantity = yieldToken.Value<int>();
                }
                craftedItem.RecipeQuantity = yieldQuantity;

                //Building craft requirement list
                foreach (JToken ingredientToken in craftToken[0]["ingredients"].Children())
                {
                    if (null == ingredientToken) continue;

                    string ingredientID = ingredientToken["id"].Value<string>();

                    //Finding right ingredient token from input list, with all informations
                    JToken fullIngredientNode = ingredientToken;
                    int indexInList = iIngredienstIDs.IndexOf(ingredientID);
                    FFXIVItem ingredient = null;
                    if (indexInList >= 0 && indexInList < iIngredientsToken.Count)
                    {
                        fullIngredientNode = iIngredientsToken[indexInList];
                        ingredient = CreateItemFromNode(iLogBox, iCookies, ref oCookies, ref oCode, fullIngredientNode, iDataNode, iIngredientsToken, iIngredienstIDs);
                    }
                    else
                    {
                        ingredient = RecBuildCraftingTree(iLogBox, ingredientID, 1);
                    }

                    //Adding Item
                    if (null != ingredient)
                    {
                        ingredient.Quantity = ingredientToken["amount"].Value<int>();
                        craftedItem.ListNeededItems.Add(ingredient);
                    }
                    else
                    {
                        Service_Misc.LogText(iLogBox, "Oups... failed to retrieve one ingredient : ID=" + ingredientToken["id"].Value<string>());
                    }
                }

                item = craftedItem;
            }
            else if (null != nodeToken)
            {
                FFXIVGatheredItem gatheredItem = ReadGatheredItem(iCookies, ref oCookies, ref oCode, iDataNode, itemToken, nodeToken);
                item = gatheredItem;
                item.Type = FFXIVItem.TypeItem.Gathered;
            }
            else if (null != reducedToken)
            {
                item = ReadReducedItem(iLogBox, reducedToken);
            }
            else if (null != tradeShop || null != vendor)
            {
                item = new FFXIVItem();
                item.Type = FFXIVItem.TypeItem.NPC;
            }
            else
            {
                item = new FFXIVItem();
                item.Type = FFXIVItem.TypeItem.Unkwown;
                Service_Misc.LogText(iLogBox, "Oups... one ingredient is unknown, don't know how to get this one.");
            }

            if (null != item)
            {
                try
                {
                    item.Name = itemToken["name"].Value<string>();
                    item.UrlImage = "http://garlandtools.org/files/icons/item/" + itemToken["icon"].Value<string>() + ".png";
                    item.UrlGarland = "http://garlandtools.org/db/#item/" + itemToken["id"].Value<string>();
                    item.ID = itemToken["id"].Value<string>();
                }
                catch (Exception)
                {
                    Service_Misc.LogText(iLogBox, "Oups... failed to retrieve one item : Token=" + Environment.NewLine + iItemNode.ToString());
                }
            }

            return item;
        }

        private static FFXIVItem ReadReducedItem(System.Windows.Forms.TextBox iLogBox, JToken reducedToken)
        {
            FFXIVItem item;
            FFXIVReducedItem reducedItem = new FFXIVReducedItem();
            reducedItem.Type = FFXIVItem.TypeItem.Reduced;

            string idReducedFrom = reducedToken[0].Value<string>();
            FFXIVItem ingredient = RecBuildCraftingTree(iLogBox, idReducedFrom, 5);
            if (null != ingredient)
            {
                reducedItem.ReducedFrom = ingredient;

                if (ingredient is FFXIVGatheredItem)
                {
                    FFXIVGatheredItem gatheredItem = ingredient as FFXIVGatheredItem;
                    if (null != gatheredItem)
                    {
                        gatheredItem.AsCollectable = true;
                    }
                }
            }
            else
            {
                Service_Misc.LogText(iLogBox, "Oups... failed to retrieve one reduced reference : ID=" + idReducedFrom);
            }

            item = reducedItem;
            return item;
        }

        public static string GetTeleportName(string iZone, string iGrid, FFXIVGatheredItem iItem)
        {
            string toSearchInGrid = iZone + " @";
            if (iGrid.Contains(toSearchInGrid))
            {
                return iGrid.Split(new string[] { toSearchInGrid }, StringSplitOptions.None)[1].Split(']')[0];
            }
            toSearchInGrid = iZone + "@";
            if (iGrid.Contains(toSearchInGrid))
            {
                return iGrid.Split(new string[] { toSearchInGrid }, StringSplitOptions.None)[1].Split(']')[0];
            }
            toSearchInGrid = iZone + Environment.NewLine + " @";
            if (iGrid.Contains(toSearchInGrid))
            {
                return iGrid.Split(new string[] { toSearchInGrid }, StringSplitOptions.None)[1].Split(']')[0];
            }
            toSearchInGrid = iZone + Environment.NewLine + "@";
            if (iGrid.Contains(toSearchInGrid))
            {
                return iGrid.Split(new string[] { toSearchInGrid }, StringSplitOptions.None)[1].Split(']')[0];
            }
            toSearchInGrid = iZone + @"\r\n" + "@";
            if (iGrid.Contains(toSearchInGrid))
            {
                return iGrid.Split(new string[] { toSearchInGrid }, StringSplitOptions.None)[1].Split(']')[0];
            }
            toSearchInGrid = iZone + @"\r\n" + " @";
            if (iGrid.Contains(toSearchInGrid))
            {
                return iGrid.Split(new string[] { toSearchInGrid }, StringSplitOptions.None)[1].Split(']')[0];
            }
            if (iZone == "Kholusia") return "Stilltide";
            if (iZone == "Central Thanalan") return "Black Brush Station";
            if (iZone == "Southern Thanalan") return "Forgotten Springs";
            if (iZone == "The Sea of Clouds") return "Camp Cloudtop";
            if (iZone == "The Azim Steppe") return "Reunion";
            if (iZone == "The Peaks") return "Ala Ghiri";
            if (iZone == "Lakeland") return "Fort Jobb";
            if (iZone == "Amh Araeng") return "Twine";
            if (iZone == "North Shroud") return "Fallgourd Float";
            if (iZone == "Lower La Noscea") return "Moraby Drydocks";
            if (iZone == "The Churning Mists") return "Zenith";
            if (iZone == "Eastern Thanalan") return "Camp Drybone";
            if (iZone == "Central Shroud") return "Bentbranch Meadows";
            if (iZone == "East Shroud") return "The Hawthorne Hut";
            if (iZone == "The Dravanian Hinterlands") return "Idyllshire";
            if (iZone == "The Fringes") return "The Peering Stones";
            if (iZone == "Yanxia") return "Namai";
            if (iZone == "The Rak'tika Greatwood") return "Fanow";

            if (iZone == "New Gridania") return "New Gridania";
            if (iZone == "Central Shroud") return "Bentbranch Meadows";
            if (iZone == "East Shroud") return "The Hawthorne Hut";
            if (iZone == "South Shroud") return "Quarrymill";
            if (iZone == "South Shroud") return "Camp Tranquil";
            if (iZone == "North Shroud") return "Fallgourd Float";
            if (iZone == "Ul'dah - Steps of Nald") return "Ul'dah - Steps of Nald";
            if (iZone == "Western Thanalan") return "Horizon";
            if (iZone == "Central Thanalan") return "Black Brush Station";
            if (iZone == "Eastern Thanalan") return "Camp Drybone";
            if (iZone == "Southern Thanalan") return "Little Ala Mhigo";
            if (iZone == "Southern Thanalan") return "Forgotten Springs";
            if (iZone == "Northern Thanalan") return "Camp Bluefog";
            if (iZone == "Northern Thanalan") return "Ceruleum Processing Plant";
            if (iZone == "The Gold Saucer") return "The Gold Saucer";
            if (iZone == "Foundation") return "Foundation";
            if (iZone == "Coerthas Central Highlands") return "Camp Dragonhead";
            if (iZone == "Coerthas Western") return "Highlands  Falcon's Nest";
            if (iZone == "The Sea of Clouds") return "Camp Cloudtop";
            if (iZone == "The Sea of Clouds") return "Ok' Zundu";
            if (iZone == "Azys Lla") return "Helix";
            if (iZone == "Idyllshire") return "Idyllshire";
            if (iZone == "The Dravanian Forelands") return "Tailfeather";
            if (iZone == "The Dravanian Forelands") return "Anyx Trine";
            if (iZone == "The Churning Mists") return "Moghome";
            if (iZone == "The Churning Mists") return "Zenith";
            if (iZone == "Rhalgr's Reach") return "Rhalgr's Reach";
            if (iZone == "The Fringes") return "Castrum Oriens";
            if (iZone == "The Fringes") return "The Peering Stones";
            if (iZone == "The Peaks") return "Ala Gannha";
            if (iZone == "The Peaks") return "Ala Ghiri";
            if (iZone == "The Lochs") return "Porta Praetoria";
            if (iZone == "The Lochs") return "The Ala Mhigan Quarter";
            if (iZone == "The Ruby Sea") return "Tamamizu";
            if (iZone == "The Ruby Sea") return "Onokoro";
            if (iZone == "Yanxia") return "Namai";
            if (iZone == "Yanxia") return "The House of the Fierce";
            if (iZone == "The Azim Steppe") return "Reunion";
            if (iZone == "The Azim Steppe") return "The Dawn Throne";
            if (iZone == "The Doman Enclave") return "The Doman Enclave";
            if (iZone == "The Crystarium") return "The Crystarium";
            if (iZone == "Lakeland") return "Fort Jobb";
            if (iZone == "Lakeland") return "The Ostall Imperative";
            if (iZone == "Kholusia") return "Stilltide";
            if (iZone == "Kholusia") return "Wright";
            if (iZone == "Amh Araeng") return "Mord Souq";
            if (iZone == "Amh Araeng") return "The Inn at Journey's Head";
            if (iZone == "Amh Araeng") return "Twine";
            if (iZone == "Il Mheg") return "Lydha Lran";
            if (iZone == "Il Mheg") return "Pla Enni";
            if (iZone == "Il Mheg") return "Wolekdorf";
            if (iZone == "The Rak'tika Greatwood") return "Slitherbough";
            if (iZone == "The Rak'tika Greatwood") return "Fanow";
            if (iZone == "Mor Dhona") return "Revenant's Toll";
            if (iZone == "Upper La Noscea") return "Camp Bronze Lake";
            if (iZone == "Middle La Noscea") return "Summerford Farms";
            if (iZone == "Coerthas Western Highlands") return "Falcon's Nest";


            return iZone;
        }
    }
}
