﻿using System;
using System.Xml.Schema;
using Assets.Resources.Scripts;
using UnityEngine;
using System.Collections;

/*
 * Базовый класс для всех кнопок игры. Содержит в себе базовое описание функционала смены текстуры в зависимости от состояния кнопки.
 */

public class ButtonBaseMouseEvents : MonoBehaviour
{
    private ButtonState _state = ButtonState.Up;
    public ButtonState _State
    {
        get { return _state; }
        set
        {
            StateChangedMethod(_state, value);
            _state = value;
        }
    }

    public Action MethodOnClick { get; set; }


    protected void OnMouseDown()
    {
        if (_State == ButtonState.Disabled) return;
        _State = ButtonState.Down;
    }

    protected void OnMouseUpAsButton()
    {
        if (_State == ButtonState.Disabled) return;
        _State = ButtonState.Over;
        if(MethodOnClick != null)
            MethodOnClick.DynamicInvoke();
    }

    protected void OnMouseEnter()
    {
        if (_State == ButtonState.Disabled) return;
        _State = ButtonState.Over;
    }

    protected void OnMouseExit()
    {
        if (_State == ButtonState.Disabled) return;
        _State = ButtonState.Up;
    }

    protected void StateChangedMethod(ButtonState prevState, ButtonState currState)
    {
        var backButtonTex = Resources.Load<Texture2D>(String.Format(@"Buttons/{0}", gameObject.GetComponent<SpriteRenderer>().sprite.texture.name.Replace(prevState.ToString(), currState.ToString())));
        gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(backButtonTex,
            new Rect(0, 0, backButtonTex.width, backButtonTex.height), new Vector2(0, 0));
    }
}