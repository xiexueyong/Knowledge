using Framework.Asset;
using System.Collections.Generic;
using Framework.Storage;
using SimpleJSON;

/// <summary>
/// 登陆
/// </summary>
public class LoginBody : Body
{
    public LoginBody(string name, string url = null) : base(name, url)
    {

    }
   
    public string type; //平台类型
    public string id;//平台账号Id
    public string deviceId;//设备id
    public string thirdtoken; //平台token

    public string appid;
    public string time;
    public string sign;
    public string pver;
    public string device;

    public string ip; //设备获取到的ip
    public string country; //设备国家
    public int appBuildNum;
}

/// <summary>
/// appUpdate\constUpdate\login三协议合一
/// </summary>
public class BundleStartBody : Body
{
    public Body constUpdate;
    public VersionInfoBody appUpdate;
    public LoginBody login;
    public BundleStartBody(string name, string url = null) : base(name, url)
    {
        constUpdate = new Body(URLConfig.ConstUpdate);
        appUpdate = new VersionInfoBody(URLConfig.AppVersion);
        appUpdate.assetVersion = AssetBundleManager.Inst.GetVersion();
        login = LoginManager.Inst.GetLoginBody();
    }

}

/// <summary>
/// 登陆
/// </summary>
public class DebugSwitchBody : Body
{
    public DebugSwitchBody(string name, string url = null) : base(name, url)
    {

    }

    public string pass; //平台类型
   
}
/// <summary>
/// 同步数据
/// </summary>
public class SyncPlayDataBody : Body
{
    public string playdata;
    public string sign;
    public string gameUid;
    // public UserInfo userInfo = new UserInfo();
    // public UserGame userGame = new UserGame();
    public SyncPlayDataBody(string _playdata, string name, string url = null) : base(name, url)
    {
        if (!string.IsNullOrEmpty(_playdata))
        {
            this.playdata = _playdata;
            this.sign = AssetUtils.EncryptWithMD5("wb"+_playdata);
            JSONNode jObj = JSONNode.Parse(_playdata);
            this.gameUid = jObj["StorageAccountInfo"]["_Uid"];
            // this.userInfo.gameUid = jObj["userInfo"]["gameUid"];
            // this.userGame.level = jObj["userGame"]["level"];
            // this.userGame.stamina = jObj["userGame"]["stamina"];
            // this.userGame.staminaRecoverTime = jObj["userGame"]["staminaRecoverTime"];
            // this.userGame.stars = jObj["userGame"]["stars"];
            // this.userGame.coin = jObj["userGame"]["coin"];
            // this.userGame.continuedWinCount = jObj["userGame"]["continuedWinCount"];
        }
    }
    
}


/// <summary>
/// 心跳
/// </summary>
public class HeartBeatBody : Body
{
    public HeartBeatBody(string method, string url = null) : base(method, url)
    {
    }
}

/// <summary>
/// 绑定
/// </summary>
public class BindBody : Body
{
    public BindBody(string name, string url = null) : base(name, url)
    {

    }
    public string type;//facebook或其它后续支持的账号类型
    public string id;//第三方账号ID
    public string thirdtoken;//第三方账号登陆验证token
    public string gameUid;

}

/// <summary>
/// 获取好友关卡排名数据
/// </summary>
public class GetFriendsLevelRankBody : Body
{
    public GetFriendsLevelRankBody(string name, string url = null) : base(name, url)
    {
        
    }

    // 关卡等级
    public int level;
    // 关卡分数
    public int score;
}

/// <summary>
/// 版本信息
/// </summary>
public class VersionInfoBody : Body
{
    public VersionInfoBody(string name, string url = null) : base(name, url)
    {

    }
    public string assetVersion;


}

/// <summary>
/// 每日签到
/// </summary>
public class SigninBody : Body
{
    public SigninBody(string method, string url = null) : base(method, url)
    {
    }
}


/// <summary>
/// 通关结算数据
/// </summary>
public class ActivitySyncScoreBody : Body
{
    public int score;
    public string activity_id;

    public ActivitySyncScoreBody(string activity_id,int score, string method,string url = null) : base(method, url)
    {
        this.activity_id = activity_id;
        this.score = score;
    }
}
/// <summary>
/// 获取所有活动
/// </summary>
public class ActivityRankingBody : Body
{
    public string activityUuid;
    public int needRewardInfo;
    public int level;
    public int totalStar;
    public ActivityRankingBody(string activity_id, int level, int totalStar, string method, string url = null) : base(method, url)
    {
        this.activityUuid = activity_id;
        needRewardInfo = 1;
        // 当前关卡等级
        this.level = level;
        // 总星星数
        this.totalStar = totalStar;
    }
}
/// <summary>
/// 获取所有活动
/// </summary>
public class ActivityReceiveRewardBody : Body
{
    public string activityUuid;
    public string activityName;
    public ActivityReceiveRewardBody(string activity_id, string activity_name, string method, string url = null) : base(method, url)
    {
        this.activityUuid = activity_id;
        this.activityName = activity_name;
    }
}

/// <summary>
/// 获取所有活动
/// </summary>
public class MailListBody<T> : Body
{
    public List<T> mail_ids;
    public MailListBody(List<T> mailIds, string method, string url = null) : base(method, url)
    {
        this.mail_ids = mailIds;
    }
}

public class WordDicBody : Body
{
    public string word;
    public WordDicBody(string word, string method, string url = null) : base(method, url)
    {
        this.word = word;
    }
}

/// <summary>
/// 打日志
/// </summary>
public class LogBody : Body
{
    public string event_name;
    public string event_data;
    public string gameUid;

    public LogBody(string eventName, string eventData, string gameUid, string method, string url = null) : base(method, url)
    {
        this.event_name = eventName;
        this.event_data = eventData;
        this.gameUid = gameUid;
    }

    public LogBody(AnalyticsTool.LogItem logItem, string method, string url = null) : base(method, url)
    {
        this.event_name = logItem.name;
        this.event_data = logItem.data;
        this.gameUid = StorageManager.Inst.GetStorage<StorageAccountInfo>().Uid;
    }
}

public class BatchLogBody : Body
{
    public string logStr;

    public BatchLogBody(JSONArray logArray, string method, string url = null) : base(method, url)
    {
        if (logArray != null && logArray.Count > 0)
        {
            logStr = logArray.ToString();
        }
    }
}
