using UnityEngine;

public class SSInteract : MonoBehaviour, IInteractable
{
    
    public void Interact()
    {
        InteractionUI.instance.Hide();
        Debug.Log("Interacted with SSInteract object.");
    }

    public string GetPromptText()
    {
        return "C";
    }
}
