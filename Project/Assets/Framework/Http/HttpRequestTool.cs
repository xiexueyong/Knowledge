using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using EventUtil;

#if UNITY_EDITOR
using UnityEditor;
#endif

[DisallowMultipleComponent]
public class HttpRequestTool : D_MonoSingleton<HttpRequestTool>
{
    public enum HttpResponseCode
    {
        Ok = 0,
        UnKnown = -1,
        VerifyFail = -5,
        SessinExpired = -12
    }


    private Queue<HttpPacket> _cacheMessageList;
    private const int MaxCacheMessageListCount = 20;

    private bool _isRequesting = false;

    private void Awake()
    {
        _cacheMessageList = new Queue<HttpPacket>();
    }
    
	public static void SendMessage(Body data, Action<string> handler, Action<string> errorHandler = null,Header header = null,bool needSession = true)
    {
        //当网络不可用时
        HttpPacket dataPacket = new HttpPacket(data, handler, errorHandler, header == null ? Header.GetHeader() : header, needSession);
        SendMessage(dataPacket);
    }
    public static void SendMessage(HttpPacket httpPacket)
    {
        //当网络不可用时
        if (Application.internetReachability == NetworkReachability.NotReachable || !GameConfig.Inst.NetEnable)
        {
            if (httpPacket.errorHandler != null)
            {
                httpPacket.errorHandler("Internet Not Reachable");
            }
            DebugUtil.LogError("Internet Not Reachable");
            //UICommonLogic.Inst.ShowMessage("Internet Not Reachable");
            return;
        }
        ////未登陆或者Session过期
        //if (LoginManager.Inst.loginStatus == LoginManager.LoginStatus.LOGOUT && httpPacket.needSession)
        //{
        //    if (httpPacket.errorHandler != null)
        //    {
        //        httpPacket.errorHandler("has not login");
        //    }
        //    return;
        //}
        //if (LoginManager.Inst.loginStatus == LoginManager.LoginStatus.TOKEN_EXPIRED && httpPacket.needSession)
        //{
        //    EventDispatcher.TriggerEvent(EventKey.SessionExpiredOrEmpty, httpPacket);
        //    if (httpPacket.errorHandler != null)
        //    {
        //        httpPacket.errorHandler("token Expired");
        //    }
        //    Debug.LogError("token Expired");
        //    return;
        //}
        ////未登陆或者Session过期
        //if (LoginManager.Inst.loginStatus == LoginManager.LoginStatus.LOGINFAIL && httpPacket.needSession)
        //{
        //    if (httpPacket.errorHandler != null)
        //    {
        //        httpPacket.errorHandler("login status is failed");
        //    }
        //    return;
        //}

        Inst.CacheMessage(httpPacket);
        Inst.PostRequest();
    }

    private void CacheMessage(HttpPacket packet)
    {
        if (_cacheMessageList.Count < MaxCacheMessageListCount)
        {
			DebugUtil.Log(string.Format("[HttpTool] CacheMessage : requestName:{0}", packet.body.method));
            _cacheMessageList.Enqueue(packet);
        }
        else
        {
            DebugUtil.LogError("need send list cache is full,_cacheMessageList.Count:"+ _cacheMessageList.Count);
        }
    }

    public void PostRequest()
    {
        if (_isRequesting)
        {
            return;
        }
        if (_cacheMessageList.Count <= 0)
        {
            return;
        }
        HttpPacket packet = _cacheMessageList.Dequeue();
        StartCoroutine(PostCoroutine(packet));
    }

    private void SetHeader(UnityWebRequest www,Header header)
    {
        foreach (var item in header.headers)
        {
            if(!string.IsNullOrEmpty( item.Key) && !string.IsNullOrEmpty(item.Value))
                www.SetRequestHeader(item.Key,item.Value);
        }
       

    }
         
	IEnumerator PostCoroutine(HttpPacket packet)
	{
		Debug.Log(string.Format("[HttpTool2] SendMessage : method:{0}", packet.body.method));
        Body body = packet.body;
		Header header = packet.header != null?packet.header: Header.GetHeader();
		string bodyStr = JsonConvert.SerializeObject(body);
        string headerStr = JsonConvert.SerializeObject(header.headers);
        DebugUtil.Log("++++++++++++++++HttpRequestTool, Url:  " + packet.body.url);
        DebugUtil.Log ("HttpRequestTool, Body:  " + bodyStr);
        DebugUtil.Log("HttpRequestTool, Header:  " + headerStr);
        using (UnityWebRequest www = new UnityWebRequest (packet.body.url, UnityWebRequest.kHttpVerbPOST)) {
            UploadHandler uploaderHandler = new UploadHandlerRaw (CommonUtil.ToBytes (bodyStr));
			uploaderHandler.contentType = "application/json";//application/json    、   application/octet-stream
            DownloadHandlerBuffer downHandler = new DownloadHandlerBuffer ();
			www.uploadHandler = uploaderHandler;
			www.downloadHandler = downHandler;
            SetHeader(www,header);

            www.timeout = 8;
			_isRequesting = true;
			yield return www.SendWebRequest ();

			if (www.isNetworkError || www.isHttpError) {
				if (packet.errorHandler != null)
				{
                    DebugUtil.LogError("HttpRequestTool,url:{0},error:{1},responseCode:{2},isNetworkError:{3},isHttpError:{4}", 
                        packet.body.url, www.error, www.responseCode, www.isNetworkError, www.isHttpError);
                    packet.errorHandler(www.error);
                    //TODO:
                    //Session过期、未注册 在此处 发事件通知LoginManager
                    //if(packet.needSession)
                    //    EventDispatcher.TriggerEvent(EventKey.HTTPNeedSession, packet);
                    //LoginManager.Inst.loginStatus = LoginManager.LoginStatus.TOKEN_EXPIRED;
                }
			} else {
                string results = www.downloadHandler.text;
                try
				{
                    SimpleJSON.JSONNode jsonNode = SimpleJSON.JSONNode.Parse(results);
                    int code = jsonNode["code"];
                    if (code == (int)HttpResponseCode.SessinExpired)
                    {
                        EventUtil.EventDispatcher.TriggerEvent(EventKey.SessionExpiredOrEmpty, packet);
                    }

                    DebugUtil.Log("[HttpTool] results " + results.Length);
					packet.handler(results);
				}
				catch (Exception e)
				{
					if (packet.errorHandler != null)
					{
                        DebugUtil.LogError("httpRequestTool www response error:" + e.StackTrace);
                        packet.errorHandler(string.Format("http receive data:{0},but error occour in sucess handler:{1}", results, e.ToString()));
                    }
				}
			}
            packet.handler = packet.errorHandler = null;
            _isRequesting = false;
		}
		PostRequest();
	}

#if UNITY_EDITOR
    [CustomEditor(typeof(HttpRequestTool))]
    public class HttpRequestToolEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector ();
            HttpRequestTool self = (HttpRequestTool)target;

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.Toggle("Requesting", self._isRequesting);
            EditorGUI.EndDisabledGroup();
            
            if(self._cacheMessageList == null)
                return;
            
            EditorGUILayout.Space();
            GUILayout.Label ("Cache Message List : " + self._cacheMessageList.Count);

            foreach (var item in self._cacheMessageList)
            {
                EditorGUILayout.BeginHorizontal();
				GUILayout.Label(item.body.method);
                EditorGUILayout.EndHorizontal();
            }
        }
    }
#endif
}