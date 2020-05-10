using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiqoCraft;
using Newtonsoft.Json.Linq;

namespace MiqoCraftTestUnit
{
    /// <summary>
    /// Description résumée pour TestGrids
    /// </summary>
    [TestClass]
    public class TestGrids
    {
        public TestGrids()
        {
            //
            // TODO: ajoutez ici la logique du constructeur
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Obtient ou définit le contexte de test qui fournit
        ///des informations sur la série de tests active, ainsi que ses fonctionnalités.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Attributs de tests supplémentaires
        //
        // Vous pouvez utiliser les attributs supplémentaires suivants lorsque vous écrivez vos tests :
        //
        // Utilisez ClassInitialize pour exécuter du code avant d'exécuter le premier test de la classe
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Utilisez ClassCleanup pour exécuter du code une fois que tous les tests d'une classe ont été exécutés
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Utilisez TestInitialize pour exécuter du code avant d'exécuter chaque test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Utilisez TestCleanup pour exécuter du code après que chaque test a été exécuté
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void TestCacheGrids()
        {
            DirectoryInfo cacheGridDirectory = new DirectoryInfo(Path.Combine(Service_Misc.GetExecutionPath(), "CacheGrid"));
            if (!cacheGridDirectory.Exists) throw new Exception("Cache grid directory does not exist !");

            foreach(FileInfo file in cacheGridDirectory.GetFiles())
            {
                string gridText = File.ReadAllText(file.FullName);

                if (gridText.IndexOf("grid.") != 0) throw new Exception("Grid text file does not start with grid. : " + file.FullName);

                string gridHeader = gridText.Split('{')[0];

                string gridContent = gridText.Replace(gridHeader, "").Trim();

                JToken gridMainToken = JObject.Parse(gridContent);

                string gridDescription = gridMainToken["description"].Value<string>();

                try
                {
                    string gridTeleportDescription = gridDescription.Split('[')[1].Split(']')[0];
                    if (!gridTeleportDescription.Contains("@")) throw new Exception("Grid description " + gridDescription + " doesn not include teleport name : " + file.FullName);

                    string gridArea = gridTeleportDescription.Split('@')[0].Trim();
                    string gridAetheryte = gridTeleportDescription.Split('@')[1].Trim();

                    List<string> listAreas = new List<string>();
                    List<string> listTeleports = new List<string>();
                    listAreas.Add("Limsa Lominsa Lower Decks"); listTeleports.Add("Limsa Lominsa Lower Decks");
                    listAreas.Add("Middle La Noscea"); listTeleports.Add("Summerford Farms");
                    listAreas.Add("Lower La Noscea"); listTeleports.Add("Moraby Drydocks");
                    listAreas.Add("Eastern La Noscea"); listTeleports.Add("Costa del Sol");
                    listAreas.Add("Eastern La Noscea"); listTeleports.Add("Wineport");
                    listAreas.Add("Western La Noscea"); listTeleports.Add("Swiftperch");
                    listAreas.Add("Western La Noscea"); listTeleports.Add("Aleport");
                    listAreas.Add("Upper La Noscea"); listTeleports.Add("Camp Bronze Lake");
                    listAreas.Add("Outer La Noscea"); listTeleports.Add("Camp Overlook");
                    listAreas.Add("Wolves' Den Pier"); listTeleports.Add("Wolves' Den Pier");
                    listAreas.Add("New Gridania"); listTeleports.Add("New Gridania");
                    listAreas.Add("Central Shroud"); listTeleports.Add("Bentbranch Meadows");
                    listAreas.Add("East Shroud"); listTeleports.Add("The Hawthorne Hut");
                    listAreas.Add("South Shroud"); listTeleports.Add("Quarrymill");
                    listAreas.Add("South Shroud"); listTeleports.Add("Camp Tranquil");
                    listAreas.Add("North Shroud"); listTeleports.Add("Fallgourd Float");
                    listAreas.Add("Ul'dah - Steps of Nald"); listTeleports.Add("Ul'dah - Steps of Nald");
                    listAreas.Add("Western Thanalan"); listTeleports.Add("Horizon");
                    listAreas.Add("Central Thanalan"); listTeleports.Add("Black Brush Station");
                    listAreas.Add("Eastern Thanalan"); listTeleports.Add("Camp Drybone");
                    listAreas.Add("Southern Thanalan"); listTeleports.Add("Little Ala Mhigo");
                    listAreas.Add("Southern Thanalan"); listTeleports.Add("Forgotten Springs");
                    listAreas.Add("Northern Thanalan"); listTeleports.Add("Camp Bluefog");
                    listAreas.Add("Northern Thanalan"); listTeleports.Add("Ceruleum Processing Plant");
                    listAreas.Add("The Gold Saucer"); listTeleports.Add("The Gold Saucer");
                    listAreas.Add("Foundation"); listTeleports.Add("Foundation");
                    listAreas.Add("Coerthas Central Highlands"); listTeleports.Add("Camp Dragonhead");
                    listAreas.Add("Coerthas Western Highlands"); listTeleports.Add("Falcon's Nest");
                    listAreas.Add("The Sea of Clouds"); listTeleports.Add("Camp Cloudtop");
                    listAreas.Add("The Sea of Clouds"); listTeleports.Add("Ok' Zundu");
                    listAreas.Add("Azys Lla"); listTeleports.Add("Helix");
                    listAreas.Add("Idyllshire"); listTeleports.Add("Idyllshire");
                    listAreas.Add("The Dravanian Forelands"); listTeleports.Add("Tailfeather");
                    listAreas.Add("The Dravanian Forelands"); listTeleports.Add("Anyx Trine");
                    listAreas.Add("The Churning Mists"); listTeleports.Add("Moghome");
                    listAreas.Add("The Churning Mists"); listTeleports.Add("Zenith");
                    listAreas.Add("Rhalgr's Reach"); listTeleports.Add("Rhalgr's Reach");
                    listAreas.Add("The Fringes"); listTeleports.Add("Castrum Oriens");
                    listAreas.Add("The Fringes"); listTeleports.Add("The Peering Stones");
                    listAreas.Add("The Peaks"); listTeleports.Add("Ala Gannha");
                    listAreas.Add("The Peaks"); listTeleports.Add("Ala Ghiri");
                    listAreas.Add("The Lochs"); listTeleports.Add("Porta Praetoria");
                    listAreas.Add("The Lochs"); listTeleports.Add("The Ala Mhigan Quarter");
                    listAreas.Add("Kugane"); listTeleports.Add("Kugane");
                    listAreas.Add("The Ruby Sea"); listTeleports.Add("Tamamizu");
                    listAreas.Add("The Ruby Sea"); listTeleports.Add("Onokoro");
                    listAreas.Add("Yanxia"); listTeleports.Add("Namai");
                    listAreas.Add("Yanxia"); listTeleports.Add("The House of the Fierce");
                    listAreas.Add("The Azim Steppe"); listTeleports.Add("Reunion");
                    listAreas.Add("The Azim Steppe"); listTeleports.Add("The Dawn Throne");
                    listAreas.Add("The Doman Enclave"); listTeleports.Add("The Doman Enclave");
                    listAreas.Add("The Crystarium"); listTeleports.Add("The Crystarium");
                    listAreas.Add("Lakeland"); listTeleports.Add("Fort Jobb");
                    listAreas.Add("Lakeland"); listTeleports.Add("The Ostall Imperative");
                    listAreas.Add("Kholusia"); listTeleports.Add("Stilltide");
                    listAreas.Add("Kholusia"); listTeleports.Add("Wright");
                    listAreas.Add("Kholusia"); listTeleports.Add("Tomra");
                    listAreas.Add("Amh Araeng"); listTeleports.Add("Mord Souq");
                    listAreas.Add("Amh Araeng"); listTeleports.Add("The Inn at Journey's Head");
                    listAreas.Add("Amh Araeng"); listTeleports.Add("Twine");
                    listAreas.Add("Il Mheg"); listTeleports.Add("Lydha Lran");
                    listAreas.Add("Il Mheg"); listTeleports.Add("Pla Enni");
                    listAreas.Add("Il Mheg"); listTeleports.Add("Wolekdorf");
                    listAreas.Add("The Rak'tika Greatwood"); listTeleports.Add("Slitherbough");
                    listAreas.Add("The Rak'tika Greatwood"); listTeleports.Add("Fanow");
                    listAreas.Add("Mor Dhona"); listTeleports.Add("Revenant's Toll");

                    int indexTeleport = listTeleports.IndexOf(gridAetheryte);
                    if (indexTeleport < 0) throw new Exception("Aetheryte unknown : " + gridAetheryte + ": " + file.FullName);

                    if (listAreas[indexTeleport].ToLower().Replace("the ", "").Trim() != gridArea.ToLower().Trim()
                        && listAreas[indexTeleport] != gridArea
                        && listAreas[indexTeleport].ToLower().Trim() != gridArea.ToLower().Trim()) throw new Exception("Incorrect Area : \"" + gridArea + "\" Expected : \"" + listAreas[indexTeleport] + "\" - File : " + file.FullName);
                }
                catch (Exception exc)
                {
                    throw new Exception("Grid description " + gridDescription + " has generated a CD : " + file.FullName + Environment.NewLine + exc.Message);
                }
            }
        }
    }
}
