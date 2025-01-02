using SideLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BossRush
{
    public class SL_BossRushDropTable : SL_DropTable
    {
        public BossRushDropTableType TypeOfBossRushDropTable;
        public string OptionalString;
    }

    [Serializable]
    public enum BossRushDropTableType
    {
        CommonChestLoot,
        SpecificChestLoot,
        BossRushCompletion
    }
}