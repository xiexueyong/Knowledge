//using UnityEngine;
//using UnityEngine.SceneManagement;
//using System.Collections;
//using AsyncOperation = UnityEngine.AsyncOperation;
//using EventUtil;
//using System;
//using System.Collections.Generic;
//using Framework.Asset;
//using Framework.Storage;
//using Framework.Tables;
//using Spine;
//using Spine.Unity;
//using Object = System.Object;

//public class SceneLoadManager_old : S_MonoSingleton<SceneLoadManager_old>
//{
//    private AsyncOperation m_SceneAsyncOperation;

//    private float m_fCurrentProgressValue = 0;

//    private float m_fToProcess;

//    private UILoading m_UILoadingScript;
//    [SerializeField]
//    private string m_CurrentSceneName;


//    private Dictionary<string, SceneTransitionAnim> dic;
//    [SerializeField]
//    private int m_EnterMapSceneCount = 0;
//    private WaitForSeconds waitForSeconds;

//    public string LastSceneName;

//    protected override void OnAwake()
//    {
//        base.OnAwake();
//        waitForSeconds = new WaitForSeconds(.6f);
//        m_CurrentSceneName = SceneName.InitScene;
//        LastSceneName = m_CurrentSceneName;
//        dic = new Dictionary<string, SceneTransitionAnim>()
//        {
//            {SceneName.StartScene, new SceneTransitionAnim("EnterGameAni","EnterGameAni","end2","open") },
//            {SceneName.ShootScene,new SceneTransitionAnim("EnterGameAni","EnterGameAni",null,"end1") },
//           // {SceneName.Login,new SceneTransitionAnim("EnterGameAni","EnterGameAni","end2","end1") },
//         //   {SceneName.Restart,new SceneTransitionAnim("EnterGameAni","EnterGameAni","end2","end1") },

//        };
//    }


//    /// <summary>
//    /// 加载场景的入口
//    /// </summary>
//    /// <param name="sceneName">场景名字</param>
//    /// <param name="callback"></param>
//    public void LoadScene(string sceneName, Action callback)
//    {
//        string tActiveSceneName = GetActiveSceneName();
//       // StartCoroutine(LoadAsyTargetScene(sceneName, callback));
//       ChangeScene(sceneName);
//    }

//    /// <summary>
//    /// 加载目标场景
//    /// </summary>
//    /// <param name="sceneName">目标场景</param>
//    /// <returns></returns>
//    private IEnumerator LoadAsyTargetScene(string sceneName, Action callback)
//    {
//        m_UILoadingScript = UIManager.Inst.ShowUI<UILoading>(UIModuleEnum.UILOADING);
//        m_SceneAsyncOperation = SceneManager.LoadSceneAsync(SceneName.IntermediaScene);
//        m_SceneAsyncOperation.allowSceneActivation = false;

//        m_UILoadingScript.FadeIn(0.2f, 1, () =>
//        {
//            if (callback != null) callback.Invoke();
//            m_SceneAsyncOperation.allowSceneActivation = true;
//        });
//        //等待中间场景
//        while (!m_SceneAsyncOperation.isDone) {
//            yield return null;
//        }

//        //加载目标场景
//        m_SceneAsyncOperation = SceneManager.LoadSceneAsync(sceneName);
//        while (!m_SceneAsyncOperation.isDone) {
//            yield return null;
//        }

//        m_UILoadingScript.FadeOut(0.3f);
//    }


//    private void OnEnable()
//    {
//        SceneManager.sceneLoaded += OnSceneFinishedLoading;
//        SceneManager.sceneUnloaded += SceneUnload;
//    }


//    private void OnDisable()
//    {
//        SceneManager.sceneLoaded -= OnSceneFinishedLoading;
//        SceneManager.sceneUnloaded -= SceneUnload;
//    }

//    /// <summary>
//    /// 场景未加载
//    /// </summary>
//    /// <param name="scene"></param>
//    public void SceneUnload(Scene scene)
//    {
//        EventDispatcher.TriggerEvent(EventKey.SceneUnload, scene.name);
//    }


//    /// <summary>
//    /// 场景完成加载
//    /// </summary>
//    /// <param name="scene"></param>
//    /// <param name="mode"></param>
//    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
//    {
       
//        if (scene.name == SceneName.TestScene)
//        {
//            Res.Inst.Release();
//            m_CurrentSceneName = SceneName.TestScene;
//        }

//    }

   


//    /// <summary>
//    /// 同步加载场景
//    /// </summary>
//    /// <param name="sceneName"></param>
//    public void LoadSceneSync(string sceneName)
//    {
//        SceneManager.LoadScene(sceneName);
//    }



//    /// <summary>
//    /// 异步加载不需要进度条
//    /// </summary>
//    /// <param name="sceneName"></param>
//    /// <param name="loadSceneMode"></param>
//    public void LoadSceneQuickAsync(string sceneName, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
//    {
//        if (m_SceneAsyncOperation != null && !m_SceneAsyncOperation.isDone)
//            return;
//        m_SceneAsyncOperation = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
//    }

//    /// <summary>
//    /// 获取活动场景
//    /// </summary>
//    /// <returns></returns>
//    public string GetActiveSceneName()
//    {
//        return SceneManager.GetActiveScene().name;
//    }

//    public void ChangeScene(string sceneName, params Object []objs)
//    {
//        UIManager.Inst.MaskUI(true);
//        StartCoroutine(LoadAsyTargetScene(sceneName,objs));
//    }

//    private IEnumerator LoadAsyTargetScene(string sceneName, params Object[] objs)
//    {
//         LastSceneName = m_CurrentSceneName;
//         if (sceneName == SceneName.GamePlayScene)
//         {
//             SetTransitonAnimName(objs);
//         }
//        yield return null;
//        //播放进入的过渡动画
//        if (sceneName == SceneName.Restart)
//        {
//            m_UILoadingScript = UIManager.Inst.ShowUI<UILoading>(UIModuleEnum.UILOADING);
//            m_UILoadingScript.Reset();
//            // m_UILoadingScript.FadeIn(0.3f, 1);

//        }
//        else if (LastSceneName == SceneName.Restart && sceneName == SceneName.MapScene)
//        {
//            m_UILoadingScript = UIManager.Inst.ShowUI<UILoading>(UIModuleEnum.UILOADING);
//            m_UILoadingScript.Reset();
//        }
//        else
//        {
//             PlaySceneTransitionAnim(LastSceneName, false, LastSceneName);

//        }
//        //if (transitionAnim == null)
//        //{
//        //    // UICommonLogic.Instance.ShowBlackScreen(0.5f,1f,null);
//        //    //Debug.Log("213456");
//        //    m_UILoadingScript = UIManager.Inst.ShowUI<UILoading>(UIModuleEnum.UILOADING);
//        //    m_UILoadingScript.FadeIn(0.3f, 1);
//        //}
//        yield return waitForSeconds;
//        //加载中间场景
//        m_SceneAsyncOperation = SceneManager.LoadSceneAsync(SceneName.IntermediaScene,LoadSceneMode.Additive);

//        while (!m_SceneAsyncOperation.isDone)
//        {
//            yield return null;
//        }
        
//        UIManager.Inst.DestroyAllUI();
//        //卸载上一个场景
//        //if (LastSceneName == SceneName.MapScene)
//        //{
//        //    PopStackManager.Inst.unRegisterUIHidenEvent();
//        //    CutsceneManager.StopAll();
//        //    MapManager.ExitMap();
//        //    Res.Inst.Release();
//        //}
//        //else if (LastSceneName == SceneName.GamePlayScene)
//        //{
//        //    PlayEffectManager.Release();
//        //    LevelManager.Instance.UnLoadLevel();
//        //    Res.Inst.Release();

//        //}
//        //else if(LastSceneName == SceneName.Restart)
//        //{
//        //    Res.Inst.Release();
//        //}

//        yield return waitForSeconds;

//        yield return SceneManager.UnloadSceneAsync(LastSceneName);
//        EventDispatcher.TriggerEvent(EventKey.SceneUnload, LastSceneName);
//        //加载目标场景
//        yield return StartCoroutine(LoadSceneASyn(sceneName, objs));

       

//        m_CurrentSceneName = sceneName;
      
//        EventDispatcher.TriggerEvent(EventKey.SceneFinishedLoading, m_CurrentSceneName);

//    }

//    /// <summary>
//    /// 异步加载场景
//    /// </summary>
//    /// <returns></returns>
//   private IEnumerator LoadSceneASyn(string targetSceneName,params Object[]objs)
//    {
//        // SceneManager.SetActiveScene(SceneManager.GetSceneByName(SceneName.IntermediaScene));

//        m_SceneAsyncOperation = SceneManager.LoadSceneAsync(targetSceneName);
//        // m_SceneAsyncOperation.allowSceneActivation  =false;
//        while (!m_SceneAsyncOperation.isDone)
//        {
//            //if (m_SceneAsyncOperation.progress < 0.9f)
//            //{
//            //    m_fToProcess = m_SceneAsyncOperation.progress;
//            //}
//            //else
//            //{
//            //    m_fToProcess = 1;
//            //}

//            //if (m_fCurrentProgressValue < m_fToProcess)
//            //{
//            //    m_fCurrentProgressValue += 0.05f;
//            //}

//            //if (null != m_UILoadingScript)
//            //{
//            //    m_UILoadingScript.SetProgressValue(m_fCurrentProgressValue);
//            //}

//            //if (m_fCurrentProgressValue > 0.995f)
//            //{
//            //    PlaySceneTransitionAnim(targetSceneName, true);
//            //    UIManager.Inst.MaskUI(false);
//            //    m_SceneAsyncOperation.allowSceneActivation = true;
//            //    // m_SceneAsyncOperation.allowSceneActivation = true;
//            //}


//            yield return null;
//        }

//        //进入目标场景的动画

//        if(targetSceneName != SceneName.Restart)
//            PlaySceneTransitionAnim(targetSceneName, true, LastSceneName);
       
//        UIManager.Inst.MaskUI(false);


//    }

//    string anim = "green";
//    private SceneTransitionAnim PlaySceneTransitionAnim(string sceneName, bool isEnter,string lastScene =null)
//    {
//        SceneTransitionAnim t = null;



//        if (!string.IsNullOrEmpty(lastScene) && (lastScene == SceneName.Login || lastScene == SceneName.Restart))
//        {
//            if (!isEnter)
//            {
//                m_UILoadingScript = UIManager.Inst.ShowUI<UILoading>(UIModuleEnum.UILOADING);
//                m_UILoadingScript.FadeIn(0.3f, 1);
//                return null;
//            }
//            else
//            {
//                m_UILoadingScript = UIManager.Inst.ShowUI<UILoading>(UIModuleEnum.UILOADING);
//                m_UILoadingScript.FadeIn(0.3f, 0, (() => { UIManager.Inst.HideUI("UILoading", true); }));
//                return null;
//            }
//        }

//        if (dic.TryGetValue(sceneName, out t))
//        {
            
//            if (isEnter)
//            {
//                if (!string.IsNullOrEmpty(t.EnterAnim))
//                {
//                    TransitionManager.Inst.PlayTransition(t.EnterPrefab, t.EnterAnim, null, true,anim);
//                }


//            }
//            else if (!isEnter)
//            {
//                if (!string.IsNullOrEmpty(t.ExitAnim))
//                {
//                     TransitionManager.Inst.PlayTransition(t.ExitPrefab, t.ExitAnim, null, false,anim);
//                }

//            }
//        }


//        return t;

//    }

//    private void SetTransitonAnimName(object[] objs)
//    {
//        if (objs.Length == 0)
//        {
//            return;
//        }

//        //PlayType type = (PlayType) objs[0];

//        //switch (type)
//        //{
//        //    case PlayType.COMMON:
//        //        int level = DataManager.Inst.userInfo.Level;
//        //        TableLevelDegree degree = Table.LevelDegree.Get(level);
//        //        if (degree != null)
//        //        {
//        //            anim = degree.Difficulty == 1 ? "purple" : "red";
//        //        }
//        //        break;
//        //    case PlayType.DAILY_PUZZLE:
//        //        break;
//        //    case PlayType.ACTIVITY:
//        //        break;
//        //    default:
//        //        anim = "green";
//        //        break;
//        //}

        
//    }
//}
