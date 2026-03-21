using UnityEngine;

[System.Serializable]
public class ItemInstance
{
    public ItemData data;
    [HideInInspector]
    public float value;
    [HideInInspector]
    public bool isSellable;
    [HideInInspector]
    public bool isEraseable;

    public ItemInstance(ItemData data)
    {
        this.data = data;
        float result = data.minPrice != data.maxPrice
                ? Random.Range(data.minPrice, data.maxPrice + 1)
                : data.minPrice;
        value = Mathf.Round(result * 100f) / 100f;
        isSellable = data.isSellable;
        isEraseable = data.isEraseable;
    }

}
