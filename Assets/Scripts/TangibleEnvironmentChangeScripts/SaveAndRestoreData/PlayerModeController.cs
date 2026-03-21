using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SpaceShip))]
[RequireComponent(typeof(SpaceShipP))]
[RequireComponent(typeof(DockingController))]
[RequireComponent(typeof(DockingCameraController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerModeController : MonoBehaviour
{
    private SpaceShip spaceShip;
    private SpaceShipP spaceShipP;
    private DockingController dockingController;
    private DockingCameraController cameraController;
    private PlayerInput playerInput;

    private readonly List<ShipUpgrade> upgrades = new();

    private void Awake()
    {
        spaceShip = GetComponent<SpaceShip>();
        dockingController = GetComponent<DockingController>();
        cameraController = GetComponent<DockingCameraController>();
        spaceShipP = GetComponent<SpaceShipP>();
        playerInput = GetComponent<PlayerInput>();
    }

    public void RegisterUpgrade(ShipUpgrade upgrade)
    {
        if (upgrade == null) return;
        if (!upgrades.Contains(upgrade))
            upgrades.Add(upgrade);
    }

    public void EnterSpaceMode()
    {
        if (spaceShip != null) spaceShip.enabled = true;
        if (spaceShipP != null) spaceShipP.enabled = false;
        if (dockingController != null) dockingController.enabled = true;
        if (cameraController != null) cameraController.enabled = true;

        if (playerInput != null) 
            playerInput.SwitchCurrentActionMap("SpaceShip Controls");

        foreach (var upgrade in upgrades)
        {
            if (upgrade == null) continue;
            upgrade.SetUpgradeActive(true);
        }
            
    }

    public void EnterPlanetMode()
    {
        if (spaceShip != null) spaceShip.enabled = false;
        if (spaceShipP != null) spaceShipP.enabled = true;
        if (dockingController != null) dockingController.enabled = false;
        if (cameraController != null) cameraController.enabled = false;

        if (playerInput != null) 
            playerInput.SwitchCurrentActionMap("SpaceShip P Controls");

        foreach (var upgrade in upgrades)
        {
            if (upgrade == null) continue;
            upgrade.SetUpgradeActive(false);
        }
            
    }
}