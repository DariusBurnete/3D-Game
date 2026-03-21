using TMPro;
using UnityEngine;

public class WarpMapUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text cooldownText;
    [SerializeField]
    private TMP_Text[] planetButtons;
    [SerializeField]
    private PlanetMarker[] planetMarkers;

    private WarpDrive warpDrive;
    private SpaceShip ship;

    private void Start()
    {
        for(int i = 0; i < planetButtons.Length; i++)
        {
            planetButtons[i].text = $"[ {planetMarkers[i].planetName} ]";
        }
    }

    private void OnEnable()
    {
        warpDrive = FindFirstObjectByType<WarpDrive>();
        ship = FindFirstObjectByType<SpaceShip>();
    }

    private void Update()
    {
        if (warpDrive.IsOnCooldown)
        {
            cooldownText.text = "Cooldown: " + warpDrive.CooldownRemaining.ToString("0.0") + "s";
        }
        else
        {
            cooldownText.text = "Warp Drive Ready";
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        ship.SetControlsEnabled(false);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        ship.SetControlsEnabled(true);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
