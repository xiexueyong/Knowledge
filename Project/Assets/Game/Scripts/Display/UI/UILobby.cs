
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Runtime.InteropServices.ComTypes;

public class UILobby : BaseUI
{
    [SerializeField]private DegreeComponent _degreeComponent;
    private Button play_btn;
    private Text level_txt;

    public override void OnAwake()
    {
        play_btn = transform.Find("root/play_btn").GetComponent<Button>();
        level_txt = play_btn.transform.Find("Text").GetComponent<Text>();
        play_btn.onClick.AddListener(OnPlayClick);
    }

    void OnPlayClick()
    {
        SoundPlay.PlaySFX(Table.Sound.click);
        if (DataManager.Inst.userInfo.Level > Table.GameConst.levelMax)
        {
            UIManager.Inst.ShowMessage("敬请期待更新");
        }
        else
        {
            UIManager.Inst.ShowUI(UIName.UIQuestion,false,DataManager.Inst.userInfo.Level);    
        }
        Close();
    }
    public override void SetData(params object[] args)
    {
        base.SetData(args);
    }

    public override void OnStart()
    {
        if (!PopStackManager.Inst.IsEmpty())
        {
            return;
        }

        level_txt.text = "知识之旅";//DataManager.Inst.userInfo.Level.ToString();

        int lb;
        var ld = LevelHelper.getDegree(DataManager.Inst.userInfo.Level,out lb);
        _degreeComponent.SetDegree(ld,lb);
        _degreeComponent.SetLevel(DataManager.Inst.userInfo.Level-1);

        ActicityRefresh();
    }

    void ActicityRefresh()
    {
        ActicityDailyWheel.Inst?.Refresh();
    }


}
