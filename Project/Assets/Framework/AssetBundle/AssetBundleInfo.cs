/*-------------------------------------------------------------------------------------------
// 模块名：AssetBundleInfo
// 模块描述：单个ab包的描述
//-------------------------------------------------------------------------------------------*/
using UnityEngine;
using System;

namespace Framework.Asset
{
    [System.Serializable]
    public struct AssetBundleInfo
    {
        // 释放策略
        public ReleaseMode releaseMode;

        public static AssetBundleInfo Empty = new AssetBundleInfo();
        // ab包名
        public string AssetBundleName;

        // 依赖列表
        public string[] DependenciesBundleNames;

        // MD5值
        public string HashString;

        // 文件大小,以K为单位
        public int Size;

        public bool BaseBundle;

        public bool Equals(AssetBundleInfo obj)
        {
            bool b1 = releaseMode == obj.releaseMode && AssetBundleName == obj.AssetBundleName && HashString == obj.HashString;
            if (!b1)
                return false;

            //依赖都是空或长度为0
            if((DependenciesBundleNames == null || DependenciesBundleNames.Length == 0) && (obj.DependenciesBundleNames == null || obj.DependenciesBundleNames.Length == 0))
                return true;

            //每个依赖都一样
            if (DependenciesBundleNames != null && obj.DependenciesBundleNames != null && DependenciesBundleNames.Length == obj.DependenciesBundleNames.Length)
            {
                for (int i = 0;i < DependenciesBundleNames.Length;i++)
                {
                    if (DependenciesBundleNames[i] != obj.DependenciesBundleNames[i])
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        public AssetBundleInfo(ReleaseMode _releaseMode= ReleaseMode.None, string _AssetBundleName = null, string[] _DependenciesBundleNames=null, string _HashString=null,bool _BaseBundle = true,int size = 0)
        {
            releaseMode = _releaseMode;
            AssetBundleName = _AssetBundleName;
            DependenciesBundleNames = _DependenciesBundleNames;
            HashString = _HashString;
            BaseBundle = _BaseBundle;
            Size = size;
        }
        public bool isEmpty()
        {
            return this .Equals( AssetBundleInfo.Empty);
        }


    }
}
