using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeScreenUI : MonoBehaviour
{
    [Header("Roots")]
    [SerializeField] 
    private GameObject root;
    [SerializeField] 
    private GameObject descriptionBackground;

    [Header("Description UI")]
    [SerializeField] 
    private TMP_Text descriptionTitle;
    [SerializeField] 
    private TMP_Text descriptionText;
    [SerializeField] 
    private TMP_Text priceText;
    [SerializeField] 
    private Image descriptionImage;

    [Header("Upgrade Buttons")]
    [SerializeField] 
    private UpgradeOptionUI[] upgradeOptions;
    [SerializeField]
    private Button purchaseButton;

    [Header("Player")]
    [SerializeField] 
    private InventoryUI playerInventoryUI;
    [SerializeField]
    private MoneyUI moneyUI;

    private UpgradeData[] upgrades;
    private UpgradeData selectedUpgrade;
    private UpgradeOptionUI selectedOptionUI;

    private void Awake()
    {
        upgrades = new UpgradeData[]
        {
        UpgradeDatabase.SpeedSensor,
        UpgradeDatabase.PlanetScanner,
        UpgradeDatabase.WarpDrive
        };
    }

    private void Start()
    {
        descriptionBackground.SetActive(false);

        for (int i = 0; i < upgradeOptions.Length; i++)
        {
            if (i < upgrades.Length)
            {
                upgradeOptions[i].Init(upgrades[i], this);
            }
            else
            {
                upgradeOptions[i].gameObject.SetActive(false);
            }
        }
    }

    public void Show()
    {
        root.SetActive(true);
    }

    public void Hide()
    {
        root.SetActive(false);
    }

    public void ShowDescription(UpgradeData data, UpgradeOptionUI optionUI)
    {
        selectedUpgrade = data;
        selectedOptionUI = optionUI;

        descriptionBackground.SetActive(true);

        descriptionImage.sprite = data.icon;
        descriptionTitle.text = data.upgradeName;
        descriptionText.text = data.description;

        string materialsString = "";
        for (int i = 0; i < data.materialsRequired.Length; i++)
        {
            var req = data.materialsRequired[i];
            materialsString += $"{req.material} x{req.amountRequired}\n";
        }

        priceText.text = $"Price: {data.price}˜ \nMaterials:\n{materialsString}";

        PlayerData player = FindFirstObjectByType<PlayerData>();
        bool hasRequiredUpgrade = player != null && HasRequiredUpgrade(player, data);
        if (!string.IsNullOrEmpty(data.requiredUpgradeId) && !hasRequiredUpgrade)
        {
            UpgradeData required = UpgradeDatabase.GetById(data.requiredUpgradeId);
            descriptionText.text += required != null ? $"\nREQUIRES: [ {required.upgradeName} ]" : "";
        }

        bool alreadyBought =  player != null && player.PurchasedUpgrades.Contains(data.upgradeId);
        purchaseButton.interactable = !alreadyBought;
    }

    public void TryPurchaseSelectedUpgrade()
    {
        if (selectedUpgrade == null) return;

        PlayerData player = FindFirstObjectByType<PlayerData>();
        if (player == null) return;
        if (!HasRequiredUpgrade(player, selectedUpgrade)) return;
        if (player.PurchasedUpgrades.Contains(selectedUpgrade.upgradeId)) return;
        if (player.Money < selectedUpgrade.price) return;
        if (!HasRequiredMaterials(player, selectedUpgrade)) return;

        player.Money -= selectedUpgrade.price;
        RemoveMaterials(player, selectedUpgrade);

        selectedUpgrade.Apply(player.gameObject);

        moneyUI.SetAmount(player.Money);
        selectedOptionUI.SetPurchasedVisual();
        purchaseButton.interactable = false;
    }

    private bool HasRequiredUpgrade(PlayerData player, UpgradeData data)
    {
        if (string.IsNullOrEmpty(data.requiredUpgradeId))
            return true;

        return player.PurchasedUpgrades.Contains(data.requiredUpgradeId);
    }

    private bool HasRequiredMaterials(PlayerData player, UpgradeData data)
    {
        foreach (var requirement in data.materialsRequired)
        {
            int count = 0;

            foreach (var item in player.Inventory.Items)
            {
                if (item != null && item.data.itemName == requirement.material)
                {
                    count++;
                }
            }

            if (count < requirement.amountRequired)
                return false;
        }

        return true;
    }

    private void RemoveMaterials(PlayerData player, UpgradeData data)
    {
        foreach (var requirement in data.materialsRequired)
        {
            int remaining = requirement.amountRequired;

            for (int i = 0; i < player.Inventory.Items.Count; i++)
            {
                var item = player.Inventory.Items[i];

                if (item != null && item.data.itemName == requirement.material && remaining > 0)
                {
                    player.Inventory.Items[i] = null;
                    remaining--;
                }
            }
        }
        playerInventoryUI.Refresh();
    }
}
