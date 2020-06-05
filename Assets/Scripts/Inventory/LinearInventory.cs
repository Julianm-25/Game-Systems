using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LinearInventory : MonoBehaviour
{
    public PlayerHandler player;
    public static List<Item> inv = new List<Item>();
    public Item selectedItem;
    public static bool showInv;
    public GameObject invButtonPrefab;
    public Scrollbar invScroll;
    public GUIStyle style;
    public GUISkin skin;

    public Vector2 scr;
    public Vector2 scrollPos;
    public string sortType = "";
    public string[] enumTypesForItems;
    public static int money;

    public Transform dropLocation;
    [System.Serializable]
    public struct Equipment
    {
        public string slotName;
        public Transform equipLocation;
        public GameObject currentItem;
    };
    public Equipment[] equipmentSlots;
    public static Chest currentChest;
    public static Shop currentShop;
    void Start()
    {
        player = this.gameObject.GetComponent<PlayerHandler>();
        enumTypesForItems = new string[] { "All", "Food", "Weapon", "Apparel", "Crafting", "Ingredients", "Potions", "Scrolls", "Quest" };
        money = 1000;
    }
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.B))
        {
            inv.Add(ItemData.CreateItem(0));
            inv.Add(ItemData.CreateItem(1));
            inv.Add(ItemData.CreateItem(2));
            inv.Add(ItemData.CreateItem(101));
            inv.Add(ItemData.CreateItem(204));
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            sortType = "Food";
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            sortType = "All";
        }
#endif
        scr.x = Screen.width / 16;
        scr.y = Screen.height / 9;
        if(Input.GetKeyDown(KeyCode.Tab) && !PauseMenu.isPaused )
        {
            showInv = !showInv;
            if (showInv)
            {
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                return;
            }
            else
            {
                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                if(currentChest)
                {
                    currentChest.showChestInv = false;
                    currentChest = null;
                }
                if (currentShop)
                {
                    currentShop.showShopInv = false;
                    currentShop = null;
                }
                return;
            }
        }
    }
    void Display()
    {
        //if we want to display everything in our inventory
        if (sortType == "All" || sortType == "")
        {
            //if we have 34 or less (space at top and bottom)
            if (inv.Count <= 34)
            {
                for (int i = 0; i < inv.Count; i++)
                {
                    if (GUI.Button(new Rect(0.5f * scr.x, 0.25f * scr.y + i * (0.25f * scr.y), 3 * scr.x, 0.25f * scr.y), inv[i].Name))
                    {
                        selectedItem = inv[i];
                    }
                }
            }
            //more than 34 items
            else
            {
                //our movable scroll position
                scrollPos = GUI.BeginScrollView(new Rect(0, 0.25f * scr.y, 3.75f * scr.x, 8.5f * scr.y), scrollPos, new Rect(0, 0, 0, inv.Count * 0.25f * scr.y), false, true);
                for (int i = 0; i < inv.Count; i++)
                {
                    if (GUI.Button(new Rect(0.5f * scr.x, i * (0.25f * scr.y), 3 * scr.x, 0.25f * scr.y), inv[i].Name))
                    {
                        selectedItem = inv[i];
                    }
                }
                GUI.EndScrollView();
            }
        }
        //if we are displaying based on type
        else
        {
            ItemType type = (ItemType)System.Enum.Parse(typeof(ItemType), sortType);
            //amount of that type
            int a = 0;
            //slot position
            int s = 0;
            for (int i = 0; i < inv.Count; i++) //search everything in the inventory
            {
                if (inv[i].Type == type)//if a match is found
                {
                    a++;//increase the type count
                }
            }
            if (a <= 34)//if there are less than or equal to 34
            {
                for (int i = 0; i < inv.Count; i++)
                {
                    if (inv[i].Type == type)
                    {
                        if (GUI.Button(new Rect(0.5f * scr.x, s * (0.25f * scr.y), 3 * scr.x, 0.25f * scr.y), inv[i].Name))
                        {
                            selectedItem = inv[i];
                        }
                        s++;
                    }
                }
            }
            else
            {
                scrollPos = GUI.BeginScrollView(new Rect(0, 0.25f * scr.y, 3.75f * scr.x, 8.5f * scr.y), scrollPos, new Rect(0, 0, 0, a * 0.25f * scr.y), false, true);
                for (int i = 0; i < inv.Count; i++)
                {
                    if (inv[i].Type == type)
                    {
                        if (GUI.Button(new Rect(0.5f * scr.x, s * (0.25f * scr.y), 3 * scr.x, 0.25f * scr.y), inv[i].Name))
                        {
                            selectedItem = inv[i];
                        }
                        s++;
                    }
                }
                GUI.EndScrollView();
            }
        }


    }
    void GenerateInventory()
    {
        for (int i = 0; i < inv.Count; i++)
        {
            GameObject invButton = Instantiate(invButtonPrefab);
            invButton.transform.position = new Vector2(invScroll.transform.position.x, invScroll.transform.position.y - 30 * (i + 1));
            invButton.transform.SetParent(invScroll.transform, true);
            invButton.GetComponentInChildren<Text>().text = inv[i].Name;
            int value = i;
            //invButton.GetComponent<Button>().onClick.AddListener(delegate { SelectItem(value); });
        }
    }
    void UseItem()
    {
        GUI.Box(new Rect(4f * scr.x, 0.25f * scr.y, 3.5f * scr.x, 7f * scr.y), "", style);
        GUI.Box(new Rect(4.25f * scr.x, 0.5f * scr.y, 3f * scr.x, 3f * scr.y), selectedItem.Icon);
        GUI.skin = skin;
        GUI.Box(new Rect(4.55f * scr.x, 3.5f * scr.y, 2.5f * scr.x, 0.5f * scr.y), selectedItem.Name);
        GUI.skin = null;
        switch (selectedItem.Type)
        {
            case ItemType.Food:
                GUI.Box(new Rect(4.25f * scr.x, 4f * scr.y, 3 * scr.x, 3 * scr.y), selectedItem.Description + "\nValue: " + selectedItem.Value + "\nAmount: " + selectedItem.Amount);
                if (player.attributes[0].currentValue < player.attributes[0].maxValue)
                {
                    if (GUI.Button(new Rect(6.25f * scr.x, 6.75f * scr.y, scr.x, 0.25f * scr.y), "Eat"))
                    {
                        player.attributes[0].currentValue = Mathf.Clamp(player.attributes[0].currentValue += selectedItem.Heal, 0, player.attributes[0].maxValue);
                        if (selectedItem.Amount > 1)
                        {
                            selectedItem.Amount--;
                        }
                        else
                        {
                            inv.Remove(selectedItem);
                            selectedItem = null;
                            return;
                        }
                    }
                }
                break;
            case ItemType.Weapon:
                GUI.Box(new Rect(4.25f * scr.x, 4f * scr.y, 3 * scr.x, 3 * scr.y), selectedItem.Description + "\nValue: " + selectedItem.Value + "\nDamage: ");
                if (equipmentSlots[2].currentItem == null || selectedItem.Name != equipmentSlots[2].currentItem.name)
                {
                    if (GUI.Button(new Rect(6.25f * scr.x, 6.75f * scr.y, scr.x, 0.25f * scr.y), "Equip"))
                    {
                        //remove what is already equipped
                        if (equipmentSlots[2].currentItem != null)
                        {
                            Destroy(equipmentSlots[2].currentItem);
                        }
                        GameObject curItem = Instantiate(selectedItem.Mesh, equipmentSlots[2].equipLocation);
                        equipmentSlots[2].currentItem = curItem;
                        curItem.name = selectedItem.Name;
                    }
                }
                else
                {
                    if (GUI.Button(new Rect(6.25f * scr.x, 6.75f * scr.y, scr.x, 0.25f * scr.y), "Unequip"))
                    {
                        Destroy(equipmentSlots[2].currentItem);
                    }
                }
                break;
            case ItemType.Apparel:
                GUI.Box(new Rect(4.25f * scr.x, 4f * scr.y, 3 * scr.x, 3 * scr.y), selectedItem.Description + "\nValue: " + selectedItem.Value + "\nArmour: ");
                if (equipmentSlots[0].currentItem == null || selectedItem.Name != equipmentSlots[0].currentItem.name)
                {
                    if (GUI.Button(new Rect(6.25f * scr.x, 6.75f * scr.y, scr.x, 0.25f * scr.y), "Equip"))
                    {
                        //remove what is already equipped
                        if (equipmentSlots[0].currentItem != null)
                        {
                            Destroy(equipmentSlots[0].currentItem);
                        }
                        GameObject curItem = Instantiate(selectedItem.Mesh, equipmentSlots[0].equipLocation);
                        equipmentSlots[0].currentItem = curItem;
                        curItem.name = selectedItem.Name;
                    }
                }
                else
                {
                    if (GUI.Button(new Rect(6.25f * scr.x, 6.75f * scr.y, scr.x, 0.25f * scr.y), "Unequip"))
                    {
                        Destroy(equipmentSlots[0].currentItem);
                    }
                }
                break;
            case ItemType.Crafting:
                GUI.Box(new Rect(4.25f * scr.x, 4f * scr.y, 3 * scr.x, 3 * scr.y), selectedItem.Description + "\nValue: " + selectedItem.Value + "\nAmount: " + selectedItem.Amount);
                if (GUI.Button(new Rect(6.25f * scr.x, 6.75f * scr.y, scr.x, 0.25f * scr.y), "Use"))
                {

                }
                break;
            case ItemType.Ingredients:
                GUI.Box(new Rect(4.25f * scr.x, 4f * scr.y, 3 * scr.x, 3 * scr.y), selectedItem.Description + "\nValue: " + selectedItem.Value + "\nAmount: " + selectedItem.Amount);
                if (GUI.Button(new Rect(6.25f * scr.x, 6.75f * scr.y, scr.x, 0.25f * scr.y), "Use"))
                {

                }
                break;
            case ItemType.Potions:
                GUI.Box(new Rect(4.25f * scr.x, 4f * scr.y, 3 * scr.x, 3 * scr.y), selectedItem.Description + "\nValue: " + selectedItem.Value + "\nAmount: " + selectedItem.Amount);
                if (GUI.Button(new Rect(6.25f * scr.x, 6.75f * scr.y, scr.x, 0.25f * scr.y), "Drink"))
                {

                }
                break;
            case ItemType.Scrolls:
                GUI.Box(new Rect(4.25f * scr.x, 4f * scr.y, 3 * scr.x, 3 * scr.y), selectedItem.Description + "\nValue: " + selectedItem.Value + "\nAmount: " + selectedItem.Amount);
                if (GUI.Button(new Rect(6.25f * scr.x, 6.75f * scr.y, scr.x, 0.25f * scr.y), "Use"))
                {

                }
                break;
            case ItemType.Quest:
                break;
            case ItemType.Money:
                break;
            default:
                break;
        }
        GUI.skin = null;

        if (GUI.Button(new Rect(4.25f * scr.x, 6.75f * scr.y, scr.x, 0.25f * scr.y), "Discard"))
        {
            for (int i = 0; i < equipmentSlots.Length; i++)
            {
                //check slots
                if (equipmentSlots[i].currentItem != null && selectedItem.Name == equipmentSlots[i].currentItem.name)
                {
                    //Destroy the one in our equipment
                    Destroy(equipmentSlots[i].currentItem);
                }
            }
            //spawn the item
            GameObject droppedItem = Instantiate(selectedItem.Mesh, dropLocation.position, Quaternion.identity);
            droppedItem.name = selectedItem.Name;
            droppedItem.AddComponent<Rigidbody>().useGravity = true;
            droppedItem.GetComponent<ItemHandler>().enabled = true;
            if (selectedItem.Amount > 1)
            {
                selectedItem.Amount--;
            }
            else
            {
                inv.Remove(selectedItem);
                selectedItem = null;
                return;
            }
        }
        if(currentChest != null)
        {
            if (GUI.Button(new Rect(5.25f * scr.x, 6.75f * scr.y, scr.x, 0.25f * scr.y), "Move Item"))
            {
                for (int i = 0; i < equipmentSlots.Length; i++)
                {
                    //check slots
                    if (equipmentSlots[i].currentItem != null && selectedItem.Name == equipmentSlots[i].currentItem.name)
                    {
                        //Destroy the one in our equipment
                        Destroy(equipmentSlots[i].currentItem);
                    }
                }
                //spawn the item
                currentChest.chestInv.Add(selectedItem);
                if (selectedItem.Amount > 1)
                {
                    selectedItem.Amount--;
                }
                else
                {
                    inv.Remove(selectedItem);
                    selectedItem = null;
                    return;
                }
            }
        }
        if (currentShop != null)
        {
            if (GUI.Button(new Rect(5.25f * scr.x, 6.75f * scr.y, scr.x, 0.25f * scr.y), "Sell Item"))
            {
                for (int i = 0; i < equipmentSlots.Length; i++)
                {
                    //check slots
                    if (equipmentSlots[i].currentItem != null && selectedItem.Name == equipmentSlots[i].currentItem.name)
                    {
                        //Destroy the one in our equipment
                        Destroy(equipmentSlots[i].currentItem);
                    }
                }
                //spawn the item
                money += selectedItem.Value - (selectedItem.Value / currentShop.valueModifier);
                currentShop.shopInv.Add(selectedItem);
                if (selectedItem.Amount > 1)
                {
                    selectedItem.Amount--;
                }
                else
                {
                    inv.Remove(selectedItem);
                    selectedItem = null;
                    return;
                }
            }
        }
    }
    public void Sort(string itemType)
    {
        sortType = itemType;
    }
    void SelectItem()
    {

    }
    private void OnGUI()
    {
        if (showInv)
        {
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
            for (int i = 0; i < enumTypesForItems.Length; i++)
            {
                if (GUI.Button(new Rect(4f * scr.x + i * scr.x, 0, scr.x, 0.25f * scr.y), enumTypesForItems[i]))
                {
                    sortType = enumTypesForItems[i];
                }
            }
            Display();
            if (selectedItem != null)
            {
                UseItem();
            }
        }
    }
}
