using TMPro;
using UnityEngine;

public class WarpButtonUI : MonoBehaviour
{
    [SerializeField] 
    private PlanetMarker planet;

    public void Warp()
    {
        if (planet == null) return;
        WarpDrive warpDrive = FindFirstObjectByType<WarpDrive>();
        if (warpDrive == null) return;

        ScreensInteraction.Instance.CloseCurrent();
        warpDrive.WarpTo(planet.transform.position);
    }
}