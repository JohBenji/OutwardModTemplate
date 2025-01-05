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
                                // Here will be: Droptables which will be added to every chest
                            }
                            if (bossRushDropDataListItem.TypeOfBossRushDropTable == BossRushDropTableType.SpecificChestLoot)
                            {
                                // The OptionalString is used to target the correct item / itemcontainer (chest). Could be an 'int' for targetting itemID, or 'string' for the itemspawncontainer identifier.
                                string ID;
                                if (bossRushDropDataListItem.OptionalString != "")
                                {                                    
                                    ID = bossRushDropDataListItem.OptionalString;
                                    // Here will be: Droptables which will be added to a particular chest                                    
                                }
                                else
                                {
                                    // DO NOTHING.
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
