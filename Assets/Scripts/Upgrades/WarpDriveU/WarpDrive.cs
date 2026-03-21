using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SpaceShip))]
public class WarpDrive : ShipUpgrade
{
    public float warpDuration = 3f;
    public float warpSpeed = 500f;
    public float warpOffset = 900f;
    public float cooldown = 120f;
    public float warpFOV = 150f;

    private Rigidbody rb;
    private SpaceShip ship;
    private Camera cam;

    private bool isWarping = false;
    private float cooldownTimer = 0f;
    private float defaultFOV;
    private bool isActive = true;
    public float CooldownRemaining => cooldownTimer;
    public bool IsOnCooldown => cooldownTimer > 0f;

    protected override void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody>();
        ship = GetComponent<SpaceShip>();
        cam = Camera.main;
        defaultFOV = cam.fieldOfView;
    }

    void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;

            if (cooldownTimer < 0f)
                cooldownTimer = 0f;
        }
    }

    public bool CanWarp => !isWarping && cooldownTimer <= 0f;

    public void WarpTo(Vector3 destination)
    {
        if (!isActive || isWarping || !CanWarp) return;
        StartCoroutine(WarpRoutine(destination));
    }

    IEnumerator WarpRoutine(Vector3 destination)
    {
        isWarping = true;

        ship.SetControlsEnabled(false);

        float timer = 0f;

        while (timer < warpDuration)
        {
            rb.linearVelocity = transform.forward * warpSpeed;

            cam.fieldOfView = Mathf.Lerp(defaultFOV, warpFOV, timer / warpDuration);

            timer += Time.deltaTime;
            yield return null;
        }

        Vector3 safePosition = destination + Random.onUnitSphere * warpOffset;

        rb.isKinematic = true;
        rb.position = safePosition;
        rb.isKinematic = false;

        cam.fieldOfView = defaultFOV;
        ship.SetControlsEnabled(true);
        cooldownTimer = cooldown;
        isWarping = false;
    }

    public override void SetUpgradeActive(bool active)
    {
        base.SetUpgradeActive(active);
        isActive = active;
    }
}