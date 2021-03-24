using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurningCircleComp : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(-Vector3.forward*5);
    }
}
