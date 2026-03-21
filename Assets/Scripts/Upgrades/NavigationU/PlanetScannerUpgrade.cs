using UnityEngine;

public class PlanetScannerUpgrade : UpgradeData
{
    public override void Apply(GameObject player)
    {
        PlayerData data = player.GetComponent<PlayerData>();

        if (data.PurchasedUpgrades.Contains(upgradeId))
            return;

        if (player.GetComponent<PlanetScannerSystem>() == null)
            player.AddComponent<PlanetScannerSystem>();

        data.PurchasedUpgrades.Add(upgradeId);
    }
}