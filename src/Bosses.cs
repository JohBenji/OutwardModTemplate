using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BossRush
{
    public class PublicBossDictionary
    {        
        public Dictionary<string, DataValue> BossDictionary = new Dictionary<string, DataValue>();


        public PublicBossDictionary()

        {
            BossDictionary = new Dictionary<string, DataValue>();
            
            // Chersonese
            BossDictionary.Add("Troglodyte Queen", new DataValue(AreaManager.AreaEnum.ChersoDungeonsBosses, 0));
            BossDictionary.Add("Elite Mantis Shrimp", new DataValue(AreaManager.AreaEnum.ChersoDungeonsBosses, 1));
            BossDictionary.Add("Brand Squire", new DataValue(AreaManager.AreaEnum.ChersoDungeonsBosses, 2));
            

            // Hallowed Marsh
            BossDictionary.Add("Ash Giant Highmonk", new DataValue(AreaManager.AreaEnum.HallowedDungeonsBosses, 0));
            BossDictionary.Add("Elite Ash Giants", new DataValue(AreaManager.AreaEnum.HallowedDungeonsBosses, 1));
            BossDictionary.Add("Elite Alpha Tuanosaur", new DataValue(AreaManager.AreaEnum.HallowedDungeonsBosses, 2));

            // Enmercar Forest
            BossDictionary.Add("Light Mender and Plague Doctor", new DataValue(AreaManager.AreaEnum.EmercarDungeonsBosses, 0));
            BossDictionary.Add("Elite Burning Man", new DataValue(AreaManager.AreaEnum.EmercarDungeonsBosses, 1));
            BossDictionary.Add("Immaculate Dreamer", new DataValue(AreaManager.AreaEnum.EmercarDungeonsBosses, 2));

            // Abrassar
            BossDictionary.Add("Calixa (boss)", new DataValue(AreaManager.AreaEnum.AbrassarDungeonsBosses, 0));
            BossDictionary.Add("Elite Beast Golem", new DataValue(AreaManager.AreaEnum.AbrassarDungeonsBosses, 1));
            BossDictionary.Add("Elite Crescent Sharks", new DataValue(AreaManager.AreaEnum.AbrassarDungeonsBosses, 2));            
            

            // Antique Plateau
            BossDictionary.Add("Immaculate's Bird", new DataValue(AreaManager.AreaEnum.AntiqueFieldDungeonsBosses, 0));
            BossDictionary.Add("Elite Sublime Shell", new DataValue(AreaManager.AreaEnum.AntiqueFieldDungeonsBosses, 1));
            BossDictionary.Add("Elite Boozu", new DataValue(AreaManager.AreaEnum.AntiqueFieldDungeonsBosses, 2));

            // Caldera
            BossDictionary.Add("Grandmother", new DataValue(AreaManager.AreaEnum.CalderaDungeonsBosses, 1));
            BossDictionary.Add("Elite Crimson Avatar", new DataValue(AreaManager.AreaEnum.CalderaDungeonsBosses, 2));
            BossDictionary.Add("Elite Gargoyles", new DataValue(AreaManager.AreaEnum.CalderaDungeonsBosses, 3));
            BossDictionary.Add("Elite Torcrab", new DataValue(AreaManager.AreaEnum.CalderaDungeonsBosses, 4));            
        }
    }

    [System.Serializable]
    public struct DataValue
    {
        public AreaManager.AreaEnum area;
        public int spawnpointindex;

        public DataValue(AreaManager.AreaEnum area, int spawnpointindex)
        {
            this.area = area;
            this.spawnpointindex = spawnpointindex;
        }
    }

}