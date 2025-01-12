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
        private List<string> sharksDroptableUIDs;
        private List<string> calixaDroptableUIDs;
        private List<string> beastDroptableUIDs;

        private List<string> cageDroptableUIDs;
        private List<string> boozuDroptableUIDs;
        private List<string> sublimeDroptableUIDs;

        private List<string> crimsonDroptableUIDs;
        private List<string> gargoyleDroptableUIDs;
        private List<string> grandmotherDroptableUIDs;
        private List<string> torcrabDroptableUIDs;

        private List<string> mantisDroptableUIDs;
        private List<string> squireDroptableUIDs;
        private List<string> trogqueenDroptableUIDs;

        private List<string> burningmanDroptableUIDs;
        private List<string> lichesroptableUIDs;
        private List<string> immaculateDroptableUIDs;

        private List<string> giantsDroptableUIDs;
        private List<string> highmonkDroptableUIDs;
        private List<string> tuanosaurDroptableUIDs;

        private int iterationNumber = 0;

        public void initiateChests()
        {
            sharksDroptableUIDs = new List<string>();
            calixaDroptableUIDs = new List<string>();
            beastDroptableUIDs = new List<string>();
            cageDroptableUIDs = new List<string>();
            boozuDroptableUIDs = new List<string>();
            sublimeDroptableUIDs = new List<string>();
            crimsonDroptableUIDs = new List<string>();
            gargoyleDroptableUIDs = new List<string>();
            grandmotherDroptableUIDs = new List<string>();
            torcrabDroptableUIDs = new List<string>();
            mantisDroptableUIDs = new List<string>();
            squireDroptableUIDs = new List<string>();
            trogqueenDroptableUIDs = new List<string>();
            burningmanDroptableUIDs = new List<string>();
            lichesroptableUIDs = new List<string>();
            immaculateDroptableUIDs = new List<string>();
            giantsDroptableUIDs = new List<string>();
            highmonkDroptableUIDs = new List<string>();
            tuanosaurDroptableUIDs = new List<string>();
        }

        public void ApplyTemplate(string sceneName, Vector3 spawnPosition, Vector3 spawnRotation, List<string> droptableUIDs)
        {
            BossRushPlugin.Log.LogMessage($"For '{sceneName}_{(int)spawnPosition.magnitude}_{(int)spawnRotation.magnitude}':");
            foreach (string uid in droptableUIDs)
            {
                BossRushPlugin.Log.LogMessage($"Adding UID: {uid}");
            }
            BossRushPlugin.Log.LogMessage($"Applying template");

            new SL_ItemContainerSpawn()
            {
                IdentifierName = $"{sceneName}_{(int)spawnPosition.magnitude}_{(int)spawnRotation.magnitude}_{string.Concat(new System.Random().Next(0, 100000000).ToString("D8"))}",
                ItemID = 1000040,
                Quantity = 1,
                SceneToSpawnIn = sceneName,
                SpawnPosition = spawnPosition,
                SpawnRotation = spawnRotation,
                ForceNonPickable = true,
                TryLightFueledContainer = true,
                SL_DropTableUIDs = droptableUIDs,
                HoursForDropRegeneration = 0
            }.ApplyTemplate();
        }
        
        public void ApplyChests()
        {
            BossRushPlugin.Log.LogMessage($"Applying chests... iteration number {iterationNumber}");
            iterationNumber = iterationNumber + 1;
            ApplyTemplate("AbrassarDungeonsBosses",new Vector3(605.782f, 1.0346f, 35.6858f),new Vector3(0f,206f,0f), sharksDroptableUIDs); // Sharks
            ApplyTemplate("AbrassarDungeonsBosses", new Vector3(39.7859f, 0.2242f, -4.6284f), new Vector3(0f, 306f, 0f), calixaDroptableUIDs); // Calixa
            ApplyTemplate("AbrassarDungeonsBosses", new Vector3(304.856f, 1.0158f, 24.9473f), new Vector3(0f, 206f, 0f), beastDroptableUIDs); // Beast
            ApplyTemplate("AntiqueFieldDungeonsBosses", new Vector3(4.7675f, 1.1341f, 24.1704f), new Vector3(0f, 202f, 0f), cageDroptableUIDs); // Cage
            ApplyTemplate("AntiqueFieldDungeonsBosses", new Vector3(903.9101f, 1.0911f, 22.4521f), new Vector3(0f, 217f, 0f), boozuDroptableUIDs); // Boozu
            ApplyTemplate("AntiqueFieldDungeonsBosses", new Vector3(595.2085f, 1.1116f, -23.3f), new Vector3(0f, 36f, 0f), sublimeDroptableUIDs); // Sublime
            ApplyTemplate("CalderaDungeonsBosses", new Vector3(-2817.868f, -92.9901f, -4834.192f), new Vector3(0f, 30f, 0f), crimsonDroptableUIDs); // Crimson
            ApplyTemplate("CalderaDungeonsBosses", new Vector3(-3.5812f, 39.0874f, -6702.122f), new Vector3(0f, 34f, 0f), gargoyleDroptableUIDs); // Gargoyle
            ApplyTemplate("CalderaDungeonsBosses", new Vector3(361.0504f, 5.679f, 1802.523f), new Vector3(0f, 300f, 0f), grandmotherDroptableUIDs); // Grandmother 
            ApplyTemplate("CalderaDungeonsBosses", new Vector3(-2339.396f, 66.1597f, -1396.156f), new Vector3(0f, 46f, 0f), torcrabDroptableUIDs); // Torcrab 
            ApplyTemplate("ChersoneseDungeonsBosses", new Vector3(307.5616f, 1.0227f, 59.5313f), new Vector3(0f, 212f, 0f), mantisDroptableUIDs); // Mantis
            ApplyTemplate("ChersoneseDungeonsBosses", new Vector3(1048.942f, 27.1739f, -62.8244f), new Vector3(0f, 196f, 0f), squireDroptableUIDs); // Squire
            ApplyTemplate("ChersoneseDungeonsBosses", new Vector3(4.3674f, 1.0777f, 21.8487f), new Vector3(0f, 196f, 0f), trogqueenDroptableUIDs); // TrogQueen 
            ApplyTemplate("EmercarDungeonsBosses", new Vector3(1401.623f, 1.217f, -249.6975f), new Vector3(0f, 44f, 0f), burningmanDroptableUIDs); // BurningMan
            ApplyTemplate("EmercarDungeonsBosses", new Vector3(3.7537f, 0.6058f, 123.4654f), new Vector3(0f, 196f, 0f), lichesroptableUIDs); // Liches
            ApplyTemplate("EmercarDungeonsBosses", new Vector3(604.1223f, 1.2068f, 42.6115f), new Vector3(0f, 196f, 0f), immaculateDroptableUIDs); // Immaculate 
            ApplyTemplate("HallowedDungeonsBosses", new Vector3(296.2264f, 38.1305f, 1267.148f), new Vector3(0f, 36f, 0f), giantsDroptableUIDs); // Giants 
            ApplyTemplate("HallowedDungeonsBosses", new Vector3(-4.7489f, 1.1121f, -64.2392f), new Vector3(0f, 17f, 0f), highmonkDroptableUIDs); // Highmonk 
            ApplyTemplate("HallowedDungeonsBosses", new Vector3(1960.069f, -389.8761f, 1674.676f), new Vector3(0f, 222f, 0f), tuanosaurDroptableUIDs); // Tuanosaur 
        }

        public void AddDropTable(string bossRushDropTableUID,string targetBoss)
        {
            if (targetBoss == "sharksDroptableUIDs") { sharksDroptableUIDs.Add(bossRushDropTableUID); }
            else if (targetBoss == "calixaDroptableUIDs") { calixaDroptableUIDs.Add(bossRushDropTableUID); }
            else if (targetBoss == "beastDroptableUIDs") { beastDroptableUIDs.Add(bossRushDropTableUID); }
            else if (targetBoss == "cageDroptableUIDs") { cageDroptableUIDs.Add(bossRushDropTableUID); }
            else if (targetBoss == "boozuDroptableUIDs") { boozuDroptableUIDs.Add(bossRushDropTableUID); }
            else if (targetBoss == "sublimeDroptableUIDs") { sublimeDroptableUIDs.Add(bossRushDropTableUID); }
            else if (targetBoss == "crimsonDroptableUIDs") { crimsonDroptableUIDs.Add(bossRushDropTableUID); }
            else if (targetBoss == "gargoyleDroptableUIDs") { gargoyleDroptableUIDs.Add(bossRushDropTableUID); }
            else if (targetBoss == "grandmotherDroptableUIDs") { grandmotherDroptableUIDs.Add(bossRushDropTableUID); }
            else if (targetBoss == "squireDroptableUIDs") { squireDroptableUIDs.Add(bossRushDropTableUID); }
            else if (targetBoss == "trogqueenDroptableUIDs") { trogqueenDroptableUIDs.Add(bossRushDropTableUID); }
            else if (targetBoss == "mantisDroptableUIDs") { mantisDroptableUIDs.Add(bossRushDropTableUID); }
            else if (targetBoss == "burningmanDroptableUIDs") { burningmanDroptableUIDs.Add(bossRushDropTableUID); }
            else if (targetBoss == "lichesroptableUIDs") { lichesroptableUIDs.Add(bossRushDropTableUID); }
            else if (targetBoss == "immaculateDroptableUIDs") { immaculateDroptableUIDs.Add(bossRushDropTableUID); }
            else if (targetBoss == "giantsDroptableUIDs") { giantsDroptableUIDs.Add(bossRushDropTableUID); }
            else if (targetBoss == "highmonkDroptableUIDs") { highmonkDroptableUIDs.Add(bossRushDropTableUID); }
            else if (targetBoss == "tuanosaurDroptableUIDs") { tuanosaurDroptableUIDs.Add(bossRushDropTableUID); }
            else { }
        }

        public void AddDropTableToAll(string bossRushDropTableUID)
        {
            sharksDroptableUIDs.Add(bossRushDropTableUID);
            calixaDroptableUIDs.Add(bossRushDropTableUID);
            beastDroptableUIDs.Add(bossRushDropTableUID);
            cageDroptableUIDs.Add(bossRushDropTableUID);
            boozuDroptableUIDs.Add(bossRushDropTableUID);
            sublimeDroptableUIDs.Add(bossRushDropTableUID);
            crimsonDroptableUIDs.Add(bossRushDropTableUID);
            gargoyleDroptableUIDs.Add(bossRushDropTableUID);
            grandmotherDroptableUIDs.Add(bossRushDropTableUID);
            torcrabDroptableUIDs.Add(bossRushDropTableUID);
            mantisDroptableUIDs.Add(bossRushDropTableUID);
            squireDroptableUIDs.Add(bossRushDropTableUID);
            trogqueenDroptableUIDs.Add(bossRushDropTableUID);
            burningmanDroptableUIDs.Add(bossRushDropTableUID);
            lichesroptableUIDs.Add(bossRushDropTableUID);
            immaculateDroptableUIDs.Add(bossRushDropTableUID);
            giantsDroptableUIDs.Add(bossRushDropTableUID);
            highmonkDroptableUIDs.Add(bossRushDropTableUID);
            tuanosaurDroptableUIDs.Add(bossRushDropTableUID);
        }
    }
}
