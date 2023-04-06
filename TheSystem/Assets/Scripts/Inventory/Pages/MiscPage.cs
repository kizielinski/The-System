using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscPage : IInventoryPage
{
    public List<Item> _items { get; set; }

    public MiscPage()
    {
        _items = new List<Item>();
    }

    public bool AddItem(InteractableModel itemData)
    {
        Debug.Log("Added Misc Item");
        _items.Add(new MiscItem(itemData.name, itemData.IsConsumable));
        return true;
    }
}
