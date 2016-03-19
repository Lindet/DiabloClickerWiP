using UnityEngine;
using System.Collections;

public class CancelButtonScript : ButtonBaseMouseEvents {


    new void OnMouseDown()
    {
        var parentObj = gameObject.transform.parent.gameObject;
        Destroy(parentObj);
    }
}
