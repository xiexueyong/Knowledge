using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LoadingTipManager
{
    public LoadingTipManager()
    {
    }
    //	private static LoadingTipManager _instance;
    //	public static LoadingTipManager Instance {
    //		get {
    //			if(_instance == null){
    //				_instance = new LoadingTipManager ();
    //			}
    //			return _instance;
    //		}
    //	}
    //
    //

    private static int refCount = 0;
    private static GameObject busyPanel;
    private static GameObject busyPanelHome;

    public static void Initialize(GameObject panel, GameObject busyHome)
    {
        busyPanel = panel;
        busyPanel.SetActive(false);
    }

    public static void AddRef()
    {
        refCount++;
        if (!busyPanel.activeSelf)
        {
            busyPanel.SetActive(true);
            busyPanel.transform.SetAsLastSibling();
        }
        
    }

    public static void ReleaseRef()
    {
        if (refCount > 0)
        {
            refCount--;
        }
        if ((0 == refCount) && busyPanel.activeSelf)
        {
            busyPanel.SetActive(false);
        }
        
    }

}


