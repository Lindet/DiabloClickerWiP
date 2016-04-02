using System;
using UnityEngine;
using System.Collections;

//This is the code for the button which scrolls HeroList 
public class ButtonScrollDown : MonoBehaviour {

    private const float scrollBarStep = 0.0956f;
    private const float heroPlateStep = 0.08f;
    private GameObject scrollBar;
    private GameObject lastHeroSlot;
    private GameObject heroSlotsList;

    void OnMouseDown()
    {
        InvokeRepeating("ScrollDown", 0f, 0.05f);

        var backButtonTex = Resources.Load<Texture2D>(@"Controls/ScrollSmallDown_Down");
        gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(backButtonTex,
            new Rect(0, 0, backButtonTex.width, backButtonTex.height), new Vector2(0, 0));
    }

    void OnMouseUp()
    {
        CancelInvoke("ScrollDown");

        var backButtonTex = Resources.Load<Texture2D>(@"Controls/ScrollSmallDown_Up");
        gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(backButtonTex,
            new Rect(0, 0, backButtonTex.width, backButtonTex.height), new Vector2(0, 0));
    }

    void OnMouseEnter()
    {
        var backButtonTex = Resources.Load<Texture2D>(@"Controls/ScrollSmallDown_Over");
        gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(backButtonTex,
            new Rect(0, 0, backButtonTex.width, backButtonTex.height), new Vector2(0, 0));
    }

    void OnMouseExit()
    {
        var backButtonTex = Resources.Load<Texture2D>(@"Controls/ScrollSmallDown_Up");
        gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(backButtonTex,
            new Rect(0, 0, backButtonTex.width, backButtonTex.height), new Vector2(0, 0));
    }

    void ScrollDown()
    {
        if (scrollBar == null) scrollBar = GameObject.Find("Scrollbar");
        if (scrollBar.transform.position.y <= -2.01f) return;

        if (lastHeroSlot == null) lastHeroSlot = GameObject.Find("HeroSlot_11");
        if (lastHeroSlot.transform.position.y == -2.3f) return; //check if it's the maximum allowed position
        if (heroSlotsList == null) heroSlotsList = GameObject.Find("HeroListGameObject");
        if (lastHeroSlot.transform.position.y + heroPlateStep > -2.3f) // Do this if there is some more length but not enough for the full step
        {
            var diff = -2.3f - lastHeroSlot.transform.position.y;
            foreach (Transform child in heroSlotsList.transform)
            {
                if (child.name.Contains("HeroSlot"))
                {
                    child.position = new Vector3(child.position.x, child.position.y + diff, child.position.z);
                }
            }
            scrollBar.transform.position = new Vector3(scrollBar.transform.position.x, -2.01f, scrollBar.transform.position.z);
            return;
        }


        foreach (Transform child in heroSlotsList.transform)
        {
            if (child.name.Contains("HeroSlot"))
            {
                child.position = new Vector3(child.position.x, child.position.y + heroPlateStep, child.position.z);
            }
        }
        scrollBar.transform.position = new Vector3(scrollBar.transform.position.x, scrollBar.transform.position.y - scrollBarStep, scrollBar.transform.position.z);
    }
}
