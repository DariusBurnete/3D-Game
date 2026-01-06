using UnityEngine;

public class SSInteract : MonoBehaviour, IInteractable
{
    
    public void Interact()
    {
        InteractionUI.instance.Hide();

        PlayerWithBodies player = FindFirstObjectByType<PlayerWithBodies>();
        player.LockInput();

        Debug.Log("Interacted with SSInteract object.");
    }

    public string GetPromptText() => "C";
}
