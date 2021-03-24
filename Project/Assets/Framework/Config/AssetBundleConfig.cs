using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Framework.Asset;
#if UNITY_EDITOR
using UnityEditor;
#endif
/// <summary>
/// 渠道
/// </summary>
public enum Appchannel
{
    unknown = 0,
    AppStore = 1,
    GooglePlay = 2,
}

/// <summary>
/// 销毁模式
/// </summary>
public enum ReleaseMode
{
    None = 0,
    Common = 1,
    Immediate = 2,
    Persistent = 3
}
/// <summary>
/// Group类型，
/// Required是游戏的基础，开始游戏时会下载
/// Option是可选的，只在特定的场景需要用
/// </summary>
public enum GroupType
{
    Required = 0,
    Option = 1
}

/// <summary>
/// Group类型，
/// Required是游戏的基础，开始游戏时会下载
/// Option是可选的，只在特定的场景需要用
/// </summary>
public enum GetBundleMethod
{
    All = 0,
    OnlyBase = 1
}


[Serializable]
public class MultiBundleInfo:BundleInfo
{
    // 多个Bundle模式，把path下的子目录都打为bundle，path下的文件不处理
    public bool Multiple = false;

    public List<BundleInfo> subBundleInfos = new List<BundleInfo>();

    [HideInInspector]
    public bool multipleView = false;

    public MultiBundleInfo(string path, bool baseBundle, ReleaseMode releaseMode):base(path,  baseBundle, releaseMode)
    {
    }

    public void update()
    {
        if (multipleView == Multiple)
            return;
        multipleView = Multiple;
        subBundleInfos.Clear();
        if (Multiple)
        {
            List<string> subFolds = FilePathTools.GetSubFoldPaths(FilePathTools.GetFullpathRelativeExport(Path));
            foreach (var subItem in subFolds)
            {
                string shortPath = FilePathTools.GetShortPathRelativeExport(subItem);
                subBundleInfos.Add(new BundleInfo(shortPath, this.BaseBundle,this.releaseMode));
            }

            List<string> filePaths = FilePathTools.GetFilePaths(FilePathTools.GetFullpathRelativeExport(Path));
            foreach (var filePath in filePaths)
            {
                string shortPath = FilePathTools.GetShortPathRelativeExport(filePath);
                subBundleInfos.Add(new BundleInfo(shortPath, this.BaseBundle, this.releaseMode));
            }
        }
    }
}
[Serializable]
public class BundleInfo
{
    // ab包路径
    public string Path;
    // 作为基础包打进包内
    public bool BaseBundle = false;

    public ReleaseMode releaseMode = ReleaseMode.Common;
    public BundleInfo(string path,bool baseBundle, ReleaseMode releaseMode)
    {
        this.releaseMode = releaseMode;
        Path = path;
        BaseBundle = baseBundle;
    }
}

[System.Serializable]
public class PreloadConditionItem
{
    public int taskCondition;
    public string groupName;
}


[Serializable]
public class BundleGroup
{
    // 作为基础包打进包内
    public bool BaseGroup = false;
    public string GroupName;
    public List<MultiBundleInfo> bundleInfos;

    /// <summary>
    /// 是否是基础包
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public bool IsBaseBundle(string path)
    {
        BundleInfo bundleInfo = GetBundleInfo(path);
        if (bundleInfo != null)
            return bundleInfo.BaseBundle;
        return false;
    }
    /// <summary>
    /// 销毁模式
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public ReleaseMode GetReleaseMode(string path)
    {
        BundleInfo bundleInfo = GetBundleInfo(path);
        if (bundleInfo != null)
            return bundleInfo.releaseMode;
        return ReleaseMode.Common;
    }

    /// <summary>
    /// 按照策略获取Bundle包的路径
    /// </summary>
    /// <returns></returns>
    public List<string> GetBundlePaths(GetBundleMethod getBundleMethod = GetBundleMethod.All)
    {
        List<string> _paths = new List<string>();
        foreach (MultiBundleInfo item in bundleInfos)
        {
            if (item.Multiple)
            {
                foreach (BundleInfo subItem in item.subBundleInfos)
                {
                    if (!subItem.BaseBundle && getBundleMethod == GetBundleMethod.OnlyBase)
                        continue;
                    _paths.Add(subItem.Path);
                }
            }
            else 
            {
                if (!item.BaseBundle && getBundleMethod == GetBundleMethod.OnlyBase)
                    continue;
                _paths.Add(item.Path);
            }
        }
        return _paths;
    }

    /// <summary>
    /// 获取BundleInfo
    /// </summary>
    /// <param name="abPath"></param>
    /// <returns></returns>
    public BundleInfo GetBundleInfo(string abPath)
    {
        foreach (MultiBundleInfo item in bundleInfos)
        {
            if (item.Multiple)
            {
                foreach (BundleInfo subItem in item.subBundleInfos)
                {
                    if (FilePathTools.NormalizePath(subItem.Path).ToLower() == abPath.ToLower())
                        return subItem;
                }
            }
            else
            {
                if (item.Path.ToLower() == abPath.ToLower())
                    return item;
            }
        }
        return null;
    }
}

public class AssetBundleConfig : ScriptableObject
{
    public static string AssetConfigPath = "Settings/AssetBundleConfig";
    private static AssetBundleConfig _instance = null;
    
    [HideInInspector]
    private Appchannel appChannel = Appchannel.AppStore;

    public Appchannel AppChannel
    {
        get
        {
            
#if UNITY_IOS
            _instance.appChannel = Appchannel.AppStore;
#elif UNITY_ANDROID
            _instance.appChannel = Appchannel.GooglePlay;
#else
            _instance.appChannel = Appchannel.unknown;
#endif
            return _instance.appChannel;
        }
    }

    public static AssetBundleConfig Inst
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<AssetBundleConfig>(AssetConfigPath);

            }
            return _instance;
        }
    }



    [Space(10)]
    [Header("--------------------- App版本号和渠道 ----------------")]
    public string appVersion = "1.0.0";
    /// <summary>
    /// Version分为3位 eg:2.0.0
    /// 第一位: App的版本号
    /// 第二位: Asset资源的版本号
    /// 第三位: Asset资源的副版本号，资源相同，主要用于区分完整包和非完整包的Asset。副版本+1可以激活资源下载。
    /// 
    /// </summary>
    [Header("--------------------- 资源版本号 ----------------------")]
    public string AssetVersion = "1.0.0";

    [Space(10)]
    //[Header("------------------ UseAssetBundle--------------------")]
    //[HideInInspector]
    public bool UseAssetBundle = false;
    public bool ReleaseImmediate = false;
    //完整素材 打进包内
    [Header("完整素材")]
    public bool CompleteAsset = true;

    [Space(10)]
    [Header("---------------------- AB包分组 -----------------------")]
    [Header("[填入相对Assets/Export的相对路径，大小写敏感，只支持文件夹]")]
    public BundleGroup[] Groups;

    [Space(10)]
    [Header("预加载条件")]
    public List<PreloadConditionItem> preloadConditions;
    public bool LocalMode()
    {
        return true;
    }

    public bool BundleMode()
    {
        return UseAssetBundle;
    }

    public BundleInfo GetAssetBundleInfo(string abName)
    {
        if (string.IsNullOrEmpty(abName))
        {
            return null;
        }
        foreach (var bundleGroup in Groups)
        {
            foreach (var multiBundle in bundleGroup.bundleInfos)
            {
                if (multiBundle.Multiple)
                {
                    foreach (var subBundleInfo in multiBundle.subBundleInfos)
                    {
                        if (subBundleInfo.Path == abName)
                        {
                            return subBundleInfo;
                        }
                    }
                }
                else
                {
                    if (multiBundle.Path == abName)
                        return multiBundle;
                }
             
            }
        }
        return null;
    }

    public BundleInfo GuessBundleByAssetName(string assetName)
    {
        if (string.IsNullOrEmpty(assetName))
        {
            return null;
        }
        int assetNameLen = assetName.Length;
        foreach (var bundleGroup in Groups)
        {
            foreach (var multiBundle in bundleGroup.bundleInfos)
            {
                if (multiBundle.Multiple)
                {
                    foreach (var subBundleInfo in multiBundle.subBundleInfos)
                    {
                        int bundleNameLen = subBundleInfo.Path.Length;
                        if (assetNameLen > bundleNameLen + 1 && assetName[bundleNameLen] == '/' && assetName.Contains(subBundleInfo.Path))
                        {
                            return subBundleInfo;
                        }
                        else if (assetNameLen == bundleNameLen && assetName == subBundleInfo.Path)
                        {
                            return subBundleInfo;
                        }
                    }
                }
                else
                {
                    int bundleNameLen = multiBundle.Path.Length;
                    if (assetNameLen > bundleNameLen + 1 && assetName[bundleNameLen] == '/' && assetName.Contains(multiBundle.Path))
                    {
                        return multiBundle;
                    }
                    else if (assetNameLen == bundleNameLen && assetName == multiBundle.Path)
                    {
                        return multiBundle;
                    }
                }
               
            }

        }
 
        return null;
    }

    public void update()
    {
        foreach (var item in this.Groups)
        {
            foreach (var bundleInfo in item.bundleInfos)
            {
                bundleInfo.update();
            }
        }
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(AssetBundleConfig))]
public class AssetBundleConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        DrawDefaultInspector();
        AssetBundleConfig assetBundleConfig = target as AssetBundleConfig;
        assetBundleConfig.update();
    }
}
#endif

