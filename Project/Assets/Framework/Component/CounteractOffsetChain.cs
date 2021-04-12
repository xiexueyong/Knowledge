using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounteractOffsetChain : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        if (UIManager.Inst.chainHeight != 0)
        {
            var chainHeight = UIManager.Inst.chainHeight;
            (transform as RectTransform).offsetMax = new Vector2(0, -chainHeight);
        }
        
    }

  
}
