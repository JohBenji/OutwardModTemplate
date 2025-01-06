using BossRush;
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
        public BossSelectionEnum SpecificChestData;
    }

    [Serializable]
    public enum BossRushDropTableType
    {
        CommonChestLoot,
        SpecificChestLoot,
        BossRushCompletion
    }
    [Serializable]
    public enum BossSelectionEnum
    {
        None,
        Elite_Crescent_Sharks,
        Calixa,
        Elite_Beast_Golem,
        Immaculates_Bird,
        Elite_Boozu,
        Elite_Sublime_Shell,
        Elite_Crimson_Avatar,
        Elite_Gargoyles,
        Grandmother,
        Elite_Torcrab,
        Elite_Mantis_Shrimp,
        Brand_Squire,
        Trolgodyte_Queen,
        Elite_Burning_Man,
        Liches,
        Dreamer_Immaculate,
        Ash_Giants,
        Ash_Giant_Highmonk,
        Elite_Alpha_Tuanosaur
    }
}