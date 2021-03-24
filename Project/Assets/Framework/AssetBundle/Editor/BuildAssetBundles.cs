/*-------------------------------------------------------------------------------------------
// 模块名：BuildAssetBundles
// 模块描述：AssetBundle打包，加密
//-------------------------------------------------------------------------------------------*/
using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using Newtonsoft.Json;

namespace Framework.Asset
{
    public static class BuildAssetBundles
    {
        // 版本文件
        private static AssetBundleVersionInfo assetBundleVersionInfo;

        // assetbundle输出路径
        static string exportPathParent = null;

        [MenuItem("AssetBundle/Build/CurTarget")]
        public static void BuildAllAssetBundleDefault()
        {
            BuildAllAssetBundle(BuildTarget.NoTarget);
        }
        [MenuItem("AssetBundle/Build/Android")]
        public static void BuildAllAssetBundleAndroid()
        {
            BuildAllAssetBundle(BuildTarget.Android);
        }
        [MenuItem("AssetBundle/Build/IOS")]
        public static void BuildAllAssetBundleIOS()
        {
            BuildAllAssetBundle(BuildTarget.iOS);
        }




        /// <summary>
        /// Bundle打包，并生成Version配置文件
        /// 1.Bundle包导出至AssttBundleOut内
        /// 2.把基础包拷贝到StreamingAssets文件夹内
        /// 3.生成VersionInfo文件，拷贝到StreamingAssets内
        /// </summary>
        public static void BuildAllAssetBundle(BuildTarget buildTarget = BuildTarget.NoTarget)
        {


            if (AssetBundleConfig.Inst.CompleteAsset)
            {
                string _assetVersion = AssetBundleConfig.Inst.AssetVersion;
            }
            else
            {
                string _assetVersion = CommonUtil.SubAssetVersionIncrease(AssetBundleConfig.Inst.AssetVersion);
            }
            if (!Directory.Exists(FilePathTools.assetBundleOutPath))
                Directory.CreateDirectory(FilePathTools.assetBundleOutPath);

            exportPathParent = FilePathTools.assetBundleOutPath + "/" + AssetBundleConfig.Inst.AssetVersion;
            //EditorUtility.DisplayCancelableProgressBar("打包中...", "0%", 0.01f);
            AssetDatabase.RemoveUnusedAssetBundleNames();

            assetBundleVersionInfo = new AssetBundleVersionInfo
            {
                Version = AssetBundleConfig.Inst.AssetVersion,
                preloadConditions = AssetBundleConfig.Inst.preloadConditions
            };

            //EditorUtility.DisplayCancelableProgressBar("打包中...", "10%", 0.1f);
            //删除老的bundle，(如果不删除可能会引起打包崩溃，但基本没发生过)
            //(不删除老的bundle，重新打bundle会很快)
            //if (Directory.Exists(exportPath))
            //    Directory.Delete(exportPath, true);

            if (Directory.Exists(FilePathTools.streamingAssetsPath_Platform))
                Directory.Delete(FilePathTools.streamingAssetsPath_Platform, true);

            DirectoryInfo outFold = new DirectoryInfo(FilePathTools.assetBundleOutPath);
            DirectoryInfo[] outBundleFolds  = outFold.GetDirectories();
            if (outBundleFolds.Length > 0)
            {
                DirectoryInfo exportFold = outBundleFolds[0];
                string foldName = exportFold.Name;
                if(foldName != AssetBundleConfig.Inst.AssetVersion)
                    exportFold.MoveTo(exportPathParent);
            }

            if (!Directory.Exists(exportPathParent))
                Directory.CreateDirectory(exportPathParent);

            //EditorUtility.ClearProgressBar();
            float deltaProgress = 0.8f / AssetBundleConfig.Inst.Groups.Length;
            //按Group打包
            for (int i = 0; i < AssetBundleConfig.Inst.Groups.Length; i++)
            {
                float currProgress = 0.1f + deltaProgress * i;
                //EditorUtility.DisplayCancelableProgressBar("打包中...", currProgress * 100 + "%", currProgress);
                BundleGroup bundleGroup = AssetBundleConfig.Inst.Groups[i];
                BuildGroup(bundleGroup, Path.Combine(exportPathParent, bundleGroup.GroupName), buildTarget);
            }

            //保存version信息，把version存到exportPathParent的上一级目录
            string assetBundleVersionInfoStr = assetBundleVersionInfo.ToString();
            string versionFileName = "Version.txt";
            FileUtil.CreateFile(exportPathParent, versionFileName, assetBundleVersionInfoStr);
            //EditorUtility.DisplayCancelableProgressBar("打包中...", "90%", 0.90f);

            //拷贝 InInitialPacket==true 的包到streamingasset文件夹下,Version.txt文件也拷贝到streamingasset文件夹下
            if (!Directory.Exists(FilePathTools.streamingAssetsPath_Platform))
                Directory.CreateDirectory(FilePathTools.streamingAssetsPath_Platform);

            foreach (BundleGroup group in AssetBundleConfig.Inst.Groups)
            {
                List<string> paths = group.GetBundlePaths(GetBundleMethod.All);
                // 拷贝到AssetBundleOutRelease下
                foreach (string item in paths)
                {
                    string srcFile = Path.Combine(Path.Combine(exportPathParent, group.GroupName), item.ToLower());
                    string destFile = Path.Combine(FilePathTools.assetBundleOutReleasePath + "/" + AssetBundleConfig.Inst.AssetVersion, item.ToLower());

                    //判断文件的存在
                    if (System.IO.File.Exists(srcFile))
                    {
                        //存在文件
                        FileUtil.CopyFile(srcFile, destFile);
                    }    
                }
                if (!group.BaseGroup && !AssetBundleConfig.Inst.CompleteAsset)
                    continue;

                // 拷贝到streamingAsset下
                foreach (string item in paths)
                {
                    string srcFile = Path.Combine(Path.Combine(exportPathParent, group.GroupName), item.ToLower());
                    string destFile = FilePathTools.streamingAssetsPath_Platform + "/" + item.ToLower();
                    //判断文件的存在
                    if (System.IO.File.Exists(srcFile))
                    {
                        //存在文件
                        FileUtil.CopyFile(srcFile, destFile);
                    }
                }
            }
            FileUtil.CopyFile(exportPathParent + "/" + versionFileName, FilePathTools.streamingAssetsPath_Platform + "/" + "Version.txt");
            //如果不是CompleteAsset，AssetbundleOut下Version的副素材版本号+1。
            //if (!AssetBundleConfig.Inst.CompleteAsset)
            //{
            //    FileUtil.CreateFile(exportPathParent, "Version.txt", assetBundleVersionInfo.SubAssetVersionIncrease().ToString());
            //}
            //AssetBundleOut文件夹下Version拷贝到All文件夹下
            FileUtil.CopyFile(exportPathParent + "/" + versionFileName, FilePathTools.assetBundleOutReleasePath + "/" + AssetBundleConfig.Inst.AssetVersion + "/" + "Version.txt");


            //EditorUtility.DisplayCancelableProgressBar("打包中...", "100%", 1f);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            DebugUtil.Log("AssetBundle 打包成功");
        }

        /// <summary>
        /// 打Group的Bundle，并生成Group的version信息
        /// </summary>
        /// <param name="singleGroup"></param>
        public static void BuildGroup(BundleGroup singleGroup, string exportPath, BuildTarget buildTarget = BuildTarget.NoTarget)
        {
            List<string> paths = singleGroup.GetBundlePaths(GetBundleMethod.All);
            if (paths == null || paths.Count <= 0) return;

            string groupName = singleGroup.GroupName;
            if (!Directory.Exists(exportPath))
                Directory.CreateDirectory(exportPath);

            List<AssetBundleBuild> buildMap = new List<AssetBundleBuild>();

            //打AssetBundle包
            for (int i = 0; i < paths.Count; i++)
            {
                string fullPath = string.Format("{0}/{1}/{2}", Application.dataPath, "Export", paths[i]);
                AssetBundleBuild build = new AssetBundleBuild();
                List<string> assetnames = new List<string>();
                if (File.Exists(fullPath))
                {
                    string itemPath = paths[i];
                    int lastDot = itemPath.LastIndexOf(".");
                    if (lastDot > 0)
                    {
                        build.assetBundleName = itemPath.Substring(0, lastDot);
                    }
                    else
                    {
                        build.assetBundleName = itemPath;
                    }
                    assetnames.Add(FilePathTools.GetRelativePath(fullPath));
                }
                else
                {
                    build.assetBundleName = paths[i];
                    FileInfo[] files = FileUtil.GetFiles(fullPath);
                    if (files.Length <= 0)
                        continue;

                    for (int j = 0; j < files.Length; j++)
                    {
                        //Debug.Log(files[j]);
                        string path = FilePathTools.GetRelativePath(files[j].FullName);
                        if (path.Contains(".DS_Store") || path.Contains(".gitkeep"))
                        {
                            continue;
                        }
                        assetnames.Add(path);
                    }
                }

                //foreach (string assetName in assetnames)
                    //AssetImporter.GetAtPath(assetName).SetAssetBundleNameAndVariant(build.assetBundleName, string.Empty);

                build.assetNames = assetnames.ToArray();
                buildMap.Add(build);
            }
            AssetBundleManifest assetBundleManifest = BuildPipeline.BuildAssetBundles(
                exportPath,
                buildMap.ToArray(),
                BuildAssetBundleOptions.ChunkBasedCompression | BuildAssetBundleOptions.DeterministicAssetBundle,
                buildTarget == BuildTarget.NoTarget ? EditorUserBuildSettings.activeBuildTarget : buildTarget
                );





            //生成Version Item Info
            AssetBundleGroupInfo assetBundleGroupInfo = new AssetBundleGroupInfo(singleGroup.GroupName, singleGroup.BaseGroup);

            string[] builtAssetBundleNames = assetBundleManifest.GetAllAssetBundles();
            for (var i = 0; i < builtAssetBundleNames.Length; ++i)
            {
                string assetBundlePath = string.Format("{0}/{1}", exportPath, builtAssetBundleNames[i]);
                string path = Directory.GetCurrentDirectory();
                BundleInfo bundleInfo = singleGroup.GetBundleInfo(builtAssetBundleNames[i]);

                string md5 = AssetUtils.BuildFileMd5(assetBundlePath);
                int size = AssetUtils.FileSize(assetBundlePath);
                AssetBundleInfo localAssetBundleInfo = new AssetBundleInfo
                {
                    AssetBundleName = builtAssetBundleNames[i],
                    DependenciesBundleNames = assetBundleManifest.GetAllDependencies(builtAssetBundleNames[i]),
                    releaseMode = bundleInfo.releaseMode,
                    BaseBundle = bundleInfo.BaseBundle
                };
                localAssetBundleInfo.HashString = md5;// 初始包里的ab包，原样拷贝md5
                localAssetBundleInfo.Size = size;// bundle的尺寸
                assetBundleGroupInfo.Add(localAssetBundleInfo.AssetBundleName, localAssetBundleInfo);
            }
            assetBundleVersionInfo.Add(groupName, assetBundleGroupInfo);

        }
    }
}

