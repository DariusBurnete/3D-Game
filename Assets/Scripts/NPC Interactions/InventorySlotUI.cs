using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour,
    IPointerDownHandler,
    IPointerEnterHandler,
    IPointerExitHandler
{
    [Header("UI References")]
    [SerializeField]
    private Image backgroundImage;
    [SerializeField] 
    private Image iconImage;
    [HideInInspector] 
    private int slotIndex;

    private InventoryUI inventoryUI;
    private Inventory Inventory => inventoryUI != null ? inventoryUI.BoundInventory : null;

    public void Init(InventoryUI ui, int index)
    {
        inventoryUI = ui;
        slotIndex = index;
    }

    private ItemInstance CurrentItemInstance
    {
        get
        {
            if (inventoryUI == null) return null;

            var inv = inventoryUI.BoundInventory;
            if (inv == null) return null;

            if (slotIndex >= inv.Items.Count) return null;

            return inv.Items[slotIndex];
        }
    }

    private bool IsEmpty => CurrentItemInstance == null;

    private bool IsLocked =>
        inventoryUI != null && 
        inventoryUI.IsShopInventory;

    private TooltipActionType ActionType => inventoryUI.IsShopInventory
            ? TooltipActionType.Buy
            : TooltipActionType.Sell;

    public void SetVisual(ItemInstance item)
    {
        iconImage.enabled = item != null;
        if (item != null) iconImage.sprite = item.data.icon;
    }

    public void ClearVisual()
    {
        iconImage.enabled = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsLocked) return;
        var dragged = DraggedItem.Instance;
        if (dragged == null) return;

        if (!dragged.HasItem && !IsEmpty)
        {
            dragged.SetItem(CurrentItemInstance);
            Inventory.Items[slotIndex] = null;
            inventoryUI.Refresh();
            return;
        }

        if (dragged.HasItem && IsEmpty)
        {
            Inventory.Items[slotIndex] = dragged.CurrentItemInstance;
            dragged.Hide();
            inventoryUI.Refresh();
            return;
        }

        if (dragged.HasItem && !IsEmpty)
        {
            ItemInstance temp = CurrentItemInstance;
            Inventory.Items[slotIndex] = dragged.CurrentItemInstance;
            dragged.SetItem(temp);
            inventoryUI.Refresh();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (CurrentItemInstance == null) return;
        InventoryUI npcUI = null;

        if (MerchantUIScreen.IsAnyOpen)
            npcUI = MerchantUIScreen.Instance.merchantInventoryUI;
        else if (MechanicUIScreen.IsAnyOpen)
            npcUI = MechanicUIScreen.Instance.mechanicInventoryUI;

        if (ItemTooltipUI.Instance != null)
            ItemTooltipUI.Instance.Show(
                CurrentItemInstance,
                IsLocked,
                inventoryUI,
                slotIndex,
                ActionType,
                npcUI
            );
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ItemTooltipUI.Instance != null)
            ItemTooltipUI.Instance.Hide();
    }

}
