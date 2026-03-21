using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using System;

public enum TooltipActionType
{
    None,
    Buy,
    Sell
}

public class ItemTooltipUI : MonoBehaviour
{
    public static ItemTooltipUI Instance { get; private set; }

    [System.Serializable]
    private class InstructionRow
    {
        public GameObject rowRoot;

        public TMP_Text part1;
        public TMP_Text key;
        public TMP_Text part2;

        public GameObject holdRoot;
        public Image holdImage;

        [HideInInspector] 
        public float holdTimer;
    }

    [Header("Roots")]
    [SerializeField] 
    private GameObject buyRoot;
    [SerializeField] 
    private GameObject sellRoot;

    [Header("Sell Rows")]
    [SerializeField] 
    private InstructionRow sellOneRow;
    [SerializeField] 
    private InstructionRow sellAllRow;
    [SerializeField] 
    private InstructionRow deleteRow;

    [Header("Buy Row")]
    [SerializeField] 
    private InstructionRow buyRow;

    [Header("Item Text")]
    [SerializeField] 
    private TMP_Text nameText;
    [SerializeField] 
    private TMP_Text valueText;
    [SerializeField] 
    private TMP_Text descriptionText;

    [Header("Hold Duration")]
    [SerializeField] 
    private float holdDuration = 1.2f;
    private float sellAllHoldTimer = 0f;

    [Header("Anchors")]
    [SerializeField] 
    private RectTransform infoRoot;
    [SerializeField] 
    private RectTransform buyAnchor;
    [SerializeField] 
    private RectTransform sellAnchor;

    private bool isLocked;
    private bool IsEraseable;
    private InventoryUI sourceInventoryUI;
    private int slotIndex;
    private TooltipActionType actionType;
    private InventoryUI npcInventoryUI;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Hide();
    }

    private bool CanSell => MechanicUIScreen.IsAnyOpen || MerchantUIScreen.IsAnyOpen;

    private void Update()
    {
        if (actionType == TooltipActionType.None) return;

        if (actionType == TooltipActionType.Buy)
            HandleBuyInput();

        if (actionType == TooltipActionType.Sell)
            HandleSellInput();
    }

    private void HandleBuyInput()
    {
        if (!isLocked) return;

        if (Keyboard.current.bKey.isPressed)
        {
            HandleHold(buyRow, CompleteBuy);
        }
        else
        {
            ResetHold(buyRow);
        }
    }

    private void HandleSellInput()
    {
        if (isLocked) return;

        if (Keyboard.current.vKey.isPressed)
            HandleHold(deleteRow, CompleteDelete);
        else
            ResetHold(deleteRow);

        if (!CanSell) return;

        if (Keyboard.current.nKey.isPressed)
        {
            sellAllHoldTimer += Time.deltaTime;
            HandleHold(sellAllRow, CompleteSellAll);
        }
        else
        {
            if (Keyboard.current.nKey.wasReleasedThisFrame && sellAllHoldTimer < holdDuration)
            {
                CompleteSellOne();
                Hide();
            }
            sellAllHoldTimer = 0f;
            ResetHold(sellAllRow);
        }
    }

    private void HandleHold(InstructionRow row, Action action)
    {
        if (row.holdImage == null) return;

        row.holdTimer += Time.deltaTime;
        row.holdImage.fillAmount = row.holdTimer / holdDuration;

        if (row.holdTimer >= holdDuration)
        {
            action.Invoke();
            Hide();
        }
    }

    private void ResetHold(InstructionRow row)
    {
        if (row.holdImage == null) return;

        row.holdTimer = 0f;
        row.holdImage.fillAmount = 0f;
    }

    public void Show(ItemInstance item, bool locked, InventoryUI inventoryUI, int index, TooltipActionType type, InventoryUI npcUI = null)
    {
        gameObject.SetActive(true);

        nameText.text = item.data.itemName;
        valueText.text = $"Price: {item.value}Åò";
        descriptionText.text = item.data.description;
        IsEraseable = item.isEraseable;

        actionType = type;
        isLocked = locked;
        sourceInventoryUI = inventoryUI;
        slotIndex = index;
        npcInventoryUI = npcUI;

        ConfigureSide();
        ConfigureInstructions();
    }

    private void ConfigureSide()
    {
        bool isShop = sourceInventoryUI.IsShopInventory;

        buyRoot.SetActive(isShop);
        sellRoot.SetActive(!isShop);

        if (isShop)
            infoRoot.position = buyAnchor.position;
        else
            infoRoot.position = sellAnchor.position;
    }

    private void ConfigureInstructions()
    {
        if (actionType == TooltipActionType.Buy)
        {
            buyRoot.SetActive(true);
            sellRoot.SetActive(false);

            SetupRow(buyRow, "Hold", "B", "to buy", true);

            return;
        }

        if (actionType == TooltipActionType.Sell)
        {
            buyRoot.SetActive(false);
            sellRoot.SetActive(true);

            SetupRow(deleteRow, "Hold", "V", "to delete", true, IsEraseable);

            SetupRow(sellOneRow, "Press", "N", "to sell", false, CanSell);
            SetupRow(sellAllRow, "Hold", "N", "to sell all", true, CanSell);
        }
    }

    private void SetupRow(InstructionRow row, string p1, string key, string p2, bool showHold, bool rowActive = true)
    {
        if (row == null) return;

        if (row.rowRoot != null)
            row.rowRoot.SetActive(rowActive);

        row.part1.text = p1;
        row.key.text = key;
        row.part2.text = p2;

        if (row.holdRoot != null)
            row.holdRoot.SetActive(showHold);

        ResetHold(row);
    }

    public void Hide()
    {
        gameObject.SetActive(false);

        isLocked = false;
        sourceInventoryUI = null;
        actionType = TooltipActionType.None;
    }

    private void CompleteSellOne()
    {
        SellAndBuy.Instance.SellOneToNPC(
            sourceInventoryUI,
            slotIndex,
            npcInventoryUI
        );
    }

    private void CompleteSellAll()
    {
        SellAndBuy.Instance.SellAllToNPC(
            sourceInventoryUI,
            npcInventoryUI
        );
    }

    private void CompleteBuy()
    {
        SellAndBuy.Instance.BuyFromNPCKey(
            sourceInventoryUI,
            slotIndex
        );
    }

    private void CompleteDelete()
    {
        SellAndBuy.Instance.DeleteItem(
            sourceInventoryUI, 
            slotIndex
        );
    }

}