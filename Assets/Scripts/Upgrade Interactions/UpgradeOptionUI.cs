using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeOptionUI : MonoBehaviour
{
    [SerializeField] 
    private TMP_Text buttonText;
    [SerializeField] 
    private Button button;
    [SerializeField] 
    private Image buttonImage;

    private UpgradeData upgradeData;
    private UpgradeScreenUI screen;

    public void Init(UpgradeData data, UpgradeScreenUI parentScreen)
    {
        upgradeData = data;
        screen = parentScreen;

        buttonText.text = data.upgradeName;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClicked);

        PlayerData player = FindFirstObjectByType<PlayerData>();
        if (player != null && player.PurchasedUpgrades.Contains(data.upgradeId))
        {
            SetPurchasedVisual();
        }
    }

    private void OnClicked()
    {
        screen.ShowDescription(upgradeData, this);
    }

    public void SetPurchasedVisual()
    {
        buttonText.text = $"{upgradeData.upgradeName} \n[ Bought ]";
        buttonImage.color = Color.green;
    }
}
