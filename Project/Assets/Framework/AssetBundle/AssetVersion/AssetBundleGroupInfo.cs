/*-------------------------------------------------------------------------------------------
// 模块名：VersionItemInfo
// 模块描述：一组assetbundle包的描述结构
//-------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.Linq;

namespace Framework.Asset
{
    [System.Serializable]
    public class AssetBundleGroupInfo
    {
        // 作为基础包打进包内
        public bool BaseGroup = false;
        public string GroupName;
        public Dictionary<string, AssetBundleInfo> AssetBundles;
        public AssetBundleGroupInfo(string groupName, bool baseGroup)
        {
            BaseGroup = baseGroup;
            GroupName = groupName;
            AssetBundles = new Dictionary<string, AssetBundleInfo>();
        }
        /// <summary>
        /// 是否包含 AssetBundleInfo
        /// </summary>
        /// <param name="abName"></param>
        /// <returns></returns>
        public bool Contain(string abName)
        {
            return AssetBundles.ContainsKey(abName) && !AssetBundles[abName].Equals(AssetBundleInfo.Empty);
        }

        public AssetBundleInfo Get(string abiName)
        {
            if (Contain(abiName))
                return AssetBundles[abiName];
            return AssetBundleInfo.Empty;
        }


        public void Add(string key, AssetBundleInfo value)
        {
            if (this.AssetBundles.ContainsKey(key))
            {
                UnityEngine.Debug.LogError("AssetBundle重名:" + key);
            }
            else
            {
                this.AssetBundles.Add(key, value);
            }
        }

        public List<string> GetAllBundleNames()
        {
            return AssetBundles.Keys.ToList();
        }

        public void Refresh(Dictionary<string, AssetBundleInfo> remoteDict)
        {
            AssetBundles.Clear();
            foreach (KeyValuePair<string, AssetBundleInfo> kv in remoteDict)
            {
                AssetBundles.Add(kv.Key, kv.Value);
            }
        }

        public void Refresh(string key, AssetBundleInfo value)
        {
            AssetBundleInfo newABI = new AssetBundleInfo();
            newABI.AssetBundleName = value.AssetBundleName;
            newABI.DependenciesBundleNames = value.DependenciesBundleNames;
            newABI.HashString = value.HashString;

            if (AssetBundles.ContainsKey(key))
            {
                AssetBundles[key] = newABI;
            }
            else
            {
                AssetBundles.Add(key, newABI);
            }
        }
    }
}
