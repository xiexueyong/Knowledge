using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Framework.Utils;

public class UIEventListener : CustomEventTrigger
{
	public event UnityEngine.Events.UnityAction<GameObject> onClick;
	public event UnityEngine.Events.UnityAction<GameObject> onDown;
	public event UnityEngine.Events.UnityAction<GameObject> onUp;

	public static UIEventListener Get(GameObject go) 
	{
		UIEventListener com = go.GetComponent<UIEventListener>();
		if(com == null) com = go.AddComponent<UIEventListener>();
		return com;
	}

	public override void OnPointerClick(PointerEventData eventData)
	{ 
		if(onClick != null) onClick(gameObject);
	}

	public override void OnPointerDown (PointerEventData eventData)
	{ 
		if(onDown != null) onDown(gameObject);
	}

	public override void OnPointerUp (PointerEventData eventData)
	{ 
		if(onUp != null) onUp(gameObject);
	}

	public void AddButtonListener(UnityEngine.Events.UnityAction func)
	{
		Utils.GetComponent<Button> (this.gameObject).onClick.AddListener(func);
	}

	public void RemoveButtonListener(UnityEngine.Events.UnityAction func)
	{
		if (this.gameObject.GetComponent<Button> () != null) {
			this.gameObject.GetComponent<Button>().onClick.RemoveListener(func);
		}
	}

	public void AddInputOnValueChanged(UnityEngine.Events.UnityAction<string> func) 
	{
		if (this.gameObject.GetComponent<InputField> () != null) {
			this.gameObject.GetComponent<InputField> ().onValueChanged.AddListener (func);
		}
	}

	public void RemoveInputOnValueChanged(UnityEngine.Events.UnityAction<string> func) 
	{
		if (this.gameObject.GetComponent<InputField> () != null) {
			this.gameObject.GetComponent<InputField> ().onValueChanged.RemoveListener (func);
		}
	}

	public void AddSliderOnValueChanged(UnityEngine.Events.UnityAction<float> func)
	{
		if (this.gameObject.GetComponent<Slider> () != null) {
			this.gameObject.GetComponent<Slider> ().onValueChanged.AddListener (func);
		}
	}

	public void RemoveSliderOnValueChanged(UnityEngine.Events.UnityAction<float> func)
	{
		if (this.gameObject.GetComponent<Slider> () != null) {
			this.gameObject.GetComponent<Slider> ().onValueChanged.RemoveListener (func);
		}
	}

	public void AddToggleOnValueChanged(UnityEngine.Events.UnityAction<bool> func) 
	{
		if (this.gameObject.GetComponent<Toggle> () != null) {
			this.gameObject.GetComponent<Toggle> ().onValueChanged.AddListener (func);
		}
	}

	public void RemoveToggleOnValueChanged(UnityEngine.Events.UnityAction<bool> func) 
	{
		if (this.gameObject.GetComponent<Toggle> () != null) {
			this.gameObject.GetComponent<Toggle> ().onValueChanged.RemoveListener (func);
		}
	}

	public void AddScrollOnValueChanged (UnityEngine.Events.UnityAction<Vector2> func)
	{
		if (this.gameObject.GetComponent<ScrollRect> () != null) {
			this.gameObject.GetComponent<ScrollRect> ().onValueChanged.AddListener (func);
		}
	}

	public void RemoveScrollOnValueChanged (UnityEngine.Events.UnityAction<Vector2> func)
	{
		if (this.gameObject.GetComponent<ScrollRect> () != null) {
			this.gameObject.GetComponent<ScrollRect> ().onValueChanged.RemoveListener (func);
		}
	}
}
