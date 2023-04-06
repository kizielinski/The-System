using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentPage : IInventoryPage
{
    public Item[][] ArmorItems { get; set; }
    public Item[][] WeaponItems { get; set; }
    public Item[][] SkillItems { get; set; }
    public EquipmentPage()
    {
        ArmorItems = new Item[3][];
        WeaponItems = new Item[3][];
        SkillItems = new Item[3][];
        InitializeArrays();
    }
    /// <summary>
    /// Add the item to the corresponding equipment page.
    /// </summary>
    /// <param name="itemData"></param>
    /// <returns></returns>
    public bool AddItem(InteractableModel itemData)
    {
        //switch (itemData.ItemType)
        //{
        //    case ItemType.Armor:
        //        {
        //            ArmorItems[itemData.UpgradeKey / 2][itemData.UpgradeKey % 2] = new EquipmentItem(itemData.SpriteName, itemData.IsConsumable);
        //            return true;
        //        }
        //    case ItemType.Weapon:
        //        {
        //            WeaponItems[itemData.UpgradeKey / 4][itemData.UpgradeKey % 4] = new EquipmentItem(itemData.SpriteName, itemData.IsConsumable);
        //            return true;
        //        }
        //    case ItemType.Skill:
        //        {
        //            SkillItems[itemData.UpgradeKey / 5][itemData.UpgradeKey % 5] = new EquipmentItem(itemData.SpriteName, itemData.IsConsumable);
        //            return true;
        //        }
        //}
        return false;
    }

    public void InitializeArrays()
    {
        for(int i = 0; i < ArmorItems.Length; i++)
        {
            ArmorItems[i] = new Item[2];
        }

        for (int i = 0; i < WeaponItems.Length; i++)
        {
            WeaponItems[i] = new Item[4];
        }

        for (int i = 0; i < SkillItems.Length; i++)
        {
            SkillItems[i] = new Item[5];
        }

        // Add items programatically.
    }

    public void UnlockAbility()
    {

    }
}
