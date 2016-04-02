using System;
using UnityEngine;
using System.Collections;


//This class used for selecting current gender of the hero which is being created. 
public class CharacterCreationGenderMouseEvents : MonoBehaviour
{
    private string gender = string.Empty;

    void Start()
    {
        gender = gameObject.name.Contains("Female") ? "Female" : "Male";
    }

    void OnMouseDown()
    {
        if (gameObject.GetComponent<SpriteRenderer>().sprite.texture.name == string.Format("BattleNetHeroCreate_Gender{0}Selected", gender))
            return;

        gameObject.GetComponent<SpriteRenderer>().sprite = StaticScripts.CreateSprite(String.Format(@"Icons/CharacterCreation/BattleNetHeroCreate_Gender{0}Down", gender));
    }

    private void OnMouseUpAsButton()
    {
        if (gameObject.GetComponent<SpriteRenderer>().sprite.texture.name ==
            string.Format("BattleNetHeroCreate_Gender{0}Selected", gender))
            return;

        gameObject.GetComponent<SpriteRenderer>().sprite = StaticScripts.CreateSprite(String.Format(@"Icons/CharacterCreation/BattleNetHeroCreate_Gender{0}Selected", gender));


        Account.CurrentAccount.GetCurrentHero().Gender = gender == "Male";
        GameObject.Find("ClassPortrait").GetComponent<SpriteRenderer>().sprite = StaticScripts.CreateSprite(string.Format("Portraits/Heroes/Portrait_{0}_{1}",
                Account.CurrentAccount.GetCurrentHero().Class,
                 Account.CurrentAccount.GetCurrentHero().Gender
                    ? "Male"
                    : "Female"));

        var obj = GameObject.Find(gender == "Female" ? gameObject.name.Replace("Female", "Male") : gameObject.name.Replace("Male", "Female"));
        if (obj == null) return;

        obj.GetComponent<SpriteRenderer>().sprite = StaticScripts.CreateSprite(String.Format(@"Icons/CharacterCreation/BattleNetHeroCreate_Gender{0}Up", gender == "Female" ? "Male" : "Female"));
}

    void OnMouseEnter()
    {
        if (gameObject.GetComponent<SpriteRenderer>().sprite.texture.name == string.Format("BattleNetHeroCreate_Gender{0}Selected", gender))
            return;

        gameObject.GetComponent<SpriteRenderer>().sprite =
            StaticScripts.CreateSprite(String.Format(@"Icons/CharacterCreation/BattleNetHeroCreate_Gender{0}Over",
                gender));
    }

    void OnMouseExit()
    {
        if (gameObject.GetComponent<SpriteRenderer>().sprite.texture.name == string.Format("BattleNetHeroCreate_Gender{0}Selected", gender))
            return;

        gameObject.GetComponent<SpriteRenderer>().sprite =
            StaticScripts.CreateSprite(String.Format(@"Icons/CharacterCreation/BattleNetHeroCreate_Gender{0}Up", gender));
    }

}
