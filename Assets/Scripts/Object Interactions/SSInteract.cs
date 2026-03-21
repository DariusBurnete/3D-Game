using UnityEngine;

public class SSInteract : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Transform stationCenter;

    public void Interact()
    {
        DockingController dockingController = FindFirstObjectByType<DockingController>();
        if (dockingController == null) return;

        if (dockingController.IsDocking) return;

        if (dockingController.IsDocked)
            dockingController.Undock();
        else
            dockingController.Dock(stationCenter);
    }

    public string GetPromptText() => "C";
    public CelestialBodyType GetCelestialBodyType() => CelestialBodyType.Station;
}
