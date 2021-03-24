using System;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using Framework.Asset;
using Framework.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class TopBarItem : MonoBehaviour
{
    [EnumToggleButtons]
    public TopBarType topBarType;

    protected Button button;
    protected Text numText;
    protected Transform iconTransform;

    protected virtual void Awake()
    {
        iconTransform = transform.Find("icon");
        button = transform.GetComponent<Button>();
        numText = transform.Find("numText")?.GetComponent<Text>();
        button?.onClick.AddListener(onClick);
       
    }
    public void setNum(int count)
    {
        if(numText != null)
            numText.text = count.ToString();
    }
    protected virtual void onClick()
    {
        if (topBarType == TopBarType.Coin)
        {
            //打开金币面板
        }

    }

    public void Fly(Vector3 startPos, Action callback = null)
    {
        StartCoroutine(FlyCorutinue(startPos,callback));
    }
    private IEnumerator FlyCorutinue(Vector3 startPos, Action callback = null)
    {
        yield return null;
        Vector3 endPos = iconTransform.position;
        Vector3[] points = { startPos, Utils.GetMiddlePoint(startPos, endPos, 0.2f), endPos };
        GameObject flyCoin = Res.LoadResource<GameObject>("Prefab/Framework/Animation/FlyCoin");
        flyCoin.transform.SetParent(transform.parent.parent);
        flyCoin.transform.localScale = Vector3.one;
        flyCoin.transform.position = startPos;

        TweenerCore<Vector3, Path, PathOptions> flyTween = flyCoin.transform.DOPath(points, 1f, PathType.CatmullRom).SetEase(Ease.InOutCubic).OnComplete(() =>
        {
            Res.Recycle(flyCoin.gameObject);
            callback?.Invoke();
        });
    }

}
