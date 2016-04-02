using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Assets.Resources.Scripts;
using Random = UnityEngine.Random;


// This struct is used to contain Stat/StatAmount pair
[Serializable]
public struct StatStruct
{
    public CharacterStat stat;
    public double StatAmount;
}


//This class is used for deserializing list of legendary items' names
[Serializable]
public class DiabloItems
{
    public ItemQuality Quality;
    public ItemType Type;
    public string Name;
}


/*
 * This class contains definition for all Items. 
 * It stores Name, Quality, Type, position in the inventory(if it's equipped or not put in the inventory, then position will be -1, -1),
 * amount and type of rolled stats, path to the icon, sell price and amount of armor
 */

[Serializable]
public class InventoryItem
{
    public string Name { get; private set; }
    public ItemQuality Quality { get; private set; }

    public ItemType Type { get; private set; }
    public Point LeftTopCornerCoordinate { get; set; }
    public int Size { get; private set; }

    public StatStruct[] itemStats { get; private set;}

    public string IconPath { get; private set;}

    public int SellValue { get; private set; }

    public int Armor { get; private set; }

    public InventoryItem(string name, ItemQuality quality, ItemType type, int size, StatStruct[] stats, int armor, int sellValue)
    {
        Name = name;
        Quality = quality;
        Type = type;
        Size = size;
        itemStats = stats;
        Armor = armor;
        SellValue = sellValue;
    }
}


//This class is used to generate new loot(after killing monsters, completing quest etc)
public class LootGenerator
{
    private static LootGenerator currentGenerator;
    public static LootGenerator CurrentGenerator
    {
        get
        {
            if (currentGenerator == null)
            {
                currentGenerator = new LootGenerator();
            }
            return currentGenerator;
        }
        set { currentGenerator = value; }
    }

    private List<DiabloItems> namesList = new List<DiabloItems>(); 

    private LootGenerator()
    {

        var sr = new XmlSerializer(typeof(List<DiabloItems>));
        namesList = sr.Deserialize(new FileStream("ItemsList.xml", FileMode.Open)) as List<DiabloItems>;
    }

    public InventoryItem GenerateNewItem()
    {
        ItemQuality quality;
        ItemType type = ItemType.SwingBlade;
        StatStruct[] stats;
        string name = string.Empty;
        int size;
        var qualityRoll = Random.Range(0, 100);
        if (qualityRoll <= 50)
            quality = ItemQuality.Rare;
        else if(qualityRoll > 50 && qualityRoll <= 70)
            quality = ItemQuality.Legendary;
        else if (qualityRoll > 70 && qualityRoll <= 80)
            quality = ItemQuality.Set;
        else
            return default(InventoryItem);

        bool flag = false;
        while (!flag) // check if there is a name for an item with such type and quality. If there is no name for that time, then we'll look for another type
        {
            var itemTypeRoll = Random.Range(0, 41);
            type = (ItemType) itemTypeRoll;

            if (quality == ItemQuality.Rare)
                flag = true;
            else if (namesList.Any(z => z.Quality == quality && z.Type == type))
                flag = true;
        }

        switch (type) //only accessories have 1 slot size
        {
            case ItemType.Amulet:
            case ItemType.Belt:
            case ItemType.MightyBelt:
            case ItemType.Ring:
                size = 1;
                break;
            default:
                size = 2;
                break;
        }

        if (quality != ItemQuality.Rare) // Rare items have another logic for name generation
        {
            var possibleNames = namesList.Where(z => z.Quality == quality && z.Type == type).ToList();
            var nameRoll = Random.Range(0, possibleNames.Count);
            name = possibleNames[nameRoll].Name;
        }

        var amountOfStats = Random.Range(2, 7);
        if (quality == ItemQuality.Legendary || quality == ItemQuality.Set)
            amountOfStats = 6;

        stats = new StatStruct[amountOfStats];
        for (int i = 0; i < amountOfStats; i++)
        {
            CharacterStat stat = CharacterStat.MainStat;
            flag = false;
            while (!flag) // if item can't have that type of stat for some reason, then we'll generate another stat until we'll have an appropriate one
            {
                var statRoll = Random.Range(0, 19);
                stat = (CharacterStat) statRoll;

                if ((stat == CharacterStat.Intelligence || stat == CharacterStat.Dexterity ||stat == CharacterStat.Strength) &&
                    stats.Any(z => z.stat == CharacterStat.Dexterity || z.stat == CharacterStat.Intelligence || z.stat == CharacterStat.Strength))
                    continue;
                if (!stats.Any(z => z.stat == stat))
                    flag = true;
            }
            double amountofStat = 0;
            int maxStatAmount = 0;
            switch (stat)
            {
                case CharacterStat.Block:
                case CharacterStat.CritChance:
                    maxStatAmount = Convert.ToInt32(Math.Pow(Account.CurrentAccount.GetCurrentHero().Level, 2)/2);
                    amountofStat = Random.Range(maxStatAmount/3 * 2, maxStatAmount);
                    break;
                case CharacterStat.BotDamage:
                    maxStatAmount =
                        Convert.ToInt32(Math.Pow(100 + Account.CurrentAccount.GetCurrentHero().Level, 2));
                    amountofStat = Random.Range(maxStatAmount/3*2, maxStatAmount)*0.001;
                    break;
                case CharacterStat.CritDamage:
                    break;
                case CharacterStat.ResistAll:
                    break;
                case CharacterStat.Life:
                    amountofStat = Random.Range(1, Account.CurrentAccount.GetCurrentHero().Level)*0.5;
                    break;
                case CharacterStat.Regen:
                    break;
                case CharacterStat.LifePerHit:
                    break;
                case CharacterStat.Sockets: //item can contain no more than 3 sockets
                    maxStatAmount = Convert.ToInt32(1 + Account.CurrentAccount.GetCurrentHero().Level/20) > 3
                        ? 3
                        : Convert.ToInt32(1 + Account.CurrentAccount.GetCurrentHero().Level/20);
                    amountofStat = Random.Range(1, maxStatAmount);
                    break;
                case CharacterStat.DamageAgainstElites:
                    break;
                case CharacterStat.DamageReduction:
                    break;
                case CharacterStat.ExtraGold:
                    break;
                case CharacterStat.ExtraExperience:
                    break;
                case CharacterStat.Thorns:
                    break;
                case CharacterStat.BotAttackSpeed:
                    break;
                case CharacterStat.Vitality: //all 'main' and vitality stats have the same logic for rolling
                case CharacterStat.Intelligence:
                case CharacterStat.Strength:
                case CharacterStat.Dexterity:
                    maxStatAmount = Convert.ToInt32((Math.Pow(Account.CurrentAccount.GetCurrentHero().Level, 2)));
                    amountofStat = Random.Range(maxStatAmount/3*2, maxStatAmount);
                    if (amountofStat == 0)
                        amountofStat = 1;
                    break;
            }
            stats[i] = new StatStruct() { stat = stat, StatAmount = amountofStat };
        }

        var Armor = 0;
        if (type != ItemType.Amulet && type != ItemType.Belt && type != ItemType.Ring && type != ItemType.MightyBelt) //there is no armor on accessories
            Armor = Random.Range((Account.CurrentAccount.GetCurrentHero().Level*5/3)*2,
            Account.CurrentAccount.GetCurrentHero().Level*5);

        var sellValue = 0;
        switch (quality)
        {
            case ItemQuality.Rare:
                sellValue = Convert.ToInt32((10 + stats.Length*5)*Account.CurrentAccount.GetCurrentHero().Level*0.1);
                break;
            case ItemQuality.Legendary:
                sellValue = Convert.ToInt32((25 + stats.Length * 5) * Account.CurrentAccount.GetCurrentHero().Level * 0.1);
                break;
            case ItemQuality.Set:
                sellValue = Convert.ToInt32((50 + stats.Length * 5) * Account.CurrentAccount.GetCurrentHero().Level * 0.1);
                break;
        }

        return new InventoryItem(name, quality, type, size, stats, Armor, sellValue);
    }
}

//Class which is used for storing items and also making some magic with those items(adding them to inventory, removing them from it etc)
[Serializable]
public class InventoryClass
{
    public delegate void InventoryChanged();

    [field: NonSerialized]
    public event InventoryChanged InventoryChangedEvent; // That event is used to get everyone know that something changed. Useful for re-drawing inventory

    bool[,] inventorySpace = new bool[12, 10]; //this array represents simple map of inventory where false is free slot and true is occupied one
    List<InventoryItem> inventory = new List<InventoryItem>();

    public ReadOnlyCollection<InventoryItem> Inventory
    {
        get { return inventory.AsReadOnly(); } // returning ReadOnly collection allows us to iterate through it without being afraid to change something outside that class
    }

    public bool[,] InventorySpace
    {
        get { return (bool[,]) inventorySpace.Clone(); } //returning clone of that array helps us not afraid of changing something from the outside
    }


    //This method is looking for the first appearance of free slot which can contain our item. Return pair x,y 
    public Point CheckForFreeSlot(int size)
    {
        for (int i = 0; i < inventorySpace.GetLength(0); i++)
        {
            for (int j = 0; j < inventorySpace.GetLength(1); j++)
            {
                if (!inventorySpace[i, j])
                {
                    if (size == 1) return new Point() {x = i, y = j};
                    if (j + 1 >= inventorySpace.GetLength(1)) continue;
                    if (!inventorySpace[i, j + 1]) return new Point() {x = i, y = j};
                }
            }
        }
        return new Point() {x = -1, y = -1};
    }

    //This method is recalculate the map of the inventory
    private void CheckSlots()
    {
        for (int i = 0; i < inventorySpace.GetLength(0); i++)
        {
            for (int j = 0; j < inventorySpace.GetLength(1); j++)
            {
                inventorySpace[i, j] = false;
            }
        }
        foreach (var item in inventory)
        {
            if (item.LeftTopCornerCoordinate.x < 0 || item.LeftTopCornerCoordinate.x > inventorySpace.GetLength(0)) continue;
            if (item.LeftTopCornerCoordinate.y < 0 || item.LeftTopCornerCoordinate.y > inventorySpace.GetLength(1)) continue;

            inventorySpace[item.LeftTopCornerCoordinate.x, item.LeftTopCornerCoordinate.y] = true;
            if (item.Size == 2 && item.LeftTopCornerCoordinate.y + 1 < inventorySpace.GetLength(1))
                inventorySpace[item.LeftTopCornerCoordinate.x, item.LeftTopCornerCoordinate.y + 1] = true;
        }
    }

    //Useful for adding items to the inventory. Checks for free slot, if there is one then it'll add it to the inventory collection. 
    public void AddItemToInventory(InventoryItem item)
    {
        var slot = CheckForFreeSlot(item.Size);
        if (slot.x == -1 && slot.y == -1) return;
        item.LeftTopCornerCoordinate = slot;
        inventory.Add(item);
        CheckSlots();

        if (InventoryChangedEvent != null)
            InventoryChangedEvent.Invoke();
    }


    //Useful for removing items from the inventory. Calls for recalculate inventory map after compliting
    public void RemoveFromInventory(Point slot)
    {
        if (inventorySpace[slot.x, slot.y])
        {
            inventory.Remove(inventory.FirstOrDefault(z => z.LeftTopCornerCoordinate.x == slot.x && z.LeftTopCornerCoordinate.y == slot.y));
        }
        CheckSlots();

        if (InventoryChangedEvent != null)
            InventoryChangedEvent.Invoke();
    }


    //TODO: Do something with that sheet. Too long and hard to read.

    //This class checks if there is a possibility for the hero to equip the item. If it is then we'll call the method for equiping it.
    //I tried to sort them in some kind of order which depends on slot type
    public void EquipItem(Point slot)
    {
        var item =
            inventory.FirstOrDefault(z => z.LeftTopCornerCoordinate.x == slot.x && z.LeftTopCornerCoordinate.y == slot.y);
        if(item == null) return;
        switch (item.Type)
        {
            case ItemType.WizardHat:
                if (Account.CurrentAccount.GetCurrentHero().Class != GameClasses.Wizard) return;
                EquipItemToSlot(0, item);
                break;
            case ItemType.VoodooMask:
                if (Account.CurrentAccount.GetCurrentHero().Class != GameClasses.WitchDoctor) return;
                EquipItemToSlot(0, item);
                break;
            case ItemType.SpiritStone: 
                if (Account.CurrentAccount.GetCurrentHero().Class != GameClasses.Monk) return;
                EquipItemToSlot(0, item);
                break;
            case ItemType.Helm:
                EquipItemToSlot(0, item);
                break;


            case ItemType.Shoulders:
                EquipItemToSlot(1, item);
                break;


            case ItemType.Amulet:
                EquipItemToSlot(2, item);
                break;


            case ItemType.Cloak:
                if (Account.CurrentAccount.GetCurrentHero().Class != GameClasses.DemonHunter) return;
                EquipItemToSlot(3, item);
                break;
            case ItemType.Chest:
                EquipItemToSlot(3, item);
                break;

            case ItemType.Gloves:
                EquipItemToSlot(4, item);
                break;

            case ItemType.Bracers:
                EquipItemToSlot(5, item);
                break;

            case ItemType.MightyBelt:
                if (Account.CurrentAccount.GetCurrentHero().Class != GameClasses.Barbarian) return;
                EquipItemToSlot(6, item);
                break;
            case ItemType.Belt:
                EquipItemToSlot(6, item);
                break;


            case ItemType.Ring:
                EquipItemToSlot(7, item);
                break;


            case ItemType.Pants:
                EquipItemToSlot(9, item);
                break;


            case ItemType.Boots:
                EquipItemToSlot(10, item);
                break;


            case ItemType.Crossbow2H:
                if (Account.CurrentAccount.GetCurrentHero().Class != GameClasses.DemonHunter) return;
                EquipItemToSlot(11, item, true);
                break;
            case ItemType.Bow:
                EquipItemToSlot(11, item, true);
                break;
            case ItemType.Flail2H:
                if (Account.CurrentAccount.GetCurrentHero().Class != GameClasses.Crusader) return;
                EquipItemToSlot(11, item, true);
                break;
            case ItemType.Mace2H:
                if (Account.CurrentAccount.GetCurrentHero().Class == GameClasses.DemonHunter) return;
                EquipItemToSlot(11, item, true);
                break;
            case ItemType.MightyWeapon2H:
                if (Account.CurrentAccount.GetCurrentHero().Class != GameClasses.Barbarian) return;
                EquipItemToSlot(11, item, true);
                break;
            case ItemType.Sword2H:
                if (Account.CurrentAccount.GetCurrentHero().Class == GameClasses.DemonHunter) return;
                EquipItemToSlot(11, item, true);
                break;
            case ItemType.Daibo:
                if (Account.CurrentAccount.GetCurrentHero().Class != GameClasses.Monk) return;
                EquipItemToSlot(11, item, true);
                break;
            case ItemType.Polearm:
                if (Account.CurrentAccount.GetCurrentHero().Class == GameClasses.DemonHunter) return;
                EquipItemToSlot(11, item, true);
                break;
            case ItemType.Spear:
                if (Account.CurrentAccount.GetCurrentHero().Class == GameClasses.DemonHunter) return;
                EquipItemToSlot(11, item, true);
                break;
            case ItemType.Staff:
                if (Account.CurrentAccount.GetCurrentHero().Class == GameClasses.DemonHunter) return;
                EquipItemToSlot(11, item, true);
                break;
            case ItemType.Axe2H:
                if (Account.CurrentAccount.GetCurrentHero().Class == GameClasses.DemonHunter) return;
                EquipItemToSlot(11, item, true);
                break;




            case ItemType.CeremonialKnife:
                if (Account.CurrentAccount.GetCurrentHero().Class != GameClasses.WitchDoctor) return;
                EquipItemToSlot(11, item);
                break;
            case ItemType.Crossbow1H:
                if (Account.CurrentAccount.GetCurrentHero().Class != GameClasses.DemonHunter) return;
                EquipItemToSlot(11, item);
                break;
            case ItemType.Dagger:
                EquipItemToSlot(11, item);
                break;
            case ItemType.Fist:
                if (Account.CurrentAccount.GetCurrentHero().Class != GameClasses.Monk) return;
                EquipItemToSlot(11, item);
                break;
            case ItemType.Flail1H: 
                if (Account.CurrentAccount.GetCurrentHero().Class != GameClasses.Crusader) return;
                EquipItemToSlot(11, item);
                break;
            case ItemType.Mace1H:
                EquipItemToSlot(11, item);
                break;
            case ItemType.MightyWeapon1H:
                if (Account.CurrentAccount.GetCurrentHero().Class != GameClasses.Barbarian) return;
                EquipItemToSlot(11, item);
                break;
            case ItemType.Sword1H:
                EquipItemToSlot(11, item);
                break;
            case ItemType.Wand:
                if (Account.CurrentAccount.GetCurrentHero().Class != GameClasses.Wizard) return;
                EquipItemToSlot(11, item);
                break;
            case ItemType.Axe1H:
                EquipItemToSlot(11, item);
                break;



            case ItemType.CrusaderShield:
                if (Account.CurrentAccount.GetCurrentHero().Class != GameClasses.Crusader) return;
                EquipItemToSlot(12, item);
                break;
            case ItemType.Mojo:
                if (Account.CurrentAccount.GetCurrentHero().Class != GameClasses.WitchDoctor) return;
                EquipItemToSlot(12, item);
                break;
            case ItemType.Orb:
                if (Account.CurrentAccount.GetCurrentHero().Class != GameClasses.Wizard) return;
                EquipItemToSlot(12, item);
                break;
            case ItemType.Quiver:
                if (Account.CurrentAccount.GetCurrentHero().Class != GameClasses.DemonHunter) return;
                EquipItemToSlot(12, item);
                break;
            case ItemType.Shield:
                if (Account.CurrentAccount.GetCurrentHero().Class != GameClasses.Wizard ||
                    Account.CurrentAccount.GetCurrentHero().Class != GameClasses.Barbarian ||
                    Account.CurrentAccount.GetCurrentHero().Class != GameClasses.Crusader ) return;
                EquipItemToSlot(12, item);
                break;
        }
    }

    /*
     * This method is used to equip item. 
     * Slot - where will be that item equipped. For relation between numbers and slot types look to the Account.cs, Hero class
     * item - item that will be equipped.
     * TwoHanded - TwoHanded items have some special logic(checking if there is any off-hand which should be dropped to the inventory)
     * There is also some special logic for rings which is occupied number 7
     * 
     * Base logic - check if there is any equipped item in that slot. If it is then drop swap it with item we want to equip. Calls for InventoryChanged event after completion
     */
    private void EquipItemToSlot(int slot, InventoryItem item, bool TwoHanded = false)
    {
        if (slot == 7)
        {
            InventoryItem equippedItem;
            if (Account.CurrentAccount.GetCurrentHero().EquippedItems[7] != null &&
                Account.CurrentAccount.GetCurrentHero().EquippedItems[8] == null)
            {
                item.LeftTopCornerCoordinate = new Point() { x = -1, y = -1 };
                Account.CurrentAccount.GetCurrentHero().EquippedItems[8] = item;

            }
            else
            {
                if (Account.CurrentAccount.GetCurrentHero().EquippedItems[7] != null)
                {
                    equippedItem = Account.CurrentAccount.GetCurrentHero().EquippedItems[7];
                    equippedItem.LeftTopCornerCoordinate = item.LeftTopCornerCoordinate; 
                    inventory.Add(equippedItem);
                }
                item.LeftTopCornerCoordinate = new Point() { x = -1, y = -1 };
                Account.CurrentAccount.GetCurrentHero().EquippedItems[7] = item;
            }

            inventory.Remove(item);

        }
        else if (Account.CurrentAccount.GetCurrentHero().EquippedItems[slot] != null)
        {

            if (TwoHanded)
            {
                if (Account.CurrentAccount.GetCurrentHero().EquippedItems[12] != null &&
                    Account.CurrentAccount.GetCurrentHero().EquippedItems[12].Type != ItemType.Quiver)
                {
                    var freeSlot = CheckForFreeSlot(2);
                    if (freeSlot.x == -1 && freeSlot.y == -1) return;
                    var equippedOffhand = Account.CurrentAccount.GetCurrentHero().EquippedItems[12];
                    equippedOffhand.LeftTopCornerCoordinate = freeSlot;
                    inventory.Add(equippedOffhand);
                    Account.CurrentAccount.GetCurrentHero().EquippedItems[12] = null;
                }
            }

            var equippedItem = Account.CurrentAccount.GetCurrentHero().EquippedItems[slot];
            equippedItem.LeftTopCornerCoordinate = item.LeftTopCornerCoordinate;
            item.LeftTopCornerCoordinate = new Point{x = -1, y = -1};
            Account.CurrentAccount.GetCurrentHero().EquippedItems[slot] = item;
            inventory.Remove(item);
            inventory.Add(equippedItem);
        }
        else
        {
            item.LeftTopCornerCoordinate = new Point {x = -1, y = -1};
            Account.CurrentAccount.GetCurrentHero().EquippedItems[slot] = item;
            inventory.Remove(item);
        }

        CheckSlots();

        if (InventoryChangedEvent != null)
            InventoryChangedEvent.Invoke();
    }
}
