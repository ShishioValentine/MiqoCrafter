using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiqoCraftCore;
using System;
using System.Collections.Generic;
using System.IO;
using VPL.Application.Data;

namespace MiqoCraftTestUnit
{
    [TestClass]
    public class TestFullScenario
    {
        bool _updateDumps = false;
        string _dumpSuffix = "";

        private void InitTestEnvironment()
        {
            VPApplication.ApplicationName = "MiqoCrafter";
            if (null == VPOptions.OptionsForUnitTest)
            {
                VPOptions.OptionsForUnitTest = new MiqoCraftOptions();
            }
        }

        [TestMethod]
        public void TestTwinsilkRobeofAiming()
        {
            VPOptions.OptionsForUnitTest = null;
            _dumpSuffix = "";
            TestFullScenarioImpl("Twinsilk", FFXIVItem.TypeItem.Crafted, 21, 8, "Twinsilk Robe of Aiming", true);
        }

        [TestMethod]
        public void TestTwinsilkHoodofHealing()
        {
            VPOptions.OptionsForUnitTest = null;
            _dumpSuffix = "";
            TestFullScenarioImpl("Twinsilk Hood of Healing", FFXIVItem.TypeItem.Crafted);
        }

        [TestMethod]
        public void TestDwarvenCottonJacket()
        {
            VPOptions.OptionsForUnitTest = null;
            _dumpSuffix = "";
            TestFullScenarioImpl("Dwarven Cotton Jacket", FFXIVItem.TypeItem.Crafted);
        }

        [TestMethod]
        public void TestTitaniumLance()
        {
            VPOptions.OptionsForUnitTest = null;
            _dumpSuffix = "";
            TestFullScenarioImpl("Titanium Lance", FFXIVItem.TypeItem.Crafted);
        }

        [TestMethod]
        public void TestTitaniumLanceWithDuplicates()
        {
            VPOptions.OptionsForUnitTest = null;
            _dumpSuffix = "_withduplicates";
            MiqoCraftOptions customOptions = new MiqoCraftOptions();

            string solution_dir = Service_Misc.GetExecutionPath();
            DirectoryInfo refDirectory = new DirectoryInfo(Path.Combine(solution_dir, "References"));
            customOptions.MiqoPresetPath = Path.Combine(refDirectory.FullName, @"Options\Set1");

            VPOptions.OptionsForUnitTest = customOptions;

            TestFullScenarioImpl("Titanium Lance", FFXIVItem.TypeItem.Crafted);
        }

        [TestMethod]
        public void TestTitaniumLanceWithQuickCraft()
        {
            VPOptions.OptionsForUnitTest = null;
            _dumpSuffix = "_withquickcraft";
            MiqoCraftOptions customOptions = new MiqoCraftOptions();
            customOptions.QuickCraft = true;
            VPOptions.OptionsForUnitTest = customOptions;

            TestFullScenarioImpl("Titanium Lance", FFXIVItem.TypeItem.Crafted);
        }

        [TestMethod]
        public void TestTitaniumLanceWithCraftSectionOnly()
        {
            VPOptions.OptionsForUnitTest = null;
            _dumpSuffix = "_withcraftonly";
            MiqoCraftOptions customOptions = new MiqoCraftOptions();
            customOptions.CraftSectionOnly = true;
            VPOptions.OptionsForUnitTest = customOptions;

            TestFullScenarioImpl("Titanium Lance", FFXIVItem.TypeItem.Crafted);
        }


        [TestMethod]
        public void TestTitaniumLanceWithEulmoreMending()
        {
            VPOptions.OptionsForUnitTest = null;
            _dumpSuffix = "_withEulmore";
            MiqoCraftOptions customOptions = new MiqoCraftOptions();

            customOptions.RepairMoveValue = MiqoCraftCore.MiqoCraftCore.RepairMode.Eulmore;

            VPOptions.OptionsForUnitTest = customOptions;

            TestFullScenarioImpl("Titanium Lance", FFXIVItem.TypeItem.Crafted);
        }

        [TestMethod]
        public void TestTitaniumLanceWithManualMending()
        {
            VPOptions.OptionsForUnitTest = null;
            _dumpSuffix = "_withManualMending";
            MiqoCraftOptions customOptions = new MiqoCraftOptions();

            customOptions.RepairMoveValue = MiqoCraftCore.MiqoCraftCore.RepairMode.Repair;

            VPOptions.OptionsForUnitTest = customOptions;

            TestFullScenarioImpl("Titanium Lance", FFXIVItem.TypeItem.Crafted);
        }

        [TestMethod]
        public void TestLignumVitaeGrindingWheel()
        {
            VPOptions.OptionsForUnitTest = null;
            _dumpSuffix = "";
            TestFullScenarioImpl("Lignum Vitae Grinding Wheel", FFXIVItem.TypeItem.Crafted, 2, 0, "", true, 10, true, 4, "Estate Hall");
        }

        [TestMethod]
        public void TestDiasporeBraceletofSlaying()
        {
            VPOptions.OptionsForUnitTest = null;
            _dumpSuffix = "";
            TestFullScenarioImpl("Diaspore Bracelet of Slaying", FFXIVItem.TypeItem.Crafted, 1, 0, "", false, 20, false, 3);
        }

        [TestMethod]
        public void TestHeadoftheDreadwyrm()
        {
            _dumpSuffix = "";
            MiqoCraftOptions customOptions = new MiqoCraftOptions();

            FFXIVCraftingOptions option = new FFXIVCraftingOptions();
            option.CustomCraftingMacro = "Toto";
            option.IgnoreItem = true;
            option.ItemID = "7588";
            customOptions.ListItemOptions.Add(option);

            option = new FFXIVCraftingOptions();
            option.CustomCraftingMacro = "Titi";
            option.IgnoreItem = false;
            option.ItemID = "5116";
            customOptions.ListItemOptions.Add(option);

            option = new FFXIVCraftingOptions();
            option.CustomCraftingMacro = "";
            option.IgnoreItem = true;
            option.ItemID = "7607";
            customOptions.ListItemOptions.Add(option);

            option = new FFXIVCraftingOptions();
            option.CustomCraftingMacro = "MyMacro";
            option.IgnoreItem = false;
            option.ItemID = "9358";
            customOptions.ListItemOptions.Add(option);

            VPOptions.OptionsForUnitTest = customOptions;

            TestFullScenarioImpl("Head of the Dreadwyrm", FFXIVItem.TypeItem.Crafted, 1, 0, "", true);
        }

        public void TestFullScenarioImpl(string iItemName, FFXIVItem.TypeItem iExpectedType, int iNbExpectedItems = 1, int iPosItemInList = 0, string iRealName = "", bool iIgnoreCatalyst = false, int iQuantity = 1, bool iCollectable = false, int iNbPerSlot = 1, string iCustomTeleport = "")
        {
            InitTestEnvironment();

            string elemToSearch = iItemName;
            int nbExpectedSearchItems = iNbExpectedItems;
            int nbItemInList = iPosItemInList;
            string itemFullName = iRealName;
            if (itemFullName.Length <= 0) itemFullName = iItemName;
            FFXIVItem.TypeItem expectedType = iExpectedType;

            List<FFXIVSearchItem> listResults = GarlandTool.Search(elemToSearch, null);
            if (listResults.Count != nbExpectedSearchItems) throw new Exception("Erreur de recherche de " + elemToSearch + ": " + listResults.Count + " items found while " + nbExpectedSearchItems + " expected.");

            FFXIVItem item = listResults[nbItemInList];
            if (item.Name != itemFullName) throw new Exception("Item name is not the expected one: " + item.Name + ", expected : " + itemFullName);

            FFXIVItem itemToCraft = GarlandTool.RecBuildCraftingTree(null, item.ID);
            if (itemToCraft.Type != expectedType) throw new Exception("Item type is not the expected one: " + itemToCraft.Type + ", expected : " + expectedType);

            string solution_dir = Service_Misc.GetExecutionPath();
            DirectoryInfo refDirectory = new DirectoryInfo(Path.Combine(solution_dir, "References"));
            if (!refDirectory.Exists) throw new Exception("Reference directory " + refDirectory.FullName + " does not exist");

            MiqoCraftCore.MiqoCraftCore.MiqobotScenarioOption options = new MiqoCraftCore.MiqoCraftCore.MiqobotScenarioOption();
            options.GatheringRotation = "HQ +10%";
            options.CraftPreset = "recommended";
            options.NQHQPreset = "balanced";
            options.CustomTeleport = iCustomTeleport;
            options.Quantity = iQuantity;
            options.NbPerNode = iNbPerSlot;
            options.Collectable = iCollectable;
            options.IgnoreCatalysts = iIgnoreCatalyst;

            string fullScenario = "";
            string res = MiqoCraftCore.MiqoCraftCore.GenerateScenario(itemToCraft, options, null, out fullScenario);

            FileInfo refFile = new FileInfo(Path.Combine(refDirectory.FullName, itemFullName + _dumpSuffix + ".txt"));

            string pathResult = refFile.FullName.Replace(".txt", "_runresult.txt");
            File.WriteAllText(pathResult, res);
            if (_updateDumps) File.WriteAllText(refFile.FullName, res);
            string fullScenarioResult = File.ReadAllText(pathResult);

            FileInfo refScenarioFile = new FileInfo(Path.Combine(refDirectory.FullName, itemFullName + _dumpSuffix + " Scenario.txt"));
            pathResult = refScenarioFile.FullName.Replace(".txt", "_runresult.txt");
            File.WriteAllText(pathResult, fullScenario);
            if (_updateDumps) File.WriteAllText(refScenarioFile.FullName, fullScenario);

            if (!refFile.Exists) throw new Exception("Reference file " + refFile.FullName + " does not exist");
            string fullScenarioReference = File.ReadAllText(refFile.FullName);

            //Testing miqobot scenario
            if (fullScenarioResult.Trim() != fullScenarioReference.Trim())
            {
                throw new Exception("Resulting scenario does not match.; See " + refFile.FullName);
            }

            //Testing plain text scenario
            fullScenarioResult = File.ReadAllText(pathResult);

            if (!refScenarioFile.Exists) throw new Exception("Reference file " + refScenarioFile.FullName + " does not exist");
            string fullScenarioTextReference = File.ReadAllText(refScenarioFile.FullName);

            if (fullScenarioResult.Trim() != fullScenarioTextReference.Trim())
            {
                throw new Exception("Resulting scenario text does not match.; See " + refScenarioFile.FullName);
            }
        }
    }
}
