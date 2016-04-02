using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Resources.Scripts;
using UnityEngine;

//This class stores amounts of hero's stats
[Serializable()]
public class ClassStats
{
    public int[] coreTalentsArray = {1, 0, 0, 0, 0};
    public int[] offenseTalentsArray = { 0, 0, 0, 0, 0 };
    public int[] defenseTalentsArray = { 0, 0, 0, 0, 0 };
    public int[] utilityTalentsArray = { 0, 0, 0, 0, 0 };
    public int[] botTalentsArray = { 0, 0, 0, 0, 0 };

    public int damage = 1;
    public int mainStat = 1;
    public int vitality = 1;
    public int botDamage = 0;
    public int BotAttackSpeed = 0;
}

/*
 * Definition for a Hero.
 * Stores:
 * - Name
 * - Level
 * - Class
 * - Gender
 * - EquippedItems. List of all equipped to the hero items
 * Has next methods:
 * - AddExperience. Use to add some amount of experienced to that hero
 * - GetAmountOfSpentPoints. Get amount of talents which were used in the selected tab.
 * - GetAmountOfUnspentCorePoints. Get amount of unused talents in the selected tab.
 * - AddTalentPoint. Add one point to selected talent.
 * - ResetTalentPoints. Return all points so you can use them once more.
 * - SubtractTalentPoints. Remove one point from selected talent.
 * - RecalculateStats. Recalculate amount of stats hero has from talents and equipped items.
 */
[Serializable()]
public class Hero
{
    [SerializeField] private string _name;

    public string Name
    {
        get { return _name; }
        set
        {
            if (State != HeroState.NotCreated) return;
            _name = value;
        }
    }

    private int level = 1;

    public int Level
    {
        get { return level; }
    }

    [SerializeField] private double experienceGained;
    [SerializeField] private double experienceForLevel = 100;


    public GameClasses Class { get; set; }

    [SerializeField]
    private ClassStats Stats { get; set; }

    public bool Gender;
    public HeroState State;
    public bool Heroic;

    public Hero()
    {
        Stats = new ClassStats();
        RecalculateStats();
        InventoryClass.InventoryChangedEvent += RecalculateStats;
    }

    public void AddExperience(double experienceAmount)
    {
        if (experienceGained + experienceAmount >= experienceForLevel)
        {
            while (experienceGained + experienceAmount >= experienceForLevel)
            {
                level++;
                var talentResult = level%4;

                switch (talentResult)
                {
                    case 0:
                        Stats.utilityTalentsArray[0]++;
                        break;
                    case 1:
                        Stats.coreTalentsArray[0]++;
                        break;
                    case 2:
                        Stats.offenseTalentsArray[0]++;
                        break;
                    case 3:
                        Stats.defenseTalentsArray[0]++;
                        break;
                }

                if (level%10 == 0)
                    Stats.botTalentsArray[0]++;

                experienceGained = (experienceGained + experienceAmount) - experienceForLevel;
                experienceForLevel = Convert.ToInt32(100*Math.Pow(1.1, level - 1));
            }
        }
        else
        {
            experienceGained += experienceAmount;
        }

        RecalculateStats();
    }

    public int GetAmountOfSpentPoints(CurrentTab tab)
    {
        switch (tab)
        {
            case CurrentTab.Core:
                return Stats.coreTalentsArray[1] + Stats.coreTalentsArray[2] + Stats.coreTalentsArray[3] +
                       Stats.coreTalentsArray[4];
            case CurrentTab.Offense:
                return Stats.offenseTalentsArray[1] + Stats.offenseTalentsArray[2] + Stats.offenseTalentsArray[3] +
                       Stats.offenseTalentsArray[4];
            case CurrentTab.Defense:
                return Stats.defenseTalentsArray[1] + Stats.defenseTalentsArray[2] + Stats.defenseTalentsArray[3] +
                       Stats.defenseTalentsArray[4];
            case CurrentTab.Utility:
                return Stats.utilityTalentsArray[1] + Stats.utilityTalentsArray[2] + Stats.utilityTalentsArray[3] +
                       Stats.utilityTalentsArray[4];
            case CurrentTab.Bot:
                return Stats.botTalentsArray[1] + Stats.botTalentsArray[2] + Stats.botTalentsArray[3] +
                       Stats.botTalentsArray[4];
            default:
                return 0;
        }
    }

    public int GetAmountOfUnspentCorePoints(CurrentTab tab)
    {
        switch (tab)
        {
            case CurrentTab.Core:
                return Stats.coreTalentsArray[0];
            case CurrentTab.Offense:
                return Stats.offenseTalentsArray[0];
            case CurrentTab.Defense:
                return Stats.defenseTalentsArray[0];
            case CurrentTab.Utility:
                return Stats.utilityTalentsArray[0];
            case CurrentTab.Bot:
                return Stats.botTalentsArray[0];
            default:
                return 0;
        }
    }

    public int GetAmountOfTalentPoints(CurrentTab tab, int index)
    {
        if (index > 4 || index < 0) return 0;
        switch (tab)
        {
            case CurrentTab.Core:
                return Stats.coreTalentsArray[index];
            case CurrentTab.Offense:
                return Stats.offenseTalentsArray[index];
            case CurrentTab.Defense:
                return Stats.defenseTalentsArray[index];
            case CurrentTab.Utility:
                return Stats.utilityTalentsArray[index];
            case CurrentTab.Bot:
                return Stats.botTalentsArray[index];
            default:
                return 0;
        }
    }

    public void AddTalentPoint(CurrentTab tab, int index)
    {
        if (index > 4) return;
        switch (tab)
        {
            case CurrentTab.Core:
                if (Stats.coreTalentsArray[0] < 1)
                    return;
                Stats.coreTalentsArray[index]++;
                Stats.coreTalentsArray[0]--;
                break;
            case CurrentTab.Offense:
                if (Stats.offenseTalentsArray[0] < 1)
                    return;
                Stats.offenseTalentsArray[index]++;
                Stats.offenseTalentsArray[0]--;
                break;
            case CurrentTab.Defense:
                if (Stats.defenseTalentsArray[0] < 1)
                    return;
                Stats.defenseTalentsArray[index]++;
                Stats.defenseTalentsArray[0]--;
                break;
            case CurrentTab.Utility:
                if (Stats.utilityTalentsArray[0] < 1)
                    return;
                Stats.utilityTalentsArray[index]++;
                Stats.utilityTalentsArray[0]--;
                break;
            case CurrentTab.Bot:
                if (Stats.botTalentsArray[0] < 1)
                    return;
                Stats.botTalentsArray[index]++;
                Stats.botTalentsArray[0]--;
                break;
        }
        RecalculateStats();
    }


    public void ResetTalentPoints(CurrentTab tab)
    {
        switch (tab)
        {
            case CurrentTab.Core:
                Stats.coreTalentsArray[0] = Stats.coreTalentsArray.Sum();
                for (int i = 1; i < Stats.coreTalentsArray.Length; i++)
                {
                    Stats.coreTalentsArray[i] = 0;
                }
                break;
            case CurrentTab.Offense: 
                Stats.offenseTalentsArray[0] = Stats.offenseTalentsArray.Sum();
                for (int i = 1; i < Stats.offenseTalentsArray.Length; i++)
                {
                    Stats.offenseTalentsArray[i] = 0;
                }
                break;
            case CurrentTab.Defense:
                Stats.defenseTalentsArray[0] = Stats.defenseTalentsArray.Sum();
                for (int i = 1; i < Stats.defenseTalentsArray.Length; i++)
                {
                    Stats.defenseTalentsArray[i] = 0;
                }
                break;
            case CurrentTab.Utility:
                Stats.utilityTalentsArray[0] = Stats.utilityTalentsArray.Sum();
                for (int i = 1; i < Stats.utilityTalentsArray.Length; i++)
                {
                    Stats.utilityTalentsArray[i] = 0;
                }
                break;
            case CurrentTab.Bot:
                Stats.botTalentsArray[0] = Stats.botTalentsArray.Sum();
                for (int i = 1; i < Stats.botTalentsArray.Length; i++)
                {
                    Stats.botTalentsArray[i] = 0;
                }
                break;
        }
    }

    public void SubtractTalentPoints(CurrentTab tab, int index)
    {
        if (index > 4) return;
        switch (tab)
        {
            case CurrentTab.Core:
                if (Stats.coreTalentsArray[index] < 1) return;
                Stats.coreTalentsArray[index]--;
                Stats.coreTalentsArray[0]++;
                break;
            case CurrentTab.Offense:
                if (Stats.offenseTalentsArray[index] < 1) return;
                Stats.offenseTalentsArray[index]--;
                Stats.offenseTalentsArray[0]++;
                break;
            case CurrentTab.Defense:
                if (Stats.defenseTalentsArray[index] < 1) return;
                Stats.defenseTalentsArray[index]--;
                Stats.defenseTalentsArray[0]++;
                break;
            case CurrentTab.Utility:
                if (Stats.utilityTalentsArray[index] < 1) return;
                Stats.utilityTalentsArray[index] --;
                Stats.utilityTalentsArray[0]++;
                break;
            case CurrentTab.Bot:
                if (Stats.botTalentsArray[index] < 1) return;
                Stats.botTalentsArray[index]--;
                Stats.botTalentsArray[0]++;
                break;
        }
        RecalculateStats();
    }

    private void RecalculateStats()
    {
        double mainStatFromItems;
        if (Class == GameClasses.Barbarian || Class == GameClasses.Crusader)
            mainStatFromItems = EquippedItems.Where(z=>z != null).Sum(z =>  z.itemStats.Where(x => x.stat == CharacterStat.Strength).Sum(x => x.StatAmount));
        else if(Class == GameClasses.Monk || Class == GameClasses.DemonHunter)
            mainStatFromItems = EquippedItems.Where(z=>z != null).Sum(z => z.itemStats.Where(x => x.stat == CharacterStat.Dexterity).Sum(x => x.StatAmount));
        else
            mainStatFromItems = EquippedItems.Where(z=>z != null).Sum(z => z.itemStats.Where(x => x.stat == CharacterStat.Intelligence).Sum(x => x.StatAmount));
        Stats.mainStat = 1 + (Stats.coreTalentsArray[1]*5) + Convert.ToInt32(mainStatFromItems);
        Stats.vitality = 1 + (Stats.coreTalentsArray[2]*5);
        Stats.damage = 1*Stats.mainStat*(level != 1 ? level/2 : 1);
        Stats.botDamage = Convert.ToInt32(EquippedItems.Where(z => z != null).Sum(z => z.itemStats.Where(x => x.stat == CharacterStat.BotDamage).Sum(x => x.StatAmount)) + (Stats.botTalentsArray[1]* 0.1 * Stats.damage));

    }

    public double ReturnStat(CharacterStat stat)
    {
        switch (stat)
        {
            case CharacterStat.Damage:
                return Stats.damage;
            case CharacterStat.CritChance:
                break;
            case CharacterStat.CritDamage:
                break;
            case CharacterStat.MainStat:
                return Stats.mainStat;
            case CharacterStat.Vitality:
                return Stats.vitality;
            case CharacterStat.BotDamage:
                return Stats.botDamage;
            case CharacterStat.BotAttackSpeed:
                return Stats.BotAttackSpeed;

        }
        return 0;
    }

    public InventoryItem[] EquippedItems = new InventoryItem[13];

    public InventoryClass InventoryClass = new InventoryClass();
}


/*
 * Class which is describes Account. Uses Singletone pattern.
 * Stores basic for all heroes information - amount of gold, blood shards, 
 * number of heroes, id of the selected hero.
 * Also contains some methods for managing heroes (adding/deleting/selecting). 
 */
[Serializable()]
public class Account
{
    public delegate void DeletedHeroHandler();

    [field: NonSerialized]
    public event DeletedHeroHandler HeroDeletedEvent;

    public delegate void ChangedHeroHandler();

    [field: NonSerialized]
    public event ChangedHeroHandler CurrentHeroChangedEvent;

    private static Account currentAccount;

    public static Account CurrentAccount
    {
        get
        {
            if (currentAccount == null)
            {
                currentAccount = new Account();
            }
            return currentAccount;
        }
        set { currentAccount = value; }
    }

    private Account()
    {
        listOfHeroes = new List<Hero>();
        currentHeroId = -1;
        CraftersLevel = new uint[3];
        CraftersLevelHardcore = new uint[3];
    }

    public string AccountName = "";

    private int currentHeroId;

    public int CurrentHeroId
    {
        get { return currentHeroId; }
    }

    private List<Hero> listOfHeroes;

    public List<Hero> ListOfHeroes
    {
        get { return listOfHeroes; }
    }

    // public Dictionary<int, bool> AchievementsList = new Dictionary<int, bool>();
    public uint ParagonLevel;
    public uint ParagonLevelHardcore;
    public uint Gold;
    public uint GoldHardcore;
    public uint BloodShards;
    public uint BloodShardsHardcore;
    public uint[] CraftersLevel;
    public uint[] CraftersLevelHardcore;

    public Hero GetCurrentHero()
    {
        return listOfHeroes[currentHeroId];
    }

    public void SetCurrentHero(int newHeroId)
    {
        Debug.Log(string.Format("NewId: {0}, time: {1}", newHeroId, DateTime.Now));
        if (newHeroId > listOfHeroes.Count - 1) return;
        currentHeroId = newHeroId;

        if (CurrentHeroChangedEvent != null)
            CurrentHeroChangedEvent();
    }

    public void AddNewHero(Hero newHero)
    {
        listOfHeroes.Add(newHero);
        currentHeroId = listOfHeroes.Count - 1;
    }

    public void DeleteHero(Hero deleteHero)
    {
        listOfHeroes.Remove(deleteHero);
        SetCurrentHero(listOfHeroes.Count - 1);

        if (HeroDeletedEvent != null)
            HeroDeletedEvent();
    }

    //this method is mostly used for checking if there is no heroes which creation were started but have never been completed.
    public void CheckAccountForErrors()
    {
        var amount = listOfHeroes.RemoveAll(z => z.State == HeroState.NotCreated);
        if (amount == 0) return;
        if (currentHeroId == 0 && listOfHeroes.Count > 0)
            return;
        if (listOfHeroes.Count == 0)
        {
            SetCurrentHero(-1);
            return;
        }
        SetCurrentHero(listOfHeroes.Count - 1);
    }
}
