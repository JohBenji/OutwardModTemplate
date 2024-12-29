using System.Collections.Generic;
using System.Linq;

// RENAME 'OutwardModTemplate' TO SOMETHING ELSE
namespace BossRush
{
    public static class DropCalculator
    {
        public static List<DropItemData> CalculateDrops(BossDropTable table)
        {
            List<DropItemData> results = new List<DropItemData>();

            int rolls = UnityEngine.Random.Range(table.MinRolls, table.MaxRolls + 1);

            // Add guaranteed drops
            if (table.GuaranteedDrops != null)
            {
                results.AddRange(table.GuaranteedDrops);
            }

            for (int i = 0; i < rolls; i++)
            {
                if (table.WeightedDrops != null && table.WeightedDrops.Count > 0)
                {
                    float totalWeight = table.WeightedDrops.Sum(item => item.Weight);
                    float roll = UnityEngine.Random.Range(0f, totalWeight);
                    float currentWeight = 0;

                    foreach (var item in table.WeightedDrops)
                    {
                        currentWeight += item.Weight;
                        if (roll <= currentWeight)
                        {
                            results.Add(item);
                            break;
                        }
                    }
                }
            }

            return results;
        }

        public static void GrantDropsTo(this Character character, List<DropItemData> drops)
        {
            foreach (var d in drops)
            {

            }

        }
    }
   
}