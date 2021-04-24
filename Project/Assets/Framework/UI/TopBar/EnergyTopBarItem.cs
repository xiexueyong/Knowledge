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
        string path = "Prefab/Framework/Animation/FlyEnergy";
        float flyTime = 1f;
        float disappearTime = 1f;
        
        Vector3[] points = { startPos, Utils.GetMiddlePoint(startPos, endPos, 0.2f), endPos };
        GameObject flyCoin = Res.LoadResource<GameObject>(path);
        //显示图标
        flyCoin.transform.GetComponent<Image>().enabled = true;
        
        flyCoin.transform.SetParent(transform.parent.parent);
        flyCoin.transform.localScale = Vector3.one;
        flyCoin.transform.position = startPos;

       
        flyCoin.transform.DOPath(points, flyTime, PathType.CatmullRom).SetEase(Ease.InOutCubic);
        yield return new WaitForSeconds(flyTime);
        //隐藏图标
        flyCoin.transform.GetComponent<Image>().enabled = false;
        callback?.Invoke();
        yield return new WaitForSeconds(disappearTime);
        Res.Recycle(flyCoin.gameObject);
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
