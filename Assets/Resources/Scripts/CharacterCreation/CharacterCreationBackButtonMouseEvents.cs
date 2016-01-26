using Assets.Resources.Scripts;
using UnityEngine;
using System.Collections;

public class CharacterCreationBackButtonMouseEvents : ButtonBaseMouseEvents {

    new void OnMouseDown()
    {
        base.OnMouseDown();

        var SceneObject = GameObject.Find("CharacterCreationSceneObject");
        DestroyImmediate(SceneObject);
        Account.CurrentAccount.DeleteHero(Account.CurrentAccount.GetCurrentHero());
        GameObject.Find("MainLogic").GetComponent<Main>().CurrentGameState = GameState.MainMenu;
    }

}
