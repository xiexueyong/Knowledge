
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using EnhancedScrollerDemos.CellEvents;

public class UIGetRewardPanel : BaseUI
{
    public Button btn_confirm;
    public Text txt_coin_num;
    private int coin_num;
    
    public override void OnAwake()
    {
        btn_confirm.onClick.AddListener(onConfirmClick);
        
    }
    public override void OnClose()
    {
    }

    void onConfirmClick()
    {
        DataManager.Inst.userInfo.ChangeGoodsCount(Table.GoodsId.Coin,coin_num);
        Close();
    }
    
    public override void SetData(params object[] args)
    {
        base.SetData(args);
        coin_num = (int)args[0];
        txt_coin_num.text = coin_num.ToString();
    }


}
