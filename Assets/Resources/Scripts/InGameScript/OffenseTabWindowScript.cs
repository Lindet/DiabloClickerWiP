using UnityEngine;
using System.Collections;
using Assets.Resources.Scripts;

/*
 * Class for Offense Talents Tab 
 * Contains methods for adding/substracting offense talents, showing currently selected talents 
 */

public class OffenseTabWindowScript : MonoBehaviour {

    private GameObject unspentPointsTextObject;
    private GameObject totalPointsTextObject;
    private GameObject bonusDamagePointsTextObject;
    private GameObject bonusDamagePointsBonusTextObject;
    private GameObject thornsPointsTextObject;
    private GameObject thornsPointsBonusTextObject;
    private GameObject critChancePointsObject;
    private GameObject critChancePointsBonusObject;
    private GameObject critDamagePointsObject;
    private GameObject critDamagePointsBonusObject;
	void Start () 
    {

        var arrayOfNames = new[] { "Bonus Damage to Elites", "Thorns", "Critical Hit Chance", "Critical Hit Damage" };
        for (int i = 0; i < 4; i++)
        {
            StaticScripts.CreateGameObj(string.Format("SkillsPanel_{0}", i), "Borders/InGame/SkillsPane_Paragon_StatBase",
                new Vector3(0.9f, 0.9f), new Vector3(1.15f, 1.6f - (1f * i), 20f), child: true, parentName: name);

            StaticScripts.CreateTextObj(string.Format("SkillsPanelText_{0}", i), arrayOfNames[i],
                new Vector3(0.02f, 0.02f), new Vector3(1.2f, 0.64f), FontType.StandartFont, 100,
                new Color32(243, 170, 85, 255), TextAlignment.Center, true, string.Format("SkillsPanel_{0}", i));

             var minusButtonObject = StaticScripts.CreateGameObj(string.Format("SkillsPanel_{0}_Minus", i), "Buttons/D3_ListBtn_Collapse_Up",
                new Vector3(1f, 1f), new Vector3(6.3f, 0.37f), true, 1, true, typeof(ButtonBaseMouseEvents), true,
                string.Format("SkillsPanel_{0}", i));

            var plusButtonObject = StaticScripts.CreateGameObj(string.Format("SkillsPanel_{0}_Plus", i), "Buttons/D3_ListBtn_Expand_Up",
                new Vector3(1f, 1f), new Vector3(7.2f, 0.37f), true, 1, true, typeof(ButtonBaseMouseEvents), true,
                string.Format("SkillsPanel_{0}", i));

            var spentPointsBonusText = StaticScripts.CreateTextObj(string.Format("SkillsPanel_{0}_SpentPointsBonus", i), "0",
                new Vector3(0.02f, 0.02f), new Vector3(3.8f, 0.65f), FontType.StandartFont, 130, Color.white,
                TextAlignment.Center, true, string.Format("SkillsPanel_{0}", i), FontStyle.Bold);

            var spentPointsText = StaticScripts.CreateTextObj(string.Format("SkillsPanel_{0}_SpentPoints", i), "0",
                new Vector3(0.02f, 0.02f), new Vector3(0f, 0f), FontType.StandartFont, 120, Color.white,
                TextAlignment.Center, true, string.Format("SkillsPanel_{0}", i), FontStyle.Bold);
            spentPointsText.transform.localPosition = new Vector3(6.85f - (spentPointsText.GetComponent<Renderer>().bounds.size.x / 4f), 0.64f);

            switch (i)
            {
                case 0:
                    bonusDamagePointsTextObject = spentPointsText;
                    bonusDamagePointsBonusTextObject = spentPointsBonusText;

                    plusButtonObject.GetComponent<ButtonBaseMouseEvents>().MethodOnClick += delegate
                    {
                        Account.CurrentAccount.GetCurrentHero().AddTalentPoint(CurrentTab.Offense, 1);
                    };

                    minusButtonObject.GetComponent<ButtonBaseMouseEvents>().MethodOnClick += delegate
                    {
                        Account.CurrentAccount.GetCurrentHero().SubtractTalentPoints(CurrentTab.Offense, 1);
                    };

                    break;
                case 1:
                    thornsPointsTextObject = spentPointsText;
                    thornsPointsBonusTextObject = spentPointsBonusText;

                    plusButtonObject.GetComponent<ButtonBaseMouseEvents>().MethodOnClick += delegate
                    {
                        Account.CurrentAccount.GetCurrentHero().AddTalentPoint(CurrentTab.Offense, 2);
                    };

                    minusButtonObject.GetComponent<ButtonBaseMouseEvents>().MethodOnClick += delegate
                    {
                        Account.CurrentAccount.GetCurrentHero().SubtractTalentPoints(CurrentTab.Offense, 2);
                    };

                    break;
                case 2:
                    critChancePointsObject = spentPointsText;
                    critChancePointsBonusObject = spentPointsBonusText;

                    plusButtonObject.GetComponent<ButtonBaseMouseEvents>().MethodOnClick += delegate
                    {
                        Account.CurrentAccount.GetCurrentHero().AddTalentPoint(CurrentTab.Offense, 3);
                    };

                    minusButtonObject.GetComponent<ButtonBaseMouseEvents>().MethodOnClick += delegate
                    {
                        Account.CurrentAccount.GetCurrentHero().SubtractTalentPoints(CurrentTab.Offense, 3);
                    };

                    break;
                case 3:
                    critDamagePointsObject = spentPointsText;
                    critDamagePointsBonusObject = spentPointsBonusText;

                    plusButtonObject.GetComponent<ButtonBaseMouseEvents>().MethodOnClick += delegate
                    {
                        Account.CurrentAccount.GetCurrentHero().AddTalentPoint(CurrentTab.Offense, 4);
                    };

                    minusButtonObject.GetComponent<ButtonBaseMouseEvents>().MethodOnClick += delegate
                    {
                        Account.CurrentAccount.GetCurrentHero().SubtractTalentPoints(CurrentTab.Offense, 4);
                    };

                    break;
            }

        }


        unspentPointsTextObject = StaticScripts.CreateTextObj("UnspentPointsTextObject", string.Format("Offense Unspent Points: {0}", 0),
            new Vector3(0.02f, 0.02f), new Vector3(0f, 0f), FontType.StandartFont, 80, new Color32(173, 164, 135, 255),
            TextAlignment.Center, true, name, FontStyle.Bold);
        unspentPointsTextObject.transform.localPosition = new Vector3(4.6f - (unspentPointsTextObject.GetComponent<Renderer>().bounds.size.x / 2f), -1.5f);
        unspentPointsTextObject.transform.localScale = new Vector3(1f, 1.1f);

        totalPointsTextObject = StaticScripts.CreateTextObj("PointsTotalTextObject", string.Format("Offense Points Total: {0}", 0),
            new Vector3(0.02f, 0.02f), new Vector3(0f, 0f), FontType.StandartFont, 80, new Color32(173, 164, 135, 255),
            TextAlignment.Center, true, name, FontStyle.Bold);
        totalPointsTextObject.transform.localPosition = new Vector3(4.6f - (totalPointsTextObject.GetComponent<Renderer>().bounds.size.x / 2f), -1.8f);
        totalPointsTextObject.transform.localScale = new Vector3(1f, 1.1f);

        var resetButtonGameObject = StaticScripts.CreateGameObj("ResetButton", "Buttons/BattleNetButton_RedUp_262x50", new Vector3(0.8f, 0.9f),
            new Vector3(3.6f, -2.85f, 10f), true, 1, true, typeof(ButtonBaseMouseEvents), true, name);
        resetButtonGameObject.GetComponent<ButtonBaseMouseEvents>().MethodOnClick += delegate { Account.CurrentAccount.GetCurrentHero().ResetTalentPoints(CurrentTab.Offense); };
        StaticScripts.CreateTextObj("ResetButtonText", "Reset", new Vector3(0.02f, 0.02f), new Vector3(0.95f, 0.35f, 0f),
            FontType.DiabloFont, 100, new Color32(243, 170, 85, 255), TextAlignment.Center, true, "ResetButton");
	}
	
	// Update is called once per frame
    void FixedUpdate()
    {
        totalPointsTextObject.GetComponent<TextMesh>().text = string.Format("Offense Points Total: {0}",
            Account.CurrentAccount.GetCurrentHero().GetAmountOfSpentPoints(CurrentTab.Offense));
        unspentPointsTextObject.GetComponent<TextMesh>().text = string.Format("Offense Unspent Points: {0}",
            Account.CurrentAccount.GetCurrentHero().GetAmountOfUnspentCorePoints(CurrentTab.Offense));

        bonusDamagePointsTextObject.GetComponent<TextMesh>().text =
            Account.CurrentAccount.GetCurrentHero().GetAmountOfTalentPoints(CurrentTab.Offense, 1).ToString();
        bonusDamagePointsBonusTextObject.GetComponent<TextMesh>().text = string.Format("{0}%",
            (Account.CurrentAccount.GetCurrentHero().GetAmountOfTalentPoints(CurrentTab.Offense, 1) * 0.5));

        thornsPointsTextObject.GetComponent<TextMesh>().text =
            Account.CurrentAccount.GetCurrentHero().GetAmountOfTalentPoints(CurrentTab.Offense, 2).ToString();
        thornsPointsBonusTextObject.GetComponent<TextMesh>().text =
            Account.CurrentAccount.GetCurrentHero().GetAmountOfTalentPoints(CurrentTab.Offense, 2).ToString();

        critChancePointsObject.GetComponent<TextMesh>().text =
            Account.CurrentAccount.GetCurrentHero().GetAmountOfTalentPoints(CurrentTab.Offense, 3).ToString();
        critChancePointsBonusObject.GetComponent<TextMesh>().text = string.Format("{0}%",
            (Account.CurrentAccount.GetCurrentHero().GetAmountOfTalentPoints(CurrentTab.Offense, 3) * 0.1));

        critDamagePointsObject.GetComponent<TextMesh>().text =
            Account.CurrentAccount.GetCurrentHero().GetAmountOfTalentPoints(CurrentTab.Offense, 4).ToString();
        critDamagePointsBonusObject.GetComponent<TextMesh>().text = string.Format("{0}%",
            Account.CurrentAccount.GetCurrentHero().GetAmountOfTalentPoints(CurrentTab.Offense, 4));
    }
}
