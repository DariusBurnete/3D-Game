using UnityEngine;

public class SpeedSensorUpgrade : UpgradeData
{
    public override void Apply(GameObject player)
    {
        PlayerData data = player.GetComponent<PlayerData>();
        if (data.PurchasedUpgrades.Contains(upgradeId)) return;

        if (player.GetComponent<SpeedSensor>() == null)
            player.AddComponent<SpeedSensor>();

        data.PurchasedUpgrades.Add(upgradeId);
    }
}
