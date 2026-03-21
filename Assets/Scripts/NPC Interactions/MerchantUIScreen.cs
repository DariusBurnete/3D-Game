using UnityEngine;

public class MerchantUIScreen : MonoBehaviour
{
    [SerializeField]
    private NPCData merchantData;

    public InventoryUI playerInventoryUI;
    public InventoryUI merchantInventoryUI;
    public NPCText npcText;

    public static MerchantUIScreen Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public bool IsOpen => gameObject.activeSelf;
    public static bool IsAnyOpen => Instance != null && Instance.IsOpen;

    public void Show(Inventory playerInv, string merchantGreeting)
    {
        gameObject.SetActive(true);
        SellAndBuy.Instance.SetActiveNPC(merchantData);
        playerInventoryUI.Bind(playerInv, false);
        merchantInventoryUI.Bind(merchantData.inventory, true);
        npcText.SetText(merchantGreeting);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        SellAndBuy.Instance.ClearActiveNPC();
    }
}

