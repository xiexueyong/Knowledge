using EventUtil;
using Framework.Storage;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class CoinTopBarItem : TopBarItem
{
    public bool isHomeBar = false;

    protected override void Awake()
    {
        base.Awake();
        numText.text = DataManager.Inst.userInfo.Coins.ToString();
        EventDispatcher.AddEventListener<int,int,int>(StorageUserInfo.StorageUserInfo_Change_GoodsCount, coinChange);
    }
    private void coinChange(int coin_goods_id,int oldValue,int newValue)
    {
        if(numText != null)
            numText.text = DataManager.Inst.userInfo.Coins.ToString();
    }

    protected override void onClick()
    {
        if (topBarType == TopBarType.Coin)
        {
            if (isHomeBar)
            {
               // uiHome?.homeTab?.SelectTab(0);
            }
            else
            {
                UIManager.Inst.ShowUI(UIModuleEnum.UIShop, false, "popup");
            }
        }
    }
}
