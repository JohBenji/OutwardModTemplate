using BepInEx;
using NodeCanvas.BehaviourTrees;
using SideLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using static AttackPattern;

namespace BossRush
{
    public class BossRushManager
    {        
        private Dictionary<string, DefeatedBossData> CurrentlyBeatenBosses = new Dictionary<string, DefeatedBossData>();

        public BossRushManager()
        {
            BossRush.BossRushPlugin.Log.LogMessage($"Running Boss Rush Manager");
            // Setting up the data holder
            FindXMLDefinitions();
        }

        private void FindXMLDefinitions()
        {
            BossRush.BossRushPlugin.Log.LogMessage($"FindXMLDefinitions");
            string[] directoriesInPluginsFolder = Directory.GetDirectories(Paths.PluginPath);
            foreach (var directory in directoriesInPluginsFolder)
            {
                string path = $"{directory}{Path.DirectorySeparatorChar}SideLoader{Path.DirectorySeparatorChar}DropTables";
                if (HasFolder(path))
                {
                    BossRush.BossRushPlugin.Log.LogMessage($"Checking {path}");
                    string[] filePaths = Directory.GetFiles(path, "*.xml", SearchOption.AllDirectories);
                    foreach (var item in filePaths)
                    {
                        BossRush.BossRushPlugin.Log.LogMessage($"Deserializing:");                        
                        SL_BossRushDropTable bossRushDropDataListItem = DeserializeFromXML<SL_BossRushDropTable>(item);
                        BossRush.BossRushPlugin.Log.LogMessage($"Deserializing, UID: {bossRushDropDataListItem.UID}");
                        //if its not null it deserialized correctly (is the correct type, exists etc)
                        if (bossRushDropDataListItem != null)
                        {
                            if (bossRushDropDataListItem.TypeOfBossRushDropTable != BossRushDropTableType.None)
                            {
                                if (BossRushPlugin.chests != null)
                                {
                                    if (bossRushDropDataListItem.TypeOfBossRushDropTable == BossRushDropTableType.CommonChestLoot)
                                    {
                                        // CURRENTLY (1/6/2025): Testing the droptable/itemspawn system. It adds the UID correctly but the itemspawn does not put the items in the chest. The chest spawns though.
                                        BossRushPlugin.chests.AddDropTableToAll(bossRushDropDataListItem.UID);
                                    }

                                    if (bossRushDropDataListItem.TypeOfBossRushDropTable == BossRushDropTableType.SpecificChestLoot)
                                    {
                                        // The OptionalString is used to target the correct item / itemcontainer (chest). Could be an 'int' for targetting itemID, or 'string' for the itemspawncontainer identifier.
                                        if (bossRushDropDataListItem.SpecificChestLootTarget != BossSelectionEnum.None)
                                        {

                                            if (bossRushDropDataListItem.SpecificChestLootTarget == BossSelectionEnum.Elite_Crescent_Sharks) { BossRushPlugin.chests.AddDropTable(bossRushDropDataListItem.UID, "sharksDroptableUIDs"); }
                                            else if (bossRushDropDataListItem.SpecificChestLootTarget == BossSelectionEnum.Calixa) { BossRushPlugin.chests.AddDropTable(bossRushDropDataListItem.UID, "calixaDroptableUIDs"); }
                                            else if (bossRushDropDataListItem.SpecificChestLootTarget == BossSelectionEnum.Elite_Beast_Golem) { BossRushPlugin.chests.AddDropTable(bossRushDropDataListItem.UID, "beastDroptableUIDs"); }
                                            else if (bossRushDropDataListItem.SpecificChestLootTarget == BossSelectionEnum.Immaculates_Bird) { BossRushPlugin.chests.AddDropTable(bossRushDropDataListItem.UID, "cageDroptableUIDs"); }
                                            else if (bossRushDropDataListItem.SpecificChestLootTarget == BossSelectionEnum.Elite_Boozu) { BossRushPlugin.chests.AddDropTable(bossRushDropDataListItem.UID, "boozuDroptableUIDs"); }
                                            else if (bossRushDropDataListItem.SpecificChestLootTarget == BossSelectionEnum.Elite_Sublime_Shell) { BossRushPlugin.chests.AddDropTable(bossRushDropDataListItem.UID, "sublimeDroptableUIDs"); }
                                            else if (bossRushDropDataListItem.SpecificChestLootTarget == BossSelectionEnum.Elite_Crimson_Avatar) { BossRushPlugin.chests.AddDropTable(bossRushDropDataListItem.UID, "crimsonDroptableUIDs"); }
                                            else if (bossRushDropDataListItem.SpecificChestLootTarget == BossSelectionEnum.Elite_Gargoyles) { BossRushPlugin.chests.AddDropTable(bossRushDropDataListItem.UID, "gargoyleDroptableUIDs"); }
                                            else if (bossRushDropDataListItem.SpecificChestLootTarget == BossSelectionEnum.Grandmother) { BossRushPlugin.chests.AddDropTable(bossRushDropDataListItem.UID, "grandmotherDroptableUIDs"); }
                                            else if (bossRushDropDataListItem.SpecificChestLootTarget == BossSelectionEnum.Elite_Torcrab) { BossRushPlugin.chests.AddDropTable(bossRushDropDataListItem.UID, "torcrabDroptableUIDs"); }
                                            else if (bossRushDropDataListItem.SpecificChestLootTarget == BossSelectionEnum.Elite_Mantis_Shrimp) { BossRushPlugin.chests.AddDropTable(bossRushDropDataListItem.UID, "mantisDroptableUIDs"); }
                                            else if (bossRushDropDataListItem.SpecificChestLootTarget == BossSelectionEnum.Brand_Squire) { BossRushPlugin.chests.AddDropTable(bossRushDropDataListItem.UID, "squireDroptableUIDs"); }
                                            else if (bossRushDropDataListItem.SpecificChestLootTarget == BossSelectionEnum.Trolgodyte_Queen) { BossRushPlugin.chests.AddDropTable(bossRushDropDataListItem.UID, "trogqueenDroptableUIDs"); }
                                            else if (bossRushDropDataListItem.SpecificChestLootTarget == BossSelectionEnum.Elite_Burning_Man) { BossRushPlugin.chests.AddDropTable(bossRushDropDataListItem.UID, "burningmanDroptableUIDs"); }
                                            else if (bossRushDropDataListItem.SpecificChestLootTarget == BossSelectionEnum.Liches) { BossRushPlugin.chests.AddDropTable(bossRushDropDataListItem.UID, "lichesroptableUIDs"); }
                                            else if (bossRushDropDataListItem.SpecificChestLootTarget == BossSelectionEnum.Dreamer_Immaculate) { BossRushPlugin.chests.AddDropTable(bossRushDropDataListItem.UID, "immaculateDroptableUIDs"); }
                                            else if (bossRushDropDataListItem.SpecificChestLootTarget == BossSelectionEnum.Ash_Giants) { BossRushPlugin.chests.AddDropTable(bossRushDropDataListItem.UID, "giantsDroptableUIDs"); }
                                            else if (bossRushDropDataListItem.SpecificChestLootTarget == BossSelectionEnum.Ash_Giant_Highmonk) { BossRushPlugin.chests.AddDropTable(bossRushDropDataListItem.UID, "highmonkDroptableUIDs"); }
                                            else if (bossRushDropDataListItem.SpecificChestLootTarget == BossSelectionEnum.Elite_Alpha_Tuanosaur) { BossRushPlugin.chests.AddDropTable(bossRushDropDataListItem.UID, "tuanosaurDroptableUIDs"); }
                                            else { BossRushPlugin.Log.LogMessage("'Optional String' must be one of the following: 'shark', 'calixa', 'beast', 'cage', 'boozu', 'sublime', 'crimson', 'gargoyle', 'grandmother', 'torcrab', 'mantis', 'squire', 'trog', 'burning', 'lich', 'dreamer', 'giant', 'high', 'tuano'"); }

                                        }
                                        else
                                        {
                                            BossRushPlugin.Log.LogMessage("Optional string emptty. The optional string must contain the name of the boss");
                                        }
                                    }
                                    if (bossRushDropDataListItem.TypeOfBossRushDropTable == BossRushDropTableType.BossRushCompletion)
                                    {
                                        // Here will be: Droptables which will be added to the host player upon completing the boss rush
                                    }
                                }
                                else
                                {
                                    BossRushPlugin.Log.LogMessage("Chests object is null, cannot append data to it.");
                                }
                            }
                        }                        
                    }
                }
            }
            BossRushPlugin.chests.ApplyChests();
        }

        public void RecordVictory(Character player, Character foe)
        {
            RecordVictory(player.UID, foe.UID);
        }

        public void RecordVictory(string playerUID, string foeUID)
        {
            BossRush.BossRushPlugin.Log.LogMessage($"Recording Victory for {playerUID} against {foeUID}");

            if (CurrentlyBeatenBosses.ContainsKey(playerUID))
            {
                DefeatedBossData defeatedBossData = CurrentlyBeatenBosses[playerUID];

                if (defeatedBossData != null)
                {
                    if (defeatedBossData.DefeatedFoes == null)
                    {
                        defeatedBossData.DefeatedFoes = new List<string>();
                    }
                    defeatedBossData.DefeatedFoes.Add(foeUID);
                    // Force add reward to chest here.
                }               
            }
            else
            {
                CurrentlyBeatenBosses.Add(playerUID, new DefeatedBossData()
                {
                    CharacterUID = playerUID,
                    DefeatedFoes = new List<string>()
                    {
                        foeUID
                    }
                });
            }
        }
        public void ClearRecord(string playerUID)
        {
            if (CurrentlyBeatenBosses.ContainsKey(playerUID))
            {
                CurrentlyBeatenBosses.Remove(playerUID);
            }
        }


        public bool HasRecord(string playerUID)
        {
            if (CurrentlyBeatenBosses.ContainsKey(playerUID))
            {
                return CurrentlyBeatenBosses[playerUID].DefeatedFoes != null && CurrentlyBeatenBosses[playerUID].DefeatedFoes.Count > 0;
            }

            return false;
        }

        public List<string> GetDefeatedFoesFor(string playerUID)
        {
            if (CurrentlyBeatenBosses.ContainsKey(playerUID))
            {
                return CurrentlyBeatenBosses[playerUID].DefeatedFoes.ToList();
            }

            return new List<string>();
        }

        /*
        public bool HasDropDataFor(string CharacterUID)
        {
            foreach (ParticularBossDropTable enemyFromStorage in BossRushDropData.SpecificChestLoot)
            {
                // Add to existing list if found as well as marking that it has been found
                if (CharacterUID == enemyFromStorage.targetCharacterUID)
                {                    
                    return true;
                }
            }
            return false;
        }
        */

        /*
        public List<BossDropTable> GetDropDataFor(string CharacterUID)
        {
            // COMMENT: Not sure if this class is needed ¯\_(ツ)_/¯. For the chests it's definitely not needed, but for giving the items directly to the player upon boss completion it might be needed.
            foreach (ParticularBossDropTable enemyFromStorage in BossRushDropData.SpecificChestLoot)
            {
                // Add to existing list if found as well as marking that it has been found
                if (CharacterUID == enemyFromStorage.targetCharacterUID)
                {
                    return enemyFromStorage.DropTables;
                }
            }
            return null;
            
        }
        */


        public T DeserializeFromXML<T>(string path)
        {
            BossRushPlugin.Log.LogMessage("Deserialize method 1");
            var serializer = new XmlSerializer(typeof(T), new Type[] { typeof(SL_BossRushDropTable), typeof(BossRushDropTableType), typeof(BossSelectionEnum) });
            BossRushPlugin.Log.LogMessage("Deserialize method 2");
            StreamReader reader = new StreamReader(path);
            BossRushPlugin.Log.LogMessage("Deserialize method 3");
            T deserialized = (T)serializer.Deserialize(reader.BaseStream);
            /*
            // Getting the following xml bug in "T deserialized = (T)serializer.Deserialize(reader.BaseStream);":
            ```txt
            InvalidOperationException: <SL_DropTable xmlns=''> was not expected.
  at Microsoft.Xml.Serialization.GeneratedAssembly.XmlSerializationReaderSL_BossRushDropTable.Read10_SL_BossRushDropTable () [0x00064] in <577450bf85de48cb90c5798f2f5174a3>:0 
  at (wrapper managed-to-native) System.Reflection.MonoMethod.InternalInvoke(System.Reflection.MonoMethod,object,object[],System.Exception&)
  at System.Reflection.MonoMethod.Invoke (System.Object obj, System.Reflection.BindingFlags invokeAttr, System.Reflection.Binder binder, System.Object[] parameters, System.Globalization.CultureInfo culture) [0x00032] in <df7127ba07dc446d9f5831a0ec7b1d63>:0 
Rethrow as InvalidOperationException: There is an error in XML document (2, 2).
  at System.Xml.Serialization.XmlSerializer.Deserialize (System.Xml.XmlReader xmlReader, System.String encodingStyle, System.Xml.Serialization.XmlDeserializationEvents events) [0x0011b] in <0f9699188f0c414ea6fb5557f5c16d15>:0 
  at System.Xml.Serialization.XmlSerializer.Deserialize (System.Xml.XmlReader xmlReader, System.String encodingStyle) [0x00000] in <0f9699188f0c414ea6fb5557f5c16d15>:0 
  at System.Xml.Serialization.XmlSerializer.Deserialize (System.IO.Stream stream) [0x0001c] in <0f9699188f0c414ea6fb5557f5c16d15>:0 
  at BossRush.BossRushManager.DeserializeFromXML[T] (System.String path) [0x0007c] in <0cfa9979b59d4e0aafe4833d42834f59>:0 
  at BossRush.BossRushManager.FindXMLDefinitions () [0x000a9] in <0cfa9979b59d4e0aafe4833d42834f59>:0 
  at BossRush.BossRushManager..ctor () [0x00023] in <0cfa9979b59d4e0aafe4833d42834f59>:0 
  at BossRush.BossRushPlugin.Awake () [0x00038] in <0cfa9979b59d4e0aafe4833d42834f59>:0 
            ```
            */
            BossRushPlugin.Log.LogMessage("Deserialize method 4");
            reader.Close();
            BossRushPlugin.Log.LogMessage("Deserialize method 5");
            return deserialized;
        }

        public void ApplySaveData(BossRushSaveExtension SaveExtension)
        {
            CurrentlyBeatenBosses = new Dictionary<string, DefeatedBossData>();
            //re-add each victory from the save data
            foreach (var item in SaveExtension.BossRushData.DefeatedBosses)
            {
                RecordVictory(SaveExtension.BossRushData.playerUID, item);
            }
        }

        private bool HasFolder(string FolderLocation)
        {
            BossRush.BossRushPlugin.Log.LogMessage($"Has Folder ({FolderLocation}):{Directory.Exists(FolderLocation)}");
            return Directory.Exists(FolderLocation);
        }
    }
}
