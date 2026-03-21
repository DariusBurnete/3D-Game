using UnityEngine;

public abstract class UpgradeData : ScriptableObject
{
    public string upgradeId;
    public string upgradeName;
    [TextArea] 
    public string description;
    public Sprite icon;
    public int price;
    public UpgradeMaterialRequirement[] materialsRequired;
    public string requiredUpgradeId;

    public abstract void Apply(GameObject player);
}
