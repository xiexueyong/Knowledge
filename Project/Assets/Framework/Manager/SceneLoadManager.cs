using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using AsyncOperation = UnityEngine.AsyncOperation;
using EventUtil;
using System;
using System.Collections.Generic;
using Framework.Asset;
using Framework.Storage;
using Framework.Tables;
using Spine;
using Spine.Unity;
using Object = System.Object;

public class SceneLoadManager : S_MonoSingleton<SceneLoadManager>
{
    private AsyncOperation m_SceneAsyncOperation;

    [SerializeField]
    private string m_CurrentSceneName;//当前场景名称
    public string LastSceneName;//前一个场景名称


    [SerializeField]
    private int m_EnterMapSceneCount = 0;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_CurrentSceneName = SceneName.InitScene;
        LastSceneName = m_CurrentSceneName;
    }


    /// <summary>
    /// 加载场景的入口
    /// </summary>
    /// <param name="targetSceneName">场景名字</param>
    public void LoadScene(string targetSceneName)
    {
        //清理UI
        UIManager.Inst.Clear();

        StartCoroutine(LoadSceneAsync(targetSceneName));
    }

    /// <summary>
    /// 加载目标场景
    /// </summary>
    /// <param name="targetSceneName">目标场景</param>
    /// <returns></returns>
    private IEnumerator LoadSceneAsync(string targetSceneName)
    {
        UISceneLoading sceneLoading = UIManager.Inst.ShowUI(UIModuleEnum.UISceneLoading) as UISceneLoading;
        sceneLoading.SetSceneChangeSequence(m_CurrentSceneName, targetSceneName);
        yield return sceneLoading.FadeIn();


        m_SceneAsyncOperation = SceneManager.LoadSceneAsync(SceneName.IntermediaScene);
        m_SceneAsyncOperation.allowSceneActivation = true;


        //yield return new WaitForSeconds(3f);
        //等待中间场景
        while (!m_SceneAsyncOperation.isDone) {
            yield return null;
        }
        //加载目标场景
        m_SceneAsyncOperation = SceneManager.LoadSceneAsync(targetSceneName);

        while (!m_SceneAsyncOperation.isDone) {
            yield return null;
            //m_SceneAsyncOperation.progress
        }

        m_CurrentSceneName = targetSceneName;
        yield return sceneLoading.FadeOut();
        UIManager.Inst.CloseUI(UIModuleEnum.UISceneLoading);
    }


    /// <summary>
    /// 同步加载场景
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadSceneSync(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}

public class SceneTransitionAnim
{
    public string EnterPrefab;
    public string ExitPrefab;
    public string EnterAnim;
    public string ExitAnim;

    public SceneTransitionAnim()
    {
    }

    public SceneTransitionAnim(string enterPrefab, string exitPrefab, string enterAnim, string exitAnim)
    {
        EnterPrefab = enterPrefab;
        ExitPrefab = exitPrefab;
        EnterAnim = enterAnim;
        ExitAnim = exitAnim;
    }
}