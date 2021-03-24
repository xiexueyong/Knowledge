using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif



[DisallowMultipleComponent]
public class BackEventManager : D_MonoSingleton<BackEventManager>
{
    public bool ForbidenExit;
    List<ActionItem> actionHandlers = new List<ActionItem>();

    protected override void OnAwake()
    {
       
    }
    //protected override void OnDestroy()
    //{
    //    //Device.Inst.RemoveBackButtonCallback(Back);
    //}
    public static bool HasActionItem(Func<bool> action)
    {
        if (Inst.actionHandlers == null || Inst.actionHandlers.Count <= 0)
        {
            return false;
        }
        foreach (var item in Inst.actionHandlers)
        {
            if (item != null && item.action == action)
            {
                return true;
            }
        }
        return false;
    }
    public static bool HasActionItem(string name)
    {
        if (Inst.actionHandlers == null || Inst.actionHandlers.Count <= 0)
        {
            return false;
        }
        foreach (var item in Inst.actionHandlers)
        {
            if (item != null && item.name == name)
            {
                return true;
            }
        }
        return false;
    }

    public static void Subscribe(Func<bool> handler,int priority = int.MaxValue,string name = "")
        {
            if (handler != null)
            {
                ActionItem ai = new ActionItem(handler, priority,name);
                if (!Inst.actionHandlers.Contains(ai))
                {
                    Inst.actionHandlers.Add(ai);
                    Inst.actionHandlers.Sort((x,y)=>
                    {
                        if (x.priority < y.priority)
                        {
                            return -1;
                        }else if (x.priority == y.priority)
                        {
                            return 0;
                        }
                        else
                        {
                            return 1;
                        }
                    });
                }
            }
        }
    public static void UnSubscribe(Func<bool> handler, int priority = int.MaxValue)
    {
        if (handler != null)
        {
            ActionItem ai = new ActionItem(handler, priority);
            if (Inst.actionHandlers.Contains(ai))
            {
                Inst.actionHandlers.Remove(ai);
            }
        }
    }
    public static void UnSubscribe(string name)
    {
        if (Inst.actionHandlers.Count == 0)
        {
            return;
        }
        for (int i= Inst.actionHandlers.Count -1;i>= 0;i--)
        {
            if (Inst.actionHandlers[i].name == name)
            {
                Inst.actionHandlers.RemoveAt(i);
            }
        }
    }
    private void Update()
    {
        #if UNITY_ANDROID || UNITY_EDITOR
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Back();
                }
        #endif
    }

    public void Back()
    {
        Log("Start Back");
        for (int i = 0;i<actionHandlers.Count;i++)
        {
            Func<bool> action = actionHandlers[i].action;

            if (action != null)
            {
                bool result = false;
                try{
                    result = action.Invoke();
                }
                catch (Exception e)
                {
                    DebugUtil.LogError(string.Format("Register action {0} throw a error in BackEventManager ", actionHandlers[i].name));
                    DebugUtil.LogError(e.StackTrace);
                }
                if (result)
                {
                    Log("{0}:true,priority:{1}", actionHandlers[i].name, actionHandlers[i].priority);
                    break;
                }
                else
                {
                    Log("{0}:false,priority:{1}", actionHandlers[i].name, actionHandlers[i].priority);
                }
            }
        }
    }
    public virtual void Log(string str, params object[] args)
    {
        DebugUtil.Log("BackEvent:" + str, args);
    }

    public bool HandleBackAction()
    {
        Log("HandleBackAction");
        //if (ForbidenExit)
        //{
        //    Log("ForbidenExit");
        //    return false;

        //}
        try
        {
            Log("show exitGame");
            UIManager.Inst.ShowUI(UIModuleEnum.ExitGame);
            //            CommonPanelData data = new CommonPanelData();
            //            data.tag = -1;
            //            data.title = "&key.UI_EXIT_Title";
            //            data.message = "&key.UI_EXIT_Msg";
            //            data.displayCloseBtn = false;
            //            data.okBtnName = "&key.UI_EXIT_BUTTON_Exit";
            //            data.okListener = () =>
            //            {
            //                //退出游戏要设置notification
            //                NotificationHelper.OnApplicationPause(true, this);
            //#if UNITY_EDITOR
            //                UnityEditor.EditorApplication.isPlaying = false;
            //#else
            //            Application.Quit();
            //#endif

            //};
            //data.cancelBtnName = "&key.UI_EXIT_BUTTON_Cancel";
            //UiManager.Inst.Pop(Pop.CommonDialogPanel, UiManager.PrefabBundleName, typeof(CommonDialogPanel), data);
        }
        catch (Exception e)
        {
            Log(e.StackTrace);
        }

        return true;

    }


    private class ActionItem
    {
        public string name;
        public int priority;
        public Func<bool> action;
        public ActionItem(Func<bool> handler, int priority = int.MaxValue,string name = "")
        {
            this.name = name;
            this.action = handler;
            this.priority = priority;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ActionItem))
                return false;
            ActionItem b = (ActionItem)obj;
            return action == b.action && priority == b.priority;
        }

        public static bool operator == (ActionItem a, ActionItem b)
        {
            if ((object)a == null)
                return (object)b == null;
            return a.Equals(b);
        }

        public static bool operator !=(ActionItem a, ActionItem b)
        {
            if ((object)a == null)
                return (object)b != null;
            return !a.Equals(b);
        }
        //public override int GetHashCode()
        //{
        //    return string.Format("{0},{1}", action.ToString(), priority.ToString()).GetHashCode();
        //}

    }


#if UNITY_EDITOR
    [CustomEditor(typeof(BackEventManager))]
    public class BackEventManagerEditor : Editor
    {
        private string gapString = " : ";
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            BackEventManager self = (BackEventManager)target;

            if (self.actionHandlers == null)
                return;

            EditorGUILayout.Space();
            GUILayout.Label("Subscribed Event List : " + self.actionHandlers.Count);
            GUILayout.Label("-----------");
            foreach (var item in self.actionHandlers)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label(item.name + gapString + item.priority.ToString());
                EditorGUILayout.EndHorizontal();
            }
        }
    }
#endif


}