using System;
using UnityEngine;
using System.Collections;
using Assets.Resources.Scripts;
using Random = UnityEngine.Random;


// Class which is combining Character Creation Screen.
public class CreateCharacterObjectScript : MonoBehaviour {

    GameClasses selectedClass = GameClasses.Wizard;
	// Use this for initialization
	void Start ()
    {
        StaticScripts.CreateGameObj("CreateHeroHeaderBorder", @"Borders/CharacterCreation/BattlenetHeroCreate_Header", new Vector3(1.1f, 1.1f), new Vector3(-8f, 2.625f, 10f), child: true, parentName: "CharacterCreationSceneObject");
        var backButton = StaticScripts.CreateGameObj("BackButton", @"Buttons/BattleNetButton_ClearUp_260x50", new Vector3(0.8f, 0.6f), new Vector3(-7.67f, -4.33f, 10f), true, 1, true, typeof(ButtonBaseMouseEvents), child: true, parentName: "CharacterCreationSceneObject");
        backButton.GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Up;
        backButton.GetComponent<ButtonBaseMouseEvents>().MethodOnClick += delegate
        {
            Account.CurrentAccount.DeleteHero(Account.CurrentAccount.GetCurrentHero());
            if (Account.CurrentAccount.CurrentHeroId == -1)
            {
                GameObject.Find("MainLogic").GetComponent<Main>().CurrentGameState = GameState.Login;
                return;
            }
            GameObject.Find("MainLogic").GetComponent<Main>().CurrentGameState = GameState.MainMenu;
        };
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
        var createHeroButton = StaticScripts.CreateGameObj("CreateHeroButton", @"Buttons/BattleNetButton_RedDisabled_398x82", new Vector3(0.8f, 0.6f), new Vector3(-1.585f, -4.35f, 10f), true, 1, true, typeof(CharacterCreationCreateHeroButtonEvents), child: true, parentName: "CharacterCreationSceneObject");
        createHeroButton.GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Disabled;
        StaticScripts.CreateTextObj("CreateHeroButtonText", "Create Hero", new Vector3(0.02f, 0.02f), new Vector3(1.18f, 0.5f, 0f), FontType.DiabloFont, 110, new Color32(79, 75, 70, 255), TextAlignment.Center, true, "CreateHeroButton");
        StaticScripts.CreateTextObj("EnterHeroNameText", "Enter Hero Name:", new Vector3(0.02f, 0.02f), new Vector3(-0.9f, -2.9f, 10f), FontType.StandartFont, 110, new Color32(255, 225, 173, 255), TextAlignment.Center, true, "CharacterCreationSceneObject");

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

    void OnEnable()
    {
        LoadHeroesButtons();
    }

    void OnDisable()
    {
        var backButton = gameObject.transform.FindChild("BackButton");
        if (backButton && backButton.GetComponent<ButtonBaseMouseEvents>()._State != ButtonState.Disabled)
            backButton.gameObject.GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Up;

        var createHeroButton = gameObject.transform.FindChild("CreateHeroButton");
        if (createHeroButton && createHeroButton.GetComponent<ButtonBaseMouseEvents>()._State != ButtonState.Disabled)
            createHeroButton.gameObject.GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Up;
    }

    void LoadHeroesButtons()
    {

        selectedClass = (GameClasses)Enum.GetValues(typeof(GameClasses)).GetValue(Random.Range(0, 6));

        Account.CurrentAccount.AddNewHero(new Hero() { Class = selectedClass, Gender = true, State = HeroState.NotCreated });
        Account.CurrentAccount.SetCurrentHero(Account.CurrentAccount.ListOfHeroes.Count - 1);
        #region ClassButtons

        var classesArray = new[] { "Barbarian", "Crusader", "Demon Hunter", "Monk", "Witch Doctor", "Wizard" };
        for (int i = 0; i < classesArray.Length; i++)
        {
            if(!gameObject.transform.FindChild(string.Format("{0}Button", classesArray[i])))
            { 
                var button = StaticScripts.CreateGameObj(string.Format("{0}Button", classesArray[i]), @"Buttons/BattleNetButton_ClearUp_397x66", new Vector3(0.65f, 0.85f), new Vector3(-8.3f, 1.9f - 0.59f*i, 10f), true, 1, true, typeof (CreateCharacterClassMouseEvents), child: true, parentName: "CharacterCreationSceneObject");
                button.GetComponent<ButtonBaseMouseEvents>()._State = selectedClass.ToString() == classesArray[i].Replace(" ", string.Empty) ? ButtonState.Selected : ButtonState.Up;
                
                var text = StaticScripts.CreateTextObj(string.Format("{0}ButtonText", classesArray[i]), classesArray[i], new Vector3(0.02f, 0.02f), new Vector3(0f, 0f, 0f), FontType.DiabloFont, 100, new Color32(243, 170, 85, 255), TextAlignment.Center, child: true, parentName: string.Format("{0}Button", classesArray[i]), style:FontStyle.Bold);
                text.transform.position = new Vector3((button.transform.position.x + (button.GetComponent<SpriteRenderer>().bounds.size.x/2f)) - (text.GetComponent<TextMesh>().GetComponent<Renderer>().bounds.size.x/2f), (button.transform.position.y + (button.GetComponent<SpriteRenderer>().bounds.size.y/2f)) + (text.GetComponent<TextMesh>().GetComponent<Renderer>().bounds.size.y/2f));

                if (classesArray[i].Replace(" ", string.Empty) == selectedClass.ToString())
                {
                    StaticScripts.CreateGameObj("ClassPortrait", string.Format("Portraits/Heroes/Portrait_{0}_{1}", classesArray[i].Replace(" ", ""), Account.CurrentAccount.GetCurrentHero().Gender ? "Male" : "Female"), new Vector3(1.5f, 1.5f), new Vector3(-1.65f, -1.15f), child: true, parentName: "CharacterCreationSceneObject");
                }
            }
            else
            {
                var button = GameObject.Find(string.Format("{0}Button", classesArray[i]));
                button.GetComponent<ButtonBaseMouseEvents>()._State = selectedClass.ToString() == classesArray[i].Replace(" ", string.Empty) ? ButtonState.Selected : ButtonState.Up;

                if (classesArray[i].Replace(" ", string.Empty) == selectedClass.ToString())
                {
                    var portraitObject = gameObject.transform.FindChild("ClassPortrait");
                    if(portraitObject)
                        DestroyImmediate(portraitObject.gameObject);
                    StaticScripts.CreateGameObj("ClassPortrait", string.Format("Portraits/Heroes/Portrait_{0}_{1}", classesArray[i].Replace(" ", ""), Account.CurrentAccount.GetCurrentHero().Gender ? "Male" : "Female"), new Vector3(1.5f, 1.5f), new Vector3(-1.65f, -1.15f), child: true, parentName: "CharacterCreationSceneObject");
                }
            }
        }

       #endregion
    }
}
