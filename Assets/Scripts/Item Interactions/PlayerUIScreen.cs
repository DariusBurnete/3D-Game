using UnityEngine;

public class PlayerUIScreen : MonoBehaviour
{
    [SerializeField] 
    private InventoryUI inventoryUI;

    private PlayerData playerData;
    private SpaceShip ship;

    public void OpenInventory(PlayerData data, SpaceShip shipRef)
    {
        OpenCommon(data, shipRef);
    }

    public void OpenPickup(PlayerData data, SpaceShip shipRef)
    {
        OpenCommon(data, shipRef);
    }

    private void OpenCommon(PlayerData data, SpaceShip shipRef)
    {
        playerData = data;
        ship = shipRef;
        
        inventoryUI.Bind(playerData.Inventory, false);

        gameObject.SetActive(true);
        ship.SetControlsEnabled(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Close()
    {
        inventoryUI.Hide();
        DraggedItem.Instance.Hide();

        ship.SetControlsEnabled(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gameObject.SetActive(false);
    }

}

