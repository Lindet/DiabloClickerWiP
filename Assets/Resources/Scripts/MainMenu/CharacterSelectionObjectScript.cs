using UnityEngine;
using System.Collections;
using Assets.Resources.Scripts;

public class CharacterSelectionObjectScript : MonoBehaviour
{
    private int lastSelectedHeroId = -1;
    private bool selectionConfirmed = false;
    
	// Use this for initialization
	void Start ()
    {
        Account.CurrentAccount.HeroDeletedEvent += UpdateAmountOfSlotsAndCreateHeroButton;
	    Account.CurrentAccount.CurrentHeroChangedEvent += UpdateCurrentHeroPortrait;

        var selectHeroButton = StaticScripts.CreateGameObj("SelectHeroButton", @"Buttons/BattleNetButton_RedUp_397x66", new Vector3(0.85f, 0.85f), new Vector3(-1.7f, -3.9f, 10f), true, 1, true, typeof(ButtonBaseMouseEvents), child: true, parentName: "CharacterSelectionSceneObject");
        selectHeroButton.GetComponent<ButtonBaseMouseEvents>().MethodOnClick += delegate
        {
            selectionConfirmed = true;
            GameObject.Find("MainLogic").GetComponent<Main>().CurrentGameState = GameState.MainMenu;
        };
        StaticScripts.CreateTextObj("SelectHeroButtonText", "Select Hero", new Vector3(0.02f, 0.02f), new Vector3(1.3f, 0.41f, 0f), FontType.DiabloFont, 90, new Color32(243, 170, 85, 255), TextAlignment.Center, true, "SelectHeroButton");

        var cancelButton = StaticScripts.CreateGameObj("CancelButton", @"Buttons/BattleNetButton_BlueUp_262x50", new Vector3(0.8f, 0.8f), new Vector3(-7.7f, -3.8f, 10f), true, 1, true, typeof(ButtonBaseMouseEvents), child: true, parentName: "CharacterSelectionSceneObject");
        cancelButton.GetComponent<ButtonBaseMouseEvents>().MethodOnClick += delegate { GameObject.Find("MainLogic").GetComponent<Main>().CurrentGameState = GameState.MainMenu; };
        StaticScripts.CreateTextObj("CancelButtonText", "Cancel", new Vector3(0.02f, 0.02f), new Vector3(1f, 0.32f, 0f), FontType.DiabloFont, 80, new Color32(243, 170, 85, 255), TextAlignment.Center, true, "CancelButton");

        var deleteHeroButton = StaticScripts.CreateGameObj("DeleteHeroButton", @"Buttons/BattleNetButton_BlueUp_262x50", new Vector3(0.8f, 0.8f), new Vector3(5.75f, -3.8f, 10f), true, 1, true, typeof(ButtonBaseMouseEvents), child: true, parentName: "CharacterSelectionSceneObject");
        deleteHeroButton.GetComponent<ButtonBaseMouseEvents>().MethodOnClick += delegate
        {
            if (Account.CurrentAccount.CurrentHeroId == -1) return;
            Account.CurrentAccount.DeleteHero(Account.CurrentAccount.GetCurrentHero());

            if (Account.CurrentAccount.CurrentHeroId == -1)
                GameObject.Find("MainLogic").GetComponent<Main>().CurrentGameState = GameState.CharacterCreation;
        };
        StaticScripts.CreateTextObj("DeleteHeroButtonText", "Delete", new Vector3(0.02f, 0.02f), new Vector3(1f, 0.32f, 0f), FontType.DiabloFont, 80, new Color32(243, 170, 85, 255), TextAlignment.Center, true, "DeleteHeroButton");

        StaticScripts.CreateGameObj("SelectHeroHeaderBorder", @"Borders/CharacterCreation/BattlenetHeroCreate_Header", new Vector3(1.15f, 1.15f), new Vector3(-8.1f, 3.5f, 10f), child: true, parentName: "CharacterSelectionSceneObject");
        StaticScripts.CreateTextObj("SelectHeroHeaderBorderText", "Select a Hero", new Vector3(0.02f, 0.02f), new Vector3(0.45f, 0.43f, 0f), FontType.DiabloFont, 100, new Color32(255, 246, 222, 255), TextAlignment.Center, true, "SelectHeroHeaderBorder");

	}

    void OnEnable()
    {
        lastSelectedHeroId = Account.CurrentAccount.CurrentHeroId;
        selectionConfirmed = false;
        UpdateAmountOfSlotsAndCreateHeroButton();
    }

    void OnDisable()
    {
        var selectHeroButton = gameObject.transform.FindChild("SelectHeroButton");
        if (selectHeroButton && selectHeroButton.GetComponent<ButtonBaseMouseEvents>()._State != ButtonState.Disabled)
            selectHeroButton.gameObject.GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Up;

        var cancelButton = gameObject.transform.FindChild("CancelButton");
        if (cancelButton && cancelButton.GetComponent<ButtonBaseMouseEvents>()._State != ButtonState.Disabled)
            cancelButton.gameObject.GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Up;

        var createHeroButton = gameObject.transform.FindChild("CreateHeroButton");
        if (createHeroButton && createHeroButton.GetComponent<ButtonBaseMouseEvents>()._State != ButtonState.Disabled)
            createHeroButton.gameObject.GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Up;

        if(!selectionConfirmed)
            Account.CurrentAccount.SetCurrentHero(lastSelectedHeroId);
    }

    void UpdateAmountOfSlotsAndCreateHeroButton()
    {
        var remainingSlotsText = gameObject.transform.FindChild("HeroSlotRemainingText");
        if (!remainingSlotsText)
            StaticScripts.CreateTextObj("HeroSlotRemainingText", string.Format("{0} Hero Slots Remaining", 12 - Account.CurrentAccount.ListOfHeroes.Count),
                new Vector3(0.02f, 0.02f), new Vector3(-7.4f, -3.15f, 0f), FontType.StandartFont, 65, new Color32(243, 170, 85, 255), TextAlignment.Center, true, "CharacterSelectionSceneObject");
        else
            remainingSlotsText.gameObject.GetComponent<TextMesh>().text = string.Format("{0} Hero Slots Remaining", 12 - Account.CurrentAccount.ListOfHeroes.Count);


        var createNewHeroButton = gameObject.transform.FindChild("CreateHeroButton");
        if (!createNewHeroButton)
        {
            StaticScripts.CreateGameObj("CreateHeroButton", @"Buttons/BattleNetButton_RedUp_397x66", new Vector3(0.85f, 0.85f), new Vector3(-8.2f, -3f, 10f), true, 1, true,
                typeof(ButtonBaseMouseEvents), child: true, parentName: "CharacterSelectionSceneObject");
            createNewHeroButton = gameObject.transform.FindChild("CreateHeroButton");
            createNewHeroButton.GetComponent<ButtonBaseMouseEvents>().MethodOnClick += delegate { GameObject.Find("MainLogic").GetComponent<Main>().CurrentGameState = GameState.CharacterCreation; };
            StaticScripts.CreateTextObj("CreateHeroButtonText", "Create", new Vector3(0.02f, 0.02f), new Vector3(1.6f, 0.4f, 0f), FontType.DiabloFont, 90, new Color32(243, 170, 85, 255), TextAlignment.Center, true, "CreateHeroButton");

        }
        createNewHeroButton.GetComponent<ButtonBaseMouseEvents>()._State = Account.CurrentAccount.ListOfHeroes.Count == 12 ? ButtonState.Disabled : ButtonState.Up;

        UpdateCurrentHeroPortrait();
    }

    void UpdateCurrentHeroPortrait()
    {
        var classPortraitObject = gameObject.transform.FindChild("ClassPortrait");
        if (!classPortraitObject)
            StaticScripts.CreateGameObj("ClassPortrait", string.Format("Portraits/Heroes/Portrait_{0}_{1}", Account.CurrentAccount.GetCurrentHero().Class,
                    Account.CurrentAccount.GetCurrentHero().Gender ? "Male" : "Female"), new Vector3(1.5f, 1.5f), new Vector3(-1.65f, -1.15f), child: true, parentName: "CharacterSelectionSceneObject");
        else
            classPortraitObject.GetComponent<SpriteRenderer>().sprite = StaticScripts.CreateSprite(string.Format("Portraits/Heroes/Portrait_{0}_{1}",
                    Account.CurrentAccount.GetCurrentHero().Class, Account.CurrentAccount.GetCurrentHero().Gender ? "Male" : "Female"));
    }
}
