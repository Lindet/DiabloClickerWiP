using System;
using System.Linq;
using System.Timers;
using System.Xml.Schema;
using Assets.Resources.Scripts;
using UnityEngine;
using System.Collections;

public class LoginPasswordTextBoxesScript : MonoBehaviour {

    GameObject caretGameObject;
    private GameObject accountNameGameObject;
    private GameObject passwordGameObject;


    public bool caretState = false;
    private Camera mainCamera;
    public string AccountName { get; private set; }
    public string Password { get; private set; }
    

    void Start()
    {
        AccountName = string.Empty;
        Password = string.Empty;
        StaticScripts.CreateTextObj("textBoxCaret", "|", new Vector3(0.02f, 0.02f, 0.02f), new Vector3(0f, 0f), FontType.StandartFont, 100, new Color32(243, 170, 85, 255), child:true, parentName:gameObject.name);
        caretGameObject = GameObject.Find("textBoxCaret");
        caretGameObject.GetComponent<Renderer>().enabled = false;

        StaticScripts.CreateTextObj("AccountName", AccountName, new Vector3(0.02f, 0.02f, 0.02f), new Vector3(0.6f, 2.48f), FontType.StandartFont, 80, new Color32(243, 170, 85, 255), child: true, parentName: gameObject.name);
        accountNameGameObject = GameObject.Find("AccountName");

        StaticScripts.CreateTextObj("Password", Password, new Vector3(0.02f, 0.02f, 0.02f), new Vector3(0.6f, 1.2f), FontType.StandartFont, 80, new Color32(243, 170, 85, 255), child: true, parentName: gameObject.name);
        passwordGameObject = GameObject.Find("Password");
        mainCamera = GameObject.Find("MainCamera").GetComponents<Camera>()[0];
    }

    void Update()
    {
        if (!caretState) return;
        if (!Input.anyKey) return;
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (caretGameObject.transform.localPosition.y == 1.3f)
                caretGameObject.transform.localPosition = new Vector3(accountNameGameObject.transform.localPosition.x, 2.53f);
            else
                caretGameObject.transform.localPosition = new Vector3(passwordGameObject.transform.localPosition.x, 1.3f);
        }
        var text = Input.inputString;
        if (text == string.Empty) return;

        if (caretGameObject.transform.localPosition.y == 1.3f)
        {
            Password = ChangeText(Password, text, 20);
            string passwordText = string.Empty;
            for (int i = 0; i < Password.Length; i++)
            {
                passwordText += "*";
            }

            passwordGameObject.GetComponent<TextMesh>().text = passwordText;
            caretGameObject.transform.localPosition =
                new Vector3(
                    passwordGameObject.transform.localPosition.x +
                    passwordGameObject.GetComponent<Renderer>().bounds.size.x + 0.015f * Password.Length, 1.3f);
        }
        else
        {
            AccountName = ChangeText(AccountName, text, 25);
            accountNameGameObject.GetComponent<TextMesh>().text = AccountName;
            caretGameObject.transform.localPosition = new Vector3(
                accountNameGameObject.transform.localPosition.x +
                accountNameGameObject.GetComponent<Renderer>().bounds.size.x + 0.015f * AccountName.Length, 2.53f);
        }

        if (Password.Length > 0 && AccountName.Length > 5)
            GameObject.Find("LoginButton").GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Up;
        if(Password.Length == 0 || AccountName.Length < 5)
            GameObject.Find("LoginButton").GetComponent<ButtonBaseMouseEvents>()._State = ButtonState.Disabled;

    }
    
    void OnMouseDown()
    {
        var currentPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (currentPosition.x >= -1.6f && currentPosition.x <= 1.6f && currentPosition.y >= -1.8f &&
            currentPosition.y <= -1.4f)
        {
            EnableCaret(new Vector3(passwordGameObject.transform.localPosition.x + passwordGameObject.GetComponent<Renderer>().bounds.size.x + 0.015f * Password.Length, 1.3f));
        }
        else if (currentPosition.x >= -1.6f && currentPosition.x <= 1.6f && currentPosition.y >= -0.7f &&
            currentPosition.y <= -0.3f)
        {
            EnableCaret(new Vector3(accountNameGameObject.transform.localPosition.x + accountNameGameObject.GetComponent<Renderer>().bounds.size.x + 0.015f * AccountName.Length, 2.53f));
        }
    }

    string ChangeText(string text, string newText, int maxChar)
    {
        if (newText == "\b" && text.Length > 0)
        {
            text = text.Remove(text.Length - 1);
        }
        else
        {
            foreach (var sym in newText.Where(Char.IsLetterOrDigit))
            {
                if (text.Length >= maxChar) return text;
                text += sym;
            }
        }
        return text;
    }
    void EnableCaret(Vector3 position)
    {
        CancelInvoke("caretFlashing");
        caretGameObject.transform.localPosition = position;
        caretGameObject.GetComponent<Renderer>().enabled = true;
        InvokeRepeating("caretFlashing", 0f, 0.5f);
        caretState = true;
    }

    private void caretFlashing()
    {
        if (!caretState)
        {
            CancelInvoke("caretFlashing");
            return;
        }
        caretGameObject.GetComponent<Renderer>().enabled = !caretGameObject.GetComponent<Renderer>().enabled;
    }
}
