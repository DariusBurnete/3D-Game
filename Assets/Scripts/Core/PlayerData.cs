using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance { get; private set; }

    public Inventory Inventory;
    public float Money;
    public HashSet<string> PurchasedUpgrades;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        Inventory = new Inventory();
        PurchasedUpgrades = new HashSet<string>();
    }

    public void ReapplyPurchasedUpgrades()
    {
        foreach (string upgradeId in PurchasedUpgrades)
        {
            UpgradeData upgrade = UpgradeDatabase.GetById(upgradeId);
            if (upgrade == null) continue;
            upgrade.Apply(gameObject);
        }
    }
}