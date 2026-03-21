using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpaceShip))]
[RequireComponent(typeof(DockingCameraController))]
[RequireComponent(typeof(PlayerWithBodies))]
[RequireComponent(typeof(Rigidbody))]
public class DockingController : MonoBehaviour
{
    [SerializeField]
    private Transform playerComponents;
    [SerializeField] 
    private float dockingSpeed = 100f;
    [SerializeField] 
    private float rotationSpeed = 100f;
    [SerializeField] 
    private float yOffset = 18f;
    [SerializeField]
    private float undockOffset = 16f;

    [SerializeField]
    private float dockingSnapDistance = 2f;
    [SerializeField]
    private float maxDockingTime = 3f;

    private Transform currentStationCenter;
    private SpaceShip spaceShip;
    private DockingCameraController cameraController;
    private PlayerWithBodies player;

    private Rigidbody shipRB;
    private RigidbodyInterpolation originalInterpolation;

    private bool isDocked;
    private bool isDocking;

    private int shipLayer;
    private int celestialLayer;

    private ShipUpgrade[] upgradeRuntimes;

    public bool IsDocked => isDocked;
    public bool IsDocking => isDocking;

    void Awake()
    {
        shipRB = GetComponent<Rigidbody>();
        spaceShip = GetComponent<SpaceShip>();
        cameraController = GetComponent<DockingCameraController>();
        player = GetComponent<PlayerWithBodies>();

        shipLayer = LayerMask.NameToLayer("Ship");
        celestialLayer = LayerMask.NameToLayer("Celestial");
    }

    public void Dock(Transform stationCenter)
    {
        if (isDocked || isDocking) return;
        if (stationCenter == null) return;

        currentStationCenter = stationCenter;
        isDocking = true;

        SetUpgradesActive(false);

        originalInterpolation = shipRB.interpolation;
        shipRB.interpolation = RigidbodyInterpolation.None;

        Physics.IgnoreLayerCollision(shipLayer, celestialLayer, true);
        spaceShip.SetControlsEnabled(false);
        cameraController.SwitchToStationView();

        shipRB.isKinematic = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StartCoroutine(AlignAndFreeze());
    }

    private IEnumerator AlignAndFreeze()
    {
        float swerve = transform.eulerAngles.y;
        float timer = 0f;

        while (true)
        {
            Vector3 targetWorldPos = currentStationCenter.position + currentStationCenter.up * yOffset;
            Quaternion targetRot = Quaternion.Euler(0f, swerve, 0f);

            float posDist = Vector3.Distance(transform.position, targetWorldPos);

            if (posDist <= dockingSnapDistance || timer >= maxDockingTime)
                break;

            transform.SetPositionAndRotation(
                Vector3.MoveTowards(
                    transform.position,
                    targetWorldPos,
                    dockingSpeed * Time.deltaTime
                ),
                Quaternion.RotateTowards(
                    transform.rotation,
                    targetRot,
                    rotationSpeed * Time.deltaTime
                )
            );

            timer += Time.deltaTime;
            yield return null;
        }

        transform.SetPositionAndRotation(
            currentStationCenter.position + currentStationCenter.up * yOffset,
            Quaternion.Euler(0f, swerve, 0f)
        );

        InteractionUI.instance.ShowMerchantUI();
        InteractionUI.instance.ShowMechanicUI();
        InteractionUI.instance.ShowTakeOff();

        isDocking = false;
        isDocked = true;

        AudioManager.Instance.PlayMusic(AudioID.MusicGameplay);
    }

    public void Undock()
    {
        if (!isDocked) return;
        StartCoroutine(UndockRoutine());
    }

    IEnumerator UndockRoutine()
    {
        isDocked = false;
        SetUpgradesActive(true);

        cameraController.SwitchToShipView();

        Vector3 undockDirection = currentStationCenter != null ? currentStationCenter.up : Vector3.up;
        Vector3 undockPosition = transform.position + undockDirection * undockOffset;

        shipRB.position = undockPosition;
        shipRB.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);

        yield return new WaitForFixedUpdate();

        shipRB.isKinematic = false;
        shipRB.interpolation = originalInterpolation;

        spaceShip.SetControlsEnabled(true);
        Physics.IgnoreLayerCollision(shipLayer, celestialLayer, false);
        yield return new WaitForFixedUpdate();
        spaceShip.enabled = true;

        if (player.CurrentInteractable != null)
        {
            var interactable = player.CurrentInteractable;
            InteractionUI.instance.ShowBodyType(
                interactable.GetPromptText(),
                player.GetSpriteForBodyType(interactable.GetCelestialBodyType())
            );
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentStationCenter = null;

        AudioManager.Instance.StopMusic();
    }

    private void RefreshUpgradeCache()
    {
        upgradeRuntimes = GetComponents<ShipUpgrade>();
    }

    private void SetUpgradesActive(bool active)
    {
        RefreshUpgradeCache();

        foreach (var upgrade in upgradeRuntimes)
            upgrade.SetUpgradeActive(active);
    }

    void LateUpdate()
    {
        if (!isDocked) return;
        if (currentStationCenter == null) return;

        shipRB.position = currentStationCenter.position + currentStationCenter.up * yOffset;
        shipRB.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
    }

}