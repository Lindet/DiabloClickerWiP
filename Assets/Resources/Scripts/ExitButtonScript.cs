using UnityEngine;
using System.Collections;

public class ExitButtonScript : ButtonBaseMouseEvents {

    void OnMouseDown()
    {
        Application.Quit();
    }
}
