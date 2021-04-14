using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplainButton : MonoBehaviour
{
    [SerializeField] private Text txt_cost;
    [SerializeField] private Image img_coin;
    // Start is called before the first frame update
    public void setExplainCost(int value)
    {
        if (value > 0)
        {
            txt_cost.text = value.ToString();
        }
        else
        {
            txt_cost.text = "免费";
        }
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
