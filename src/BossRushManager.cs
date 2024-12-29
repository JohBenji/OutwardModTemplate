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

        private Dictionary<string, List<BossDropData>> BossDropTables = new Dictionary<string, List<BossDropData>>();

        private Dictionary<string, DefeatedBossData> CurrentlyBeatenBosses = new Dictionary<string, DefeatedBossData>();

        public BossRushManager()
        {
            FindXMLDefinitions();
        }

        private void FindXMLDefinitions()
        {
            BossRush.BossRushPlugin.Log.LogMessage($"FindXMLDefinitions");
            string[] directoriesInPluginsFolder = Directory.GetDirectories(Paths.PluginPath);
            foreach (var directory in directoriesInPluginsFolder)
            {
                string Path = $"{directory}/BossDrops";

                if (HasFolder(Path))
                {
                    BossRush.BossRushPlugin.Log.LogMessage($"Checking {Path}");

                    string[] filePaths = Directory.GetFiles(Path, "*.xml", SearchOption.AllDirectories);

                    foreach (var item in filePaths)
                    {
                        BossDropData bossDropTable = DeserializeFromXML<BossDropData>(item);

                        //if its not null it deserialized correctly (is the correct type, exists etc)
                        if (bossDropTable != null)
                        {

                            BossRush.BossRushPlugin.Log.LogMessage($"Found drop table for UID : {bossDropTable.targetCharacterUID} Table count : {bossDropTable.DropTables.Count}");
                            //does our dictionary already contain this character UID?
                            if (BossDropTables.ContainsKey(bossDropTable.targetCharacterUID))
                            {
                                //if so update the list of drop data
                                BossDropTables[bossDropTable.targetCharacterUID].Add(bossDropTable);
                            }
                            else
                            {
                                //add new an entry, initialise the list aswell.
                                List<BossDropData> newList = new List<BossDropData>();
                                newList.Add(bossDropTable);
                                BossDropTables.Add(bossDropTable.targetCharacterUID, newList);
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
            return BossDropTables.ContainsKey(CharacterUID);
        }

        public List<BossDropData> GetDropDataFor(string CharacterUID)
        {
            if (HasDropDataFor(CharacterUID))
            {
                return BossDropTables[CharacterUID];
            }

            //there is no data
            return null;
        }

        public T DeserializeFromXML<T>(string path)
        {
            var serializer = new XmlSerializer(typeof(T), new Type[] { typeof(BossDropData), typeof(BossDropTable), typeof(DropItemData), typeof(WeightedDropItemData)});
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
            return Directory.Exists(FolderLocation);
        }
}



    /*
    public class CollectRewards
    {
        public List<string> droptableUIDs;
        public List<BossRushRewardData> bossRushRewardData;

        public CollectRewards() 
        {
            droptableUIDs = new List<string>();
            bossRushRewardData = new List<BossRushRewardData>();

            // Collect UIDs
            List<string> classPathCollection = new List<string>();
            string[] bepInExPluginPlugins = Directory.GetDirectories(Paths.PluginPath);
            for (int i = 0; i < bepInExPluginPlugins.Length; i++)
            {
                string pluginFolderPath = bepInExPluginPlugins[i];
                if (pluginFolderPath != null) 
                {
                    string[] allXMLPathsInPlugin = Directory.GetFiles(pluginFolderPath, "*.xml", SearchOption.AllDirectories);
                    string[] validXMLPaths = FilterXMLPaths(allXMLPathsInPlugin);
                    classPathCollection.AddRange(validXMLPaths);
                }
            }
            
            // Now to serialize the collected paths
            for (int j = 0; j < classPathCollection.Count; j++)
            {
                string classPath = classPathCollection[j];
                bossRushRewardData.Add(DeserializeXml<BossRushRewardData>(classPath));
            }
            BossRushPlugin.bossRushRewardDatas = bossRushRewardData;
        }
        private string[] FilterXMLPaths(string[] inpaths)
        {
            // Converting to lists first
            List<string> pathsList = inpaths.ToList();
            List<string> outpathsList = new List<string>();

            // Checks each xml individually
            for (int i = 0; i < pathsList.Count; i++) 
            {
                string path = pathsList[i];
                if (IsXmlOfType<BossRushRewardData>(path))
                {
                    outpathsList.Add(path);
                }
            }
            string[] outpaths = outpathsList.ToArray();
            return outpaths;
        }

        public static bool IsXmlOfType<T>(string xmlPath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (FileStream fs = new FileStream(xmlPath, FileMode.Open))
                using (XmlReader reader = XmlReader.Create(fs))
                {
                    return serializer.CanDeserialize(reader);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during validation: {ex.Message} Merry Christmas!");
                return false;
            }
        }

        public static T DeserializeXml<T>(string xmlPath)
        {
            if (!File.Exists(xmlPath))
            {
                BossRushPlugin.Log.LogError($"XML file not found at path: {xmlPath}");
                throw new FileNotFoundException($"File not found at {xmlPath}");
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                using (FileStream stream = new FileStream(xmlPath, FileMode.Open))
                {
                    T data = (T)serializer.Deserialize(stream);
                    BossRushPlugin.Log.LogMessage("XML Deserialization Successful!");
                    return data;
                }
            }
            catch (Exception ex)
            {
                BossRushPlugin.Log.LogError($"Failed to deserialize XML: {ex.Message}");
                throw;
            }
        }
    }

    [Serializable] // Required for Unity
    [XmlRoot("BossRushRewardContainer")] // Optional, specifies the root element name in XML
    public class BossRushRewardData
    {
        [XmlArray("DropTableUID")]
        [XmlArrayItem("String")]
        public List<string> Strings = new List<string>();

        // Optional: Parameterless constructor for XmlSerializer
        public BossRushRewardData() { }

        public BossRushRewardData(List<string> strings)
        {
            Strings = strings;
        }
    }
    */
}
