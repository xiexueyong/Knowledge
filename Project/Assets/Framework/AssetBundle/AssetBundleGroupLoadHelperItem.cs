/*-------------------------------------------------------------------------------------------
// 模块名：AssetBundleGroupLoadHelperItem
// 用于辅助下载 AssetBundleGroup
//-------------------------------------------------------------------------------------------*/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Networking;
using EventUtil;

namespace Framework.Asset
{
    public enum LoadingStatus
    {
        None = 0,
        Loading = 1,
        Pause = 2,
        Fail = 3
    }

    public class AssetBundleGroupLoadHelperItem:MonoBehaviour
    {
        //group的名字
        public string groupName;
        //对应的BundleGroupInfo
        public AssetBundleGroupInfo assetBundleGroupInfo;
        //下载过程
        public ProgressInfo progressInfo;
        //是否下载成功
        private bool _isLoaded;

        public string assetUrl;
        public bool quietDownLoad;

        public LoadingStatus loadingStatus;


        public void Awake()
        {
           
        }
        public void Start()
        {

        }

        public static AssetBundleGroupLoadHelperItem Create(string _groupName, AssetBundleGroupInfo _assetBundleGroupInfo)
        {
            GameObject gameObject = new GameObject();
            gameObject.transform.SetParent(AssetBundleManager.Inst.transform);
            AssetBundleGroupLoadHelperItem helper = gameObject.AddComponent<AssetBundleGroupLoadHelperItem>();
            helper.Init(_groupName, _assetBundleGroupInfo);
            return helper;
        }
        public void Init(string _groupName, AssetBundleGroupInfo _assetBundleGroupInfo)
        {
            groupName = _groupName;
            assetBundleGroupInfo = _assetBundleGroupInfo;
            progressInfo = new ProgressInfo();
        }

        /// <summary>
        /// 下载某个Group内的Bundle
        /// </summary>
        /// <param name="assetUrl"></param>
        /// <param name="groupName"></param>
        /// <param name="progressInfo"></param>
        /// <param name="quietDownLoad"></param>
        /// <returns></returns>
        public ProgressInfo Load(string assetUrl, bool quietDownLoad = false)
        {
            if (string.IsNullOrEmpty(assetUrl))
            {
                DebugUtil.LogError("AssetBundleGroupLoadHelperItem asset url is null");
                progressInfo.fail = true;
                progressInfo.sucess = false;
                return progressInfo;
            }
            if (loadingStatus == LoadingStatus.Loading)
            {
                return progressInfo;
            }
            if (IsLoaded)
            {
                return null;
            }

            this.assetUrl = assetUrl;
            this.quietDownLoad = quietDownLoad;

            List<string> needLoadBundleNames = new List<string>();
            foreach (var bundleInfo in this.assetBundleGroupInfo.AssetBundles)
            {
                if (FileUtil.IsFileExistsInPersistetntDataPath(bundleInfo.Value.AssetBundleName, bundleInfo.Value.HashString))
                {
                    continue;
                }
                needLoadBundleNames.Add(bundleInfo.Value.AssetBundleName);
            }
            progressInfo.totalCount = assetBundleGroupInfo.AssetBundles.Count;
            progressInfo.startIndex = assetBundleGroupInfo.AssetBundles.Count - needLoadBundleNames.Count;
            StartCoroutine(LoadAssetBundleGroupIEnumerator(assetUrl,needLoadBundleNames,quietDownLoad));
            return progressInfo;
        }


        private IEnumerator LoadAssetBundleGroupIEnumerator(string assetUrl, List<string> needLoadBundleNames, bool quietDownLoad = false)
        {
            //下载
            loadingStatus = LoadingStatus.Loading;
            progressInfo.start = true;
            int i = 0;
            while (i < needLoadBundleNames.Count)
            {
                float fileSize = this.assetBundleGroupInfo.Get("needLoadBundleNames").Size/1024;
                int freeSpace = SimpleDiskUtils.DiskUtils.CheckAvailableSpace();
                if (fileSize > freeSpace)
                {
                    UIManager.Inst.ShowUI(UIModuleEnum.UICommonTipWithTitle, false, "Available space is not enough,please retry after release", "ok");
                    while (fileSize > freeSpace)
                    {
                        if (loadingStatus == LoadingStatus.Pause)
                        {
                            yield break;
                        }
                        yield return new WaitForSeconds(3f);
                        freeSpace = SimpleDiskUtils.DiskUtils.CheckAvailableSpace();
                    }
                }


                progressInfo.curIndex = progressInfo.startIndex + i;
                //yield return DownloadManager.DownLoadFile(needLoadBundleNames[i], string.Format("{0}/{1}", assetUrl, needLoadBundleNames[i]), FilePathTools.persistentDataPath_Platform, progressInfo, null);
                string fileName = needLoadBundleNames[i];
                string filePath = string.Format("{0}/{1}", assetUrl, needLoadBundleNames[i]);
                string desFolderName = FilePathTools.persistentDataPath_Platform;
                using (UnityWebRequest www = new UnityWebRequest(filePath))
                {
                    DownloadHandlerBuffer downHandler = new DownloadHandlerBuffer();
                    www.downloadHandler = downHandler;
                    www.SendWebRequest();
                    while (!www.isDone)
                    {
                        if (loadingStatus == LoadingStatus.Pause)
                        {
                            //设置为Pause状态时，退出下载
                            www.Abort();
                            www.Dispose();
                            yield break;
                        }
                        if (progressInfo != null)
                            progressInfo.curProgress = www.downloadProgress;
                        yield return null;
                    }
                    if (string.IsNullOrEmpty(www.error))
                    {
                        byte[] b = www.downloadHandler.data;
                        string destFile = string.Format("{0}/{1}", desFolderName, fileName);
                        string destDir = Path.GetDirectoryName(destFile);
                        if (!Directory.Exists(destDir))
                        {
                            Directory.CreateDirectory(destDir);
                        }

                        if (Directory.Exists(destDir))
                        {
                            Log("AssetBundleGroupLoadHelperItem : create fold {0} sucess", destDir);
                        }
                        else
                        {
                            Log("AssetBundleGroupLoadHelperItem : create fold {0} Fail", destDir);
                        }

                        if (File.Exists(destFile))
                        {
                            File.Delete(destFile);
                        }

                        try
                        {
                            using (FileStream fs = new FileStream(destFile, FileMode.Create))
                            {
                                fs.Seek(0, SeekOrigin.Begin);
                                fs.Write(b, 0, b.Length);
                                fs.Close();
                                Log("AssetBundleGroupLoadHelperItem :write sucess: {0},length:{1}", fileName, b.Length);
                            }
                        }
                        catch (Exception e)
                        {
                            Log("AssetBundleGroupLoadHelperItem: save to local exception:{0}", e.ToString());
                        }
                        if (File.Exists(destFile))
                        {
                            if (progressInfo != null)
                                progressInfo.curIndexDone = true;
                        }
                        else
                        {
                            if (progressInfo != null)
                                progressInfo.curIndexDone = false;
                        }
                    }
                    else
                    {
                        Log("AssetBundleGroupLoadHelperItem,file path:{0} , error:{1}" ,filePath, www.error);
                        if (progressInfo != null)
                            progressInfo.curIndexDone = false;

                        yield return new WaitForSeconds(3f);
                    }
                }

                if (progressInfo.curIndexDone)
                {
                    i++;
                }
                else if (!quietDownLoad)
                {
                    //if (!UIManager.Inst.IsUIVisible(UIModuleEnum.UICommonTipWithTitle))
                        //UIManager.Inst.ShowUI(UIModuleEnum.UICommonTipWithTitle, "", LanguageTool.Get("Net_Problem_Tips"), LanguageTool.Get("Button_ok"));
                }
            }
            progressInfo.sucess = true;
            EventDispatcher.TriggerEvent<string>(EventKey.AssetBundleGroupLoadSucess, groupName);
        }

        public void Pause()
        {
            loadingStatus = LoadingStatus.Pause;
        }

        public void Resume()
        {
            Load(this.assetUrl,this.quietDownLoad);
        }

        public bool IsLoaded
        {
            get
            {
                if (_isLoaded)
                {
                    return true;
                }
                if (progressInfo != null && progressInfo.sucess)
                {
                    return true;
                }
                foreach (var item in this.assetBundleGroupInfo.AssetBundles)
                {
                    if (!FileUtil.IsFileExistsInPersistetntDataPath(item.Key, item.Value.HashString))
                    {
                        return false;
                    }
                }
                _isLoaded = true;
                return true;
            }
            set
            {
                _isLoaded = value;
            }


         
        }

        private static void Log(string str, params object[] args)
        {
            DebugUtil.Log("AssetBundleGroupLoadHelperItem: " + str,args);
        }

    }
}
