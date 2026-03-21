using UnityEngine;

public class EnterToPlanet : MonoBehaviour
{
    private void Start()
    {
        PlayerModeController modeController = FindFirstObjectByType<PlayerModeController>();
        if (modeController != null) modeController.EnterPlanetMode();

        if (GameStateManager.Instance != null && GameStateManager.Instance.CurrentPlanet != null)
            SkyboxHelper.ApplySkybox(GameStateManager.Instance.CurrentPlanet.planetSkybox);

        PlayerWithBodies player = FindFirstObjectByType<PlayerWithBodies>();
        if (player != null)
        {
            player.ClearCurrentInteractable();
            player.LockInteractionTemporarily();
        }
    }
}