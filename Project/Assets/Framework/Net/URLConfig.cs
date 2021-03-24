using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;

public static class URLConfig
{
    
    /**
    参数:
    "appUpdate"，Json，里面是原来AppUpdate的参数
    "constUpdate"，Json，里面是原来ConstUpdate的参数
    "login"，Json，里面是原来Login的参数

    返回值：
    "appUpdate"，Json，里面是原来AppUpdate的返回值
    "constUpdate"，Json，里面是原来ConstUpdate的返回值
    "login"，Json，里面是原来Login的返回值
    **/
    public static string BundleStart = "BundleStart";


    // 注册/根据设备id找回账号
    public static string Login = "FindOrReg";
    // 心跳
    public static string HeartBeat = "Sync";
    // 上传数据
    public static string SyncPlayData = "PushUserData";
    // 获取更新信息
    public static string AppVersion = "AppUpdate";
    // 获取配置信息
    public static string ConstUpdate = "ConstUpdate";
    // 上传log
    public static string PushLog = "CCLog";
    // 上传批量log
    public static string PushBatchLog = "CCBatchLog";
    // 绑定账号
    public static string BindUser = "BindUser";
    // 获取好友关卡排名信息
    public static string GetFriendsLevelRank = "GetFriendsLevelRank";


    //获取活动信息
    public static string GetActivityInfo = "GetActivityInfo";

    //获取个人军备排行数据：
    //参数：
    //{"gameUid":"15893032410571470409","activityUuid":"685fcecd-e201-4ec1-8a58-222e75a2e964","needRewardInfo":1}
    public static string GetPersonalRaceStarRankInfo = "GetPersonalRaceStarRankInfo";

    //"领奖协议"
    //参数：
    //{"gameUid":"15893032410571470409","activityUuid":"685fcecd-e201-4ec1-8a58-222e75a2e964","activityName":"psr"}
    public static string ReceiveActivityReward = "ReceiveActivityReward";




    //废弃的
    public static string Signin = "QSignIn";
    public static string ActivitySyncScore = "QActivitySyncScore";
    public static string ActivityRank = "QActivityRank";
    public static string DebugSwitch = "QDebugSwitch";
    public static string MailList = "QMailList";
    public static string DeleteMails = "QDeleteMails";

    public static void Init()
    {

    }
}