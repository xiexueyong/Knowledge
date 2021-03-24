using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Storage;
using SimpleJSON;
/// <summary>
/// 签到
/// </summary>
public class SigninIno
{
   
    public enum SignType
    {
        NotSign = 1,
        Sign = 2
    }
    public enum SignResult
    {
        Sucess = 1,
        Fail = 2
    }

    /// <summary>
    /// 回调的第一个参数：连续登陆的天数
    /// 回调的第一个参数：签到成功
    /// </summary>
    /// <param name="callback"></param>
    public void Signin(Action<int,bool> callback)
    {
        Body body = new SigninBody(URLConfig.Signin);
        HttpRequestTool.SendMessage(
            body,
            (x) =>
            {
                Debug.Log("dailySign Success:" + x);
                JSONNode res = JSONNode.Parse(x);
                int code = res["code"];
                if (code == 0)
                {
                    int days = res["days"];
                    int signStatus = res["status"];
                    signLog[DateTime.UtcNow.Day] = signStatus == (int)SignResult.Sucess;

                    if (callback != null)
                    {
                        callback.Invoke(days, signStatus == (int)SignResult.Sucess);
                    }
                }
                else
                {
                    Debug.Log("dailySign Fail:" + x);
                }
            },
            (y) =>
            {
                Debug.Log("dailySign Fail" + y);
            }
            );
    }

    //< key:月中的天，value:是否已经签到 >
    public Dictionary<int, bool> signLog = new Dictionary<int, bool>();

    public bool canSign()
    {
        int serverSignDays = ServerDataManager.Inst.heartBeatInfo.signDays;
        int dayInMonth = DateTime.UtcNow.Day;
        if ((!signLog.ContainsKey(dayInMonth) || !signLog[dayInMonth]) && ServerDataManager.Inst.heartBeatInfo.signStatus == (int)SignType.NotSign)
        {
            return true;
        }
        return false;
    }
    //private void SignToday()
    //{
    //    DateTime lastSigninDate = SystemClock.Instance.SecondToDateTime(StorageManager.Inst.GetStorage<StorageUserInfo>().SigninTime);
    //    if (SystemClock.Instance.SameDay(lastSigninDate.AddDays(1.0), DateTime.Now))
    //    {
    //        StorageManager.Inst.GetStorage<StorageUserInfo>().SigninDays += 1;
    //        StorageManager.Inst.GetStorage<StorageUserInfo>().SigninTime = SystemClock.Now;
    //    }
    //    else
    //    {
    //        StorageManager.Inst.GetStorage<StorageUserInfo>().SigninDays = 1;
    //        StorageManager.Inst.GetStorage<StorageUserInfo>().SigninTime = SystemClock.Now;
    //    }
    //}

}
