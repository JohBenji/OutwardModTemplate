using System;
using System.Collections.Generic;
using System.Xml.Serialization;

// RENAME 'OutwardModTemplate' TO SOMETHING ELSE
namespace BossRush
{
    [Serializable]
    public class BossDropTable
    {
        public int MinRolls = 1;
        public int MaxRolls = 1;
        [XmlArray("GuaranteedDrops")]
        [XmlArrayItem("DropItem")]
        public List<DropItemData> GuaranteedDrops;
        public List<WeightedDropItemData> WeightedDrops;


        public int GetRolls()
        {
            return UnityEngine.Random.Range(MinRolls, MaxRolls);  
        }
    }
   
}