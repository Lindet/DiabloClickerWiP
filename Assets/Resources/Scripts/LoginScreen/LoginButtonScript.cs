using System;
using System.IO;
using System.Linq;
using Assets.Resources.Scripts;
using UnityEngine;
using System.Collections;

public class LoginButtonScript : ButtonBaseMouseEvents
{
    new void OnMouseDown()
    {
        base.OnMouseDown();
        if(_State == ButtonState.Disabled) return;
        if (GameObject.Find("LogonStatusWindow") != null) return;
        var textBoxes = GameObject.Find("LoginAndPasswordTextBoxes");
        var login = textBoxes.GetComponent<LoginPasswordTextBoxesScript>().AccountName;
        var pass = textBoxes.GetComponent<LoginPasswordTextBoxesScript>().Password;


        try
        {
            using (var sr = new StreamReader(new FileStream(@"Accounts.txt", FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite)))
            {
                bool flag = false;
                while (!flag && !sr.EndOfStream)
                {
                    var text = sr.ReadLine();
                    if (login == text)
                    {
                        if (pass == sr.ReadLine())
                        {
                            flag = true;
                            ReadWriteAccountDateScript.ReadAccountData(login);
                            DestroyImmediate(GameObject.Find("LoginSceneObject"));
                            GameObject.Find("MainLogic").GetComponent<Main>().CurrentGameState = GameState.MainMenu;
                        }
                        else
                        {
                            StaticScripts.CreateGameObj("LogonStatusWindow", @"Borders/Login/BattleNetLogin_LogonStatusWindow", new Vector3(0.85f, 0.85f), new Vector3(-2.35f, -2.05f, -2f),sortingOrder:10);
                            StaticScripts.CreateGameObj("LogonButtonOk", @"Buttons/BattleNetButton_RedUp_245x66", new Vector3(1f, 1f), new Vector3(1.5f, 0.4f, -3f), true, 1, true, typeof(CancelButtonScript), true, "LogonStatusWindow");
                            StaticScripts.CreateTextObj("WarningText", "Your login information is incorrect. Please try again.", new Vector3(0.02f, 0.02f), new Vector3(0.8f, 2.8f, -5f), FontType.StandartFont, 80, new Color32(243, 170, 85, 255), TextAlignment.Center, true, "LogonStatusWindow");
                            StaticScripts.CreateTextObj("WarningTitle", "WARNING!", new Vector3(0.02f, 0.02f), new Vector3(2.35f, 3.95f, -5f), FontType.StandartFont, 80, new Color32(243, 170, 85, 255), child:true, parentName:"LogonStatusWindow");
                            StaticScripts.CreateTextObj("OKButtonText", "OK", new Vector3(0.02f, 0.02f), new Vector3(1.1f, 0.4f, -5f), FontType.DiabloFont, 80, new Color32(243, 170, 85, 255), child:true, parentName:"LogonButtonOk");
                            return;
                        }
                    }
                }

                if (!flag)
                {
                    StaticScripts.CreateGameObj("LogonStatusWindow", @"Borders/Login/BattleNetLogin_LogonStatusWindow", new Vector3(0.85f, 0.85f), new Vector3(-2.35f, -2.05f, -2f), sortingOrder:10);
                    StaticScripts.CreateGameObj("LogonButtonCreate", @"Buttons/BattleNetButton_RedUp_245x66", new Vector3(1f, 1f), new Vector3(0.3f, 0.35f, -3f), true, 1, true, typeof(CreateNewAccountButtonScript), true, "LogonStatusWindow");
                    StaticScripts.CreateGameObj("LogonButtonCancel", @"Buttons/BattleNetButton_RedUp_245x66", new Vector3(1f, 1f), new Vector3(2.78f, 0.35f, -3f), true, 1, true, typeof(CancelButtonScript), true, "LogonStatusWindow");
                    StaticScripts.CreateTextObj("WarningTitle", "WARNING!", new Vector3(0.02f, 0.02f), new Vector3(2.35f, 3.95f, -5f), FontType.StandartFont, 80, new Color32(243, 170, 85, 255), child: true, parentName: "LogonStatusWindow");
                    StaticScripts.CreateTextObj("WarningText", "There is no information about an account with such\nname. Do you want to create new account with that \nname?", new Vector3(0.02f, 0.02f), new Vector3(0.8f, 2.8f, 0f), FontType.StandartFont, 80, new Color32(243, 170, 85, 255), TextAlignment.Center, true, "LogonStatusWindow");
                    StaticScripts.CreateTextObj("CreateButtonText", "Create", new Vector3(0.02f, 0.02f), new Vector3(0.85f, 0.4f, -5f), FontType.DiabloFont, 80, new Color32(243, 170, 85, 255), child: true, parentName: "LogonButtonCreate");
                    StaticScripts.CreateTextObj("CancelButtonText", "Cancel", new Vector3(0.02f, 0.02f), new Vector3(0.9f, 0.4f, -5f), FontType.DiabloFont, 80, new Color32(243, 170, 85, 255), child: true, parentName: "LogonButtonCancel");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogException(ex, this);
        }
        

    }
}
