using static AreaManager;

// RENAME 'OutwardModTemplate' TO SOMETHING ELSE
namespace BossRush
{
    public class PortalManager
    {
        private bool AreaSwitchInProgress = false;        
        private AreaManager.AreaEnum targetArea = AreaEnum.Berg;
        private Character targetCharacter;
        private int spawnpointindex;

        public void StartAreaSwitchAndSetPosition(Character Character, AreaManager.AreaEnum areaEnum, int SpawnPointIndex)
        {
            if (!CanStartAreaSwitch(areaEnum))
            {
                return;
            }

            spawnpointindex = SpawnPointIndex;
            targetArea = areaEnum;
            targetCharacter = Character;
            StartAreaSwitch(Character, areaEnum, spawnpointindex);
        }

        public void StartAreaSwitch(Character Character, AreaManager.AreaEnum areaEnum, int spawnPointIndex, bool moveBag = true)
        {
            if (!CanStartAreaSwitch(areaEnum))
            {
                return;
            }

            BossRushPlugin.Log.LogMessage("Starting area switch teleport");

            AreaSwitchInProgress = true;

            Area ChosenArea = AreaManager.Instance.GetArea(areaEnum);

            if (ChosenArea != null)
            {
                NetworkLevelLoader.Instance.RequestSwitchArea(ChosenArea.SceneName, spawnPointIndex, 1.5f, moveBag);
                // Start Coroutine to check when area has been switched
            }
            else
            {
                BossRushPlugin.Log.LogError($"Failed to start Teleport to {areaEnum} Area could not be found");
            }
        }
        private bool CanStartAreaSwitch(AreaEnum targetArea)
        {
            return !AreaSwitchInProgress && targetArea != AreaEnum.Nath_Test;
        }
    }
   
}