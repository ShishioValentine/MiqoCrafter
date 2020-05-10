using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MiqoCraft
{
    public class Service_Misc
    {
        delegate void voidTextBoxStringDelegate(System.Windows.Forms.TextBox iBox, string itext);

        /// <summary>
        /// Log a text to a text box
        /// </summary>
        /// <param name="iLogBox"></param>
        /// <param name="iText"></param>
        public static void LogText(System.Windows.Forms.TextBox iLogBox, string iText)
        {
            if (null == iLogBox) return;
            if (iLogBox.InvokeRequired)
            {
                voidTextBoxStringDelegate d = new voidTextBoxStringDelegate(LogText);
                iLogBox.Invoke(d, new object[] { iLogBox, iText });
            }
            else
            {
                try
                {
                    iLogBox.AppendText(DateTime.Now.ToString() + " - " + iText + Environment.NewLine);
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// Extract a token from a log form
        /// </summary>
        /// <param name="iDoc"></param>
        /// <param name="iTokenTag"></param>
        /// <param name="attrName"></param>
        /// <returns></returns>
        public static string ExtractToken(HtmlDocument iDoc, string iTokenTag = "input", string attrName = "_token")
        {
            if (null == iDoc) return "";

            string result = "";
            List<HtmlNode> listNodes = iDoc.DocumentNode.Descendants(iTokenTag).ToList();
            foreach (HtmlNode node in listNodes)
            {
                HtmlAttribute attr = node.Attributes[attrName];
                if (null != attr)
                {
                    return attr.Value;
                }
                HtmlAttribute nameAttr = node.Attributes["Name"];
                if (null != nameAttr && nameAttr.Value == attrName)
                {
                    attr = node.Attributes["Value"];
                    if (null != attr) return attr.Value;
                }
            }
            return "";
        }

        /// <summary>
        /// Return a HTML document from a Telerik raw request.
        /// Useful to copy real interaction.
        /// Will also chain through post responses, result HTML document is the final response.
        /// </summary>
        /// <param name="iRequest"></param>
        /// <param name="iCookies"></param>
        /// <param name="iSpecificCOntent"></param>
        /// <returns></returns>
        public static string GetContentFromRequest(string iRequest, CookieCollection iCookies, ref CookieCollection oCookies, ref HttpStatusCode oStatus, string iSpecificCOntent = null, Encoding iDefaultEncoding = null)
        {
            List<string> listRequestArguments = iRequest.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (listRequestArguments.Count <= 0) return null;

            List<string> urlSplit = listRequestArguments[0].Split(' ').ToList();
            if (urlSplit.Count != 3) return null;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlSplit[1]);
            request.Method = urlSplit[0];
            //request.ProtocolVersion = HttpVersion.Version10;

            var sp = request.ServicePoint;
            var prop = sp.GetType().GetProperty("HttpBehaviour",
                                    BindingFlags.Instance | BindingFlags.NonPublic);
            prop.SetValue(sp, (byte)0, null);

            //Telerik Arguments
            string telerikCookies = "";
            for (int i = 1; i < listRequestArguments.Count; i++)
            {
                string argument = listRequestArguments[i];

                if (!argument.Contains(":")) continue;

                string key = argument.Split(':')[0].Trim();
                string value = argument.Split(':')[1].Trim();

                if (key == "Connection")
                {
                    if (value.ToLower() == "keep-alive")
                    {
                        request.KeepAlive = false;
                    }
                    else
                    {
                        request.KeepAlive = false;
                    }
                }
                else if (key == "Accept")
                {
                    request.Accept = value;
                }
                else if (key == "Referer")
                {
                    request.Referer = argument.Replace(key + ":", "");
                }
                else if (key == "User-Agent")
                {
                    request.UserAgent = value;
                }
                else if (key == "Content-Type")
                {
                    request.ContentType = value;
                }
                else if (key == "Cookie")
                {
                    telerikCookies = value;
                }
                else if (key == "Content-Length")
                {
                    continue; //Recomputed, see content section
                }
                else if (key == "Host")
                {
                    request.Host = value;
                }
                else
                {
                    request.Headers.Add(argument);
                }
            }

            //Uncompressed
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            request.ServicePoint.Expect100Continue = false;

            //Cookies
            request.CookieContainer = new CookieContainer();
            if (telerikCookies != "")
            {
                string fullCookieList = "Cookie:";
                List<string> telerikCookieList = telerikCookies.Split(';').ToList();
                CookieCollection existingCookies = new CookieCollection();
                foreach (string telerikCookie in telerikCookieList)
                {
                    string cookieName = telerikCookie.Split('=')[0].Trim();
                    string cookieValue = telerikCookie.Split('=')[1].Trim();

                    Cookie correspondingCookie = null;
                    foreach (Cookie cookie in iCookies)
                    {
                        if (cookie.Name == cookieName)
                        {
                            correspondingCookie = cookie;
                            break;
                        }
                    }
                    if (null == correspondingCookie)
                    {
                        correspondingCookie = new Cookie(cookieName, cookieValue, "/", ExtractDomainFromURL(urlSplit[1]));
                    }
                    fullCookieList += correspondingCookie.Name + "=" + correspondingCookie.Value + ";";
                    request.CookieContainer.Add(correspondingCookie);

                    existingCookies.Add(correspondingCookie);
                }
                foreach (Cookie cookie in iCookies)
                {
                    Cookie correspondingCookie = null;
                    foreach (Cookie existingCookie in existingCookies)
                    {
                        if (cookie.Name == existingCookie.Name)
                        {
                            correspondingCookie = existingCookie;
                            break;
                        }
                    }
                    if (correspondingCookie != null) continue; //Cookie has been added already
                    fullCookieList += cookie.Name + "=" + cookie.Value + ";";
                    try
                    {
                        request.CookieContainer.Add(cookie);
                        existingCookies.Add(cookie);
                    }
                    catch
                    {

                    }
                }
                request.Headers.Add(fullCookieList);
            }
            else
            {
                string fullCookieList = "Cookie:";
                foreach (Cookie cookie in iCookies)
                {
                    fullCookieList += cookie.Name + "=" + cookie.Value + ";";
                    try
                    {
                        request.CookieContainer.Add(cookie);
                    }
                    catch
                    {

                    }
                }
                request.Headers.Add(fullCookieList);
            }

            //Content
            if (urlSplit[0] == "POST")
            {
                //Content is in telerik request ?
                string content = listRequestArguments[listRequestArguments.Count - 1];
                if (content.Contains(":"))
                {
                    content = "";
                }
                if (null != iSpecificCOntent && iSpecificCOntent != "") content = iSpecificCOntent;

                UTF8Encoding encoding = new UTF8Encoding();
                byte[] bytes = encoding.GetBytes(content);

                request.ContentLength = bytes.Length;
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Close();
                }
            }

            //Avoid Redirect loop
            if (request.Method == "POST")
            {
                request.AllowAutoRedirect = false;
            }

            //Start request
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException exception)
            {
                response = (HttpWebResponse)exception.Response;
            }
            if (null != response)
            {
                foreach (Cookie cookie in response.Cookies)
                {
                    Cookie existingCookie = null;
                    foreach (Cookie otherCookie in oCookies)
                    {
                        if (cookie.Name == otherCookie.Name)
                        {
                            existingCookie = otherCookie;
                            break;
                        }
                    }
                    if (null == existingCookie)
                    {
                        oCookies.Add(cookie);
                    }
                    else
                    {
                        existingCookie.Value = cookie.Value;
                    }
                }

                oStatus = response.StatusCode;

                var resultStream = response.GetResponseStream();
                string result = "";
                Encoding encoding = Encoding.Default;
                if (null != iDefaultEncoding)
                {
                    encoding = iDefaultEncoding;
                }
                using (StreamReader reader = new StreamReader(resultStream, encoding))
                {
                    result = reader.ReadToEnd();
                }
                string setCookies = response.Headers[HttpResponseHeader.SetCookie];
                string url = response.Headers[HttpResponseHeader.Location];
                if (null != url && !url.Contains("www.") && !url.Contains("http"))
                {
                    url = urlSplit[1].Split(':')[0] + "://" + request.Host + url;
                }

                response.Close();


                //Adding cookies from setcookie
                if (null != setCookies && setCookies != "")
                {
                    if (setCookies.Contains(Environment.NewLine))
                    {
                        List<string> setCookieList = setCookies.Split(';').ToList();

                        foreach (string setCookieItem in setCookieList)
                        {
                            string cookieName = setCookieItem.Split('=')[0].Trim();
                            string cookieValue = "";
                            if (setCookieItem.Split('=').Count() > 1) cookieValue = setCookieItem.Split('=')[1].Trim();

                            bool exist = false;
                            try
                            {
                                Cookie correspondingCookie = new Cookie(cookieName, cookieValue, "/", ExtractDomainFromURL(urlSplit[1]));
                                for (int i = 0; i < oCookies.Count; i++)
                                {
                                    Cookie existingCookie = oCookies[i];
                                    if (null != existingCookie && existingCookie.Name == cookieName)
                                    {
                                        exist = true;
                                        break;
                                    }
                                }
                                if (exist) continue;
                                oCookies.Add(correspondingCookie);
                            }
                            catch
                            {

                            }
                        }
                    }
                    else
                    {

                    }
                }

                while (oStatus == HttpStatusCode.Found || oStatus == HttpStatusCode.Moved)
                {
                    string requestLoop = "GET " + url + " HTTP/1.1" + iRequest.Replace(listRequestArguments[0], "");

                    CookieCollection cookiesLoop = new CookieCollection();
                    cookiesLoop.Add(iCookies);
                    cookiesLoop.Add(oCookies);


                    result = GetContentFromRequest(requestLoop, cookiesLoop, ref oCookies, ref oStatus, null, iDefaultEncoding);
                }


                return result;
            }

            return null;
        }

        /// <summary>
        /// Return a HTML document from a Telerik raw request.
        /// Useful to copy real interaction.
        /// Will also chain through post responses, result HTML document is the final response.
        /// </summary>
        /// <param name="iRequest"></param>
        /// <param name="iCookies"></param>
        /// <param name="iSpecificCOntent"></param>
        /// <returns></returns>
        public static HtmlDocument GetWebPageFromRequest(string iRequest, CookieCollection iCookies, ref CookieCollection oCookies, ref HttpStatusCode oStatus, string iSpecificCOntent = null, Encoding iDefaultEncoding = null)
        {
            string content = GetContentFromRequest(iRequest, iCookies, ref oCookies, ref oStatus, iSpecificCOntent);
            if (null == content) return null;

            HtmlDocument doc = new HtmlDocument();
            if (null == iDefaultEncoding) doc.OptionDefaultStreamEncoding = Encoding.UTF8;// Encoding.GetEncoding("ISO-8859-1");
            else doc.OptionDefaultStreamEncoding = iDefaultEncoding;
            doc.LoadHtml(content);

            return doc;
        }

        /// <summary>
        /// Fill a cookie collection from a cookie string
        /// </summary>
        /// <param name="iCookie"></param>
        /// <param name="iDomain"></param>
        /// <param name="oCollection"></param>
        public static void GetCookiesFromString(string iCookie, string iDomain, ref CookieCollection oCollection)
        {
            if (null == iCookie) return;
            List<string> telerikCookieList = iCookie.Split(';').ToList();
            CookieCollection existingCookies = new CookieCollection();
            foreach (string telerikCookie in telerikCookieList)
            {
                string cookieName = telerikCookie.Split('=')[0].Trim();
                string cookieValue = telerikCookie.Replace(cookieName + "=", "").Replace(cookieName + " =", "").Trim();

                Cookie correspondingCookie = null;
                foreach (Cookie cookie in oCollection)
                {
                    if (cookie.Name == cookieName)
                    {
                        correspondingCookie = cookie;
                        break;
                    }
                }
                if (null == correspondingCookie)
                {
                    correspondingCookie = new Cookie(cookieName, cookieValue, "/", iDomain);
                    oCollection.Add(correspondingCookie);
                }
            }
        }

        /// <summary>
        /// Extract a domain url from a full URL
        /// </summary>
        /// <param name="sURL"></param>
        /// <returns></returns>
        public static string ExtractDomainFromURL(string sURL)
        {

            Regex rg = new Regex(@"://(?<host>([a-z\d][-a-z\d]*[a-z\d]\.)*[a-z][-a-z\d]+[a-z])");

            if (rg.IsMatch(sURL))

                return rg.Match(sURL).Result("${host}").Replace("www", "");

            else

                return String.Empty;

        }

        /// <summary>
        /// Extract a link from the first a node
        /// </summary>
        /// <param name="iNode"></param>
        /// <returns></returns>
        public static string ExtractLink(HtmlNode iNode)
        {
            if (null == iNode) return "";

            List<HtmlNode> listA = iNode.Descendants("a").ToList();
            if (listA.Count > 0)
            {
                return listA[0].GetAttributeValue("href", "");
            }

            return iNode.GetAttributeValue("href", "");
        }

        /// <summary>
        /// Extract a picture from the first img node
        /// </summary>
        /// <param name="iNode"></param>
        /// <returns></returns>
        public static string ExtractImage(HtmlNode iNode)
        {
            if (null == iNode) return "";

            List<HtmlNode> listA = iNode.Descendants("img").ToList();
            if (listA.Count > 0)
            {
                return listA[0].GetAttributeValue("src", "");
            }

            return iNode.GetAttributeValue("src", "");
        }

        /// <summary>
        /// Retrieve the first child node of specified type
        /// </summary>
        /// <param name="iNode"></param>
        /// <param name="iTagName"></param>
        /// <param name="iClassName"></param>
        /// <param name="iInnerText"></param>
        /// <returns></returns>
        public static HtmlNode GetFirstChildNode(HtmlNode iNode, string iTagName, string iClassName, string iInnerText = "")
        {
            if (null == iNode) return null;

            List<HtmlNode> listChilds = iNode.Descendants(iTagName).ToList();
            foreach (HtmlNode node in listChilds)
            {
                if (node.GetAttributeValue("class", "") == iClassName && node.InnerText.ToLower().Contains(iInnerText.ToLower())) return node;
                if (node.GetAttributeValue("class", "").Contains(iClassName) && node.InnerText.ToLower().Contains(iInnerText.ToLower())) return node;
                if (iClassName == "" && node.InnerText.ToLower().Contains(iInnerText.ToLower())) return node;
            }

            return null;
        }

        /// <summary>
        /// Convert time from unix to epoch
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTime FromUnixTime(long unixTime)
        {
            return epoch.AddSeconds(unixTime);
        }
        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static string GetExecutionPath()
        {
            try
            {
                return Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            }
            catch
            {
                return Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)));
            }
        }

        public static string RemoveIllegalCharacters(string iString)
        {
            return Encoding.ASCII.GetString(
       Encoding.Convert(
           Encoding.UTF8,
           Encoding.GetEncoding(
               Encoding.ASCII.EncodingName,
               new EncoderReplacementFallback(string.Empty),
               new DecoderExceptionFallback()
               ),
           Encoding.UTF8.GetBytes(iString)
       )).Replace("竕", "")
       .Replace("%}", "%\"}")
       .Replace("+1}", "+1\"}")
       .Replace("・・", "\""); 
        }
    }
    public static class ZipArchiveExtensions
    {
        public static void ExtractToDirectory(this ZipArchive archive, string destinationDirectoryName, bool overwrite)
        {
            if (!overwrite)
            {
                archive.ExtractToDirectory(destinationDirectoryName);
                return;
            }

            DirectoryInfo di = Directory.CreateDirectory(destinationDirectoryName);
            string destinationDirectoryFullPath = di.FullName;

            foreach (ZipArchiveEntry file in archive.Entries)
            {
                string completeFileName = Path.GetFullPath(Path.Combine(destinationDirectoryFullPath, file.FullName));

                if (!completeFileName.StartsWith(destinationDirectoryFullPath, StringComparison.OrdinalIgnoreCase))
                {
                    throw new IOException("Trying to extract file outside of destination directory. See this link for more info: https://snyk.io/research/zip-slip-vulnerability");
                }

                if (file.Name == "")
                {// Assuming Empty for Directory
                    Directory.CreateDirectory(Path.GetDirectoryName(completeFileName));
                    continue;
                }
                file.ExtractToFile(completeFileName, true);
            }
        }
    }
}
