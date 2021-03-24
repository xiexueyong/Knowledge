/*-------------------------------------------------------------------------------------------
// Copyright (C) 2019
// 模块名：DownloadingTask
// 创建日期：2019-2-13
// 模块描述：正在下载中的任务
//-------------------------------------------------------------------------------------------*/
using BestHTTP;

namespace Framework.Asset
{
    public class DownloadingTask
    {
        public DownloadingTask(DownloadInfo info)
        {
            this.DownloadInfo = info;
        }

        // 任务数据
        public DownloadInfo DownloadInfo { private set; get; }

        // 下载器
        public BreakPointDownloader Downloader;

        // http head
        public HTTPRequest HeadRequest;
    }
}
