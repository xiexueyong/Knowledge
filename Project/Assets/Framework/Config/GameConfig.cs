using UnityEngine;
using System;

public enum VersionStatus
{
    RELEASE = 0,
    DEBUG = 1,
    LOCAL = 2
}

public class GameConfig : ScriptableObject
{

    public enum ServerType
    {
        Fotoable = 1,
        Local = 2,

    }
    public static string ConfigurationControllerPath = "Settings/GameConfig";
    private static GameConfig _instance = null;
    public static GameConfig Inst
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<GameConfig>(ConfigurationControllerPath);
            }
            return _instance;
        }
    }

    public string Appid = "";
    public string Pver = "";

    public static string ServerTypeKey = "ServerTypeKey";
    [Space(10)]
    [Header("网关地址")]
    [SerializeField]
    private ServerType _serverType = ServerType.Fotoable;
    public ServerType serverType
    {
        get
        {
#if UNITY_EDITOR
            return _serverType;
#else
            if (PlayerPrefs.HasKey(ServerTypeKey))
            {
                int st = PlayerPrefs.GetInt(ServerTypeKey);
                return (ServerType)(st);
            }
            return _serverType;
#endif

        }
        set
        {
            PlayerPrefs.SetInt(ServerTypeKey,(int)(value));
            _serverType = value;
        }
    }
    public string Fotoable_Gateway = "http://polarblast.crushgame.net:18088/pb/";// 贝塔外网gateway
    public string Local_Gateway = "http://127.0.0.1:18088/pb/";// 内网gateway
  

    //打开调试功能
    [Space(10)]
    [Header("调试开关")]
    public bool DebugEnable = false;
    [Header("网络开关")]
    public bool NetEnable = true;

    [Header("使用调试关卡")]
    public bool UseDebugLevel = false;

    [Header("激励视频成功")]
    public bool RewardSucess = false;

    [Space(10)]
    [Header("当前BuildNumber")]
    public string CurrentBuildNumber = "null";

    [Space(10)]
    [Header("隐藏引导")]
    public bool HideGuide = false;

    [Header("启动AI")]
    public bool autoPilot = false;

    [Header("连胜奖励")]
    public bool giveContinueWinBonus = true;



    //目标平台的名称
#if UNITY_ANDROID
    public static readonly string TargetPlatformName = "android";
#elif UNITY_IPHONE || UNITY_IOS
        public static readonly string TargetPlatformName = "iphone";
#else
    public static readonly string TargetPlatformName = "pc";
#endif




    #region  游戏相关的参数
    public const float defaultCamSize_const = 30;
    public static string defaultCameraSizeKey = "defaultCameraSizeKey";
    private static float _defaultCameraSize;
    public static float defaultCameraSize
    {
        get
        {
            if (_defaultCameraSize <= 0)
                return _defaultCameraSize = PlayerPrefs.GetFloat(defaultCameraSizeKey, defaultCamSize_const);
            else
                return _defaultCameraSize;
        }
        set
        {
            _defaultCameraSize = value;
            PlayerPrefs.SetFloat(defaultCameraSizeKey, value);
        }
    }

    public const float aimCamSize_const = 10;
    public static string aimCameraSizeKey = "defaultCameraSizeKey";
    private static float _aimCameraSize;
    public static float aimCameraSize
    {
        get
        {
            if (_aimCameraSize <= 0)
                return _aimCameraSize = PlayerPrefs.GetFloat(aimCameraSizeKey, aimCamSize_const);
            else
                return _aimCameraSize;
        }
        set
        {
            _aimCameraSize = value;
            PlayerPrefs.SetFloat(aimCameraSizeKey, value);
        }
    }

    #endregion

}
