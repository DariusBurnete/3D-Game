using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpeedSensor : ShipUpgrade
{
    private Rigidbody rb;
    private SpeedUI speedUI;

    protected override void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody>();
        TryResolveSpeedUI();  
    }

    private void Update()
    {
        TryResolveSpeedUI();

        if (speedUI == null) return;
        float speed = rb.linearVelocity.magnitude;
        speedUI.SetSpeed(speed);
    }

    private void TryResolveSpeedUI()
    {
        if (speedUI == null)
            speedUI = FindFirstObjectByType<SpeedUI>(FindObjectsInactive.Include);

        if (speedUI != null)
            speedUI.Show();
    }

    public override void SetUpgradeActive(bool active)
    {
        base.SetUpgradeActive(active);
        TryResolveSpeedUI();

        if (speedUI != null)
            speedUI.gameObject.SetActive(active);
    }
}
