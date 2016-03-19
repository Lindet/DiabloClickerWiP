using Assets.Resources.Scripts;
using UnityEngine;
using UnityEngine.Rendering;

public class HeroSlotScript : ButtonBaseMouseEvents
{
    public int CharacterId = -1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    new void OnMouseDown()
    {
        if(_State == ButtonState.Selected) return;

        for (int i = 0; i < 19; i++)
        {
            var obj = GameObject.Find(string.Format("HeroSlot_{0}", i));
            if (obj == null || obj == gameObject) continue;
            if (obj.GetComponent<SpriteRenderer>().sprite.texture.name != "BattleNetButton_MainMenu_Selected")
                continue;
            obj.GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Up;

            var prevNameTextObject = obj.transform.FindChild(obj.name + "_name");
            if (prevNameTextObject)
                prevNameTextObject.gameObject.GetComponent<Renderer>().material.color = new Color32(243, 170, 85, 255);

            var prevLevelTextObject = obj.transform.FindChild(obj.name + "_class_level");
            if (prevLevelTextObject)
                prevLevelTextObject.gameObject.GetComponent<Renderer>().material.color = new Color32(141, 105, 53, 255);
        }

        var nameTextObject = gameObject.transform.FindChild(gameObject.name + "_name");
        if (nameTextObject)
            nameTextObject.gameObject.GetComponent<Renderer>().material.color = Color.white;

        var levelTextObject = gameObject.transform.FindChild(gameObject.name + "_class_level");
        if (levelTextObject)
            levelTextObject.gameObject.GetComponent<Renderer>().material.color = Color.white;

        _State = ButtonState.Selected;
        Account.CurrentAccount.SetCurrentHero(CharacterId); 
    }

    new void OnMouseEnter()
    {
        if (_State == ButtonState.Selected) return;
        base.OnMouseEnter();
    }

    new void OnMouseExit()
    {
        if (_State == ButtonState.Selected) return;
        base.OnMouseExit();
    }

    new void OnMouseUpAsButton()
    {
        if (_State == ButtonState.Selected) return;
        base.OnMouseUpAsButton();
    }

    public void SetThisSlotSelected()
    {
        var nameTextObject = gameObject.transform.FindChild(gameObject.name + "_name");
        if (nameTextObject)
            nameTextObject.gameObject.GetComponent<Renderer>().material.color = Color.white;

        var levelTextObject = gameObject.transform.FindChild(gameObject.name + "_class_level");
        if (levelTextObject)
            levelTextObject.gameObject.GetComponent<Renderer>().material.color = Color.white;

        _State = ButtonState.Selected;
        Account.CurrentAccount.SetCurrentHero(CharacterId); 
    }
    
}
