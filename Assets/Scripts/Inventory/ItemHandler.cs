using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    public int itemId;
    public ItemType itemType;
    public int amount;

    private void Start()
    {
        
    }
    public void OnCollection()
    {
        if (itemType == ItemType.Money)
        {
            LinearInventory.money += amount;
        }
        else if(itemType == ItemType.Weapon || itemType == ItemType.Apparel || itemType == ItemType.Quest)//Weapon, Apparel, Quest
        {
            LinearInventory.inv.Add(ItemData.CreateItem(itemId));
        }
        else //Food, Crafting, Ingredients, Potions, Scrolls
        {
            int found = 0;
            int addIndex = 0;
            for(int i = 0; i < LinearInventory.inv.Count; i++)
            {
                if(itemId == LinearInventory.inv[i].ID)
                {
                    found = 1;
                    addIndex = i;
                    break;
                }
            }
            if (found == 1)
            {
                LinearInventory.inv[addIndex].Amount += amount;
            }
            else
            {

            }LinearInventory.inv.Add(ItemData.CreateItem(itemId));
            if(amount > 1)
            {
                for (int i = 0; i < LinearInventory.inv.Count; i++)
                {
                    if(itemId == LinearInventory.inv[i].ID)
                    {
                        LinearInventory.inv[i].Amount = amount;
                    }
                }
            }
        }
        Destroy(gameObject);
    }
}
