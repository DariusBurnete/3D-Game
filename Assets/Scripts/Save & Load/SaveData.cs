using System;
using System.Collections.Generic;

[Serializable]
public class SaveData
{
    public string sceneName;

    public float money;
    public List<string> purchasedUpgrades;

    public List<InventoryItemSave> inventory;

    public float[] shipPosition;
    public float[] shipRotation;

    public List<CelestialStateSave> celestialStates;
}

[Serializable]
public class InventoryItemSave
{
    public string itemID;
    public float value;
    public bool isSellable;
    public bool isEraseable;
}

[Serializable]
public class CelestialStateSave
{
    public string uniqueId;

    public float[] position;
    public float[] rotation;
    public float[] linearVelocity;
    public float[] angularVelocity;
}