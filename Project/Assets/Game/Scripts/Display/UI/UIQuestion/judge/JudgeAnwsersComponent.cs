using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JudgeAnwsersComponent : MonoBehaviour
{
   
    private string[] _anwserKeys = new[] {"Y", "N"};
    
    private Dictionary<string, JudgeAnwserItem> anwserItems;

    private string rightAnwser;

    public Action<bool> OnSelectListener;
    // Start is called before the first frame update
    void Init()
    {
        anwserItems = new Dictionary<string, JudgeAnwserItem>();
        foreach (var key in _anwserKeys)
        {
            var c = transform.Find(key).GetComponent<JudgeAnwserItem>();
            c.OnSelectListener += OnSelectAnwser;
            anwserItems[key] = c;
        }
    }

    public void SetAnwsers(string rightAnwser)
    {
        this.rightAnwser = rightAnwser;
        foreach (var anwserItem in anwserItems)
        {
            var key = anwserItem.Key;
            anwserItem.Value.SetAnwser(key,key=="Y"?"对":"错",rightAnwser == key);
        }
    }

    public void OnSelectAnwser(bool isRight)
    {
        OnSelectListener.Invoke(isRight);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
