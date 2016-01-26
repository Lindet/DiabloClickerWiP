using UnityEngine;
using System.Collections;

public class ScrollBarScript : MonoBehaviour
{

    private bool isPressed;
    private Vector3 prevMousePosition;
    private Camera curCam;
    private const float scrollBarStep = 0.0478f;
    private const float heroPlateStep = 0.04f;

    void Start()
    {
        curCam = Camera.main;
    }

    void Update()
    {
        if (!isPressed) return; //Движение происходит только при нажатой кнопки
        var curMousePosition = curCam.ScreenToWorldPoint(Input.mousePosition);
        if (prevMousePosition == curMousePosition) return; //Если мышь не сдвинулась - ничего не происходит
        var diff = curMousePosition - prevMousePosition; //разница позиций
        prevMousePosition = curMousePosition;
        if(curMousePosition.y > 2.77f && diff.y < 0) return; // Максимальная точка, при которой просчитывается движение
        if (curMousePosition.y < -2.01f && diff.y > 0) return; // Минимальная точка, при которой просчитывается движение
        if(transform.position.y + diff.y > 2.77f)
            transform.position = new Vector3(transform.position.x, 2.77f, transform.position.z);
        else if (transform.position.y + diff.y < -2.01f)
            transform.position = new Vector3(transform.position.x, -2.01f, transform.position.z);
        else
            transform.position = new Vector3(transform.position.x, transform.position.y + diff.y, transform.position.z);

        MoveHeroSlots(diff.y);
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
        if (lastSlotObject.transform.position.y == - 2.3f && stepCount < 0) return;
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
    }

    void OnMouseDown()
    {
        isPressed = true;
        prevMousePosition = curCam.ScreenToWorldPoint(Input.mousePosition);

        var backButtonTex = Resources.Load<Texture2D>(@"Controls/ScrollbarThumb_Down");
        gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(backButtonTex,
            new Rect(0, 0, backButtonTex.width, backButtonTex.height), new Vector2(0, 0));
    }

    void OnMouseUp()
    {
        isPressed = false;

        var backButtonTex = Resources.Load<Texture2D>(@"Controls/ScrollbarThumb_Up");
        gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(backButtonTex,
            new Rect(0, 0, backButtonTex.width, backButtonTex.height), new Vector2(0, 0));
    }
    void OnMouseEnter()
    {
        var backButtonTex = Resources.Load<Texture2D>(@"Controls/ScrollbarThumb_Over");
        gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(backButtonTex,
            new Rect(0, 0, backButtonTex.width, backButtonTex.height), new Vector2(0, 0));
    }

    void OnMouseExit()
    {
        var backButtonTex = Resources.Load<Texture2D>(@"Controls/ScrollbarThumb_Up");
        gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(backButtonTex,
            new Rect(0, 0, backButtonTex.width, backButtonTex.height), new Vector2(0, 0));
    }
}
