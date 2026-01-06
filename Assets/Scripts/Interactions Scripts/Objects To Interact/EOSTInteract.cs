using UnityEngine;

public class EOSTInteract : MonoBehaviour, IInteractable
{

    public void Interact()
    {
          Debug.Log("Interacted with EOSTInteract object.");
    }

    public string GetPromptText() => "C";
}
