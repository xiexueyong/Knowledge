using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using SimpleJSON;
using System;
using Framework;
using Framework.Storage;

/**  *   * 此类用户注册、登录  * */


public class LoginManager : D_MonoSingleton<LoginManager> {

    public string Session;

    public enum LoginStatus
    {
        LOGOUT,//未登陆
        LOGINING,//正在登陆
        LOGINSUCESS,//登陆
        LOGINFAIL,//登陆失败
        TOKEN_EXPIRED,//Session过期
    }
    public Queue<HttpPacket> needSessionHttpPackets;

    public LoginStatus loginStatus = LoginStatus.LOGOUT;

    /// <summary>
    /// 登录流程是否已经走完了
    /// </summary>
    /// <returns>true：走完了</returns>
    public bool isLoginFinished()
    {
        return loginStatus == LoginStatus.LOGINSUCESS || loginStatus == LoginStatus.LOGINFAIL ||
               loginStatus == LoginStatus.TOKEN_EXPIRED;
    }
    
    protected override void OnAwake()
    {
        base.OnAwake();
        needSessionHttpPackets = new Queue<HttpPacket>();
        EventUtil.EventDispatcher.AddEventListener<HttpPacket> (EventKey.SessionExpiredOrEmpty, OnSessionExpiredOrEmpty);
    }

    public void OnSessionExpiredOrEmpty(HttpPacket httpPacket)
    {
        //if (!needSessionHttpPackets.Contains(httpPacket))
        //{
        //    needSessionHttpPackets.Enqueue(httpPacket);
        //}

        if (loginStatus != LoginStatus.LOGINING)
        {
            loginStatus = LoginStatus.TOKEN_EXPIRED;
            Login();
        }
    }
    private void ReSendNeedSessionHttpPacket()
    {
        while (needSessionHttpPackets != null && needSessionHttpPackets.Count > 0)
        {
            HttpRequestTool.SendMessage(needSessionHttpPackets.Dequeue());
        }
    }
    public string fbid
    {
        get
        {
            return StorageManager.Inst.GetStorage<StorageAccountInfo>().FbId;
        }
        set
        {
            StorageManager.Inst.GetStorage<StorageAccountInfo>().FbId = value;
            PlayerPrefs.SetString("fbid", value);
        }
    }

    //public void LoginOld( Action<bool> callback = null)
    //{
    //    //UIManager.Inst.NetMask(true);
    //    loginStatus = LoginStatus.LOGINING;
    //    Dictionary<string, string> headerDic = new Dictionary<string, string>();
    //    string timeStr = ((int)SystemClock.Now).ToString();
    //    headerDic["appid"] = GameConfig.Inst.Appid;
    //    headerDic["time"] = timeStr;
    //    headerDic["devId"] = DeviceHelper.GetDeviceId();
    //    headerDic["pver"] = GameConfig.Inst.Pver;
    //    headerDic["sign"] = Framework.Asset.AssetUtils.EncryptWithMD5(GameConfig.Inst.Appid + timeStr + Header.ClientKey);

    //    LoginBody body = new LoginBody(URLConfig.Login);
    //    body.type = "guest";

    //    //facebook的相关数据
    //    if (!string.IsNullOrEmpty(fbid))
    //    {
    //        body.id = fbid;
    //        body.type = "facebook";
    //        body.thirdtoken = "fbtoken_12345";
    //    }


    //    body.deviceId = DeviceHelper.GetDeviceId();
    //    body.appid = GameConfig.Inst.Appid;
    //    body.time = timeStr;
    //    body.sign = headerDic["sign"];
    //    body.pver = headerDic["pver"];
    //    body.device = DeviceInfo.deviceInfo.ToString();

    //    Header header = Header.GetHeader(headerDic);
    //    HttpRequestTool.SendMessage(
    //        body, 
    //        (x) => {
    //            Debug.Log("Login Sucess :" + x);

    //            JSONNode respons = JSONNode.Parse(x);
    //            string session = respons["session"];
    //            int code = respons["code"];
    //            int uid = respons["userInfo"]["gameUid"];//respons["pid"];

    //            string type = respons["type"];
    //            string id = respons["id"];

    //            DebugUtil.Log("=========serverUId:"+uid);
    //            //UIManager.Inst.NetMask(false);
    //            if (!string.IsNullOrEmpty(session))
    //                Session = session;
    //            if(code == 0 && uid > 0)
    //                StartCoroutine(OnReceiveServerData(respons["playdata"],uid,id,type,callback));
    //            else
    //                loginStatus = LoginStatus.LOGINFAIL;
    //        },
    //        (x) => {
    //            //UIManager.Inst.NetMask(false);
    //            Debug.LogError("Login Fail :" + x);
    //            if (callback != null)
    //                callback.Invoke(false);
    //            loginStatus = LoginStatus.LOGINFAIL;
    //        }, 
    //        header,
    //        false);
    //}

    public LoginBody GetLoginBody()
    {
        LoginBody body = new LoginBody(URLConfig.Login);
        //facebook的相关数据
        if (!string.IsNullOrEmpty(fbid))
        {
            body.id = fbid;
            body.type = "facebook";
        }

        body.deviceId = DeviceInfo.deviceInfo.deviceId;
        body.ip = DeviceHelper.GetIP(DeviceHelper.ADDRESSFAM.IPv4);
        body.country = DeviceInfo.deviceInfo.devicecountry;
        body.appBuildNum = DeviceInfo.deviceInfo.appvercode;
        return body;
    }
    public void Login(Action<bool> callback = null)
    {
        loginStatus = LoginStatus.LOGINING;
        Dictionary<string, string> headerDic = new Dictionary<string, string>();
        string timeStr = ((int)SystemClock.Now).ToString();

        LoginBody body = GetLoginBody();
        Header header = Header.GetHeader(headerDic);
        HttpRequestTool.SendMessage(
        body,
        (x) => {
            Debug.Log("Login Sucess :" + x);

            JSONNode respons = JSONNode.Parse(x);
            ParseLoginResponse(respons, callback);
        },
        (x) => {
            Debug.LogError("Login Fail :" + x);
            if (callback != null)
                callback.Invoke(false);
            loginStatus = LoginStatus.LOGINFAIL;
        },
        header,
        false);
    }
    void ParseLoginResponse(JSONNode respons,Action<bool> callback = null)
    {
        if (respons == null)
        {
            loginStatus = LoginStatus.LOGINFAIL;
            return;
        }
        string session = "debug_session";
        string uid = respons["userInfo"]["gameUid"];
        DebugUtil.Log("=========serverUId:" + uid);
        if (!string.IsNullOrEmpty(session))
            Session = session;
        if (!string.IsNullOrEmpty(uid))
            StartCoroutine(OnReceiveServerData(respons["playdata"], uid, callback));
        else
            loginStatus = LoginStatus.LOGINFAIL;
    }

    private IEnumerator OnReceiveServerData(string playData, string uid, Action<bool> callback = null)
    {
        if (!string.IsNullOrEmpty(playData))
        {
            JSONNode storageJson = JSONNode.Parse(playData);
            if (storageJson != null && storageJson["StorageAccountInfo"] != null)
            {
                int storageVersion = storageJson["StorageAccountInfo"]["_StorageVersion"];
                int storageId = storageJson["StorageAccountInfo"]["_StorageId"];
                string storageDevId = storageJson["StorageAccountInfo"]["_DevId"];

                int localStorageVersion = StorageManager.Inst.GetStorage<StorageAccountInfo>().StorageVersion;
                int localStorageId = StorageManager.Inst.GetStorage<StorageAccountInfo>().StorageId;
                string localDevid = StorageManager.Inst.GetStorage<StorageAccountInfo>().DevId;

                if (string.IsNullOrEmpty(localDevid))
                {
                    //本地没有数据，自动使用服务端数据
                    StorageManager.Inst.SetData(playData, false);
                }
                else if (storageVersion > localStorageVersion)
                {
                    //服务端版本更高，则用服务端数据
                    StorageManager.Inst.SetData(playData, false);
                }
                else
                {
                    //使用本地数据
                }
                
                
                
                // else if (localDevid == storageDevId)
                // {
                //     //自动使用本地数据
                // }
                // else
                // {
                //     //用户选择是否覆盖,（数据设备号不一样）
                //     //UISelectGameData uiSelectGameData = UIManager.Inst.ShowUI(UIModuleEnum.UISelectGameData) as UISelectGameData;
                //     //while (!UISelectGameData.close)
                //     //{
                //     //    yield return null;
                //     //}
                //
                //     //try
                //     //{
                //     //    if (UISelectGameData.selectedServer)
                //     //    {
                //     //        StorageManager.Inst.SetData(playData);   
                //     //    }
                //     //    DebugUtil.Log("=========user server UId:" + StorageManager.Inst.GetStorage<StorageAccountInfo>().Uid);
                //     //}
                //     //catch (Exception e)
                //     //{
                //     //    DebugUtil.LogError("LoginManager.OnReceiveServerData fail: " + playData);
                //     //}
                // }

                SetDevidAndUid(uid);
            }
        }
        else
        {
            SetDevidAndUid(uid);
        }

        TableBI.Login(SystemClock.Now);
        loginStatus = LoginStatus.LOGINSUCESS;
        if (callback != null)
            callback.Invoke(true);
        ReSendNeedSessionHttpPacket();
        yield break;
    }

    private void SetDevidAndUid(string uid)
    {
        if(StorageManager.Inst.Inited)
        {
            StorageManager.Inst.GetStorage<StorageAccountInfo>().DevId = Framework.DeviceInfo.deviceInfo.deviceId;
            if (!string.IsNullOrEmpty(uid))
                StorageManager.Inst.GetStorage<StorageAccountInfo>().Uid = uid;
        }
    }

    public void LogoutFB()
    {

        UIManager.Inst.ShowMessage(LanguageTool.Get("fb_logout_success"));
        // fb退出登录
        //FBTool.logout();
        // 清除fb数据
        fbid = null;
        // 清除玩家数据
        PlayerPrefs.DeleteAll();
        // 重启游戏
        SceneLoadManager.Inst.LoadScene(SceneName.StartScene);
        
    }
    public void BindFB (Action bindSucessCallback = null)
    {
        //FBTool.login((_fbtoken, _fbid) => {

        //    if (!string.IsNullOrEmpty(_fbtoken) && !string.IsNullOrEmpty(_fbid))
        //    {
        //        BindFBActual(_fbtoken, _fbid, bindSucessCallback);
        //    }
        //});
        //TableBI.BindAccount(1, fbid);
    }

    public void BindFBActual(string _fbtoken,string _fbid, Action bindSucessCallback = null)
    {
        BindBody body = new BindBody(URLConfig.BindUser);
        body.type = "facebook";
        body.id = _fbid;
        body.thirdtoken = _fbtoken;
        body.gameUid = StorageManager.Inst.GetStorage<StorageAccountInfo>().Uid;

        HttpRequestTool.SendMessage(
            body,
            (x) => {
                JSONNode respons = JSONNode.Parse(x);
                int code = respons["code"];
                if (code == 0)
                {
                    fbid = _fbid;
                    Debug.Log("Bind Success 1:" + x);
                    if (bindSucessCallback != null)
                        bindSucessCallback();
                    EventUtil.EventDispatcher.TriggerEvent(EventKey.FB_Bind_Sucess);
                    UIManager.Inst.ShowMessage(LanguageTool.Get("bind_fb_success"));
                    //弹出绑定成功面板
                    //UIManager.Inst.ShowUI(UIModuleEnum.UIFacebookTips, FacebookTipType.Login_Suc);
                }
                else if (code == -10008)
                {
                    Debug.Log("Bind Success 2 :" + "该fb之前已经绑定过当前游戏账号了，无需再次操作");
                    UIManager.Inst.ShowMessage(LanguageTool.Get("bind_fb_success"));
                    fbid = _fbid;
                    //弹出绑定失败面板
                    //UIManager.Inst.ShowUI(UIModuleEnum.UIFacebookTips, FacebookTipType.Login_Fail);
                }
                else if (code == -10010)
                {
                    Debug.Log("Bind Success 3 :" + "找到fb之前绑定过的游戏进度，即将切换到该游戏进度");
                    UIManager.Inst.ShowMessage(LanguageTool.Get("fb_bind_account_reloading"));
                    // 清除fb数据
                    fbid = _fbid;
                    // 清除本地缓存数据
                    PlayerPrefs.DeleteAll();
                    //重启游戏
                    // GameController.Inst.Restart();
                    SceneLoadManager.Inst.LoadScene(SceneName.StartScene);
                }
            },
            (x) => {
                Debug.LogError("Bind Fail :" + x);
            },null,true
            );
    }
    
    private DateTime lastRecordTime = SystemClock.StartTime;
    private double lastStepTime = 0;
    private Dictionary<string, string> _initStepCostTime = new Dictionary<string, string>();
    public Dictionary<string, string> InitStepCostTime
    {
        get => _initStepCostTime;
    }
}