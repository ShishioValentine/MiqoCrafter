using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiqoCraftCore
{
    public class MiqobotGrid
    {
        public List<string> ItemNames = new List<string>();
        public string Header;
        public string Content;
        public string Description;
        public int MaxDistance;
        public List<FFXIVPosition> Points = new List<FFXIVPosition>();

        public void ParseFromLine(string iLine)
        {
            try
            {
                Content = iLine;
                if(iLine.Contains(Environment.NewLine)) iLine = iLine.Split(new string[] { Environment.NewLine }, StringSplitOptions.None)[1];
                string jsonContent = iLine;
                List<string> listTest = jsonContent.Split('}').ToList();
                if (listTest[listTest.Count - 1] != "")
                {
                    jsonContent = jsonContent.Substring(0, jsonContent.Length - listTest[listTest.Count - 1].Length);
                }

                JToken mainToken = JObject.Parse(jsonContent);
                if (null == mainToken)  return;

                Description = mainToken["description"].Value<string>();
                string maxDistance = mainToken["maxaway"].Value<string>();
                int.TryParse(maxDistance, out MaxDistance);

                Points.Clear();
                JToken pointListToken = mainToken["points"];
                foreach (JToken pointToken in pointListToken.Children())
                {
                    FFXIVPosition pointGrid = new FFXIVPosition(pointToken["x"].Value<double>(), pointToken["y"].Value<double>(), pointToken["z"].Value<double>());

                    Points.Add(pointGrid);
                }
            }
            catch(Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }

        public override string ToString()
        {
            return Header + " - " + Description;
        }
    }
}
