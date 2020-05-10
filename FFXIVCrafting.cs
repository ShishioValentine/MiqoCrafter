using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MiqoCraft
{
    public static class FFXIVCrafting
    {
        public static List<FFXIVCraftingSearchItem> Search(string iElemToSearch, System.Windows.Forms.TextBox iLogBox = null)
        {
            Service_Misc.LogText(iLogBox, "Let me search this item for you : " + iElemToSearch);
            CookieCollection iCookies = new CookieCollection();
            CookieCollection oCookies = new CookieCollection();
            HttpStatusCode oCode = HttpStatusCode.NotFound;

            bool itemAdded = true;
            int pageNb = 0;
            List<FFXIVCraftingSearchItem> listItems = new List<FFXIVCraftingSearchItem>();
            while (itemAdded)
            {
                itemAdded = false;
                pageNb++;

                string searchString = iElemToSearch.ToLower().Replace(" ", "+");
                string searchResultContent = Service_Misc.GetContentFromRequest("GET https://ffxivcrafting.com/recipes/search?page=" + pageNb + "&name=" + searchString + "&min=1&max=999&class=all&per_page=50&sorting=name.asc HTTP/1.1|Host: ffxivcrafting.com|Connection: keep-alive|Accept: application/json, text/javascript, */*; q=0.01|X-Requested-With: XMLHttpRequest|User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36|Sec-Fetch-Mode: cors|Sec-Fetch-Site: same-origin|Referer: https://ffxivcrafting.com/recipes|Accept-Encoding: gzip, deflate, br|Accept-Language: fr-FR,fr;q=0.9,en-US;q=0.8,en;q=0.7|",
                    iCookies, ref oCookies, ref oCode);

                string innerHTML = ("<body><table id=\"recipe-book\"><tbody> " + searchResultContent.Replace("{\"tbody\":\"", "").Split(new string[] { "\",\"tfoot\"" }, StringSplitOptions.None)[0] + "</tbody></table></body>")
                    .Replace("\\n", "\n")
                    .Replace("\\t", "\t")
                    .Replace("\\/", "/")
                    .Replace("\\n", "\n");

                HtmlDocument searchResultDoc = new HtmlDocument();
                searchResultDoc.LoadHtml(innerHTML);
                if (null == searchResultDoc)
                {
                    Service_Misc.LogText(iLogBox, "Looks like there is no more items to find... (Page " + pageNb + ")");
                    return listItems;
                }

                HtmlNode tableNode = searchResultDoc.GetElementbyId("recipe-book");
                if (null == tableNode)
                {
                    Service_Misc.LogText(iLogBox, "Looks like there is no more items to find... (Page " + pageNb + ")");
                    return listItems;
                }

                HtmlNode tableBodyNode = Service_Misc.GetFirstChildNode(tableNode, "tbody", "");
                if (null == tableBodyNode)
                {
                    Service_Misc.LogText(iLogBox, "Looks like there is no more items to find... (Page " + pageNb + ")");
                    return listItems;
                }
                if(tableBodyNode.InnerText.Contains("No Results"))
                {
                    Service_Misc.LogText(iLogBox, "Looks like there is no more items to find... (Page " + pageNb + ")");
                    return listItems;
                }

                List<HtmlNode> listTRs = tableBodyNode.Descendants("tr").ToList();
                if (listTRs.Count > 0)
                {
                    Service_Misc.LogText(iLogBox, "Hey I found some items !");
                }
                else
                {
                    Service_Misc.LogText(iLogBox, "Looks like there is no more items to find... (Page " + pageNb + ")");
                    return listItems;
                }
                foreach (HtmlNode trNode in listTRs)
                {
                    if (null == trNode) continue;

                    List<HtmlNode> listTDs = trNode.Descendants("td").ToList();
                    if (listTDs.Count < 4)
                    {
                        Service_Misc.LogText(iLogBox, "Couldn't read this item : ");
                        Service_Misc.LogText(iLogBox, trNode.InnerHtml);
                        continue;
                    }

                    string itemName = "Unknown Item";
                    try
                    {
                        FFXIVCraftingSearchItem item = new FFXIVCraftingSearchItem();

                        item.Name = listTDs[0].InnerText.Replace(Environment.NewLine, "").Trim();
                        itemName = item.Name;
                        item.UrlImage = Service_Misc.ExtractImage(listTDs[0]);
                        item.UrlGarland = Service_Misc.ExtractLink(listTDs[0]);

                        item.Class = Service_Misc.GetFirstChildNode(listTDs[1], "img", "").GetAttributeValue("title", "");
                        item.UrlClass = "https://ffxivcrafting.com" + Service_Misc.ExtractImage(listTDs[1]);

                        item.Level = listTDs[2].InnerText.Replace(Environment.NewLine, "").Trim();

                        item.ID = Service_Misc.GetFirstChildNode(listTDs[3], "button", "").GetAttributeValue("data-item-id", "");

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

            return listItems;
        }
    }
}
