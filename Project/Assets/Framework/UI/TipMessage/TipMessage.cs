using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Framework.Asset;
using System;

public class TipMessage : MonoBehaviour
{
    protected Text textMessage;
    protected Tweener tweener;
    public virtual void Awake()
    {
        textMessage = GetComponentInChildren<Text>();
    }
    public virtual void setTip(string tips)
    {
        textMessage.text = tips;
    }
    public virtual void StartAnim(TweenCallback callback)
    {
        tweener = this.transform.DOLocalMoveY(transform.localPosition.y+150,1.8f);
        tweener.OnComplete(callback);
    }
    public virtual void Reset()
    {
        transform.localPosition = new Vector3(0,0,0);
        if (tweener != null)
        {
            tweener.Kill();
            tweener = null;
        }
    }
}
