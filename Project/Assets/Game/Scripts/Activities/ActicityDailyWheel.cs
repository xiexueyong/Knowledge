using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActicityDailyWheel : ActicityBase
{
    protected override void OnAwake()
    {
        Button btn = transform.GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            UIManager.Inst.ShowUI(UIName.UIWheel,false,WheelTools.GetWheel(1),WheelTools.WheelType.DailyWheel,1);
        });
        
    }

    public override void Refresh()
    {
        // gameObject.SetActive(false);
    }
    
}
