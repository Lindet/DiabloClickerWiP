using System;
using System.Linq;
using System.Runtime.Remoting;
using Assets.Resources.Scripts;
using UnityEngine;
using System.Collections;

public class CharacterCreationTextBox : MonoBehaviour
{
    GameObject caretGameObject;
    private string characterName = string.Empty;
    private GameObject characterNameGameObject;

    public string CharacterName
    {
        get { return characterName; }
    }

    void Start()
    {
        StaticScripts.CreateTextObj("CharacterName", "", new Vector3(0.02f, 0.02f), new Vector3(0.3f, 0.4f, 0f), FontType.StandartFont, 110, new Color32(243, 170, 85, 255), TextAlignment.Center, true, "NameTextBox");
        characterNameGameObject = GameObject.Find("CharacterName");

        StaticScripts.CreateTextObj("characterNameCaret", "|", new Vector3(0.02f, 0.02f, 0.02f), new Vector3(0.2f, 0.45f), FontType.StandartFont, 120, new Color32(243, 170, 85, 255), child: true, parentName: gameObject.name);
        caretGameObject = GameObject.Find("characterNameCaret");
        caretGameObject.GetComponent<Renderer>().enabled = false;
        EnableCaret(new Vector3(characterNameGameObject.transform.localPosition.x, 0.45f));
    }

    void Update()
    {
        if (!Input.anyKey) return;
       
        var text = Input.inputString;
        if (text == string.Empty) return;
        if (text == "\b" && CharacterName.Length > 0)
        {
            characterName = CharacterName.Remove(CharacterName.Length - 1);

            if (CharacterName.Length < 3)
            {
                var button = GameObject.Find("CreateHeroButton");
                button.GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Disabled;
                button.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = new Color32(79, 75, 70, 255);
            }

        }
        else
        {
            foreach (var sym in text.Where(Char.IsLetterOrDigit))
            {
                if (CharacterName.Length > 12) return;
                if (CharacterName.Length == 3)
                {
                    var button = GameObject.Find("CreateHeroButton");
                    button.GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Up;
                    button.transform.GetChild(0).gameObject.GetComponent<Renderer>().material.color = new Color32(243,
                        170, 85, 255);
                }
                characterName = CharacterName + sym;

                
            }
        }
        characterNameGameObject.GetComponent<TextMesh>().text = CharacterName;
        Account.CurrentAccount.GetCurrentHero().Name = characterName;
        EnableCaret(new Vector3(characterNameGameObject.transform.localPosition.x + characterNameGameObject.GetComponent<Renderer>().bounds.size.x + 0.02f * CharacterName.Length, 0.45f));
    }


    void EnableCaret(Vector3 position)
    {
        CancelInvoke("caretFlashing");
        caretGameObject.transform.localPosition = position;
        caretGameObject.GetComponent<Renderer>().enabled = true;
        InvokeRepeating("caretFlashing", 0f, 0.5f);
    }

    private void caretFlashing()
    {
        caretGameObject.GetComponent<Renderer>().enabled = !caretGameObject.GetComponent<Renderer>().enabled;
    }

    public void OnDestroy()
    {
        CancelInvoke("caretFlashing");
    }
}
