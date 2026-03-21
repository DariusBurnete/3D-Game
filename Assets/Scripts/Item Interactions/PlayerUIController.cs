using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SpaceShip))]
[RequireComponent(typeof(PlayerData))]
[RequireComponent(typeof(DockingController))]
public class PlayerUIController : MonoBehaviour
{
    [SerializeField]
    private PlayerUIScreen playerUIScreen;

    private SpaceShip ship;
    private PlayerData playerData;
    private DockingController dockingController;

    private void Awake()
    {
        playerData = GetComponent<PlayerData>();
        ship = GetComponent<SpaceShip>();
        dockingController = GetComponent<DockingController>();
    }

    private void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            if (dockingController != null && (dockingController.IsDocked || dockingController.IsDocking)) return;

            ScreensInteraction.Instance.OpenPlayerInventory(playerData, ship);
        }

        if (Keyboard.current.mKey.wasPressedThisFrame)
        {
            if (GetComponent<WarpDrive>() == null) return;
            if (dockingController != null && (dockingController.IsDocked || dockingController.IsDocking)) return;

            ScreensInteraction.Instance.OpenWarpMap();
        }
    }

}

