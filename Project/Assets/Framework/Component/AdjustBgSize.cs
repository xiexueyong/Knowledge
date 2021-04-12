using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdjustBgSize : MonoBehaviour
{
    public Vector2 DesignSize = new Vector2(720,1558);
    public bool offsetChain = false;
    public Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    void Start()
    {
        if (image != null)
        {
            Vector2 parentSizeDelta = (UIManager.Inst.UICanvas.transform as RectTransform).sizeDelta;
            float rx = parentSizeDelta.x / DesignSize.x;
            float ry = parentSizeDelta.y / DesignSize.y;
            float rt = rx > ry ? rx : ry;

            RectTransform rectTransform = image.transform as RectTransform;
            rectTransform.localScale = new Vector3(rt, rt, 1);
            if(offsetChain)
                rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y - UIManager.Inst.chainHeight, rectTransform.localPosition.z);
        }
    }
}
