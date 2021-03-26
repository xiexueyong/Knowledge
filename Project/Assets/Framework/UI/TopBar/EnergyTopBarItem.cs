using EventUtil;
using Framework.Storage;
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
