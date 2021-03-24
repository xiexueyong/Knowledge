/*-------------------------------------------------------------------------------------------
// 模块名：CachedAssetBundleContainer
// 模块描述：AssetBundle包的缓存池
//-------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using UObject = UnityEngine.Object;

namespace Framework.Asset
{
    /// <summary>
    /// 缓存一组AssetBundle的容器
    /// </summary>
    public class CachedAssetBundleContainer:IdleClass
    {
        public readonly Dictionary<string, CachedAssetBundle> CachedAssetBundleDictionary = new Dictionary<string, CachedAssetBundle>();

        //检查是否有AssetBundle
        public bool CheckAssetBundleExists(string assetBundleName)
        {
            return CachedAssetBundleDictionary.ContainsKey(assetBundleName);
        }
        /// <summary>
        /// 获取 CachedAssetBundle
        /// </summary>
        /// <param name="assetBundleName">精确的bundle文件名</param>
        /// <returns></returns>
        public CachedAssetBundle GetCachedAssetBundle(string assetBundleName)
        {
            if (this.CachedAssetBundleDictionary.ContainsKey(assetBundleName) && this.CachedAssetBundleDictionary[assetBundleName] != null)
            {
                return this.CachedAssetBundleDictionary[assetBundleName];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取 CachedAssetBundle,如果没有该Bundle则通过LoadBundle下载
        /// </summary>
        /// <param name="assetBundleName">精确的bundle文件名</param>
        /// <returns></returns>
        public CachedAssetBundle LoadCachedAssetBundle(AssetBundleInfo bundleInfo)
        {
            LoadBundle(bundleInfo);

            if (this.CachedAssetBundleDictionary.ContainsKey(bundleInfo.AssetBundleName) && this.CachedAssetBundleDictionary[bundleInfo.AssetBundleName] != null)
            {
                return this.CachedAssetBundleDictionary[bundleInfo.AssetBundleName];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 加载Bundle,会加载该Bundle的依赖
        /// </summary>
        /// <param name="assetBundleName"></param>
        private void LoadBundle(AssetBundleInfo bundleInfo)
        {
            //加载自己
            if (!this.CachedAssetBundleDictionary.ContainsKey(bundleInfo.AssetBundleName) || this.CachedAssetBundleDictionary[bundleInfo.AssetBundleName] == null)
            {
                string url = FilePathTools.persistentDataPath_Platform + "/" + bundleInfo.AssetBundleName;
                AssetBundle ab = AssetUtils.LoadLocalAssetBundle(url);
                if (ab != null)
                {
                    CachedAssetBundle cab = new CachedAssetBundle(ab, bundleInfo, this);
                    this.CachedAssetBundleDictionary[bundleInfo.AssetBundleName] = cab;
                }
                else
                {
                    return;
                }
            }
            //加载依赖
            string[] dependencies = bundleInfo.DependenciesBundleNames;
            foreach (string fileName in dependencies)
            {
                LoadBundle(fileName);
            }
        }
        private void LoadBundle(string assetBundleName)
        {
            AssetBundleInfo abInfo = AssetBundleManager.Inst.GetAssetBundleInfo(assetBundleName);
            LoadBundle(abInfo);
        }



            /// <summary>
            /// 释放不使用的（Idle为true）资源，一般会定期、切换场景、收到内存警告时调用此函数，
            /// </summary>
        public void Release()
        {
            if(CachedAssetBundleDictionary.Count > 0)
            {
                List<string> unloadKeys = new List<string>();
                foreach (var key in CachedAssetBundleDictionary.Keys)
                {
                    if (CachedAssetBundleDictionary[key].bundleInfo.releaseMode == ReleaseMode.Persistent)
                        continue;

                    if (CachedAssetBundleDictionary[key].Idle && 
                        !IsDepended(key))
                    {
                        unloadKeys.Add(key);
                    }
                    else
                    {
                        CachedAssetBundleDictionary[key].Release();
                    }
                }
                if (unloadKeys.Count > 0)
                {
                    foreach(var item in unloadKeys)
                    {
                        Unload(item);
                    }

                }
            }
        }
        /// <summary>
        /// 卸载指定的Bundle
        /// </summary>
        /// <param name="assetBundleName"></param>
        public void Unload(string assetBundleName)
        {
            if (this.CachedAssetBundleDictionary.ContainsKey(assetBundleName) && this.CachedAssetBundleDictionary[assetBundleName] != null)
            {
                this.CachedAssetBundleDictionary[assetBundleName].Unload(true);
                this.CachedAssetBundleDictionary.Remove(assetBundleName);
            }
        }
        private bool IsDepended(string bundleName)
        {
            foreach (var key in CachedAssetBundleDictionary.Keys)
            {
                if (bundleName != key &&
                    AssetBundleManager.Inst.GetAssetBundleInfo(key).DependenciesBundleNames.Contains(bundleName))
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 卸载全部Bundle，一般用不到此功能
        /// </summary>
        public void UnloadAll(bool unloadAllLoadedObject = false)
        {
            foreach (var item in this.CachedAssetBundleDictionary.Values)
            {
                if (item != null && item.assetBundle != null)
                {
                    item.Unload(unloadAllLoadedObject);
                    //item.assetBundle.Unload(false);
                }
            }
            this.CachedAssetBundleDictionary.Clear();
        }


    }

    
}
