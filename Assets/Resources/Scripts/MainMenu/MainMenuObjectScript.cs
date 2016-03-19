using UnityEngine;
using System.Collections;
using System.Linq;
using Assets.Resources.Scripts;

public class MainMenuObjectScript : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        StaticScripts.CreateGameObj("DiabloLogo", "Logo/BattleNetLogo_Diablo3_x1", new Vector3(0.6f, 0.6f), new Vector3(-10.1f, 1.1f, 10f), sortingOrder: 8, child: true, parentName: "MainMenuSceneObject");
        var startGameButton = StaticScripts.CreateGameObj("StartGameButton", "Buttons/BattleNetButton_RedUp_398x82", new Vector3(0.82f, 0.8f), new Vector3(-8.33f, -0.1f, 10f), true, 1, true, typeof(ButtonBaseMouseEvents), sortingOrder: 9, child: true, parentName: "MainMenuSceneObject");
        startGameButton.GetComponent<ButtonBaseMouseEvents>().MethodOnClick += delegate
        {
            if (!GameObject.Find("TristramMainObject"))
            {
                Application.LoadLevelAdditive("TristramScene");
                GameObject.Find("MainLogic").SetActive(false); 
            }
            else
            {
                GameObject.Find("TristramMainObject").transform.Cast<Transform>()
                    .FirstOrDefault(child => child.name == "SceneObjects").gameObject.SetActive(true);

                GameObject.Find("MainLogic").SetActive(false);
            }
        };
        StaticScripts.CreateTextObj("StartGameButtonText", "Start Game", new Vector3(0.02f, 0.02f), new Vector3(1.15f, 0.52f), FontType.DiabloFont, 120, new Color32(243, 170, 85, 255), TextAlignment.Center, true, "StartGameButton");

        var gameSettingsButton = StaticScripts.CreateGameObj("GameSettingsButton", "Buttons/BattleNetButton_ClearUp_260x50", new Vector3(0.95f, 0.65f), new Vector3(-7.95f, -0.55f, 10f), true, 1, true, typeof(ButtonBaseMouseEvents), true, "MainMenuSceneObject");
        gameSettingsButton.GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Disabled;
        StaticScripts.CreateTextObj("GameSettingsButtonText", "Game settings", new Vector3(0.02f, 0.02f), new Vector3(0.65f, 0.4f), FontType.DiabloFont, 75, new Color32(243, 170, 85, 255), TextAlignment.Center, true, "GameSettingsButton");

        var switchHeroButton = StaticScripts.CreateGameObj("SwitchHeroButton", "Buttons/BattleNetButton_RedUp_262x50", new Vector3(0.85f, 0.8f), new Vector3(-1.15f, -3.8f, 10f), true, 1, true, typeof(ButtonBaseMouseEvents), true, "MainMenuSceneObject");
        StaticScripts.CreateTextObj("SwitchHeroButtonText", "Switch Hero", new Vector3(0.02f, 0.02f), new Vector3(0.78f, 0.31f), FontType.DiabloFont, 70, new Color32(243, 170, 85, 255), TextAlignment.Center, true, "SwitchHeroButton");
        switchHeroButton.GetComponent<ButtonBaseMouseEvents>().MethodOnClick += delegate { GameObject.Find("MainLogic").GetComponent<Main>().CurrentGameState = GameState.CharacterSelection; };

        var profileButton = StaticScripts.CreateGameObj("ProfileButton", "Buttons/BattleNetButton_ClearUp_74x66", new Vector3(0.85f, 0.85f), new Vector3(6.15f, -4.65f, 10f), true, 1, true, typeof(ButtonBaseMouseEvents), true, "MainMenuSceneObject");
        profileButton.GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Disabled;
        StaticScripts.CreateGameObj("ProfileButtonIcon", "Icons/BattleNetFooter_ProfileButtonIcon", new Vector3(1f, 1f), new Vector3(0.23f, 0.15f, 10f), child: true, parentName: "ProfileButton");

        var leaderboardButton = StaticScripts.CreateGameObj("LeaderboardButton", "Buttons/BattleNetButton_ClearUp_74x66", new Vector3(0.85f, 0.85f), new Vector3(6.8f, -4.65f, 10f), true, 1, true, typeof(ButtonBaseMouseEvents), true, "MainMenuSceneObject");
        leaderboardButton.GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Disabled;
        StaticScripts.CreateGameObj("LeaderboardButtonIcon", "Icons/BattleNetFooter_LeaderboardButtonIcon", new Vector3(1f, 1f), new Vector3(0.23f, 0.15f, 10f), child: true, parentName: "LeaderboardButton");

        var achievementButton = StaticScripts.CreateGameObj("AchievementsButton", "Buttons/BattleNetButton_ClearUp_74x66", new Vector3(0.85f, 0.85f), new Vector3(7.45f, -4.65f, 10f), true, 1, true, typeof(ButtonBaseMouseEvents), true, "MainMenuSceneObject");
        achievementButton.GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Disabled;
        StaticScripts.CreateGameObj("AchievementsButtonIcon", "Icons/BattleNetFooter_AchievementsButtonIcon", new Vector3(1f, 1f), new Vector3(0.26f, 0.18f, 10f), child: true, parentName: "AchievementsButton");

        var optionButton = StaticScripts.CreateGameObj("OptionsButton", "Buttons/BattleNetButton_ClearUp_74x66", new Vector3(0.85f, 0.85f), new Vector3(8.1f, -4.65f, 10f), true, 1, true, typeof(ButtonBaseMouseEvents), true, "MainMenuSceneObject");
        optionButton.GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Disabled;
        StaticScripts.CreateGameObj("OptionsButtonIcon", "Icons/BattleNetFooter_OptionsButtonIcon", new Vector3(1f, 1f), new Vector3(0.23f, 0.15f, 10f), child: true, parentName: "OptionsButton");
	}

    void OnEnable()
    {
        UpdateHeroPortraitAndName();
    }

    void OnDisable()
    {
        var switchHeroButton = gameObject.transform.FindChild("SwitchHeroButton");
        if(switchHeroButton && switchHeroButton.GetComponent<ButtonBaseMouseEvents>()._State != ButtonState.Disabled)
            switchHeroButton.gameObject.GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Up;
    }

    void UpdateHeroPortraitAndName()
    {
        var classPortraitObject = gameObject.transform.FindChild("ClassPortrait");
        if (!classPortraitObject)
            StaticScripts.CreateGameObj("ClassPortrait", string.Format("Portraits/Heroes/Portrait_{0}_{1}", Account.CurrentAccount.GetCurrentHero().Class,
                    Account.CurrentAccount.GetCurrentHero().Gender ? "Male" : "Female"), new Vector3(1.5f, 1.5f), new Vector3(-1.65f, -1.15f), child: true, parentName: "MainMenuSceneObject");
        else
            classPortraitObject.GetComponent<SpriteRenderer>().sprite = StaticScripts.CreateSprite(string.Format("Portraits/Heroes/Portrait_{0}_{1}",
                    Account.CurrentAccount.GetCurrentHero().Class, Account.CurrentAccount.GetCurrentHero().Gender ? "Male" : "Female"));


        var classSmallPortraitObject = gameObject.transform.FindChild("Hero_portrait");
        if (!classSmallPortraitObject)
        {
            StaticScripts.CreateGameObj("Hero_portrait_background", @"Portraits/Heroes/ParagonBorders/Prestige_PortraitCircle_01", new Vector3(0.9f, 0.9f),
                new Vector3(-0.5f, -3.45f, 15f), child: true, parentName: "MainMenuSceneObject");
        }
        string pathToTexture = string.Empty;
        switch (Account.CurrentAccount.GetCurrentHero().Class)
        {
            case GameClasses.Barbarian:
                pathToTexture = Account.CurrentAccount.GetCurrentHero().Gender ? @"Portraits/Heroes/Portrait_Barbarian_Male" : @"Portraits/Heroes/Portrait_Barbarian_Female";
                break;
            case GameClasses.Crusader:
                pathToTexture = Account.CurrentAccount.GetCurrentHero().Gender ? @"Portraits/Heroes/Portrait_Crusader_Male" : @"Portraits/Heroes/Portrait_Crusader_Female";
                break;
            case GameClasses.DemonHunter:
                pathToTexture = Account.CurrentAccount.GetCurrentHero().Gender ? @"Portraits/Heroes/Portrait_Demonhunter_Male" : @"Portraits/Heroes/Portrait_Demonhunter_Female";
                break;
            case GameClasses.Monk:
                pathToTexture = Account.CurrentAccount.GetCurrentHero().Gender ? @"Portraits/Heroes/Portrait_Monk_Male" : @"Portraits/Heroes/Portrait_Monk_Female";
                break;
            case GameClasses.WitchDoctor:
                pathToTexture = Account.CurrentAccount.GetCurrentHero().Gender ? @"Portraits/Heroes/Portrait_Witchdoctor_Male" : @"Portraits/Heroes/Portrait_Witchdoctor_Female";
                break;
            case GameClasses.Wizard:
                pathToTexture = Account.CurrentAccount.GetCurrentHero().Gender ? @"Portraits/Heroes/Portrait_Wizard_Male" : @"Portraits/Heroes/Portrait_Wizard_Female";
                break;
        }
        if(!classSmallPortraitObject)
            StaticScripts.CreateGameObj("Hero_portrait", pathToTexture, new Vector3(0.18f, 0.18f), new Vector3(-0.21f, -2.83f, 10f), child: true, parentName: "MainMenuSceneObject");
        else
            classSmallPortraitObject.GetComponent<SpriteRenderer>().sprite = StaticScripts.CreateSprite(pathToTexture);

        if (!classSmallPortraitObject)
            StaticScripts.CreateGameObj("Hero_border", @"Portraits/Heroes/ParagonBorders/Prestige_PortraitFrame_00",
                new Vector3(0.85f, 0.85f), new Vector3(-0.45f, -3.35f, 5f), child: true, parentName: "MainMenuSceneObject");

        if (!classSmallPortraitObject)
            StaticScripts.CreateTextObj("HeroLevel", Account.CurrentAccount.GetCurrentHero().Level.ToString(),
                new Vector3(0.02f, 0.02f), new Vector3(Account.CurrentAccount.GetCurrentHero().Level > 9 ? 0.4f : 0.45f, 1.35f, 10f), FontType.StandartFont, 80, Color.white,
                TextAlignment.Center, true, "Hero_border");
        else
        {
            var border = gameObject.transform.FindChild("Hero_border");
            if (border)
            {
                var levelTextObject = border.FindChild("HeroLevel");
                levelTextObject.gameObject.GetComponent<TextMesh>().text = Account.CurrentAccount.GetCurrentHero().Level.ToString();
                levelTextObject.gameObject.transform.localPosition = new Vector3(Account.CurrentAccount.GetCurrentHero().Level > 9 ? 0.4f : 0.45f, 1.35f, 10f);
            }
        }

        if (!classSmallPortraitObject)
        {
            var nameTextObject =  StaticScripts.CreateTextObj("HeroName", Account.CurrentAccount.GetCurrentHero().Name,
                new Vector3(0.02f, 0.02f), new Vector3(0.45f, 1.6f, 10f), FontType.StandartFont, 100, new Color32(131, 176, 209, 255),
                TextAlignment.Center, true, "Hero_border");
            nameTextObject.transform.localPosition = new Vector3(nameTextObject.transform.localPosition.x - nameTextObject.GetComponent<Renderer>().bounds.size.x/2, nameTextObject.transform.localPosition.y);
        }
        else
        {
            var border = gameObject.transform.FindChild("Hero_border");
            if (border)
            {
                var nameTextObject = border.FindChild("HeroName");
                nameTextObject.gameObject.GetComponent<TextMesh>().text = Account.CurrentAccount.GetCurrentHero().Name;
                nameTextObject.gameObject.transform.localPosition = new Vector3(0.45f, 1.6f, 10f);
                nameTextObject.transform.localPosition = new Vector3(nameTextObject.transform.localPosition.x - nameTextObject.GetComponent<Renderer>().bounds.size.x / 2, nameTextObject.transform.localPosition.y);
            }
        }

        if (!classSmallPortraitObject)
        {
            StaticScripts.CreateGameObj("LevelTextGlowGameObject", "Effects/TextGlowBlack", new Vector3(0.1f, 0.6f),
                new Vector3(-0.23f, -2.8f, 3f), child: true, parentName: "MainMenuSceneObject");

            StaticScripts.CreateGameObj("NameTextGlowGameObject", "Effects/TextGlowBlack", new Vector3(0.6f, 0.7f),
               new Vector3(-1.2f, -2.75f, 3f), child: true, parentName: "MainMenuSceneObject");
        }
    }
}
