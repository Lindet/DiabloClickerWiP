using UnityEngine;


//This is the code for the button which scrolls HeroList 
public class ButtonScrollUp : MonoBehaviour {

    private const float scrollBarStep = 0.0956f;
    private const float heroPlateStep = 0.08f;
    private GameObject scrollBar;
    private GameObject firstHeroSlot;
    private GameObject heroSlotsList;

    void OnMouseDown()
    {
        InvokeRepeating("ScrollUp", 0f, 0.05f);

        var backButtonTex = Resources.Load<Texture2D>(@"Controls/ScrollSmallUp_Down");
        gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(backButtonTex,
            new Rect(0, 0, backButtonTex.width, backButtonTex.height), new Vector2(0, 0));
    }

    void OnMouseUp()
    {
        CancelInvoke("ScrollUp");

        var backButtonTex = Resources.Load<Texture2D>(@"Controls/ScrollSmallUp_Up");
        gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(backButtonTex,
            new Rect(0, 0, backButtonTex.width, backButtonTex.height), new Vector2(0, 0));
    }

    void OnMouseEnter()
    {
        var backButtonTex = Resources.Load<Texture2D>(@"Controls/ScrollSmallUp_Over");
        gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(backButtonTex,
            new Rect(0, 0, backButtonTex.width, backButtonTex.height), new Vector2(0, 0));
    }

    void OnMouseExit()
    {
        var backButtonTex = Resources.Load<Texture2D>(@"Controls/ScrollSmallUp_Up");
        gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(backButtonTex,
            new Rect(0, 0, backButtonTex.width, backButtonTex.height), new Vector2(0, 0));
    }

    void ScrollUp()
    {
        if (scrollBar == null) scrollBar = GameObject.Find("Scrollbar");
        if (scrollBar.transform.position.y >= 2.77f) return; 

        if (firstHeroSlot == null) firstHeroSlot = GameObject.Find("HeroSlot_0");
        if (firstHeroSlot.transform.position.y == 2.6f) return; //check if it's the minimum allowed position
        if (heroSlotsList == null) heroSlotsList = GameObject.Find("HeroListGameObject");
        if (firstHeroSlot.transform.position.y - heroPlateStep < 2.6f) // Do this if there is some more length but not enough for the full step
        {
            var diff = firstHeroSlot.transform.position.y - 2.6f;
            foreach (Transform child in heroSlotsList.transform)
            {
                if (child.name.Contains("HeroSlot"))
                {
                    child.position = new Vector3(child.position.x, child.position.y - diff, child.position.z);
                }
            }
            scrollBar.transform.position = new Vector3(scrollBar.transform.position.x, 2.77f, scrollBar.transform.position.z);
            return;
        }


        foreach (Transform child in heroSlotsList.transform)
        {
            if (child.name.Contains("HeroSlot"))
            {
                child.position = new Vector3(child.position.x, child.position.y - heroPlateStep, child.position.z);
            }
        }
        scrollBar.transform.position = new Vector3(scrollBar.transform.position.x, scrollBar.transform.position.y + scrollBarStep, scrollBar.transform.position.z);
    }

}
