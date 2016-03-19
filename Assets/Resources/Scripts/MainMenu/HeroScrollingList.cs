using System;
using UnityEngine;
using System.Collections;
using Assets.Resources.Scripts;

public class HeroScrollingList : MonoBehaviour
{
    private bool isInitialized = false;
   
    private const float scrollBarStep = 0.0478f;
    private const float heroPlateStep = 0.04f;

	void Start ()
	{
	    Account.CurrentAccount.HeroDeletedEvent += CreateHeroesList;

	    for (int i = 0; i < 19; i ++)
	    {
            StaticScripts.CreateGameObj(string.Format("ScrollBackgroundPiece_{0}", i), @"Controls/ScrollSmallMiddle", new Vector3(0.7f, 0.7f), new Vector3(-5.03f, 2.8f - (0.27f*i), 10f), child: true, parentName: name, sortingOrder: 2);
	    }
        var heroListFrame = StaticScripts.CreateGameObj("HeroListFrame", @"Borders/MainMenu/BattlenetHeroSelect_ListFrame", new Vector3(0.83f, 0.83f), new Vector3(-8.27f, -2.35f, 10f), child:true, parentName:name);
        StaticScripts.CreateGameObj("ScrollUp", @"Controls/ScrollSmallUp_Up", new Vector3(0.7f, 0.7f), new Vector3(-5.03f, 3.07f, 10f), true, 1, true, typeof(ButtonScrollUp), true, name, 3);
        StaticScripts.CreateGameObj("ScrollDown", @"Controls/ScrollSmallDown_Up", new Vector3(0.7f, 0.7f), new Vector3(-5.03f, -2.3f, 10f), true, 1, true, typeof(ButtonScrollDown), true, name, 3);
        StaticScripts.CreateGameObj("Scrollbar", @"Controls/ScrollbarThumb_Up", new Vector3(0.9f, 0.9f), new Vector3(-5f, 2.75f, 10f), true, 1, true, typeof(ScrollBarScript), true, name, 3);

	    isInitialized = true;
        CreateHeroesList();

	}

    void Update()
    {}

    void CreateHeroesList()
    {
        if(!isInitialized) return;

        var mainGameObject = GameObject.Find("MainLogic");
        if (mainGameObject == null ||
            mainGameObject.GetComponent<Main>().CurrentGameState != GameState.CharacterSelection)
            return;

        var ParentHeroSlotsObject = GameObject.Find("ParentHeroSlotsObject");
        if (ParentHeroSlotsObject != null)
            DestroyImmediate(ParentHeroSlotsObject);
        ParentHeroSlotsObject = new GameObject("ParentHeroSlotsObject");
        ParentHeroSlotsObject.transform.parent = transform;

        var Scrollbar = GameObject.Find("Scrollbar");
        if (Scrollbar != null && Scrollbar.transform.position != new Vector3(-5f, 2.75f, 10f))
            Scrollbar.transform.position = new Vector3(-5f, 2.75f, 10f);

        var shader = Shader.Find("GUI/3D Text Shader - Cull Back");

        var heightForSelectedChampion = 0f;
        
        for (int i = 0; i < 12; i++)
        {
            if (i < Account.CurrentAccount.ListOfHeroes.Count)
            {
                var heroSlot = StaticScripts.CreateGameObj(string.Format("HeroSlot_{0}", i), @"Buttons/BattleNetButton_MainMenu_Up", new Vector3(0.83f, 0.83f), new Vector3(-8.24f, 2.6f - (0.8f * i), 150f), true, 1, true, typeof(HeroSlotScript), child: true, parentName: "ParentHeroSlotsObject", sortingOrder: 3);
                heroSlot.GetComponent<HeroSlotScript>().CharacterId = i;
                if (Account.CurrentAccount.CurrentHeroId == i)
                {
                    heroSlot.GetComponent<HeroSlotScript>()._State = ButtonState.Selected;
                    heightForSelectedChampion = 2.15f - (0.8f*i);
                }
                StaticScripts.CreateGameObj(string.Format("HeroSlot_{0}_portrait_background", i), @"Portraits/Heroes/ParagonBorders/Prestige_PortraitCircle_01", new Vector3(1f, 1f), new Vector3(-8.35f, 2.15f - (0.8f * i), 160f), child: true, parentName: "ParentHeroSlotsObject", sortingOrder: 3);

                string pathToTexture = string.Empty;
                switch (Account.CurrentAccount.ListOfHeroes[i].Class)
                {
                    case GameClasses.Barbarian:
                        pathToTexture = Account.CurrentAccount.ListOfHeroes[i].Gender ? @"Portraits/Heroes/Portrait_Barbarian_Male" : @"Portraits/Heroes/Portrait_Barbarian_Female";
                        break;
                    case GameClasses.Crusader:
                        pathToTexture = Account.CurrentAccount.ListOfHeroes[i].Gender ? @"Portraits/Heroes/Portrait_Crusader_Male" : @"Portraits/Heroes/Portrait_Crusader_Female";
                        break;
                    case GameClasses.DemonHunter:
                        pathToTexture = Account.CurrentAccount.ListOfHeroes[i].Gender ? @"Portraits/Heroes/Portrait_Demonhunter_Male" : @"Portraits/Heroes/Portrait_Demonhunter_Female";
                        break;
                    case GameClasses.Monk:
                        pathToTexture = Account.CurrentAccount.ListOfHeroes[i].Gender ? @"Portraits/Heroes/Portrait_Monk_Male" : @"Portraits/Heroes/Portrait_Monk_Female";
                        break;
                    case GameClasses.WitchDoctor:
                        pathToTexture = Account.CurrentAccount.ListOfHeroes[i].Gender ? @"Portraits/Heroes/Portrait_Witchdoctor_Male" : @"Portraits/Heroes/Portrait_Witchdoctor_Female";
                        break;
                    case GameClasses.Wizard:
                        pathToTexture = Account.CurrentAccount.ListOfHeroes[i].Gender ? @"Portraits/Heroes/Portrait_Wizard_Male" : @"Portraits/Heroes/Portrait_Wizard_Female";
                        break;
                }
                StaticScripts.CreateGameObj(string.Format("HeroSlot_{0}_portrait", i), pathToTexture, new Vector3(0.2f, 0.2f), new Vector3(-8.07f, 2.75f - (0.8f * i), 155f), child: true, parentName: "ParentHeroSlotsObject", sortingOrder: 3);


                StaticScripts.CreateGameObj(string.Format("HeroSlot_{0}_background", i), @"Controls/BattleNetHeroSelect_BaseSlot", new Vector3(0.83f, 0.83f), new Vector3(-8.23f, 2.6f - (0.8f * i), 170f), child: true, parentName: "ParentHeroSlotsObject", sortingOrder: 3);


                var nameTextObject = StaticScripts.CreateTextObj(string.Format("HeroSlot_{0}_name", i), Account.CurrentAccount.ListOfHeroes[i].Name, new Vector3(0.02f, 0.02f), new Vector3(0.9f, 0.75f),
                    FontType.StandartFont, 120, new Color32(243, 170, 85, 255), child: true, parentName: string.Format("HeroSlot_{0}", i));
                nameTextObject.GetComponent<Renderer>().material.shader = shader;

                var levelTextObject = StaticScripts.CreateTextObj(string.Format("HeroSlot_{0}_class_level", i), string.Format("Level {0} {1}", Account.CurrentAccount.ListOfHeroes[i].Level, Account.CurrentAccount.ListOfHeroes[i].Class), new Vector3(0.02f, 0.02f), new Vector3(0.9f, 0.45f),
                    FontType.StandartFont, 80, new Color32(141, 105, 53, 255), child: true, parentName: string.Format("HeroSlot_{0}", i));
                levelTextObject.GetComponent<Renderer>().material.shader = shader;

                if (Account.CurrentAccount.ListOfHeroes[i].Heroic)
                    StaticScripts.CreateGameObj(string.Format("HeroSlot_{0}_HeroicIcon", i), @"Icons/CharacterCreation/BattleNet_HardcoreIcon", new Vector3(0.9f, 0.9f), new Vector3(1.2f + nameTextObject.GetComponent<Renderer>().bounds.size.x, 0.45f), child:true, parentName: string.Format("HeroSlot_{0}", i));

                if(Account.CurrentAccount.CurrentHeroId == i)
                    heroSlot.GetComponent<HeroSlotScript>().SetThisSlotSelected();

                continue;
            }
            StaticScripts.CreateGameObj(string.Format("HeroSlot_{0}", i), @"Controls/BattleNetHeroSelect_EmptySlot", new Vector3(0.83f, 0.83f), new Vector3(-8.24f, 2.6f - (0.8f * i), 150f), child: true, parentName: "ParentHeroSlotsObject", sortingOrder: 3);
        }

        MoveHeroSlots(heightForSelectedChampion, true);
    }

    void OnEnable()
    {
        CreateHeroesList();
    }

    void MoveHeroSlots(float height, bool autoScroll)
    {
        var stepCount = height / scrollBarStep;
        stepCount *= heroPlateStep;

        var mainHeroListObject = GameObject.Find("HeroListGameObject");
        var firstSlotObject = GameObject.Find("HeroSlot_0");
        if (firstSlotObject.transform.position.y == 2.6f && stepCount > 0) return;
        if (stepCount > 0 && firstSlotObject.transform.position.y - stepCount < 2.6f)
        {
            stepCount = firstSlotObject.transform.position.y - 2.6f;
        }

        var lastSlotObject = GameObject.Find("HeroSlot_11");
        if (lastSlotObject.transform.position.y == -2.3f && stepCount < 0) return;
        if (stepCount < 0 && lastSlotObject.transform.position.y - stepCount > -2.3f)
        {
            stepCount = lastSlotObject.transform.position.y - (-2.3f);
        }


        foreach (Transform child in mainHeroListObject.transform)
        {
            if (child.name.Contains("HeroSlot"))
            {
                child.position = new Vector3(child.position.x, child.position.y - stepCount, child.position.z);
            }
        }

        if (!autoScroll) return;

        var scrollBarObject = GameObject.Find("Scrollbar");
        if (scrollBarObject)
        {
            if (scrollBarObject.transform.position.y + height > 2.77f )
                scrollBarObject.transform.position = new Vector3(scrollBarObject.transform.position.x, 2.77f, scrollBarObject.transform.position.z);
            else if (scrollBarObject.transform.position.y + height < -2.01f)
                scrollBarObject.transform.position = new Vector3(scrollBarObject.transform.position.x, -2.01f, scrollBarObject.transform.position.z);
            else
                scrollBarObject.transform.position = new Vector3(scrollBarObject.transform.position.x, scrollBarObject.transform.position.y + height, scrollBarObject.transform.position.z);
        }

    }

}
