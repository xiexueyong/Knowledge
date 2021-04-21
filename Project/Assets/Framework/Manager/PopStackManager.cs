using System.Collections;
using System.Collections.Generic;
using EventUtil;
using UnityEngine;
using System.Linq;
using System;

//首页自动弹出面板的优先级
public enum PopPriority
{
    NPCTaskClick = 90,
    ContinueWinPanel = 100,
    UIPersonalStarRaceInvitePanel = 200,
    ChestLevelTutorial = 300,//等级宝箱的引导
    TutorialDailyTarget = 310,//每日收集目标的引导
    TutorialDailyWheel = 320,//每日转盘的引导
    UIGainGoodsPanel = 500//引导道具前，弹出道具引导
}
public class PopData
{
    public string UIName;
    public object[] objs;
    public Action<BaseUI> customAction;
    public int popIndex;



    public PopData(string uiName,int popIndex, Action<BaseUI> customAction, object[] objs)
    {
        UIName = uiName;
        this.objs = objs;
        this.popIndex = popIndex;
        this.customAction = customAction;
    }
}

public class PopStackManager : D_MonoSingleton<PopStackManager>
{
  
    public List<PopData> PopStack = new List<PopData>();


    protected override void OnAwake()
    {

        //UIManager.Inst.OnUIShow += OnUIShow;
        //UIManager.Inst.OnUIClose += OnUIClose;
        uisFromLastPop = new List<BaseUI>();
    }


    //先弹出popIndex较消的面板
    public void Push(string uiName,int popIndex, Action<BaseUI> customAction, params object[] objs)
    {
        PopData data = new PopData(uiName, popIndex, customAction,objs);
        if (PopStack.Count == 0)
        {
            PopStack.Add(data);
            return;
        }
        int i = 0;
        while(i < PopStack.Count)
        {
            if(PopStack[i].popIndex > popIndex)
            {
                PopStack.Insert(i,data);
                return;
            }
            i++;
        }

        PopStack.Add(data);
    }
    //加到队首
    //public void EnQueue(string uiName,Action<BaseUI> customAction, params object[] objs)
    //{
    //    PopData data = new PopData(uiName, customAction, objs);
    //    PopStack.Insert(0,data);
    //}
    //打开队首队面板
    private BaseUI CurPopUI;
    private List<BaseUI> uisFromLastPop;
    public void Pop()
    {
        //if (uisFromLastPop.Count > 0)
        //{
        //    return;
        //}

        uisFromLastPop.Clear();
        if (PopStack.Count <= 0)
        {
            return;
        }

        PopData data = PopStack[0];
        PopStack.RemoveAt(0);

        CurPopUI = UIManager.Inst.ShowUI(data.UIName,true, data.objs);
        data.customAction?.Invoke(CurPopUI);
        CurPopUI.OnCloseLister += OnCurPopUIClose;
    }

    private void OnCurPopUIClose()
    {
        CurPopUI.OnCloseLister -= OnCurPopUIClose;
        CurPopUI = null;
        Pop();
    }

    public bool IsEmpty()
    {
        return PopStack == null || PopStack.Count == 0;

    }

    void OnUIShow(BaseUI ui)
    {
        if (!ui.IsPop)
        {
            uisFromLastPop.Add(ui);
        }
    }

    void OnUIClose(BaseUI ui)
    {
        if (!ui.IsPop)
        {
            uisFromLastPop.Remove(ui);
        }
        if (uisFromLastPop.Count <= 0)
        {
            Pop();
        }
    }


    public void Clear()
    {
        if(PopStack!=null)
            PopStack.Clear();
    }
}
