using UnityEngine;
using System.Collections;

public class ExitButtonScript : ButtonBaseMouseEvents {

    new void OnMouseDown()
    {
        Application.Quit();
    }
}
