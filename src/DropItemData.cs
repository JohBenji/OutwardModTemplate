using System;

// RENAME 'OutwardModTemplate' TO SOMETHING ELSE
namespace BossRush
{
    [Serializable]
    public class DropItemData
    {
        public int ItemID;
        public int MinAmount;
        public int MaxAmount;

        public virtual int GetRandomAmount()
        {
            if (MinAmount == MaxAmount) return MinAmount;
            return UnityEngine.Random.Range(MinAmount, MaxAmount + 1);
        }
    }
   
}