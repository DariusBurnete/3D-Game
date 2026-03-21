using System.Collections.Generic;

public class Inventory
{
    public List<ItemInstance> Items;

    public Inventory(int size = 30)
    {
        Items = new List<ItemInstance>(size);
        for (int i = 0; i < size; i++)
            Items.Add(null);
    }

    /*public bool Add(ItemData item)
    {
        if (item == null) return false;

        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i] == null)
            {
                Items[i] = new ItemInstance(item);
                return true;
            }
        }
        return false;
    }*/

    public bool Add(ItemInstance instance)
    {
        if (instance == null) return false;

        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i] == null)
            {
                Items[i] = instance;
                return true;
            }
        }
        return false;
    }

    public bool Remove(ItemInstance instance)
    {
        if (instance == null) return false;

        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i] == instance)
            {
                Items[i] = null;
                return true;
            }
        }
        return false;
    }

    public bool Contains(ItemInstance instance)
    {
        return Items.Contains(instance);
    }
}