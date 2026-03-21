using UnityEngine;

[RequireComponent(typeof(PlayerModeController))]
public abstract class ShipUpgrade : MonoBehaviour
{
    protected PlayerModeController controller;

    protected virtual void Awake()
    {
        controller = GetComponent<PlayerModeController>();

        if (controller != null)
            controller.RegisterUpgrade(this);
    }

    public virtual void SetUpgradeActive(bool active)
    {
        enabled = active;
    }
}