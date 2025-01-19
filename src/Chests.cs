using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using SideLoader;
using UnityEngine;


namespace BossRush
{
    public class Chests
    {
        /*
         * Purpose:
         * The purpose of this class is to create chests at the end of Unknown Arena Bosses. The droptable data is sent from the BossRushManager.cs and stored here.
         */
        public Dictionary<BossSelectionEnum, BossContainerData> chestDictionary;
        private bool applied = false;

        public void initiateChests()
        {
            /*
             * Purpose:
             * Inserting the default information for a particular boss such as: Scene name, position of the chest, rotation of the chest, and an empty list of strings which will be populated with DropTableUIDs.
             */
            chestDictionary = new Dictionary<BossSelectionEnum, BossContainerData>();
            chestDictionary.Add(BossSelectionEnum.Elite_Crescent_Sharks, new BossContainerData("AbrassarDungeonsBosses", new Vector3(605.782f, 1.0346f, 35.6858f), new Vector3(0f, 206f, 0f), new List<string>()));
            chestDictionary.Add(BossSelectionEnum.Calixa, new BossContainerData("AbrassarDungeonsBosses", new Vector3(39.7859f, 0.2242f, -4.6284f), new Vector3(0f, 306f, 0f), new List<string>()));
            chestDictionary.Add(BossSelectionEnum.Elite_Beast_Golem, new BossContainerData("AbrassarDungeonsBosses", new Vector3(304.856f, 1.0158f, 24.9473f), new Vector3(0f, 206f, 0f), new List<string>()));
            chestDictionary.Add(BossSelectionEnum.Immaculates_Bird, new BossContainerData("AntiqueFieldDungeonsBosses", new Vector3(4.7675f, 1.1341f, 24.1704f), new Vector3(0f, 202f, 0f), new List<string>()));
            chestDictionary.Add(BossSelectionEnum.Elite_Boozu, new BossContainerData("AntiqueFieldDungeonsBosses", new Vector3(903.9101f, 1.0911f, 22.4521f), new Vector3(0f, 217f, 0f), new List<string>()));
            chestDictionary.Add(BossSelectionEnum.Elite_Sublime_Shell, new BossContainerData("AntiqueFieldDungeonsBosses", new Vector3(595.2085f, 1.1116f, -23.3f), new Vector3(0f, 36f, 0f), new List<string>()));
            chestDictionary.Add(BossSelectionEnum.Elite_Crimson_Avatar, new BossContainerData("CalderaDungeonsBosses", new Vector3(-2817.868f, -92.9901f, -4834.192f), new Vector3(0f, 30f, 0f), new List<string>()));            
            chestDictionary.Add(BossSelectionEnum.Elite_Gargoyles, new BossContainerData("CalderaDungeonsBosses", new Vector3(-3.5812f, 39.0874f, -6702.122f), new Vector3(0f, 34f, 0f), new List<string>()));
            chestDictionary.Add(BossSelectionEnum.Grandmother, new BossContainerData("CalderaDungeonsBosses", new Vector3(361.0504f, 5.679f, 1802.523f), new Vector3(0f, 300f, 0f), new List<string>()));
            chestDictionary.Add(BossSelectionEnum.Elite_Torcrab, new BossContainerData("CalderaDungeonsBosses", new Vector3(-2339.396f, 66.1597f, -1396.156f), new Vector3(0f, 46f, 0f), new List<string>()));
            chestDictionary.Add(BossSelectionEnum.Elite_Mantis_Shrimp, new BossContainerData("ChersoneseDungeonsBosses", new Vector3(307.5616f, 1.0227f, 59.5313f), new Vector3(0f, 212f, 0f), new List<string>()));
            chestDictionary.Add(BossSelectionEnum.Brand_Squire, new BossContainerData("ChersoneseDungeonsBosses", new Vector3(1048.942f, 27.1739f, -62.8244f), new Vector3(0f, 196f, 0f), new List<string>()));
            chestDictionary.Add(BossSelectionEnum.Trolgodyte_Queen, new BossContainerData("ChersoneseDungeonsBosses", new Vector3(4.3674f, 1.0777f, 21.8487f), new Vector3(0f, 196f, 0f), new List<string>()));
            chestDictionary.Add(BossSelectionEnum.Elite_Burning_Man, new BossContainerData("EmercarDungeonsBosses", new Vector3(1401.623f, 1.217f, -249.6975f), new Vector3(0f, 44f, 0f), new List<string>()));
            chestDictionary.Add(BossSelectionEnum.Liches, new BossContainerData("EmercarDungeonsBosses", new Vector3(3.7537f, 0.6058f, 123.4654f), new Vector3(0f, 196f, 0f), new List<string>()));
            chestDictionary.Add(BossSelectionEnum.Dreamer_Immaculate, new BossContainerData("EmercarDungeonsBosses", new Vector3(604.1223f, 1.2068f, 42.6115f), new Vector3(0f, 196f, 0f), new List<string>()));
            chestDictionary.Add(BossSelectionEnum.Ash_Giants, new BossContainerData("HallowedDungeonsBosses", new Vector3(296.2264f, 38.1305f, 1267.148f), new Vector3(0f, 36f, 0f), new List<string>()));
            chestDictionary.Add(BossSelectionEnum.Ash_Giant_Highmonk, new BossContainerData("HallowedDungeonsBosses", new Vector3(-4.7489f, 1.1121f, -64.2392f), new Vector3(0f, 17f, 0f), new List<string>()));
            chestDictionary.Add(BossSelectionEnum.Elite_Alpha_Tuanosaur, new BossContainerData("HallowedDungeonsBosses", new Vector3(1960.069f, -389.8761f, 1674.676f), new Vector3(0f, 222f, 0f), new List<string>()));
            
        }

        private void ApplyTemplateChest(BossSelectionEnum boss, BossContainerData bossContainerData)
        {
            /*
             * Purpose:
             * Creates a chest using the dictionary key and value using the SL_ItemContainerSpawn class from sideloaderw. This creates a chest object in a particular scene which has persistent content and can take custom droptableUIDs.
             */
            new SL_ItemContainerSpawn()
            {
                IdentifierName = $"{boss.ToString()}_Chest",
                ItemID = 1000040,
                Quantity = 1,
                SceneToSpawnIn = bossContainerData.Scene,
                SpawnPosition = bossContainerData.position,
                SpawnRotation = bossContainerData.rotation,
                ForceNonPickable = true,
                TryLightFueledContainer = true,
                SL_DropTableUIDs = bossContainerData.dropTableUIDs,
                HoursForDropRegeneration = 0
            }.ApplyTemplate();
        }

        public void Apply() 
        {
            /*
             * Purpose:
             * Applies all the chests.            
             */
            if (!applied) 
            {                
                foreach (var boss in chestDictionary)
                {
                    ApplyTemplateChest(boss.Key, boss.Value);
                }
            }            
            applied = true;
        }

        public void AddDropTable(BossSelectionEnum targetboss, string bossRushDropTableUID)
        {
            /*
             * Purpose:
             * Adds a droptableUID to the target boss. This is the method used for appending more droptables to the chests.
             */
            BossRushPlugin.Log.LogMessage($"Testing AddDropTable");
            if (chestDictionary != null) 
            {
                
                if (chestDictionary.TryGetValue(targetboss, out BossContainerData currentValue))
                {
                    BossRushPlugin.Log.LogMessage($"targetboss found");
                    BossRushPlugin.Log.LogMessage($"Reading old value");
                    // Fetching old values            
                    string Scene = currentValue.Scene;
                    Vector3 position = currentValue.position;
                    Vector3 rotation = currentValue.rotation;
                    List<string> dropTableUIDs = currentValue.dropTableUIDs;
                    dropTableUIDs.Add(bossRushDropTableUID); // Appends the droptableUID

                    // Setting up new value
                    BossRushPlugin.Log.LogMessage($"Setting up new value");
                    BossContainerData newValue = new BossContainerData(Scene, position, rotation, dropTableUIDs);
                    

                    // Inserting old value with new value
                    BossRushPlugin.Log.LogMessage($"Inserting new value");
                    chestDictionary[targetboss] = newValue;
                }
                else
                {
                    BossRushPlugin.Log.LogMessage($"Key '{targetboss}' not found in the dictionary.");
                }
            }                        
        }

        public void AddDropTableToAll(string bossRushDropTableUID)
        {
            /*
             * Purpose:
             * Same as AddDropTable but does it to all the unknown arena bosses.
             */
            AddDropTable(BossSelectionEnum.Elite_Crescent_Sharks, bossRushDropTableUID);
            AddDropTable(BossSelectionEnum.Calixa, bossRushDropTableUID);
            AddDropTable(BossSelectionEnum.Elite_Beast_Golem, bossRushDropTableUID);
            AddDropTable(BossSelectionEnum.Immaculates_Bird, bossRushDropTableUID);
            AddDropTable(BossSelectionEnum.Elite_Boozu, bossRushDropTableUID);
            AddDropTable(BossSelectionEnum.Elite_Sublime_Shell, bossRushDropTableUID);
            AddDropTable(BossSelectionEnum.Elite_Crimson_Avatar, bossRushDropTableUID);
            AddDropTable(BossSelectionEnum.Elite_Gargoyles, bossRushDropTableUID);
            AddDropTable(BossSelectionEnum.Grandmother, bossRushDropTableUID);
            AddDropTable(BossSelectionEnum.Elite_Torcrab, bossRushDropTableUID);
            AddDropTable(BossSelectionEnum.Elite_Mantis_Shrimp, bossRushDropTableUID);
            AddDropTable(BossSelectionEnum.Brand_Squire, bossRushDropTableUID);
            AddDropTable(BossSelectionEnum.Trolgodyte_Queen, bossRushDropTableUID);
            AddDropTable(BossSelectionEnum.Elite_Burning_Man, bossRushDropTableUID);
            AddDropTable(BossSelectionEnum.Liches, bossRushDropTableUID);
            AddDropTable(BossSelectionEnum.Dreamer_Immaculate, bossRushDropTableUID);
            AddDropTable(BossSelectionEnum.Ash_Giants, bossRushDropTableUID);
            AddDropTable(BossSelectionEnum.Ash_Giant_Highmonk, bossRushDropTableUID);
            AddDropTable(BossSelectionEnum.Elite_Alpha_Tuanosaur, bossRushDropTableUID);
        }
    }

    public struct BossContainerData
    {
        /*
         * Purpose:
         * Contains all the information relevant for contructing a chest using SL_ItemContainerSpawn.
         */
        public string Scene;
        public Vector3 position;
        public Vector3 rotation;
        public List<string> dropTableUIDs;

        public BossContainerData(string scene, Vector3 position, Vector3 rotation, List<string> dropTableUIDs)
        {
            this.Scene = scene;
            this.position = position;
            this.rotation = rotation;
            this.dropTableUIDs = dropTableUIDs;
        }
    }
}
