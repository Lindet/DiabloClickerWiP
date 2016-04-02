using System;
using UnityEngine;
using System.Collections;

/*
 * Class, which is used to determine base behavior of the CheckBoxes - changing states, textures etc with every mouse event.
 */
public class CheckBoxMouseEvents : MonoBehaviour
{
    protected bool Checked = false;

    public event ValueChangedEventHandler ValueChanged;
    public delegate void ValueChangedEventHandler();

    void Start()
    {
        ValueChanged += OnValueChanged;
    }

    private void OnValueChanged()
    {
        Checked = !Checked;
        var checkBoxCheckedTex = Checked ? Resources.Load<Texture2D>(@"UI/BattleNetLogin_CheckboxCheckedOver") : Resources.Load<Texture2D>(@"UI/BattleNetLogin_CheckboxOver");
        gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(checkBoxCheckedTex,
                    new Rect(0, 0, checkBoxCheckedTex.width, checkBoxCheckedTex.height), new Vector2(0, 0));
    }

    protected void OnMouseUpAsButton()
    {
        if(ValueChanged != null)
            ValueChanged();
    }

    protected void OnMouseEnter()
    {
        var checkBoxCheckedTex = Checked ? Resources.Load<Texture2D>(@"UI/BattleNetLogin_CheckboxCheckedOver") : Resources.Load<Texture2D>(@"UI/BattleNetLogin_CheckboxOver");
        gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(checkBoxCheckedTex,
                    new Rect(0, 0, checkBoxCheckedTex.width, checkBoxCheckedTex.height), new Vector2(0, 0));
    }

    protected void OnMouseExit()
    {
        var checkBoxCheckedTex = Checked ? Resources.Load<Texture2D>(@"UI/BattleNetLogin_CheckboxCheckedUp") : Resources.Load<Texture2D>(@"UI/BattleNetLogin_CheckboxUp");
        gameObject.GetComponent<SpriteRenderer>().sprite = Sprite.Create(checkBoxCheckedTex,
                    new Rect(0, 0, checkBoxCheckedTex.width, checkBoxCheckedTex.height), new Vector2(0, 0));
    }
}
