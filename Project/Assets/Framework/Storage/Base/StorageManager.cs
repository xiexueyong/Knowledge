//*************************************************//
/*
 * Example
 * 必须且只能初始化一次
 * StorageManager.Inst.Init(new List<StorageBase>());
 * 
 * StorageCook cook = StorageManager.Inst.GetStorage<StorageCook>();
 * cook.Coins["island_1"] = 200;
 * 
 * 这种模式只支持客户端向服务器发送数据，不支持服务端改变客户端数据 ，
 * 如服务端需要修改数据，一般是客户端发起请求，在客户端的Response内修改数据
 */
//*************************************************//

using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Framework.Utils;
using System;
using SimpleJSON;
using EventUtil;

namespace Framework.Storage
{
    public class StorageManager : D_MonoSingleton<StorageManager>
    {
        // 远端同步间隔
        float remoteInterval = 20.0f;
        // 本地同步间隔
        float localInterval = 2.0f;

        // 距上次存储数据的时间间隔
        float LocalTickTime;
        // 上次存储至本地的版本号
        float RemoteTickTime;

        // 上次存储至本地的版本号
        ulong lastSavedLocalVersion;
        // 上次存储至本地的版本号
        ulong lastSavedRemoteVersion;

        //强制同步，用于存储敏感数据
        public bool SyncForce
        {
            get;
            set;
        }
        //正在同步，不可进行下次同步
        bool syncLock;

        // 本地存档版本
        private ulong _localVersion;
        public ulong LocalVersion
        {
            get
            {
                return _localVersion;
            }
            set
            {
                _localVersion = value;
                GetStorage<StorageAccountInfo>().StorageVersion++;
                //DebugUtil.Log("[storage] changed local_version to {0}", value);
            }
        }

    


        public const string storageKey = "StorageData";
        Dictionary<string, StorageBase> storageMap;
        public T GetStorage<T>() where T : StorageBase
        {
            if (!Inited)
            {
                throw new Exception("Storage has not been initialised,it is intialised in GameController");
            }
            string storageName = typeof(T).Name;
            if (!storageMap.ContainsKey(storageName))
            {
                throw new Exception(string.Format("Storage: ={0}= has not been initialised,You should init it before use it",storageName));
            }
            return (T)storageMap[storageName];
        }

        public static void Clear()
        {
            if (Inst.Inited)
            {
                PlayerPrefs.DeleteKey(storageKey);
            }
        }
    

        public bool Inited
        {
            get;
            private set;
        }

        /// <summary>
        /// 存储至网络
        /// </summary>
        public void SaveToRemote(ulong dataVersion)
        {
            //return;
            //if (LoginManager.Instance.loginStatus != LoginManager.LoginStatus.LOGINSUCESS
            //    || Application.internetReachability == NetworkReachability.NotReachable)
            //{
            //    return;
            //}


            //if (!string.IsNullOrEmpty(SROptions.Current.GetUId))
            //{
            //    DebugUtil.Log("setuid data not syndata");
            //    return;
            //}

            string jsonData = getJson();
            if (!string.IsNullOrEmpty(jsonData))
            {

                SyncPlayDataBody body = new SyncPlayDataBody(jsonData,URLConfig.SyncPlayData);
                //body.playdata = jsonData;

                syncLock = true;
                HttpRequestTool.SendMessage(
                    body,
                    (x) => {
                        lastSavedRemoteVersion = dataVersion;
                        Debug.Log("SaveToRemote Sucess :" + x);
                        syncLock = false;
                    },
                    (x) => {
                        Debug.LogError("SaveToRemote Fail :" + x);
                        syncLock = false;
                    },null,true
                    );
            }
            else
            {
                DebugUtil.LogError("StorageManager.SaveToRemote , Storage Local data do not exist");
            }
        }
        /// <summary>
        /// 把当前数据存储至本地
        /// </summary>

        private void SaveToLocal(string jsonData = null)
        {
            if (LocalVersion == lastSavedLocalVersion && string.IsNullOrEmpty(jsonData))
                return;

            string saveData = null;
            if (string.IsNullOrEmpty(jsonData))
            {
                saveData = getJson();
            }
            else
            {
                saveData = jsonData;
            }


            if (!string.IsNullOrEmpty(saveData))
            {
                PlayerPrefs.SetString(storageKey, saveData);
                lastSavedLocalVersion = LocalVersion;
            }
            else
            {
                DebugUtil.LogError("StorageManager.SaveToLocal , Storage Local data do not exist, it is a very serious matter");
            }
        }

        // /// <summary>
        // /// 从json生成Storage对象
        // /// </summary>
        // /// <param name="jsonData">json字符串</param>
        // /// <param name="fromLocal">是否是本地存储</param>
        // public void SetData(string jsonData, bool fromLocal = true)
        // {
        //     JSONNode jObj = null;
        //     try
        //     {
        //         jObj = JSONNode.Parse(jsonData);
        //     }
        //     catch (Exception e)
        //     {
        //         DebugUtil.LogError("Storage deserialization error,Can't load data from local");
        //         DebugUtil.LogError(e.StackTrace);
        //         return;
        //     }
        //
        //     SetData(jObj, fromLocal);
        // }
        
        /// <summary>
        /// 从json生成Storage对象
        /// </summary>
        /// <param name="jsonData"></param>
        public void SetData(string jsonData,bool fromLocal = true)
        {
            JObject jObj = null;
            try
            {
                jObj = JObject.Parse(jsonData);
            }
            catch (Exception e)
            {
                DebugUtil.LogError("Storage deserialization error,Can't load data from local");
                DebugUtil.LogError(e.StackTrace);
                return;
            }
            
            foreach (var type in storageMap.Keys)
            {
                var token = jObj[type];
                if (token == null)
                {
                    continue;
                }
                var str = token.ToString();
                try
                {
                    JsonSerializerSettings setting = new JsonSerializerSettings();
                    setting.NullValueHandling = NullValueHandling.Ignore;
                    JsonConvert.PopulateObject(str, storageMap[type], setting);
                }
                catch (Exception e)
                {
                    DebugUtil.LogError("StorageManager.FromJson,  initialise Storage error , it is a very serious matter");
                    DebugUtil.LogError("StorageManager.FromJson, Initialise Storage:{0}, storage string:{1} ", type,str);
                    DebugUtil.LogError(e.Message);
                    DebugUtil.LogError(e.StackTrace);
                }
            } 
            
            DebugUtil.Log("=========local UId:"+(storageMap["StorageAccountInfo"] as StorageAccountInfo).Uid);
            if (!fromLocal)
            {
                SaveToLocal(jsonData);
            }
        }

        /// <summary>
        /// 从json生成Storage对象
        /// </summary>
        /// <param name="jsonData">json对象</param>
        /// <param name="fromLocal">是否是本地存储</param>
        public void SetData(JSONNode jObj, bool fromLocal = true)
        {
            if (null == jObj)
            {
                DebugUtil.LogError("StorageManager SetData, but jObj is null");
                return;
            }

            foreach (var storageKey in storageMap.Keys)
            {
                string jsonKey = storageKey;
                if ("StorageUserInfo".Equals(jsonKey))
                {
                    jsonKey = "userGame";
                }
                var token = jObj[jsonKey];
                if (token == null)
                {
                    continue;
                }
                var str = token.ToString();
                try
                {
                    JsonSerializerSettings setting = new JsonSerializerSettings();
                    setting.NullValueHandling = NullValueHandling.Ignore;
                    JsonConvert.PopulateObject(str, storageMap[storageKey], setting);
                }
                catch (Exception e)
                {
                    DebugUtil.LogError("StorageManager.FromJson,  initialise Storage error , it is a very serious matter");
                    DebugUtil.LogError("StorageManager.FromJson, Initialise Storage:{0}, storage string:{1} ", storageKey, str);
                    DebugUtil.LogError(e.Message);
                    DebugUtil.LogError(e.StackTrace);
                }
            } 
            
            DebugUtil.Log("=========local UId:"+((StorageAccountInfo) storageMap[convertTypeNameToRemoteName("StorageAccountInfo")]).Uid);
            if (!fromLocal)
            {
                SaveToLocal(jObj.ToString());
            }
        }



        /// <summary>
        /// 刷新数据
        /// </summary>
        public void RefreshAppData()
        {
            if (Inited)
            {
                GetStorage<StorageAccountInfo>().AppVersion = DeviceHelper.GetAppVersion();
            }
        }
        /// <summary>
        /// 初始化Storage
        /// </summary>
        public void Init()
        {
            if (!Inited)
            {
                //PlayerPrefs.DeleteKey(storageKey);
                CreateStorages();
                ReadFromLocal();
                Inited = true;
                EventDispatcher.TriggerEvent(EventKey.StorageInit);
                RefreshAppData();
            }
            else
            {
                Debug.Assert(false, "Init Storage Error !!!");
            }
        }
        private void CreateStorages()
        {
            List<StorageBase> storageBases = new List<StorageBase>();
            storageBases.Add(new StorageAccountInfo());
            storageBases.Add(new StorageUserInfo());
            storageMap = new Dictionary<string, StorageBase>();
            foreach (var storage in storageBases)
            {
                var type = storage.GetType().Name;
                storageMap[type] = storage;
            }
        }

        private String convertTypeNameToRemoteName(String typeName)
        {
            string retStr;
            switch (typeName)
            {
                case "StorageAccountInfo":
                    retStr = "userInfo";
                    break;
                case "StorageUserInfo":
                    retStr = "userGame";
                    break;
                default:
                    retStr = typeName;
                    break;
            }
            return retStr;
        }
        
        public void Refresh()
        {
            CreateStorages();
            ReadFromLocal();
        }

        /// <summary>
        /// 将本地数据转为json
        /// </summary>
        /// <returns></returns>
        private string getJson()
        {
            if (storageMap == null)
            {
                DebugUtil.LogError("StorageManager.ToJson   StorageManager storageMap is null , it is a very serious matter");
                return null;
            }
            return JsonConvert.SerializeObject(storageMap);
        }

        /// <summary>
        /// 从本地读取json 并生成Storage数据
        /// </summary>
        private void ReadFromLocal()
        {
            // 读取本地存档
            string jsonData = "{}";
            if (PlayerPrefs.HasKey(storageKey))
            {
                jsonData = PlayerPrefs.GetString(storageKey);
                DebugUtil.Log(" read storage json from local : " + jsonData);
                SetData(jsonData);
            }
            else
            {
                DebugUtil.Log("No local storage data can read! ");
            }
        }
        void Update()
        {
            if (!Inited)
                return;

            LocalTickTime += Time.deltaTime;
            //if (SyncForce || (LocalTickTime > localInterval && LocalVersion > lastSavedLocalVersion))
            if (LocalTickTime > localInterval && LocalVersion > lastSavedLocalVersion)
            {
                SyncForce = false;
                LocalTickTime = 0.0f;
                SaveToLocal();
               
            }

            RemoteTickTime += Time.deltaTime;
            if (LoginManager.Inst.isLoginFinished() 
                && RemoteTickTime > remoteInterval 
                && LocalVersion > lastSavedRemoteVersion 
                && !syncLock)
            {
                RemoteTickTime = 0.0f;
                SaveToRemote(LocalVersion);
            }
        }


        private void OnApplicationFocus(bool focus)
        {
            
        }

        private void OnApplicationPause(bool pause)
        {
            SaveToLocal();
            if (StorageManager.Inst.Inited && LoginManager.Inst.isLoginFinished())
            {
                SaveToRemote(LocalVersion);   
            }
        }

        private void OnApplicationQuit()
        {
            
        }
    }
}