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
        private BossRushDropData BossRushDropData = new BossRushDropData();       
        private Dictionary<string, List<BossDropTable>> ParticularChestDropTables = new Dictionary<string, List<BossDropTable>>();        
        private Dictionary<string, DefeatedBossData> CurrentlyBeatenBosses = new Dictionary<string, DefeatedBossData>();

        public BossRushManager()
        {
            BossRush.BossRushPlugin.Log.LogMessage($"Running Boss Rush Manager");
            // Setting up the data holder
            BossRushDropData.CommonChestLoot = new List<BossDropTable>();
            BossRushDropData.SpecificChestLoot = new List<ParticularBossDropTable>();
            BossRushDropData.BossRushCompletion = new List<BossDropTable>();
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
                    BossRush.BossRushPlugin.Log.LogMessage($"Fetching file paths");
                    foreach (var item in filePaths)
                    {
                        BossRush.BossRushPlugin.Log.LogMessage($"Serializing:");
                        BossRushDropData bossRushDropDataListItem = DeserializeFromXML<BossRushDropData>(item);
                        //if its not null it deserialized correctly (is the correct type, exists etc)
                        if (bossRushDropDataListItem != null)
                        {
                            // Inserting the data into BossRushDropData
                            List<ParticularBossDropTable> specificChestLoot = bossRushDropDataListItem.SpecificChestLoot;
                            
                            //does our dictionary already contain this character UID?                           
                            foreach (ParticularBossDropTable enemyFromXML in specificChestLoot)
                            {                                
                                // Check each element and combine droptables if UID is matching;                                                                                                         
                                bool found = false;                                
                                foreach (ParticularBossDropTable enemyFromStorage in BossRushDropData.SpecificChestLoot)
                                {                                
                                    // Add to existing list if found as well as marking that it has been found                                                                
                                    if (enemyFromXML.targetCharacterUID == enemyFromStorage.targetCharacterUID)
                                    {                                        
                                        found = true;
                                        enemyFromStorage.DropTables.AddRange(enemyFromXML.DropTables);
                                        BossRush.BossRushPlugin.Log.LogMessage($"Adding element associated with: {enemyFromXML.targetCharacterUID}");
                                        break;
                                    }
                                }
                                if (!found)
                                {
                                    //add new an entry, initialise the list aswell.
                                    List<ParticularBossDropTable> newList = new List<ParticularBossDropTable>();
                                    newList.Add(enemyFromXML);
                                    BossRushDropData.SpecificChestLoot.AddRange(newList);
                                    BossRush.BossRushPlugin.Log.LogMessage($"Adding element associated with: {enemyFromXML.targetCharacterUID}");
                                }
                            }
                            if (bossRushDropDataListItem.CommonChestLoot != null)
                            {
                                BossRush.BossRushPlugin.Log.LogMessage($"Adding element associated with commonchest");
                                BossRushDropData.CommonChestLoot.AddRange(bossRushDropDataListItem.CommonChestLoot);
                            }
                            if (bossRushDropDataListItem.BossRushCompletion != null)
                            {
                                BossRush.BossRushPlugin.Log.LogMessage($"Adding element associated with boss rush completion");
                                BossRushDropData.BossRushCompletion.AddRange(bossRushDropDataListItem.BossRushCompletion);
                            }                            
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

        public List<BossDropTable> GetDropDataFor(string CharacterUID)
        {
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

        public T DeserializeFromXML<T>(string path)
        {
            var serializer = new XmlSerializer(typeof(T), new Type[] { typeof(BossRushDropData), typeof(ParticularBossDropTable), typeof(BossDropTable), typeof(DropItemData), typeof(WeightedDropItemData)});
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
