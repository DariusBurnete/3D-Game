using UnityEngine;

public class SpaceSceneRestore : MonoBehaviour
{
    private void Start()
    {
        RestoreSkybox();
        RestoreShip();
        RestorePlayerMode();
        LockPlayerInteraction();
    }

    private void RestoreSkybox()
    {
        if (GameStateManager.Instance != null)
            SkyboxHelper.ApplySkybox(GameStateManager.Instance.SpaceSkybox);
    }

    private void RestoreShip()
    {
        if (!GameStateManager.Instance.HasSavedShipTransform) return;
        if (GameStateManager.Instance == null) return;
        SpaceShip ship = FindFirstObjectByType<SpaceShip>();
        Rigidbody rb = ship.GetComponent<Rigidbody>();
        if (ship == null) return;

        Vector3 pos = GameStateManager.Instance.SavedShipPosition;
        Quaternion rot = GameStateManager.Instance.SavedShipRotation;

        if (rot == Quaternion.identity || rot == new Quaternion(0, 0, 0, 0))
            rot = Quaternion.identity;
        else
            rot = Quaternion.Normalize(rot);

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.position = pos;
            rb.rotation = rot;
        }
        else
        {
            ship.transform.SetPositionAndRotation(pos, rot);
        }
    }

    private void RestorePlayerMode()
    {
        PlayerModeController modeController = FindFirstObjectByType<PlayerModeController>();
        if (modeController != null) modeController.EnterSpaceMode();
    }

    private void LockPlayerInteraction()
    {
        PlayerWithBodies player = FindFirstObjectByType<PlayerWithBodies>();
        if (player != null)
        {
            player.ClearCurrentInteractable();
            player.LockInteractionTemporarily();
        }
    }
}