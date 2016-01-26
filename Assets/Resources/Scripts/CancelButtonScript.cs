using UnityEngine;
using System.Collections;

public class CancelButtonScript : ButtonBaseMouseEvents {


    void OnMouseDown()
    {
        var parentObj = gameObject.transform.parent.gameObject;
        Destroy(parentObj);
    }
}
