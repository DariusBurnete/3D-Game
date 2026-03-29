using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance { get; private set; }

    [SerializeField] private List<ItemData> allItems = new();

    private Dictionary<string, ItemData> lookup;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        lookup = new Dictionary<string, ItemData>();

        foreach (var item in allItems)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.itemID))
                continue;

            if (!lookup.ContainsKey(item.itemID))
                lookup.Add(item.itemID, item);
            else
                Debug.LogWarning($"Duplicate itemID found: {item.itemID}");
        }
    }

    public ItemData GetById(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return null;

        lookup.TryGetValue(id, out var item);
        return item;
    }
}