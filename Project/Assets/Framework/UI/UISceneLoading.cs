
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

public class UISceneLoading : BaseUI
{
    private CanvasGroup canvasGroup;

    GameObject _staticObj;
    GameObject _dynamicObj;

    public override void OnAwake()
    {
        _dynamicObj = transform.Find("root/dynamic").gameObject;
        _staticObj = transform.Find("root/static").gameObject;
        canvasGroup = transform.Find("root/dynamic").GetComponent<CanvasGroup>();

    }
    public void SetSceneChangeSequence(string curentSceneName,string targetSceneName)
    {
        if (curentSceneName == SceneName.InitScene || curentSceneName == SceneName.StartScene)
        {
            _dynamicObj.SetActive(false);
            _staticObj.SetActive(true);
        }
        else
        {
            _dynamicObj.SetActive(true);
            _staticObj.SetActive(false);
        }
    }

    Tweener fadeTweener;
    public IEnumerator FadeIn()
    {
        fadeTweener?.Kill();
        fadeTweener  = canvasGroup.DOFade(1f,0.5f);
        yield return new WaitForSeconds(0.4f);
    }
    public IEnumerator FadeOut()
    {
        fadeTweener?.Kill();
        fadeTweener = canvasGroup.DOFade(0f, 1.2f);
        yield return new WaitForSeconds(1.1f);
    }

    public override void OnStart()
    {
        canvasGroup.alpha = 1f;
    }


}
