using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using Framework.Asset;
using Framework.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class SettingTopBarItem : TopBarItem
{
    private Button button;
    protected override void Awake()
    {
        button = transform.GetComponent<Button>();
        button?.onClick.AddListener(onClick);
    }
    protected void onClick()
    {
        if (topBarType == TopBarType.Setting)
        {
            //打开金币面板
            UIManager.Inst.ShowUI(UIName.UISettingPanel);
        }

    }

  

}
