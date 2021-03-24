/*-------------------------------------------------------------------------------------------
// 模块名：FilePathTools
// 模块描述：文件、路径操作封装
//-------------------------------------------------------------------------------------------*/

using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System;

namespace Framework.Asset
{
    public static class FilePathTools
    {
        // persistent目录
        public static readonly string persistentDataPath_Platform = Application.persistentDataPath + "/DownLoad/" + GameConfig.TargetPlatformName;
        public static readonly string persistentDataPath_Platform_ForWWWLoad = "file://" + persistentDataPath_Platform;

        //streamingAssets目录缓存
        public static readonly string streamingAssetsPath_Platform = Application.streamingAssetsPath + "/" + GameConfig.TargetPlatformName;
#if !UNITY_EDITOR && UNITY_ANDROID
        public static readonly string streamingAssetsPath_Platform_ForWWWLoad = streamingAssetsPath_Platform;
#else
        public static readonly string streamingAssetsPath_Platform_ForWWWLoad = "file:///" + streamingAssetsPath_Platform;
#endif
        //assetbundle打完包后的存放路径
        public static readonly string assetBundleOutPath = Application.dataPath + "/AssetBundleOut/" + GameConfig.TargetPlatformName;
        public static readonly string assetBundleOutReleasePath = Application.dataPath + "/AssetBundleOutRelease/" + GameConfig.TargetPlatformName;
        public static readonly string assetBundleOutPath_ForWWWLoad = "file:///" + Application.dataPath + "/AssetBundleOut/" + GameConfig.TargetPlatformName;


        // 返回bundle包的绝对路径
        public static string GetBundleLoadPath(string relativePath)
        {
            return persistentDataPath_Platform + "/" + relativePath;
        }


        // 创建文件目录前的文件夹，保证创建文件的时候不会出现文件夹不存在的情况
        public static void CreateFolderByFilePath(string path)
        {
            FileInfo fi = new FileInfo(path);
            DirectoryInfo dir = fi.Directory;
            if (!dir.Exists)
            {
                dir.Create();
            }
        }

        // 创建指定文件夹
        public static void CreateFolder(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            if (!dir.Exists)
            {
                dir.Create();
            }
        }

        // 规范化路径名称 修正路径中的正反斜杠
        public static string NormalizePath(string path)
        {
            return path.Replace(@"\", "/");
        }

        // 将短路径拼接成编辑器下的全路径
        public static string GetAssetEditorPath(string path)
        {
            return "Assets/Export/" + path;
        }

        // 获取相对路径,只用于bundle打包
        public static string GetRelativePath(string fullPath)
        {
            string path = NormalizePath(fullPath);
            path = ReplaceFirst(path, Application.dataPath, "Assets");
            return path;
        }

        // 替换掉第一个遇到的指定字符串
        public static string ReplaceFirst(string str, string oldValue, string newValue)
        {
            int i = str.IndexOf(oldValue, System.StringComparison.Ordinal);
            str = str.Remove(i, oldValue.Length);
            str = str.Insert(i, newValue);
            return str;
        }


        // 获取文件夹下的所有子文件夹， 不包含.meta文件
        public static List<string> GetSubFoldPaths(string path)
        {
            List<string> filesList = new List<string>();

            if (Directory.Exists(path))
            {
                DirectoryInfo folder = new DirectoryInfo(path);
                DirectoryInfo[] subFolders = folder.GetDirectories();
                foreach (DirectoryInfo subFolder in subFolders)
                {
                    FileInfo[] fileInfos = FileUtil.GetFiles(subFolder.FullName);
                    if (fileInfos.Length > 0)
                        filesList.Add(subFolder.FullName);
                }
            }
            return filesList;
        }

        // 获取文件夹下的所有文件(不获取子文件夹内的文件)， 不包含.meta文件
        public static List<string> GetFilePaths(string path)
        {
            List<string> filesList = new List<string>();

            if (Directory.Exists(path))
            {
                DirectoryInfo folder = new DirectoryInfo(path);
                FileInfo[] files = folder.GetFiles();
                foreach (FileInfo file in files)
                {
                    if(file.Extension != ".meta" && file.Name != ".DS_Store")
                        filesList.Add(file.FullName);
                }
            }
            return filesList;
        }

        /// <summary>
        /// 获取资源的全路径，在Export基础上
        /// </summary>
        /// <returns></returns>
        public static string GetFullpathRelativeExport(string relativePath)
        {
            return string.Format("{0}/{1}/{2}", Application.dataPath, "Export", relativePath);
        }

        /// <summary>
        /// 获取资源的相对路径，在Export基础上
        /// </summary>
        /// <returns></returns>
        public static string GetShortPathRelativeExport(string fullpath)
        {
            fullpath = FilePathTools.NormalizePath(fullpath);
            int i = fullpath.IndexOf("Export/");
            string sp = fullpath.Substring(i + 7);
            return sp;
        }
    }
}

