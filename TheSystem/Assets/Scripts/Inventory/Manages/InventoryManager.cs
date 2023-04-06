using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class InventoryManager : MonoBehaviour
{
    public bool showEquipment;
    public bool showArmorInEquipment;
    public bool showWeaponInEquipment;
    public bool showSkillInEquipment;

    public bool showQuest;
    public bool showMisc;

    public GameObject staticImages;

    public GameObject equipmentPage;
    public GameObject ArmorPageInEquipment;
    public GameObject WeaponPageInEquipment;
    public GameObject SkillPageInEquipment;

    public GameObject QuestPage;
    public GameObject MiscPage;

    public GameObject[,] ArmorDisplay;
    public GameObject[,] WeaponDisplay;
    public GameObject[,] SkillDisplay;

    public GameObject[,] QuestDisplay;
    public GameObject[,] MiscDisplay;

    public GameObject useButton;

    // Menu button 'tabs'
    public GameObject menuButtons;
    public GameObject subMenuButtons;

    public GameObject playerDisplay;

    public bool useButtonSelected;

    public Sprite playerDisplaySprite;

    public Sprite itemBoxSprite;

    public Sprite uIBar;

    private GameObject player;

    public GameObject selectedItem;

    private float gridTileSize = 100;

    UnityEvent playUISoundEffect = new UnityEvent();


    // Start is called before the first frame update
    public void Awake()
    {
        playUISoundEffect.AddListener(PlaySound);
        staticImages = this.transform.Find("StaticImages").gameObject;
        staticImages.SetActive(false);
        // Parent page
        equipmentPage = this.transform.Find("EquipmentPage").gameObject;
        // Tabs inside of the parent page
        ArmorPageInEquipment = equipmentPage.transform.Find("ArmorSubPage").gameObject;
        WeaponPageInEquipment = equipmentPage.transform.Find("WeaponSubPage").gameObject;
        SkillPageInEquipment = equipmentPage.transform.Find("SkillSubPage").gameObject;
        // Other two pages
        QuestPage = this.transform.Find("QuestPage").gameObject;
        MiscPage = this.transform.Find("MiscPage").gameObject;
        showEquipment = false;
        showQuest = false;
        showMisc = false;
        itemBoxSprite = Resources.Load<Sprite>("ItemImages/Unselected_Box");
        // UI representation of items
        ArmorDisplay = new GameObject[3, 2];
        WeaponDisplay = new GameObject[3, 4];
        SkillDisplay = new GameObject[3, 5]; 
        QuestDisplay = new GameObject[3, 3];
        MiscDisplay = new GameObject[3, 3];
        player = GameObject.Find("Player");
        // Player image in inventory
        playerDisplay = GameObject.Find("PlayerDisplay");
        equipmentPage.SetActive(false);
        QuestPage.SetActive(false);
        MiscPage.SetActive(false);
        GenerateButtons();
        GenerateInventorySlots();
        HideInventory();
    }

    /// <summary>
    /// Create the slot layout of each of the inventory pages.
    /// </summary>
    private void GenerateInventorySlots()
    {
        Image img;

        // Armor display in equipment page
        for (int i = 0; i < ArmorDisplay.GetLength(0); i++)
        {
            for (int j = 0; j < ArmorDisplay.GetLength(1); j++)
            {
                GameObject g = new GameObject();
                img = g.AddComponent<Image>();
                img.sprite = itemBoxSprite;
                Button b = g.AddComponent<Button>();
                //b.onClick.AddListener(() => SelectItem(g));
                g.AddComponent<ItemId>();
                g.transform.position = SetTilePosition(i, j, "armor");
                g.transform.localScale = new Vector2(.9f, .9f);
                ArmorDisplay[i, j] = g;
                g.transform.SetParent(ArmorPageInEquipment.transform, false);
            }
        }
        ArmorPageInEquipment.transform.parent = equipmentPage.transform;

        // Weapon display in equipment page
        for (int i = 0; i < WeaponDisplay.GetLength(0); i++)
        {
            for (int j = 0; j < WeaponDisplay.GetLength(1); j++)
            {
                GameObject g = new GameObject();
                img = g.AddComponent<Image>();
                img.sprite = itemBoxSprite;
                Button b = g.AddComponent<Button>();
                //b.onClick.AddListener(() => SelectItem(g));
                g.AddComponent<ItemId>();
                g.transform.position = SetTilePosition(i, j, "weapon");
                g.transform.localScale = new Vector2(.9f, .9f);
                WeaponDisplay[i, j] = g;
                g.transform.SetParent(WeaponPageInEquipment.transform, false);
            }
        }
        WeaponPageInEquipment.transform.parent = equipmentPage.transform;

        // Skill display in equipment page
        for (int i = 0; i < SkillDisplay.GetLength(0); i++)
        {
            for (int j = 0; j < SkillDisplay.GetLength(1); j++)
            {
                GameObject g = new GameObject();
                img = g.AddComponent<Image>();
                img.sprite = itemBoxSprite;
                Button b = g.AddComponent<Button>();
                //b.onClick.AddListener(() => SelectItem(g));
                g.AddComponent<ItemId>();
                g.transform.position = SetTilePosition(i, j, "skills");
                g.transform.localScale = new Vector2(.9f, .9f);
                SkillDisplay[i, j] = g;
                g.transform.SetParent(SkillPageInEquipment.transform, false);
            }
        }
        SkillPageInEquipment.transform.parent = equipmentPage.transform;

        // Quest page
        for (int i = 0; i < QuestDisplay.GetLength(0); i++)
        {
            for (int j = 0; j < QuestDisplay.GetLength(1); j++)
            {
                GameObject g = new GameObject();
                img = g.AddComponent<Image>();
                img.sprite = itemBoxSprite;
                Button b = g.AddComponent<Button>();
                //b.onClick.AddListener(() => SelectItem(g));
                g.AddComponent<ItemId>();
                g.transform.position = SetTilePosition(i, j, "quest");
                g.transform.localScale = new Vector2(.4f, .4f);
                QuestDisplay[i, j] = g;
                g.transform.SetParent(QuestPage.transform, false);
            }
        }

        // Misc Page
        for (int i = 0; i < MiscDisplay.GetLength(0); i++)
        {
            for (int j = 0; j < MiscDisplay.GetLength(1); j++)
            {
                GameObject g = new GameObject();
                img = g.AddComponent<Image>();
                img.sprite = itemBoxSprite;
                Button b = g.AddComponent<Button>();
                //b.onClick.AddListener(() => SelectItem(g));
                g.AddComponent<ItemId>();
                g.transform.position = SetTilePosition(i, j, "misc");
                g.transform.localScale = new Vector2(.4f, .4f);
                MiscDisplay[i, j] = g;
                g.transform.SetParent(MiscPage.transform, false);
            }
        }

        //MapArrayToArray(((EquipmentPage)(player.GetComponent<PlayerInventory>().inventoryPages["Equipment"])).ArmorItems, ArmorDisplay);
        //MapArrayToArray(((EquipmentPage)(player.GetComponent<PlayerInventory>().inventoryPages["Equipment"])).WeaponItems, WeaponDisplay);
        //MapArrayToArray(((EquipmentPage)(player.GetComponent<PlayerInventory>().inventoryPages["Equipment"])).SkillItems, SkillDisplay);
        //MapListToArr(((QuestPage)(player.GetComponent<PlayerInventory>().inventoryPages["Quest"]))._items, QuestDisplay);
        //MapListToArr(((MiscPage)(player.GetComponent<PlayerInventory>().inventoryPages["Misc"]))._items, MiscDisplay);
    }
    private void GenerateButtons()
    {
        Button btn;
        Image img;

        // Generate the use button
        useButton = new GameObject();
        useButton.transform.parent = this.transform.parent;
        btn = useButton.AddComponent<Button>();
            //btn.onClick.AddListener(() => UseItem());
        useButton.AddComponent<CanvasGroup>();
        img = useButton.AddComponent<Image>();
        img.sprite = itemBoxSprite;
        useButton.transform.position = new Vector2(Screen.width - 100, Screen.height - 100);

        // Hide the use button.
        useButton.transform.GetComponent<CanvasGroup>().alpha = 0;
        useButton.transform.GetComponent<CanvasGroup>().interactable = false;

        // Find the main menu buttons and hook up the onClick events.
        menuButtons = this.gameObject.transform.Find("MenuButtons").gameObject;
        CanvasGroup menuCanvasGroup = menuButtons.AddComponent<CanvasGroup>();
        menuCanvasGroup.alpha = 1;
        menuCanvasGroup.interactable = true;
        menuCanvasGroup.blocksRaycasts = true;
        menuButtons.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => SwitchMenu("Equipment"));
        menuButtons.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => SwitchMenu("Misc"));
        menuButtons.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => SwitchMenu("Quest"));
        subMenuButtons = menuButtons.transform.GetChild(3).gameObject;

        // Find the sub menu buttons and hook up the onClick events.
        CanvasGroup subMenuCanvasGroup = subMenuButtons.AddComponent<CanvasGroup>();
        subMenuCanvasGroup.alpha = 1;
        subMenuCanvasGroup.interactable = true;
        subMenuCanvasGroup.blocksRaycasts = true;
        subMenuButtons.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => SwitchEquipmentMenu("Armor"));
        subMenuButtons.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(() => SwitchEquipmentMenu("Weapon"));
        subMenuButtons.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() => SwitchEquipmentMenu("Skill"));
    }

    /// <summary>
    /// Used by input manager to show the inventory.
    /// </summary>
    public void ShowInventory()
    {
        //MapArrayToArray(((EquipmentPage)(player.GetComponent<PlayerInventory>().inventoryPages["Equipment"])).ArmorItems, ArmorDisplay);
        //MapArrayToArray(((EquipmentPage)(player.GetComponent<PlayerInventory>().inventoryPages["Equipment"])).WeaponItems, WeaponDisplay);
        //MapArrayToArray(((EquipmentPage)(player.GetComponent<PlayerInventory>().inventoryPages["Equipment"])).SkillItems, SkillDisplay);
        //MapListToArr(((MiscPage)(player.GetComponent<PlayerInventory>().inventoryPages["Misc"]))._items, MiscDisplay);
        //MapListToArr(((QuestPage)(player.GetComponent<PlayerInventory>().inventoryPages["Quest"]))._items, QuestDisplay);

        selectedItem = null;

        showEquipment = !showEquipment;
        showArmorInEquipment = true;
        showWeaponInEquipment = false;
        showSkillInEquipment = false;
        showQuest = false;
        showMisc = false;

        // Activate
        staticImages.SetActive(true);
        equipmentPage.SetActive(true);
        ArmorPageInEquipment.SetActive(true);
        WeaponPageInEquipment.SetActive(false);
        SkillPageInEquipment.SetActive(false);
        menuButtons.SetActive(true);

        // Deactivate
        QuestPage.SetActive(false);
        MiscPage.SetActive(false);

        // Show the use button.
        useButton.transform.GetComponent<CanvasGroup>().alpha = 1;
        useButton.transform.GetComponent<CanvasGroup>().interactable = true;
    }
    /// <summary>
    /// Used by input manager to hide the inventory completely.
    /// </summary>
    public void HideInventory()
    {
        // Deactivate
        staticImages.SetActive(false);
        equipmentPage.SetActive(false);
        showEquipment = false;
        showArmorInEquipment = false;
        showWeaponInEquipment = false;
        showSkillInEquipment = false;
        QuestPage.SetActive(false);
        showQuest = false;
        MiscPage.SetActive(false);
        showMisc = false;
        menuButtons.SetActive(false);

        // Hide the use button.
        useButton.transform.GetComponent<CanvasGroup>().alpha = 0;
        useButton.transform.GetComponent<CanvasGroup>().interactable = false;
    }

    public void SwitchEquipmentMenu(string menuName)
    {
        if (!showEquipment) return;

        switch (menuName)
        {
            case "Armor":
                {
                    if (showArmorInEquipment) return;

                    showArmorInEquipment = true;
                    showWeaponInEquipment = false;
                    showSkillInEquipment = false;

                    ArmorPageInEquipment.SetActive(true);
                    WeaponPageInEquipment.SetActive(false);
                    SkillPageInEquipment.SetActive(false);
                    break;
                }
            case "Weapon":
                {
                    if (showWeaponInEquipment) return;

                    showWeaponInEquipment = true;
                    showArmorInEquipment = false;
                    showSkillInEquipment = false;

                    ArmorPageInEquipment.SetActive(false);
                    WeaponPageInEquipment.SetActive(true);
                    SkillPageInEquipment.SetActive(false);
                    break;
                }
            case "Skill":
                {
                    if (showSkillInEquipment) return;

                    showSkillInEquipment = true;
                    showArmorInEquipment = false;
                    showWeaponInEquipment = false;

                    ArmorPageInEquipment.SetActive(false);
                    WeaponPageInEquipment.SetActive(false);
                    SkillPageInEquipment.SetActive(true);
                    break;
                }
        }

        playUISoundEffect.Invoke();
    }
    /// <summary>
    /// Used by input manager to switch between inventory pages.
    /// </summary>
    public void SwitchMenu(string menuName)
    {
        switch (menuName)
        {
            case "Equipment":
                {
                    if (showEquipment) return;

                    selectedItem = null;

                    showEquipment = true;
                    showArmorInEquipment = true;
                    showWeaponInEquipment = false;
                    showSkillInEquipment = false;
                    showQuest = false;
                    showMisc = false;

                    // Activate
                    equipmentPage.SetActive(true);
                    ArmorPageInEquipment.SetActive(true);
                    subMenuButtons.SetActive(true);

                    // Deactivate
                    WeaponPageInEquipment.SetActive(false);
                    SkillPageInEquipment.SetActive(false);
                    QuestPage.SetActive(false);
                    MiscPage.SetActive(false);

                    break;
                }
            case "Quest":
                {
                    if (showQuest) return;

                    selectedItem = null;

                    showQuest = true;
                    showEquipment = false;
                    showMisc = false;

                    // Activate
                    QuestPage.SetActive(true);

                    // Deactivate
                    equipmentPage.SetActive(false);
                    MiscPage.SetActive(false);
                    subMenuButtons.SetActive(false);

                    break;
                }
            case "Misc":
                {
                    if (showMisc) return;

                    selectedItem = null;

                    showMisc = true;
                    showQuest = false;
                    showEquipment = false;

                    // Activate
                    MiscPage.SetActive(true);

                    // Deactivate
                    equipmentPage.SetActive(false);
                    QuestPage.SetActive(false);
                    subMenuButtons.SetActive(false);

                    break;
                }
        }

        playUISoundEffect.Invoke();
    }

    /// <summary>
    /// Set tile positions for inventory tiles.
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <returns></returns>
    private Vector2 SetTilePosition(int i, int j, string type)
    {
        switch (type)
        {
            case "armor":
            case "weapon":
                {
                    return new Vector2(843f + (j * 193), 650f - (i * 193));
                }
            case "skills":
                {
                    if (j == 0 || j == 1)
                    {
                        return new Vector2(843f + (j * 193), 650f - (i * 193));
                    }
                    else if (j == 2)
                    {
                        return new Vector2(843f + (j * 193), 650f - (i * 193) + 46f);
                    }
                    else if (j == 3)
                    {
                        j = 2;
                        return new Vector2(843f + (j * 193), 650f - (i * 193) - 46f);
                    }
                    // j = 4
                    else
                    {
                        return new Vector2(843f + ((j - 1) * 193), 650f - (i * 193));
                    }
                }
            case "misc":
            case "quest":
                {
                    return new Vector2(-28 + (j * 87), 92 - (i * 87));
                }
        }

        return Vector2.zero;
    }
    /// <summary>
    /// Helper method to map items in a page to the 2 dimensional array used to display items on screen.
    /// </summary>
    /// <param name="list"></param>
    /// <param name="arr"></param>
    private void MapListToArr(List<Item> list, GameObject[,] arr)
    {
        int totalItemsCount = list.Count;
        int currentListIndex = 0;
        for (int i = 0; i < arr.GetLength(0); i++)
        {
            for (int j = 0; j < arr.GetLength(1); j++)
            {
                if(currentListIndex < totalItemsCount)
                {
                    arr[i, j].GetComponent<Image>().sprite = itemBoxSprite;
                    // Item not already in this slot.
                    if (arr[i, j].transform.childCount == 0)
                    {
                        GameObject g = new GameObject();
                        g.AddComponent<Image>();
                        g.GetComponent<Image>().sprite = list[currentListIndex]._image;
                        g.transform.localScale = new Vector2(.9f, .9f);
                        g.transform.parent = arr[i, j].transform;
                        g.transform.position = arr[i, j].transform.position;
                    }
                    // Item already in this slot, so replace it.
                    else
                    {
                        arr[i, j].transform.GetChild(0).GetComponent<Image>().sprite = list[currentListIndex]._image;
                    }
                    arr[i, j].GetComponent<ItemId>().Id = currentListIndex++;
                }
                else
                {
                    arr[i, j].GetComponent<Image>().sprite = itemBoxSprite;
                    if (arr[i, j].transform.childCount == 1)
                    {
                        Destroy(arr[i, j].transform.GetChild(0).gameObject);
                    }
                    arr[i, j].GetComponent<ItemId>().Id = -1;
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="start"></param>
    /// <param name="end"></param>
    private void MapArrayToArray(Item[][] start, GameObject[,] end)
    {
        int count = 0;
        for(int i = 0; i < start.Length; i++)
        {
            for(int j = 0; j < start[i].Length; j++)
            {
                if (start[i][j] != null)
                {
                    end[i, j].GetComponent<Image>().sprite = itemBoxSprite;
                    // Item not already in this slot
                    if (end[i, j].transform.childCount == 0)
                    {
                        GameObject g = new GameObject();
                        g.AddComponent<Image>();
                        g.GetComponent<Image>().sprite = start[i][j]._image;
                        g.transform.localScale = new Vector2(.9f, .9f);
                        g.transform.parent = end[i, j].transform;
                        g.transform.position = end[i, j].transform.position;
                    }
                    // Item already in this slot, so replace it.
                    else
                    {
                        end[i, j].transform.GetChild(0).GetComponent<Image>().sprite = start[i][j]._image;
                    }
                    end[i, j].GetComponent<ItemId>().Id = count;
                }
                else
                {
                    end[i, j].GetComponent<Image>().sprite = itemBoxSprite;
                    if (end[i, j].transform.childCount == 1)
                    {
                        Destroy(end[i, j].transform.GetChild(0).gameObject);
                    }
                    end[i, j].GetComponent<ItemId>().Id = -1;
                }
                count++;
            }
        }
    }

    // Will Bertiz code START
    private void PlaySound() 
    {
        //FMODUnity.RuntimeManager.PlayOneShot("event:/UI/Menu/Change_Option_1");
    }
    // END
}
