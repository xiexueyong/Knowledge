using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomScrollRect : ScrollRect
{
    public Action OnEndDragListener;
    public Action OnBeginDragListener;
    public Action OnDragListener;

    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        OnBeginDragListener?.Invoke();
    }


    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);
        OnDragListener?.Invoke();
    }


    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        OnEndDragListener?.Invoke();
    }

}