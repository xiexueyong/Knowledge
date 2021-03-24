using UnityEngine;
using System.Collections;
using System;

public class CoroutineManager : D_MonoSingleton<CoroutineManager> {
	/// <summary>
	/// 全局协程入口
	/// </summary>
	public Coroutine StartCoroutine(IEnumerator routine)
	{
		return StartCoroutine (routine, this);
	}

	/// <summary>
	/// 协程（该函数可根据需要传入特定的MonoBehaviour）
	/// </summary>
	public Coroutine StartCoroutine(IEnumerator routine, MonoBehaviour mono)
	{
		return mono.StartCoroutine (routine);
	}

	public void DelayCall(Action action,float delay)
    {
		StartCoroutine(_delayCall(action,delay));
    }
	private IEnumerator _delayCall(Action action, float delay)
    {
		yield return new WaitForSeconds(delay);
		action.Invoke();
    }
}
