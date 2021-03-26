
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;
using System.Runtime.InteropServices.ComTypes;

public class UILobby : BaseUI
{
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
        UIManager.Inst.ShowUI(UIName.UIQuestion,false,DataManager.Inst.userInfo.Level);
    }
    public override void SetData(params object[] args)
    {
        base.SetData(args);
    }

    public override void OnStart()
    {
        level_txt.text = DataManager.Inst.userInfo.Level.ToString();
    }


}
