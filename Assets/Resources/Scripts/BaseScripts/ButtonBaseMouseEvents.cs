using System;
using System.Xml.Schema;
using Assets.Resources.Scripts;
using UnityEngine;
using System.Collections;

/*
 * Base class for nearly every button in the game. 
 * Contains basic functionality for changing textures which depends on current button state.
 * Also there is a special Action to simplify adding method which will be called after mouse click
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
        if (!backButtonTex) return; 
        gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(backButtonTex,
            new Rect(0, 0, backButtonTex.width, backButtonTex.height), new Vector2(0, 0));
    }
}
