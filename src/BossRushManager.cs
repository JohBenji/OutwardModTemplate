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
                string path = $"{directory}{Path.DirectorySeparatorChar}SideLoader{Path.DirectorySeparatorChar}BossRushDrops";
                if (HasFolder(path))
                {
                    BossRush.BossRushPlugin.Log.LogMessage($"Checking {path}");
                    string[] filePaths = Directory.GetFiles(path, "*.xml", SearchOption.AllDirectories);
                    foreach (var item in filePaths)
                    {
                        BossRush.BossRushPlugin.Log.LogMessage($"Deserializing:");
                        SL_BossRushDropTable bossRushDropDataListItem = DeserializeFromXML<SL_BossRushDropTable>(item);
                        //if its not null it deserialized correctly (is the correct type, exists etc)
                        if (bossRushDropDataListItem != null)
                        {
                            if (bossRushDropDataListItem.TypeOfBossRushDropTable == BossRushDropTableType.CommonChestLoot)
                            {
                                BossRushPlugin.applyChests.AddDropTableToAll(bossRushDropDataListItem.UID);
                            }
                            if (bossRushDropDataListItem.TypeOfBossRushDropTable == BossRushDropTableType.SpecificChestLoot)
                            {
                                // The OptionalString is used to target the correct item / itemcontainer (chest). Could be an 'int' for targetting itemID, or 'string' for the itemspawncontainer identifier.
                                if (bossRushDropDataListItem.OptionalString != "")
                                {
                                    if (bossRushDropDataListItem.OptionalString.Contains("shark", StringComparison.OrdinalIgnoreCase)) { BossRushPlugin.applyChests.AddDropTable(bossRushDropDataListItem.UID, "sharksDroptableUIDs"); }
                                    else if (bossRushDropDataListItem.OptionalString.Contains("calixa", StringComparison.OrdinalIgnoreCase)) { BossRushPlugin.applyChests.AddDropTable(bossRushDropDataListItem.UID, "calixaDroptableUIDs"); }
                                    else if (bossRushDropDataListItem.OptionalString.Contains("beast", StringComparison.OrdinalIgnoreCase) || bossRushDropDataListItem.OptionalString.Contains("golem", StringComparison.OrdinalIgnoreCase)) { BossRushPlugin.applyChests.AddDropTable(bossRushDropDataListItem.UID, "beastDroptableUIDs"); }
                                    else if (bossRushDropDataListItem.OptionalString.Contains("cage", StringComparison.OrdinalIgnoreCase) || bossRushDropDataListItem.OptionalString.Contains("nicholas", StringComparison.OrdinalIgnoreCase) || bossRushDropDataListItem.OptionalString.Contains("bird", StringComparison.OrdinalIgnoreCase)) { BossRushPlugin.applyChests.AddDropTable(bossRushDropDataListItem.UID, "cageDroptableUIDs"); }
                                    else if (bossRushDropDataListItem.OptionalString.Contains("boozu", StringComparison.OrdinalIgnoreCase)) { BossRushPlugin.applyChests.AddDropTable(bossRushDropDataListItem.UID, "boozuDroptableUIDs"); }
                                    else if (bossRushDropDataListItem.OptionalString.Contains("sublime", StringComparison.OrdinalIgnoreCase) || bossRushDropDataListItem.OptionalString.Contains("shell", StringComparison.OrdinalIgnoreCase)) { BossRushPlugin.applyChests.AddDropTable(bossRushDropDataListItem.UID, "sublimeDroptableUIDs"); }
                                    else if (bossRushDropDataListItem.OptionalString.Contains("crimson", StringComparison.OrdinalIgnoreCase) || bossRushDropDataListItem.OptionalString.Contains("avatar", StringComparison.OrdinalIgnoreCase)) { BossRushPlugin.applyChests.AddDropTable(bossRushDropDataListItem.UID, "crimsonDroptableUIDs"); }
                                    else if (bossRushDropDataListItem.OptionalString.Contains("gargoyle", StringComparison.OrdinalIgnoreCase)) { BossRushPlugin.applyChests.AddDropTable(bossRushDropDataListItem.UID, "gargoyleDroptableUIDs"); }
                                    else if (bossRushDropDataListItem.OptionalString.Contains("grandmother", StringComparison.OrdinalIgnoreCase) || bossRushDropDataListItem.OptionalString.Contains("krypteia", StringComparison.OrdinalIgnoreCase)) { BossRushPlugin.applyChests.AddDropTable(bossRushDropDataListItem.UID, "grandmotherDroptableUIDs"); }
                                    else if (bossRushDropDataListItem.OptionalString.Contains("torcrab", StringComparison.OrdinalIgnoreCase)) { BossRushPlugin.applyChests.AddDropTable(bossRushDropDataListItem.UID, "torcrabDroptableUIDs"); }
                                    else if (bossRushDropDataListItem.OptionalString.Contains("mantis", StringComparison.OrdinalIgnoreCase) || bossRushDropDataListItem.OptionalString.Contains("shrimp", StringComparison.OrdinalIgnoreCase)) { BossRushPlugin.applyChests.AddDropTable(bossRushDropDataListItem.UID, "mantisDroptableUIDs"); }
                                    else if (bossRushDropDataListItem.OptionalString.Contains("squire", StringComparison.OrdinalIgnoreCase) || bossRushDropDataListItem.OptionalString.Contains("brand", StringComparison.OrdinalIgnoreCase)) { BossRushPlugin.applyChests.AddDropTable(bossRushDropDataListItem.UID, "squireDroptableUIDs"); }
                                    else if (bossRushDropDataListItem.OptionalString.Contains("trog", StringComparison.OrdinalIgnoreCase)) { BossRushPlugin.applyChests.AddDropTable(bossRushDropDataListItem.UID, "trogqueenDroptableUIDs"); }
                                    else if (bossRushDropDataListItem.OptionalString.Contains("burning", StringComparison.OrdinalIgnoreCase)) { BossRushPlugin.applyChests.AddDropTable(bossRushDropDataListItem.UID, "burningmanDroptableUIDs"); }
                                    else if (bossRushDropDataListItem.OptionalString.Contains("lich", StringComparison.OrdinalIgnoreCase)) { BossRushPlugin.applyChests.AddDropTable(bossRushDropDataListItem.UID, "lichesroptableUIDs"); }
                                    else if (bossRushDropDataListItem.OptionalString.Contains("dreamer", StringComparison.OrdinalIgnoreCase) || (bossRushDropDataListItem.OptionalString.Contains("immaculate", StringComparison.OrdinalIgnoreCase) && !bossRushDropDataListItem.OptionalString.Contains("bird", StringComparison.OrdinalIgnoreCase))) { BossRushPlugin.applyChests.AddDropTable(bossRushDropDataListItem.UID, "immaculateDroptableUIDs"); }
                                    else if (bossRushDropDataListItem.OptionalString.Contains("giant", StringComparison.OrdinalIgnoreCase) && !bossRushDropDataListItem.OptionalString.Contains("high", StringComparison.OrdinalIgnoreCase)) { BossRushPlugin.applyChests.AddDropTable(bossRushDropDataListItem.UID, "giantsDroptableUIDs"); }
                                    else if (bossRushDropDataListItem.OptionalString.Contains("high", StringComparison.OrdinalIgnoreCase) || bossRushDropDataListItem.OptionalString.Contains("monk", StringComparison.OrdinalIgnoreCase)) { BossRushPlugin.applyChests.AddDropTable(bossRushDropDataListItem.UID, "highmonkDroptableUIDs"); }
                                    else if (bossRushDropDataListItem.OptionalString.Contains("tuano", StringComparison.OrdinalIgnoreCase)) { BossRushPlugin.applyChests.AddDropTable(bossRushDropDataListItem.UID, "tuanosaurDroptableUIDs"); }
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

                            // ADDITIONAL NOTES BUT NOT RELEVANT TO WHAT IS DONE HERE NECESSARILY. KEEP IN MIND THE THAT WE NEED MORE STEPS:                            
                            // STEP 1: Deserialization (THIS IS WHAT WE DO HERE)
                            // STEP 2: Setup the chests: (1) with itemID of chests or (2) with ID string for the itemcontainer. For a particular chest, chest_A the droptable needs to be DT(CommonChestLoot) + DT(SpecificChest_A)
                            // Step 3: Make the boss rush completion work with DT (not sure how this would be done). Maybe a conversion of SL_DropTable or being forced to make a class that is also in XML but doesn't inherit from SL_DropTable
                        }


                        //try
                        //{



                        //}
                        //catch (Exception ex)
                        //{
                        //    BossRush.BossRushPlugin.Log.LogMessage(ex);
                        //}

                    }
                }
            }
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
            var serializer = new XmlSerializer(typeof(T), new Type[] { typeof(SL_BossRushDropTable)});
            StreamReader reader = new StreamReader(path);
            T deserialized = (T)serializer.Deserialize(reader.BaseStream);
            reader.Close();
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
