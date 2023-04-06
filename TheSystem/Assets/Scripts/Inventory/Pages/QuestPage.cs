using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPage : IInventoryPage
{
    public List<Item> _items { get; set; }

    public QuestPage()
    {
        _items = new List<Item>();
    }
    public bool AddItem(InteractableModel itemData)
    {
       
        return true;
    }
}
