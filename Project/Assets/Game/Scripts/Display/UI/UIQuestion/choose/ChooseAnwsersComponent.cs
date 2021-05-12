using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseAnwsersComponent : MonoBehaviour
{
    
    private string[] _anwserKeys = new[] {"A", "B", "C", "D"};
    
    private Dictionary<string, ChooseAnwserItem> anwserItems;

    private string rightAnwser;

    public Action<bool> OnSelectListener;
    // Start is called before the first frame update
    public void Init()
    {
        anwserItems = new Dictionary<string, ChooseAnwserItem>();
        foreach (var key in _anwserKeys)
        {
            var c = transform.Find(key).GetComponent<ChooseAnwserItem>();
            c.OnSelectListener += OnSelectAnwser;
            anwserItems[key] = c;
        }
    }

    public void SetAnwsers(Dictionary<string,string> anwsers,string rightAnwser)
    {
        this.rightAnwser = rightAnwser;
        foreach (var anwserItem in anwserItems)
        {
            var key = anwserItem.Key;
            anwserItem.Value.SetAnwser(key,anwsers[key],rightAnwser.Trim().ToLower() == key.Trim().ToLower());
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
