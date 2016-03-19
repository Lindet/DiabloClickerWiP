using System;
using System.Collections.Generic;
using Assets.Resources.Scripts;
using UnityEngine;


[Serializable()]
public class ClassStats
{
    public int[] coreTalentsArray = {1, 2, 3, 4, 5};
    public int[] offenseTalentsArray = { 1, 2, 3, 4, 5 };
    public int[] defenseTalentsArray = { 1, 2, 3, 4, 5 };
    public int[] utilityTalentsArray = { 1, 2, 3, 4, 5 };
    public int[] botTalentsArray = { 1, 2, 3, 4, 5 };
}

[Serializable()]
public class Items
{
    private uint id;
    private uint textireId;
    private uint height;
    private ItemQuality quality;
}

[Serializable()]
public class Hero
{
    [SerializeField]
    private string _name;

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
        get { return level;}
    }

    private int experienceGained;
    private int experienceForLevel = 100;


    public GameClasses Class { get; set; }

    private ClassStats Stats { get; set; }
    public bool Gender;
    public HeroState State;
    public bool Heroic;

    public string[] Abilities { get; private set; }

    public Hero()
    {
        Stats = new ClassStats();
    }

    public void AddExperience(int experienceAmount)
    {
        if (experienceGained + experienceAmount >= experienceForLevel)
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
            experienceForLevel = (int)(experienceForLevel*1.2);
        }
        else
        {
            experienceGained += experienceAmount;
        }
    }

    public int GetAmountOfSpentPoints(CurrentTab tab)
    {
        switch (tab)
        {
            case CurrentTab.Core:
                return Stats.coreTalentsArray[1] + Stats.coreTalentsArray[2] + Stats.coreTalentsArray[3] + Stats.coreTalentsArray[4];
            case CurrentTab.Offense:
                return Stats.offenseTalentsArray[1] + Stats.offenseTalentsArray[2] + Stats.offenseTalentsArray[3] + Stats.offenseTalentsArray[4];
            case CurrentTab.Defense:
                return Stats.defenseTalentsArray[1] + Stats.defenseTalentsArray[2] + Stats.defenseTalentsArray[3] + Stats.defenseTalentsArray[4];
            case CurrentTab.Utility:
                return Stats.utilityTalentsArray[1] + Stats.utilityTalentsArray[2] + Stats.utilityTalentsArray[3] + Stats.utilityTalentsArray[4];
            case CurrentTab.Bot:
                return Stats.botTalentsArray[1] + Stats.botTalentsArray[2] + Stats.botTalentsArray[3] + Stats.botTalentsArray[4];
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


    // public Dictionary<int, bool> QuestBook { get; private set; }

    // public int[] Inventory { get; private set; }
}

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
