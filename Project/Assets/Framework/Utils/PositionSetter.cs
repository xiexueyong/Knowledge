using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class PositionSetter : MonoBehaviour
{
    Tweener localPositionTweener;
    private Vector3 _localPostion;
    public Vector3 localPosition
    {
        set
        {
            _localPostion = value;
            localPositionTweener?.Kill();
            localPositionTweener = transform.DOLocalMove(value, 0.3f);
        }
    }


    Tweener horizontalTweener;
    private float _horizontalNormalizedPosition;
    public float horizontalNormalizedPosition
    {
        set
        {
            _horizontalNormalizedPosition = value;
            horizontalTweener?.Kill();
            horizontalTweener = transform.GetComponent<ScrollRect>().DOHorizontalNormalizedPos(value, 0.3f);
        }
    }
}
