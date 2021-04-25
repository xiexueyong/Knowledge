
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using EnhancedScrollerDemos.CellEvents;

public class UICoinPanel : BaseUI
{
    public Button btn_ad;
    public Button btn_close;
    public Text txt_coin_count;

    public override void OnAwake()
    {
        btn_ad.onClick.AddListener(onBtnAdClick);
        btn_close.onClick.AddListener(() => { Close();});
    }
    void onBtnAdClick()
    {
        TopBarManager.Inst.FlyToBar(TopBarType.Coin,btn_ad.transform.position,
            () =>
            {
                DataManager.Inst.userInfo.ChangeGoodsCount(Table.GoodsId.Coin,200);        
            }
        );
        
        txt_coin_count.text = DataManager.Inst.userInfo.Coins.ToString();
    }

    public override void SetData(params object[] args)
    {
        base.SetData(args);
        txt_coin_count.text = "+200";
    }


}
