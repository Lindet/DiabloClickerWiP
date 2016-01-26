﻿using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using UnityEngine;
using System.Collections;

public class CreateNewAccountButtonScript : ButtonBaseMouseEvents {


    void OnMouseDown()
    {
        var textBoxes = GameObject.Find("LoginAndPasswordTextBoxes");
        var login = textBoxes.GetComponent<LoginPasswordTextBoxesScript>().AccountName;
        var pass = textBoxes.GetComponent<LoginPasswordTextBoxesScript>().Password;

        using (var sw = new StreamWriter(@"Accounts.txt", true))
        {
            sw.WriteLine(login);
            sw.WriteLine(pass);
            Account.CurrentAccount.AccountName = login;
        }

        DestroyImmediate(GameObject.Find("LogonStatusWindow"));
        ReadWriteAccountDateScript.WriteAccountData(login);
    }
}