using System;
using System.Collections.Generic;

namespace BossRush
{
    [Serializable]
    public class DefeatedBossData
    {
        public string CharacterUID;
        public List<string> DefeatedFoes;

        public bool HasDeafeated(string CharacterUID)
        {
            return DefeatedFoes.Contains(CharacterUID);
        }
    }   
}
