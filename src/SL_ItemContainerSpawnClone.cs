using System;
using System.Collections.Generic;
using UnityEngine;
using SideLoader;

namespace BossRush
{    
    public class SL_ItemContainerSpawnClone : SL_ItemSpawn
    {
        static SL_ItemContainerSpawnClone()
        {
            SL.OnSceneLoaded += SL_ItemContainerSpawnClone.ClearActiveSpawns;
        }
        
        private static void ClearActiveSpawns()
        {
            SL_ItemContainerSpawnClone.activeSpawns.Clear();
        }

        /// <inheritdoc />
        protected override Item GenerateItem()
        {
            return base.GenerateItem();
        }

        /// <inheritdoc />
        protected override void ApplyToItem(Item item)
        {
            base.ApplyToItem(item);
            SelfFilledItemContainer selfFilledItemContainer = item as SelfFilledItemContainer;           
            List<SL_DropTable> list = new List<SL_DropTable>();
            bool flag = this.SL_DropTableUIDs != null;
            if (flag)
            {
                foreach (string key in this.SL_DropTableUIDs)
                {
                    SL_DropTable item2;
                    bool flag2 = SL_DropTable.s_registeredTables.TryGetValue(key, out item2);
                    if (flag2)
                    {
                        list.Add(item2);
                    }
                }
            }
            selfFilledItemContainer.m_dropPrefabs = new Dropable[0];           
            selfFilledItemContainer.m_drops = new List<Dropable>();
            foreach (SL_DropTable sl_DropTable in list)
            {
                Dropable item3 = sl_DropTable.AddAsDropableToGameObject(selfFilledItemContainer.gameObject, selfFilledItemContainer is Gatherable, selfFilledItemContainer.UID + "_" + sl_DropTable.UID, this.HoursForDropRegeneration);                
                selfFilledItemContainer.m_drops.Add(item3);

            }
            foreach (DropTable dropTable in selfFilledItemContainer.GetComponentsInChildren<DropTable>())
            {
                dropTable.LoadSavedInfos();
            }
            RigidbodySuspender component = selfFilledItemContainer.GetComponent<RigidbodySuspender>();
            bool flag3 = component != null;           
            if (flag3)
            {
                UnityEngine.Object.Destroy(component);
                UnityEngine.Object.Destroy(selfFilledItemContainer.GetComponent<Rigidbody>());
            }
            Transform transform = selfFilledItemContainer.transform.Find("Sphere");
            bool flag4 = transform != null;
            if (flag4)
            {
                UnityEngine.Object.Destroy(transform.gameObject);
            }
            SL_ItemContainerSpawnClone.activeSpawns.Add(selfFilledItemContainer, this);
        }

        public List<string> SL_DropTableUIDs = new List<string>();

        public float HoursForDropRegeneration;

        private static readonly Dictionary<SelfFilledItemContainer, SL_ItemContainerSpawnClone> activeSpawns = new Dictionary<SelfFilledItemContainer, SL_ItemContainerSpawnClone>();
    }
}
