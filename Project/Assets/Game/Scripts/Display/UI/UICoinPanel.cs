﻿
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using EnhancedScrollerDemos.CellEvents;

public class UICoinPanel : BaseUI
{
    public Button btn_ad;
    public Text txt_coin_count;

    public override void OnAwake()
    {
        btn_ad.onClick.AddListener(onBtnAdClick);
        
    }
    void onBtnAdClick()
    {
        DataManager.Inst.userInfo.ChangeGoodsCount(Table.GoodsId.Coin,200);
        txt_coin_count.text = DataManager.Inst.userInfo.Coins.ToString();
    }

    public override void SetData(params object[] args)
    {
        base.SetData(args);
        txt_coin_count.text = DataManager.Inst.userInfo.Coins.ToString();
    }


}