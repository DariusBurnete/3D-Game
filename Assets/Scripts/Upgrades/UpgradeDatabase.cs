using UnityEngine;

public static class UpgradeDatabase
{
    public static readonly UpgradeData SpeedSensor;
    public static readonly UpgradeData PlanetScanner;
    public static readonly UpgradeData WarpDrive;

    private static UpgradeData[] All => new UpgradeData[]
    {
        SpeedSensor,
        PlanetScanner,
        WarpDrive
    };

    public static UpgradeData GetById(string id)
    {
        foreach (var upgrade in All)
        {
            if (upgrade.upgradeId == id)
                return upgrade;
        }

        return null;
    }

    static UpgradeDatabase()
    {
        SpeedSensor = ScriptableObject.CreateInstance<SpeedSensorUpgrade>();
        SpeedSensor.upgradeId = "speed_sensor";
        SpeedSensor.upgradeName = "Ship Speed Sensor Part";
        SpeedSensor.description = "Displays live ship speed permanently.";
        SpeedSensor.icon = null;
        SpeedSensor.price = 225;
        SpeedSensor.requiredUpgradeId = null;
        SpeedSensor.materialsRequired = new UpgradeMaterialRequirement[]
        {
            new() { material = "Scrap", amountRequired = 3 },
        };

        PlanetScanner = ScriptableObject.CreateInstance<PlanetScannerUpgrade>();
        PlanetScanner.upgradeId = "planet_scanner";
        PlanetScanner.upgradeName = "Planet Scanner Module";
        PlanetScanner.description = "Displays planet indicators with distance.";
        PlanetScanner.icon = null;
        PlanetScanner.price = 500;
        PlanetScanner.requiredUpgradeId = "speed_sensor";
        PlanetScanner.materialsRequired = new UpgradeMaterialRequirement[]
        {
            new() { material = "Scrap", amountRequired = 4 },
        };

        WarpDrive = ScriptableObject.CreateInstance<WarpDriveUpgrade>();
        WarpDrive.upgradeId = "warp_drive";
        WarpDrive.upgradeName = "Warp Drive Core";
        WarpDrive.description = "Allows instant travel between planets.";
        WarpDrive.icon = null;
        WarpDrive.price = 1200;
        WarpDrive.requiredUpgradeId = "planet_scanner";
        WarpDrive.materialsRequired = new UpgradeMaterialRequirement[]
        {
            new() { material = "Scrap", amountRequired = 8 },
        };
    }
}
