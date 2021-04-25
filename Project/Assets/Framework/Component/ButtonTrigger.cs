using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTrigger : MonoBehaviour
{
    private bool _on;
    public bool On
    {
        get
        {
            return _on;
        }
        set
        {
            _on = value;
            RefreshUI();
        }
    }
    public Text txt_on;
    public Text txt_off;
    public RectTransform dot;

    private int x_off = -26;
    private int x_on = 26;
    public Action<bool> Listener;
    

    public Button btn;

    private void Awake()
    {
        btn.onClick.AddListener(onClick);
        RefreshUI();
    }

    void onClick()
    {
        On = !On;
        RefreshUI();
        Listener.Invoke(On);
    }

    void RefreshUI()
    {
        if (On)
        {
            txt_off.gameObject.SetActive(false);
            txt_on.gameObject.SetActive(true);
            dot.localPosition = new Vector3(x_on,0,0);
        }
        else
        {
            txt_off.gameObject.SetActive(true);
            txt_on.gameObject.SetActive(false);
            dot.localPosition = new Vector3(x_off,0,0); 
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
