using UnityEngine;

public static class ItemData
{
    public static Item CreateItem(int itemID)
    {
        string _name = "";
        string _description = "";
        int _value = 0;
        int _amount = 0;
        string _icon = "";
        string _mesh = "";
        ItemType _type = ItemType.Apparel;
        int _damage = 0;
        int _armour = 0;
        int _heal = 0;
        switch (itemID)
        {
            #region Food 0 - 99
            case 0:
                 _name = "Apple";
                 _description = "Munchies and Crunchies";
                 _value = 1;
                 _amount = 1;
                 _icon = "Food/Apple";
                 _mesh = "Food/Apple";
                 _type = ItemType.Food;
                _heal = 15;
                break;
            case 1:
                _name = "Meat";
                _description = "Steamed Hams";
                _value = 10;
                _amount = 1;
                _icon = "Food/Meat";
                _mesh = "Food/Meat";
                _type = ItemType.Food;
                _heal = 30;
                break;
            #endregion
            #region Weapon 100 - 199
            case 100:
                _name = "Axe";
                _description = "AXE is AXE";
                _value = 150;
                _amount = 1;
                _icon = "Weapon/Axe";
                _mesh = "Weapon/Axe";
                _type = ItemType.Weapon;
                _damage = 15;
                break;
            case 101:
                _name = "Bow";
                _description = "It's a bow";
                _value = 75;
                _amount = 1;
                _icon = "Weapon/Bow";
                _mesh = "Weapon/Bow";
                _type = ItemType.Weapon;
                _damage = 5;
                break;
            case 102:
                _name = "Sword";
                _description = "It's a sword";
                _value = 100;
                _amount = 1;
                _icon = "Weapon/Sword";
                _mesh = "Weapon/Sword";
                _type = ItemType.Weapon;
                _damage = 10;
                break;
            #endregion
            #region Apparel 200 - 299
            case 200:
                _name = "Armour";
                _description = "Armour for the body";
                _value = 200;
                _amount = 1;
                _icon = "Apparel/Armour/Armour";
                _mesh = "Apparel/Armour/Armour";
                _type = ItemType.Apparel;
                _armour = 30;
                break;
            case 201:
                _name = "Boots";
                _description = "Armour for the feet";
                _value = 100;
                _amount = 1;
                _icon = "Apparel/Armour/Boots";
                _mesh = "Apparel/Armour/Boots";
                _type = ItemType.Apparel;
                _armour = 10;
                break;
            case 202:
                _name = "Braces";
                _description = "Armour for the arms";
                _value = 100;
                _amount = 1;
                _icon = "Apparel/Armour/Braces";
                _mesh = "Apparel/Armour/Braces";
                _type = ItemType.Apparel;
                _armour = 5;
                break;
            case 203:
                _name = "Gloves";
                _description = "Armour for the hands";
                _value = 100;
                _amount = 1;
                _icon = "Apparel/Armour/Gloves";
                _mesh = "Apparel/Armour/Gloves";
                _type = ItemType.Apparel;
                _armour = 5;
                break;
            case 204:
                _name = "Helmet";
                _description = "Armour for the head";
                _value = 200;
                _amount = 1;
                _icon = "Apparel/Armour/Helmet";
                _mesh = "Apparel/Armour/Helmet";
                _type = ItemType.Apparel;
                _armour = 20;
                break;
            case 205:
                _name = "Pauldrons";
                _description = "Armour for the shoulders";
                _value = 170;
                _amount = 1;
                _icon = "Apparel/Armour/Pauldrons";
                _mesh = "Apparel/Armour/Pauldrons";
                _type = ItemType.Apparel;
                _armour = 15;
                break;
            case 206:
                _name = "Shield";
                _description = "Armour that you can throw";
                _value = 145;
                _amount = 1;
                _icon = "Apparel/Armour/Shield";
                _mesh = "Apparel/Armour/Shield";
                _type = ItemType.Apparel;
                _armour = 25;
                break;
            case 207:
                _name = "Belt";
                _description = "Armour for the waist";
                _value = 75;
                _amount = 1;
                _icon = "Apparel/Belt";
                _mesh = "Apparel/Belt";
                _type = ItemType.Apparel;
                _armour = 10;
                break;
            case 208:
                _name = "Cloak";
                _description = "Nice and warm";
                _value = 175;
                _amount = 1;
                _icon = "Apparel/Cloak";
                _mesh = "Apparel/Cloak";
                _type = ItemType.Apparel;
                _armour = 15;
                break;
            case 209:
                _name = "Necklace";
                _description = "Armour for the neck";
                _value = 50;
                _amount = 1;
                _icon = "Apparel/Necklace";
                _mesh = "Apparel/Necklace";
                _type = ItemType.Apparel;
                _armour = 5;
                break;
            case 210:
                _name = "Pants";
                _description = "Armour for the legs";
                _value = 200;
                _amount = 1;
                _icon = "Apparel/Pants";
                _mesh = "Apparel/Pants";
                _type = ItemType.Apparel;
                _armour = 25;
                break;
            case 211:
                _name = "Ring";
                _description = "Ring-shaped";
                _value = 25;
                _amount = 1;
                _icon = "Apparel/Ring";
                _mesh = "Apparel/Ring";
                _type = ItemType.Apparel;
                _armour = 5;
                break;
            #endregion
            #region Crafting 300 - 399
            case 300:
                _name = "Gem";
                _description = "Shiny";
                _value = 300;
                _amount = 1;
                _icon = "Crafting/Gem";
                _mesh = "Crafting/Gem";
                _type = ItemType.Crafting;
                break;
            case 301:
                _name = "Ingot";
                _description = "Heavy";
                _value = 100;
                _amount = 1;
                _icon = "Crafting/Ingot";
                _mesh = "Crafting/Ingot";
                _type = ItemType.Crafting;
                break;
            #endregion
            #region Ingredients 400 - 499
            #endregion
            #region Potions 500 - 599
            case 500:
                _name = "Health Potion";
                _description = "Good for your bones";
                _value = 150;
                _amount = 1;
                _icon = "Potions/HealthPotion";
                _mesh = "Potions/HealthPotion";
                _type = ItemType.Potions;
                break;
            case 501:
                _name = "Mana Potion";
                _description = "Good for your brain";
                _value = 350;
                _amount = 1;
                _icon = "Potions/ManaPotion";
                _mesh = "Potions/ManaPotion";
                _type = ItemType.Potions;
                break;
            #endregion
            #region Scrolls 600 - 699
            case 600:
                _name = "Book";
                _description = "Has words";
                _value = 50;
                _amount = 1;
                _icon = "Scrolls/Book";
                _mesh = "Scrolls/Book";
                _type = ItemType.Scrolls;
                break;
            case 601:
                _name = "Scroll";
                _description = "Book with no brim";
                _value = 50;
                _amount = 1;
                _icon = "Scrolls/Scroll";
                _mesh = "Scrolls/Scroll";
                _type = ItemType.Scrolls;
                break;
            #endregion
            #region Quest 700 - 799
            #endregion
            default:
                itemID = 0;
                _name = "Apple";
                _description = "Munchies and Crunchies";
                _value = 1;
                _amount = 1;
                _icon = "Food/Apple";
                _mesh = "Food/Apple";
                _type = ItemType.Food;
                _heal = 15;
                break;
        }
        Item temp = new Item
        {
            ID = itemID,
            Name = _name,
            Description = _description,
            Value = _value,
            Amount = _amount,
            Type = _type,
            Icon = Resources.Load("Icons/" + _icon) as Texture2D,
            Mesh = Resources.Load("Mesh/" + _mesh) as GameObject,
            Damage = _damage,
            Armour = _armour,
            Heal = _heal
        };
        return temp;
    }
}