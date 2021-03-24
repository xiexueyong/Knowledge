/// <summary>
/// 生命周期 OnAwak、OnStart、OnPause、OnClose
/// 从Res缓存再次创建：OnStart、OnPause、OnClose
/// 被其他面板覆盖后又通过Back恢复：OnPause、OnStart
/// 关闭：OnPause、OnClose
/// </summary>
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using UnityEngine.UI;

[Flags]
public enum TopBarType
{
    None = 0,
    Coin = 1 << 0,
    Energy = 1 << 1,
    Star = 1 << 2,
    Bg = 1 << 3,//是否显示Topbar的背景
    Setting = 1 << 5,
    All = Coin | Setting | Energy | Star | Bg
}

public class BaseUI : MonoBehaviour
{
    [HideInInspector]
    public bool IsPop = false;

    [TabGroup("UIDetail")]
    public bool Multiple;//多个同类型面板共存
    [TabGroup("UIDetail")]
    public bool HasBgMask = true;//是否又黑色背景遮罩
    [TabGroup("UIDetail")]
    public bool CanCloseByBgMask = true;//点击黑色背景遮罩可以关闭
    [TabGroup("UIDetail"), EnumToggleButtons, GUIColor(1, 1, 0)]
    public UILayer Layer = UILayer.COMMON; //所在层级
    [TabGroup("UIDetail"), EnumToggleButtons, GUIColor(1, 1, 0)]
    public HideType HideType = HideType.DESTROY; //隐藏方式


    [TabGroup("TopBar"), GUIColor(0, 1, 0)]
    public bool IgnoreControlTopBar = false;
    [TabGroup("TopBar"), EnumToggleButtons, GUIColor(1, 1, 0)]
    public TopBarType m_TopBarType;

    [TabGroup("Animatioin"), HideLabel, GUIColor(0.12f, 1, 1)]
    [SerializeField]
    public UIAnimatioinDetail m_AnimationDetail;

    protected Button CloseButton;



    private UIStatus Status;
    [HideInInspector] 
    public string m_PrefabName;
    [HideInInspector]
    public Transform m_root;

    public Action OnCloseLister;
    public Action OnOpenLister;


    //已经执行过Start函数（生命周期）
    [HideInInspector]
    public bool _start;

    private enum UIStatus
    {
        Pause = 0,
        Active = 1
    }
    public bool IsCloseing { get; private set; }


    #region 屏蔽系统的三个函数
    private void Awake()
    {
        //请override OnAwake,不要使用Awake
        m_root = transform.Find("root");
        OnAwake();
    }
    /// <summary>
    /// 被OnStart代替
    /// </summary>
    private void Start()
    {
        //请override OnStart,不要使用Start
        OnStart();
        _start = true;
    }
    /// <summary>
    /// 被OnClose代替
    /// </summary>
    private void OnDestroy()
    {
        //请override OnClose,不要使用OnDestroy
    }
    #endregion

    public virtual void SetData(params object[] args)
    {
    }

    public virtual void OnAwake()
    {
    }
    /// <summary>
    ///相当与start函数，解决被Res缓存后再次使用时Start函数不能被调用的问题
    /// </summary>
    public virtual void OnStart()
    {

    }

    /// <summary>
    ///关闭前、被其他覆盖时会被调
    /// </summary>
    public virtual void OnPause()
    {
    }
 
    /// <summary>
    /// 被关闭的回调函数，先调OnPause,再调OnClose
    /// </summary>
    public virtual void OnClose()
    {
    }

    /// <summary>
    /// android用户点击返回键，调用OnEscape
    /// </summary>
    public virtual void OnEscape()
    {
        Close();
    }

    public void Update()
    {

    }
    //通过UIManager关闭自己
    public void Close(bool immediately = false)
    {
        UIManager.Inst.CloseUI(this);
    }

    public bool IsActive
    {
        get { return Status == UIStatus.Active; }
    }
    #region Internal函数，开发者不可调用
    //只能由UIManager系统调用，开发者不能调用
    public void internal_Active()
    {
        Status = UIStatus.Active;
        if(!IgnoreControlTopBar)
            TopBarManager.Inst.ShowTopBar(m_TopBarType);
        if(_start)
            OnStart();
    }
    //只能由UIManager系统调用，开发者不能调用
    public void internal_Pause()
    {
        Status = UIStatus.Pause;
    }

    public void internal_EndOpening()
    {
        OnOpenLister?.Invoke();
        OnOpenLister = null;
    }
    public void internal_BeginClosing()
    {
        IsCloseing = true;
    }
    public void internal_EndClosing()
    {
        IsCloseing = false;
        OnCloseLister?.Invoke();
        OnCloseLister = null;
    }


    /// <summary>
    /// 进入、退出界面播放动画
    /// </summary>
    public virtual void internal_PlayAnimation(Action callBack, bool isEnter)
    {
        if (m_root != null)
        {
            UIAnimationControl.Inst.PlayAnimation(this, callBack, isEnter);
        }
        else
        {
            callBack?.Invoke();
        }
    }
    #endregion
}