using HarmonyLib;
using NodeCanvas.Tasks.Actions;
using SideLoader.SaveData;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.TextCore;


namespace BossRush
{
    public class BossRushSaveExtension : PlayerSaveExtension
    {
        public BossRushSaveData BossRushData = new BossRushSaveData();

        public override void Save(Character character, bool isWorldHost)
        {
            if (BossRushPlugin.Instance != null)
            {
                BossRushData.BossOrder = BossRushPlugin.teleportArray;
                BossRushData.BossIndex = BossRushPlugin.teleportindex;
                BossRushData.Activated = BossRushPlugin.activated;
                BossRushData.RewardReq1= BossRushPlugin.rewardReq1;
                BossRushData.RewardReq2 = BossRushPlugin.rewardReq2;

                //update the save data list with all the defeated foes the boss rush manager has registered
                BossRushData.DefeatedBosses = BossRushPlugin.Instance.BossRushManager.GetDefeatedFoesFor(character.UID);
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
                BossRushPlugin.Instance.BossRushManager.ApplySaveData(this);
            }
        }
    }

    [System.Serializable]
    public class BossRushSaveData
    {
        public string playerUID;
        public string[] BossOrder;
        public int BossIndex;
        public bool Activated;

        public List<string> DefeatedBosses;

        public bool RewardReq1;
        public bool RewardReq2;

        public BossRushSaveData()
        {

        }
    }

    [HarmonyPatch(typeof(Character), nameof(Character.Die))]
    public class BossRush_Die_Patch
    {
        static void Prefix(Character __instance, UnityEngine.Vector3 _hitVec, bool _loadedDead = false)
        {
            if (__instance != null && __instance.IsAI)
            {
                if (BossRushPlugin.Instance.BossRushManager != null)
                {
                    Character worldHost = CharacterManager.Instance.GetWorldHostCharacter();

                    if (worldHost != null) 
                    {
                        BossRushPlugin.Instance.BossRushManager.RecordVictory(worldHost.UID, __instance.UID);
                        if (BossRushPlugin.Instance.BossRushManager.HasDropDataFor(__instance.UID))
                        {
                            foreach (var table in BossRushPlugin.Instance.BossRushManager.GetDropDataFor(__instance.UID))
                            {
                                /*
                                table.RollAndGrantDrops(worldHost);
                                */
                            }
                        }
                    }               
                }
            }        
        }
    }
}
