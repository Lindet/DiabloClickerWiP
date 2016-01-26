using System;
using System.Runtime.Remoting.Messaging;
using System.Text;
using Assets.Resources.Scripts;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileMode = System.IO.FileMode;
using Random = System.Random;

public class Main : MonoBehaviour
{
    public Camera mainCamera;
    private GameState _currentGameState;

    public GameState CurrentGameState
    {
        get { return _currentGameState; }
        set
        {
            if (value == GameState.CharacterCreation)
            {
                CharacterCreationStaff();
            }
            else if (value == GameState.MainMenu && Account.CurrentAccount.ListOfHeroes.Count == 0) //Если на аккаунте еще нет созданных персонажей, тогда вместо главного меню открывается экран создания персонажа
            {
                CurrentGameState = GameState.CharacterCreation;
                return;
            }
            else if (value == GameState.MainMenu)
            {
              MainMenu();
            }
            _currentGameState = value;
        }
    }
    public GameClasses selectedClass = GameClasses.Wizard;

	void Start ()
	{
       // ReadDDS(@"J:\Projects\ForDiabloProject\Work\Base\Textures\D3TexConv\UI\2DUI_Bnet_Login.dds", false);
	    MakeCameraDarker();
	    ShowTopAndBottomBorders();
        LoginMenu();
	}

	void Update () 
	{
	}

    void CharacterCreationStaff()
    {
        try
        {
            var CharacterCreationGameObject = new GameObject("CharacterCreationSceneObject");
            CharacterCreationGameObject.transform.position = new Vector3(0, 0, 10);
            CharacterCreationGameObject.AddComponent<SpriteRenderer>().sprite = Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1),
                new Vector2());

            selectedClass = (GameClasses)Enum.GetValues(typeof(GameClasses)).GetValue(new Random().Next(0, 6));
            Account.CurrentAccount.AddNewHero(new Hero() { Class = selectedClass, Gender = true, State = HeroState.NotCreated });
            
            Act1Background();

            StaticScripts.CreateGameObj("CreateHeroHeaderBorder", @"Borders/CharacterCreation/BattlenetHeroCreate_Header", new Vector3(1.1f, 1.1f), new Vector3(-8f, 2.625f, 10f), child: true, parentName: "CharacterCreationSceneObject");
            StaticScripts.CreateGameObj("BackButton", @"Buttons/BattleNetButton_ClearUp_260x50", new Vector3(0.8f, 0.6f), new Vector3(-7.67f, -4.33f, 10f), true, 1, true, typeof(CharacterCreationBackButtonMouseEvents), child: true, parentName: "CharacterCreationSceneObject");
            GameObject.Find("BackButton").GetComponent<CharacterCreationBackButtonMouseEvents>()._State = ButtonState.Up;
            StaticScripts.CreateTextObj("BackButtonText", "Back", new Vector3(0.02f, 0.02f), new Vector3(1f, 0.44f, 0f), FontType.DiabloFont, 100, new Color32(243, 170, 85, 255), TextAlignment.Center, true, "BackButton");

            #region ClassDetails

                var classDetailTex = string.Empty;
                switch (selectedClass)
                {
                    case GameClasses.Barbarian:
                        classDetailTex = @"Borders/CharacterCreation/BattleNetHeroCreate_DetailsBarbarian";
                        break;
                    case GameClasses.Crusader:
                        classDetailTex = @"Borders/CharacterCreation/BattleNetHeroCreate_DetailsCrusader";
                        break;
                    case GameClasses.DemonHunter:
                        classDetailTex = @"Borders/CharacterCreation/BattleNetHeroCreate_DetailsDemonHunter";
                        break;
                    case GameClasses.Monk:
                        classDetailTex = @"Borders/CharacterCreation/BattleNetHeroCreate_DetailsMonk";
                        break;
                    case GameClasses.WitchDoctor:
                        classDetailTex = @"Borders/CharacterCreation/BattleNetHeroCreate_DetailsWitchDoctor";
                        break;
                    case GameClasses.Wizard:
                        classDetailTex = @"Borders/CharacterCreation/BattleNetHeroCreate_DetailsWizard";
                        break;
                }

                StaticScripts.CreateGameObj("ClassDetails", classDetailTex, new Vector3(0.85f, 0.85f), new Vector3(4f, -4.39f, 10f), child: true, parentName: "CharacterCreationSceneObject");

            #endregion

            #region HeroName

                StaticScripts.CreateGameObj("NameTextBox", @"Borders/CharacterCreation/BattleNetHeroCreate_TextInput", new Vector3(0.8f, 0.8f), new Vector3(-1.56f, -3.8f, 10f), true, 1, true, typeof(CharacterCreationTextBox), child: true, parentName: "CharacterCreationSceneObject");
                StaticScripts.CreateGameObj("CreateHeroButton", @"Buttons/BattleNetButton_RedDisabled_398x82", new Vector3(0.8f, 0.6f), new Vector3(-1.585f, -4.35f, 10f), true, 1, true, typeof(CharacterCreationCreateHeroButtonEvents), child: true, parentName: "CharacterCreationSceneObject");
                GameObject.Find("CreateHeroButton").GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Disabled;
                StaticScripts.CreateTextObj("CreateHeroButtonText", "Create Hero", new Vector3(0.02f, 0.02f), new Vector3(1.18f, 0.5f, 0f), FontType.DiabloFont, 110, new Color32(79, 75, 70, 255), TextAlignment.Center, true, "CreateHeroButton");
                StaticScripts.CreateTextObj("EnterHeroNameText", "Enter Hero Name:", new Vector3(0.02f, 0.02f), new Vector3(-0.9f, -2.9f, 10f), FontType.StandartFont, 110, new Color32(255, 225, 173, 255), TextAlignment.Center, true, "CharacterCreationSceneObject");
        
            #endregion

            #region ClassButtons

                var classesArray = new[] { "Barbarian", "Crusader", "Demon Hunter", "Monk", "Witch Doctor", "Wizard" };
                for (int i = 0; i < classesArray.Length; i++)
                {
                    StaticScripts.CreateGameObj(string.Format("{0}Button", classesArray[i]), @"Buttons/BattleNetButton_ClearUp_397x66", new Vector3(0.65f, 0.85f), new Vector3(-8.3f, 1.9f - 0.59f * i, 10f), true, 1, true, typeof(CreateCharacterClassMouseEvents), child: true, parentName: "CharacterCreationSceneObject");
                    var button = GameObject.Find(string.Format("{0}Button", classesArray[i]));
                    button.GetComponent<ButtonBaseMouseEvents>()._State = selectedClass.ToString() == classesArray[i].Replace(" ", string.Empty) ? ButtonState.Selected : ButtonState.Up;
                    StaticScripts.CreateTextObj(string.Format("{0}ButtonText", classesArray[i]), classesArray[i], new Vector3(0.02f, 0.02f), new Vector3(0f, 0f, 0f), FontType.DiabloFont, 100, new Color32(243, 170, 85, 255), TextAlignment.Center, child: true, parentName: string.Format("{0}Button", classesArray[i]));

                    var text = GameObject.Find(string.Format("{0}ButtonText", classesArray[i]));
                    text.transform.position = new Vector3(
                            (button.transform.position.x + (button.GetComponent<SpriteRenderer>().bounds.size.x / 2f)) -
                            (text.GetComponent<TextMesh>().GetComponent<Renderer>().bounds.size.x / 2f),
                            (button.transform.position.y + (button.GetComponent<SpriteRenderer>().bounds.size.y / 2f)) +
                           (text.GetComponent<TextMesh>().GetComponent<Renderer>().bounds.size.y / 2f));

                    if (classesArray[i].Replace(" ", string.Empty) == selectedClass.ToString())
                    {
                        StaticScripts.CreateGameObj("ClassPortrait", string.Format("Portraits/Portrait_{0}_{1}", classesArray[i].Replace(" ", ""), Account.CurrentAccount.GetCurrentHero().Gender ? "Male" : "Female"), new Vector3(1.5f, 1.5f), new Vector3(-1.65f, -1.15f), child: true, parentName: "CharacterCreationSceneObject");
                    }
                }


             #endregion

            StaticScripts.CreateTextObj("CreateHeroText", "Choose your class", new Vector3(0.02f, 0.02f, 0.02f), new Vector3(-0.15f, 0.45f, 0f), FontType.DiabloFont, 125, new Color32(255, 246, 222, 255), TextAlignment.Center, true, "CreateHeroHeaderBorder");


            StaticScripts.CreateGameObj("CreateHeroGenderBorder", @"Borders/CharacterCreation/BattlenetHeroCreate_GenderButtons", new Vector3(0.8f, 0.8f), new Vector3(-8f, -2.17f, 10f), child: true, parentName: "CharacterCreationSceneObject");
            StaticScripts.CreateGameObj("MaleButton", @"Icons/CharacterCreation/BattleNetHeroCreate_GenderMaleSelected", new Vector3(0.65f, 0.65f), new Vector3(-7.35f, -1.92f, 5f), true, 1, true, typeof(CharacterCreationGenderMouseEvents), true, "CharacterCreationSceneObject");

            StaticScripts.CreateGameObj("FemaleButton", @"Icons/CharacterCreation/BattleNetHeroCreate_GenderFemaleUp", new Vector3(0.65f, 0.65f), new Vector3(-6.55f, -1.92f, 5f), true, 1, true, typeof(CharacterCreationGenderMouseEvents), true, "CharacterCreationSceneObject");
            StaticScripts.CreateTextObj("MaleButtonText", "Male", new Vector3(0.02f, 0.02f), new Vector3(0.95f, 0.3f, 0f), FontType.DiabloFont, 80, new Color32(128, 100, 75, 255), TextAlignment.Center, true, "CreateHeroGenderBorder");
            StaticScripts.CreateTextObj("FemaleButtonText", "Female", new Vector3(0.02f, 0.02f), new Vector3(1.85f, 0.3f, 0f), FontType.DiabloFont, 80, new Color32(128, 100, 75, 255), TextAlignment.Center, true, "CreateHeroGenderBorder");

            StaticScripts.CreateGameObj("HeroicCheckBox", @"UI/BattleNetLogin_CheckboxUp", new Vector3(0.8f, 0.8f), new Vector3(-7.62f, -2.792f, 0f), true, 1, true, typeof(CharacterCreationHeroicCheckBoxScript), true, "CharacterCreationSceneObject");
            StaticScripts.CreateGameObj("HeroicIcon", @"Icons/CharacterCreation/BattleNet_HardcoreIcon", new Vector3(1f, 1f), new Vector3(0.4f, 0.01f, 0f), true, 1, child: true, parentName: "HeroicCheckBox");
            StaticScripts.CreateTextObj("HeroicText", "Hardcore Hero", new Vector3(0.02f, 0.02f), new Vector3(0.7f, 0.3f, 0f), FontType.StandartFont, 100, new Color32(128, 100, 75, 255), TextAlignment.Left, true, "HeroicCheckBox", FontStyle.Bold);
           
        }
        catch (Exception ex)
        {
            Debug.LogException(ex, this);
        }
    }

    void MainMenu()
    {
        Act5Background();
        var CharacterCreationGameObject = new GameObject("MainMenuSceneObject");
            CharacterCreationGameObject.transform.position = new Vector3(0, 0, 10);
            CharacterCreationGameObject.AddComponent<SpriteRenderer>().sprite = Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1),
                new Vector2());
        StaticScripts.CreateGameObj("DiabloLogo", "Logo/BattleNetLogo_Diablo3_x1", new Vector3(0.6f, 0.6f), new Vector3(-10.1f, 1.1f, 10f), sortingOrder:8, child: true, parentName: "MainMenuSceneObject");
        StaticScripts.CreateGameObj("StartGameButton", "Buttons/BattleNetButton_RedUp_398x82", new Vector3(0.82f, 0.8f), new Vector3(-8.33f, -0.1f, 10f), true, 1, true, typeof(ButtonBaseMouseEvents), sortingOrder: 9, child: true, parentName: "MainMenuSceneObject");
        GameObject.Find("StartGameButton").GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Disabled;
        StaticScripts.CreateTextObj("StartGameButtonText", "Start Game", new Vector3(0.02f, 0.02f), new Vector3(1.15f, 0.52f), FontType.DiabloFont, 120, new Color32(243, 170, 85, 255), TextAlignment.Center, true, "StartGameButton");
       
        StaticScripts.CreateGameObj("GameSettingsButton", "Buttons/BattleNetButton_ClearUp_260x50", new Vector3(0.95f, 0.65f), new Vector3(-7.95f, -0.55f, 10f), true, 1, true, typeof(ButtonBaseMouseEvents), true, "MainMenuSceneObject");
       // GameObject.Find("GameSettingsButton").GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Disabled;
        StaticScripts.CreateTextObj("GameSettingsButtonText", "Game settings", new Vector3(0.02f, 0.02f), new Vector3(0.65f, 0.4f), FontType.DiabloFont, 75, new Color32(243, 170, 85, 255), TextAlignment.Center, true, "GameSettingsButton");
        
        StaticScripts.CreateGameObj("SwitchHeroButton", "Buttons/BattleNetButton_RedUp_262x50", new Vector3(0.85f, 0.8f), new Vector3(-1.15f, -3.8f, 10f), true, 1, true, typeof(ButtonBaseMouseEvents), true, "MainMenuSceneObject");
        StaticScripts.CreateTextObj("SwitchHeroButtonText", "Switch Hero", new Vector3(0.02f, 0.02f), new Vector3(0.78f, 0.31f), FontType.DiabloFont, 70, new Color32(243, 170, 85, 255), TextAlignment.Center, true, "SwitchHeroButton");
        GameObject.Find("SwitchHeroButton").GetComponent<ButtonBaseMouseEvents>().MethodOnClick += SelectHeroMenu;

        StaticScripts.CreateGameObj("ProfileButton", "Buttons/BattleNetButton_ClearUp_74x66", new Vector3(0.85f, 0.85f), new Vector3(6.15f, -4.65f, 10f), true, 1, true, typeof(ButtonBaseMouseEvents), true, "MainMenuSceneObject");
        GameObject.Find("ProfileButton").GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Disabled;
        StaticScripts.CreateGameObj("ProfileButtonIcon", "Icons/BattleNetFooter_ProfileButtonIcon", new Vector3(1f, 1f), new Vector3(0.23f, 0.15f, 10f), child: true, parentName: "ProfileButton");

        StaticScripts.CreateGameObj("LeaderboardButton", "Buttons/BattleNetButton_ClearUp_74x66", new Vector3(0.85f, 0.85f), new Vector3(6.8f, -4.65f, 10f), true, 1, true, typeof(ButtonBaseMouseEvents), true, "MainMenuSceneObject");
        GameObject.Find("LeaderboardButton").GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Disabled;
        StaticScripts.CreateGameObj("LeaderboardButtonIcon", "Icons/BattleNetFooter_LeaderboardButtonIcon", new Vector3(1f, 1f), new Vector3(0.23f, 0.15f, 10f), child: true, parentName: "LeaderboardButton");

        StaticScripts.CreateGameObj("AchievementsButton", "Buttons/BattleNetButton_ClearUp_74x66", new Vector3(0.85f, 0.85f), new Vector3(7.45f, -4.65f, 10f), true, 1, true, typeof(ButtonBaseMouseEvents), true, "MainMenuSceneObject");
        GameObject.Find("AchievementsButton").GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Disabled;
        StaticScripts.CreateGameObj("AchievementsButtonIcon", "Icons/BattleNetFooter_AchievementsButtonIcon", new Vector3(1f, 1f),new Vector3(0.26f, 0.18f, 10f), child: true, parentName: "AchievementsButton");

        StaticScripts.CreateGameObj("OptionsButton", "Buttons/BattleNetButton_ClearUp_74x66", new Vector3(0.85f, 0.85f), new Vector3(8.1f, -4.65f, 10f), true, 1, true, typeof(ButtonBaseMouseEvents), true, "MainMenuSceneObject");
        GameObject.Find("OptionsButton").GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Disabled;
        StaticScripts.CreateGameObj("OptionsButtonIcon", "Icons/BattleNetFooter_OptionsButtonIcon", new Vector3(1f, 1f), new Vector3(0.23f, 0.15f, 10f), child: true, parentName: "OptionsButton");
    }

    void SelectHeroMenu()
    {
        var prevMenu = GameObject.Find("MainMenuSceneObject");
        if(prevMenu != null) prevMenu.SetActive(false);
        Act5Background();
        var CharacterCreationGameObject = new GameObject("CharacterSelectionSceneObject");
        CharacterCreationGameObject.transform.position = new Vector3(0, 0, 10);
        CharacterCreationGameObject.AddComponent<SpriteRenderer>().sprite = Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1),
            new Vector2());

        StaticScripts.CreateGameObj("CreateHeroButton", @"Buttons/BattleNetButton_RedUp_397x66", new Vector3(0.85f, 0.85f), new Vector3(-8.2f, -3f, 10f), true, 1, true, typeof(ButtonBaseMouseEvents), child: true, parentName: "CharacterSelectionSceneObject");
        GameObject.Find("CreateHeroButton").GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Disabled;
        StaticScripts.CreateTextObj("CreateHeroButtonText", "Create", new Vector3(0.02f, 0.02f), new Vector3(1.6f, 0.4f, 0f), FontType.DiabloFont, 90, new Color32(243, 170, 85, 255), TextAlignment.Center, true, "CreateHeroButton");

        StaticScripts.CreateGameObj("SelectHeroButton", @"Buttons/BattleNetButton_RedUp_397x66", new Vector3(0.85f, 0.85f), new Vector3(-1.7f, -3.9f, 10f), true, 1, true, typeof(ButtonBaseMouseEvents), child: true, parentName: "CharacterSelectionSceneObject");
        GameObject.Find("SelectHeroButton").GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Disabled;
        StaticScripts.CreateTextObj("SelectHeroButtonText", "Select Hero", new Vector3(0.02f, 0.02f), new Vector3(1.3f, 0.41f, 0f), FontType.DiabloFont, 90, new Color32(243, 170, 85, 255), TextAlignment.Center, true, "SelectHeroButton");

        StaticScripts.CreateGameObj("CancelButton", @"Buttons/BattleNetButton_BlueUp_262x50", new Vector3(0.8f, 0.8f), new Vector3(-7.7f, -3.8f, 10f), true, 1, true, typeof(ButtonBaseMouseEvents), child: true, parentName: "CharacterSelectionSceneObject");
        GameObject.Find("CancelButton").GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Disabled;
        StaticScripts.CreateTextObj("CancelButtonText", "Cancel", new Vector3(0.02f, 0.02f), new Vector3(1f, 0.32f, 0f), FontType.DiabloFont, 80, new Color32(243, 170, 85, 255), TextAlignment.Center, true, "CancelButton");

        StaticScripts.CreateGameObj("DeleteHeroButton", @"Buttons/BattleNetButton_BlueUp_262x50", new Vector3(0.8f, 0.8f), new Vector3(5.75f, -3.8f, 10f), true, 1, true, typeof(ButtonBaseMouseEvents), child: true, parentName: "CharacterSelectionSceneObject");
        GameObject.Find("DeleteHeroButton").GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Disabled;
        StaticScripts.CreateTextObj("DeleteHeroButtonText", "Delete", new Vector3(0.02f, 0.02f), new Vector3(1f, 0.32f, 0f), FontType.DiabloFont, 80, new Color32(243, 170, 85, 255), TextAlignment.Center, true, "DeleteHeroButton");

        StaticScripts.CreateGameObj("SelectHeroHeaderBorder", @"Borders/CharacterCreation/BattlenetHeroCreate_Header", new Vector3(1.15f, 1.15f), new Vector3(-8.1f, 3.5f, 10f), child: true, parentName: "CharacterSelectionSceneObject");
        StaticScripts.CreateTextObj("SelectHeroHeaderBorderText", "Select a Hero", new Vector3(0.02f, 0.02f), new Vector3(0.45f, 0.43f, 0f), FontType.DiabloFont, 100, new Color32(255, 246, 222, 255), TextAlignment.Center, true, "SelectHeroHeaderBorder");



        StaticScripts.CreateTextObj("HeroSlotRemainingText", string.Format("{0} Hero Slots Remaining", 12 - Account.CurrentAccount.ListOfHeroes.Count), new Vector3(0.02f, 0.02f), new Vector3(-7.4f, -3.15f, 0f), FontType.StandartFont, 65, new Color32(243, 170, 85, 255), TextAlignment.Center, true, "CharacterSelectionSceneObject");

        var HeroListObject = new GameObject("HeroListGameObject");
        CharacterCreationGameObject.transform.position = new Vector3(0, 0, 10);
        HeroListObject.AddComponent<HeroScrollingList>();


        var shader = Shader.Find("Masked/Mask");

        var DepthMaskTop = new GameObject("DepthMaskTopGameObject");
        DepthMaskTop.AddComponent<SpriteRenderer>().sprite = Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1), new Vector2());
        DepthMaskTop.GetComponent<SpriteRenderer>().material = new Material(shader);
        DepthMaskTop.transform.localScale = new Vector3(500, 200);
        DepthMaskTop.transform.position = new Vector3(-9f, 3.35f, 140f);

        var DepthMaskBottom = new GameObject("DepthMaskBottomGameObject");
        DepthMaskBottom.AddComponent<SpriteRenderer>().sprite = Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1), new Vector2());
        DepthMaskBottom.GetComponent<SpriteRenderer>().material = new Material(shader);
        DepthMaskBottom.transform.localScale = new Vector3(500, 300);
        DepthMaskBottom.transform.position = new Vector3(-9f, -5.31f, 140f);

    }

    void Act5Background()
    {
        var gameObj = GameObject.Find("BackgroundObject");
        if (gameObj != null)
        {
            DestroyImmediate(gameObj);
        }

        var CharacterCreationGameObject = new GameObject("BackgroundObject");
        CharacterCreationGameObject.transform.position = new Vector3(0, 0, 10);
        CharacterCreationGameObject.AddComponent<SpriteRenderer>().sprite = Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1),
            new Vector2());

        StaticScripts.CreateGameObj("StairsObject", "Backgrounds/Act5/Act5_foreground_0005_Layer-6", new Vector3(0.95f, 0.95f), new Vector3(-9.9f, -7.7f, 20f), child:true, parentName:"BackgroundObject");
        StaticScripts.CreateGameObj("FloorObject", "Backgrounds/Act5/Act5_foreground_0004_Layer-5", new Vector3(0.87f, 0.87f), new Vector3(-9f, -2.35f, 30f), child: true, parentName: "BackgroundObject");
        StaticScripts.CreateGameObj("GatesObject", "Backgrounds/Act5/Act5_bridge_0000_Layer-1", new Vector3(0.76f, 0.76f), new Vector3(-8f, -1.65f, 35f), child: true, parentName: "BackgroundObject");

        StaticScripts.CreateGameObj("House1", "Backgrounds/Act5/Act5_foreground_0002_Layer-3", new Vector3(0.6f, 0.6f), new Vector3(-5.7f, 1.4f, 45f), child: true, parentName: "BackgroundObject");
        StaticScripts.CreateGameObj("House2", "Backgrounds/Act5/Act5_buildings_0000_Layer-1", new Vector3(0.65f, 0.65f), new Vector3(-2.3f, -1f, 40f), child:true, parentName:"BackgroundObject" );

        StaticScripts.CreateGameObj("House3", "Backgrounds/Act5/Act5_buildings_0004_Layer-5", new Vector3(0.75f, 0.75f), new Vector3(2.35f, 0.73f, 45f), child: true, parentName: "BackgroundObject");
        StaticScripts.CreateGameObj("House4", "Backgrounds/Act5/Act5_buildings_0002_Layer-3", new Vector3(0.6f, 0.6f), new Vector3(2.3f, 1f, 50f), child: true, parentName: "BackgroundObject");
        StaticScripts.CreateGameObj("House5", "Backgrounds/Act5/Act5_buildings_0005_Layer-6", new Vector3(0.75f, 0.75f), new Vector3(-3.25f, -0.05f, 40f), child: true, parentName: "BackgroundObject");

        StaticScripts.CreateGameObj("Statue1", "Backgrounds/Act5/Act5_foreground_0000_Layer-1", new Vector3(0.8f, 0.8f), new Vector3(-9.8f, -2f, 25f), child: true, parentName: "BackgroundObject");
        StaticScripts.CreateGameObj("Statue2", "Backgrounds/Act5/Act5_foreground_0001_Layer-2", new Vector3(0.75f, 0.75f), new Vector3(6.15f, -1.2f, 25f), child: true, parentName: "BackgroundObject");
    }

    void Act1Background()
    {
        var gameObj = GameObject.Find("BackgroundObject");
        if (gameObj != null)
        {
            DestroyImmediate(gameObj);
        }

        var CharacterCreationGameObject = new GameObject("BackgroundObject");
        CharacterCreationGameObject.transform.position = new Vector3(0, 0, 10);
        CharacterCreationGameObject.AddComponent<SpriteRenderer>().sprite = Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1),
            new Vector2());

        StaticScripts.CreateGameObj("CharacterCreationBackground", @"Backgrounds/CharacterCreation/Battlenet_MainScreenBackground_flipped", new Vector3(1.1f, 1.1f), new Vector3(-12.1f, -5.5f, 100f), child: true, parentName: "BackgroundObject");
        StaticScripts.CreateGameObj("CharacterCreationTree", @"Backgrounds/CharacterCreation/Battlenet_MainScreenBackground_Alpha_flipped", new Vector3(1.3f, 1.3f), new Vector3(-11.65f, -6.5f, 95f), child: true, parentName: "BackgroundObject");
      
        #region CharacterPlatform
        StaticScripts.CreateGameObj("platform2", @"Backgrounds/CharacterCreation/CharacterPlatform_0000_Layer-1", new Vector3(1.1f, 0.6f), new Vector3(-2.3f, -5.8f, 60f), child: true, parentName: "BackgroundObject");
        StaticScripts.CreateGameObj("platform1", @"Backgrounds/CharacterCreation/CharacterPlatform_0001_Layer-2", new Vector3(1.15f, 0.9f), new Vector3(-4.88f, -5.45f, 50f), child: true, parentName: "BackgroundObject");
        StaticScripts.CreateGameObj("platform3", @"Backgrounds/CharacterCreation/CharacterPlatform_0004_Layer-5", new Vector3(1.2f, 1.3f), new Vector3(-12.30f, -8.55f, 45f), child: true, parentName: "BackgroundObject");
        StaticScripts.CreateGameObj("platform4", @"Backgrounds/CharacterCreation/CharacterPlatform_0003_Layer-4", new Vector3(0.7f, 0.7f), new Vector3(-4f, -4f, 55f), child: true, parentName: "BackgroundObject");
        StaticScripts.CreateGameObj("platform5", @"Backgrounds/CharacterCreation/CharacterPlatform_0002_Layer-3", new Vector3(1f, 1f), new Vector3(6.57f, -1.65f, 40f), child: true, parentName: "BackgroundObject");
        #endregion

    }

    void LoginMenu()
    {
        var LoginGameObject = new GameObject("LoginSceneObject");
        LoginGameObject.transform.position = new Vector3(0, 0, 10);
        LoginGameObject.AddComponent<SpriteRenderer>().sprite = Sprite.Create(new Texture2D(1, 1), new Rect(0, 0, 1, 1),
            new Vector2());

        StaticScripts.CreateGameObj("LoginAndPasswordTextBoxes", @"UI/Login/BattleNetLogin_InputContainer", new Vector3(0.85f, 0.85f), new Vector3(-1.96f, -2.54f, 10f), true, 1, true, typeof(LoginPasswordTextBoxesScript), true, "LoginSceneObject");
        StaticScripts.CreateGameObj("Diablo3Logo", @"Logo/BattleNetLogo_Diablo3_x1", new Vector3(0.82f, 0.85f), new Vector3(-4.69f, -1.13f, 5f), child: true, parentName: "LoginSceneObject");
        StaticScripts.CreateGameObj("RememberMe", @"UI/BattleNetLogin_CheckboxUp", new Vector3(0.85f, 0.85f), new Vector3(-0.98f, -2.195f, 0f), true, 1, true, typeof(CheckBoxMouseEvents), child: true, parentName: "LoginSceneObject");
        StaticScripts.CreateTextObj("AccountNameText", "Account Name", new Vector3(0.02f, 0.02f, 0.02f), new Vector3(-0.972f, 0.17f, 0f), FontType.DiabloFont, 110, new Color32(255, 246, 222, 255), child: true, parentName: "LoginSceneObject");
        StaticScripts.CreateTextObj("PasswordText", "Password", new Vector3(0.02f, 0.02f, 0.02f), new Vector3(-0.635f, -0.92f, 0f), FontType.DiabloFont, 110, new Color32(255, 246, 222, 255), child: true, parentName: "LoginSceneObject");


        StaticScripts.CreateGameObj("ManageAccountsButton", @"Buttons/BattleNetButton_BlueUp_262x50", new Vector3(0.82f, 0.85f), new Vector3(-8.11f, -1.83f, 10f), true, 1, true, typeof(ButtonBaseMouseEvents), child: true, parentName: "LoginSceneObject");
        GameObject.Find("ManageAccountsButton").GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Disabled;
        StaticScripts.CreateTextObj("ManageAccountsButtonText", "Manage Accounts", new Vector3(0.02f, 0.02f, 0.02f), new Vector3(-7.8f, -1.55f, 0f), FontType.DiabloFont, 70, new Color32(243, 170, 85, 255), child: true, parentName: "LoginSceneObject");

        StaticScripts.CreateGameObj("OptionsButton", @"Buttons/BattleNetButton_BlueUp_262x50", new Vector3(0.82f, 0.85f), new Vector3(-8.11f, -2.28f, 10f), true, 1, true, typeof(ButtonBaseMouseEvents), child: true, parentName: "LoginSceneObject");
        GameObject.Find("OptionsButton").GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Disabled;
        StaticScripts.CreateTextObj("OptionsButtonText", "Options", new Vector3(0.02f, 0.02f, 0.02f), new Vector3(-7.37f, -2f, 0f), FontType.DiabloFont, 70, new Color32(243, 170, 85, 255), child: true, parentName: "LoginSceneObject");

        StaticScripts.CreateGameObj("CreditsButton", @"Buttons/BattleNetButton_BlueUp_262x50", new Vector3(0.82f, 0.85f), new Vector3(-8.11f, -2.73f, 10f), true, 1, true, typeof(ButtonBaseMouseEvents), child: true, parentName: "LoginSceneObject");
        GameObject.Find("CreditsButton").GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Disabled;
        StaticScripts.CreateTextObj("CreditsButtonText", "Credits", new Vector3(0.02f, 0.02f, 0.02f), new Vector3(-7.35f, -2.45f, 0f), FontType.DiabloFont, 70, new Color32(243, 170, 85, 255), child: true, parentName: "LoginSceneObject");

        StaticScripts.CreateGameObj("ExitButton", @"Buttons/BattleNetButton_BlueUp_262x50", new Vector3(0.82f, 0.85f), new Vector3(-8.11f, -3.95f, 10f), true, 1, true, typeof(ExitButtonScript), child: true, parentName: "LoginSceneObject");
        StaticScripts.CreateTextObj("ExitButtonText", "Exit", new Vector3(0.02f, 0.02f, 0.02f), new Vector3(-7.25f, -3.67f, 0f), FontType.DiabloFont, 70, new Color32(243, 170, 85, 255), child: true, parentName: "LoginSceneObject");

        StaticScripts.CreateGameObj("LoginButton", @"Buttons/BattleNetButton_RedDisabled_397x66", new Vector3(0.845f, 0.85f), new Vector3(-1.66f, -3.2f, 10f), true, 1, true, typeof(LoginButtonScript), child: true, parentName: "LoginSceneObject");
        GameObject.Find("LoginButton").GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Disabled;
        StaticScripts.CreateTextObj("LoginButtonText", "Login", new Vector3(0.02f, 0.02f, 0.02f), new Vector3(-0.3f, -2.84f, 0f), FontType.DiabloFont, 96, new Color32(243, 170, 85, 255), child: true, parentName: "LoginSceneObject");

        StaticScripts.CreateGameObj("BreakingNews", @"UI/Login/BattleNetLogin_BreakingNewsBg", new Vector3(0.85f, 0.85f), new Vector3(5.3f, -4f), child: true, parentName: "LoginSceneObject");
        StaticScripts.CreateTextObj("BreakingNewsHeaderText", "Login Help", new Vector3(0.02f, 0.02f), new Vector3(0.95f, 4.15f, 0f), FontType.DiabloFont, 150, new Color32(255, 246, 222, 255), child: true, parentName: "BreakingNews");
        StaticScripts.CreateTextObj("BreakingNewsBodyText", "\tYou can create new account by entering \ndata in \"Account Name\" and \"Password\" fields. \nBe sure that you have at least 6 characters at \n\"Account Name\" field and at least 1 character \nat \"Password\" field.", new Vector3(0.02f, 0.02f), new Vector3(0.2f, 3.2f, 0f), FontType.StandartFont, 80, new Color32(255, 246, 222, 255), child: true, parentName: "BreakingNews");
    }

    void ShowTopAndBottomBorders()
    {
        StaticScripts.CreateGameObj("footer", @"Backgrounds/CharacterCreation/BattleNetFooter_BG_4x3", new Vector3(1f, 0.5f), new Vector3(-10.24f, -5f, 10f), sortingOrder:7);
        StaticScripts.CreateGameObj("footerRight", @"Backgrounds/CharacterCreation/BattleNetFooter_BG_EndCapRight", new Vector3(1f, 0.5f), new Vector3(10.24f, -5f, 10f), sortingOrder: 7);
        StaticScripts.CreateGameObj("footerLeft", @"Backgrounds/CharacterCreation/BattleNetFooter_BG_EndCapLeft", new Vector3(1f, 0.5f), new Vector3(-12.8f, -5f, 10f), sortingOrder: 7);

        StaticScripts.CreateGameObj("header", @"Backgrounds/CharacterCreation/BattleNetHeader_BG_4x3", new Vector3(0.7f, 1f), new Vector3(-7.168f, 4.36f, 10f), sortingOrder: 7);
        StaticScripts.CreateGameObj("headerLeft", @"Backgrounds/CharacterCreation/BattleNetHeader_BG_EndCapLeft", new Vector3(0.7f, 1f), new Vector3(-8.96f, 4.36f, 10f), sortingOrder: 7);
        StaticScripts.CreateGameObj("headerRight", @"Backgrounds/CharacterCreation/BattleNetHeader_BG_EndCapRight", new Vector3(0.7f, 1f), new Vector3(7.168f, 4.36f, 10f), sortingOrder: 7);
    }

    void OnDestroy()
    {
        ReadWriteAccountDateScript.WriteAccountData(Account.CurrentAccount.AccountName);
    }

    void MakeCameraDarker()
    {
        var texture = new Texture2D(1, 1);
        var obj = new GameObject("CameraDarkFilter");
        obj.transform.localScale = new Vector3(1920, 1080);
        obj.AddComponent<SpriteRenderer>().sprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2());
        obj.GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, 100);
        obj.transform.position = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 5f));
    }
	void ReadDDS(string path, bool alpha)
	{
        /*
         * Метод для чтения изначально "выдранных" из Diablo 3 dds файлов, которые Unity отказывается считывать правильно - появляется сдвиг на 32 пикселя вправо, а сама картинка перевернута. 
         * Кроме того, файлы, в которых объедененны несколько спрайтов, имеют при себе txt файл с набором "Имя" - "Координаты", благодаря которым можно порезать файл на отдельные спрайты.
         */
	    int height, width;
        using (var br = new BinaryReader(File.Open(path, FileMode.Open)))
        {
            br.ReadBytes(12);
            height = BitConverter.ToInt32(br.ReadBytes(4),0);
            width = BitConverter.ToInt32(br.ReadBytes(4), 0);
            br.ReadBytes(64);
            
            var type = Encoding.UTF8.GetString(br.ReadBytes(4));

            if (type != "DXT5" && type != "DXT3") return;
        }

	    var bytes = File.ReadAllBytes(path);
        var texture = new Texture2D((int)width, (int)height, TextureFormat.DXT5, false);
	    var nonflippedtexture = new Texture2D(texture.width, texture.height);
	    texture.LoadRawTextureData(bytes.ToArray());
        for (int i = 0; i < texture.width; i++)
        {
            for (int j = 0; j < texture.height; j++)
            {
                var pixel = texture.GetPixel(i, j);
                nonflippedtexture.SetPixel(i, j, pixel);
            }
        }
        nonflippedtexture.Apply();
        var flippedtexture = new Texture2D(texture.width, texture.height);
        for (int i = 0; i < texture.width; i++)
        {
            for (int j = 0; j < texture.height; j++)
            {
                var pixel = texture.GetPixel(i, j);
                if (alpha)
                    pixel.a = 1;
                if (i > 31)
                    flippedtexture.SetPixel(i - 31, texture.height - 1 - j, pixel);
                else
                {
                    flippedtexture.SetPixel(width - 1 - (31 - i), texture.height - j, pixel);
                }
            }
        }

        for (int i = flippedtexture.width - 31; i < flippedtexture.width; i++)
        {
            var arr = flippedtexture.GetPixels(i, 0, 1, flippedtexture.height);
            var flippedArray = new Texture2D(1, flippedtexture.height).GetPixels();
            Array.Copy(arr, 0, flippedArray, 3, arr.Length - 3);
            Array.Copy(arr, arr.Length - 3, flippedArray, 0, 3);
            flippedtexture.SetPixels(i, 0, 1, flippedtexture.height, flippedArray);
        }

        flippedtexture.Apply();

	    using (var br = new BinaryWriter(new FileStream(path.Replace(".dds", "_flipped.png"), FileMode.OpenOrCreate)))
	    {
	        br.Write(flippedtexture.EncodeToPNG());
	    }

	    if (File.Exists(path.Replace(".dds", "_atlas.txt")))
	    {
	        using (var sr = new StreamReader(path.Replace(".dds", "_atlas.txt")))
	        {
	            sr.ReadLine();
	            while (!sr.EndOfStream)
	            {
                    var FirstLine = sr.ReadLine().Split('\t');
	                var tex = new Texture2D(int.Parse(FirstLine[7]) - (int.Parse(FirstLine[5]) + 1),
	                    int.Parse(FirstLine[8]) - (int.Parse(FirstLine[6]) + 2));
	                int x = 0;
	                for (int i = int.Parse(FirstLine[5]) + 1; i <= int.Parse(FirstLine[7]); i++)
	                {
	                    int y = 0;
	                    for (int j = int.Parse(FirstLine[6]) + 1; j < int.Parse(FirstLine[8]); j++)
	                    {
	                        var color = flippedtexture.GetPixel(i, flippedtexture.height-j);
	                        tex.SetPixel(x, tex.height - y, color);
	                        y++;
	                    }
	                    x++;
	                }
                    tex.Apply();
                    using (var br = new BinaryWriter(new FileStream(FirstLine[0] + ".png", FileMode.OpenOrCreate)))
	                {
	                    br.Write(tex.EncodeToPNG());
	                }
	            }
	        }
	    }
	}
}




