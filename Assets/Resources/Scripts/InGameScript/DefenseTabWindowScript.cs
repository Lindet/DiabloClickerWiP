using UnityEngine;
using Assets.Resources.Scripts;

/*
 * Class for Defense Talents Tab 
 * Contains methods for adding/substracting defense talents, showing currently selected talents 
 */

public class DefenseTabWindowScript : MonoBehaviour {

    private GameObject unspentPointsTextObject;
    private GameObject totalPointsTextObject;
    private GameObject lifePercentPointsTextObject;
    private GameObject lifePercentPointsBonusTextObject;
    private GameObject armorPointsTextObject;
    private GameObject armorPointsBonusTextObject;
    private GameObject resistAllPointsObject;
    private GameObject resistAllPointsBonusObject;
    private GameObject lifeRegenerationPointsObject;
    private GameObject lifeRegenerationPointsBonusObject;

	void Start () 
    {

        var arrayOfNames = new[] { "Life %", "Armor", "Resist All", "Life Regeneration" };
        for (int i = 0; i < 4; i++)
        {
            StaticScripts.CreateGameObj(string.Format("SkillsPanel_{0}", i), "Borders/InGame/SkillsPane_Paragon_StatBase",
                new Vector3(0.9f, 0.9f), new Vector3(1.15f, 1.6f - (1f * i), 20f), child: true, parentName: name, sortingOrder: 3);

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
                    lifePercentPointsTextObject = spentPointsText;
                    lifePercentPointsBonusTextObject = spentPointsBonusText;

                    plusButtonObject.GetComponent<ButtonBaseMouseEvents>().MethodOnClick += delegate
                    {
                        Account.CurrentAccount.GetCurrentHero().AddTalentPoint(CurrentTab.Defense, 1);
                    };

                    minusButtonObject.GetComponent<ButtonBaseMouseEvents>().MethodOnClick += delegate
                    {
                        Account.CurrentAccount.GetCurrentHero().SubtractTalentPoints(CurrentTab.Defense, 1);
                    };

                    break;
                case 1:
                    armorPointsTextObject = spentPointsText;
                    armorPointsBonusTextObject = spentPointsBonusText;

                    plusButtonObject.GetComponent<ButtonBaseMouseEvents>().MethodOnClick += delegate
                    {
                        Account.CurrentAccount.GetCurrentHero().AddTalentPoint(CurrentTab.Defense, 2);
                    };

                    minusButtonObject.GetComponent<ButtonBaseMouseEvents>().MethodOnClick += delegate
                    {
                        Account.CurrentAccount.GetCurrentHero().SubtractTalentPoints(CurrentTab.Defense, 2);
                    };

                    break;
                case 2:
                    resistAllPointsObject = spentPointsText;
                    resistAllPointsBonusObject = spentPointsBonusText;

                    plusButtonObject.GetComponent<ButtonBaseMouseEvents>().MethodOnClick += delegate
                    {
                        Account.CurrentAccount.GetCurrentHero().AddTalentPoint(CurrentTab.Defense, 3);
                    };

                    minusButtonObject.GetComponent<ButtonBaseMouseEvents>().MethodOnClick += delegate
                    {
                        Account.CurrentAccount.GetCurrentHero().SubtractTalentPoints(CurrentTab.Defense, 3);
                    };

                    break;
                case 3:
                    lifeRegenerationPointsObject = spentPointsText;
                    lifeRegenerationPointsBonusObject = spentPointsBonusText;

                    plusButtonObject.GetComponent<ButtonBaseMouseEvents>().MethodOnClick += delegate
                    {
                        Account.CurrentAccount.GetCurrentHero().AddTalentPoint(CurrentTab.Defense, 4);
                    };

                    minusButtonObject.GetComponent<ButtonBaseMouseEvents>().MethodOnClick += delegate
                    {
                        Account.CurrentAccount.GetCurrentHero().SubtractTalentPoints(CurrentTab.Defense, 4);
                    };

                    break;
            }
        }


        unspentPointsTextObject = StaticScripts.CreateTextObj("UnspentPointsTextObject", string.Format("Defense Unspent Points: {0}", 0),
            new Vector3(0.02f, 0.02f), new Vector3(0f, 0f), FontType.StandartFont, 80, new Color32(173, 164, 135, 255),
            TextAlignment.Center, true, name, FontStyle.Bold);
        unspentPointsTextObject.transform.localPosition = new Vector3(4.6f - (unspentPointsTextObject.GetComponent<Renderer>().bounds.size.x / 2f), -1.5f);
        unspentPointsTextObject.transform.localScale = new Vector3(1f, 1.1f);

        totalPointsTextObject = StaticScripts.CreateTextObj("PointsTotalTextObject", string.Format("Defense Points Total: {0}", 0),
            new Vector3(0.02f, 0.02f), new Vector3(0f, 0f), FontType.StandartFont, 80, new Color32(173, 164, 135, 255),
            TextAlignment.Center, true, name, FontStyle.Bold);
        totalPointsTextObject.transform.localPosition = new Vector3(4.6f - (totalPointsTextObject.GetComponent<Renderer>().bounds.size.x / 2f), -1.8f);
        totalPointsTextObject.transform.localScale = new Vector3(1f, 1.1f);

        var resetButtonGameObject = StaticScripts.CreateGameObj("ResetButton", "Buttons/BattleNetButton_RedUp_262x50", new Vector3(0.8f, 0.9f),
            new Vector3(3.6f, -2.85f, 10f), true, 1, true, typeof(ButtonBaseMouseEvents), true, name);
        resetButtonGameObject.GetComponent<ButtonBaseMouseEvents>().MethodOnClick += delegate { Account.CurrentAccount.GetCurrentHero().ResetTalentPoints(CurrentTab.Defense); };
        StaticScripts.CreateTextObj("ResetButtonText", "Reset", new Vector3(0.02f, 0.02f), new Vector3(0.95f, 0.35f, 0f),
            FontType.DiabloFont, 100, new Color32(243, 170, 85, 255), TextAlignment.Center, true, "ResetButton");
	}
	
	// Update is called once per frame
    void FixedUpdate()
    {
        totalPointsTextObject.GetComponent<TextMesh>().text = string.Format("Defense Points Total: {0}",
            Account.CurrentAccount.GetCurrentHero().GetAmountOfSpentPoints(CurrentTab.Defense));
        unspentPointsTextObject.GetComponent<TextMesh>().text = string.Format("Defense Unspent Points: {0}",
            Account.CurrentAccount.GetCurrentHero().GetAmountOfUnspentCorePoints(CurrentTab.Defense));

        lifePercentPointsTextObject.GetComponent<TextMesh>().text =
            Account.CurrentAccount.GetCurrentHero().GetAmountOfTalentPoints(CurrentTab.Defense, 1).ToString();
        lifePercentPointsBonusTextObject.GetComponent<TextMesh>().text = string.Format("{0}%", 
            Account.CurrentAccount.GetCurrentHero().GetAmountOfTalentPoints(CurrentTab.Defense, 1));

        armorPointsTextObject.GetComponent<TextMesh>().text =
            Account.CurrentAccount.GetCurrentHero().GetAmountOfTalentPoints(CurrentTab.Defense, 2).ToString();
        armorPointsBonusTextObject.GetComponent<TextMesh>().text =
            (Account.CurrentAccount.GetCurrentHero().GetAmountOfTalentPoints(CurrentTab.Defense, 2) * 5).ToString();

        resistAllPointsObject.GetComponent<TextMesh>().text =
            Account.CurrentAccount.GetCurrentHero().GetAmountOfTalentPoints(CurrentTab.Defense, 3).ToString();
        resistAllPointsBonusObject.GetComponent<TextMesh>().text =
            (Account.CurrentAccount.GetCurrentHero().GetAmountOfTalentPoints(CurrentTab.Defense, 3) * 5).ToString();

        lifeRegenerationPointsObject.GetComponent<TextMesh>().text =
            Account.CurrentAccount.GetCurrentHero().GetAmountOfTalentPoints(CurrentTab.Defense, 4).ToString();
        lifeRegenerationPointsBonusObject.GetComponent<TextMesh>().text = string.Format(@"{0} /sec",
            Account.CurrentAccount.GetCurrentHero().GetAmountOfTalentPoints(CurrentTab.Defense, 4));
    }
}
