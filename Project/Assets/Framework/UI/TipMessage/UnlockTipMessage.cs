using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Framework.Asset;
using System;

public class UnlockTipMessage : TipMessage
{

    public override void StartAnim(TweenCallback callback)
    {
        tweener?.Kill();
        tweener = this.transform.DOLocalMoveY(transform.localPosition.y+70,0.8f);
        tweener.OnComplete(callback);
    }
}
