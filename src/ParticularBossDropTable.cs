using System;
using System.Collections.Generic;
using System.Xml.Serialization;

// RENAME 'OutwardModTemplate' TO SOMETHING ELSE
namespace BossRush
{
    [Serializable]
    public class ParticularBossDropTable
    {
        public string targetCharacterUID;
        [XmlArray("DropTables")]
        [XmlArrayItem("DropTable")]
        public List<BossDropTable> DropTables;

        /*
        public int GetRolls()
        {
            return UnityEngine.Random.Range(MinRolls, MaxRolls);
        }
        */

    }
}