using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Asset;
using UnityEngine;
using UnityEngine.UI;

public class TopBarManager : S_MonoSingleton<TopBarManager>
{
    private Transform topbarLayer;
    private bool Inited = false;

    private Dictionary<TopBarType, TopBarItem> topbarItems;
    protected override void OnAwake()
    {

    }

    private void InitContent()
    {
        if (!Inited)
        {
            topbarItems = new Dictionary<TopBarType, TopBarItem>();
            GameObject content = Res.LoadResource<GameObject>("Prefab/UI/TopBarMain");
            content.transform.SetParent(transform.Find("Canvas/Tips"),false);
            Inited = true;

            TopBarItem[] items = transform.GetComponentsInChildren<TopBarItem>();
            foreach (var item in items)
            {
                item.gameObject.SetActive(false);
                topbarItems.Add(item.topBarType, item);
            }
        }
    }

    public void ShowTopBar(TopBarType topBarType)
    {
        InitContent();
        foreach (var item in topbarItems)
        {
            int r = ((int)topBarType & (int)item.Key);
            item.Value.gameObject.SetActive(r > 0);
        }
    }
    public void HideTopBar(TopBarType topBarType)
    {
        if (topbarItems != null && topbarItems.Count > 0)
        {
            foreach (var item in topbarItems)
            {
                item.Value.gameObject.SetActive(false);
            }
        }
    }


    public void Fly(TopBarType topBarType,Vector3 startPos,Action callback = null)
    {
        if (topbarItems.ContainsKey(topBarType))
        {
            bool _oldActive = topbarItems[topBarType].gameObject.activeSelf;
            topbarItems[topBarType].gameObject.SetActive(true);
            topbarItems[topBarType]?.Fly(
                startPos,
                ()=> {
                    topbarItems[topBarType].gameObject.SetActive(_oldActive);
                    callback?.Invoke();
                }
            );
        }
            
    }


}
