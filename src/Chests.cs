using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SideLoader;
using UnityEngine;

namespace BossRush
{
    public class ApplyChests
    {      
        public void ApplyTemplate(string sceneName, Vector3 spawnPosition, Vector3 spawnRotation, List<string> droptableUIDs)
        {
            new SL_ItemContainerSpawn()
            {
                IdentifierName = sceneName,
                ItemID = 1,
                Quantity = 1,
                SceneToSpawnIn = sceneName,
                SpawnPosition = spawnPosition,
                SpawnRotation = spawnRotation,
                ForceNonPickable = true,
                TryLightFueledContainer = true,
                SL_DropTableUIDs = droptableUIDs,
                HoursForDropRegeneration = 0
            }.ApplyTemplate();
        }

        public ApplyChests()
        {
            // COMMENT: I need to move all the chests from Sideloader/ItemSources over to here.
            ApplyTemplate("AbrassarDungeonsBosses",new Vector3(39.7859f,0.2242f,-4.6284f),new Vector3(0f,0f,0f),null); // Instead of null a list of droptable UIDs need to be inserted.
        }
    }
}
