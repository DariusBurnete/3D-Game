using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item")]
public class ItemData: ScriptableObject
{
    [Header("Basic Info")]
    public string itemName;
    public Sprite icon;
    [TextArea(3, 6)]
    public string description;

    [Header("Economy")]          
    public float minPrice;       
    public float maxPrice;

    [Header("Type")]
    public string itemType;
    public bool isSellable;
    public bool isEraseable;
}

