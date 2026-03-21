using UnityEngine;

public class SellAndBuy : MonoBehaviour
{
    public static SellAndBuy Instance { get; private set; }

    [SerializeField]
    private MoneyUI moneyUI;

    private PlayerData player;
    private NPCData activeNPC;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        player = FindFirstObjectByType<PlayerData>();
    }

    public void SetActiveNPC(NPCData npc)
    {
        activeNPC = npc;
    }

    public void ClearActiveNPC()
    {
        activeNPC = null;
    }

    private void SellToNPC(NPCData npc, InventoryUI playerInventoryUI, int? specificSlot = null) 
    {
        if (npc == null || playerInventoryUI == null) return;

        Inventory playerInv = playerInventoryUI.BoundInventory;
        if (playerInv == null) return;

        float totalValue = 0f;

        for (int i = 0; i < playerInv.Items.Count; i++)
        {
            if (specificSlot.HasValue && i != specificSlot.Value)
                continue;

            ItemInstance item = playerInv.Items[i];
            if (item == null) continue;
            if (!item.isSellable) continue;

            int npcSlot = GetFreeSlot(npc.inventory);
            if (npcSlot == -1) break;

            npc.inventory.Items[npcSlot] = item;
            playerInv.Items[i] = null;

            item.isSellable = false;
            totalValue += item.value;
        }

        player.Money += totalValue;
        moneyUI.SetAmount(player.Money);

        playerInventoryUI.Refresh();
    }

    private int GetFreeSlot(Inventory inventory)
    {
        for (int i = 0; i < inventory.Items.Count; i++)
        {
            if (inventory.Items[i] == null)
                return i;
        }

        return -1; 
    }

    public void SellOneToNPC(InventoryUI playerInventoryUI, int slotIndex, InventoryUI npcInventoryUI)
    {
        SellToNPC(activeNPC, playerInventoryUI, slotIndex);
        if (npcInventoryUI != null) npcInventoryUI.Refresh();
    }

    public void SellAllToNPC(InventoryUI playerInventoryUI, InventoryUI npcInventoryUI)
    {
        SellToNPC(activeNPC, playerInventoryUI, null);
        if (npcInventoryUI != null) npcInventoryUI.Refresh();
    }

    public void BuyFromNPCKey(InventoryUI npcInventoryUI, int slotIndex)
    {
        if (activeNPC == null) return;

        Inventory npcInv = npcInventoryUI.BoundInventory;
        if (npcInv == null) return;

        if (slotIndex < 0 || slotIndex >= npcInv.Items.Count) return;

        ItemInstance itemInstance = npcInv.Items[slotIndex];
        if (itemInstance == null) return;

        if (player.Money < itemInstance.value) return;

        player.Money -= itemInstance.value;
        moneyUI.SetAmount(player.Money);

        itemInstance.isSellable = true;
        itemInstance.value = Mathf.Round(itemInstance.value * 50f) / 100f;

        npcInv.Items[slotIndex] = null;

        npcInventoryUI.Refresh();

        DraggedItem.Instance.SetItem(itemInstance);
    }

    public void DeleteItem(InventoryUI inventoryUI, int slotIndex)
    {
        if (inventoryUI == null) return;

        Inventory inventory = inventoryUI.BoundInventory;
        if (inventory == null) return;

        if (slotIndex < 0 || slotIndex >= inventory.Items.Count) return;
        if (inventory.Items[slotIndex] == null) return;
        if (!inventory.Items[slotIndex].isEraseable) return;

        inventory.Items[slotIndex] = null;
        inventoryUI.Refresh();
    }

}
