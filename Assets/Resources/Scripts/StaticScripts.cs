using System;
using System.Runtime.Remoting;
using Assets.Resources.Scripts;
using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

public static class StaticScripts {

    public static void CreateGameObj(string gameObjName, string texturePath, Vector3 localScale, Vector3 position,
          bool needCollider = false, byte ColliderType = 0, bool specialClass = false, Type className = null, bool child = false, string parentName = "", int sortingOrder = 1)
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
                if (obj.GetComponent<SpriteRenderer>() != null && sortingOrder < obj.GetComponent<SpriteRenderer>().sortingOrder + 1)
                    gameObj.GetComponent<SpriteRenderer>().sortingOrder = obj.GetComponent<SpriteRenderer>().sortingOrder + 1;
                else
                    gameObj.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;
                return;
            }
        }

        if (sortingOrder != 1)
            gameObj.GetComponent<SpriteRenderer>().sortingOrder = sortingOrder;

        gameObj.transform.localScale = localScale;
        gameObj.transform.position = position;
    }


    public static void CreateTextObj(string gameObjName, string text, Vector3 localScale, Vector3 position, FontType font, int fontSize,
       Color32 textColor, TextAlignment alignment = TextAlignment.Left, bool child = false, string parentName = "", FontStyle style = FontStyle.Normal)
    {
        Font currentFont = font == FontType.DiabloFont ? Resources.Load<Font>(@"Fonts/exocet-blizzard-light") : Resources.Load<Font>(@"Fonts/blizzard-regular");

        var textGameObj = new GameObject(gameObjName);
        var textMesh = textGameObj.AddComponent<TextMesh>();
        textGameObj.transform.localScale = localScale;
        textGameObj.GetComponent<Renderer>().material = currentFont.material;
        textGameObj.GetComponent<Renderer>().material.color = textColor;
        textMesh.text = text;
        textMesh.font = currentFont;
        textMesh.fontSize = fontSize;
        textMesh.alignment = alignment;
        textMesh.fontStyle = style;
        if (child)
        {
            var obj = GameObject.Find(parentName);
            if (obj != null)
            {
                textGameObj.transform.parent = obj.transform;
                textGameObj.transform.localScale = localScale;
                textGameObj.transform.localPosition = position;
                if (obj.GetComponent<SpriteRenderer>() != null)
                    textGameObj.GetComponent<Renderer>().sortingOrder =
                        obj.GetComponent<SpriteRenderer>().sortingOrder + 1;
                else
                    textGameObj.GetComponent<Renderer>().sortingOrder = 1;
                return;
            }
        }

        textGameObj.transform.localScale = localScale;
        textGameObj.transform.position = position;
    }


    public static Sprite CreateSprite(string texturePath, int widthTex = 0, int heightTex = 0)
    {
        var texture = Resources.Load<Texture2D>(texturePath);
        return Sprite.Create(texture, new Rect(0, 0,
            widthTex != 0? widthTex : texture.width,
            heightTex !=  0 ? heightTex : texture.height),
            new Vector2(0, 0));
    }
}
