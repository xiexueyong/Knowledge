/*-------------------------------------------------------------------------------------------
// 模块名：CachedAssetBundleContainer
// 模块描述：AssetBundle包的缓存池
//-------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;
using System.IO;
using UObject = UnityEngine.Object;

namespace Framework.Asset
{
    /// <summary>
    /// 缓存的AssetBundle
    /// </summary>
    public class CachedAssetBundle:IdleClass
    {
        public readonly Dictionary<string, CachedObject> CachedObjectDictionary = new Dictionary<string, CachedObject>();

        public string AssetBundleName { get; private set; }

        public AssetBundle assetBundle { get; private set; }

        public AssetBundleInfo bundleInfo;

        public CachedAssetBundle(AssetBundle assetBundle, AssetBundleInfo bundleInfo, CachedAssetBundleContainer _parent)
        {
            this.AssetBundleName = assetBundle.name;
            this.assetBundle = assetBundle;
            this.bundleInfo = bundleInfo;
        }

        /// <summary>
        /// 加载Bundle包内的文件，如果缓存不存在，则加载
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName">文件名，已经被处理过</param>
        /// <returns></returns>
        [Obsolete]
        private T LoadAsset<T>(string assetName,bool cache) where T : UObject
        {
            if (!CachedObjectDictionary.ContainsKey(assetName) || CachedObjectDictionary[assetName] == null)
            {
                T t = assetBundle.LoadAsset<T>(assetName);
                if (t != null)
                    CachedObjectDictionary[assetName] = new CachedObject(t, this);
            }
            if (CachedObjectDictionary.ContainsKey(assetName) && CachedObjectDictionary[assetName] != null)
            {
                return CachedObjectDictionary[assetName].GetAsset<T>(cache);
            }
            else
            {
                return null;
            }
        }



        /// <summary>
        /// 获取CachedObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public CachedObject LoadCachedObject<T>(string assetName) where T : UObject
        {
            if (!CachedObjectDictionary.ContainsKey(assetName) || CachedObjectDictionary[assetName] == null)
            {
                T t = assetBundle.LoadAsset<T>(assetName);
                if (t != null)
                    CachedObjectDictionary[assetName] = new CachedObject(t, this);
            }
            return GetCachedObject(assetName);
        }
        public override void CheckIdle()
        {
            base.CheckIdle();
            bool _idle = true;
            foreach (var item in CachedObjectDictionary.Values)
            {
                if (!item.Idle)
                {
                    _idle = false;
                }
            }
            Idle = _idle;
        }

        /// <summary>
        /// 获取CachedObject
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName"></param>
        /// <returns></returns>
        public CachedObject GetCachedObject(string assetName)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                return null;
            }
            if (CachedObjectDictionary.ContainsKey(assetName) && CachedObjectDictionary[assetName] != null)
            {
                return CachedObjectDictionary[assetName];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 释放无占用的（Idle为true）资源，一般会定期、切换场景、收到内存警告时调用此函数，
        /// </summary>
        public void Release()
        {
            List<string> unloadKeys = new List<string>();
            foreach (var item in CachedObjectDictionary)
            {
                item.Value.Release();
                if (item.Value.Idle)
                {
                    unloadKeys.Add(item.Key);
                    item.Value.Unload(true);
                }
            }
            foreach (var key in unloadKeys)
            {
                CachedObjectDictionary.Remove(key);
            }
        }
        /// <summary>
        /// 卸载此Bundle及缓存的文件，只能被父类调用
        /// </summary>
        public void Unload(bool unloadallLoadedObjects = false)
        {
            assetBundle.Unload(false);
            assetBundle = null;
            foreach (var item in CachedObjectDictionary.Values)
            {
                item.Unload(unloadallLoadedObjects);
            }
            CachedObjectDictionary.Clear();
        }

    }
}
