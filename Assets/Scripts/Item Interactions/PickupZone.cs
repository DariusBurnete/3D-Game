using UnityEngine;
using UnityEngine.InputSystem;

public class PickupZone : MonoBehaviour
{
    [SerializeField] 
    private ItemData itemToGive;

    private PlayerData playerData;
    private SpaceShip ship;
    private bool playerInZone;

    private void Update()
    {
        if (!playerInZone) return;

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            if (!ScreensInteraction.Instance.OpenPickup(playerData, ship)) return;

            ItemInstance itemInstance = new(itemToGive);
            DraggedItem.Instance.SetItem(itemInstance);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        ship = other.GetComponent<SpaceShip>();
        playerData = other.GetComponent<PlayerData>();
        playerInZone = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInZone = false;
    }
}
