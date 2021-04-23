using System;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using EventUtil;
using Framework.Asset;
using Framework.Storage;
using Framework.Utils;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class CoinTopBarItem : TopBarItem
{
    public bool isHomeBar = false;
    private Text numText;
    private int _coin = -1;
    private int _tempCoin = -1;

    protected override void Awake()
    {
        base.Awake();
        numText = transform.Find("CountText").GetComponent<Text>();
        _tempCoin = _coin = DataManager.Inst.userInfo.Coins;
        numText.text = _coin.ToString();
        EventDispatcher.AddEventListener<int,int,int>(StorageUserInfo.StorageUserInfo_Change_GoodsCount, coinChange);
    }

    private Tweener _tweener;
    private void coinChange(int coin_goods_id,int oldValue,int newValue)
    {
        if (coin_goods_id == Table.GoodsId.Coin && numText != null && _coin != newValue)
        {
            _coin = newValue;
            if (_tweener != null)
            {
                DOTween.Kill(_tweener);
            }
            _tweener = DOTween.To(() => _tempCoin, x => { _tempCoin = x;numText.text = _tempCoin.ToString(); }, _coin, 1f);
        }
            
    }
    
    protected override IEnumerator FlyCorutinue(Vector3 startPos,Vector3 endPos, Action callback = null)
    {
        yield return null;
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

    protected void onClick()
    {
        if (topBarType == TopBarType.Coin)
        {
            if (isHomeBar)
            {
               // uiHome?.homeTab?.SelectTab(0);
            }
            else
            {
                UIManager.Inst.ShowUI(UIName.UIShop, false, "popup");
            }
        }
    }
}
