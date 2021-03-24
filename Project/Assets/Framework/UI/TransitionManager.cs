using System;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using Framework.Asset;

public class TransitionSpinData
{
    public TrackEntry m_TrackEntry;
    public string m_AniName;
    public Action m_Callback;
    public SkeletonGraphic m_Skeleton;
    public bool m_AutoHide = true;
    public TransitionSpinData(SkeletonGraphic skeleton,TrackEntry trackEntry, string aniName,Action callBack,bool autoHide = true)
    {
        m_TrackEntry = trackEntry;
        m_AniName = aniName;
        m_Callback = callBack;
        m_Skeleton = skeleton;
        m_AutoHide = autoHide;
    }
}
/// <summary>
/// 用于转场动画
/// </summary>
public class TransitionManager : S_MonoSingleton<TransitionManager>
{
    public Dictionary<TrackEntry ,TransitionSpinData> SpineDataDic = new Dictionary<TrackEntry, TransitionSpinData>();
    private Dictionary<string, SkeletonGraphic> SpineAniDic = new Dictionary<string, SkeletonGraphic>();
    public Transform m_Parent;



    public TrackEntry PlayTransition(string prefabName, string aniName, Action callback = null,bool isAutoHide = true,string skiName =null)
    {
        string path = "Prefab/Spine/" + prefabName;
        SkeletonGraphic spine;
        if (SpineAniDic.ContainsKey(path)) {
            spine = SpineAniDic[path];
        }
        else {
//            GameObject ani = Instantiate(Resources.Load<GameObject>(path)) ;
            GameObject ani = Res.LoadResource<GameObject>(path);
            ani.transform.SetParent(m_Parent);
            ani.transform.localScale = Vector3.one;
            ani.transform.localPosition = Vector3.zero;
            spine = ani.GetComponent<SkeletonGraphic>();
            if (null == spine) {
                return null;
            }
        }

        if (!string.IsNullOrEmpty(skiName))
        {
            if (spine.initialSkinName != skiName)
            {
                spine.initialSkinName = skiName;
                spine.Initialize(true);
            }
        }

        spine.gameObject.SetActive(true);
        TrackEntry trackEntry = spine.AnimationState.SetAnimation(0, aniName, false);
        spine.AnimationState.Complete += SpineAniComplete;
        TransitionSpinData aniData = new TransitionSpinData(spine,trackEntry,aniName,callback,isAutoHide);
        SpineDataDic[trackEntry] = aniData;
        SpineAniDic[path] = spine;
        return trackEntry;
    }





    /// <summary>
    /// 动画播放结束
    /// </summary>
    /// <param name="enter"></param>
    private void SpineAniComplete(TrackEntry enter)
    {
        if (SpineDataDic.ContainsKey(enter)) {
            var spineData = SpineDataDic[enter];
            
            //开始弹出奖励面板
            if (enter.Animation.Name == spineData.m_AniName) {
                if (null != spineData.m_Callback) {
                    spineData.m_Callback.Invoke();
                }
            }
            spineData.m_Skeleton.AnimationState.Complete -= SpineAniComplete;
            if (spineData.m_AutoHide) {
                spineData.m_Skeleton.gameObject.SetActive(false); 
            }
            SpineDataDic.Remove(enter);
        }
        
    }
}