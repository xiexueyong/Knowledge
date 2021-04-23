
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using EnhancedScrollerDemos.CellEvents;

public class UIEnergyPanel : BaseUI
{
    public Button btn_ad;
    public Button btn_coin;
    public Text txt_cd;
    public Text txt_energy_count;

    public override void OnAwake()
    {
        btn_ad.onClick.AddListener(onBtnAdClick);
        btn_coin.onClick.AddListener(onBtnCoinClick);
        
    }
    public override void OnStart()
    {
        SystemClock.AddListener(OnSecond); 
        RefreshData();
    }
    public override void OnClose()
    {
        SystemClock.RemoveListener(OnSecond); 
    }

    void onBtnAdClick()
    {
        TopBarManager.Inst.FlyToBar(TopBarType.Energy,btn_ad.transform.position,
            () =>
            {
                DataManager.Inst.userInfo.ChangeEnergy(MLife.LifeMaxCount);
                RefreshData();        
            }
        );
        
    }
    void onBtnCoinClick()
    {
        TopBarManager.Inst.FlyToBar(TopBarType.Energy,btn_coin.transform.position,
            () =>
            {
                DataManager.Inst.userInfo.ChangeEnergy(MLife.LifeMaxCount);
                RefreshData();        
            }
        );
    }

    void OnSecond()
    {
        if (!MLifeManager.Inst.LifeFull())
        {
            RefreshData();    
        }
        
    }

    void RefreshData()
    {
        txt_energy_count.text = DataManager.Inst.userInfo.Energy.ToString();
        txt_cd.text = CommonUtil.SecondToTimeFormat(MLifeManager.Inst.RemainInfiniteSecond);
    }
    public override void SetData(params object[] args)
    {
        base.SetData(args);
    }


}
