using UnityEngine;

public class WarpDriveUpgrade : UpgradeData
{
    public override void Apply(GameObject player)
    {
        PlayerData data = player.GetComponent<PlayerData>();

        if (data.PurchasedUpgrades.Contains(upgradeId))
            return;

        if (player.GetComponent<WarpDrive>() == null)
            player.AddComponent<WarpDrive>();

        data.PurchasedUpgrades.Add(upgradeId);
    }
}