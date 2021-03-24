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
using EventUtil;
using Framework.Tables;

namespace Framework.Asset
{
    public class QuietDownLoadManager : D_MonoSingleton<QuietDownLoadManager>
    {

        //可以使用蜂窝下载
        public bool canLoadWithCellular;

        private bool _initQuietLoad;
        //下载完所有的资源
        private bool _finishLoad;
        private bool _finishMustLoad;
        private bool _finishPreLoad;

        private List<string> _mustLoadGroupNames;
        private List<string> _preLoadGroupNames;

        public QuietDownLoadInfo quietDownLoadInfo;

        private AssetBundleGroupLoadHelperItem curAssetBundleGroupLoadHelper;


        protected override void OnAwake()
        {
            EventDispatcher.AddEventListener<string>(EventKey.AssetBundleGroupLoadSucess, OnAssetBundleLoadSucess);
        }

        private void OnAssetBundleLoadSucess(string groupName)
        {
            if (_mustLoadGroupNames.Contains(groupName))
            {
                _mustLoadGroupNames.Remove(groupName);
                quietDownLoadInfo.curMustLoadIndex++;
            }
                
            if (_preLoadGroupNames.Contains(groupName))
            {
                _preLoadGroupNames.Remove(groupName);
                quietDownLoadInfo.curPreLoadIndex++;
            }
            curAssetBundleGroupLoadHelper = null;
            QuietLoad();
        }

        /// <summary>
        /// 可以进入游戏
        /// </summary>
        /// <returns></returns>
        public bool canEnterGame()
        {
            return _mustLoadGroupNames.Count <= 0;
        }

        /// <summary>
        /// 开始静默下载
        /// </summary>
        public void StartQuietLoad()
        {
            return;
            if (AssetBundleConfig.Inst.UseAssetBundle && !_finishLoad)
            {
                quietDownLoadInfo = new QuietDownLoadInfo();
                _initQuietLoad = true;
                SetQuietLoad();
                QuietLoad();
            }
        }
        /// <summary>
        /// 刷新静默下载
        /// </summary>
        public void RefreshQuietLoad()
        {
            return;
            if (AssetBundleConfig.Inst.UseAssetBundle && _finishLoad)
            {
                quietDownLoadInfo = new QuietDownLoadInfo();
                _finishLoad = false;
                _finishMustLoad = false;
                _finishPreLoad = false;
                SetQuietLoad();
                QuietLoad();
            }
        }
        private void QuietLoad()
        {

            quietDownLoadInfo.loadingStatus = LoadingStatus.Loading;
            //_mustLoadGroupNames = AssetBundleManager.Inst.GetNotBaseGroupNames();
            if (!_finishMustLoad && _mustLoadGroupNames.Count <= 0)
            {
                _finishMustLoad = true;
                EventDispatcher.TriggerEvent(EventKey.QuietLoadMustSucess);//完成必须下载的
            }
            if (!_finishPreLoad && _preLoadGroupNames.Count <= 0)
            {
                _finishPreLoad = true;
                EventDispatcher.TriggerEvent(EventKey.QuietLoadPreSucess);//完成预下载的
            }

            if (_mustLoadGroupNames.Count <= 0 && _preLoadGroupNames.Count <= 0)
            {
                _finishLoad = true;
                quietDownLoadInfo.loadingStatus = LoadingStatus.Finish;
                EventDispatcher.TriggerEvent(EventKey.QuietLoadAllSucess);//完成全部静默下载
            }
            
            //必下载的 
            for (int i = 0; i < _mustLoadGroupNames.Count; i++)
            {
                if (!AssetBundleManager.Inst.IsBundleGroupLoaded(_mustLoadGroupNames[i]))
                {
                    DebugUtil.Log("QuietDownLoadManager : start load {0}", _mustLoadGroupNames[i]);
                    AssetBundleManager.Inst.LoadBundleGroup(_mustLoadGroupNames[i],true);
                    curAssetBundleGroupLoadHelper = AssetBundleManager.Inst.GetAssetBundleGroupLoadHelperItem(_mustLoadGroupNames[i]);
                    return;
                }
            }
            //预下载的
            for (int i = 0; i < _preLoadGroupNames.Count; i++)
            {
                if (!AssetBundleManager.Inst.IsBundleGroupLoaded(_preLoadGroupNames[i]))
                {
                    DebugUtil.Log("QuietDownLoadManager : start load {0}", _preLoadGroupNames[i]);
                    AssetBundleManager.Inst.LoadBundleGroup(_preLoadGroupNames[i], true);
                    curAssetBundleGroupLoadHelper = AssetBundleManager.Inst.GetAssetBundleGroupLoadHelperItem(_preLoadGroupNames[i]);
                    return;
                }
            }

        }
        /// <summary>
        /// 暂停下载
        /// </summary>
        public void Pause()
        {
            if (_initQuietLoad && curAssetBundleGroupLoadHelper != null)
            {
                curAssetBundleGroupLoadHelper.Pause();
                curAssetBundleGroupLoadHelper = null;
                quietDownLoadInfo.loadingStatus = LoadingStatus.Pause;
                EventDispatcher.TriggerEvent(EventKey.QuietLoadStatusChanged);
            }
            
        }
        /// <summary>
        /// 恢复下载
        /// </summary>
        public void Resume()
        {
            if (_initQuietLoad && curAssetBundleGroupLoadHelper == null)
            {
                QuietLoad();
                EventDispatcher.TriggerEvent(EventKey.QuietLoadStatusChanged);
            }
        }
        /// <summary>
        /// 检查网络状况，如果是cellular 则停止下载，如果是wifi则启动下载
        /// </summary>
        private void CheckNetStatus()
        {
            if (curAssetBundleGroupLoadHelper != null && !canLoadWithCellular && DeviceHelper.GetNetworkStatus() == DeviceHelper.NetworkStatus.Cellular)
            {
                Pause();
                quietDownLoadInfo.networkStatus = DeviceHelper.NetworkStatus.Cellular;
            }
            else if (!_finishLoad && curAssetBundleGroupLoadHelper == null && DeviceHelper.GetNetworkStatus() == DeviceHelper.NetworkStatus.Wifi)
            {
                Resume();
                quietDownLoadInfo.networkStatus = DeviceHelper.NetworkStatus.Wifi;
            }else if (DeviceHelper.GetNetworkStatus() == DeviceHelper.NetworkStatus.NoNetwork)
            {
                quietDownLoadInfo.networkStatus = DeviceHelper.NetworkStatus.NoNetwork;
            }

        }


        /// <summary>
        /// 设置必须下载和预加载的资源队列
        /// </summary>
        private void SetQuietLoad()
        {
            _preLoadGroupNames = AssetBundleManager.Inst.GetMustGroups();
            _mustLoadGroupNames = new List<string>();

            quietDownLoadInfo.totalMustLoad = _mustLoadGroupNames.Count;
            quietDownLoadInfo.totalPreLoad = _preLoadGroupNames.Count;
            quietDownLoadInfo.networkStatus = DeviceHelper.GetNetworkStatus();

            foreach (var item in _preLoadGroupNames)
            {
                DebugUtil.Log("----QuietDownLoadManager ,_preLoadGroupNames:{0}",item);
            }
        }


        private int f = 0;
        public void Update()
        {
            //if (curAssetBundleGroupLoadHelper != null)
            //{
            //    DebugUtil.Log("=========================== {0}:{1}", curAssetBundleGroupLoadHelper.groupName, curAssetBundleGroupLoadHelper.progressInfo.progress);
            //}
            f++;
            if (_initQuietLoad && f > 180 && AssetBundleConfig.Inst.UseAssetBundle)
            {
                f = 0;
                CheckNetStatus();
            }
        }



        public enum LoadingStatus
        {
            Unknown = 0,
            Loading = 1,
            Pause = 2,
            Finish = 3
        }


        public class QuietDownLoadInfo
        {
            public int totalMustLoad = 0;
            public int curMustLoadIndex = 0;


            public int totalPreLoad = 0;
            public int curPreLoadIndex = 0;

            public DeviceHelper.NetworkStatus networkStatus;

            public LoadingStatus loadingStatus;

            public QuietDownLoadInfo()
            {


            }
            public int totalCount
            {
                get
                {
                    if (totalMustLoad > 0)
                        return totalMustLoad;
                    else if (totalPreLoad > 0)
                        return totalPreLoad;
                    else
                        return -1;
                }
            }
            public int curLoadIndex
            {
                get
                {
                    if (totalMustLoad > 0)
                        return curMustLoadIndex+1;
                    else if (totalPreLoad > 0)
                        return curPreLoadIndex+1;
                    else
                        return -1;
                }
            }

            public float cruProgress
            {
                get{
                    if (Inst.curAssetBundleGroupLoadHelper != null && Inst.curAssetBundleGroupLoadHelper.progressInfo != null)
                    {
                        return Inst.curAssetBundleGroupLoadHelper.progressInfo.progress;
                    }
                    else
                    {
                        return 0;
                    }
                }

            }


        }
    }
}




