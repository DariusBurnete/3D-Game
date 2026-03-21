using UnityEngine;

public class MechanicUIScreen : MonoBehaviour
{
    [SerializeField]
    private NPCData mechanicData;

    public InventoryUI playerInventoryUI;
    public InventoryUI mechanicInventoryUI;
    public NPCText npcText;

    public static MechanicUIScreen Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public bool IsOpen => gameObject.activeSelf;
    public static bool IsAnyOpen => Instance != null && Instance.IsOpen;

    public void Show(Inventory playerInv, string mechanicGreeting)
    {
        gameObject.SetActive(true);
        SellAndBuy.Instance.SetActiveNPC(mechanicData);
        playerInventoryUI.Bind(playerInv, false);
        mechanicInventoryUI.Bind(mechanicData.inventory, true);
        npcText.SetText(mechanicGreeting);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        SellAndBuy.Instance.ClearActiveNPC();
    }
}
