using UnityEngine;
using System.Collections;

public class CharacterCreationHeroicCheckBoxScript : CheckBoxMouseEvents
{

    new void OnMouseUpAsButton()
    {
        base.OnMouseUpAsButton();

        Account.CurrentAccount.GetCurrentHero().Heroic = Checked;
    }
}
