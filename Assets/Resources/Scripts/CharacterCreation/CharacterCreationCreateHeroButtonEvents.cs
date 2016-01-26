using Assets.Resources.Scripts;
using UnityEngine;
using System.Collections;

public class CharacterCreationCreateHeroButtonEvents : ButtonBaseMouseEvents {

    new void OnMouseDown()
    {
        base.OnMouseDown();
        var hero = Account.CurrentAccount.GetCurrentHero();
        hero.State = HeroState.Normal;
        var SceneObject = GameObject.Find("CharacterCreationSceneObject");
        GameObject.Find("MainLogic").GetComponent<Main>().CurrentGameState = GameState.MainMenu;
        DestroyImmediate(SceneObject);
    }
}
