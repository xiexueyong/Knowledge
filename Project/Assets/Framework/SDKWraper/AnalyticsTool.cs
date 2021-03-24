using System;
using System.Collections.Generic;
using Framework;
using Newtonsoft.Json;
using Framework.Storage;
using Framework.Asset;
using SimpleJSON;
using UnityEngine;

public class AnalyticsTool
{

    private static string BatchLogCacheKey = "BatchLogCacheKey";
    private static JSONArray cacheLogArray;
    // 是否有log正在上传（用于控制同一时间只处理一个上传请求，如果有正在上传的请求，则只把当前的log加入到本地缓存中，下次一起上传）
    private static bool isPushingLog = false;

    public static void AddLogToCache(string eventName, string eventData)
    {
        if (cacheLogArray == null)
        {
            string cacheStr = PlayerPrefs.GetString(BatchLogCacheKey, null);
            if (string.IsNullOrEmpty(cacheStr))
            {
                cacheLogArray = new JSONArray();   
            }
            else
            {
                cacheLogArray = (JSONArray) JSONNode.Parse(cacheStr);
            }
        }
        JSONObject obj = new JSONObject();
        obj.Add("event_name", eventName);
        obj.Add("event_data", eventData);
        cacheLogArray.Add(obj);
        PlayerPrefs.SetString(BatchLogCacheKey, cacheLogArray.ToString());
    }

    private static void PushBatchLog()
    {
        // 如果（前一次）正在上传，则本次直接退出（此时log已经加入到本地缓存中了）
        if (isPushingLog)
        {
            return;
        }

        // 是否还有本地log
        if (cacheLogArray.Count == 0)
        {
            return;
        }
        
        // 标记开始上传
        isPushingLog = true;
        
        // 准备上传的log
        JSONArray pushingLogArray = new JSONArray();
        foreach (var log in cacheLogArray)
        {
            pushingLogArray.Add(log);
        }
        Body logBody = new BatchLogBody(pushingLogArray, URLConfig.PushBatchLog);
        HttpRequestTool.SendMessage(logBody,
            (successData) =>
            {
                DebugUtil.Log("push batch log success:" + successData);
                // 删除已经上传成功的log
                foreach (var log in pushingLogArray)
                {
                    cacheLogArray.Remove(log);
                }
                // 如果本地log数量为0，则清除缓存；如果本地log还有，则重新写入缓存。
                if (cacheLogArray.Count == 0)
                {
                    PlayerPrefs.DeleteKey(BatchLogCacheKey);   
                }
                else
                {
                    PlayerPrefs.SetString(BatchLogCacheKey, cacheLogArray.ToString());
                }
                isPushingLog = false;
            },
            (failData) =>
            {
                DebugUtil.LogError("push batch log fail:" + failData);
                isPushingLog = false;
            });
    }

    private static void Analytics(string eventName, string eventData)
    {
        // Body logBody = new LogBody(logItem, URLConfig.PushLog);
        // HttpRequestTool.SendMessage(logBody,
        //     (successData) =>
        //     {
        //         DebugUtil.Log("push log success:" + successData);
        //     },
        //     (failData) =>
        //     {
        //         DebugUtil.LogError("push log fail:" + failData);
        //     },
        //     null,
        //     false
        // );
        AddLogToCache(eventName, eventData);
        PushBatchLog();
    }
    
    /// <summary>
    /// 可传 Dictionary<string,string>参数
    /// </summary>
    /// <param name="name">Name.</param>
    /// <param name="values">Valus.</param>
    public static void Analytics(string name, Dictionary<string,string> values)
	{
        return;
        try
        {
            if (!string.IsNullOrEmpty(name) && values != null)
            {
                if (!values.ContainsKey("playerid"))
                    values.Add("playerid", StorageManager.Inst.GetStorage<StorageAccountInfo>().Uid.ToString());
                if (!values.ContainsKey("debugMode"))
                    values.Add("debugMode", GameConfig.Inst.DebugEnable.ToString());
                if (!values.ContainsKey("channel"))
                    values.Add("channel",AssetBundleConfig.Inst.AppChannel.ToString());
                // 记录日志时间（和服务器同步后的数据，即UTC时间，单位到秒）
                if (!values.ContainsKey("logTime"))
                    values.Add("logTime", CommonUtil.GetTimeStamp(true).ToString());
            
                if (!values.ContainsKey("AppVersion"))
                    values.Add("AppVersion", DeviceHelper.GetAppVersion());
                if (!values.ContainsKey("AssetVersion"))
                    values.Add("AssetVersion", AssetBundleManager.Inst.GetVersion());
            
                if (!values.ContainsKey("CurrentLevel"))
                    values.Add("CurrentLevel", DataManager.Inst.userInfo.Level.ToString());
            
                if (!values.ContainsKey("devId"))
                {
                    string devId = Framework.DeviceInfo.deviceInfo.deviceId;
                    if (devId == null)
                    {
                        DebugUtil.LogError("BI devID ==null");
                    }
                    else
                    {
                        values.Add("devId", devId);
                    }
                }
                      
#if UNITY_EDITOR
                if (GameConfig.Inst.DebugEnable)
                {
            
                    foreach (KeyValuePair<string, string> keyValuePair in values)
                    {
                        try
                        {
                            DebugUtil.LogWarning("BI key: " + keyValuePair.Key + " value: " + keyValuePair.Value);
                        }
                        catch (Exception e)
                        {
                            DebugUtil.LogError("key value exception");
                        }
                    }
            
                    DebugUtil.Log("FTDBI,key:{0},value:{1}", name, JsonConvert.SerializeObject(values));
                }
#endif
            
                // Analytics (new LogItem(name, values));
                Analytics (name, JsonConvert.SerializeObject(values));
            }
        }
        catch(Exception e)
        {
            DebugUtil.LogError("Analytics Log error:" + e.ToString());
        }
    }


    public class LogItem
    {
        public string name;
        public string data;

        public LogItem(string name, Dictionary<string, string> data)
        {
            this.name = name;
            this.data = JsonConvert.SerializeObject(data);
        }
        
        public LogItem(string name)
        {
            this.name = name;
            this.data = "{}";
        }

    }
}


