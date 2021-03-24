/*-------------------------------------------------------------------------------------------
// 模块名：VersionManager
// 模块描述：版本管理
//-------------------------------------------------------------------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System;
using Newtonsoft.Json.Linq;
using Framework.Utils;
using SimpleJSON;
using UnityEngine.Networking;
using Framework.Tables;

namespace Framework.Asset
{
    public class AssetBundleManager : D_MonoSingleton<AssetBundleManager>
    {

        private readonly string versionFileName = "Version.txt";
        private AssetBundleVersionInfo _versionInfo;

        private AssetBundleVersionInfo _streamingVersionInfo;
        private AssetBundleVersionInfo _oldPersistentVersionInfo;
        private AssetBundleVersionInfo _serverVersionInfo;

        private string persistentVersionFile;

        /// <summary>
        /// 下载某个AssetBundleGroup
        /// </summary>
        private Dictionary<string, AssetBundleGroupLoadHelperItem> assetBundleGroupLoadHelpers;

        private string assetUrlKey = "AssetUrlKey";
        private string _assetUrl;
        private string assetUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_assetUrl) && PlayerPrefs.HasKey(assetUrlKey))
                {
                    _assetUrl = PlayerPrefs.GetString(assetUrlKey);
                }
                return _assetUrl;
            }
            set
            {
                if(!string.IsNullOrEmpty(value) && _assetUrl != value)
                {
                    _assetUrl = value;
                    PlayerPrefs.SetString(assetUrlKey,value);
                }
            }
        }
        private string latestAssetUrl;


        /// <summary>
        /// 1.检测是否需要将streaming下文件移动到persistent下
        /// </summary>
        /// <returns></returns>
        public IEnumerator Init()
        {
            DebugUtil.Log("streamingAssetsPath_Platform:" + FilePathTools.streamingAssetsPath_Platform);
            DebugUtil.Log("streamingAssetsPath_Platform_ForWWWLoad:" + FilePathTools.streamingAssetsPath_Platform_ForWWWLoad);
            DebugUtil.Log("persistentDataPath_Platform:" + FilePathTools.persistentDataPath_Platform);
            DebugUtil.Log("persistentDataPath_Platform_ForWWWLoad:" + FilePathTools.persistentDataPath_Platform_ForWWWLoad);

            assetBundleGroupLoadHelpers = new Dictionary<string, AssetBundleGroupLoadHelperItem>();
#if !UNITY_EDITOR
            AssetBundleConfig.Inst.UseAssetBundle = true;// 非编辑器模式下，永远使用ab包
#endif
            if (!AssetBundleConfig.Inst.UseAssetBundle)
                yield break;
#if UNITY_EDITOR 
            //Editor状态下 删除persistent 方便快速测试
            //if (Directory.Exists(FilePathTools.persistentDataPath_Platform))
            //    Directory.Delete(FilePathTools.persistentDataPath_Platform,true);
#endif
            //读取persistent下的VersionFile
            string oldPersistentVersionFilePath = string.Format("{0}/{1}", FilePathTools.persistentDataPath_Platform, versionFileName);
            if (File.Exists(oldPersistentVersionFilePath))
            {
                string oldPersistentVersionFilePathWWWLoad = string.Format("{0}/{1}", FilePathTools.persistentDataPath_Platform_ForWWWLoad, versionFileName);
                DebugUtil.Log("load oldPersistent VersionFile,path:"+ oldPersistentVersionFilePathWWWLoad);
                DataContainer dataContainer = new DataContainer();
                yield return LoadVersionFile(oldPersistentVersionFilePathWWWLoad, dataContainer);
                if (!string.IsNullOrEmpty(dataContainer.strData))
                    this._oldPersistentVersionInfo = JsonConvert.DeserializeObject<AssetBundleVersionInfo>(dataContainer.strData);
                else
                    this._oldPersistentVersionInfo = null;
            }
            else
            {
                this._oldPersistentVersionInfo = null;
            }
          

            //读取streaming下的VersionFile
            string streamingVersionFilePath = string.Format("{0}/{1}", FilePathTools.streamingAssetsPath_Platform_ForWWWLoad, versionFileName);
            DataContainer dataContainerStreaming = new DataContainer();
            yield return LoadVersionFile(streamingVersionFilePath, dataContainerStreaming);
           this._streamingVersionInfo = JsonConvert.DeserializeObject<AssetBundleVersionInfo>(dataContainerStreaming.strData);

            //移动Streaming
            if (_oldPersistentVersionInfo == null || CommonUtil.CompareVersion(this._streamingVersionInfo.Version ,this._oldPersistentVersionInfo.Version)> 0)
            {
                yield return MoveStreamingToPersistent();
                this._versionInfo = this._streamingVersionInfo;
            }
            else
            {
                this._versionInfo = this._oldPersistentVersionInfo;
            }
           
            while (!_finishMovingStreaming)
                yield return null;
        }
        /// <summary>
        /// 获取最新的版本信息
        /// </summary>
        /// <param name="upgradeInfo"></param>
        /// <returns></returns>
        public IEnumerator Upgrade(GameController.UpgradeInfo upgradeInfo)
        {
            //yield break;
            if (!AssetBundleConfig.Inst.UseAssetBundle)
                yield break;

            //请求服务器版本信息
            bool finishCheckVersion = false;
            DataContainer dataContainer = new DataContainer();
            VersionInfoBody vb = new VersionInfoBody(URLConfig.AppVersion);
            vb.assetVersion = this._versionInfo.Version;
            HttpRequestTool.SendMessage(vb,
                            (sucessData) =>
                            {
                                DebugUtil.Log("appversion sucess:" + sucessData);
                                JSONNode jsonNode = JSONNode.Parse(sucessData);
                                latestAssetUrl = jsonNode["assetUrl"];
                                if (string.IsNullOrEmpty(assetUrl))
                                    assetUrl = latestAssetUrl;

                                if (upgradeInfo != null)
                                {
                                    upgradeInfo.appUrl = jsonNode["appUrl"];
                                    upgradeInfo.assetUrl = latestAssetUrl;

                                    upgradeInfo.assetUpdateType = jsonNode["assetUpdateType"];
                                    upgradeInfo.appUpdateType = jsonNode["appUpdateType"];

                                    upgradeInfo.appVersion = jsonNode["appVersion"];
                                    upgradeInfo.assetVersion = jsonNode["assetVersion"];

                                    if (upgradeInfo.appUpdateType == 1 || upgradeInfo.appUpdateType == 2)
                                    {
                                        upgradeInfo.noticeTitle = jsonNode["appNotice"]["noticeTitle"];
                                        upgradeInfo.noticeContent = jsonNode["appNotice"]["noticeContent"];
                                        TableBI.Version_Updating(vb.appVersion, upgradeInfo.appVersion, vb.assetVersion, upgradeInfo.assetVersion);
                                    }
                                    else if (upgradeInfo.assetUpdateType == 1)
                                    {
                                        upgradeInfo.noticeTitle = jsonNode["assetNotice"]["noticeTitle"];
                                        upgradeInfo.noticeContent = jsonNode["assetNotice"]["noticeContent"];
                                        TableBI.Version_Updating(vb.appVersion, upgradeInfo.appVersion, vb.assetVersion, upgradeInfo.assetVersion);
                                    }

                                }
                                finishCheckVersion = true;
                            },
                            (failData) =>
                            {
                                DebugUtil.LogError("appversion fail:" + failData);
                                finishCheckVersion = true;
                            }, null, false);
            

            //等待版本信息
            while (!finishCheckVersion)
                yield return null;
        }
        void ParseVersionInfoResponse(JSONNode jsonNode, string oldAppVersion,string oldAssetVersion, GameController.UpgradeInfo upgradeInfo)
        {
            if (jsonNode == null)
            {
                return;
            }
            latestAssetUrl = jsonNode["assetUrl"];
            if (string.IsNullOrEmpty(assetUrl))
                assetUrl = latestAssetUrl;

            if (upgradeInfo != null)
            {
                upgradeInfo.appUrl = jsonNode["appUrl"];
                upgradeInfo.assetUrl = latestAssetUrl;

                upgradeInfo.assetUpdateType = jsonNode["assetUpdateType"];
                upgradeInfo.appUpdateType = jsonNode["appUpdateType"];

                upgradeInfo.appVersion = jsonNode["appVersion"];
                upgradeInfo.assetVersion = jsonNode["assetVersion"];

                if (upgradeInfo.appUpdateType == 1 || upgradeInfo.appUpdateType == 2)
                {
                    upgradeInfo.noticeTitle = jsonNode["appNotice"]["noticeTitle"];
                    upgradeInfo.noticeContent = jsonNode["appNotice"]["noticeContent"];
                }
                else if (upgradeInfo.assetUpdateType == 1)
                {
                    upgradeInfo.noticeTitle = jsonNode["assetNotice"]["noticeTitle"];
                    upgradeInfo.noticeContent = jsonNode["assetNotice"]["noticeContent"];
                }
            }
        }
       
        public AssetBundleInfo GetAssetBundleInfo(string assetBundleName)
        {
            var result = AssetBundleInfo.Empty;
            foreach (var kv in _versionInfo.assetBundleGroups)
            {
                if (kv.Value.AssetBundles.ContainsKey(assetBundleName))
                {
                    result = kv.Value.AssetBundles[assetBundleName];
                    return result;
                }
            }
            return result;
        }
        public AssetBundleInfo GuessBundleByAssetName(string assetName)
        {
            return _versionInfo.GuessBundleByAssetName(FilePathTools.NormalizePath(assetName.ToLower()));
        }


        private bool _finishMovingStreaming = true;
        /// <summary>
        /// 如果是第一次安装，将persistent下的文件移动到streaming下
        /// </summary>
        private IEnumerator MoveStreamingToPersistent()
        {
            _finishMovingStreaming = false;
            if (this._streamingVersionInfo == null)
            {
                Log("Fail to Load version file in streamingAssets ");
                yield break;
            }
            else
            {
                Log("Finish Load version file in streamingAssets ");
            }

            List<string> abFileNames = _streamingVersionInfo.GetAllBundles();
        
            abFileNames.Add(versionFileName);
            DownloadManager.DownloadBatchFiles(
                FilePathTools.streamingAssetsPath_Platform_ForWWWLoad,
                FilePathTools.persistentDataPath_Platform, abFileNames,
                ()=> {
                    Log("Finish Copy Bundle with DownloadManager");
                    _finishMovingStreaming = true;
                });
        }



        #region Bundle 下载相关
        /// <summary>
        /// 更新基础的和必须的AssetBundle
        /// </summary>
        /// <param name="assetUrl"></param>
        /// <param name="groupName"></param>
        /// <param name="progressInfo"></param>
        /// <param name="loadBaseBundle"></param>
        /// <returns></returns>
        public IEnumerator UpdateBaseAssetBundle(string assetUrl, ProgressInfo progressInfo, bool loadBaseBundle = false)
        {
            //下载服务器的Version 文件
            string serverVersionFilePath = assetUrl + "/Version.txt";
            DataContainer dataContainer = new DataContainer();
            yield return LoadVersionFile(serverVersionFilePath, dataContainer);
            if (string.IsNullOrEmpty(dataContainer.strData))
            {
                progressInfo.sucess = false;
                progressInfo.fail = true;
                progressInfo.start = true;
                DebugUtil.LogError("load version file fail ,Url:", assetUrl);
                yield break;
            }
            AssetBundleVersionInfo _serverVersionInfo = JsonConvert.DeserializeObject<AssetBundleVersionInfo>(dataContainer.strData);
            //临时存储versionInfo文件
            FileUtil.CreateFile(FilePathTools.persistentDataPath_Platform, "Version.txt.temp", dataContainer.strData);


            List<string> mustGroups = GetMustGroups(_serverVersionInfo.preloadConditions);
            List<AssetBundleGroupInfo> baseGroupinfos = new List<AssetBundleGroupInfo>();
            foreach (var item in _serverVersionInfo.assetBundleGroups.Values)
            {
                if (item.BaseGroup || mustGroups.Contains(item.GroupName))
                    baseGroupinfos.Add(item);
            }


            List<string> needLoadBundleNames = new List<string>();
            foreach (var group in baseGroupinfos)
            {
                foreach (var bundleInfo in group.AssetBundles)
                {
                    if (FileUtil.IsFileExistsInPersistetntDataPath(bundleInfo.Value.AssetBundleName, bundleInfo.Value.HashString))
                    {
                        continue;
                    }
                    needLoadBundleNames.Add(bundleInfo.Value.AssetBundleName);
                }
            }
            yield return LoadAssetBundle(assetUrl, needLoadBundleNames, progressInfo);


            //重命名为Version.txt
            FileUtil.Rename(FilePathTools.persistentDataPath_Platform + "/Version.txt.temp", FilePathTools.persistentDataPath_Platform + "/Version.txt");

            //更新_versionInfo
            //string persistentVersionFilePath = string.Format("{0}/{1}", FilePathTools.persistentDataPath_Platform_ForWWWLoad, versionFileName);
            //DataContainer dataContainer2 = new DataContainer();
            //yield return LoadVersionFile(persistentVersionFilePath, dataContainer2);
            //this._versionInfo = JsonConvert.DeserializeObject<AssetBundleVersionInfo>(dataContainer2.strData);
            this._versionInfo = _serverVersionInfo;
            this.assetUrl = assetUrl;//记录当前基础素材的 服务器地址 ，用于后面下载其他group资源。
        }


        /// <summary>
        /// 更新必须的AssetBundle
        /// </summary>
        /// <param name="assetUrl"></param>
        /// <param name="groupName"></param>
        /// <param name="progressInfo"></param>
        /// <param name="loadBaseBundle"></param>
        /// <returns></returns>
        public IEnumerator UpdateMustAssetBundle(string assetUrl, List<string> mustGroups, ProgressInfo progressInfo, bool loadBaseBundle = false)
        {
            List<AssetBundleGroupInfo> mustGroupinfos = new List<AssetBundleGroupInfo>();
            foreach (var item in _versionInfo.assetBundleGroups.Values)
            {
                if (mustGroups.Contains(item.GroupName) && !IsBundleGroupLoaded(item.GroupName))
                    mustGroupinfos.Add(item);
            }


            List<string> needLoadBundleNames = new List<string>();
            foreach (var group in mustGroupinfos)
            {
                foreach (var bundleInfo in group.AssetBundles)
                {
                    if (FileUtil.IsFileExistsInPersistetntDataPath(bundleInfo.Value.AssetBundleName, bundleInfo.Value.HashString))
                    {
                        continue;
                    }
                    needLoadBundleNames.Add(bundleInfo.Value.AssetBundleName);
                }
            }
            yield return LoadAssetBundle(assetUrl, needLoadBundleNames, progressInfo);
        }


        /// <summary>
        /// 下载 某个BundleGroup
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="quietDownLoad"></param>
        /// <returns></returns>
        public ProgressInfo LoadBundleGroup( string groupName, bool quietDownLoad = false)
        {
            AssetBundleGroupLoadHelperItem assetBundleGroupLoadHelperItem = GetAssetBundleGroupLoadHelperItem(groupName);
            if (assetBundleGroupLoadHelperItem != null)
            {
                return assetBundleGroupLoadHelperItem.Load(assetUrl, quietDownLoad);
            }
            return null;
        }



        /// <summary>
        /// 下载最新的AssetBundle
        /// </summary>
        /// <param name="assetUrl"></param>
        /// <param name="loadBaseBundle"></param>
        /// <returns></returns>
        public static IEnumerator LoadAssetBundle(string assetUrl, List<string> needLoadBundleNames, ProgressInfo progressInfo, bool quietDownLoad = false)
        {
            //下载
            progressInfo.totalCount = needLoadBundleNames.Count;
            progressInfo.start = true;
            int i = 0;
            while (i < needLoadBundleNames.Count)
            {
                float fileSize = Inst._versionInfo.GuessBundleByAssetName(needLoadBundleNames[i]).Size / 1024;
                int freeSpace = SimpleDiskUtils.DiskUtils.CheckAvailableSpace();
                if (fileSize > freeSpace)
                {
                    UIManager.Inst.ShowUI(UIModuleEnum.UICommonTipWithTitle, false, "Available space is not enough,please retry after release", "ok");
                    while (fileSize > freeSpace)
                    {
                        yield return new WaitForSeconds(2f);
                    }
                    freeSpace = SimpleDiskUtils.DiskUtils.CheckAvailableSpace();
                }

                progressInfo.curIndex = i;
                string filePath = string.Format("{0}/{1}", assetUrl, needLoadBundleNames[i]);
                yield return DownloadManager.DownLoadFile(needLoadBundleNames[i], filePath, FilePathTools.persistentDataPath_Platform, progressInfo, null);
                if (progressInfo.curIndexDone)
                {
                    i++;
                }
                else if (!quietDownLoad)
                {
                    bool closeTip = false;
                    Action tipCallback = () =>
                    {
                        closeTip = true;
                    };
                    DebugUtil.LogError("AssetBundleManager.LoadAssetBundle fail,filePath:" + filePath);
                    UIManager.Inst.ShowUI(UIModuleEnum.UICommonTipWithTitle, false, "Net_Problem_Tips" + "..", "Button_ok", tipCallback);
                    while (!closeTip)
                    {
                        yield return null;
                    }
                }
            }
            progressInfo.sucess = true;
        }

        // 加载本地version文件
        private static IEnumerator LoadVersionFile(string fileUrl, DataContainer dataContainer, Action onComplete = null)
        {
            Log("LoadVersionFile :{0}", fileUrl);


            UnityWebRequest www = new UnityWebRequest(fileUrl);
            DownloadHandlerBuffer downHandler = new DownloadHandlerBuffer();
            www.downloadHandler = downHandler;
            yield return www.SendWebRequest();
            if (string.IsNullOrEmpty(www.error))
            {
                Log("LoadVersionFile www.text:{0}", www.downloadHandler.text);
                dataContainer.strData = www.downloadHandler.text;
                www.Dispose();
                if (onComplete != null)
                    onComplete();
                Log("Finish load version file with WWW:{0}", fileUrl);
                yield break;
            }
            else
            {
                DebugUtil.LogError("load version file {0} fail,error:{1}", fileUrl, www.error);
            }
        }
        #endregion

        /// <summary>
        /// 某个BundleGroup是否下载
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public bool IsBundleGroupLoaded(string groupName)
        {
            AssetBundleGroupLoadHelperItem assetBundleGroupLoadHelperItem = GetAssetBundleGroupLoadHelperItem(groupName);
            if (assetBundleGroupLoadHelperItem != null)
            {
                return assetBundleGroupLoadHelperItem.IsLoaded;
            }
            return false;
        }


        public AssetBundleGroupLoadHelperItem GetAssetBundleGroupLoadHelperItem(string groupName)
        {
            if (!assetBundleGroupLoadHelpers.ContainsKey(groupName))
            {
                if (!_versionInfo.ContainGroup(groupName))
                {
                    return null;
                }
                AssetBundleGroupLoadHelperItem assetBundleGroupLoadHelperItem = AssetBundleGroupLoadHelperItem.Create(groupName, _versionInfo.GetGroup(groupName));
                assetBundleGroupLoadHelpers[groupName] = assetBundleGroupLoadHelperItem;
            }
            return assetBundleGroupLoadHelpers[groupName];
        }

        /// <summary>
        /// 获取必须下载的Room group
        /// </summary>
        public List<string> GetMustGroupsButNotLoad()
        {
            List<string> groupNotLoad = new List<string>();
            if (AssetBundleConfig.Inst.UseAssetBundle)
            {
                List<string> groupNames = GetMustGroups(this._versionInfo.preloadConditions);

                foreach (var item in groupNames)
                {
                    if (!IsBundleGroupLoaded(item))
                    {
                        groupNotLoad.Add(item);
                    }
                }
            }
            return groupNotLoad;
        }

        /// <summary>
        /// 获取必须下载的Room group
        /// </summary>
        public List<string> GetMustGroups(List<PreloadConditionItem> preloadConditionItems)
        {
            List<string> groupNames = new List<string>();

            int i = 0;
            while (preloadConditionItems != null && i < preloadConditionItems.Count)
            {
                DebugUtil.Log("preloadConditionItems[{0}].taskCondition:{1}",i, preloadConditionItems[i].taskCondition);
                DebugUtil.Log("preloadConditionItems[{0}].groupName:{1}", i, preloadConditionItems[i].groupName);
                //if (preloadConditionItems[i].taskCondition == 10000 || DataManager.Inst.taskInfo.TaskIsFinshed(preloadConditionItems[i].taskCondition))
                //{
                //    string namesStr = preloadConditionItems[i].groupName;
                //    if (!string.IsNullOrEmpty(namesStr) )
                //    {
                //        string[] names = namesStr.Split(',');
                //        if (names != null && names.Length > 0)
                //        {
                //            groupNames.AddRange(names);
                //        }
                //    }
                //}
                i++;
            }

            foreach (var item in groupNames)
            {
                DebugUtil.Log("---- AssetBundleManager GetMustGroups:{0}",item);
            }
            return groupNames;
        }
        public List<string> GetMustGroups()
        {
            if (this._versionInfo == null  || this._versionInfo.preloadConditions == null )
            {
                return new List<string>();
            }
            return GetMustGroups(this._versionInfo.preloadConditions);
        }

        public string GetVersion()
        {
            if(_versionInfo != null)
            {
                return _versionInfo.Version;
            }
            return "EmptyVersionInfo";
        }
        public string[] GetNotBaseGroupNames()
        {
            return _versionInfo.GetNotBaseGroupNames(); ;
        }
       

        private static void Log(string _log, params object[] args)
        {
            DebugUtil.Log(string.Format("AssetBundleManager:{0}",_log), args);
        }




    }
}


