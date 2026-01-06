using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SpaceShip))]
public class PlayerWithBodies : MonoBehaviour
{
    private IInteractable currentInteractable;
    private bool inputLocked;
    [SerializeField]
    private SpaceShip spaceShip;

    private void Awake()
    {
        if(spaceShip == null) spaceShip = GetComponent<SpaceShip>();
    }

    void Update()
    {
        if(inputLocked) return;
        if (currentInteractable == null || !Keyboard.current.cKey.wasPressedThisFrame) return;
        currentInteractable.Interact();
    }

    public void LockInput()
    {
        inputLocked = true;
        spaceShip.enabled = false;
    }
    public void UnlockInput()
    {
        inputLocked = false;
        spaceShip.enabled = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        if(inputLocked) return;
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
