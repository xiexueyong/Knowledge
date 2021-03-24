using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;

public class CustomEventTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IDropHandler, IScrollHandler, IUpdateSelectedHandler, ISelectHandler, IDeselectHandler, IMoveHandler, ISubmitHandler, ICancelHandler, IEventSystemHandler
{
	//
	// Fields
	//
	[Obsolete ("Please use triggers instead (UnityUpgradable) -> triggers", true)]
	public List<CustomEventTrigger.Entry> delegates;

	//
	// Properties
	//
	public List<CustomEventTrigger.Entry> triggers {
		get;
		set;
	}

	//
	// Constructors
	//
	protected CustomEventTrigger () { }

	//
	// Methods
	//
	private void Execute (EventTriggerType id, BaseEventData eventData) { }

	public virtual void OnCancel (BaseEventData eventData) {}

	public virtual void OnDeselect (BaseEventData eventData) {}

	public virtual void OnDrop (PointerEventData eventData) {}

	public virtual void OnInitializePotentialDrag (PointerEventData eventData) {}

	public virtual void OnMove (AxisEventData eventData) {}

	public virtual void OnPointerClick (PointerEventData eventData) {}

	public virtual void OnPointerDown (PointerEventData eventData) {}

	public virtual void OnPointerEnter (PointerEventData eventData) {}

	public virtual void OnPointerExit (PointerEventData eventData) {}

	public virtual void OnPointerUp (PointerEventData eventData) {}

	public virtual void OnScroll (PointerEventData eventData) {}

	public virtual void OnSelect (BaseEventData eventData) {}

	public virtual void OnSubmit (BaseEventData eventData) {}

	public virtual void OnUpdateSelected (BaseEventData eventData) {}

	//
	// Nested Types
	//
	[Serializable]
	public class Entry
	{
		public EventTriggerType eventID;

		public CustomEventTrigger.TriggerEvent callback;

		public Entry () {}
	}

	[Serializable]
	public class TriggerEvent : UnityEvent<BaseEventData>
	{
		public TriggerEvent () {}
	}
}
