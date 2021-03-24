using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Comp_Move : MonoBehaviour
{
    //x  -7 7
    public float speedX = 5;
    public float deltaXMin = -7;
    public float deltaXMax = 7;
    private float deltaX;
    private Vector3 oldPos;

    void Start()
    {
        deltaX = 0f;
        oldPos = transform.localPosition;
    }

    void Update()
    {
        deltaX += speedX * Time.deltaTime;
        if (deltaX > deltaXMax)
        {
            speedX = -Math.Abs(speedX);
        }
        else if(deltaX < deltaXMin)
        {
            speedX = Math.Abs(speedX);
        }


        transform.localPosition = new Vector3(oldPos.x + deltaX, oldPos.y, oldPos.z);


    }
}
