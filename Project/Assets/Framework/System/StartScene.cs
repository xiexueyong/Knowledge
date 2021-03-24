using System.Collections;
using System.Collections.Generic;
using Framework.Storage;
using Framework.Asset;
using UnityEngine;

public class StartScene : MonoBehaviour
{
    private void Awake()
    {
        UIManager.Inst.StartGameImage.gameObject.SetActive(true);
    }

    void Start()
    {
       
       StartCoroutine(InitData());
    }

    IEnumerator InitData()
    {
        UIManager.Inst.Clear();
        Res.Inst.Release();
        yield return new WaitForSeconds(0.1f);

        //登陆
        LoginManager.Inst.Login();
        while (LoginManager.Inst.loginStatus == LoginManager.LoginStatus.LOGINING)
        {
            yield return null;
        }
        DataManager.Inst.SetDefaultValue();
        GamePatcher.Patcher();
        ServerDataManager.Inst.Init();
        SceneLoadManager.Inst.LoadScene(SceneName.HomeScene);
        TableBI.InitTime(LoginManager.Inst.InitStepCostTime);
    }
    private void OnDestroy()
    {
        
    }

}
