using System;
using System.Collections;
using System.Collections.Generic;
using EventUtil;
using UnityEngine;
using Framework.Storage;
using SimpleJSON;
using Newtonsoft.Json;
using System.Linq;


public enum ActivityType
{
    Ranking=1,
    
}

public class ActivityItem
{
    //{
    //    "EndTime":1599609600,
    //    "isRewarded":1,
    //    "activityName":"psr",
    //    "StartTime":1599436800,
    //    "rank":2,
    //    "activityUuid":"685fcecd-e201-4ec1-8a58-222e75a2e964",
    //    "CompetitionEndTime":1599580800
    //}

    public ActivityItem(JSONNode node)
    {
        StartTime = node["StartTime"];
        CompetitionEndTime = node["CompetitionEndTime"];
        EndTime = node["EndTime"];

        activityName = node["activityName"];
        activityUuid = node["activityUuid"];

        rank = node["rank"];
        isRewarded = node["isRewarded"];//领奖状态,0:未领奖,1:已领奖
    }

    public int StartTime;
    public int CompetitionEndTime;
    public int EndTime;

    public string activityName;
    public string activityUuid;
    public int rank;
    public int isRewarded;

    /// <summary>
    /// 是否处于比赛时间内
    /// </summary>
    /// <returns></returns>
    public bool IsRacing()
    {
        return StartTime < SystemClock.Now && SystemClock.Now < CompetitionEndTime;
    }
}


public class ActivityInfo
{
    public static string AcitivityInfoCacheKey = "AcitivityInfoCacheKey";

    public List<ActivityItem> activities;

    public ActivityInfo()
    {
        activities = new List<ActivityItem>();

        string a = PlayerPrefs.GetString(AcitivityInfoCacheKey, string.Empty);
        activities = ParseActivityInfo(a);
    }
    public ActivityItem GetActivityItem(string activityName)
    {
        for(int i = 0;i< activities.Count;i++)
        {
            if (activityName == activities[i].activityName)
                return activities[i];
        }
        return null;
    }
    public void Clear()
    {
        activities.Clear();
    }
    /// <summary>
    /// 获取所有活动的时间
    /// </summary>
    /// <param name="callback"></param>
    public void GetAllActivity(Action<List<ActivityItem>> callback = null)
    {

        //if (activities != null && activities.Count > 0)
        //{
        //    if (callback != null)
        //        callback(activities);
        //    return;
        //}

        Body body = new Body(URLConfig.GetActivityInfo);

        HttpRequestTool.SendMessage(
            body,
            (x) => {
                Debug.Log("get all activity Sucess :" + x);
                var acs = ParseActivityInfo(x);
                if (acs.Count > 0)
                {
                    PlayerPrefs.SetString(AcitivityInfoCacheKey, x);
                    activities = acs;
                    EventDispatcher.TriggerEvent(EventKey.OnActivityItemsUpdate);
                    if (callback != null)
                        callback(activities);
                }
            },
            (x) => {
                if (callback != null)
                    callback(null);
                Debug.LogError("get all activity Fail :" + x);
            }, null, true
            );
    }
    List<ActivityItem> ParseActivityInfo(string data)
    {
        var acs = new List<ActivityItem>();
        try
        {
            if (string.IsNullOrEmpty(data))
            {
                return acs;
            }
            JSONNode nodes = JSONNode.Parse(data);
            foreach (var item in nodes)
            {
                acs.Add(new ActivityItem(item));
            }

        }
        catch(Exception e)
        {
            return acs;
        }
        return acs;
    }
}
