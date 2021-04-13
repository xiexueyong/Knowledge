using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using EventUtil;
using Framework.Asset;
using Framework.Storage;

public enum GameState
{
    start,
    version,
    load,
    home,
    game
}

public class GameController : S_MonoSingleton<GameController>
{
    public static bool ApplicationQuit;

    protected override void OnAwake()
    {
        StartCoroutine(Init());
    }
    private void Start()
    {
        // //取消通知
        // #if UNITY_ANDROID
        // NotifyManager.Inst.cancleAndroidSchaduleAndroidNotification();
        // #elif UNITY_IOS
        // NotifyManager.Inst.cancleIOsSchaduleAndroidNotification();
        // #endif
        // //注册通知
        // #if UNITY_ANDROID
        //         NotifyManager.Inst.schaduleAndroidNotifications();
        // #elif UNITY_IOS
        //         NotifyManager.Inst.schaduleIosNotifications();
        // #endif

    }

    public IEnumerator Init()
    {

        //支付初始化
        StorageManager.Inst.Init();
        InitDebugCanvas();

#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
         AssetBundleConfig.Inst.UseAssetBundle = true;
#endif
#if UNITY_EDITOR
        EventDispatcher.Inst.Init();
#endif
        Application.targetFrameRate = 60;
        Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;

        //初始化需要提前初始化的东西==================
        URLConfig.Init();
        SystemClock.Inst.Init();
        SoundPlay.Init();
        // AssetBundleManager初始化
        yield return AssetBundleManager.Inst.Init();
        Res.Init();
        Table.Init();
        //检查版本
        yield return AppUpdate.Update();
        yield return ConstUpdate.Update();
        // FaceBook SDK初始化
        //FBTool.init();
        EventDispatcher.TriggerEvent(EventKey.FinishGameInit);
        SceneLoadManager.Inst.LoadScene(SceneName.StartScene);

        //返回键侦听
        //BackEventManager.Subscribe(WaitingManager.Inst.Back, int.MinValue, "WaitingManager.Back");
        //BackEventManager.Subscribe(GuideManager.Inst.Back, -5, "Guide.Back");
        BackEventManager.Subscribe(UIManager.Inst.Back, 1, "CK_UIManager.Back");
        BackEventManager.Subscribe(BackEventManager.Inst.HandleBackAction, int.MaxValue, "Exit Panel");
    }

    private void InitCallback()
    {
        //if (FB.IsInitialized)
        //{
        //    // Signal an app activation App Event
        //    FB.ActivateApp();
        //    // Continue with Facebook SDK
        //}
        //else
        //{
        //    Debug.Log("Failed to Initialize the Facebook SDK");
        //}
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }

    public void Restart()
    {
       
    }
   
    //初始化DebugCanvas
    private void InitDebugCanvas()
    {
        if(GameConfig.Inst.DebugEnable)
        {
            SRDebug.Init();
        }
    }
    public void StartGame()
    {
    }
    public void Update()
    {

    }
    void OnDestroy()
    {
        //BackEventManager.Subscribe(WaitingManager.Inst.Back, int.MinValue, "WaitingManager.Back");
        //BackEventManager.UnSubscribe(GuideManager.Inst.Back, -5);
        BackEventManager.UnSubscribe(UIManager.Inst.Back, 1);
        BackEventManager.UnSubscribe(BackEventManager.Inst.HandleBackAction, int.MaxValue);
    }
    private void OnApplicationPause(bool pauseStatus)
    {
       
    }
    void OnApplicationFocus()
    {

    }
    private void OnApplicationQuit()
    {
        ApplicationQuit = true;
        EventDispatcher.TriggerEvent(EventKey.ApplicationQuit);
    }



    public enum UpdateType
    {
        NoUpgrade = 0,//不更新
        Upgrade = 1,//强制更新
        NotForeUpgrade = 2//非强制更新
    }
    public class UpgradeInfo
    {
        //0 不更新,1 更新
        public int appUpdateType;
        //0 不更新,1 更新
        public int assetUpdateType;
        public string appUrl;
        public string assetUrl;

        public string appVersion;
        public string assetVersion;

        public string noticeTitle;
        public string noticeContent;
    }
}