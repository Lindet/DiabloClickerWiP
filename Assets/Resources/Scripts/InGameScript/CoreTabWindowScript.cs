using System;
using UnityEngine;
using System.Collections;
using Assets.Resources.Scripts;

public class CoreTabWindowScript : MonoBehaviour
{
    private GameObject unspentPointsTextObject;
    private GameObject totalPointsTextObject;
    private GameObject mainStatPointsTextObject;
    private GameObject mainStatPointsBonusTextObject;
    private GameObject vitalityPointsTextObject;
    private GameObject vitalityPointsBonusTextObject;
    private GameObject perkTextPointsObject1;
    private GameObject perkTextPointsBonusObject1;
    private GameObject perkTextPointsObject2;
    private GameObject perkTextPointsBonusObject2;

	// Use this for initialization
	void Start ()
	{
	    var firstStatText = "";
	    switch (Account.CurrentAccount.GetCurrentHero().Class)
	    {
	        case GameClasses.Barbarian:
	        case GameClasses.Crusader:
	            firstStatText = "Strength";
	            break;
	        case GameClasses.DemonHunter:
	        case GameClasses.Monk:
	            firstStatText = "Dexterity";
	            break;
	        case GameClasses.WitchDoctor:
	        case GameClasses.Wizard:
	            firstStatText = "Intelligence";
	            break;
	        default:
	            throw new ArgumentOutOfRangeException();
	    }
	    var arrayOfNames = new[] {firstStatText, "Vitality", "SomePerk-1", "SomePerk-2"};
	    for (int i = 0; i < 4; i++)
	    {
	        StaticScripts.CreateGameObj(string.Format("SkillsPanel_{0}", i), "Borders/InGame/SkillsPane_Paragon_StatBase", new Vector3(0.9f, 0.9f), new Vector3(1.15f, 1.6f - (1f*i), 20f), child: true, parentName: name, sortingOrder: 3);

	        StaticScripts.CreateTextObj(string.Format("SkillsPanelText_{0}", i), arrayOfNames[i], new Vector3(0.02f, 0.02f), new Vector3(1.2f, 0.64f), FontType.StandartFont, 100, new Color32(243, 170, 85, 255), TextAlignment.Center, true, string.Format("SkillsPanel_{0}", i));

	        StaticScripts.CreateGameObj(string.Format("SkillsPanel_{0}_Minus", i), "Buttons/D3_ListBtn_Collapse_Up", new Vector3(1f, 1f), new Vector3(6.3f, 0.37f), true, 1, true, typeof (ButtonBaseMouseEvents), true, string.Format("SkillsPanel_{0}", i));

	        StaticScripts.CreateGameObj(string.Format("SkillsPanel_{0}_Plus", i), "Buttons/D3_ListBtn_Expand_Up", new Vector3(1f, 1f), new Vector3(7.2f, 0.37f), true, 1, true, typeof (ButtonBaseMouseEvents), true, string.Format("SkillsPanel_{0}", i));

	        var spentPointsBonusText = StaticScripts.CreateTextObj(string.Format("SkillsPanel_{0}_SpentPointsBonus", i), "0", new Vector3(0.02f, 0.02f), new Vector3(3.8f, 0.65f), FontType.StandartFont, 130, Color.white, TextAlignment.Center, true, string.Format("SkillsPanel_{0}", i), FontStyle.Bold);

	        var spentPointsText = StaticScripts.CreateTextObj(string.Format("SkillsPanel_{0}_SpentPoints", i), "0", new Vector3(0.02f, 0.02f), new Vector3(0f, 0f), FontType.StandartFont, 120, Color.white, TextAlignment.Center, true, string.Format("SkillsPanel_{0}", i), FontStyle.Bold);
	        spentPointsText.transform.localPosition = new Vector3(6.85f - (spentPointsText.GetComponent<Renderer>().bounds.size.x/4f), 0.64f);

	        switch (i)
	        {
                case 0:
	                mainStatPointsTextObject = spentPointsText;
	                mainStatPointsBonusTextObject = spentPointsBonusText;
                    break;
                case 1:
	                vitalityPointsTextObject = spentPointsText;
	                vitalityPointsBonusTextObject = spentPointsBonusText;
                    break;
                case 2:
	                perkTextPointsObject1 = spentPointsText;
	                perkTextPointsBonusObject1 = spentPointsBonusText;
                    break;
                case 3:
	                perkTextPointsObject2 = spentPointsText;
	                perkTextPointsBonusObject2 = spentPointsBonusText;
                    break;
	        }
	    }


	    unspentPointsTextObject = StaticScripts.CreateTextObj("UnspentPointsTextObject", string.Format("Core Unspent Points: {0}", 0), new Vector3(0.02f, 0.02f), new Vector3(0f, 0f), FontType.StandartFont, 80, new Color32(173, 164, 135, 255), TextAlignment.Center, true, name, FontStyle.Bold);
	    unspentPointsTextObject.transform.localPosition = new Vector3(4.6f - (unspentPointsTextObject.GetComponent<Renderer>().bounds.size.x/2f), -1.5f);
	    unspentPointsTextObject.transform.localScale = new Vector3(1f, 1.1f);

	    totalPointsTextObject = StaticScripts.CreateTextObj("CorePointsTotalTextObject", string.Format("Core Points Total: {0}", 0), new Vector3(0.02f, 0.02f), new Vector3(0f, 0f), FontType.StandartFont, 80, new Color32(173, 164, 135, 255), TextAlignment.Center, true, name, FontStyle.Bold);
	    totalPointsTextObject.transform.localPosition = new Vector3(4.6f - (totalPointsTextObject.GetComponent<Renderer>().bounds.size.x/2f), -1.8f);
	    totalPointsTextObject.transform.localScale = new Vector3(1f, 1.1f);

	    StaticScripts.CreateGameObj("AcceptButton", "Buttons/BattleNetButton_RedUp_262x50", new Vector3(0.8f, 0.9f), new Vector3(1.8f, -2.85f, 10f), true, 1, true, typeof (ButtonBaseMouseEvents), true, name);
	    StaticScripts.CreateTextObj("AcceptButtonText", "Accept", new Vector3(0.02f, 0.02f), new Vector3(0.9f, 0.35f, 0f), FontType.DiabloFont, 100, new Color32(243, 170, 85, 255), TextAlignment.Center, true, "AcceptButton");

	    StaticScripts.CreateGameObj("ResetButton", "Buttons/BattleNetButton_RedUp_262x50", new Vector3(0.8f, 0.9f), new Vector3(5.4f, -2.85f, 10f), true, 1, true, typeof (ButtonBaseMouseEvents), true, name);
	    StaticScripts.CreateTextObj("ResetButtonText", "Reset", new Vector3(0.02f, 0.02f), new Vector3(0.95f, 0.35f, 0f), FontType.DiabloFont, 100, new Color32(243, 170, 85, 255), TextAlignment.Center, true, "ResetButton");
	}


    void FixedUpdate()
    {
        totalPointsTextObject.GetComponent<TextMesh>().text = string.Format("Core Points Total: {0}",
            Account.CurrentAccount.GetCurrentHero().GetAmountOfSpentPoints(CurrentTab.Core));
        unspentPointsTextObject.GetComponent<TextMesh>().text = string.Format("Core Unspent Points: {0}",
            Account.CurrentAccount.GetCurrentHero().GetAmountOfUnspentCorePoints(CurrentTab.Core));

        mainStatPointsTextObject.GetComponent<TextMesh>().text =
            Account.CurrentAccount.GetCurrentHero().GetAmountOfTalentPoints(CurrentTab.Core, 1).ToString();
        mainStatPointsBonusTextObject.GetComponent<TextMesh>().text =
            (Account.CurrentAccount.GetCurrentHero().GetAmountOfTalentPoints(CurrentTab.Core, 1)*5).ToString();

        vitalityPointsTextObject.GetComponent<TextMesh>().text =
            Account.CurrentAccount.GetCurrentHero().GetAmountOfTalentPoints(CurrentTab.Core, 2).ToString();
        vitalityPointsBonusTextObject.GetComponent<TextMesh>().text =
            (Account.CurrentAccount.GetCurrentHero().GetAmountOfTalentPoints(CurrentTab.Core, 2)*5).ToString();

        perkTextPointsObject1.GetComponent<TextMesh>().text =
            Account.CurrentAccount.GetCurrentHero().GetAmountOfTalentPoints(CurrentTab.Core, 3).ToString();
        perkTextPointsBonusObject1.GetComponent<TextMesh>().text =
            (Account.CurrentAccount.GetCurrentHero().GetAmountOfTalentPoints(CurrentTab.Core, 3) * 5).ToString();

        perkTextPointsObject2.GetComponent<TextMesh>().text =
            Account.CurrentAccount.GetCurrentHero().GetAmountOfTalentPoints(CurrentTab.Core, 4).ToString();
        perkTextPointsBonusObject2.GetComponent<TextMesh>().text =
            (Account.CurrentAccount.GetCurrentHero().GetAmountOfTalentPoints(CurrentTab.Core, 4) * 5).ToString();
    }
}
