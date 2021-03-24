//using UnityEngine;
//using UnityEngine.UI;
//using System;
//using DG.Tweening;
//using System.Collections.Generic;
//using System.Collections;

//public class UIMaskPanelControl : MonoBehaviour
//{
//    private bool IsInit = false;
//    private Action MaskBgClickEvent;
//    private Image MaskBg;
//    private Tweener MaskBgTweener = null;

//    private readonly float ShowMaskAlpha = 0.6f;

//    private readonly float MaskBgAnimationTime = 0.4f;

//    [HideInInspector]
//    public bool CurrentStatus = false;

//    private Transform m_CurrentParent;
    
//    /// <summary>
//    /// 忽略控制mask
//    /// </summary>
//    private List<string> IgnoreControlMask = new List<string>()
//    {
//        UIModuleEnum.UIBlackBar,
//        "UIBubble",
//        UIModuleEnum.UIMainGameView,
//        UIModuleEnum.CommonPlay,
//        UIModuleEnum.DailyPuzzlePlay
//    };

//    // Start is called before the first frame update
//    void Init()
//    {
//        if (!IsInit) {
//            MaskBg = GetComponent<Image>();
//            UIEventListener.Get(gameObject).onClick += MaskBgClick;
//            IsInit = true;
//        }
//    }

//    /// <summary>
//    /// 设置遮挡层的位置
//    /// </summary>
//    public void UpdateMaskBg(BaseUI sourceBaseUI = null)
//    {
//        if (sourceBaseUI && IgnoreControlMask.Contains(sourceBaseUI.m_PrefabName)) {
//            return;
//        }
//        if (!IsInit) {
//            Init();
//        }

//        BaseUI baseUI = null;
//        for (int i = UIManager.Inst.UIModulesList.Count - 1; i >= 0; i--) {
//            BaseUI tempUI = UIManager.Inst.UIModulesList[i];
//            if (UIManager.Inst.IsUIVisible(tempUI) && !tempUI.m_UIDetail.m_HideMaskBg) {
//                baseUI = tempUI;
//                break;
//            }
//        }

//        if (baseUI) {
//            SetEnable(true);
//            if (baseUI.m_UIDetail.m_ClickMaskBgHide) {
//                MaskBgClickEvent = baseUI.OnCloseTrigger;
//            }
//            else {
//                MaskBgClickEvent = null;
//            }

//            int baseUIIndex = baseUI.transform.GetSiblingIndex();
//            int maskIndex = Mathf.Clamp(baseUIIndex, 0, baseUI.transform.GetSiblingIndex());
//            Transform parent = UIManager.Inst.GetLayerParent(baseUI);
//            transform.SetParent(parent);
//            if (m_CurrentParent != null && parent == m_CurrentParent) {
//                if (transform.GetSiblingIndex() > maskIndex) {
//                    maskIndex = Mathf.Clamp(baseUIIndex, 0, baseUI.transform.GetSiblingIndex());
//                }
//                else {
//                    maskIndex = Mathf.Clamp(baseUIIndex - 1, 0, baseUI.transform.GetSiblingIndex());
//                }
//            }

//            m_CurrentParent = parent;
//            transform.SetSiblingIndex(maskIndex);
//        }
//        else {
//            MaskBgClickEvent = null;
//            SetEnable(false);
//        }
//    }

//    void PlayMaskBgAnimation(bool isEnter = true)
//    {
//        float endValue = isEnter ? ShowMaskAlpha : 0;
//        float startValue = isEnter ? 0 : ShowMaskAlpha;
        
//        //if (MaskBgTweener != null && MaskBgTweener.IsPlaying()) {
//        //    MaskBg.SetColorAlpha(MaskBg.color.a);
//        //    MaskBgTweener.Kill(true);
//        //}
//        //else {
//        //    MaskBg.SetColorAlpha(startValue);
//        //}
        
//        //if (isEnter) {
//        //    CommonUtil.EnableMutiTouch(false);
//        //    gameObject.SetActive(true);
//        //}
        
//        //MaskBgTweener = MaskBg.DOFade(endValue, MaskBgAnimationTime).OnComplete(() =>
//        //{
//        //    if (!isEnter) {
//        //        MaskBg.SetColorAlpha(0);
//        //        CommonUtil.EnableMutiTouch(true);
//        //        gameObject.SetActive(false);
//        //    }
//        //});
//    }

//    public void SetEnable(bool enable)
//    {
//        CurrentStatus = enable;
//        CoroutineManager.Inst.StartCoroutine(NextFramePlayMaskBgAnimation());
//    }

//    IEnumerator NextFramePlayMaskBgAnimation()
//    {
//        yield return new WaitForEndOfFrame();
//        if (IsNeedPlayAnimation(CurrentStatus)) {
//            PlayMaskBgAnimation(CurrentStatus);
//        }
//    }
    
//    bool IsNeedPlayAnimation(bool isShow)
//    {
//        bool isNeed = false;
//        //关闭动画，需要播放隐藏动画
//        if (!isShow && gameObject.activeInHierarchy) {
//            isNeed = true;
//        }
//        //显示动画 1.对象隐藏，2.动画没有达到效果
//        if (!gameObject.activeInHierarchy || (gameObject.activeInHierarchy && !IsReachAlpha())) {
//            isNeed = true;
//        }
//        return isNeed;
//    }

//    bool IsReachAlpha()
//    {
//        return Math.Abs(ShowMaskAlpha - MaskBg.color.a) < 0.05;
//    }

//    void MaskBgClick(GameObject go)
//    {
//        if (null != MaskBgClickEvent) {
//            MaskBgClickEvent();
//            MaskBgClickEvent = null;
//        }
//    }
//}