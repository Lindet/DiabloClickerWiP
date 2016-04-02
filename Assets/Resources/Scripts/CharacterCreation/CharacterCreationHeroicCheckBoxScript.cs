using UnityEngine;
using System.Collections;


//Class for heroic checkBox at the CharacterCreatingScreen
public class CharacterCreationHeroicCheckBoxScript : CheckBoxMouseEvents
{

    new void OnMouseUpAsButton()
    {
        base.OnMouseUpAsButton();

        Account.CurrentAccount.GetCurrentHero().Heroic = Checked;
    }
}
