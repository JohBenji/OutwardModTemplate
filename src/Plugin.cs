using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using static NodeCanvas.Tasks.Actions.DebugLogText;
using SideLoader;
using System.Resources;
using NodeCanvas.Tasks.Actions;
using UnityEngine.Assertions;
using System.Drawing.Printing;
using MapMagic;
using static AreaManager;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.TextCore;
using static MapMagic.Erosion;
using static UnityEngine.UIElements.UIRAtlasAllocator;
using System.Diagnostics;

// RENAME 'OutwardModTemplate' TO SOMETHING ELSE
namespace BossRush
{
    [BepInPlugin(GUID, NAME, VERSION)]
    public class BossRushPlugin : BaseUnityPlugin
    {
        // Choose a GUID for your project. Change "myname" and "mymod".
        public const string GUID = "johbenji.bossrush";
        // Choose a NAME for your project, generally the same as your Assembly Name.
        public const string NAME = "Boss Rush";
        // Increment the VERSION when you release a new version of your mod.
        public const string VERSION = "1.0.0";

        public static int teleportindex = 0;
        public static bool activated = false;
        public static string[] teleportArray;
        public static bool displayProgress = false;
        public static bool rewardReq1 = false;
        public static bool rewardReq2 = false;
        public static PortalManager portalManager;
        public static Character portalCharacter;
        //public static List<BossRushRewardData> bossRushRewardDatas;

        // For accessing your BepInEx Logger from outside of this class (eg Plugin.Log.LogMessage("");)
        internal static ManualLogSource Log;

        // If you need settings, define them like so:
        public static ConfigEntry<bool> ExampleConfig;


        //your plugin is already a singleton so you can put the manager/deserializer here.
        public BossRushManager BossRushManager { get; private set; }

        public static BossRushPlugin Instance { get; private set; }


        // Awake is called when your plugin is created. Use this to set up your mod.
        internal void Awake()
        {
            Instance = this;
            Log = this.Logger;

            //create a new instance of the class calls the constructor which calls FindXMLDefinitions
            BossRushManager = new BossRushManager();
            portalManager = new PortalManager();

            ConfigElements.Init(base.Config);
            SceneManager.sceneLoaded += SceneManager_sceneLoaded;
            new Harmony(GUID).PatchAll();       

            /*
            Plan:
            - Fire particle effect when activated
            */
        }


        [HarmonyPatch(typeof(EnvironmentSave), nameof(EnvironmentSave.ApplyData))]
        class resetParameters
        {
            static void Postfix(ref bool __result, string ___AreaName)
            {
                if (__result) 
                {
                    if (___AreaName == "Emercar")
                    {
                        BossRushPlugin.activated = false;
                        BossRushPlugin.rewardReq1 = false;
                        BossRushPlugin.rewardReq2 = false;
                    }
                }                
            }
        }


        internal void SceneManager_sceneLoaded(Scene Scene, LoadSceneMode LoadMode)
        {            
            if (Scene.name == "Emercar")
            {
                GameObject sceneholder = new GameObject();
                sceneholder.name = "JOHBENJI_DLC";
                BossRushPlugin.Log.LogMessage("Scene Found");
                // Find tree gameobject
                sceneholder.transform.position = new Vector3((float)855.2936, (float)-11.3909, (float)1589.459); ;
                if (GameObject.Find("CentralTreeBossRush") == null)
                {
                    GameObject StolenTree = GameObject.Find("Environment/Assets/Foliage/Tree/mdl_env_treeKapokBergB (29)");
                    if (StolenTree != null)
                    {
                        GameObject newTree = GameObject.Instantiate(StolenTree);
                        newTree.transform.parent = sceneholder.transform;
                        newTree.name = "CentralTreeBossRush";
                        Vector3 newTreeLocPos = new Vector3((float)0, (float)0, (float)0);
                        Vector3 newTreeSca = new Vector3((float)0.1, (float)0.1, (float)0.1);
                        Transform newTreeTransform = newTree.transform;
                        newTreeTransform.localPosition = newTreeLocPos;
                        newTreeTransform.localScale = newTreeSca;
                        GameObject tree_kapokBergB = FindChild(newTree, "tree_kapokBergB");

                        // Remove the gathering component
                        CentralGatherableAccessPoint gatherComp = newTree.GetComponent<CentralGatherableAccessPoint>();
                        if (gatherComp != null) { Destroy(gatherComp); }

                        // Fix the tree
                        fixTreeColorVisuals(tree_kapokBergB);
                        fixTreeVFX(newTree);

                        // Add the altar
                        addAltar(sceneholder);
                    }
                }
                else
                {
                    BossRushPlugin.Log.LogMessage("Tree not found.");
                }
            }
            else if (Scene.name == "ChersoneseDungeonsBosses" && BossRushPlugin.activated)
            {
                GameObject trogQueenTeleporter = GameObject.Find("Environment/Spawns/ZSpawn1/AreaSwitchQueen/Trigger");
                if (trogQueenTeleporter != null) { altarFunctionality(trogQueenTeleporter, new Vector3(0f, 1.25f, -3.1167f), new Vector3(1.445f, 1.645f, 2.3633f), new Vector3(2.89f, 3.29f, 4.7266f)); }
                else { BossRushPlugin.Log.LogMessage($"Teleporter not found"); }
                GameObject ShrimpTeleporter = GameObject.Find("Environment/Spawns/ZSpawn2/AreaSwitchShrimp/Trigger");
                if (ShrimpTeleporter != null) { altarFunctionality(ShrimpTeleporter, new Vector3(0f, 1.25f, -3.58f), new Vector3(1.445f, 1.645f, 1.9f), new Vector3(2.89f, 3.29f, 3.8f)); }
                else { BossRushPlugin.Log.LogMessage($"Teleporter not found"); }
                GameObject SquireTeleporter = GameObject.Find("Environment/Spawns/ZSpawn3/AreaSwitchSquire/Trigger");
                if (SquireTeleporter != null) { altarFunctionality(SquireTeleporter, new Vector3(0f, 1.25f, -2.1925f), new Vector3(1.445f, 1.645f, 3.2875f), new Vector3(2.89f, 3.29f, 6.575f)); }
                else { BossRushPlugin.Log.LogMessage($"Teleporter not found"); }
            }
            else if (Scene.name == "AbrassarDungeonsBosses" && BossRushPlugin.activated)
            {
                GameObject calixaTeleporter = GameObject.Find("Environment/Spawns/ZSpawn1/AreaSwitchCalixa/Trigger");
                if (calixaTeleporter != null) { altarFunctionality(calixaTeleporter, new Vector3(0f, 1.25f, -3.1167f), new Vector3(1.445f, 1.645f, 2.3633f), new Vector3(2.89f, 3.29f, 4.7266f)); }
                else { BossRushPlugin.Log.LogMessage($"Teleporter not found"); }
                GameObject golemTeleporter = GameObject.Find("Environment/Spawns/ZSpawn2/AreaSwitchGolem/Trigger");
                if (golemTeleporter != null) { altarFunctionality(golemTeleporter, new Vector3(0f, 1.25f, -3.58f), new Vector3(1.445f, 1.645f, 1.9f), new Vector3(2.89f, 3.29f, 3.8f)); }
                else { BossRushPlugin.Log.LogMessage($"Teleporter not found"); }
                GameObject sharkTeleporter = GameObject.Find("Environment/Spawns/ZSpawn3/AreaSwitchShark/Trigger");
                if (sharkTeleporter != null) { altarFunctionality(sharkTeleporter, new Vector3(0f, 1.25f, -3.58f), new Vector3(1.445f, 1.645f, 1.9f), new Vector3(2.89f, 3.29f, 3.8f)); }
                else { BossRushPlugin.Log.LogMessage($"Teleporter not found"); }
            }
            else if (Scene.name == "AntiqueFieldDungeonsBosses" && BossRushPlugin.activated)
            {
                GameObject birdTeleporter = GameObject.Find("Environment/Spawns/ZSpawn1/AreaSwitchBird/Trigger");
                if (birdTeleporter != null) { altarFunctionality(birdTeleporter, new Vector3(-0.0687f, 1.25f, -1.3934f), new Vector3(1.1325f, 1.645f, 1.0054f), new Vector3(2.265f, 3.29f, 2.0109f)); }
                else { BossRushPlugin.Log.LogMessage($"Teleporter not found"); }
                GameObject sublimeTeleporter = GameObject.Find("Environment/Spawns/ZSpawn2/AreaSwitchOpulence/Trigger");
                if (sublimeTeleporter != null) { altarFunctionality(sublimeTeleporter, new Vector3(0f, 1.25f, -4.23f), new Vector3(1.445f, 1.645f, 1.25f), new Vector3(2.89f, 3.29f, 2.5f)); }
                else { BossRushPlugin.Log.LogMessage($"Teleporter not found"); }
                GameObject boozuTeleporter = GameObject.Find("Environment/Spawns/ZSpawn3/AreaSwitchBoozu/Trigger");
                if (boozuTeleporter != null) { altarFunctionality(boozuTeleporter, new Vector3(0.0824f, 1.25f, -3.2298f), new Vector3(1.5274f, 1.645f, 1.7033f), new Vector3(3.0547f, 3.29f, 3.4066f)); }
                else { BossRushPlugin.Log.LogMessage($"Teleporter not found"); }
            }
            else if (Scene.name == "EmercarDungeonsBosses" && BossRushPlugin.activated)
            {
                GameObject lichTeleporter = GameObject.Find("Environment/Spawns/ZSpawn1/AreaSwitchLiches/Trigger");
                if (lichTeleporter != null) { altarFunctionality(lichTeleporter, new Vector3(0f, 1.25f, -3.58f), new Vector3(1.445f, 1.645f, 1.9f), new Vector3(2.89f, 3.29f, 3.8f)); }
                else { BossRushPlugin.Log.LogMessage($"Teleporter not found"); }
                GameObject burningTeleporter = GameObject.Find("Environment/Spawns/ZSpawn2/AreaSwitchBurnBob/Trigger");
                if (burningTeleporter != null) { altarFunctionality(burningTeleporter, new Vector3(0f, 1.25f, -3.58f), new Vector3(1.445f, 1.645f, 1.9f), new Vector3(2.89f, 3.29f, 3.8f)); }
                else { BossRushPlugin.Log.LogMessage($"Teleporter not found"); }
                GameObject immaculateTeleporter = GameObject.Find("Environment/Spawns/ZSpawn3/AreaSwitchImmaculate/Trigger");
                if (immaculateTeleporter != null) { altarFunctionality(immaculateTeleporter, new Vector3(0, 1.25f, -3.58f), new Vector3(1.445f, 1.645f, 1.9f), new Vector3(2.89f, 3.29f, 3.8f)); }
                else { BossRushPlugin.Log.LogMessage($"Teleporter not found"); }
            }
            else if (Scene.name == "HallowedDungeonsBosses" && BossRushPlugin.activated)
            {
                GameObject priestTeleporter = GameObject.Find("Environment/Spawns/ZSpawn1/AreaSwitchPriest/Trigger");
                if (priestTeleporter != null) { altarFunctionality(priestTeleporter, new Vector3(0f, 1.25f, -3.58f), new Vector3(1.445f, 1.645f, 1.9f), new Vector3(2.89f, 3.29f, 3.8f)); }
                else { BossRushPlugin.Log.LogMessage($"Teleporter not found"); }
                GameObject giantTeleporter = GameObject.Find("Environment/Spawns/ZSpawn2/AreaSwitchGiant/Trigger");
                if (giantTeleporter != null) { altarFunctionality(giantTeleporter, new Vector3(0f, 1.25f, -3.58f), new Vector3(1.445f, 1.645f, 1.9f), new Vector3(2.89f, 3.29f, 3.8f)); }
                else { BossRushPlugin.Log.LogMessage($"Teleporter not found"); }
                GameObject tuanoTeleporter = GameObject.Find("Environment/Spawns/ZSpawn3/AreaSwitchTuano/Trigger");
                if (tuanoTeleporter != null) { altarFunctionality(tuanoTeleporter, new Vector3(0, 1.25f, -3.58f), new Vector3(1.445f, 1.645f, 1.9f), new Vector3(2.89f, 3.29f, 3.8f)); }
                else { BossRushPlugin.Log.LogMessage($"Teleporter not found"); }
            }
            else if (Scene.name == "CalderaDungeonsBosses" && BossRushPlugin.activated)
            {
                GameObject grandmotherTeleporter = GameObject.Find("Environment/AreaSwitches/AreaSwitch_GrandMother/Trigger");
                if (grandmotherTeleporter != null) { altarFunctionality(grandmotherTeleporter, new Vector3(-0.1121f, 0.3876f, 0.3471f), new Vector3(1.575f, 0.8876f, 0.847f), new Vector3(3.1499f, 1.7752f, 1.6941f)); }
                else { BossRushPlugin.Log.LogMessage($"Teleporter not found"); }
                GameObject torcrabTeleporter = GameObject.Find("Environment/AreaSwitches/AreaSwitch_TorcrabFireIce/Trigger");
                if (torcrabTeleporter != null) { altarFunctionality(torcrabTeleporter, new Vector3(0.184f, 0.5281f, -1.5924f), new Vector3(1.554f, 1.0281f, 3.2116f), new Vector3(3.1079f, 2.0563f, 6.4233f)); }
                else { BossRushPlugin.Log.LogMessage($"Teleporter not found"); }
                GameObject gargoyleTeleporter = GameObject.Find("Interactions/Caldera_Bosses/InteractionGargoylesBosses/prf_env_gargoyleFake/"); //WORKAROUND WORKS
                if (gargoyleTeleporter != null) { altarFunctionality(gargoyleTeleporter, new Vector3(-0.0546f, 0.1773f, -0.4812f), new Vector3(1.645f, 0.7258f, 1.3162f), new Vector3(3.2899f, 1.4516f, 2.6323f)); }
                else { BossRushPlugin.Log.LogMessage($"Teleporter not found"); }
                GameObject gargoyleDenyTeleporter = GameObject.Find("Environment/AreaSwitches/AreaSwitch_Gargoyles/Trigger");  //WORKAROUND WORKS
                if (gargoyleDenyTeleporter != null) { altarFunctionalityDeny(gargoyleDenyTeleporter); }
                else { BossRushPlugin.Log.LogMessage($"Teleporter not found"); }
                GameObject crimsonTeleporter = GameObject.Find("Environment/AreaSwitches/AreaSwitch_CrimsoneElite/Trigger");
                if (crimsonTeleporter != null) { altarFunctionality(crimsonTeleporter, new Vector3(0.1079f, -0.7207f, 0.2016f), new Vector3(1.0021f, 1.2207f, 0.7016f), new Vector3(2.0042f, 2.4414f, 1.4032f)); }
                else { BossRushPlugin.Log.LogMessage($"Teleporter not found"); }
            }

            // Display progress
            if (BossRushPlugin.activated && BossRushPlugin.displayProgress)
            {
                if (Scene.name == "Emercar" || Scene.name == "ChersoneseDungeonsBosses" ||  Scene.name == "AbrassarDungeonsBosses" || Scene.name == "AntiqueFieldDungeonsBosses" || Scene.name == "EmercarDungeonsBosses" || Scene.name == "HallowedDungeonsBosses" || Scene.name == "CalderaDungeonsBosses")
                {
                    BossRushPlugin.Instance.StartCoroutine(DelayedProgressDisplay());
                    BossRushPlugin.displayProgress = false;                    
                }
            }

            // Give rewards
            if (BossRushPlugin.rewardReq1 && BossRushPlugin.rewardReq2)
            {
                if (Scene.name == "Emercar")
                {
                    BossRushPlugin.Instance.StartCoroutine(GiveBossRushRewardsHub());
                    BossRushPlugin.rewardReq1 = false;
                    BossRushPlugin.rewardReq2 = false;
                }
            }
        }

        private IEnumerator DelayedProgressDisplay()
        {
            yield return new WaitForSeconds(5f);

            if (ConfigElements.includeProgressMessage.Value) 
            {
                int progress = Mathf.RoundToInt(Mathf.Clamp(100 * BossRushPlugin.teleportindex / 18, 0, 100));
                CharacterManager characterManager = CharacterManager.Instance;
                for (int j = 0; j < characterManager.m_playerCharacters.Count; j++)
                {
                    Character character = characterManager.m_characters[characterManager.m_playerCharacters.Values[j]];
                    if (character != null)
                    {
                        character.CharacterUI.NotificationPanel.ShowNotification($"Boss Rush Progress: {progress}%");
                    }
                }
            }                      
        }

        private IEnumerator GiveBossRushRewardsHub()
        {
            yield return new WaitForSeconds(5f);

            CharacterManager characterManager = CharacterManager.Instance;
            for (int j = 0; j < characterManager.m_playerCharacters.Count; j++)
            {
                Character character = characterManager.m_characters[characterManager.m_playerCharacters.Values[j]];
                if (character != null)
                {                    
                    giveReward(character);
                }
            }
        }

        private static IEnumerator DelayTeleport()
        {
            yield return new WaitForSeconds(1.3f);

            // Do teleport
            PublicBossDictionary publicBossDictionary = new PublicBossDictionary();
            if (publicBossDictionary != null) 
            {
                Dictionary<string, DataValue> dictionary = publicBossDictionary.BossDictionary;
                string key = BossRushPlugin.teleportArray[BossRushPlugin.teleportindex];
                DataValue dataValue = dictionary[key];
                AreaManager.AreaEnum area = dataValue.area;
                int spawnPointIndex = dataValue.spawnpointindex;
                if (BossRushPlugin.portalCharacter != null) 
                {
                    Character character = BossRushPlugin.portalCharacter;
                    if (character != null) 
                    {
                        BossRushPlugin.portalManager.StartAreaSwitchAndSetPosition(character, area, spawnPointIndex);
                    }
                }                
                BossRushPlugin.displayProgress = true;
            }            
        }

        public void giveReward(Character character)
        {
            // Here should the reward be given upon completing the boss rush! [THIS IS NOT FOR THE CHESTS]
        }


        public static void delayTeleport(Character character) 
        {
            BossRushPlugin.portalCharacter = character;
            if (character != null && BossRushPlugin.portalCharacter != null)
            {
                if (portalManager != null)
                {
                    BossRushPlugin.Instance.StartCoroutine(DelayTeleport());
                }
            }
        }

        static GameObject FindChild(GameObject parent, string name)
        {
            if (parent != null)
            {
                foreach (Transform child in parent.transform)
                {
                    if (child.name == name)
                    {
                        return child.gameObject;
                    }
                }
                BossRushPlugin.Log.LogMessage($"Child with name {name} not found under {parent.name}");
            }
            else
            {
                BossRushPlugin.Log.LogMessage($"Parent is null");
            }
            return null;

        }

        static void PrintAllChildren(GameObject parent)
        {
            if (parent != null)
            {
                foreach (Transform child in parent.transform)
                {
                    BossRushPlugin.Log.LogMessage($"Child={child.name}, child of parent {parent.name}, child is null = {child == null} ");
                }
                if (parent.transform.childCount == 0)
                {
                    BossRushPlugin.Log.LogMessage($"{parent.name} has 0 children");
                }
            }
            else
            {
                BossRushPlugin.Log.LogMessage($"Parent is null");
            }
        }

        static void PrintAllComponents(GameObject gameobject)
        {
            {
                if (gameobject == null)
                {
                    BossRushPlugin.Log.LogMessage($"GameObject is null. Please provide a valid GameObject.");
                    return;
                }

                Component[] components = gameobject.GetComponents<Component>();
                if (components.Length == 0)
                {
                    BossRushPlugin.Log.LogMessage($"No components attached to the GameObject.");
                    return;
                }

                BossRushPlugin.Log.LogMessage($"Components attached to GameObject '{gameobject.name}':");
                foreach (Component component in components)
                {
                    BossRushPlugin.Log.LogMessage(component.GetType().Name);
                }
            }
        }


        static void fixTreeColorVisuals(GameObject treeParent)
        {
            GameObject tree_kapokBergB_LOD0 = FindChild(treeParent, "tree_kapokBergB_LOD0");
            GameObject tree_kapokBergB_LOD1 = FindChild(treeParent, "tree_kapokBergB_LOD1");
            GameObject tree_kapokBergB_LOD2 = FindChild(treeParent, "tree_kapokBergB_LOD2");
            List<GameObject> treeLODs = new List<GameObject>();
            treeLODs.Add(tree_kapokBergB_LOD0);
            treeLODs.Add(tree_kapokBergB_LOD1);
            treeLODs.Add(tree_kapokBergB_LOD2);
            foreach (GameObject tree in treeLODs)
            {
                if (tree != null)
                {
                    MeshRenderer meshRenderer = tree.GetComponent<MeshRenderer>();
                    if (meshRenderer != null)
                    {
                        foreach (Material mat in meshRenderer.materials)
                        {
                            if (mat.name == "KapokLeavesSummer (Instance)")
                            {
                                mat.color = new Color(0.5f, 0.5f, 1f, 1f);
                            }
                            else if (mat.name == "KapokBark (Instance)")
                            {
                                mat.color = new Color(0.75f, 0.75f, 1f, 1f);
                            }
                            else
                            {
                                BossRushPlugin.Log.LogMessage("Weird material found which is incompatible");
                            }
                        }
                    }
                }
            }
            GameObject tree_kapokBergBBillboard = FindChild(treeParent, "tree_kapokBergB_Billboard");
            if (tree_kapokBergBBillboard != null)
            {
                BillboardRenderer billboardRenderer = tree_kapokBergBBillboard.GetComponent<BillboardRenderer>();
                if(billboardRenderer != null)
                {
                    BillboardAsset billboard = billboardRenderer.billboard;
                    if (billboard != null)
                    {
                        billboard.material.color = new Color(0.5f,0.5f, 1f, 1f);
                    }
                }
            }
        }
        
        static void fixTreeVFX(GameObject treeParent)
        {
            GameObject stolenVFXBase = GameObject.Find("Environment/Assets/FX/Prefab_fx_fire1x4/FireClose");
            if (stolenVFXBase != null) 
            {
                GameObject newVFXBase = GameObject.Instantiate(stolenVFXBase);
                Vector3 newVFXBaseLocPos = new Vector3((float)0, (float)0, (float)0);
                newVFXBase.transform.parent = treeParent.transform;
                newVFXBase.transform.localPosition = newVFXBaseLocPos;                
                ParticleSystemRenderer fireParticleSystemRenderer = newVFXBase.GetComponent<ParticleSystemRenderer>();
                ParticleSystem fireParticleSystem = newVFXBase.GetComponent<ParticleSystem>();
                if (fireParticleSystemRenderer != null && fireParticleSystem != null)
                {
                    Destroy(fireParticleSystemRenderer);
                    Destroy(fireParticleSystem);
                }
                GameObject smokeObject = FindChild(newVFXBase, "SmokeVolumetric");
                if (smokeObject != null)
                {
                    ParticleSystem smokeEffect = smokeObject.GetComponent<ParticleSystem>();
                    if (smokeEffect != null)
                    {
                        smokeEffect.playbackSpeed = 0.03f;
                        smokeEffect.startColor = new Color(0.4608f, 0.4558f, 10f, 0.2f);
                    }
                }
                GameObject embersObject = FindChild(newVFXBase, "Embers");
                if (embersObject != null)
                {
                    embersObject.transform.localScale = new Vector3((float)0.1, (float)0.1, (float)0.1);
                    embersObject.transform.localPosition = new Vector3((float)0, (float)0, (float)0);
                    ParticleSystem embersEffect = embersObject.GetComponent<ParticleSystem>();
                    if (embersEffect != null)
                    {
                        embersEffect.startColor = new Color(0.01f, 0.01f, 10f, 1f);
                        embersEffect.playbackSpeed = 0.3f;
                    }
                }
            }
        }

        static void addAltar(GameObject parent)
        {
            // Adding visuals for the well and coal
            Mesh mesh = null;
            Mesh mesh2 = null;
            Material commonMat = null;
            GameObject templateItem = null;
            GameObject altarBase = GameObject.Find("Environment/Assets/Foliage/Tree/mdl_env_natureTrunkCutA (1)");
            if (altarBase != null)
            {
                // Add container
                GameObject container = GameObject.Instantiate(altarBase);
                container.name = "altar";
                container.transform.parent = parent.transform;
                container.transform.localPosition = new Vector3((float)-2, (float)-0.5345, (float)-0.6);
                container.transform.localScale = new Vector3((float)0.5, (float)0.5, (float)0.5);
                LODGroup lODGroup = container.GetComponent<LODGroup>();
                AdvancedMover advancedMover1 = container.GetComponent<AdvancedMover>();
                if (lODGroup != null && advancedMover1 != null)
                {
                    Destroy(lODGroup);
                    Destroy(advancedMover1);

                    // Storing template item for later cloning
                    templateItem = GameObject.Instantiate(container);
                    templateItem.name = "templateitem";
                    templateItem.transform.parent = parent.transform;
                    templateItem.transform.localPosition = new Vector3((float)-2, (float)-5.345, (float)-0.6);
                    templateItem.transform.localScale = new Vector3((float)1, (float)1, (float)1);

                    // Getting the mesh
                    Item itemMeshHolder = ResourcesPrefabManager.Instance.GetItemPrefab(-65007);
                    GameObject gameobjectItemVisual = itemMeshHolder.GetItemVisual(itemMeshHolder.HasSpecialVisualPrefab).gameObject;
                    if (gameobjectItemVisual != null)
                    {
                        GameObject firepitclone = FindChild(gameobjectItemVisual, "firepit(Clone)");
                        if (firepitclone != null)
                        {
                            MeshFilter targetMeshComp = firepitclone.GetComponent<MeshFilter>();
                            if (targetMeshComp != null)
                            {
                                mesh = targetMeshComp.mesh;
                            }
                        }                        
                    }
                    
                    MeshFilter containerMeshComp = container.GetComponent<MeshFilter>();
                    MeshRenderer containerRendererComp = container.GetComponent<MeshRenderer>();
                    if (containerMeshComp != null && containerRendererComp != null && mesh != null)
                    {
                        containerMeshComp.mesh = mesh;
                        GameObject firePitReference = GameObject.Find("Environment/Assets/Structures/mdl_env_buildingRuinsFirepitA/mdl_env_buildingRuinsFirepitA_LOD0");
                        if (firePitReference != null)
                        {

                            MeshRenderer firePitReferenceRendererComp = firePitReference.GetComponent<MeshRenderer>();
                            if (firePitReferenceRendererComp != null)
                            {
                                Material[] referenceMats = firePitReferenceRendererComp.materials;
                                Material[] containerMats = containerRendererComp.materials;

                                // Create lists from the arrays for easier manipulation
                                var containerList = new List<Material>(containerMats);
                                //Destroy(containerList[2]);

                                // Iterate through the materials to move
                                for(int i = 0 ; i < referenceMats.Length; i++)
                                {
                                    Material mat1 = referenceMats[i];
                                    if (mat1.name == "mat_env_rockCharcoal (Instance)")
                                    {
                                        for (int j = 0; j < containerMats.Length; j++)
                                        {
                                            Material mat2 = containerMats[j];
                                            
                                            if (mat2.name == "mat_env_buildingLogWoodSculptVertex (Instance)")
                                            {                                                
                                                containerList[j] = mat1;
                                            }                                            
                                            else if (mat2.name == "ConiferCierzoNeedlesSeason (Instance)")
                                            {                                         
                                                mat2.color = new Color(0,0,0,0);
                                                containerList[j] = mat2;
                                            }
                                        }
                                    }
                                    else if (mat1.name == "mat_env_ruinsStoneUnevenRockyGrassSculptStone (Instance)")
                                    {
                                        for (int j = 0; j < containerMats.Length; j++)
                                        {
                                            Material mat2 = containerMats[j];
                                            
                                            if (mat2.name == "ConiferCierzoBark (Instance)")
                                            {                                                
                                                containerList[j] = mat1;
                                            }
                                        }
                                    }
                                }
                                Material[] replacementMats = containerList.ToArray();
                                containerRendererComp.materials = replacementMats;
                            }
                        }
                    }
                    // Fixes the collider size
                    MeshCollider originalMeshCollider = container.GetComponent<MeshCollider>();
                    if (originalMeshCollider != null)
                    {
                        Destroy(originalMeshCollider);
                        BoxCollider boxCollider = container.AddComponent<BoxCollider>();
                        boxCollider.center = new Vector3((float)0.0615, (float)1.4725, (float)-0.0576);
                        boxCollider.extents = new Vector3((float)1.5, (float)1.5215, (float)1.5);
                        boxCollider.size = new Vector3((float)2, (float)2.0429, (float)2);
                    }
                }

                // Add pot ontop
                if (container != null && templateItem != null)
                {
                    GameObject pot = GameObject.Instantiate(templateItem);
                    pot.name = "pot";
                    pot.transform.parent = container.transform;
                    pot.transform.localPosition = new Vector3((float)0, (float)-0.6, (float)-0.04);   
                    pot.transform.eulerAngles = new Vector3((float)0, (float)28, (float)0);
                    MeshFilter potMeshComp = pot.GetComponent<MeshFilter>();
                    MeshRenderer potRendererComp = pot.GetComponent<MeshRenderer>();

                    // Getting the mesh from the template item
                    Item itempotMeshHolder = ResourcesPrefabManager.Instance.GetItemPrefab(-65008);
                    GameObject gameobjectpotItemVisual = itempotMeshHolder.GetItemVisual(itempotMeshHolder.HasSpecialVisualPrefab).gameObject;
                    if (gameobjectpotItemVisual != null)
                    {
                        GameObject potclone1 = FindChild(gameobjectpotItemVisual, "pot(Clone)");
                        if (potclone1 != null)
                        {
                            MeshFilter targetpotMeshComp = potclone1.GetComponent<MeshFilter>();
                            if (targetpotMeshComp != null)
                            {
                                mesh2 = targetpotMeshComp.mesh;
                            }
                        }
                    }

                    // Getting the material from the cooking pot
                    Item itemCookingPotMeshHolder = ResourcesPrefabManager.Instance.GetItemPrefab(5010100);
                    GameObject gameobjectCookingPotItemVisual = itemCookingPotMeshHolder.GetItemVisual(itemCookingPotMeshHolder.HasSpecialVisualPrefab).gameObject;
                    if (gameobjectCookingPotItemVisual != null)
                    {                       
                        GameObject cookingPotclone2 = FindChild(gameobjectCookingPotItemVisual, "mdl_env_propPotLargeA");
                        
                        if (cookingPotclone2 != null)
                        {
                            MeshRenderer targetCookingPotRendererComp = cookingPotclone2.GetComponent<MeshRenderer>();
                            if (targetCookingPotRendererComp != null)
                            {
                                commonMat = targetCookingPotRendererComp.material;
                            }
                        }
                    }


                    // Assigning the mesh and material to the target
                    if (pot != null)
                    { 
                        Destroy(pot.GetComponent<MeshCollider>());
                        Destroy(pot.GetComponent<AdvancedMover>());
                        Destroy(pot.GetComponent<LODGroup>());
                        if (potRendererComp != null && potMeshComp != null && mesh2 != null && commonMat != null)
                        {
                            potMeshComp.mesh = mesh2;
                            List<Material> materialscommonmat = new List<Material>();
                            materialscommonmat.Add(commonMat);
                            materialscommonmat.Add(commonMat);
                            materialscommonmat.Add(commonMat);
                            potRendererComp.materials = materialscommonmat.ToArray();                            
                        }              
                    }

                    // Adding functionality to the altar
                    if (pot != null)
                    {
                        altarFunctionality(pot, new Vector3(0.0615f, 1.4725f, -0.0576f), new Vector3(1.5f, 1.5215f, 1.5f), new Vector3(4f, 4.0429f, 4));
                    }

                    // Adding VFX for the pot
                    if (pot != null)
                    {
                        lightAltar();
                    }                    
                }                
            }
        }


        public static void altarFunctionality(GameObject gameObject, Vector3 centerIn, Vector3 extentsIn, Vector3 sizeIn)
        {
            if (gameObject != null)
            {                
                InteractionTriggerBase triggerBase = gameObject.GetComponent<InteractionTriggerBase>();
                InteractionFunctionality functionality = gameObject.GetComponent<InteractionFunctionality>();
                InteractionActivator activator = gameObject.GetComponent<InteractionActivator>();
               
                if (triggerBase != null) 
                {
                    Destroy(triggerBase);
                }
                if (functionality != null)
                {
                    Destroy(triggerBase);
                }
                if (activator != null)
                {
                    Destroy(activator);
                }

                BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
                boxCollider.center = centerIn;
                boxCollider.extents = extentsIn;

                float scalar = 1.1f;
                Vector3 sizeInNew = scalar * sizeIn;

                boxCollider.size = sizeInNew;
                boxCollider.isTrigger = true;                

                activator = gameObject.AddComponent<InteractionActivator>();
                triggerBase = gameObject.AddComponent<InteractionTriggerBase>();
                functionality = gameObject.AddComponent<InteractionFunctionality>();
                activator.AutoTogglePauseTime = 0.5f;
                activator.BasicInteraction = functionality;
            }
        }

        public static void altarFunctionalityDeny(GameObject gameObject)
        {
            if (gameObject != null)
            {
                Destroy(gameObject);
            }
        }
        
        public static void lightAltar()
        {
            GameObject altar = GameObject.Find("JOHBENJI_DLC/altar");
            if (altar != null)
            {
                GameObject fireOnAltarOrg = GameObject.Find("JOHBENJI_DLC/altar/fire");
                if (activated)
                {
                    GameObject fireRef = GameObject.Find("Environment/Assets/FX/Prefab_fx_fire1x3Far/SmokeVolumetric");
                    if (fireRef != null)
                    {
                        GameObject fire = GameObject.Instantiate(fireRef);
                        fire.name = "fire";
                        fire.transform.parent = altar.transform;
                        fire.transform.localPosition = new Vector3(0f, 1.4f, -0.04f);
                        ParticleSystem particleSystem = fire.GetComponent<ParticleSystem>();
                        ParticleSystemRenderer particleRenderer = fire.GetComponent<ParticleSystemRenderer>();
                        if (particleSystem != null && particleRenderer != null)
                        {
                            particleRenderer.maxParticleSize = 0.06f;
                            particleSystem.emissionRate = 25f;
                            particleSystem.startColor = new Color(0.2604f, 0.2604f, 0.6039f, 1f);
                            ParticleSystem.ShapeModule shapeModule = particleSystem.shape;
                            shapeModule.box = new Vector3(0.1f, 0.1f, 0.1f);
                        }                                                                      
                    }
                }
            }
        }
    }
}