//using System;
//using System.Collections.Generic;
//using System.Linq;
//using EventUtil;
//using UnityEngine;
//using UnityEngine.UI;
//using Framework.Asset;

///// <summary>
///// UI管理器
///// </summary>
//public class UIManager_Old : S_MonoSingleton<UIManager_Old>
//{
//    /// <summary>
//    /// other 层的索引列表
//    /// </summary>
//    [HideInInspector] public List<BaseUI> UIModulesList = new List<BaseUI>();

//    /// <summary>
//    /// 界面栈
//    /// </summary>
//    [HideInInspector] public List<string> UIStack = new List<string>();

//    /// <summary>
//    /// 忽略进入堆栈UI
//    /// </summary>
//    private List<string> IgnorePushUIName = new List<string>()
//    {
//        UIModuleEnum.UIMainGameView,
//        UIModuleEnum.CommonPlay,
//        UIModuleEnum.DailyPuzzlePlay
//    };

//    private int MaskCount = 0;
//    private int NetMaskCount = 0;
//    private List<string> MaskKeyList = new List<string>();

//    //事件遮罩
//    [SerializeField] private GameObject MaskEventPanel;
//    [SerializeField] private GameObject NetMaskEventPanel;

//    [SerializeField] private UIMaskPanelControl MaskPanelControl;

//    [SerializeField] private Transform MapLayer;
//    [SerializeField] private Transform CommonLayer;
//    [SerializeField] private Transform TipLayer;

//    public Camera UICamera;

//    [SerializeField] private CanvasScaler m_CanvasScaler;
//    [SerializeField] private Canvas m_Canvas;

//    //层级对应的父对象
//    private Dictionary<UILayer, Transform> UILayerParent = new Dictionary<UILayer, Transform>();


//    /// <summary>
//    /// 初始化操作
//    /// </summary>
//    protected override void OnAwake()
//    {
//        UILayerParent = new Dictionary<UILayer, Transform>()
//        {
//            {UILayer.MAP, MapLayer},
//            {UILayer.COMMON, CommonLayer},
//            {UILayer.TIPS, TipLayer}
//        };
//    }

//    public T ShowUI<T>(string prefabName, string prevPath, params object[] objs) where T : BaseUI
//    {
//        BaseUI baseUI = CreateUI<BaseUI>(prefabName, prevPath);
//        ShowBegin(baseUI, objs);
//        return baseUI as T;
//    }


//    public BaseUI ShowUI(string prefabName, params object[] objs)
//    {
//        BaseUI baseUI = CreateUI<BaseUI>(prefabName);
//        baseUI.m_bPushFlag = false;
//        //显示UI
//        ShowBegin(baseUI, objs);
//        return baseUI;
//    }

//    public BaseUI PushUI(string prefabName, params object[] objs)
//    {
//        BaseUI baseUI = CreateUI<BaseUI>(prefabName);
//        baseUI.m_bPushFlag = true;
//        //显示UI
//        ShowBegin(baseUI, objs);
//        return baseUI;
//    }

//    public T ShowUI<T>(string prefabName, params object[] objs) where T : BaseUI
//    {
//        BaseUI baseUI = CreateUI<BaseUI>(prefabName);
//        ShowBegin(baseUI, objs);
//        return baseUI as T;
//    }

//    /// <summary>
//    /// 开始显示
//    /// </summary>
//    /// <param name="prefabName"></param>
//    /// <param name="objs"></param>
//    void ShowBegin(BaseUI baseUI, params object[] objs)
//    {
//        if (null == baseUI) {
//            return;
//        }

//        baseUI.transform.SetAsLastSibling();
//        baseUI.m_CurrentStage = UILifeCircle.InitData;
//        baseUI.InitData(objs);
//        baseUI.gameObject.SetActive(true);
//        baseUI.m_CurrentStage = UILifeCircle.ShowBegin;
//        try {
//            baseUI.OnShowBegin();
//        }
//        catch (Exception e) {
//            Debug.LogError(string.Format("=========OnShowBegin Begin========= ({0})",baseUI.m_PrefabName));
//            Debug.LogError("Exception==>> "+e);
//            Debug.LogError("=========OnShowBegin End=========:");
//        }
//        finally {
//            EventDispatcher.TriggerEvent(EventKey.UIShowBegine, baseUI);
//            PushUI(baseUI);
//            baseUI.PlayAnimation(() => { ShowEnd(baseUI); });
//            MaskPanelControl.UpdateMaskBg(baseUI);
//        }
//    }


//    /// <summary>
//    /// 显示结束
//    /// </summary>
//    /// <param name="prefabName"></param>
//    void ShowEnd(BaseUI baseUI)
//    {
//        if (baseUI.m_CurrentStage < UILifeCircle.ShowEnd) {
//            baseUI.m_CurrentStage = UILifeCircle.ShowEnd;
            
//            try {
//                baseUI.OnShowEnd();
//            }
//            catch (Exception e) {
//                Debug.LogError(string.Format("=========OnShowEnd Begin========= ({0})",baseUI.m_PrefabName));
//                Debug.LogError("Exception==>> "+e);
//                Debug.LogError("=========OnShowEnd End=========:");
//            }
//            finally {
//                EventDispatcher.TriggerEvent(EventKey.UIShowEnd, baseUI);
//            }
//        }
//    }

//    /// <summary>
//    /// 隐藏面板
//    /// </summary>
//    /// <param name="prefabName"></param>
//    /// <param name="immediately"></param>
//    public void HideUI(string prefabName, bool immediately = false)
//    {
//        BaseUI baseUI = UIModulesList.Find(innerUI => innerUI.m_PrefabName == prefabName);
//        HideBegin(baseUI, immediately);
//    }

//    /// <summary>
//    /// 隐藏面板
//    /// </summary>
//    /// <param name="prefabName"></param>
//    /// <param name="immediately"></param>
//    public void HideUI(BaseUI baseUI, bool immediately = false)
//    {
//        HideBegin(baseUI, immediately);
//    }

//    /// <summary>
//    /// 开始关闭
//    /// </summary>
//    /// <param name="prefabName"></param>
//    /// <param name="immediately"></param>
//    void HideBegin(BaseUI baseUI, bool immediately = false)
//    {
//        if (null == baseUI) {
//            return;
//        }

//        if (baseUI.m_CurrentStage < UILifeCircle.HideBegin) {
//            baseUI.m_CurrentStage = UILifeCircle.HideBegin;
//            EventDispatcher.TriggerEvent(EventKey.UIHideBegine, baseUI);
            
//            try {
//                baseUI.OnHideBegin();
//            }
//            catch (Exception e) {
//                Debug.LogError(string.Format("=========OnHideBegin Begin========= ({0})",baseUI.m_PrefabName));
//                Debug.LogError("Exception==>> "+e);
//                Debug.LogError("=========OnHideBegin End=========:");
//            }
            
            
//            if (immediately) {
//                HideEnd(baseUI);
//            }
//            else {
//                baseUI.PlayAnimation(() => { HideEnd(baseUI); }, false);
//            }
//        }
//    }

//    /// <summary>
//    /// 关闭结束
//    /// </summary>
//    /// <param name="baseUI"></param>
//    void HideEnd(BaseUI baseUI)
//    {
//        if (null == baseUI) {
//            return;
//        }

//        string prefabName = UIStack.Find(prefabname => prefabname == baseUI.m_PrefabName);
//        if (!string.IsNullOrEmpty(prefabName)) {
//            //最后一个
//            bool isResume = UIStack.LastOrDefault() == prefabName;
//            UIStack.Remove(UIStack.LastOrDefault());
//            //往后显示UI
//            if (isResume) {
//                if (baseUI.m_bPushFlag) {
//                    for (int i = UIStack.Count - 1; i >= 0; i--) {
//                        BaseUI tempUI = GetUI(UIStack[i]);
//                        if (tempUI) {
//                            tempUI.gameObject.SetActive(true);
////                            tempUI.PlayAnimation(null);
//                            tempUI.OnResume();
//                            if (tempUI.m_bPushFlag) {
//                                break;
//                            }

//                            tempUI.m_bPushFlag = false;
//                        }
//                    }
//                }
//                else {
//                    BaseUI tempUI = GetUI(UIStack.LastOrDefault());
//                    if (UIStack.Count > 0 && tempUI) {
//                        tempUI.gameObject.SetActive(true);
////                        tempUI.PlayAnimation(null);
//                        tempUI.OnResume();
//                        tempUI.m_bPushFlag = false;
//                    }
//                }
//            }
//        }


//        if (baseUI.m_CurrentStage < UILifeCircle.HideEnd) {
//            baseUI.m_CurrentStage = UILifeCircle.HideEnd;
            
//            try {
//                baseUI.OnHideEnd();
//            }
//            catch (Exception e) {
//                Debug.LogError(string.Format("=========OnHideEnd Begin========= ({0})",baseUI.m_PrefabName));
//                Debug.LogError("Exception==>> "+e);
//                Debug.LogError("=========OnHideEnd End=========:");
//            }
//            finally {
//                EventDispatcher.TriggerEvent(EventKey.UIHideEnd, baseUI);
//            }
//        }

//        baseUI.gameObject.SetActive(false);
//        if (IsDestroy(baseUI)) {
//            DestroyUI(baseUI);
//        }
//        else {
//            DealHideUI(baseUI);
//        }
//    }


//    /// <summary>
//    /// 处理隐藏界面
//    /// </summary>
//    /// <param name="baseUI"></param>
//    private void DealHideUI(BaseUI baseUI, bool forceDestroy = false)
//    {
//        if (null == baseUI || !UIModulesList.Contains(baseUI)) {
////            Debug.LogError(string.Format("{0} UI is not exsit", baseUI.m_PrefabName));
//            return;
//        }

//        baseUI.gameObject.SetActive(false);
//        if (forceDestroy) {
//            //移除UI
//            Res.Recycle(baseUI.gameObject);
//            UIModulesList.Remove(baseUI);
//        }

//        MaskPanelControl.UpdateMaskBg(baseUI);
//    }

//    private T CreateUI<T>(string prefabName, string prevPath = null) where T : BaseUI
//    {
//        BaseUI baseUI = GetUI(prefabName);
//        if (baseUI) {
//            if (baseUI.m_UIDetail.m_EnableMuti) {
//                baseUI = CreateUIModule(prefabName, prevPath);
//            }
//            else {
//                if(baseUI.m_UIDetail.HideType == HideType.DESTROY) {
//                    if (IsUIVisible(baseUI)) {
//                        HideUI(baseUI, true);
//                    }
//                    baseUI = CreateUIModule(prefabName, prevPath);
//                }
//            }
//        }
//        else {
//            baseUI = CreateUIModule(prefabName, prevPath);
//        }

//        return baseUI as T;
//    }

//    private BaseUI CreateUIModule(string prefabName,string prevPath = null)
//    {
//        GameObject go = null;
//        if (string.IsNullOrEmpty(prevPath))
//        {
//            go = Res.LoadResource<GameObject>(Const.UIPanelPath + prefabName);
//        }
//        else
//        {
//            go = Res.LoadResource<GameObject>(prevPath + prefabName);
//        }
//        RectTransform rt = go.GetComponent<RectTransform>();
//        Vector2 offsetMin = rt.offsetMin;
//        Vector2 offsetMax = rt.offsetMax;
//        BaseUI baseUI = go.GetComponent<BaseUI>();
//        go.transform.SetParent(GetLayerParent(baseUI), false);
//        rt.offsetMin = offsetMin;
//        rt.offsetMax = offsetMax;
//        go.transform.localScale = Vector3.one;
//        baseUI.m_PrefabName = prefabName;
//        if (null != baseUI) {
//            UIModulesList.Add(baseUI);
//        }

//        return baseUI;
//    }

//    public BaseUI GetUI(string prefabName)
//    {
//        BaseUI baseUI = UIModulesList.Find(innerUI => innerUI.m_PrefabName == prefabName);
//        return baseUI;
//    }

//    public T GetUI<T>(string prefabName, bool forceCreate = false) where T : BaseUI
//    {
//        BaseUI baseUI = GetUI(prefabName);
//        if (baseUI && forceCreate) {
//            T com = CreateUI<T>(prefabName);
//            com.gameObject.SetActive(false);
//            return com;
//        }
//        else {
//            return GetUI(prefabName) as T;
//        }
//    }

//    public bool IsInstantiateUI(string prefabName)
//    {
//        return null != GetUI(prefabName);
//    }

//    public void HideAllUI(bool immediately = true)
//    {
//        for (int i = 0; i < UIModulesList.Count; i++) {
//            var baseUI = UIModulesList[i];
//            HideUI(baseUI, immediately);
//        }

//        EventDispatcher.TriggerEvent(EventKey.HideAllUI);
//    }

//    /// <summary>
//    /// 销毁UI
//    /// </summary>
//    /// <param name="baseUI"></param>
//    void DestroyUI(BaseUI baseUI)
//    {
//        if (IsUIVisible(baseUI)) {
//            baseUI.OnHideBegin();
//            baseUI.OnHideEnd();
//        }

//        DealHideUI(baseUI, true);
//    }

//    public void DestroyAllUI()
//    {
//        for (int i = 0; i < UIModulesList.Count; i++) {
//            var item = UIModulesList[i];
//            if (IsUIVisible(item)) {
//                item.OnHideBegin();
//                item.OnHideEnd();
//            }

//            Res.Recycle(item.gameObject);
//        }

//        EventDispatcher.TriggerEvent(EventKey.DestroyAllUI);

//        UIModulesList.Clear();
//        UIStack.Clear();
//        MaskPanelControl.UpdateMaskBg();
//    }


//    /// <summary>
//    /// UI 出栈
//    /// </summary>
//    public void Back()
//    {
//        //隐藏当前界面
//        if (UIStack.Count > 0) {
//            BaseUI lastUI = GetUI(UIStack.LastOrDefault());
//            //如果当前界面正在显示，立刻隐藏
//            if (IsUIVisible(lastUI)) {
//                lastUI.Hide();
//            }
//            else {
//                UIStack.Remove(lastUI.m_PrefabName);
//            }
//        }

//        //显示上一个界面
//        if (UIStack.Count > 0) {
//            BaseUI lastUI = GetUI(UIStack.LastOrDefault());
//            if (lastUI) {
//                lastUI.gameObject.SetActive(true);
//                lastUI.OnResume();
//            }
//        }
//    }

//    /// <summary>
//    /// UI 入栈
//    /// </summary>
//    void PushUI(BaseUI baseUI)
//    {
//        if (IgnorePushUIName.Contains(baseUI.m_PrefabName) || !IsBelongCommonLayout(baseUI)) {
//            return;
//        }

//        PauseUI();
//        UIStack.Add(baseUI.m_PrefabName);
//        if (baseUI.m_bPushFlag) {
//            if (UIStack.Count > 1) {
//                for (int i = UIStack.Count - 2; i >= 0; i--) {
//                    if (GetUI(UIStack[i])) {
//                        GetUI(UIStack[i]).gameObject.SetActive(false);
//                    }
//                }
//            }
//        }
//    }

//    void PauseUI()
//    {
//        BaseUI baseUI = GetUI(UIStack.LastOrDefault());
//        if (baseUI) {
//            GetUI(UIStack.LastOrDefault()).OnPause();
//        }
//    }

//    /// <summary>
//    /// 清空界面栈
//    /// </summary>
//    public void ClearUIStack()
//    {
//        List<BaseUI> hideList = new List<BaseUI>();

//        foreach (string prefabName in UIStack) {
//            BaseUI baseUI = GetUI(prefabName);
//            if (baseUI && IsUIVisible(baseUI)) {
//                hideList.Add(baseUI);
//            }
//        }

//        for (int i = 0; i < hideList.Count; i++) {
//            hideList[i].Hide(true);
//        }

//        UIStack.Clear();
//    }

//    /// <summary>
//    /// 获取UI的父节点
//    /// </summary>
//    /// <param name="prefabName"></param>
//    /// <returns></returns>
//    public Transform GetLayerParent(BaseUI baseUI)
//    {
//        if (baseUI) {
//            return UILayerParent[baseUI.m_UIDetail.LayerType];
//        }

//        return UILayerParent[UILayer.COMMON];
//    }

//    /// <summary>
//    /// 获取UI的父节点
//    /// </summary>
//    /// <param name="prefabName"></param>
//    /// <returns></returns>
//    public Transform GetLayerParent(UILayer layer)
//    {
//        return UILayerParent[layer];
//    }

//    /// <summary>
//    /// 判断是否属于common 层
//    /// </summary>
//    /// <param name="prefabName"></param>
//    /// <returns></returns>
//    public bool IsBelongCommonLayout(BaseUI baseUI)
//    {
//        bool isBelong = false;
//        if (baseUI && baseUI.m_UIDetail.LayerType == UILayer.COMMON) {
//            isBelong = true;
//        }

//        return isBelong;
//    }

//    /// <summary>
//    /// 判断销毁
//    /// </summary>
//    /// <param name="prefabName"></param>
//    /// <returns></returns>
//    private bool IsDestroy(BaseUI baseUI)
//    {
//        return baseUI && baseUI.m_UIDetail.HideType == HideType.DESTROY;
//    }

//    /// <summary>
//    /// 判断面板显示和隐藏状态
//    /// </summary>
//    /// <param name="prefabName"></param>
//    public bool IsUIVisible(string prefabName)
//    {
//        BaseUI baseUI = GetUI(prefabName);
//        if (baseUI) {
//            return baseUI.gameObject.activeInHierarchy;
//        }

//        return false;
//    }

//    /// <summary>
//    /// 判断面板显示和隐藏状态
//    /// </summary>
//    /// <param name="prefabName"></param>
//    public bool IsUIVisible(BaseUI baseUI)
//    {
//        if (null != baseUI) {
//            return baseUI.gameObject.activeInHierarchy;
//        }

//        return false;
//    }

//    /// <summary>
//    /// 屏蔽点击事件
//    /// </summary>
//    /// <param name="isMask"></param>
//    public void MaskUI(bool isMask = true, string maskKey = "")
//    {
//        if (isMask) {
//            MaskCount += 1;
//            if (!string.IsNullOrEmpty(maskKey)) {
//                MaskKeyList.Add(maskKey);
//            }
//        }
//        else {
//            if (MaskCount > 0) {
//                if (!string.IsNullOrEmpty(maskKey)) {
//                    if (MaskKeyList.Contains(maskKey)) {
//                        MaskCount -= 1;
//                        MaskKeyList.Remove(maskKey);
//                    }
//                }
//                else {
//                    MaskCount -= 1;
//                }
//            }
//        }

//        MaskEventPanel.SetActive(MaskCount != 0);
//    }

//    public void NetMask(bool isMask = true, string maskKey = "")
//    {
//        if (isMask) {
//            NetMaskCount += 1;
//            if (!string.IsNullOrEmpty(maskKey)) {
//                MaskKeyList.Add(maskKey);
//            }
//        }
//        else {
//            if (NetMaskCount > 0) {
//                if (!string.IsNullOrEmpty(maskKey)) {
//                    if (MaskKeyList.Contains(maskKey)) {
//                        NetMaskCount -= 1;
//                        MaskKeyList.Remove(maskKey);
//                    }
//                }
//                else {
//                    NetMaskCount -= 1;
//                }
//            }
//        }
//        if (NetMaskCount < 0)
//        {
//            NetMaskCount = 0;
//        }

//        //CommonUtil.DelayCall(0.5f, () =>
//        //{
//        //    NetMaskEventPanel.SetActive(NetMaskCount>0);
//        //});
//    }

//    /// <summary>
//    /// 设置canvas scaler mode
//    /// </summary>
//    /// <param name="mode"></param>
//    public void SetScreenMode(CanvasScaler.ScreenMatchMode mode)
//    {
//        if (null != m_CanvasScaler) {
//            m_CanvasScaler.screenMatchMode = mode;
//        }
//    }

//    /// <summary>
//    /// 获取UI Canvas
//    /// </summary>
//    public Canvas GetCanvas()
//    {
//        return m_Canvas;
//    }


//    /// <summary>
//    /// mask显示状态
//    /// </summary>
//    /// <returns></returns>
//    public bool IsMaskVisible()
//    {
//        return MaskPanelControl.CurrentStatus;
//    }
//}