using UnityEngine;
using System.Collections;

public class HeroScrollListScript : MonoBehaviour {

    private const float scrollBarStep = 0.0478f;
    private const float heroPlateStep = 0.04f;

    private bool isMouseOver = false;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(isMouseOver && Input.mouseScrollDelta != Vector2.zero)
            MoveHeroSlots(Input.mouseScrollDelta.y /2);
	}

    void MoveHeroSlots(float height)
    {
        var stepCount = height / scrollBarStep;
        stepCount *= heroPlateStep;

        var mainHeroListObject = GameObject.Find("HeroListGameObject");
        var firstSlotObject = GameObject.Find("HeroSlot_0");
        if (firstSlotObject.transform.position.y == 2.6f && stepCount > 0) return;
        if (stepCount > 0 && firstSlotObject.transform.position.y - stepCount < 2.6f)
        {
            stepCount = firstSlotObject.transform.position.y - 2.6f;
        }

        var lastSlotObject = GameObject.Find("HeroSlot_11");
        if (lastSlotObject.transform.position.y == -2.3f && stepCount < 0) return;
        if (stepCount < 0 && lastSlotObject.transform.position.y - stepCount > -2.3f)
        {
            stepCount = lastSlotObject.transform.position.y - (-2.3f);
        }


        foreach (Transform child in mainHeroListObject.transform)
        {
            if (child.name.Contains("HeroSlot"))
            {
                child.position = new Vector3(child.position.x, child.position.y - stepCount, child.position.z);
            }
        }

        var scrollBarObject = GameObject.Find("Scrollbar");
        if (!scrollBarObject) return;

        if (scrollBarObject.transform.position.y + height > 2.77f)
            scrollBarObject.transform.position = new Vector3(scrollBarObject.transform.position.x, 2.77f, scrollBarObject.transform.position.z);
        else if (scrollBarObject.transform.position.y + height < -2.01f)
            scrollBarObject.transform.position = new Vector3(scrollBarObject.transform.position.x, -2.01f, scrollBarObject.transform.position.z);
        else
            scrollBarObject.transform.position = new Vector3(scrollBarObject.transform.position.x, scrollBarObject.transform.position.y + height, scrollBarObject.transform.position.z);
    }

    void OnMouseExit()
    {
        isMouseOver = false;
    }

    void OnMouseEnter()
    {
        isMouseOver = true;
    }
}
