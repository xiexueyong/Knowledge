using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;

//动画类型
public enum UIAnimationStyle
{
    NONE = 0,
    SCALE,
    MOVE_FROM_LEFT,
    MOVE_FROM_RIGHT,
    MOVE_FROM_TOP,
    MOVE_FROM_B0TTOM
}

//内置动画类型
public enum UIBuildInAnimationStyle
{
    NONE = 0,
    SCALE,
}

public enum UIAnimationType
{
    None,
    Built_In,
    Custom,
   
}


public enum EaseType
{
    Ease,
    Curve
}


[Serializable]
public class UIAnimatioinDetail
{
    [EnumToggleButtons, HideLabel, GUIColor(0f, 1, 0.6f)]
    public UIAnimationType m_AnimationType = UIAnimationType.Built_In;

    [ShowIf("m_AnimationType", UIAnimationType.Built_In)]
    public UIBuildInAnimationStyle m_BuildInAnimation = UIBuildInAnimationStyle.SCALE;

    [ShowIf("m_AnimationType", UIAnimationType.Custom)]
    public UIAnimationStyle m_ShowAnimationStyle = UIAnimationStyle.NONE;

    [ShowIf("m_AnimationType", UIAnimationType.Custom)]
    public UIAnimationStyle m_HideAnimationStyle = UIAnimationStyle.NONE;

    [ShowIf("m_AnimationType", UIAnimationType.Custom)]
    public float m_EnterTime = 0.5f;

    [ShowIf("m_AnimationType", UIAnimationType.Custom)]
    public float m_ExitTime = 0.5f;

    [ShowIf("m_AnimationType", UIAnimationType.Custom)] [EnumToggleButtons]
    public EaseType m_EaseType;

    [ShowIf("m_AnimationType", UIAnimationType.Custom)] [ShowIf("m_EaseType", EaseType.Ease)]
    public Ease m_EnterEaseType = Ease.Linear;

    [ShowIf("m_AnimationType", UIAnimationType.Custom)] [ShowIf("m_EaseType", EaseType.Ease)]
    public Ease m_ExitEaseType = Ease.Linear;

    [ShowIf("m_AnimationType", UIAnimationType.Custom)] [ShowIf("m_EaseType", EaseType.Curve)]
    public AnimationCurve m_EnterCurve = null;

    [ShowIf("m_AnimationType", UIAnimationType.Custom)] [ShowIf("m_EaseType", EaseType.Curve)]
    public AnimationCurve m_ExitCurve = null;
}


public class UIAnimationControl : S_MonoSingleton<UIAnimationControl>
{
    private Dictionary<BaseUI, Tweener> m_AnimationDic = new Dictionary<BaseUI, Tweener>();


    /// <summary>
    /// 进入界面播放动画
    /// </summary>
    public void PlayAnimation(BaseUI baseUI, Action callBack, bool isEnter = true)
    {
        UIAnimatioinDetail animationDetail = baseUI.m_AnimationDetail;
        Tween m_CurrentTween = m_AnimationDic.ContainsKey(baseUI) ? m_AnimationDic[baseUI] : null;

        if (m_CurrentTween != null && m_CurrentTween.IsPlaying())
        {
            m_CurrentTween.Kill();
        }

        //自定义动画
        if (animationDetail.m_AnimationType == UIAnimationType.None)
        {
            AnimationComplete(baseUI, callBack);
        }
        else if(animationDetail.m_AnimationType == UIAnimationType.Custom)
        {
            float startValue = isEnter ? 0 : 1;
            float endValue = isEnter ? 1 : 0;
            Vector3 startPos = Vector3.zero;
            Vector3 endPos = Vector3.zero;
            //运动时间
            float animationTime = isEnter ? animationDetail.m_EnterTime : animationDetail.m_ExitTime;


            //            startValue = baseUI.m_root.transform.localScale.x;
            startPos = baseUI.m_root.transform.localPosition;

            SetAnimtionPos(animationDetail, ref startPos, ref endPos, isEnter);

            TweenParams tempTweenParams = new TweenParams();
            tempTweenParams.OnComplete(() => { AnimationComplete(baseUI, callBack); });

            Tweener tempTweener = null;
            //设置动作类型
            tempTweenParams.SetEase(Ease.Linear);
            if (isEnter)
            {
                gameObject.SetActive(true);
                if (animationDetail.m_EaseType == EaseType.Ease)
                {
                    tempTweenParams.SetEase(animationDetail.m_EnterEaseType);
                }
                else if (animationDetail.m_EnterCurve != null)
                {
                    tempTweenParams.SetEase(animationDetail.m_EnterCurve);
                }
            }
            else
            {
                if (animationDetail.m_EaseType == EaseType.Ease)
                {
                    tempTweenParams.SetEase(animationDetail.m_ExitEaseType);
                }
                else if (animationDetail.m_EnterCurve != null)
                {
                    tempTweenParams.SetEase(animationDetail.m_ExitCurve);
                }
            }

            UIAnimationStyle aniStyle = isEnter ? animationDetail.m_ShowAnimationStyle : animationDetail.m_HideAnimationStyle;

            //没有动画
            if (aniStyle == UIAnimationStyle.NONE)
            {
                AnimationComplete(baseUI, callBack);
            }
            else
            {
                if (aniStyle == UIAnimationStyle.SCALE)
                {
                    baseUI.m_root.transform.localScale = new Vector3(startValue, startValue, startValue);
                    tempTweener = baseUI.m_root.transform.DOScale(endValue, animationTime).SetAs(tempTweenParams);
                }
                else
                {
                    baseUI.m_root.transform.localPosition = startPos;
                    tempTweener = baseUI.m_root.transform.DOLocalMove(endPos, animationTime).SetAs(tempTweenParams);
                }

                m_AnimationDic[baseUI] = tempTweener;
            }
        }

        //预制的动画
        else
        {
            PlayBuildInAnimation(baseUI, animationDetail.m_BuildInAnimation, callBack, isEnter);
        }
    }


    /// <summary>
    /// 播放内置动画
    /// </summary>
    void PlayBuildInAnimation(BaseUI baseUI, UIBuildInAnimationStyle animationType, Action callBack, bool isEnter = true)
    {
        float endValue = isEnter ? 1 : 0;
        float startValue = isEnter ? 0 : 1;
        switch (animationType) {
            case UIBuildInAnimationStyle.NONE:
                AnimationComplete(baseUI, callBack);
                break;
            case UIBuildInAnimationStyle.SCALE:
                float aniTime = 0.5f;
                baseUI.m_root.transform.localScale = new Vector3(startValue, startValue, startValue);
                TweenParams tempTweenParams = new TweenParams();
                tempTweenParams.SetEase(isEnter ? Ease.OutBack : Ease.InBack);
                tempTweenParams.OnComplete(() => { AnimationComplete(baseUI, callBack); });

                Tweener tempTweener = baseUI.m_root.transform.DOScale(endValue, aniTime).SetAs(tempTweenParams);
                m_AnimationDic[baseUI] = tempTweener;
                break;
        }
    }


    /// <summary>
    /// 动画结束
    /// </summary>
    /// <param name="target"></param>
    /// <param name="callback"></param>
    void AnimationComplete(BaseUI baseUI, Action callback)
    {
        callback?.Invoke();
        callback = null;
        if (m_AnimationDic.ContainsKey(baseUI)) {
            m_AnimationDic.Remove(baseUI);
        }
    }

    /// <summary>
    /// 设置运动的起点和终点
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <param name="isEnter"></param>
    void SetAnimtionPos(UIAnimatioinDetail animationDetail, ref Vector3 startPos, ref Vector3 endPos, bool isEnter)
    {
        UIAnimationStyle aniStyle = isEnter ? animationDetail.m_ShowAnimationStyle : animationDetail.m_HideAnimationStyle;
        switch (aniStyle) {
            case UIAnimationStyle.MOVE_FROM_LEFT:
                startPos = new Vector3(-Screen.width * 1.5f, 0, 0);
                endPos = Vector3.zero;
                break;
            case UIAnimationStyle.MOVE_FROM_RIGHT:
                startPos = new Vector3(Screen.width * 1.5f, 0, 0);
                endPos = Vector3.zero;
                break;
            case UIAnimationStyle.MOVE_FROM_TOP:
                startPos = new Vector3(0, Screen.height * 1.5f, 0);
                endPos = Vector3.zero;
                break;
            case UIAnimationStyle.MOVE_FROM_B0TTOM:
                startPos = new Vector3(0, -Screen.height * 1.5f, 0);
                endPos = Vector3.zero;
                break;
        }

        if (!isEnter) {
            Vector3 temp = startPos;
            startPos = endPos;
            endPos = temp;
        }
    }
}