using System;
using UnityEngine;
using System.Collections;
using Assets.Resources.Scripts;

public class CharacterTabWindowScript : MonoBehaviour {

	// Use this for initialization

    private GameObject LevelAmountTextObject;
    private GameObject MainStatAmountTextObject;

	void Start ()
	{
	    string backgroundTex = "";
	    switch (Account.CurrentAccount.GetCurrentHero().Class)
	    {
            case GameClasses.Barbarian: 
                backgroundTex = Account.CurrentAccount.GetCurrentHero().Gender
                    ?"Borders/InGame/Inventory/Class_Background_Barbarian_Male" :
                    "Borders/InGame/Inventory/Class_Background_Barbarian_Female";
	            break;
	        case GameClasses.Crusader:
	            backgroundTex = Account.CurrentAccount.GetCurrentHero().Gender
                    ? "Borders/InGame/Inventory/Class_Background_Crusader_Male"
                    : "Borders/InGame/Inventory/Class_Background_Crusader_Female";
	            break;
            case GameClasses.DemonHunter:
                backgroundTex = Account.CurrentAccount.GetCurrentHero().Gender
                     ? "Borders/InGame/Inventory/Class_Background_DemonHunter_Male"
                     : "Borders/InGame/Inventory/Class_Background_DemonHunter_Female";
	            break;
            case GameClasses.Monk:
                backgroundTex = Account.CurrentAccount.GetCurrentHero().Gender
                     ? "Borders/InGame/Inventory/Class_Background_Monk_Male"
                     : "Borders/InGame/Inventory/Class_Background_Monk_Female";
	            break;
            case GameClasses.WitchDoctor:
                backgroundTex = Account.CurrentAccount.GetCurrentHero().Gender
                     ? "Borders/InGame/Inventory/Class_Background_Witchdoctor_Male"
                     : "Borders/InGame/Inventory/Class_Background_Witchdoctor_Female";
	            break;
            case GameClasses.Wizard:
                backgroundTex = Account.CurrentAccount.GetCurrentHero().Gender
                     ? "Borders/InGame/Inventory/Class_Background_Wizard_Male"
                     : "Borders/InGame/Inventory/Class_Background_Wizard_Female";
	            break;
	    }
	    var background = StaticScripts.CreateGameObj("classBackground", backgroundTex, new Vector3(1.5f, 1.4f), new Vector3(1f, -3.14f, 10f), child: true, parentName: name, sortingOrder: 3);

	    var levelTextObject = StaticScripts.CreateTextObj("LevelTextObject", "Level:", new Vector3(0.02f, 0.02f), new Vector3(0.15f, 4.1f, 0f),
            FontType.StandartFont, 64, new Color32(243, 170, 85, 255), TextAlignment.Center, true, background.name);
        levelTextObject.transform.localScale = new Vector3(1f, 1.05f);

        LevelAmountTextObject = StaticScripts.CreateTextObj("CharacterTextObject", Account.CurrentAccount.GetCurrentHero().Level.ToString(),
	        new Vector3(0.02f, 0.02f), new Vector3(0f, 0f, 0f), FontType.StandartFont, 68, Color.white,
            TextAlignment.Center, true, background.name);
        LevelAmountTextObject.transform.localScale = new Vector3(1f, 1.05f);
        LevelAmountTextObject.transform.localPosition = new Vector3(1.7f - (LevelAmountTextObject.GetComponent<Renderer>().bounds.size.x/4f), 4.115f);

	    GameObject mainStatObject;
	    if (Account.CurrentAccount.GetCurrentHero().Class == GameClasses.Barbarian ||
	        Account.CurrentAccount.GetCurrentHero().Class == GameClasses.Crusader)
	    {
	        mainStatObject = StaticScripts.CreateTextObj("MainStatNameTextObject", "Strength:", new Vector3(0.02f, 0.02f),
                new Vector3(0.15f, 3.9f, 0f), FontType.StandartFont, 64, new Color32(243, 170, 85, 255), TextAlignment.Center,
                true, background.name);
	    }
        else if (Account.CurrentAccount.GetCurrentHero().Class == GameClasses.DemonHunter ||
                 Account.CurrentAccount.GetCurrentHero().Class == GameClasses.Monk)
        {
            mainStatObject = StaticScripts.CreateTextObj("MainStatNameTextObject", "Dexterity:", new Vector3(0.02f, 0.02f),
                new Vector3(0.15f, 3.9f, 0f), FontType.StandartFont, 64, new Color32(243, 170, 85, 255), TextAlignment.Center,
                true, background.name);
        }
        else
        {
            mainStatObject = StaticScripts.CreateTextObj("MainStatNameTextObject", "Intelligence:", new Vector3(0.02f, 0.02f),
                new Vector3(0.15f, 3.9f, 0f), FontType.StandartFont, 64, new Color32(243, 170, 85, 255), TextAlignment.Center,
                true, background.name);
        }
        mainStatObject.transform.localScale = new Vector3(1f, 1.1f);

        MainStatAmountTextObject = StaticScripts.CreateTextObj("MainStatAmountTextObject", "10", new Vector3(0.02f, 0.02f),
                new Vector3(0f, 0f, 0f), FontType.StandartFont, 68, Color.white, TextAlignment.Center,
                true, background.name);
        MainStatAmountTextObject.transform.localScale = new Vector3(1f, 1.1f);
        MainStatAmountTextObject.transform.localPosition = new Vector3(1.7f - (MainStatAmountTextObject.GetComponent<Renderer>().bounds.size.x / 4f), 3.91f);


        var vitalityStatTextObject = StaticScripts.CreateTextObj("VitalityTextObject", "Vitality:", new Vector3(0.02f, 0.02f),
                new Vector3(0.15f, 3.7f, 0f), FontType.StandartFont, 64, new Color32(243, 170, 85, 255), TextAlignment.Center,
                true, background.name);
        vitalityStatTextObject.transform.localScale = new Vector3(1f, 1.1f);

        var vitalityStatAmountTextObject = StaticScripts.CreateTextObj("VitalityAmountTextObject", "10", new Vector3(0.02f, 0.02f),
                new Vector3(0f, 0f, 0f), FontType.StandartFont, 68, Color.white, TextAlignment.Center,
                true, background.name);
        vitalityStatAmountTextObject.transform.localScale = new Vector3(1f, 1.1f);
        vitalityStatAmountTextObject.transform.localPosition = new Vector3(1.7f - (vitalityStatAmountTextObject.GetComponent<Renderer>().bounds.size.x / 4f), 3.71f);

        var goldFindTextObject = StaticScripts.CreateTextObj("GoldFindTextObject", "Gold find:", new Vector3(0.02f, 0.02f),
                new Vector3(0.15f, 3.5f, 0f), FontType.StandartFont, 64, new Color32(243, 170, 85, 255), TextAlignment.Center,
                true, background.name);
        goldFindTextObject.transform.localScale = new Vector3(1f, 1.1f);

        var goldFindStatAmountTextObject = StaticScripts.CreateTextObj("GoldFindAmountTextObject", "10 %", new Vector3(0.02f, 0.02f),
                new Vector3(0f, 0f, 0f), FontType.StandartFont, 68, Color.white, TextAlignment.Center,
                true, background.name);
        goldFindStatAmountTextObject.transform.localScale = new Vector3(1f, 1.1f);
        goldFindStatAmountTextObject.transform.localPosition = new Vector3(1.7f - (goldFindStatAmountTextObject.GetComponent<Renderer>().bounds.size.x / 4f), 3.51f);

        var magicFindTextObject = StaticScripts.CreateTextObj("MagicFindTextObject", "Magic find:", new Vector3(0.02f, 0.02f),
               new Vector3(0.15f, 3.3f, 0f), FontType.StandartFont, 64, new Color32(243, 170, 85, 255), TextAlignment.Center,
               true, background.name);
        magicFindTextObject.transform.localScale = new Vector3(1f, 1.1f);

        var magicFindStatAmountTextObject = StaticScripts.CreateTextObj("MagicFindAmountTextObject", "10 %", new Vector3(0.02f, 0.02f),
                new Vector3(0f, 0f, 0f), FontType.StandartFont, 68, Color.white, TextAlignment.Center,
                true, background.name);
        magicFindStatAmountTextObject.transform.localScale = new Vector3(1f, 1.1f);
        magicFindStatAmountTextObject.transform.localPosition = new Vector3(1.7f - (magicFindStatAmountTextObject.GetComponent<Renderer>().bounds.size.x / 4f), 3.31f);

        var critChanceTextObject = StaticScripts.CreateTextObj("CritChanceTextObject", "Critical Hit Chance:", new Vector3(0.02f, 0.02f),
               new Vector3(0.15f, 3.1f, 0f), FontType.StandartFont, 64, new Color32(243, 170, 85, 255), TextAlignment.Center,
               true, background.name);
        critChanceTextObject.transform.localScale = new Vector3(0.9f, 1.1f);

        var critChanceStatAmountTextObject = StaticScripts.CreateTextObj("CritChanceStatAmountTextObject", "10 %", new Vector3(0.02f, 0.02f),
                new Vector3(0f, 0f, 0f), FontType.StandartFont, 68, Color.white, TextAlignment.Center,
                true, background.name);
        critChanceStatAmountTextObject.transform.localScale = new Vector3(1f, 1.1f);
        critChanceStatAmountTextObject.transform.localPosition = new Vector3(1.7f - (critChanceStatAmountTextObject.GetComponent<Renderer>().bounds.size.x / 4f), 3.11f);

        var critDamageTextObject = StaticScripts.CreateTextObj("CritDamageTextObject", "Critical Hit Damage:", new Vector3(0.02f, 0.02f),
               new Vector3(0.15f, 2.9f, 0f), FontType.StandartFont, 64, new Color32(243, 170, 85, 255), TextAlignment.Center,
               true, background.name);
        critDamageTextObject.transform.localScale = new Vector3(0.9f, 1.1f);

        var critDamageStatAmountTextObject = StaticScripts.CreateTextObj("CritDamageStatAmountTextObject", "10 %", new Vector3(0.02f, 0.02f),
                new Vector3(0f, 0f, 0f), FontType.StandartFont, 68, Color.white, TextAlignment.Center,
                true, background.name);
        critDamageStatAmountTextObject.transform.localScale = new Vector3(1f, 1.1f);
        critDamageStatAmountTextObject.transform.localPosition = new Vector3(1.7f - (critDamageStatAmountTextObject.GetComponent<Renderer>().bounds.size.x / 4f), 2.91f);

        var armorTextObject = StaticScripts.CreateTextObj("ArmorTextObject", "Armor:", new Vector3(0.02f, 0.02f),
               new Vector3(0.15f, 2.7f, 0f), FontType.StandartFont, 64, new Color32(243, 170, 85, 255), TextAlignment.Center,
               true, background.name);
        armorTextObject.transform.localScale = new Vector3(1f, 1.1f);

        var armorStatAmountTextObject = StaticScripts.CreateTextObj("ArmorStatAmountTextObject", "10", new Vector3(0.02f, 0.02f),
                new Vector3(0f, 0f, 0f), FontType.StandartFont, 68, Color.white, TextAlignment.Center,
                true, background.name);
        armorStatAmountTextObject.transform.localScale = new Vector3(1f, 1.1f);
        armorStatAmountTextObject.transform.localPosition = new Vector3(1.7f - (armorStatAmountTextObject.GetComponent<Renderer>().bounds.size.x / 4f), 2.71f);

	    var resistsTextObject = StaticScripts.CreateTextObj("ResistsTextObject", "Resist All:", new Vector3(0.02f, 0.02f),
            new Vector3(0.15f, 2.5f, 0f), FontType.StandartFont, 64, new Color32(243, 170, 85, 255), TextAlignment.Center,
	        true, background.name);
        resistsTextObject.transform.localScale = new Vector3(1f, 1.1f);

        var resistsStatAmountTextObject = StaticScripts.CreateTextObj("ResistsStatAmountTextObject", "10", new Vector3(0.02f, 0.02f),
                new Vector3(0f, 0f, 0f), FontType.StandartFont, 68, Color.white, TextAlignment.Center,
                true, background.name);
        resistsStatAmountTextObject.transform.localScale = new Vector3(1f, 1.1f);
        resistsStatAmountTextObject.transform.localPosition = new Vector3(1.7f - (resistsStatAmountTextObject.GetComponent<Renderer>().bounds.size.x / 4f), 2.51f);


        //Bottom part of the list. All of them will be drawn in the middle of the border

	    var damageStatTextObject = StaticScripts.CreateTextObj("DamageTextObject", "Damage per Click:",
	        new Vector3(0.02f, 0.02f),
	        new Vector3(0.4f, 2.05f, 0f), FontType.StandartFont, 68, new Color32(243, 170, 85, 255), TextAlignment.Center,
                true, background.name);
        damageStatTextObject.transform.localScale = new Vector3(1f, 1.1f);

        var damageStatAmountTextObject = StaticScripts.CreateTextObj("DamageAmountTextObject", "100 000", new Vector3(0.02f, 0.02f),
                new Vector3(0f, 0f, 0f), FontType.StandartFont, 68, Color.white, TextAlignment.Center,
                true, background.name);
        damageStatAmountTextObject.transform.localScale = new Vector3(1f, 1.1f);
        damageStatAmountTextObject.transform.localPosition = new Vector3(0.95f - (damageStatAmountTextObject.GetComponent<Renderer>().bounds.size.x / 4f), 1.87f);

        var toughnessStatTextObject = StaticScripts.CreateTextObj("ToughnessTextObject", "Toughness:", new Vector3(0.02f, 0.02f),
                new Vector3(0.62f, 1.67f, 0f), FontType.StandartFont, 68, new Color32(243, 170, 85, 255), TextAlignment.Center,
                true, background.name);
        toughnessStatTextObject.transform.localScale = new Vector3(1f, 1.1f);

        var toughnessAmountTextObject = StaticScripts.CreateTextObj("ToughnessAmountTextObject", "10", new Vector3(0.02f, 0.02f),
                new Vector3(0f, 0f, 0f), FontType.StandartFont, 68, Color.white, TextAlignment.Center,
                true, background.name);
        toughnessAmountTextObject.transform.localScale = new Vector3(1f, 1.1f);
        toughnessAmountTextObject.transform.localPosition = new Vector3(0.95f - (toughnessAmountTextObject.GetComponent<Renderer>().bounds.size.x / 4f), 1.49f);

        var healingTextObject = StaticScripts.CreateTextObj("HealingTextObject", "Healing:", new Vector3(0.02f, 0.02f),
                new Vector3(0.73f, 1.29f, 0f), FontType.StandartFont, 68, new Color32(243, 170, 85, 255), TextAlignment.Center,
                true, background.name);
        healingTextObject.transform.localScale = new Vector3(1f, 1.1f);

        var healingAmountTextObject = StaticScripts.CreateTextObj("HealingAmountTextObject", "10", new Vector3(0.02f, 0.02f),
                new Vector3(0f, 0f, 0f), FontType.StandartFont, 68, Color.white, TextAlignment.Center,
                true, background.name);
        healingAmountTextObject.transform.localScale = new Vector3(1f, 1.1f);
        healingAmountTextObject.transform.localPosition = new Vector3(0.95f - (healingAmountTextObject.GetComponent<Renderer>().bounds.size.x / 4f), 1.11f);

        var botDamageTextObject = StaticScripts.CreateTextObj("BotDamageTextObject", "Bot Damage per Tick:", new Vector3(0.02f, 0.02f),
                new Vector3(0.3f, 0.91f, 0f), FontType.StandartFont, 68, new Color32(243, 170, 85, 255), TextAlignment.Center,
                true, background.name);
        botDamageTextObject.transform.localScale = new Vector3(1f, 1.1f);

        var botDamageAmountTextObject = StaticScripts.CreateTextObj("BotDamageAmountTextObject", "10", new Vector3(0.02f, 0.02f),
                new Vector3(0f, 0f, 0f), FontType.StandartFont, 68, Color.white, TextAlignment.Center,
                true, background.name);
        botDamageAmountTextObject.transform.localScale = new Vector3(1f, 1.1f);
        botDamageAmountTextObject.transform.localPosition = new Vector3(0.95f - (botDamageAmountTextObject.GetComponent<Renderer>().bounds.size.x / 4f), 0.73f);

        var ticksPerSecondTextObject = StaticScripts.CreateTextObj("TicksPerSecondTextObject", "Bot Ticks per second:", new Vector3(0.02f, 0.02f),
                new Vector3(0.31f, 0.53f, 0f), FontType.StandartFont, 68, new Color32(243, 170, 85, 255), TextAlignment.Center,
                true, background.name);
        ticksPerSecondTextObject.transform.localScale = new Vector3(1f, 1.1f);

        var tickPerSecondAmountTextObject = StaticScripts.CreateTextObj("TicksPerSecondAmountTextObject", "10", new Vector3(0.02f, 0.02f),
                new Vector3(0f, 0f, 0f), FontType.StandartFont, 68, Color.white, TextAlignment.Center,
                true, background.name);
        tickPerSecondAmountTextObject.transform.localScale = new Vector3(1f, 1.1f);
        tickPerSecondAmountTextObject.transform.localPosition = new Vector3(0.95f - (tickPerSecondAmountTextObject.GetComponent<Renderer>().bounds.size.x / 4f), 0.35f);
	}

    void FixedUpdate()
    {
        LevelAmountTextObject.GetComponent<TextMesh>().text = Account.CurrentAccount.GetCurrentHero().Level.ToString();
        LevelAmountTextObject.transform.localPosition = new Vector3(1.7f - (LevelAmountTextObject.GetComponent<Renderer>().bounds.size.x / 4f), 4.115f);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
