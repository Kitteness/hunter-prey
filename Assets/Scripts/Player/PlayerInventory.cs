using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private HashSet<string> ownedItems = new HashSet<string>();

    public bool HasItem(string itemID)
    {
        return ownedItems.Contains(itemID);
    }

    public void AddItem(string itemID)
    {
        if (!ownedItems.Contains(itemID))
        {
            ownedItems.Add(itemID);
            Debug.Log($"Get item: {itemID}");
        }
    }

    public void RemoveItem(string itemID)
    {
        if (ownedItems.Contains(itemID))
        {
            ownedItems.Remove(itemID);
            Debug.Log($"Remove item: {itemID}");
        }
    }
}
