using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class LinearInventory : MonoBehaviour
{
    public PlayerHandler player;
    #region Inventory bits
    public static List<Item> inv = new List<Item>();
    public Item selectedItem;
    public int shopChestSelectedItem;
    public static bool showInv;
    public GameObject invButtonPrefab;
    public GameObject invScroll;
    public GUIStyle style;
    public GUISkin skin;
    public RawImage invIcon;
    public Text invName, invDescription, invValue;
    public GameObject playerInvWindow;
    public GameObject inventory;
    public GameObject inventoryPanel;
    public GameObject eatButton;
    public GameObject equipButton;
    public GameObject offhandButton;
    public GameObject sortButtons;
    public GameObject sellButton;
    public GameObject storeButton;
    public GameObject takeButton;
    public GameObject buyButton;
    public GameObject shopChestScroll;
    public RawImage shopChestIcon;
    public Text shopChestName, shopChestDescription, shopChestValue;
    public GameObject shopChestWindow;
    public GameObject shopChestInv;
    public GameObject hotbarButton;
    #endregion

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
            AddItem(0);
            AddItem(0);
            AddItem(101);
            AddItem(102);
            AddItem(204);
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
        if (Input.GetKeyDown(KeyCode.Tab) && !PauseMenu.isPaused)
        {
            GenerateInventory();
            showInv = !showInv;
            if (showInv)
            {
                inventory.SetActive(true);
                sortButtons.SetActive(true);
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                return;
            }
            else
            {
                inventory.SetActive(false);
                playerInvWindow.SetActive(false);
                sortButtons.SetActive(false);
                shopChestWindow.SetActive(false);
                shopChestInv.SetActive(false);

                Time.timeScale = 1;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                if (currentChest)
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
    /*void Display()
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


    }*/
    #region Player Inventory
    public void GenerateInventory()
    {
        //getting rid of stuff at the start
        for (int i = invScroll.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(invScroll.transform.GetChild(i).gameObject);
        }
        //adding for each item
        for (int i = 0; i < inv.Count; i++)
        {
            bool sortCorrect = false;
            switch (sortType)
            {
                case "Food":
                    if (inv[i].Type == ItemType.Food)
                    {
                        sortCorrect = true;
                    }
                    break;
                case "Weapon":
                    if (inv[i].Type == ItemType.Weapon)
                    {
                        sortCorrect = true;
                    }
                    break;
                case "Apparel":
                    if (inv[i].Type == ItemType.Apparel)
                    {
                        sortCorrect = true;
                    }
                    break;
                case "Crafting":
                    if (inv[i].Type == ItemType.Crafting)
                    {
                        sortCorrect = true;
                    }
                    break;
                case "Ingredients":
                    if (inv[i].Type == ItemType.Ingredients)
                    {
                        sortCorrect = true;
                    }
                    break;
                case "Potions":
                    if (inv[i].Type == ItemType.Potions)
                    {
                        sortCorrect = true;
                    }
                    break;
                case "Scrolls":
                    if (inv[i].Type == ItemType.Scrolls)
                    {
                        sortCorrect = true;
                    }
                    break;
                case "Quest":
                    if (inv[i].Type == ItemType.Quest)
                    {
                        sortCorrect = true;
                    }
                    break;
                default:
                    sortCorrect = true;
                    break;
            }
            if (sortCorrect)
            {
                GameObject invButton = Instantiate(invButtonPrefab);
                invButton.transform.position = new Vector2(invScroll.transform.position.x, invScroll.transform.position.y - 30 * (i + 1));
                invButton.transform.SetParent(invScroll.transform, true);
                invButton.GetComponentInChildren<Text>().text = inv[i].Name;
                int value = i;
                invButton.GetComponent<Button>().onClick.AddListener(delegate { SelectItem(value); });
            }
        }
    }
    /*void UseItem()
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
        if (currentChest != null)
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
    }*/
    public void Sort(string itemType)
    {
        sortType = itemType;
        GenerateInventory();
    }
    void SelectItem(int invItem)
    {
        selectedItem = inv[invItem];
        playerInvWindow.SetActive(true);
        invIcon.texture = inv[invItem].Icon;
        invName.text = inv[invItem].Name + " x" + inv[invItem].Amount;
        invDescription.text = inv[invItem].Description;
        invValue.text = "Value: " + inv[invItem].Value.ToString();
        if (selectedItem.Type == ItemType.Food)
        {
            eatButton.SetActive(true);
            equipButton.SetActive(false);
            offhandButton.SetActive(false);
            hotbarButton.SetActive(true);
        }
        else if (selectedItem.Type == ItemType.Apparel)
        {
            equipButton.SetActive(true);
            offhandButton.SetActive(false);
            eatButton.SetActive(false);
            hotbarButton.SetActive(false);
        }
        else if (selectedItem.Type == ItemType.Weapon)
        {
            equipButton.SetActive(true);
            offhandButton.SetActive(true);
            eatButton.SetActive(false);
            hotbarButton.SetActive(false);
        }
        else
        {
            equipButton.SetActive(false);
            offhandButton.SetActive(false);
            eatButton.SetActive(false);
            hotbarButton.SetActive(false);
        }
        if(currentChest)
        {
            storeButton.SetActive(true);
            sellButton.SetActive(false);
        }
        else if(currentShop)
        {
            storeButton.SetActive(false);
            sellButton.SetActive(true);
        }
    }
    public void AddItem(int itemID)
    {
        bool stacking = false;
        for (int i = 0; i < inv.Count; i++)
        {
            if (inv[i].ID == itemID)
            {
                if (inv[i].Type != ItemType.Weapon && inv[i].Type != ItemType.Apparel)
                {
                    inv[i].Amount++;
                    stacking = true;
                }
            }
        }
        if (!stacking)
        {
            inv.Add(ItemData.CreateItem(itemID));
        }
        GenerateInventory();
    }
    public void RemoveItem()
    {
        if (selectedItem.Amount <= 1)
        {
            inv.Remove(selectedItem);
            playerInvWindow.SetActive(false);
        }
        else
        {
            selectedItem.Amount--;
            invName.text = selectedItem.Name + " x" + selectedItem.Amount;
        }

    }
    public void Drop()
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
        RemoveItem();
        GenerateInventory();
    }
    public void Eat()
    {
        if (player.attributes[0].currentValue < player.attributes[0].maxValue)
        {
            player.attributes[0].currentValue = Mathf.Clamp(player.attributes[0].currentValue += selectedItem.Heal, 0, player.attributes[0].maxValue);
            RemoveItem();
        }
        GenerateInventory();
    }
    public void AddToHotbar()
    {
        if (selectedItem.ID == 0)
        {
            GetComponent<Hotbar>().hotbarOne.SetActive(true);
        }
        else if (selectedItem.ID == 1)
        {
            GetComponent<Hotbar>().hotbarTwo.SetActive(true);
        }
    }
    public void Equip()
    {
        if (selectedItem.Type == ItemType.Weapon)
        {
            if (equipmentSlots[2].currentItem == null || selectedItem.Name != equipmentSlots[2].currentItem.name)
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
            else
            {
                Destroy(equipmentSlots[2].currentItem);
            }
        }
        else if (selectedItem.Type == ItemType.Apparel)
        {
            if (equipmentSlots[0].currentItem == null || selectedItem.Name != equipmentSlots[0].currentItem.name)
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
            else
            {
                Destroy(equipmentSlots[0].currentItem);
            }
        }
        GenerateInventory();
    }
    public void OffhandEquip()
    {
        if (selectedItem.Type == ItemType.Weapon)
        {
            if (equipmentSlots[1].currentItem == null || selectedItem.Name != equipmentSlots[1].currentItem.name)
            {
                //remove what is already equipped
                if (equipmentSlots[1].currentItem != null)
                {
                    Destroy(equipmentSlots[1].currentItem);
                }
                GameObject curItem = Instantiate(selectedItem.Mesh, equipmentSlots[1].equipLocation);
                equipmentSlots[1].currentItem = curItem;
                curItem.name = selectedItem.Name;
            }
            else
            {
                Destroy(equipmentSlots[1].currentItem);
            }
        }
        GenerateInventory();
    }
    #endregion
    #region Shop & Chest Inventory
    public void GenerateShopChest()
    {
        //getting rid of stuff at the start
        for (int i = shopChestScroll.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(shopChestScroll.transform.GetChild(i).gameObject);
        }
        //adding for each item
        if(currentChest)
        {
            for (int i = 0; i < currentChest.chestInv.Count; i++)
            {
                GameObject invButton = Instantiate(invButtonPrefab);
                invButton.transform.position = new Vector2(shopChestScroll.transform.position.x, shopChestScroll.transform.position.y - 30 * (i + 1));
                invButton.transform.SetParent(shopChestScroll.transform, true);
                invButton.GetComponentInChildren<Text>().text = currentChest.chestInv[i].Name;
                int value = i;
                invButton.GetComponent<Button>().onClick.AddListener(delegate { SelectShopChest(value); });
                shopChestWindow.SetActive(true);
            }
        }
        else if(currentShop)
        {
            for (int i = 0; i < currentShop.shopInv.Count; i++)
            {
                GameObject invButton = Instantiate(invButtonPrefab);
                invButton.transform.position = new Vector2(shopChestScroll.transform.position.x, shopChestScroll.transform.position.y - 30 * (i + 1));
                invButton.transform.SetParent(shopChestScroll.transform, true);
                invButton.GetComponentInChildren<Text>().text = currentShop.shopInv[i].Name;
                int value = i;
                invButton.GetComponent<Button>().onClick.AddListener(delegate { SelectShopChest(value); });
                shopChestWindow.SetActive(true);
            }
        }
        
    }
    void SelectShopChest(int shopChestItem)
    {
        shopChestSelectedItem = shopChestItem;
        shopChestInv.SetActive(true);
        if(currentChest)
        {
            takeButton.SetActive(true);
            buyButton.SetActive(false);
            shopChestValue.text = "Value: " + currentChest.chestInv[shopChestItem].Value.ToString();
            shopChestIcon.texture = currentChest.chestInv[shopChestItem].Icon;
            shopChestName.text = currentChest.chestInv[shopChestItem].Name + " x" + currentChest.chestInv[shopChestItem].Amount;
            shopChestDescription.text = currentChest.chestInv[shopChestItem].Description;
        }
        else if(currentShop)
        {
            takeButton.SetActive(false);
            buyButton.SetActive(true);
            shopChestValue.text = "Value: " + (currentShop.shopInv[shopChestSelectedItem].Value + (currentShop.shopInv[shopChestSelectedItem].Value / currentShop.valueModifier)).ToString();
            shopChestIcon.texture = currentShop.shopInv[shopChestItem].Icon;
            shopChestName.text = currentShop.shopInv[shopChestItem].Name + " x" + currentShop.shopInv[shopChestItem].Amount;
            shopChestDescription.text = currentShop.shopInv[shopChestItem].Description;
        }
    }
    public void TakeItem()
    {
        AddItem(currentChest.chestInv[shopChestSelectedItem].ID);
        currentChest.chestInv.RemoveAt(shopChestSelectedItem);
        shopChestInv.SetActive(false);
        GenerateInventory();
        GenerateShopChest();
    }
    public void BuyItem()
    {
        if (money >= currentShop.shopInv[shopChestSelectedItem].Value + (currentShop.shopInv[shopChestSelectedItem].Value / currentShop.valueModifier))
        {
            money -= currentShop.shopInv[shopChestSelectedItem].Value + (currentShop.shopInv[shopChestSelectedItem].Value / currentShop.valueModifier);
            AddItem(currentShop.shopInv[shopChestSelectedItem].ID);
            currentShop.shopInv.RemoveAt(shopChestSelectedItem);
            shopChestInv.SetActive(false);
            GenerateInventory();
            GenerateShopChest();
        }
    }
    public void StoreItem()
    {
        currentChest.chestInv.Add(selectedItem);
        RemoveItem();
        playerInvWindow.SetActive(false);
        GenerateInventory();
        GenerateShopChest();
    }
    public void SellItem()
    {
        money += selectedItem.Value - (selectedItem.Value / currentShop.valueModifier);
        currentShop.shopInv.Add(selectedItem);
        RemoveItem();
        playerInvWindow.SetActive(false);
        GenerateInventory();
        GenerateShopChest();
        money += currentShop.shopInv[shopChestSelectedItem].Value - (currentShop.shopInv[shopChestSelectedItem].Value / currentShop.valueModifier);
    }
    #endregion
    /*private void OnGUI()
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
    }*/
}
