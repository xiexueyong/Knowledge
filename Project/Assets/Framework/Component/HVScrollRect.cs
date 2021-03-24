using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HVScrollRect : ScrollRect
{

    public ScrollRect parentScroll;

    public bool isVertical = false;

    private bool isSelf = false;

    public Action OnEndDragListener;
    public Action OnBeginDragListener;
    public Action OnDragListener;

    public override void OnBeginDrag(PointerEventData eventData)
    {
        Vector2 touchDeltaPosition = Vector2.zero;
#if UNITY_EDITOR
        float delta_x = Input.GetAxis("Mouse X");
        float delta_y = Input.GetAxis("Mouse Y");
        touchDeltaPosition = new Vector2(delta_x, delta_y);
#endif

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        touchDeltaPosition = Input.GetTouch(0).deltaPosition;
#endif
        if (isVertical)
        {
            if (Mathf.Abs(touchDeltaPosition.x) < Mathf.Abs(touchDeltaPosition.y))
            {
                isSelf = true;
                base.OnBeginDrag(eventData);
            }
            else
            {
                isSelf = false;
                parentScroll.OnBeginDrag(eventData);
            }
        }
        else
        {
            if (Mathf.Abs(touchDeltaPosition.x) > Mathf.Abs(touchDeltaPosition.y))
            {
                isSelf = true;
                base.OnBeginDrag(eventData);
            }
            else
            {
                isSelf = false;
                parentScroll.OnBeginDrag(eventData);
            }
        }
        OnBeginDragListener?.Invoke();
    }


    public override void OnDrag(PointerEventData eventData)
    {
        if (isSelf)
        {
            base.OnDrag(eventData);
        }
        else
        {
            parentScroll.OnDrag(eventData);
        }
        OnDragListener?.Invoke();
    }



    public override void OnEndDrag(PointerEventData eventData)
    {
        if (isSelf)
        {
            base.OnEndDrag(eventData);
        }
        else
        {
            parentScroll.OnEndDrag(eventData);
        }
        OnEndDragListener?.Invoke();
    }

}