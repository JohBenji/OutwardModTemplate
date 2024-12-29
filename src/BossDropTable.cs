using System;
using System.Collections.Generic;

// RENAME 'OutwardModTemplate' TO SOMETHING ELSE
namespace BossRush
{
    [Serializable]
    public class BossDropTable
    {
        public int MinRolls = 1;
        public int MaxRolls = 1;
        public List<DropItemData> GuaranteedDrops;
        public List<WeightedDropItemData> WeightedDrops;


        public int GetRolls()
        {
            return UnityEngine.Random.Range(MinRolls, MaxRolls);  
        }
    }
   
}