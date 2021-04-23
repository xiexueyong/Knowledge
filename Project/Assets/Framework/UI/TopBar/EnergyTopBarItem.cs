using System;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using EventUtil;
using Framework.Asset;
using Framework.Storage;
using Framework.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class EnergyTopBarItem : TopBarItem
{
    private Image InfiniteImage;
    private Text CuontText;
    private Text TimeText;

    protected override void Awake()
    {
        base.Awake();
        CuontText = this.transform.Find("CuontText").GetComponent<Text>();
        TimeText = this.transform.Find("TimeText").GetComponent<Text>();

        ShowEnergy();
        SystemClock.AddListener(ShowEnergy);
    }

    private void ShowEnergy()
    {
        int energy = DataManager.Inst.userInfo.Energy;
        if (MLifeManager.Inst.InfiniteLife())
        {
            string time = CommonUtil.SecondToTimeFormat(MLifeManager.Inst.RemainInfiniteSecond);
            TimeText.text = string.Format(time);
            CuontText.gameObject.SetActive(false);
            InfiniteImage.gameObject.SetActive(true);
        }
        else
        {
            if (energy < MLife.LifeMaxCount)
            {
                string time = CommonUtil.SecondToTimeFormat(MLifeManager.Inst.RemainRecoverSecond);
                TimeText.text = time;
            }
            else
            {
                TimeText.text = "满";
            }

            // InfiniteImage.gameObject.SetActive(false);
            CuontText.gameObject.SetActive(true);
            CuontText.text = string.Format("{0}/{1}",energy.ToString(),MLife.LifeMaxCount);
        }
    }
    
    protected override IEnumerator FlyCorutinue(Vector3 startPos,Vector3 endPos, Action callback = null)
    {
        yield return null;
        Vector3[] points = { startPos, Utils.GetMiddlePoint(startPos, endPos, 0.2f), endPos };
        GameObject flyCoin = Res.LoadResource<GameObject>("Prefab/Framework/Animation/FlyEnergy");
        flyCoin.transform.SetParent(transform.parent.parent);
        flyCoin.transform.localScale = Vector3.one;
        flyCoin.transform.position = startPos;

        TweenerCore<Vector3, Path, PathOptions> flyTween = flyCoin.transform.DOPath(points, 1f, PathType.CatmullRom).SetEase(Ease.InOutCubic).OnComplete(() =>
        {
            Res.Recycle(flyCoin.gameObject);
            callback?.Invoke();
        });
    }

    protected void onClick()
    {
        if (topBarType == TopBarType.Energy)
        {
            if (DataManager.Inst.userInfo.Energy < MLife.LifeMaxCount)
            {
                //打开LifePanel
                UIManager.Inst.ShowUI(UIName.GameLifeSimplePanel);
            }
            else
            {
                UIManager.Inst.ShowMessage(LanguageTool.Get("full_lives"));
            }

        }

    }

    private void OnDestroy()
    {
        SystemClock.RemoveListener(ShowEnergy);
    }

}
