using UnityEngine;
using System.Collections;
using System.Linq;
using Assets.Resources.Scripts;

public class StartGameScript : MonoBehaviour {

    void OnMouseUpAsButton()
    {
        GameObject.Find("SceneObjects").SetActive(false);
        //GameObject.Find("MenuGameObject").SetActive(true);
        //var mainObj = GameObject.Find("MainGameObject");
        //if (!mainObj) return;
        //foreach (Transform child in GameObject.Find("MainGameObject").transform)
        //{

        //}
        var menuObj = GameObject.Find("MenuGameObject").transform.Cast<Transform>().FirstOrDefault(child => child.name == "MainLogic").gameObject;
        menuObj.SetActive(true);
        menuObj.GetComponent<Main>().CurrentGameState = GameState.InGame;
    }
}
