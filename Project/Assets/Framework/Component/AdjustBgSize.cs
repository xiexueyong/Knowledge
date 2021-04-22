using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdjustBgSize : MonoBehaviour
{
    // public bool offsetChain = false;
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
            var designSize = new Vector2(720,1558);
            float rx = parentSizeDelta.x / image.sprite.texture.width;
            float ry = parentSizeDelta.y / image.sprite.texture.height;
            float rt = rx > ry ? rx : ry;

            RectTransform rectTransform = image.transform as RectTransform;
            // rectTransform.localScale = new Vector3(rt, rt, 1);
            // rectTransform.sizeDelta = new Vector2(image.sprite.texture.width*rt,image.sprite.texture.height*rt);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,image.sprite.texture.width*rt);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,image.sprite.texture.height*rt);
            // if(offsetChain)
            //     rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, rectTransform.localPosition.y - UIManager.Inst.chainHeight, rectTransform.localPosition.z);
        }
    }
}
