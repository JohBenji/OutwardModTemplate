using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore;

namespace BossRush
{
    public class InteractionFunctionality : InteractionBase        
    {
        private static Character targetCharacter;
        public InteractionFunctionality() 
        {

        }

        public override void Activate(Character _character)
        {
            targetCharacter = _character;
            string currentSceneName = SceneManager.GetActiveScene().name;

            // State before
            BossRushPlugin.Log.LogMessage($"Pre-state:");
            BossRushPlugin.Log.LogMessage($"Activated = {BossRushPlugin.activated}, Teleport index = {BossRushPlugin.teleportindex}");
            if (BossRushPlugin.teleportArray != null)
            {
                if (BossRushPlugin.teleportArray.Length > 0)
                {
                    printStringArray(BossRushPlugin.teleportArray);
                }
            }

            PublicBossDictionary publicBossDictionary = new PublicBossDictionary();
            if (publicBossDictionary != null)
            {
                Dictionary<string, DataValue> dictionary = publicBossDictionary.BossDictionary;
                if (dictionary != null)
                {
                    BossRushPlugin.portalManager = new PortalManager();
                    if (BossRushPlugin.portalManager != null)
                    {
                        if (currentSceneName == "Emercar")
                        {
                            BossRushPlugin.teleportindex = 0;
                            if (BossRushPlugin.activated == false)
                            {
                                RelicItems relics = new RelicItems();
                                foreach (int itemID in relics.relicList)
                                {
                                    if (_character.Inventory.OwnsItem(itemID, 1))
                                    {
                                        _character.Inventory.RemoveItem(itemID, 1);
                                        BossRushPlugin.activated = true;
                                        BossRushPlugin.rewardReq1 = true;
                                        List<string> teleportListOriginal = dictionary.Keys.ToList();
                                        List<string> teleportList = teleportListOriginal;
                                        teleportList = teleportList.OrderBy(x => UnityEngine.Random.value).ToList();
                                        BossRushPlugin.teleportArray = teleportList.ToArray();                                        
                                        BossRushPlugin.lightAltar();
                                        Animator animator = _character.Animator;
                                        animator.Play("PaulPickUpGround");                                        
                                        PlaySound(GlobalAudioManager.Sounds.SFX_StartFire);
                                        break;
                                    }
                                }
                                if (BossRushPlugin.activated == false)
                                {
                                    _character.CharacterUI.ShowInfoNotification("Bringing an Unknown Arena Craftable might do something...");
                                }
                            }
                            else if (BossRushPlugin.activated == true) 
                            {                                
                                Animator animator = _character.Animator;                                
                                animator.Play("Death Indirect");
                                BossRushPlugin.delayTeleport(_character);                                                                
                            }                            
                        }
                        else
                        {       
                            BossRushPlugin.teleportindex = BossRushPlugin.teleportindex + 1;
                            if (BossRushPlugin.teleportindex >= 18)
                            {
                                BossRushPlugin.rewardReq2 = true;
                                BossRushPlugin.portalManager.StartAreaSwitchAndSetPosition(_character, AreaManager.AreaEnum.Emercar, 1);
                            }
                            else
                            {
                                // Iterating
                                
                                string key = BossRushPlugin.teleportArray[BossRushPlugin.teleportindex];
                                DataValue dataValue = dictionary[key];
                                AreaManager.AreaEnum area = dataValue.area;
                                int spawnPointIndex = dataValue.spawnpointindex;
                                BossRushPlugin.portalManager.StartAreaSwitchAndSetPosition(_character, area, spawnPointIndex);                                
                            }
                            BossRushPlugin.displayProgress = true;
                        }                        
                    }                    
                }
                else
                {
                    BossRushPlugin.Log.LogMessage("Dictionary is null");
                }
            }
            else
            {
                BossRushPlugin.Log.LogMessage("EntityManager is null!");
            }

            // State after
            BossRushPlugin.Log.LogMessage($"Post-state:");
            BossRushPlugin.Log.LogMessage($"Activated = {BossRushPlugin.activated}, Teleport index = {BossRushPlugin.teleportindex}");
            if (BossRushPlugin.teleportArray != null)
            {
                if (BossRushPlugin.teleportArray.Length > 0)
                {
                    printStringArray(BossRushPlugin.teleportArray);
                }
            }
        }


        public void printStringArray(string[] array)
        {
            foreach (string str in array)
            {
                BossRushPlugin.Log.LogMessage(str);
            }
        }
    }
}
