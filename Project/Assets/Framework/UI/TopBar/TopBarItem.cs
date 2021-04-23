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
    public Transform iconTransform;
    protected virtual void Awake()
    {
    }

    public void FlyToBar(Vector3 startPos, Action callback = null)
    {
        StartCoroutine(FlyCorutinue(startPos,iconTransform.position,callback));
    }
    public void FlyFromBar(Vector3 endPos, Action callback = null)
    {
        StartCoroutine(FlyCorutinue(iconTransform.position,endPos,callback));
    }
    protected virtual IEnumerator FlyCorutinue(Vector3 startPos,Vector3 endPos, Action callback = null)
    {
        yield return null;
    }
}
