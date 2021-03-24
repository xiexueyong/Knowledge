using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

/// <summary>
/// 通用PageView
/// 挂在 ScrollView 上
/// </summary>
public class CommonPageView : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    //m_ScrollRect
    public ScrollRect m_ScrollRect;

    //m_ScrollRect Content
    private Transform m_Content;

    //页面数量
    [HideInInspector]
    public int m_nPageCount = 0;

    //每一页占用的比例
    private float m_fPagePercent = 0f;

    //惯性的阻力
    public float m_fFrontalResistance = 100f;

    //平滑速度
    public float m_fSmoothing = 4;

    //每个界面的所在位置百分比(初始化长度为4
    private List<float> m_PageList = new List<float>();

    //翻页到目标坐标
    private float m_fTargetHorizontalPosition = 0;

    //是否正在拖拽
    private bool m_bIsDraging = false;

    //当前处于的页码
    private int m_nPageIndex = 0;

    public Action<int> OnPageChanged;

    public void InitPage()
    {
        m_Content = m_ScrollRect.content;
        int count = 0;
        foreach (Transform child in m_Content) {
            if (child.gameObject.activeInHierarchy) {
                count++;
            }
        }

        m_nPageCount = count;
        m_PageList.Clear();
        m_fPagePercent = 1f / (m_nPageCount - 1f);
        //初始化每个页面的百分比
        for (int i = 0; i < m_nPageCount; i++) {
            m_PageList.Add(i * m_fPagePercent);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_bIsDraging == false)
            SetPage();
    }

    /// <summary>
    /// 设置页面
    /// </summary>
    void SetPage()
    {
        m_ScrollRect.horizontalNormalizedPosition = Mathf.Lerp(m_ScrollRect.horizontalNormalizedPosition,m_fTargetHorizontalPosition, Time.deltaTime * m_fSmoothing);
    }

    /// <summary>
    /// 开始拖拽
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        m_bIsDraging = true;
    }

    /// <summary>
    /// 拖拽操作
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        float speedX = -eventData.delta.x;
        m_bIsDraging = false;
        //当前滚动位置
        float posX = m_ScrollRect.horizontalNormalizedPosition + Mathf.Clamp(speedX / m_fFrontalResistance, -m_fPagePercent, m_fPagePercent);
        int index = 0;
        //当前page页和滚动当前位置的距离
        float offset = Mathf.Abs(m_PageList[index] - posX);
        //遍历所有的page页，找到距离最近的page，然后这个page页
        for (int i = 1; i < m_PageList.Count; i++) {
            float offsetTemp = Mathf.Abs(m_PageList[i] - posX);
            if (offsetTemp < offset) {
                index = i;
                offset = offsetTemp;
            }
        }

        TurnToPage(index);
    }

    /// <summary>
    /// 翻到到下一页
    /// </summary>
    public void NextPage()
    {
        if (m_nPageIndex < m_nPageCount - 1) {
            m_nPageIndex++;
        }

        TurnToPage(m_nPageIndex);
    }

    /// <summary>
    /// 翻到上一页
    /// </summary>
    public void PrePage()
    {
        if (m_nPageIndex > 0) {
            m_nPageIndex--;
        }

        TurnToPage(m_nPageIndex);
    }

    public void TurnToPage(int index, bool immediately = false)
    {
        if (index >= m_PageList.Count) {
            Debug.LogError("Page not exist!!!");
            return;
        }

        //目标移动到的位置
        float tartPos = m_PageList[index];
        if (immediately) {
            m_ScrollRect.horizontalNormalizedPosition = tartPos;
        }
        else {
            if (Mathf.Abs(index - m_nPageIndex) > 1) {
                //向右滚动
                if (index > m_nPageIndex) {
                    m_ScrollRect.horizontalNormalizedPosition = m_PageList[index - 1];
                }
                //向左滚动
                else {
                    m_ScrollRect.horizontalNormalizedPosition = m_PageList[index + 1];
                }
            }
        }

        m_nPageIndex = index;
        m_fTargetHorizontalPosition = tartPos;
        if (null != OnPageChanged) {
            OnPageChanged(m_nPageIndex);
        }
        SetPage();
    }

    /// <summary>
    /// 移动到首页
    /// </summary>
    public void ScrollToFirstPage()
    {
        m_nPageIndex = 0;
        TurnToPage(m_nPageIndex);
    }

    /// <summary>
    /// 滑动到初始位置
    /// </summary>
    public void ScrollToStartPos()
    {
        m_nPageIndex = 0;
        m_fTargetHorizontalPosition = 0;
        m_ScrollRect.horizontalNormalizedPosition = 0;
    }
    
}