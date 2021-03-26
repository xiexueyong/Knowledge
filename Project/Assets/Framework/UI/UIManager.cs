using System;
using System.Collections.Generic;
using System.Linq;
using EventUtil;
using UnityEngine;
using UnityEngine.UI;
using Framework.Asset;
using Framework.Utils;

public enum UILayer
{
    COMMON = 0,
    TIPS = 1,//弹出框
    Transition = 2, //地图层（气泡、雪条、倒计时等）
  
}
public enum HideType
{
    DESTROY,
    HIDE
}

public partial class UIManager : S_MonoSingleton<UIManager>
{
    public List<BaseUI> UIList = new List<BaseUI>();
    public List<BaseUI> UIStack = new List<BaseUI>();

    public Camera UICamera;
    public Canvas UICanvas;

    [NonSerialized]public int chainHeight = 0;
    [SerializeField] public Transform BgMask;
    [SerializeField] public Transform StartGameImage;

    [SerializeField] private Transform CommonLayer;
    [SerializeField] private Transform TipLayer;
    [SerializeField] private Transform TransitionLayer;
    private GameObject WaitingObj;

    public Action<BaseUI> OnUIShow;
    public Action<BaseUI> OnUIClose;

    protected override void OnAwake()
    {
        base.OnAwake();
        WaitingObj = transform.Find("Canvas/Waiting").gameObject;
        BgMask.GetComponent<Button>().onClick.AddListener(OnCloseBgMask);
        if (Utils.IsChinScreen())
        {
            chainHeight = -55;
        }
        else
        {
            chainHeight = 0;
        }

        var rectTrans1 = BgMask.GetComponent<RectTransform>();
        rectTrans1.offsetMax = new Vector2(0, chainHeight);

        var rectTrans3 = CommonLayer.GetComponent<RectTransform>();
        rectTrans3.offsetMax = new Vector2(0, chainHeight);

        var rectTrans4 = TipLayer.GetComponent<RectTransform>();
        rectTrans4.offsetMax = new Vector2(0, chainHeight);

        //TransitionLayer充满全屏，不需要下移
        //var rectTrans5 = TransitionLayer.GetComponent<RectTransform>();
        //rectTrans5.offsetMax = new Vector2(0, chainHeight);
    }
    private void OnCloseBgMask()
    {
        if (curUI != null && curUI.IsActive && curUI.CanCloseByBgMask)
        {
            CloseUI(curUI);
        }
    }





    private BaseUI curUI;
    public BaseUI CurUI
    {
        get
        {
            return curUI;
        }
    }

    public BaseUI ShowUI(string prefabName,bool isPop = false, params object[] objs)
    {
        BaseUI baseUI = createUI(prefabName, objs);
        if (baseUI == null)
            return null;

        if (!baseUI.IsActive)
        {
            baseUI.internal_Active();
        }
        baseUI.IsPop = isPop;

        if (curUI != null && curUI.IsActive)
        {
            curUI.internal_Pause();
            curUI.OnPause();
        }
       

        Transform _uiParent = CommonLayer;
        switch (baseUI.Layer)
        {
            case UILayer.COMMON:
                _uiParent = CommonLayer;
                break;
            case UILayer.TIPS:
                _uiParent = TipLayer;
                break;
            case UILayer.Transition:
                _uiParent = TransitionLayer;
                break;
        }

        baseUI.transform.SetParent(_uiParent,false);
        baseUI.transform.position = Vector3.zero;
        (baseUI.transform as RectTransform).offsetMin = Vector2.zero;
        (baseUI.transform as RectTransform).offsetMax = Vector2.zero;
        baseUI.transform.localScale = Vector3.one;
        baseUI.transform.localRotation = Quaternion.identity;
        baseUI.transform.SetAsLastSibling();
        setBgMask(baseUI);
        addToUIStack(baseUI);

        curUI = baseUI;
        curUI.internal_PlayAnimation(()=> { ActualOpenUI(baseUI, null); }, true);
        OnUIShow?.Invoke(curUI);
        return baseUI;
    }
    //设置面板的黑色遮罩
    private void setBgMask(BaseUI baseUI)
    {
        if (baseUI.HasBgMask && baseUI.IsActive)
        {
            BgMask.transform.SetParent(baseUI.transform.parent);
            BgMask.gameObject.SetActive(true);
            (BgMask.transform as RectTransform).offsetMin = Vector2.zero;
            (BgMask.transform as RectTransform).offsetMax = new Vector2(0,-chainHeight);
            BgMask.localScale = Vector3.one;
            BgMask.localRotation = Quaternion.identity;

            int uiIndex = baseUI.transform.GetSiblingIndex();
            int maskIndex = BgMask.transform.GetSiblingIndex();
            if (uiIndex > maskIndex)
            {
                BgMask.transform.SetSiblingIndex(uiIndex-1);
            }
            else
            {
                BgMask.transform.SetSiblingIndex(uiIndex);
            }

        }
    }
    //添加进UI队列
    private void addToUIStack(BaseUI baseUI)
    {
        if (UIStack.Contains(baseUI))
        {
            UIStack.Remove(baseUI);
        }
        UIStack.Add(baseUI);
    }
    //移出UI队列
    private void removeFromUIStack(BaseUI baseUI)
    {
        if (UIStack.Contains(baseUI))
        {
            UIStack.Remove(baseUI);
        }
    }
    //关闭面板时恢复前一个面板
    private void ResumePreUI()
    {
        if (UIStack.Count > 0 && curUI == null)
        {
            BaseUI ui = UIStack.ElementAt(UIStack.Count - 1);
            curUI = ui;
            ui.internal_Active();
            setBgMask(ui);
        }
    }
    public void CloseUI(string prefabName,Action callback = null, bool immediately = false)
    {
        BaseUI baseUI = GetUI(prefabName);
        CloseUI(baseUI, callback, immediately);
    }
    public void CloseUI(BaseUI baseUI, Action callback = null, bool immediately = false)
    {
        if (baseUI != null && !baseUI.IsCloseing)
        {
            baseUI.internal_BeginClosing();
            if (immediately)
                ActualCloseUI(baseUI, callback);
            else
                baseUI.internal_PlayAnimation(()=> ActualCloseUI(baseUI,callback),false);
        }
    }
    private void ActualOpenUI(BaseUI baseUI, Action callback = null)
    {
        baseUI.internal_EndOpening();
    }

    private void ActualCloseUI(BaseUI baseUI, Action callback = null)
    {
        if (curUI == baseUI)
        {
            BgMask.gameObject.SetActive(false);
            curUI = null;
        }
        if (baseUI.IsActive)
        {
            baseUI.internal_Pause();
            baseUI.OnPause();
        }
        baseUI.internal_EndClosing();
        baseUI.OnClose();
        UIList.Remove(baseUI);
        removeFromUIStack(baseUI);
        OnUIClose?.Invoke(curUI);
        Res.Recycle(baseUI.gameObject);
        callback?.Invoke();
        ResumePreUI();
    }

    public void Clear(int keepCount = 0)
    {
        while (UIList.Count > keepCount)
        {
            CloseUI(UIList[UIList.Count - 1], null, true);
        }
    }

    private BaseUI GetUI(string prefabName)
    {
        BaseUI baseUI = UIList.Find(x => x.m_PrefabName == prefabName);
        return baseUI;
    }

    private BaseUI createUI(string prefabName, params object[] objs)
    {
        BaseUI baseUI = GetUI(prefabName);

        if (baseUI == null || baseUI.Multiple)
        {
            baseUI = Res.LoadResource<GameObject>(prefabName).GetComponent<BaseUI>();
            //baseUI.gameObject.name = prefabName;
            baseUI.m_PrefabName = prefabName;
            UIList.Add(baseUI);
        }
        baseUI.SetData(objs);
        return baseUI;
    }

    //return true 表示阻止事件继续往下传播
    public bool Back()
    {
        if (curUI != null && curUI.IsActive)
        {
            curUI.OnBack();
            return true;
        }
        return false;
    }





    int showWaitingCount = 0;
    public void showWaiting()
    {
        showWaitingCount = showWaitingCount > 0 ? showWaitingCount : 0;
        showWaitingCount++;
        WaitingObj.SetActive(true);
    }

    public void hideWaiting()
    {
        showWaitingCount--;
        if (showWaitingCount > 0)
        {
            WaitingObj.SetActive(true);
        }
        else
        {
            WaitingObj.SetActive(false);
        }
    }










    #region TipMessageView

    private TipMessage message;
    private TipMessage customMessage;

    public void ShowMessage(string str)
    {
        if (message == null) {
            GameObject go = Res.LoadResource<GameObject>("Prefab/Framework/DefaultTipMessage");
            go.transform.SetParent(TipLayer);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = Vector3.one;
            message = go.GetComponent<TipMessage>();
        }

        message.Reset();
        message.StartAnim(OnTipMessageFinish);
        message.setTip(str);
    }

    private void OnTipMessageFinish()
    {
        if (message != null) {
            Res.Recycle(message.gameObject);
            message.Reset();
            message = null;
        }
    }

    private void OnCustomTipMessageFinish()
    {
        if (customMessage != null) {
            Res.Recycle(customMessage.gameObject);
            customMessage.Reset();
            customMessage = null;
        }
    }

    public void ShowCustomMessage(string prefabName, string str, Transform anchor, Vector3 offset)
    {
        if (customMessage == null) {
            GameObject go = Res.LoadResource<GameObject>(prefabName);
            go.transform.SetParent(TipLayer);
            go.transform.localScale = Vector3.one;
            customMessage = go.GetComponent<TipMessage>();
        }

        customMessage.Reset();
        customMessage.transform.position = anchor.position;
        customMessage.transform.localPosition += offset;
        customMessage.setTip(str);
        customMessage.StartAnim(OnCustomTipMessageFinish);
    }

    #endregion

}