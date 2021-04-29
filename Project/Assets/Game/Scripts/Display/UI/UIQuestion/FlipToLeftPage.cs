using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FlipToLeftPage : MonoBehaviour
{
    public float time_mask = 1.5f;
    public float time_page = 1.5f;
    public RectTransform rectTransform_mask;
    public RectTransform rectTransform_page;
    public RectTransform rectTransform_shadow;
    public RawImage page_image;
    public RawImage mask_image;

    private bool _setSize;
    private Vector2? screenSize;
    void setSize()
    {
        if(_setSize)
            return;
        _setSize = true;

        screenSize = UIManager.Inst.GetUISize();
        rectTransform_mask.sizeDelta = new Vector2(screenSize.Value.x,screenSize.Value.y+200);
        rectTransform_page.sizeDelta = new Vector2(screenSize.Value.x+100,screenSize.Value.y+200);
        rectTransform_shadow.sizeDelta = new Vector2(screenSize.Value.x,screenSize.Value.y+200);
    }

    void resetPosition()
    {
        rectTransform_mask.localPosition = Vector3.zero;
        rectTransform_shadow.localPosition = new Vector3(30,0,0);
        rectTransform_page.localPosition = new Vector3(screenSize.Value.x,0,0);
        rectTransform_page.localRotation = Quaternion.Euler(0, 0, -15f);
    }
    
    public void FlipPage(Action callback,Texture2D texture2D)
    {
        gameObject.SetActive(true);
        StartCoroutine(flipPage(callback));
    }

    IEnumerator flipPage(Action callback)
    {
        yield return null;
        setSize();
        resetPosition();
        rectTransform_mask.DOLocalMove(new Vector3(-720, 0, 0), time_mask);
        rectTransform_page.DOLocalMove(new Vector3(0, 0, 0), time_page);
        rectTransform_page.DOLocalRotate(new Vector3(0, 0, 0), 1.3f,RotateMode.Fast).SetDelay(0.1f);
        yield return new WaitForSeconds(0.5f);
        callback?.Invoke();
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
    
    

    public void SetTexture(Texture2D texture2D)
    {
        page_image.texture = texture2D;
    }
}
