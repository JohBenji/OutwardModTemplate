using SideLoader.SaveData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SideLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using SideLoader.SaveData;
using UnityEngine;

namespace BossRush
{
    /*
     * Comment: There is particular save manager that makes it difficult to run namely the line:
     * dictionary = SLItemSpawnSaveManager.LoadItemSpawnData();
     * The internal keyword here locks us out of debugging this class.
     * */

    public class SL_ItemSpawnClone : SL_ItemSource
    {
        /*
        static SL_ItemSpawnClone()
        {
            SL.OnGameplayResumedAfterLoading += SL_ItemSpawnClone.OnSceneLoaded;
        }
               
        public bool ShouldSave
        {
            get
            {
                return !this.ForceNonPickable || this is SL_ItemContainerSpawn;
            }
        }

        protected override void ApplyActualTemplate()
        {
            base.ApplyActualTemplate();
            bool flag = string.IsNullOrEmpty(this.IdentifierName);
            if (flag)
            {
                SL.LogWarning("Cannot register an SL_ItemSpawn without an InternalName set!");
            }
            else
            {
                bool flag2 = SL_ItemSpawnClone.s_registeredSpawnSources.ContainsKey(this.IdentifierName);
                if (flag2)
                {
                    SL.LogWarning("An SL_ItemSpawn with the UID '" + this.IdentifierName + "' has already been registered!");
                }
                else
                {
                    SL_ItemSpawnClone.s_registeredSpawnSources.Add(this.IdentifierName, this);
                }
            }
        }

        static void OnSceneLoaded()
        {
            bool isNonMasterClientInRoom = PhotonNetwork.isNonMasterClientInRoom;
            if (!isNonMasterClientInRoom)
            {
                SL.Log("Checking SL_ItemSpawns...");
                SL_ItemSpawnClone.s_activeSavableSpawns.Clear();
                Dictionary<string, SLItemSpawnSaveManager.ItemSpawnInfo> dictionary = null;
                bool flag = !SL.WasLastSceneReset;
                if (flag)
                {                    
                    dictionary = SLItemSpawnSaveManager.LoadItemSpawnData();
                }
                foreach (SL_ItemSpawnClone sl_ItemSpawn in SL_ItemSpawnClone.s_registeredSpawnSources.Values)
                {
                    bool flag2 = dictionary != null && dictionary.ContainsKey(sl_ItemSpawn.IdentifierName);
                    if (flag2)
                    {
                        SLItemSpawnSaveManager.ItemSpawnInfo itemSpawnInfo = dictionary[sl_ItemSpawn.IdentifierName];
                        SL_ItemSpawnClone.s_activeSavableSpawns.Add(itemSpawnInfo);
                        Item item = ItemManager.Instance.GetItem(itemSpawnInfo.ItemUID);
                        bool flag3 = item && string.IsNullOrEmpty(item.MostRecentOwnerUID);
                        if (flag3)
                        {
                            SL_ItemSpawnClone.s_registeredSpawnSources[sl_ItemSpawn.IdentifierName].ApplyToItem(item);
                        }
                    }
                    else
                    {
                        bool flag4 = sl_ItemSpawn.SceneToSpawnIn == SceneManagerHelper.ActiveSceneName;
                        if (flag4)
                        {
                            sl_ItemSpawn.GenerateItem();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Called when the item is spawned for the first time after a scene reset, or if not pickable.
        /// </summary>
        protected virtual Item GenerateItem()
        {
            bool isNonMasterClientInRoom = PhotonNetwork.isNonMasterClientInRoom;
            Item result;
            if (isNonMasterClientInRoom)
            {
                result = null;
            }
            else
            {
                bool flag = SL_ItemSpawnClone.s_activeSavableSpawns.Any((SLItemSpawnSaveManager.ItemSpawnInfo it) => it.SpawnIdentifier == this.IdentifierName);
                if (flag)
                {
                    SL.LogWarning("Trying to spawn two SL_ItemSpawns with the same Identifier: " + this.IdentifierName);
                    result = null;
                }
                else
                {
                    Item itemPrefab = ResourcesPrefabManager.Instance.GetItemPrefab(this.ItemID);
                    bool flag2 = !itemPrefab;
                    if (flag2)
                    {
                        SL.LogWarning(string.Format("SL_ItemSpawn: Could not find any item by ID '{0}'!", this.ItemID));
                        result = null;
                    }
                    else
                    {
                        SL.Log("SL_ItemSpawn '" + this.IdentifierName + "' spawning...");
                        Item item = ItemManager.Instance.GenerateItemNetwork(this.ItemID);
                        bool shouldSave = this.ShouldSave;
                        if (shouldSave)
                        {
                            SL_ItemSpawnClone.s_activeSavableSpawns.Add(new SLItemSpawnSaveManager.ItemSpawnInfo
                            {
                                SpawnIdentifier = this.IdentifierName,
                                ItemID = this.ItemID,
                                ItemUID = item.UID
                            });
                        }
                        this.ApplyToItem(item);
                        result = item;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Called for new spawns, or if loading from save and item hasnt been picked up yet.
        /// </summary>
        protected virtual void ApplyToItem(Item item)
        {
            item.ChangeParent(null, this.SpawnPosition, Quaternion.Euler(this.SpawnRotation));
            bool flag = this.Quantity > 1 && item.HasMultipleUses;
            if (flag)
            {
                item.RemainingAmount = this.Quantity;
            }
            item.IsPickable = !this.ForceNonPickable;
            item.ForceNonSavable = !this.ShouldSave;            
        }

        [XmlIgnore]
        internal static readonly Dictionary<string, SL_ItemSpawnClone> s_registeredSpawnSources = new Dictionary<string, SL_ItemSpawnClone>();

        [XmlIgnore]
        internal static readonly HashSet<SLItemSpawnSaveManager.ItemSpawnInfo> s_activeSavableSpawns = new HashSet<SLItemSpawnSaveManager.ItemSpawnInfo>();

        public int ItemID;

        public int Quantity = 1;

        public string SceneToSpawnIn = "";

        public Vector3 SpawnPosition;

        public Vector3 SpawnRotation;

        public bool ForceNonPickable;

        public bool TryLightFueledContainer = true;
        */
    }
}
