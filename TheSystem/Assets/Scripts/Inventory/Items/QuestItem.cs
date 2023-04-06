using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestItem : Item
{
    public QuestItem(string name, bool consumable)
    {
        _name = name;
        _image = Resources.Load<Sprite>($"ItemImages/{name}");
        IsConsumable = consumable;
    }
}
