﻿using System;
using Assets.Resources.Scripts;
using UnityEngine;

//This static class contains some static methods to simplify GameObject creation.
public static class StaticScripts
{

    /*
     * This method creates new GameObject with parameters which were passed. 
     * - needCollider and ColliderType. Use it if you want to interact with the mouse events(over, down etc). 1 is for squared object, 2 for circle
     * - specialClass and className. Use it if you want to add some custom component to your object
     * - sortingOrder. Use it if you want that object to be behind/on top of another one. 
     *                 child-object will have parentSortingOrder + 1 here.
     */

    public static GameObject CreateGameObj(string gameObjName, string texturePath, Vector3 localScale, Vector3 position,
        bool needCollider = false, byte ColliderType = 0, bool specialClass = false, Type className = null,
        bool child = false, string parentName = "", int sortingOrder = 1)
    {
        var gameObj = new GameObject(gameObjName);
        gameObj.AddComponent<SpriteRenderer>().sprite = CreateSprite(@texturePath);

        if (needCollider)
        {
            switch (ColliderType)
            {
                case 0:
                    throw new Exception("Wrong ColliderType");
                case 1:
                    gameObj.AddComponent<BoxCollider2D>();
                    break;
                case 2:
                    gameObj.AddComponent<CircleCollider2D>();
                    break;
            }
        }

        if (specialClass)
        {
            gameObj.AddComponent(className);
        }

        if (child)
        {
            var obj = GameObject.Find(parentName);
            if (obj != null)
            {
                gameObj.transform.parent = obj.transform;
                gameObj.transform.localScale = localScale;
                gameObj.transform.localPosition = position;
                if (obj.GetComponent<SpriteRenderer>() != null &&
                    sortingOrder < obj.GetComponent<SpriteRenderer>().sortingOrder + 1)
                    gameObj.GetComponent<SpriteRenderer>().sortingOrder =
                        obj.GetComponent<SpriteRenderer>().sortingOrder + 1;
                else
                    gameObj.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
                return gameObj;
            }
        }

        if (sortingOrder != 1)
            gameObj.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;

        gameObj.transform.localScale = localScale;
        gameObj.transform.position = position;

        return gameObj;
    }



    /*
     * This method creates new GameObject with parameters which were passed. 
     * - localScale. Nearly deprecated. Was used to down-scale the size of the text so it won't be so ugly
     * - textColor. TODO: Make a enum with all used numbers.
     */

    public static GameObject CreateTextObj(string gameObjName, string text, Vector3 localScale, Vector3 position,
        FontType font, int fontSize,
        Color32 textColor, TextAlignment alignment = TextAlignment.Left, bool child = false, string parentName = "",
        FontStyle style = FontStyle.Normal)
    {
        Font currentFont = font == FontType.DiabloFont
            ? Resources.Load<Font>(@"Fonts/exocet-blizzard-light")
            : Resources.Load<Font>(@"Fonts/blizzard-regular");

        var textGameObj = new GameObject(gameObjName);
        var textMesh = textGameObj.AddComponent<TextMesh>();
        textGameObj.GetComponent<Renderer>().material = currentFont.material;
        textGameObj.GetComponent<Renderer>().material.color = textColor;
        textMesh.text = text;
        textMesh.font = currentFont;
        textMesh.fontSize = fontSize/2;
        textMesh.characterSize = 0.04f;
        textMesh.alignment = alignment;
        textMesh.fontStyle = style;
        if (child)
        {
            var obj = GameObject.Find(parentName);
            if (obj != null)
            {
                textGameObj.transform.parent = obj.transform;
                textGameObj.transform.localScale = new Vector3(1f, 1f);
                textGameObj.transform.localPosition = position;
                if (obj.GetComponent<SpriteRenderer>() != null)
                    textGameObj.GetComponent<Renderer>().sortingOrder =
                        obj.GetComponent<SpriteRenderer>().sortingOrder + 1;
                else
                    textGameObj.GetComponent<Renderer>().sortingOrder = 1;
                return textGameObj;
            }
        }

        textGameObj.transform.position = position;
        return textGameObj;
    }



    //Just created sprite, nothing more.
    public static Sprite CreateSprite(string texturePath, int widthTex = 0, int heightTex = 0)
    {
        Sprite sprite = new Sprite();
        try
        {
            var texture = Resources.Load<Texture2D>(texturePath);
            sprite = Sprite.Create(texture, new Rect(0, 0,
                widthTex != 0 ? widthTex : texture.width,
                heightTex != 0 ? heightTex : texture.height),
                new Vector2(0, 0));
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
            Debug.Log(texturePath);
        }

        return sprite;
    }
}
