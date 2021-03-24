/*-------------------------------------------------------------------------------------------
// Copyright (C) 2019
// 模块名：DownloadInfo
// 创建日期：2019-1-11
// 模块描述：单个下载任务的描述
//-------------------------------------------------------------------------------------------*/
using System;

namespace Framework.Asset
{
    public enum DownloadResult
    {
        Unknown = -1,
        Success,
        Failed,
        TimeOut,
        ServerUnreachable,
        Md5Error
    }

    public class DownloadInfo
    {
        // 文件名
        public string fileName = "";

        // 文件md5
        public string fileMd5 = "";

        // 下载地址
        public string url = "";

        // 下载成功后的保存路径
        public string savePath = "";

        // 断点文件的保存路径
        public string tempPath = "";

        // 超时时间
        public float timeout = 20.0f;

        // 需下载大小(字节数)
        public int downloadSize;

        // 已下载大小(字节数)
        public int downloadedSize;

        // 处理下载结果的回调
        public Action<DownloadInfo> onComplete = null;

        // 重试次数
        public int retry = 3;

        // 当前下载的进度
        public float currProgress = 0f;

        // 是否下载结束
        public bool isFinished = false;

        // 下载结果
        public DownloadResult result = DownloadResult.Unknown;

        public DownloadInfo(string _filename, string _md5, Action<DownloadInfo> _callback)
        {
            fileName = _filename;
            fileMd5 = _md5;
            onComplete = _callback;
            //url = string.Format("{0}/{1}/{2}", FilePathTools.AssetBundleDownloadPath, VersionManager.Instance.GetResRootVersion(), fileName);
            savePath = string.Format("{0}/{1}", FilePathTools.persistentDataPath_Platform, fileName);
            tempPath = string.Format("{0}/{1}.temp", FilePathTools.persistentDataPath_Platform, fileName);
        }
    }
}
