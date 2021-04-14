using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplainComponent : MonoBehaviour
{
    [SerializeField] private Text txt_subject;
    [SerializeField] private Text txt_explain;
    [SerializeField] private Button btn_mask;
    public void Awake()
    {
    }

    public void Init()
    {
        btn_mask.onClick.AddListener(onClickMask);
    }

    void onClickMask()
    {
       Hide();
    }

    public void Show(string subject,string content)
    {
        transform.gameObject.SetActive(true);
        txt_subject.text = subject;
        txt_explain.text = content;
    }
    public void Hide()
    {
        if (transform.gameObject.activeSelf)
        {
            transform.gameObject.SetActive(false);
        }
    }
    
}
