using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(DockingController))]
public class PlayerWithBodies : MonoBehaviour
{
    public IInteractable currentInteractable;

    [SerializeField]
    private float interactionCooldownDuration = 0.35f;
    private float interactionCooldownTimer;

    public IInteractable CurrentInteractable => currentInteractable;

    private void Update()
    {
        if (interactionCooldownTimer > 0f)
            interactionCooldownTimer -= Time.deltaTime;

        if (Keyboard.current.cKey.wasPressedThisFrame)
            InteractionUI.instance.PressKeyFeedback();

        if (!Keyboard.current.cKey.wasReleasedThisFrame)
            return;

        InteractionUI.instance.ReleaseKeyFeedback();

        if (interactionCooldownTimer > 0f) return;
        if (currentInteractable == null) return;
        if (InteractionUI.instance.IsMerchantOpen) return;
        if (InteractionUI.instance.IsMechanicOpen) return;

        currentInteractable.Interact();
    }

    public void OnTriggerEnter(Collider other)
    {
        TrySetInteractable(other);
    }

    public void OnTriggerStay(Collider other)
    {
        if (currentInteractable == null)
            TrySetInteractable(other);
    }

    public void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("InteractionCollider")) return;

        IInteractable interactable = other.GetComponentInParent<IInteractable>();
        if (interactable == null) return;
        if (currentInteractable != interactable) return;

        DockingController dockingController = GetComponent<DockingController>();
        bool isStation = interactable.GetCelestialBodyType() == CelestialBodyType.Station;

        if (isStation && (dockingController.IsDocking || dockingController.IsDocked))
            return;

        currentInteractable = null;
        InteractionUI.instance.HideAll();
    }

    private void TrySetInteractable(Collider other)
    {
        if (!other.CompareTag("InteractionCollider")) return;

        IInteractable interactable = other.GetComponentInParent<IInteractable>();
        if (interactable == null) return;

        currentInteractable = interactable;

        InteractionUI.instance.ShowBodyType(
            currentInteractable.GetPromptText(),
            GetSpriteForBodyType(currentInteractable.GetCelestialBodyType())
        );
    }

    public Sprite GetSpriteForBodyType(CelestialBodyType type)
    {
        return type switch
        {
            CelestialBodyType.Moon => InteractionUI.instance.moonSprite,
            CelestialBodyType.Planet => InteractionUI.instance.planetSprite,
            CelestialBodyType.Station => InteractionUI.instance.stationSprite,
            _ => InteractionUI.instance.landUI.defaultSprite
        };
    }

    public void LockInteractionTemporarily()
    {
        interactionCooldownTimer = interactionCooldownDuration;
    }

    public void ClearCurrentInteractable()
    {
        currentInteractable = null;
        InteractionUI.instance.HideAll();
    }
}