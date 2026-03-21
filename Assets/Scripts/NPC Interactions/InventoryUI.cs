using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] 
    private InventorySlotUI[] slots;
    public InventorySlotUI[] Slots => slots;

    private Inventory boundInventory;
    public Inventory BoundInventory => boundInventory;

    public bool IsShopInventory { get; private set; }

    public void Bind(Inventory inventory, bool isShop)
    {
        boundInventory = inventory;
        IsShopInventory = isShop;

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Init(this, i);
        }

        Refresh();
    }

    public void Refresh()
    {
        if (boundInventory == null) return;

        for (int i = 0; i < slots.Length; i++)
        {
            if (i < boundInventory.Items.Count)
                slots[i].SetVisual(boundInventory.Items[i]);
            else
                slots[i].ClearVisual();
        }
    }

    public void Hide()
    {
        boundInventory = null;
    }
}

