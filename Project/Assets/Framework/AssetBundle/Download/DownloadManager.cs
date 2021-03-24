/*-------------------------------------------------------------------------------------------
// Copyright (C) 2019
// 模块名：DownloadManager
// 创建日期：2019-1-11
// 模块描述：下载管理器
//-------------------------------------------------------------------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using BestHTTP;
using Framework.Utils;
using UnityEngine.Networking;

namespace Framework.Asset
{
    public class DownloadManager : D_MonoSingleton<DownloadManager>
    {
        // 最大并行下载个数
        private readonly int maxDownloads = 4;

        // 等待下载的任务队列
        protected Queue<DownloadInfo> pendingDownloads = new Queue<DownloadInfo>();

        // 下载任务列表
        public Dictionary<string, DownloadingTask> m_DownloadingTasks = new Dictionary<string, DownloadingTask>();

        // 当前同时进行的加载数量
        protected int downloadThreads = 0;

        // 添加一个下载任务
        public DownloadInfo DownloadInSeconds(string filename, string md5, Action<DownloadInfo> onComplete)
        {
            filename = FilePathTools.NormalizePath(filename);
            DownloadInfo info = new DownloadInfo(filename, md5, onComplete);

            pendingDownloads.Enqueue(info);
            return info;
        }

        // 重试下载任务 Warning:只有下载器需要调用这个借口，业务层不要调用
        public void RetryDownload(DownloadInfo info)
        {
            if (info.retry > 0)
            {
                info.retry -= 1;
                pendingDownloads.Enqueue(info);
                m_DownloadingTasks.Remove(info.fileName);
                --downloadThreads;
            }
            else
            {
                FinishDownloadTask(info);
            }
        }

        // 中止所有下载任务
        public void AbortAllDownloadTask()
        {
            pendingDownloads.Clear();

            List<DownloadingTask> list = new List<DownloadingTask>(this.m_DownloadingTasks.Values);
            for (int i = 0; i < list.Count; i++)
            {
                DownloadingTask task = list[i];
                if (task != null)
                {
                    HTTPRequest headRequest = task.HeadRequest;
                    if (headRequest != null)
                    {
                        //EventManager.Instance.Trigger<SDKEvents.DownloadFileEvent>().Data(task.DownloadInfo.fileName, "head_abort", 0, task.DownloadInfo.retry.ToString()).Trigger();
                        headRequest.Abort();
                    }
                    if (task.Downloader != null)
                    {
                        //EventManager.Instance.Trigger<SDKEvents.DownloadFileEvent>().Data(task.DownloadInfo.fileName, "abort", task.DownloadInfo.downloadedSize, task.DownloadInfo.retry.ToString()).Trigger();
                        task.Downloader.CancelDownload(false);
                    }
                    task.DownloadInfo.isFinished = true;
                    task.DownloadInfo.retry = 0;//保证不会再被添加到下载队列里了
                }
            }

            this.m_DownloadingTasks.Clear();
            pendingDownloads.Clear();
            downloadThreads = 0;
        }

        void Update()
        {
            if (pendingDownloads.Count > 0)
            {
                int freeThreads = maxDownloads - downloadThreads;
                for (int i = 0; i < freeThreads; ++i)
                {
                    if (pendingDownloads.Count > 0)
                    {
                        DownloadInfo info = pendingDownloads.Dequeue();
                        StartTask(info);
                    }
                }
            }
        }

        // 开始一个下载任务
        private void StartTask(DownloadInfo taskinfo)
        {
            if (this.m_DownloadingTasks.ContainsKey(taskinfo.fileName))
            {
                Debug.LogErrorFormat("task already in progress :{0}", taskinfo.fileName);
            }
            else
            {
                DownloadingTask value = new DownloadingTask(taskinfo);
                this.m_DownloadingTasks[taskinfo.fileName] = value;

                if (!File.Exists(taskinfo.tempPath))
                {
                    FilePathTools.CreateFolderByFilePath(taskinfo.tempPath);
                    File.Create(taskinfo.tempPath).Dispose();
                }

                using (var sw = new FileStream(taskinfo.tempPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    taskinfo.downloadedSize = (int)sw.Length;
                }

                HttpDownload(value);
            }
        }

        // 新建下载器开始下载
        private void HttpDownload(DownloadingTask task)
        {
            Debug.LogFormat("Start Download:{0}", task.DownloadInfo.fileName);

            DownloadingTask tmpTask = task;
            HTTPRequest httprequest = new HTTPRequest(new Uri(tmpTask.DownloadInfo.url), HTTPMethods.Head, (req, rep) =>
            {
                switch (req.State)
                {
                    case HTTPRequestStates.ConnectionTimedOut:
                    case HTTPRequestStates.TimedOut:
                        //EventManager.Instance.Trigger<SDKEvents.DownloadFileEvent>().Data(tmpTask.DownloadInfo.fileName, "head_timeout", 0, tmpTask.DownloadInfo.retry.ToString()).Trigger();
                        break;
                    case HTTPRequestStates.Aborted:
                        //EventManager.Instance.Trigger<SDKEvents.DownloadFileEvent>().Data(tmpTask.DownloadInfo.fileName, "head_abort", 0, tmpTask.DownloadInfo.retry.ToString()).Trigger();
                        break;
                    case HTTPRequestStates.Error:
                        //EventManager.Instance.Trigger<SDKEvents.DownloadFileEvent>().Data(tmpTask.DownloadInfo.fileName, "head_failure", 0, tmpTask.DownloadInfo.retry.ToString()).Trigger();
                        break;
                    case HTTPRequestStates.Finished:
                        if (rep != null && rep.StatusCode >= 200 && rep.StatusCode < 400)
                        {
                            //EventManager.Instance.Trigger<SDKEvents.DownloadFileEvent>().Data(tmpTask.DownloadInfo.fileName, "head_finish", 0, tmpTask.DownloadInfo.retry.ToString()).Trigger();
                        }
                        else
                        {
                            //EventManager.Instance.Trigger<SDKEvents.DownloadFileEvent>().Data(tmpTask.DownloadInfo.fileName, "head_failure", 0, tmpTask.DownloadInfo.retry.ToString()).Trigger();
                        }
                        break;
                }

                if (!this.m_DownloadingTasks.ContainsKey(tmpTask.DownloadInfo.fileName))
                {
                    Debug.LogErrorFormat("Cancelled Task :{0}", tmpTask.DownloadInfo.fileName);
                    return;
                }

                tmpTask.HeadRequest = null;
                if (rep == null)
                {
                    Debug.LogErrorFormat("Download failed due to network for :{0}", tmpTask.DownloadInfo.fileName);
                    tmpTask.DownloadInfo.result = DownloadResult.ServerUnreachable;
                    RetryDownload(tmpTask.DownloadInfo);
                }
                else if (rep.StatusCode == 200)
                {
                    try
                    {
                        string firstHeaderValue = rep.GetFirstHeaderValue("Content-Length");
                        tmpTask.DownloadInfo.downloadSize = int.Parse(firstHeaderValue);
                        Debug.LogFormat("Will download {0} bytes for  '{1}'", tmpTask.DownloadInfo.downloadSize, tmpTask.DownloadInfo.fileName);
                        BreakPointDownloader downloader = new BreakPointDownloader(tmpTask.DownloadInfo, this.m_DownloadingTasks);
                        tmpTask.Downloader = downloader;
                        downloader.StartDownload();
                    }
                    catch (Exception ex)
                    {
                        Debug.LogErrorFormat("An error occured during download '{0}' due to {1}", tmpTask.DownloadInfo.fileName, ex);
                        tmpTask.DownloadInfo.result = DownloadResult.Failed;
                        RetryDownload(tmpTask.DownloadInfo);
                    }
                }
                else
                {
                    Debug.LogErrorFormat("Response is not ok! for: {0}", tmpTask.DownloadInfo.url);
                    tmpTask.DownloadInfo.result = DownloadResult.Failed;
                    RetryDownload(tmpTask.DownloadInfo);
                }
            })
            {
                DisableCache = true
            };
            this.m_DownloadingTasks[tmpTask.DownloadInfo.fileName].HeadRequest = httprequest;
            //EventManager.Instance.Trigger<SDKEvents.DownloadFileEvent>().Data(tmpTask.DownloadInfo.fileName, "head_start", 0, tmpTask.DownloadInfo.retry.ToString()).Trigger();
            httprequest.Send();
        }

        // 结束一个下载任务，归还并发数
        public void FinishDownloadTask(DownloadInfo info)
        {
            info.isFinished = true;

            if (info.onComplete != null)
                info.onComplete(info);

            m_DownloadingTasks.Remove(info.fileName);
            --downloadThreads;
        }

        #region 拷贝AB包

        public static void DownloadBatchFiles(string sourceFolderName, string desFolderName, List<string> fileNames, Action onComlete)
        {
            int totalCount = fileNames.Count;
            int hasCopyCount = 0;

            //Debug.Log("desFolderName--------->"+ desFolderName);
            if (Directory.Exists(desFolderName))
            {
                DirectoryInfo dir = new DirectoryInfo(desFolderName);
                dir.Empty();
            }
            Log("sourceFolderName:{0}", sourceFolderName);
            Log("desFolderName:{0}", desFolderName);
            foreach (var item in fileNames)
            {
                Log("file Names:{0}", item);
            }

            for (int k = 0; k < totalCount; k++)
            {
                string fileName = fileNames[k];
                string filePath = string.Format("{0}/{1}", sourceFolderName, fileName);
                DownloadManager.Inst.StartCoroutine(DownLoadFile(fileName, filePath, desFolderName,null, () =>
                {
                    hasCopyCount += 1;
                    if (hasCopyCount >= totalCount)
                    {
                        if (onComlete != null)
                            onComlete();
                    }
                }));
            }
        }


        public static IEnumerator DownLoadFile(string fileName, string filePath, string desFolderName, ProgressInfo progressInfo, Action onComlete)
        {
            //Log("DownLoadFile: filePath :{0}", filePath);
            //Log("DownLoadFile: desFolderName :{0}", desFolderName);
            //Log("DownLoadFile: fileName :{0}", fileName);
            DebugUtil.Log("DownloadManager.DownLoadFile(filePath):"+ filePath);
            using (UnityWebRequest www = new UnityWebRequest(filePath))
            {
                DownloadHandlerBuffer downHandler = new DownloadHandlerBuffer();
                www.downloadHandler = downHandler;
                www.SendWebRequest();
                while (!www.isDone)
                {
                    if (progressInfo != null)
                        progressInfo.curProgress = www.downloadProgress;
                    yield return null;
                }
                if (string.IsNullOrEmpty(www.error))
                {
                    byte[] b = www.downloadHandler.data;
                    string destFile = string.Format("{0}/{1}", desFolderName, fileName);
                    string destDir = Path.GetDirectoryName(destFile);
                    Log("DownLoadFile: destFile :{0}", destFile);
                    if (!Directory.Exists(destDir))
                    {
                        Directory.CreateDirectory(destDir);
                    }

                    if (Directory.Exists(destDir))
                    {
                        Log("DownLoadFile : create fold {0} sucess", destDir);
                    }
                    else
                    {
                        Log("DownLoadFile : create fold {0} Fail", destDir);
                    }

                    if (File.Exists(destFile))
                    {
                        File.Delete(destFile);
                    }

                    try
                    {
                        using (FileStream fs = new FileStream(destFile, FileMode.Create))
                        {
                            fs.Seek(0, SeekOrigin.Begin);
                            fs.Write(b, 0, b.Length);
                            fs.Close();
                            Log("DownLoadFile :write sucess: {0},length:{1}", fileName,b.Length);
                        }
                    }catch(Exception e)
                    {
                        Log("DownLoadFile: exception:{0}", e.ToString());
                    }
                    if (File.Exists(destFile))
                    {
                        Log("DownLoadFile: complete copy:{0}", fileName);
                        if (progressInfo != null)
                            progressInfo.curIndexDone = true;
                    }
                    else
                    {
                        Log("DownLoadFile: Faile copy:{0}", fileName);
                        if (progressInfo != null)
                            progressInfo.curIndexDone = false;
                    }
                   
                    if (onComlete != null)
                        onComlete();
                }
                else
                {
                    DebugUtil.LogError("DownLoadFile: copy error:" + www.error);
                    if (progressInfo != null)
                        progressInfo.curIndexDone = false;
                    if (onComlete != null)
                        onComlete();
                }
            }
        }

        private static void Log(string str,params object[] args)
        {
            DebugUtil.Log("DownLoadManager: "+str,args);
        }

        #endregion
    }
}