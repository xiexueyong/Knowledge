using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SceneMask : MonoBehaviour
{
    public float changeTime = 0.5f;
    public enum Phase
    {
        None,
        Showing,
        Hiding,
        Show,
        Hide
    }

    private Coroutine _hideCoroutine;
    private Coroutine _showCoroutine;

    private Phase _phase;
    private Phase _status;
    public CanvasGroup CanvasGroup;
    public float Show(bool immediately= false)
    {
        if (_status == Phase.Show)
        {
            return 0;
        }
        
        if (_phase == Phase.Showing)
        {
            return changeTime;
        }
        
        if (_phase == Phase.Hiding)
        {
            StopCoroutine(_hideCoroutine);
        }
        
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        CanvasGroup.alpha = 0;
        _showCoroutine = StartCoroutine(ShowIEnumerator());
        return changeTime;
    }

    public IEnumerator ShowIEnumerator()
    {
        _phase = Phase.Showing;
        _status= Phase.None;
        CanvasGroup.DOFade(1f, changeTime);
        yield return new WaitForSeconds(changeTime);
        _phase = Phase.None;
        _status= Phase.Show;
    }
    
    public float Hide(bool immediately= false)
    {
        if (_status == Phase.Hide)
        {
            return 0;
        }
        
        if (!gameObject.activeSelf)
        {
            return 0f;
        }
        if (_phase == Phase.Hiding)
        {
            return changeTime;
        }
        
        if (_phase == Phase.Showing)
        {
            StopCoroutine(_showCoroutine);
        }
        
        
        _hideCoroutine = StartCoroutine(HideIEnumerator());
        return changeTime;
    }
    public IEnumerator HideIEnumerator()
    {
        _phase = Phase.Hiding;
        _status= Phase.None;
        CanvasGroup.DOFade(0f, changeTime);
        yield return new WaitForSeconds(changeTime);
        gameObject.SetActive(false);
        _phase = Phase.None;
        _status= Phase.Hide;
        
    }
}
