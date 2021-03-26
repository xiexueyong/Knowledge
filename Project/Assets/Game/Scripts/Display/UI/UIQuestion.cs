
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class UIQuestion : BaseUI
{
    private Button play_btn;

    public override void OnAwake()
    {
    }
    
    public override void OnStart()
    {
    }

    public override void SetData(params object[] args)
    {
        int level = (int)args[0];
        
    }
}
