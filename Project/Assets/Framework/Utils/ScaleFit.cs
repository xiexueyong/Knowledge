using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleFit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(Screen.height/Screen.width >= 1280f/720f) {
            this.transform.localScale = new Vector3(1f, (720f/Screen.width * Screen.height)/1280f, 1f);
        } else {
            this.transform.localScale = new Vector3((1280f/Screen.height * Screen.width)/720f, 1f, 1f);
        }
    }
}
