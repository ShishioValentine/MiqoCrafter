using MiqoCraftCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiqoCraftCore
{
    public class FFXIVItemGridF : MiqobotGrid
    {
        public string ItemName = "";
        public string Status = "";
        public string ComputedDescription = "";
        public FileInfo GridFile;
        public bool IsValid = false;

        public List<FFXIVPosition> ListAetheryteClosePoints = new List<FFXIVPosition>();

        public List<FFXIVAetheryte> ZoneAetherytes = new List<FFXIVAetheryte>();
        public List<double> ZoneAetherytesDistance = new List<double>();
        public FFXIVAetheryte ClosestAetheryte;
        public double ClosestAetheryteDistance = 0;

        public List<FFXIVGatheringNode> AllNodes = new List<FFXIVGatheringNode>();
        public List<double> AllNodesDistance = new List<double>();
        public FFXIVGatheringNode ClosestNode;
        public double ClosestNodeDistance = 0;

        public Bitmap Picture;

        public void BuildPicture()
        {
            //if (null != Picture) return;
            double minX = 0, minY = 0, maxX = 0, maxY = 0;
            int index = 0;
            foreach (FFXIVPosition point in Points)
            {
                if (point.X < minX || index == 0) minX = point.X;
                if (point.X > maxX || index == 0) maxX = point.X;
                if (point.Y < minY || index == 0) minY = point.Y;
                if (point.Y > maxY || index == 0) maxY = point.Y;
                index++;
            }
            foreach (FFXIVGatheringNode node in AllNodes)
            {
                if (null == node) continue;
                if (null == node.Position) continue;

                if (node.Position.X < minX) minX = node.Position.X;
                if (node.Position.X > maxX) maxX = node.Position.X;
                if (node.Position.Y < minY) minY = node.Position.Y;
                if (node.Position.Y > maxY) maxY = node.Position.Y;
            }
            foreach (FFXIVAetheryte aetherythe in ZoneAetherytes)
            {
                if (null == aetherythe) continue;

                if (aetherythe.Position.X < minX) minX = aetherythe.Position.X;
                if (aetherythe.Position.X > maxX) maxX = aetherythe.Position.X;
                if (aetherythe.Position.Y < minY) minY = aetherythe.Position.Y;
                if (aetherythe.Position.Y > maxY) maxY = aetherythe.Position.Y;
            }

            //Drawing Bitmap
            minX -= 10;
            maxX += 10;
            minY -= 10;
            maxY += 10;
            int width = (int)(maxX - minX + 0.5);
            int height = (int)(maxY - minY + 0.5);
            int thicknessW = width / 100;
            int thicknessH = height / 100;
            int thickness = thicknessH;
            if (thicknessW > thickness) thickness = thicknessW;
            if (thickness < 2) thickness = 2;
            if (width > 0 && height > 0)
            {
                Bitmap bm = new Bitmap(width, height);
                using (Graphics gr = Graphics.FromImage(bm))
                {
                    gr.SmoothingMode = SmoothingMode.AntiAlias;

                    Rectangle rect = new Rectangle(0, 0, width, height);
                    gr.FillRectangle(Brushes.White, rect);

                    foreach (FFXIVPosition point in Points)
                    {
                        int coordX = (int)point.X + (int)(-minX) - thickness / 2;
                        int coordY = (int)point.Y + (int)(-minY) - thickness / 2;

                        Rectangle rectPoint = new Rectangle(coordX, coordY, thickness, thickness);
                        gr.FillEllipse(Brushes.Black, rectPoint);
                    }

                    foreach (FFXIVPosition point in ListAetheryteClosePoints)
                    {
                        int coordX = (int)point.X + (int)(-minX) - thickness / 2;
                        int coordY = (int)point.Y + (int)(-minY) - thickness / 2;

                        Rectangle rectPoint = new Rectangle(coordX, coordY, thickness, thickness);
                        gr.FillEllipse(Brushes.Brown, rectPoint);
                    }

                    foreach (FFXIVGatheringNode node in AllNodes)
                    {
                        if (null == node.Position) continue;
                        int coordX = (int)node.Position.X + (int)(-minX) - thickness / 2;
                        int coordY = (int)node.Position.Y + (int)(-minY) - thickness / 2;

                        Rectangle rectPoint = new Rectangle(coordX, coordY, thickness, thickness);
                        gr.FillEllipse(Brushes.GreenYellow, rectPoint);
                    }

                    foreach (FFXIVAetheryte aetheryte in ZoneAetherytes)
                    {
                        int coordX = (int)aetheryte.Position.X + (int)(-minX) - thickness / 2;
                        int coordY = (int)aetheryte.Position.Y + (int)(-minY) - thickness / 2;

                        Rectangle rectPoint = new Rectangle(coordX, coordY, thickness, thickness);
                        gr.FillEllipse(Brushes.Blue, rectPoint);
                    }

                    if (null != ClosestNode && null != ClosestNode.Position)
                    {
                        int coordX = (int)ClosestNode.Position.X + (int)(-minX) - thickness / 2;
                        int coordY = (int)ClosestNode.Position.Y + (int)(-minY) - thickness / 2;

                        Rectangle rectPoint = new Rectangle(coordX, coordY, thickness, thickness);
                        gr.FillEllipse(Brushes.Green, rectPoint);
                    }

                    if (null != ClosestAetheryte)
                    {
                        int coordX = (int)ClosestAetheryte.Position.X + (int)(-minX) - thickness / 2;
                        int coordY = (int)ClosestAetheryte.Position.Y + (int)(-minY) - thickness / 2;

                        Rectangle rectPoint = new Rectangle(coordX, coordY, thickness, thickness);
                        gr.FillEllipse(Brushes.Violet, rectPoint);
                    }
                }
                Picture = bm;
            }
        }

        public void Analyze(List<FFXIVAetheryte> iAetherytes, List<FFXIVGatheringNode> iNodes, ref Dictionary<string, double> dictionaryClosestAetherytes)
        {
            Status = "Unknown";
            IsValid = false;
            try
            {
                string contentGrid = File.ReadAllText(GridFile.FullName);
                if (contentGrid == "")
                {
                    Status = "Empty Grid";
                    return;
                }

                ParseFromLine(contentGrid);

                //Finding item gathering nodes
                bool noGatheringNodeMode = false;
                List<FFXIVGatheringNode> correspondingNodes = new List<FFXIVGatheringNode>();
                foreach (FFXIVGatheringNode node in iNodes)
                {
                    if (null == node) continue;

                    string foundItemName = node.NodeItems.Find(x => x != null && x.ToLower().Trim() == ItemName.ToLower().Trim());
                    if (foundItemName != null && foundItemName != "")
                    {
                        correspondingNodes.Add(node);
                    }
                }
                if (correspondingNodes.Count <= 0 && ItemName != "")
                {
                    List<FFXIVSearchItem> resultGarlandTools = GarlandTool.Search(ItemName, null, FFXIVItem.TypeItem.Gathered);
                    if (resultGarlandTools.Count > 0)
                    {
                        List<FFXIVGatheringNode> correspondingNodesFromGarland = GarlandTool.GetGatheringNodesFromItem(resultGarlandTools[0].ID);

                        foreach (FFXIVGatheringNode garlandNode in correspondingNodesFromGarland)
                        {
                            string foundItemName = garlandNode.NodeItems.Find(x => x != null && x.ToLower().Trim() == ItemName.ToLower().Trim());
                            if (foundItemName != null && foundItemName != "")
                            {
                                correspondingNodes.Add(garlandNode);
                            }
                            /*
                                FFXIVGatheringNode bestNode = null;
                                double maxCorrespondance = -1;
                                foreach (FFXIVGatheringNode node in iNodes)
                                {
                                    if(node.Zone.ToLower().Trim() != garlandNode.Zone.ToLower().Trim())
                                    {
                                        continue;
                                    }

                                    double nbSameItem = 0;
                                    double nbTotalItem = node.NodeItems.Count;

                                    foreach(string itemName in node.NodeItems)
                                    {
                                        if (null != garlandNode.NodeItems.Find(x => x != null && x.ToLower().Trim() == itemName.ToLower().Trim()))
                                        {
                                            nbSameItem += 1.0;
                                        }
                                    }
                                    double correspondance = 100.0 * nbSameItem / nbTotalItem;
                                    if(maxCorrespondance < correspondance)
                                    {
                                        maxCorrespondance = correspondance;
                                        bestNode = node;
                                    }
                                }
                                if (maxCorrespondance < 0) continue;
                                if (null == bestNode) continue;
                                correspondingNodes.Add(bestNode);
                                */
                        }
                    }
                    noGatheringNodeMode = correspondingNodes.Count > 0;
                }
                AllNodes = correspondingNodes;

                //Finding closest node
                double minDistance = -1;
                if (!noGatheringNodeMode)
                {
                    foreach (FFXIVGatheringNode node in correspondingNodes)
                    {
                        if (null == node) continue;

                        double nodeDistance = -1;
                        foreach (FFXIVPosition point in Points)
                        {
                            double distance = node.Position.PlanarDistanceTo(point);
                            if (minDistance < 0 || distance < minDistance)
                            {
                                minDistance = distance;
                                ClosestNode = node;
                                ClosestNodeDistance = minDistance;
                            }
                            if (nodeDistance < 0 || distance < nodeDistance)
                            {
                                nodeDistance = distance;
                            }
                        }
                        AllNodesDistance.Add(nodeDistance);
                    }
                    if (minDistance > 50)
                    {
                        Status = "Too far from closest gathering node.";
                        return;
                    }
                    if (null == ClosestNode)
                    {
                        Status = "No gathering node.";
                        return;
                    }
                }
                else
                {
                    if(AllNodes.Count == 1)
                    {
                        ClosestNode = AllNodes[0];
                    }
                    foreach (FFXIVGatheringNode node in correspondingNodes)
                    {
                        AllNodesDistance.Add(-1);
                    }
                }

                string statusPrefix = "";
                if (noGatheringNodeMode) statusPrefix = "[No-Node] ";
                //Finding zone aetherytes
                List<FFXIVAetheryte> listZoneAetherytes = new List<FFXIVAetheryte>();
                foreach (FFXIVAetheryte aetherythe in iAetherytes)
                {
                    if(noGatheringNodeMode)
                    {
                        foreach (FFXIVGatheringNode node in correspondingNodes)
                        {
                            if (aetherythe.Zone == node.Zone)
                            {
                                listZoneAetherytes.Add(aetherythe);
                            }
                        }
                    }
                    else
                    {
                        if (aetherythe.Zone == ClosestNode.Zone)
                        {
                            listZoneAetherytes.Add(aetherythe);
                        }
                    }
                }

                //Finding closest aetheryte
                double minDistanceAetheryte = -1;
                List<FFXIVPosition> listAetheryteClosePoints = new List<FFXIVPosition>();
                foreach (FFXIVAetheryte aetherythe in listZoneAetherytes)
                {
                    if (null == aetherythe) continue;

                    double distanceAetheryte = -1;
                    foreach (FFXIVPosition point in Points)
                    {
                        double distance = aetherythe.Position.PlanarDistanceTo(point);
                        if (minDistanceAetheryte < 0 || distance < minDistanceAetheryte)
                        {
                            minDistanceAetheryte = distance;
                            ClosestAetheryte = aetherythe;
                            ClosestAetheryteDistance = minDistanceAetheryte;
                        }
                        if (distanceAetheryte < 0 || distance < distanceAetheryte)
                        {
                            distanceAetheryte = distance;
                        }
                        if (distance < 50)
                        {
                            listAetheryteClosePoints.Add(point);
                        }
                    }
                    ZoneAetherytes.Add(aetherythe);
                    ZoneAetherytesDistance.Add(distanceAetheryte);
                }
                if (minDistanceAetheryte > 40)
                {
                    Status = statusPrefix + "Too far from closest aetheryte.";
                    return;
                }
                if (null == ClosestAetheryte)
                {
                    Status = statusPrefix + "No Aetheryte.";
                    return;
                }
                if (listAetheryteClosePoints.Count < 1)
                {
                    Status = statusPrefix + "Not enough close points.";
                    return;
                }
                ListAetheryteClosePoints = listAetheryteClosePoints;
                if (dictionaryClosestAetherytes.ContainsKey(ItemName))
                {
                    double distance = dictionaryClosestAetherytes[ItemName];
                    if (distance < minDistanceAetheryte)
                    {
                        Status = statusPrefix + "Other grid is closer to grid points.";
                        return;
                    }
                }
                dictionaryClosestAetherytes[ItemName] = minDistanceAetheryte;

                Status = statusPrefix + "OK";
                IsValid = true;
            }
            catch (Exception e)
            {
                Status = e.Message;
            }
        }

        public void SaveAsGrids()
        {
            if (!IsValid) return;

            BuildPicture();

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
            if(null != ClosestNode)
            {
                foreach (string itemName in ClosestNode.NodeItems)
                {
                    string fileName = Service_Misc.UppercaseWords(itemName).Replace(" (Hidden)", "").Replace(" (Rare)", "").Trim() + " Grid";
                    fileName = fileName.Split(new string[] { "---" }, StringSplitOptions.None)[0] + ".txt";

                    Picture.Save(Path.Combine(newGridBitmapDirectory.FullName, fileName) + "_" + ClosestNodeDistance + ".png", ImageFormat.Png);

                    string contentGrid = File.ReadAllText(GridFile.FullName);
                    string correctedGrid = contentGrid;
                    string correctedDescription = "[" + ClosestAetheryte.Zone.Trim() + " @" + ClosestAetheryte.Name.Trim() + "]";

                    string startDescription = "{\"description\":";
                    string endDescription = ",\"maxaway\"";
                    if (!correctedGrid.Contains(startDescription) || !correctedGrid.Contains(endDescription))
                    {
                        Status = "Bad format for automatic description";
                        return;
                    }

                    string beginGrid = contentGrid.Split(new string[] { startDescription }, StringSplitOptions.None)[0];
                    string endGrid = contentGrid.Split(new string[] { endDescription }, StringSplitOptions.None)[1];

                    ComputedDescription = correctedDescription;
                    correctedGrid = beginGrid + startDescription + "\"" + correctedDescription + "\"" + endDescription + endGrid;
                    File.WriteAllText(Path.Combine(newGridDirectory.FullName, Service_Misc.UppercaseWords(fileName)), correctedGrid);
                }
            }
            else
            {
                string fileName = Service_Misc.UppercaseWords(ItemName).Replace(" (Hidden)", "").Replace(" (Rare)", "").Trim() + " Grid";
                fileName = fileName.Split(new string[] { "---" }, StringSplitOptions.None)[0] + ".txt";

                Picture.Save(Path.Combine(newGridBitmapDirectory.FullName, fileName) + "_" + ClosestNodeDistance + ".png", ImageFormat.Png);

                string contentGrid = File.ReadAllText(GridFile.FullName);
                string correctedGrid = contentGrid;
                string correctedDescription = "[" + ClosestAetheryte.Zone.Trim() + " @" + ClosestAetheryte.Name.Trim() + "]";

                string startDescription = "{\"description\":";
                string endDescription = ",\"maxaway\"";
                if (!correctedGrid.Contains(startDescription) || !correctedGrid.Contains(endDescription))
                {
                    Status = "Bad format for automatic description";
                    return;
                }

                string beginGrid = contentGrid.Split(new string[] { startDescription }, StringSplitOptions.None)[0];
                string endGrid = contentGrid.Split(new string[] { endDescription }, StringSplitOptions.None)[1];

                ComputedDescription = correctedDescription;
                correctedGrid = beginGrid + startDescription + "\"" + correctedDescription + "\"" + endDescription + endGrid;
                File.WriteAllText(Path.Combine(newGridDirectory.FullName, Service_Misc.UppercaseWords(fileName)), correctedGrid);
            }


        }
    }
}
