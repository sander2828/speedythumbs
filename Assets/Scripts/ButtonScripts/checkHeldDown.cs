using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class checkHeldDown : MonoBehaviour
{
    public bool isHeldDown = false;
    
    public void OnPress()
    {
        isHeldDown = true;
       // Debug.Log(isHeldDown);
    }

    public void OnRelease()
    {
        isHeldDown = false;
        //Debug.Log(isHeldDown);
    }
}
