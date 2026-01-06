using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWithBodies : MonoBehaviour
{
    private IInteractable currentInteractable;

    void Update()
    {
        if (currentInteractable == null || !Keyboard.current.cKey.wasPressedThisFrame) return;
        currentInteractable.Interact();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("InteractionCollider")) return;
        currentInteractable = other.GetComponentInParent<IInteractable>();
        InteractionUI.instance.Show(currentInteractable.GetPromptText());
    }

    public void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("InteractionCollider")) return;
        currentInteractable = null;
        InteractionUI.instance.Hide();
    }

}
