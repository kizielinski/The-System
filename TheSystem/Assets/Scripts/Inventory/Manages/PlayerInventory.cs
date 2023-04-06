using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    //public PlayerAbilityManager playerAbilityManager;
    public Dictionary<string, IInventoryPage> inventoryPages;

    public void Awake()
    {
        inventoryPages = new Dictionary<string, IInventoryPage>();
        inventoryPages.Add("Equipment", new EquipmentPage());
        inventoryPages.Add("Quest", new QuestPage());
        inventoryPages.Add("Misc", new MiscPage());

        TestAddItems();
    }
    // Start is called before the first frame update
    public void Start()
    {
        //playerAbilityManager = this.gameObject.GetComponent<PlayerAbilityManager>();
    }

    // Update is called once per frame
    public void Update()
    {
        // Detect collision with collectibles

    }
    /// <summary>
    /// Add the item to the players inventory. If it is an equipment item, perform the logic to unlock the ability for the player.
    /// </summary>
    /// <param name="spriteName"></param>
    /// <param name="type"></param>
    /// <param name="isConsumable"></param>
    /// <param name="arrayPosition"></param>
    public void AddItem(InteractableModel itemData)
    {
        //switch (itemData.ItemType)
        //{
            //case ItemType.Armor:
            //case ItemType.Weapon:
            //case ItemType.Skill:
            //    {
            //        // Add item to the relevant equipment sub page.
            //        inventoryPages["Equipment"].AddItem(itemData);
            //        // Unlock the ability withing the player.
            //        playerAbilityManager.UnlockAbility(itemData);
            //        Debug.Log("Added Equipment item");
            //        break;
            //    }
            //case ItemType.Quest:
            //    {
            //        inventoryPages["Quest"].AddItem(itemData);
            //        break;
            //    }
            //case ItemType.Misc:
            //    {
            //        inventoryPages["Misc"].AddItem(itemData);
            //        break;
            //    }
        //}
    }
    public void TestAddItems()
    {
        //inventoryPages["Quest"].AddItem(new QuestItem("Holotape", true));
        //inventoryPages["Quest"].AddItem(new QuestItem("QuantumKey", true));
        //inventoryPages["Quest"].AddItem(new QuestItem("Holotape", true));

        //inventoryPages["Misc"].AddItem(new MiscItem("QuantumKey", false));
        //inventoryPages["Misc"].AddItem(new QuestItem("Holotape", false));
        //inventoryPages["Misc"].AddItem(new MiscItem("QuantumKey", false));
        //inventoryPages["Misc"].AddItem(new QuestItem("Holotape", false));
    }
}
