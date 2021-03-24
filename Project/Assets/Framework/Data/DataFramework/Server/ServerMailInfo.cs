using System;
using System.Collections;
using System.Collections.Generic;
using EventUtil;
using UnityEngine;
using Framework.Storage;
using SimpleJSON;
using Newtonsoft.Json;
using System.Linq;


public class ServerMailInfo
{
    public ServerMailInfo()
    {
    }

    /// <summary>
    /// 获取服务端的邮件
    /// </summary>
    /// <param name="callback"></param>
    public void RequestMail(List<int> existMailids, Action callback = null)
    {
        MailListBody<int> body = new MailListBody<int>(existMailids,URLConfig.MailList);
        HttpRequestTool.SendMessage(
            body,
            (x) =>
            {
                Debug.Log("get mail Sucess :" + x);
                JSONNode node = JSONNode.Parse(x);
                if (node["code"] == 0)
                {
                   
                }
                else
                {
                    if (callback != null)
                        callback();
                }
            },
            (x) =>
            {
                if (callback != null)
                    callback();
                Debug.LogError("get mail Fail :" + x);
            }, null, true
            );
    }

    /// <summary>
    /// 删除服务端的邮件
    /// </summary>
    /// <param name="callback"></param>
    public void DeleteMail(List<int> deleteMailIds ,Action<List<int>> callback = null)
    {
        MailListBody<int> body = new MailListBody<int>(deleteMailIds,URLConfig.DeleteMails);
        
        HttpRequestTool.SendMessage(
            body,
            (x) =>
            {
                Debug.Log("delete mail Sucess :" + x);
                JSONNode node = JSONNode.Parse(x);
                if (node["code"] == 0)
                {
                    if (callback != null)
                        callback(deleteMailIds);
                }
            },
            (x) =>
            {
                Debug.LogError("delete mail Fail :" + x);
            }, null, true
            );
    }
}