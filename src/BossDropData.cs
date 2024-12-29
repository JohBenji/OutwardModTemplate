using System;
using System.Collections.Generic;

// RENAME 'OutwardModTemplate' TO SOMETHING ELSE
namespace BossRush
{
    [Serializable]
    public class BossDropData
    {
        public string targetCharacterUID;
        public List<BossDropTable> DropTables;

        public void RollAndGrantDrops(Character character)
        {
            if (DropTables == null || character == null)
                return;

            foreach (var table in DropTables)
            {
                // Roll each table the specified number of times
                for (int i = 0; i < table.GetRolls(); i++)
                {
                    // Grant guaranteed drops
                    if (table.GuaranteedDrops != null)
                    {
                        foreach (var drop in table.GuaranteedDrops)
                        {
                            int amount = UnityEngine.Random.Range(drop.MinAmount, drop.MaxAmount + 1);
                            character.Inventory.ReceiveItemReward(drop.ItemID, amount, false);
                        }
                    }
                }
            }
        }
    }
   
}