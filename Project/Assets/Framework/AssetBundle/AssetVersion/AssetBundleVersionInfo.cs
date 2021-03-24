/*-------------------------------------------------------------------------------------------
// 模块名：VersionInfo
// 模块描述：version文件的数据结构，用来描述所有assetbundle包
//-------------------------------------------------------------------------------------------*/
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Asset
{
    [System.Serializable]
    public class AssetBundleVersionInfo
    {
        public string Version;

        public Dictionary<string, AssetBundleGroupInfo> assetBundleGroups;
        public List<PreloadConditionItem> preloadConditions;

        public AssetBundleVersionInfo()
        {
            assetBundleGroups = new Dictionary<string, AssetBundleGroupInfo>();
        }
        /// <summary>
        /// 是否包含
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public bool ContainGroup(string groupName)
        {
            return assetBundleGroups.ContainsKey(groupName) && assetBundleGroups[groupName] != null;
        }
        /// <summary>
        /// 获取Group
        /// </summary>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public AssetBundleGroupInfo GetGroup(string groupName)
        {
            if (ContainGroup(groupName))
                return assetBundleGroups[groupName];
            return null;
        }
        public string[] GetGroupNames()
        {
            return assetBundleGroups.Keys.ToArray();
        }
        public string[] GetNotBaseGroupNames()
        {
            List<string> names = new List<string>();
            foreach (var item in assetBundleGroups)
            {
                if (!item.Value.BaseGroup)
                {
                    names.Add(item.Value.GroupName);
                }
            }
            names.Sort((x, y) =>
            {
                int ix = getNameIndex(x);
                int iy = getNameIndex(y);
                if (ix < iy)
                {
                    return -1;
                }
                else if (ix == iy)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            });
            return names.ToArray();
        }
        private int getNameIndex(string name)
        {
            string[] datas = name.Split('_');
            if (datas.Length >= 2)
            {
                int i = 100;
                int.TryParse(datas[1],out i);
                return i;
            }
            else
            {
                return 100;
            }
        }

        public void Add(string key, AssetBundleGroupInfo value)
        {
            if (this.assetBundleGroups.ContainsKey(key))
            {
                UnityEngine.Debug.LogError("资源组重名:" + key);
            }
            else
            {
                this.assetBundleGroups.Add(key, value);
            }
        }
        public List<string> GetAllBundles()
        {
            List<string> allABFiles = new List<string>();
            foreach (AssetBundleGroupInfo group in assetBundleGroups.Values)
            {
                allABFiles.AddRange(group.GetAllBundleNames());
            }
            return allABFiles;
    
        }
        /// <summary>
        /// 获取某个Group内的所有的Bundle文件名
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<string> GetAssetBundlesByGroupName(string key)
        {
            List<string> names = new List<string>();
            if (assetBundleGroups.ContainsKey(key))
            {
                foreach (string name in assetBundleGroups[key].AssetBundles.Keys)
                {
                    names.Add(name);
                }
            }
            return names;
        }

        public string GetAssetBundleByKeyAndName(string key, string name)
        {
            string abname = string.Empty;


            if (assetBundleGroups.ContainsKey(key) && !string.IsNullOrEmpty(name))
            {
                if (assetBundleGroups[key].AssetBundles.ContainsKey(name))
                {
                    abname = assetBundleGroups[key].AssetBundles[name].AssetBundleName;
                }
            }
            return abname;
        }
        public AssetBundleInfo GuessBundleByAssetName(string assetName)
        {
            if (string.IsNullOrEmpty(assetName))
            {
                return AssetBundleInfo.Empty;
            }
            int assetNameLen = assetName.Length;
            foreach (var group in assetBundleGroups.Values)
            {
                foreach (var bundleName in group.AssetBundles.Keys)
                {
                    int bundleNameLen = bundleName.Length;
                    if (assetNameLen > bundleNameLen + 1 && assetName[bundleNameLen] == '/' && assetName.Contains(bundleName))
                    {
                        return group.AssetBundles[bundleName];
                    }else if (assetNameLen == bundleNameLen && assetName == bundleName)
                    {
                        return group.AssetBundles[bundleName];
                    }
                }
            }
            return AssetBundleInfo.Empty;
        }

        public string GetAssetBundleHash(string key, string name)
        {
            string strMd5 = "";
            if (assetBundleGroups.ContainsKey(key) && !string.IsNullOrEmpty(name))
            {
                if (assetBundleGroups[key].AssetBundles.ContainsKey(name))
                {
                    strMd5 = assetBundleGroups[key].AssetBundles[name].HashString;
                }
                else
                {
                    UnityEngine.Debug.LogError("GetAssetBundleHash error2:" + key + "->" + name);
                }
            }
            else
            {
                UnityEngine.Debug.LogError("GetAssetBundleHash error:" + key + "->" + name);
            }
            return strMd5;
        }
        public AssetBundleVersionInfo SubAssetVersionIncrease()
        {
            Version = CommonUtil.SubAssetVersionIncrease(Version);
            return this;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// 两个AssetBundleVersionInfo的差异
        /// </summary>
        /// <param name="oldInfo"></param>
        /// <param name="newInfo"></param>
        /// <returns></returns>
        //public static AssetBundleVersionInfo Diff(AssetBundleVersionInfo oldInfo, AssetBundleVersionInfo newInfo)
        //{
        //    AssetBundleVersionInfo diffABVInfo = new AssetBundleVersionInfo();
        //    foreach (var abgroup in newInfo.assetBundleGroups)
        //    {
        //        if (!oldInfo.ContainGroup(abgroup.Key))
        //        {
        //            diffABVInfo.Add(abgroup.Key,abgroup.Value);
        //            continue;
        //        }
        //        foreach (var abinfo in abgroup.Value.AssetBundles)
        //        {
        //        }
        //    }
        //    return diffABVInfo;
        //}
    }
}
