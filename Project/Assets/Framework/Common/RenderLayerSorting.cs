using UnityEngine;
using UnityEngine.UI;

public class RenderLayerSorting : MonoBehaviour
{
    public bool update;
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
    public string sortLayerName = "Default"; 

     //GraphicRaycaster
    private GraphicRaycaster raycaster;

    /// <summary>
    /// 深度，默认为零
    /// </summary>
    [SerializeField] private int depth;

    public int Depth
    {
        set { depth = value; RefreshDepth(); }
        get { return depth; }
    }

    void Update()
    {
        if(update) {
            update = false;
            RefreshDepth();
        }
    }

    private void Start()
    {
        if (startEnable) { RefreshDepth(); }
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
            foreach (Renderer render in renders) {
                render.sortingLayerName = sortLayerName;
                render.sortingOrder = depth;
            }
        }
    }

    public void RemoveUICanvas()
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
        }
    }
}