/*-------------------------------------------------------------------------------------------
// Copyright (C) 2019
// 模块名：BreakPointDownloader
// 创建日期：2019-1-11
// 修改日期：2019-2-13 使用besthttp插件实现下载，原框架有线程不安全的问题
// 模块描述：断点下载
//-------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.IO;
using BestHTTP;
using UnityEngine;

namespace Framework.Asset
{
    public class BreakPointDownloader
    {
        private readonly DownloadInfo m_DownloadInfo;

        private HTTPRequest m_HttpRequest;

        public BreakPointDownloader(DownloadInfo info, Dictionary<string, DownloadingTask> tasks)
        {
            this.m_DownloadInfo = info;
            Debug.Log("start breakpoint download:" + this.m_DownloadInfo.tempPath);
        }

        public void StartDownload()
        {
            this.m_HttpRequest = new HTTPRequest(new Uri(this.m_DownloadInfo.url), HTTPMethods.Get, (req, rep) =>
            {
                if (rep == null)
                {
                    Debug.LogErrorFormat("Response null. Server unreachable? Try again later. {0}", this.m_DownloadInfo.fileName);
                    this.m_DownloadInfo.result = DownloadResult.ServerUnreachable;
                    //EventManager.Instance.Trigger<SDKEvents.DownloadFileEvent>().Data(this.m_DownloadInfo.fileName, "failure", 0).Trigger();
                    CancelDownload(true);
                }
                else if (rep.StatusCode >= 200 && rep.StatusCode < 300)
                {
                    this.ReadHttpStream(req, rep);
                }
                else
                {
                    Debug.LogErrorFormat("Unexpected response from server: {0}, StatusCode = {1}", this.m_DownloadInfo.url, rep.StatusCode);
                    if (rep.StatusCode == 416)//416表示本地续传文件字节数 大于 服务器上对应文件的字节数，所以需要删除本地的续传文件，不然会进入死循环
                    {
                        if (File.Exists(this.m_DownloadInfo.tempPath))
                            File.Delete(this.m_DownloadInfo.tempPath);
                    }
                    this.m_DownloadInfo.result = DownloadResult.Failed;
                   // EventManager.Instance.Trigger<SDKEvents.DownloadFileEvent>().Data(this.m_DownloadInfo.fileName, "failure", this.m_DownloadInfo.downloadedSize).Trigger();
                    CancelDownload(true);
                }
            });

            this.m_HttpRequest.SetRangeHeader(this.m_DownloadInfo.downloadedSize, this.m_DownloadInfo.downloadSize);
            this.m_HttpRequest.IsKeepAlive = true;
            this.m_HttpRequest.UseStreaming = true;
            this.m_HttpRequest.StreamFragmentSize = 1 * 1024 * 10; // 10K  1M
            this.m_HttpRequest.DisableCache = true;
            this.m_HttpRequest.IsCookiesEnabled = false;
            this.m_HttpRequest.ConnectTimeout = TimeSpan.FromSeconds(5);
            this.m_HttpRequest.Timeout = TimeSpan.FromSeconds(20);
            this.m_HttpRequest.EnableTimoutForStreaming = true;
           // EventManager.Instance.Trigger<SDKEvents.DownloadFileEvent>().Data(this.m_DownloadInfo.fileName, "start", 0).Trigger();
            this.m_HttpRequest.Send();
        }

        public void CancelDownload(bool retry = true)
        {
            Debug.LogFormat("Cancelling download: {0}", this.m_DownloadInfo.fileName);

            this.m_HttpRequest.Abort();

            if (retry)//需要重试
            {
               // DownloadManager.Inst.RetryDownload(this.m_DownloadInfo);
            }
        }

        private void ReadHttpStream(HTTPRequest request, HTTPResponse response)
        {
            string localfile = this.m_DownloadInfo.tempPath;
            try
            {
                List<byte[]> streamedFragments = response.GetStreamedFragments();
                if (streamedFragments != null)
                {
                    int num = 0;
                    using (FileStream fileStream = new FileStream(localfile, FileMode.Append))
                    {
                        foreach (byte[] array in streamedFragments)
                        {
                            num += array.Length;
                            fileStream.Write(array, 0, array.Length);
                        }
                    }
                    this.m_DownloadInfo.downloadedSize += num;
                }
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("An error occured while downloading {0} due to {1}.Cancelling!", localfile, ex);
                this.m_DownloadInfo.result = DownloadResult.Failed;
                //EventManager.Instance.Trigger<SDKEvents.DownloadFileEvent>().Data(this.m_DownloadInfo.fileName, "failure", this.m_DownloadInfo.downloadedSize).Trigger();
                CancelDownload(true);
                this.CancelDownload(true);
                return;
            }

            float num2 = (float)this.m_DownloadInfo.downloadedSize / (float)this.m_DownloadInfo.downloadSize;
            this.m_DownloadInfo.currProgress = num2;

            Debug.LogFormat("Downloading {0} Status: Range {1}/{2} ({3:0.00})", this.m_DownloadInfo.fileName, this.m_DownloadInfo.downloadedSize, this.m_DownloadInfo.downloadSize, this.m_DownloadInfo.currProgress);
            if (!response.IsStreamingFinished || request.State != HTTPRequestStates.Finished)
            {
                return;
            }

            Debug.LogFormat("Download finished : {0}", this.m_DownloadInfo.savePath);
            this.m_DownloadInfo.currProgress = 1.0f;

            // 续传完成后，做一手MD5验证
            string localMd5 = AssetUtils.BuildFileMd5(localfile);
            if (localMd5.Trim() != m_DownloadInfo.fileMd5.Trim())
            {
                //md5验证失败，删除临时文件
                if (File.Exists(localfile))
                    File.Delete(localfile);

                Debug.LogError("md5 error, retry download:" + localfile + " =>" + localMd5 + " ## " + m_DownloadInfo.fileMd5);
                this.m_DownloadInfo.result = DownloadResult.Md5Error;
                this.m_DownloadInfo.currProgress = 0.0f;
                CancelDownload(true);
            }
            else
            {// md5验证通过，临时文件转为最终文件
                string localfile2 = m_DownloadInfo.savePath;
                if (File.Exists(localfile2))
                    File.Delete(localfile2);

                File.Move(localfile, localfile2);
                this.m_DownloadInfo.result = DownloadResult.Success;
                DownloadManager.Inst.FinishDownloadTask(this.m_DownloadInfo);
            }

            //EventManager.Instance.Trigger<SDKEvents.DownloadFileEvent>().Data(this.m_DownloadInfo.fileName, "finish", m_DownloadInfo.downloadedSize).Trigger();
        }
    }
}
