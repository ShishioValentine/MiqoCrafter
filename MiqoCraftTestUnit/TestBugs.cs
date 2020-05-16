using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System.IO;
using MiqoCraft;
using System.Web;
using System.Text.RegularExpressions;
using MiqoCraftCore;

namespace MiqoCraftTestUnit
{
    /// <summary>
    /// Description résumée pour UnitTest1
    /// </summary>
    [TestClass]
    public class TestBugs
    {
        public TestBugs()
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
        public void TestiNight()
        {
            JToken token = JObject.Parse(Service_Misc.RemoveIllegalCharacters(File.ReadAllText(Path.Combine(Service_Misc.GetExecutionPath(), @"References\iNight_Bug\Searchlog.log"))));

            string rawText = File.ReadAllText(Path.Combine(Service_Misc.GetExecutionPath(), @"References\iNight_Bug\GeneralDataBase.log"));
            File.WriteAllText(Path.Combine(Service_Misc.GetExecutionPath(), @"References\iNight_Bug\GeneralDataBase_raw.log"), rawText);
            string correctedText = Service_Misc.RemoveIllegalCharacters(File.ReadAllText(Path.Combine(Service_Misc.GetExecutionPath(), @"References\iNight_Bug\GeneralDataBase.log")));
            File.WriteAllText(Path.Combine(Service_Misc.GetExecutionPath(), @"References\iNight_Bug\GeneralDataBase_corrected.log"), correctedText);
            JToken token2 = JObject.Parse(Service_Misc.RemoveIllegalCharacters(File.ReadAllText(Path.Combine(Service_Misc.GetExecutionPath(), @"References\iNight_Bug\GeneralDataBase.log"))));
        }
    }
}
