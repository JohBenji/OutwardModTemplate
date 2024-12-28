using NodeCanvas.Tasks.Actions;
using SideLoader.SaveData;
using System.Collections.Generic;
using System.Linq;


namespace BossRush
{
    public class BossRushSaveData : PlayerSaveExtension
    {
        public SerializablePortalData BossRushData = new SerializablePortalData();

        public override void Save(Character character, bool isWorldHost)
        {
            if (BossRushPlugin.Instance != null)
            {
                BossRushData.BossOrder = BossRushPlugin.teleportArray;
                BossRushData.BossIndex = BossRushPlugin.teleportindex;
                BossRushData.Activated = BossRushPlugin.activated;
                BossRushData.RewardReq1= BossRushPlugin.rewardReq1;
                BossRushData.RewardReq2 = BossRushPlugin.rewardReq2;
            }

        }

        public override void ApplyLoadedSave(Character character, bool isWorldHost)
        {
            if (BossRushPlugin.Instance != null)
            {
                BossRushPlugin.teleportArray = BossRushData.BossOrder;
                BossRushPlugin.teleportindex = BossRushData.BossIndex;
                BossRushPlugin.activated = BossRushData.Activated;
                BossRushPlugin.rewardReq1 = BossRushData.RewardReq1;
                BossRushPlugin.rewardReq2 = BossRushData.RewardReq2;
            }
        }
    }

    [System.Serializable]
    public class SerializablePortalData
    {
        public string[] BossOrder;
        public int BossIndex;
        public bool Activated;
        public bool RewardReq1;
        public bool RewardReq2;


        public SerializablePortalData()
        {

        }

        public SerializablePortalData(string[] bossOrder, int bossIndex, bool activated, bool rewardReq1, bool rewardReq2)
        {
            BossOrder = bossOrder;
            BossIndex = bossIndex;
            Activated = activated;
            RewardReq1 = rewardReq1;
            RewardReq2 = rewardReq2;
        }
    }
}
