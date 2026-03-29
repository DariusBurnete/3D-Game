using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ScreenMode
{
    None,          
    PlayerInventory,
    PickupZone,
    Merchant,
    Mechanic,
    Upgrade,
    WarpMap,
    SceneMenu
}

public class ScreensInteraction : MonoBehaviour
{
    public static ScreensInteraction Instance { get; private set; }

    [SerializeField] 
    private MoneyUI moneyUI;

    [Header("Screens")]
    [SerializeField] 
    private PlayerUIScreen playerScreen;
    [SerializeField] 
    private MerchantUIScreen merchantScreen;
    [SerializeField]
    private MechanicUIScreen mechanicScreen;
    [SerializeField]
    private UpgradeScreenUI upgradeScreen;
    [SerializeField]
    private WarpMapUI warpMap;

    public ScreenMode Mode { get; set; } = ScreenMode.None;

    private PlayerData playerData;
    private int TierIndex = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        playerData = FindFirstObjectByType<PlayerData>();
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
            CloseCurrent();
    }

    public bool OpenPlayerInventory(PlayerData player, SpaceShip ship)
    {
        if (Mode != ScreenMode.None) return false;

        Mode = ScreenMode.PlayerInventory;
        ShowMoney();
        playerScreen.OpenInventory(player, ship);
        return true;
    }

    public bool OpenPickup(PlayerData player, SpaceShip ship)
    {
        if (Mode == ScreenMode.PickupZone) return true;
        if (Mode != ScreenMode.None) return false;

        Mode = ScreenMode.PickupZone;
        ShowMoney();
        playerScreen.OpenPickup(player, ship);
        return true;
    }

    public void OpenMerchant(PlayerData player, NPCData npc)
    {
        if (Mode != ScreenMode.None) return;

        Mode = ScreenMode.Merchant;
        PopulateShopTier(npc);
        ShowMoney();

        merchantScreen.Show(
            player.Inventory,
            npc.greeting
        );
    }

    public void OpenMechanic(PlayerData player, NPCData npc)
    {
        if (Mode != ScreenMode.None) return;

        Mode = ScreenMode.Mechanic;
        PopulateShopTier(npc);
        ShowMoney();

        mechanicScreen.Show(
            player.Inventory,
            npc.greeting
        );
    }

    public void OpenUpgrade()
    {
        if (Mode != ScreenMode.Mechanic) return;

        Mode = ScreenMode.Upgrade;
        upgradeScreen.Show();
    }

    public void OpenWarpMap()
    {
        if (Mode != ScreenMode.None) return;
        if (warpMap == null) return;

        Mode = ScreenMode.WarpMap;
        warpMap.Show();
    }

    public void CloseCurrent()
    {
        if (Mode == ScreenMode.None) return;

        switch (Mode)
        {
            case ScreenMode.WarpMap:
                warpMap.Hide();
                break;

            case ScreenMode.Upgrade:
                upgradeScreen.Hide();
                Mode = ScreenMode.Mechanic;
                return;

            case ScreenMode.PlayerInventory:
            case ScreenMode.PickupZone:
                playerScreen.Close();
                break;

            case ScreenMode.Merchant:
                merchantScreen.Hide();
                break;

            case ScreenMode.Mechanic:
                mechanicScreen.Hide();
                break;
        }

        Mode = ScreenMode.None;
        HideMoney();
    }

    private void ShowMoney()
    {
        moneyUI.SetAmount(playerData.Money);
        moneyUI.gameObject.SetActive(true);
    }

    private void HideMoney()
    {
        moneyUI.gameObject.SetActive(false);
    }

    private void PopulateShopTier(NPCData npc)
    {
        if (npc.shopTiers == null || npc.shopTiers.Length == 0) return;
        if (TierIndex < 0 || TierIndex >= npc.shopTiers.Length) return;
        if ((DateTime.Now - npc.lastRefreshTime).TotalSeconds < 180) return;

        for (int i = 0; i < npc.inventory.Items.Count; i++)
            npc.inventory.Items[i] = null;

        ShopTier tier = npc.shopTiers[TierIndex];

        for (int i = 0; i < tier.items.Length && i < npc.inventory.Items.Count; i++)
            npc.inventory.Items[i] = new ItemInstance(tier.items[i]);

        npc.lastRefreshTime = DateTime.Now;
    }

}

