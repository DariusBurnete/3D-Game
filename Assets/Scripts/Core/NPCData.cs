using System;
using UnityEngine;

[System.Serializable]
public class ShopTier
{
    public ItemData[] items;
}

[CreateAssetMenu(menuName = "NPCs/New NPC")]
public class NPCData : ScriptableObject
{
    public string npcName;
    public string greeting;
    public Inventory inventory = new(24);

    [Header("Shop tiers (progression)")]
    public ShopTier[] shopTiers;

    [HideInInspector]
    public DateTime lastRefreshTime = DateTime.MinValue;
}

