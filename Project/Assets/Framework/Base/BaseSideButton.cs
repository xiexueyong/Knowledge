using System;
using DG.Tweening;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public enum SideButtonDirType
{
    Left_Top,
    Left_Bottom,
    Right,
    Bottom
}

public enum SideButtonType
{
    Single,
    Group   
}

public abstract class BaseSideButton : MonoBehaviour
{
    private bool m_bIsInit = false;

    [EnumToggleButtons, HideLabel, GUIColor(0f, 1, 0.6f)]
    public SideButtonType m_SideButtonType = SideButtonType.Single;
    
//    [HideInInspector]
    public int m_Priority = 0;
    
    [ShowIf("m_SideButtonType", SideButtonType.Group)]
    public RectTransform m_ParentRectTrans;

    public RectTransform m_ContentRect;
    public RectTransform m_MainObjRectTrans;

    public SideButtonDirType m_DirType;

    private Vector2 m_ContentSize;
    private Vector2 m_StartAnchorePos;
    private Tweener m_CurrentTweener = null;

    private void Init()
    {
        if (!m_bIsInit) {
            if (null == m_MainObjRectTrans) {
                m_MainObjRectTrans = gameObject.GetComponent<RectTransform>();
            }

            m_ContentSize = m_ContentRect.sizeDelta;
            m_StartAnchorePos = m_MainObjRectTrans.anchoredPosition;
            UIEventListener.Get(m_MainObjRectTrans.gameObject).onClick += OnPointerClick;
            UIEventListener.Get(m_MainObjRectTrans.gameObject).onDown += OnPointerDown;
            UIEventListener.Get(m_MainObjRectTrans.gameObject).onUp += OnPointerUp;
            m_bIsInit = true;
        }
    }

    public abstract bool IsEnable();

    public virtual int UnLockLevel()
    {
        return 0;
    }

    public virtual void OnShowBegin()
    {
    }

    public virtual void OnShowEnd()
    {
    }

    public virtual void OnHideBegin()
    {
    }

    public virtual void OnHideEnd()
    {
    }

    public void SetVisible(bool isVis, bool immediately = false)
    {
        if (!m_bIsInit) {
            Init();
        }

        //显示
        if (isVis) {
            m_ContentRect.sizeDelta = m_ContentSize;
            m_MainObjRectTrans.gameObject.SetActive(true);
            if (m_ParentRectTrans && m_SideButtonType == SideButtonType.Group) {
                LayoutRebuilder.ForceRebuildLayoutImmediate(m_ParentRectTrans);
            }
            
            OnShowBegin();
            if (immediately) {
                OnShowEnd();
            }
            else {
                PlayAnimation(true, () =>
                {
                    OnShowEnd(); 
                });
            }
        }
        //隐藏
        else {
            OnHideBegin();
            if (immediately) {
                m_MainObjRectTrans.gameObject.SetActive(false);
                m_ContentRect.sizeDelta = new Vector2(m_ContentSize.x, 0);
                OnHideEnd();
                if (m_SideButtonType == SideButtonType.Group) {
                    LayoutRebuilder.ForceRebuildLayoutImmediate(m_ParentRectTrans);
                }
            }
            else {
                PlayAnimation(false, () =>
                {
                    m_MainObjRectTrans.gameObject.SetActive(false);
                    m_ContentRect.sizeDelta = new Vector2(m_ContentSize.x, 0);
                    OnHideEnd();
                    if (m_SideButtonType == SideButtonType.Group) {
                        LayoutRebuilder.ForceRebuildLayoutImmediate(m_ParentRectTrans);
                    }
                });
            }
        }
    }


    public void ReActive()
    {
        m_ContentRect.SetAsFirstSibling();
        SetVisible(true);
    }


    public virtual void OnPointerClick(GameObject go)
    {
        SoundPlay.PlaySFX(Table.Sound.sfx_btn_normal);
    }

    public virtual void OnPointerDown(GameObject go)
    {
    }

    public virtual void OnPointerUp(GameObject go)
    {
    }

    void PlayAnimation(bool isEnter = true, Action callback = null)
    {
        if (m_CurrentTweener != null && m_CurrentTweener.IsPlaying()) {
            m_CurrentTweener.Kill();
        }

        float overDisRatio = 1.2f;

        float aniTime = isEnter ? 1f : 0.5f;
        Vector2 endPos = m_StartAnchorePos;

        TweenParams aniParams = new TweenParams();
        aniParams.SetEase(isEnter ? Ease.OutBack : Ease.OutCubic);
        aniParams.SetDelay(isEnter ? 0.1f * m_Priority : 0);
        aniParams.OnComplete(() =>
        {
            if (null != callback) {
                callback();
            }
        });
        if (!isEnter) {
            m_MainObjRectTrans.anchoredPosition = m_StartAnchorePos;
        }

        switch (m_DirType) {
            case SideButtonDirType.Left_Top:
            case SideButtonDirType.Left_Bottom:
                Vector2 tempLeftPos = new Vector2(- m_ContentRect.anchoredPosition.x - m_MainObjRectTrans.rect.width, m_StartAnchorePos.y);
                if (isEnter) {
                    m_MainObjRectTrans.anchoredPosition = tempLeftPos;
                }
                else {
                    endPos = tempLeftPos;
                }
                break;
            case SideButtonDirType.Right:
                Vector2 tempRightPos = new Vector2(m_StartAnchorePos.x + m_ContentRect.rect.width * overDisRatio, m_StartAnchorePos.y);
                if (isEnter) {
                    m_MainObjRectTrans.anchoredPosition = tempRightPos;
                }
                else {
                    endPos = tempRightPos;
                }
                break;
            case SideButtonDirType.Bottom:

                Vector2 tempBottomPos = new Vector2(m_StartAnchorePos.x, m_StartAnchorePos.y - m_ContentRect.rect.width * overDisRatio);
                if (isEnter) {
                    m_MainObjRectTrans.anchoredPosition = tempBottomPos;
                }
                else {
                    endPos = tempBottomPos;
                }

                break;
        }

        m_CurrentTweener = m_MainObjRectTrans.DOAnchorPos(endPos, aniTime).SetAs(aniParams);
    }
}