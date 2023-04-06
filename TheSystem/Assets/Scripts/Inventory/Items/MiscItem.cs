using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscItem : Item
{
    public MiscItem(string name, bool consumable)
    {
        _name = name;
        _image = Resources.Load<Sprite>($"ItemImages/{name}");
        IsConsumable = consumable;
    }
}
