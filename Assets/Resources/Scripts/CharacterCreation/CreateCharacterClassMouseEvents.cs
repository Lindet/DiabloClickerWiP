using System.Linq;
using Assets.Resources.Scripts;
using UnityEngine;
using System.Collections;


/*
 * Functionality for changing gender of the hero at the CharacterCreation Screen.
 * - Helps to re-color selected button(especially text of that button)
 * - Deselects previously selected button
 * - Loads another character details texture
 */
public class CreateCharacterClassMouseEvents : ButtonBaseMouseEvents {

    string[] classesArray = { "Barbarian", "Crusader", "Demon Hunter", "Monk", "Witch Doctor", "Wizard" };

    new void OnMouseDown()
    {
        if (_State == ButtonState.Disabled || _State == ButtonState.Selected) return;
        _State = ButtonState.Down;
    }

    new void OnMouseUpAsButton()
    {
        if (_State == ButtonState.Disabled || _State == ButtonState.Selected) return;
        _State = ButtonState.Selected;

        var buttonText = GameObject.Find(gameObject.name + "Text");
        if (buttonText == null) return;
        buttonText.GetComponent<Renderer>().material.color = new Color32(255, 255, 255, 255);

        var classDetails = GameObject.Find("ClassDetails");
        var classDetailTex = new Texture2D(0, 0);
        


        switch (gameObject.name.Replace("Button", string.Empty).Replace(" ", string.Empty))
        {
            case "Barbarian":
                classDetailTex = Resources.Load<Texture2D>(@"Borders/CharacterCreation/BattleNetHeroCreate_DetailsBarbarian");
                Account.CurrentAccount.GetCurrentHero().Class =
                        GameClasses.Barbarian;
                break;
            case "Crusader":
                classDetailTex =
                    Resources.Load<Texture2D>(@"Borders/CharacterCreation/BattleNetHeroCreate_DetailsCrusader");
                Account.CurrentAccount.GetCurrentHero().Class =
                        GameClasses.Crusader;
                break;
            case "DemonHunter":
                classDetailTex =
                    Resources.Load<Texture2D>(@"Borders/CharacterCreation/BattleNetHeroCreate_DetailsDemonHunter");
                Account.CurrentAccount.GetCurrentHero().Class =
                        GameClasses.DemonHunter;
                break;
            case "Monk":
                classDetailTex =
                    Resources.Load<Texture2D>(@"Borders/CharacterCreation/BattleNetHeroCreate_DetailsMonk");
                Account.CurrentAccount.GetCurrentHero().Class =
                        GameClasses.Monk;
                break;
            case "WitchDoctor":
                classDetailTex =
                    Resources.Load<Texture2D>(@"Borders/CharacterCreation/BattleNetHeroCreate_DetailsWitchDoctor");
                Account.CurrentAccount.GetCurrentHero().Class =
                        GameClasses.WitchDoctor;
                break;
            case "Wizard":
                classDetailTex =
                    Resources.Load<Texture2D>(@"Borders/CharacterCreation/BattleNetHeroCreate_DetailsWizard");
                Account.CurrentAccount.GetCurrentHero().Class =
                        GameClasses.Wizard;
                break;
        }
        classDetails.GetComponent<SpriteRenderer>().sprite = Sprite.Create(classDetailTex,
            new Rect(0, 0, classDetailTex.width, classDetailTex.height), new Vector2(0, 0));

        GameObject.Find("ClassPortrait").GetComponent<SpriteRenderer>().sprite = StaticScripts.CreateSprite(string.Format("Portraits/Heroes/Portrait_{0}_{1}",
                Account.CurrentAccount.GetCurrentHero().Class,
                 Account.CurrentAccount.GetCurrentHero().Gender
                    ? "Male"
                    : "Female"));
                    

        foreach (var s in classesArray)
        {
            var obj = GameObject.Find(string.Format("{0}Button", s));
            if (obj == null || obj == gameObject) continue;
            if (obj.GetComponent<SpriteRenderer>().sprite.texture.name != "BattleNetButton_ClearSelected_397x66")
                continue;
            obj.GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Up;

            var buttonUpText = GameObject.Find(obj.name + "Text");
            if (buttonUpText == null) return;
            buttonUpText.GetComponent<Renderer>().material.color = new Color32(243, 170, 85, 255);
        }
    }

    new void OnMouseEnter()
    {
        if (_State == ButtonState.Disabled || _State == ButtonState.Selected) return;
        _State = ButtonState.Over;
        var buttonText = GameObject.Find(gameObject.name + "Text");
        if (buttonText == null) return;
        buttonText.GetComponent<Renderer>().material.color = new Color32(255, 255, 255, 255);
    }

    new void OnMouseExit()
    {
        if (_State == ButtonState.Disabled || _State == ButtonState.Selected) return;
        _State = ButtonState.Up;
        var buttonText = GameObject.Find(gameObject.name + "Text");
        if (buttonText == null) return;
        buttonText.GetComponent<Renderer>().material.color = new Color32(243, 170, 85, 255);
    }
}
