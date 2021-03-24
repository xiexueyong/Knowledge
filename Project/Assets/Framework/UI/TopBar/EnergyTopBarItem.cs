using EventUtil;
using Framework.Storage;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class EnergyTopBarItem : TopBarItem
{
    private Image InfiniteImage;
    private Text EnergyCountText;
    private Text TimeText;

    protected override void Awake()
    {
        base.Awake();
        EnergyCountText = this.transform.Find("EnergyCountText").GetComponent<Text>();
        TimeText = this.transform.Find("TimeText").GetComponent<Text>();
        InfiniteImage = this.transform.Find("InfiniteImage").GetComponent<Image>();

        ShowEnergy();
        //EventManager.Subscribe(EventManagerType.PassOneSecond, ShowEnergy);
        SystemClock.AddListener(ShowEnergy);
    }

    private void ShowEnergy()
    {
        int energy = DataManager.Inst.userInfo.Energy;
        if (MLifeManager.Inst.InfiniteLife())
        {
            string time = CommonUtil.SecondToTimeFormat(MLifeManager.Inst.RemainInfiniteSecond);
            TimeText.text = string.Format(time);
            EnergyCountText.gameObject.SetActive(false);
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
                TimeText.text = LanguageTool.Get("full");
            }

            InfiniteImage.gameObject.SetActive(false);
            EnergyCountText.gameObject.SetActive(true);
            EnergyCountText.text = energy.ToString();
        }
    }

    protected override void onClick()
    {
        if (topBarType == TopBarType.Energy)
        {
            if (DataManager.Inst.userInfo.Energy < MLife.LifeMaxCount)
            {
                //打开LifePanel
                UIManager.Inst.ShowUI(UIModuleEnum.GameLifeSimplePanel);
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
