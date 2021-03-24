using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Storage;
using SimpleJSON;
using EventUtil;

public class HeartBeatInfo
{
    //服务器时间戳
    public int serverTime = 0;
    //1:当天未签到   2:当天已签到
    public int signStatus = 0;
    //已经签到的天数
    public int signDays = -1;


    public HeartBeatInfo()
    {

    }



    public void HeartBeat()
    {
        return;
        if (!string.IsNullOrEmpty(LoginManager.Inst.Session))
        {
            HeartBeatBody body = new HeartBeatBody(URLConfig.HeartBeat);
            HttpRequestTool.SendMessage(
                body,
                (respons) =>
                {
                    Debug.Log("HeartBeat Sucess :" + respons);
                    JSONNode jsonNode = JSONNode.Parse(respons);
                    int code = jsonNode["code"];
                    if (code == 0)
                    {
                        serverTime = (int)(jsonNode["serverTime"].AsDouble / 1000L);
                        //signStatus = jsonNode["signin_status"];
                        //signDays = jsonNode["signin_days"];
                        EventDispatcher.TriggerEvent(EventKey.HeartBeat);
                    }
                },
                (respons) =>
                {
                    Debug.LogError("HeartBeat Fail :" + respons);
                });
        }
    }

}
