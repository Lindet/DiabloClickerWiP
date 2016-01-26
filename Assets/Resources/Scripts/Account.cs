using System;
using System.Collections.Generic;
using Assets.Resources.Scripts;
using UnityEngine;

public class ClassStats
{
    private uint strength;
    private uint dexterity;
    private uint intelligence;
    private uint vitality;

    public uint Strength
    {
        get { return strength; }
    }

    public uint Dexterity
    {
        get { return dexterity; }
    }

    public uint Intelligence
    {
        get { return intelligence; }
    }

    public uint Vitality
    {
        get { return vitality; }
    }
}

public class Items
{
    private uint id;
    private uint textireId;
    private uint height;
    private ItemQuality quality;
}

public class Hero
{
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

    public GameClasses Class { get; set; }

    public ClassStats Stats { get; set; }
    public bool Gender;
    public HeroState State;
    public bool Heroic;

    public string[] Abilities { get; private set; }
    
   // public Dictionary<int, bool> QuestBook { get; private set; }

   // public int[] Inventory { get; private set; }
}

[Serializable]
public class Account
{
    private static Account currentAccount;
    public static Account CurrentAccount {
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

    [SerializeField]
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
        return listOfHeroes[CurrentHeroId];
    }

    public void SetCurrentHero(int newHeroId)
    {
        if (newHeroId > listOfHeroes.Count - 1) return;
        currentHeroId = newHeroId;
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
    }
}
