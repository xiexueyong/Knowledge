using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIDepth : MonoBehaviour
{
    /// <summary>
    ///开始启动
    /// </summary>
    public bool startEnable = true;

    /// <summary>
    /// 是否是UI
    /// </summary>
    public bool isUI = true;

    /// <summary>
    /// 排序层名称
    /// </summary>
    public string sortLayerName = "UI";

    /// <summary>
    /// 深度，默认为零
    /// </summary>
    [SerializeField] private int depth;

    //GraphicRaycaster
    private GraphicRaycaster raycaster;

    private int lastDepth = 0;
    private bool isFirstSet = false;


    // Use this for initialization
    void Start()
    {
        if (startEnable) {
            Depth = depth;
        }
    }

    private void RefreshDepth()
    {
        if (isUI) {
            Canvas canvas = GetComponent<Canvas>();
            raycaster = GetComponent<GraphicRaycaster>();
            if (canvas == null) canvas = gameObject.AddComponent<Canvas>();
            if (raycaster == null) raycaster = gameObject.AddComponent<GraphicRaycaster>();
            canvas.overrideSorting = true;
            canvas.sortingLayerName = sortLayerName;
            canvas.sortingOrder = depth;
        }
        else {
            Renderer[] renders = GetComponentsInChildren<Renderer>();
            Dictionary<int, List<Renderer>> sortDic = new Dictionary<int, List<Renderer>>();
            foreach (Renderer render in renders) {
                render.sortingLayerName = sortLayerName;
//                render.sortingOrder = depth;
                if (!sortDic.ContainsKey(render.sortingOrder)) {
                    sortDic[render.sortingOrder] = new List<Renderer>();
                }

                sortDic[render.sortingOrder].Add(render);
            }

            sortDic = sortDic.OrderBy(o => o.Key).ToDictionary(o => o.Key, p => p.Value);
            int tempIndex = sortDic.Count;
            foreach (var dicItem in sortDic) {
                foreach (var listItem in dicItem.Value) {
                    listItem.sortingOrder = depth - tempIndex;
                }
                tempIndex--;
            }
        }
    }

    public void RemoveCanvas()
    {
        if (isUI) {
            raycaster = GetComponent<GraphicRaycaster>();
            if (raycaster != null) {
                Destroy(raycaster);
            }

            Canvas canvas = GetComponent<Canvas>();
            if (canvas != null) {
                Destroy(canvas);
            }

            depth = lastDepth;
        }
        else {
            Renderer[] renders = GetComponentsInChildren<Renderer>();
            foreach (Renderer render in renders) {
                render.sortingLayerName = sortLayerName;
                render.sortingOrder = lastDepth;
            }

            depth = lastDepth;
        }
    }

    public int Depth
    {
        set
        {
            depth = value;
            if (!isFirstSet) {
                lastDepth = depth;
                isFirstSet = true;
            }

            RefreshDepth();
        }
        get { return depth; }
    }

    /// <summary>
    /// 屏蔽点击事件
    /// </summary>
    /// <param name="isMask"></param>
    public void IsMaskTouch(bool isMask)
    {
        if (null != raycaster) {
            raycaster.enabled = isMask;
        }
    }
}