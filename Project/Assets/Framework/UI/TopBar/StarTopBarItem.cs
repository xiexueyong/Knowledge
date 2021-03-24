using EventUtil;
using Framework.Storage;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class StarTopBarItem : TopBarItem
{
    public bool isHomeBar = false;

    protected override void Awake()
    {
        base.Awake();
        numText.text = StorageManager.Inst.GetStorage<StorageUserInfo>().CurStar.ToString();
        EventDispatcher.AddEventListener<int,int,int>(StorageUserInfo.StorageUserInfo_Change_GoodsCount, starChange);
    }
    private void starChange(int goods_id,int oldValue,int newValue)
    {
        if(numText != null && goods_id == Table.GoodsId.Star)
            numText.text = StorageManager.Inst.GetStorage<StorageUserInfo>().CurStar.ToString();
    }

    protected override void onClick()
    {
        if (topBarType == TopBarType.Star)
        {
            UIManager.Inst.ShowUI(UIModuleEnum.UIGetMoreStarPanel, false,0, "popup");
        }
    }
}
