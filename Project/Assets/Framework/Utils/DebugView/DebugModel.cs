/*
 * @file DebugModel
 * Debug
 * @author lu
 */

using EventUtil;
using Framework.Asset;
using Framework.Tables;
using Framework.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using Framework.Storage;

public class DebugCfg
{
    public string TitleStr { get; set; }
    public Action<string, string> ClickCallBack { get; set; }
}

public class DebugModel
{
    public static List<DebugCfg> GetCfg()
    {
        var finalCfg = new List<DebugCfg>();
        //// 跳关
        //finalCfg.Add(new DebugCfg
        //{
        //    TitleStr = "跳关2",
        //    ClickCallBack = (string inputText1, string inputText2) =>
        //    {
        //        if (!string.IsNullOrEmpty(inputText1))
        //        {
        //            int level = 0;
        //            int.TryParse(inputText1, out level);
        //            if (level > 0)
        //            {
        //                DataManager.Inst.userInfo.Level = level;
        //            }
        //        }
        //    }
        //});

        //finalCfg.Add(new DebugCfg
        //{
        //    TitleStr = "设置金币",
        //    ClickCallBack = (string inputText1, string inputText2) =>
        //    {
        //        if (string.IsNullOrEmpty(inputText1))
        //            return;
        //        int targetNum = int.Parse(inputText1);
        //        DataManager.Inst.userInfo.SetGoodsCount(Table.GoodsId.Coin,targetNum);
        //    }
        //});

        //finalCfg.Add(new DebugCfg
        //{
        //    TitleStr = "清理本地数据",
        //    ClickCallBack = (string inputText1, string inputText2) =>
        //    {
        //        PlayerPrefs.DeleteAll();
        //    }
        //});
        //finalCfg.Add(new DebugCfg
        //{
        //    TitleStr = "测试Dialog",
        //    ClickCallBack = (string inputText1, string inputText2) =>
        //    {
        //        DebugUtil.Log("inputText1:" + inputText1);
              
        //        int v = int.Parse(inputText1);

        //        DebugUtil.Log("v:" + v);
        //        UIManager.Inst.ShowUI(UIModuleEnum.UIDialog,v>0?v:0);

        //    }
        //});
        //finalCfg.Add(new DebugCfg
        //{
        //    TitleStr = "测试Login",
        //    ClickCallBack = (string inputText1, string inputText2) =>
        //    {
        //        LoginManager.Instance.Login();
        //    }
        //});
        //finalCfg.Add(new DebugCfg
        //{
        //    TitleStr = "测试Bind",
        //    ClickCallBack = (string inputText1, string inputText2) =>
        //    {
        //        LoginManager.Instance.BindFB();
        //    }
        //});
        

        //finalCfg.Add(new DebugCfg
        //{
        //    TitleStr = "加20颗星星",
        //    ClickCallBack = (string inputText1, string inputText2) => { DataManager.Inst.userInfo.Stars += 20; }
        //});
        //finalCfg.Add(new DebugCfg
        //{
        //    TitleStr = "跳到某任务",
        //    ClickCallBack = (string inputText1, string inputText2) =>
        //    {
        //        int taskId = 0;
        //        int.TryParse(inputText1, out taskId);
        //        TableTask taskConf = Table.Task.Get(taskId);
        //        if (taskConf == null)
        //        {
        //            Debug.LogError(taskId+"  不存在");
        //            return;
        //        }
        //        DataManager.Inst.taskInfo.CheatToSkipToTask(taskId);
        //    }
        //});
        //finalCfg.Add(new DebugCfg
        //{
        //    TitleStr = "卸载资源",
        //    ClickCallBack = (string inputText1, string inputText2) =>
        //    {
        //        Res.Inst.Release();
        //    }
        //});
        //finalCfg.Add(new DebugCfg
        //{
        //    TitleStr = "添加邮件id:1",
        //    ClickCallBack = (string inputText1, string inputText2) =>
        //    {
        //        ActivityManager.Instance.MailLogic.AddNewMail(1);
        //    }
        //});

      
        //finalCfg.Add(new DebugCfg
        //{
        //    TitleStr = "播放音乐",
        //    ClickCallBack = (string inputText1, string inputText2) =>
        //    {
        //        SoundPlay.PlayMusic(inputText1);
        //    }
        //});
        //finalCfg.Add(new DebugCfg
        //{
        //    TitleStr = "whatever",
        //    ClickCallBack = (string inputText1, string inputText2) =>
        //    {
        //        //ServerDataManager.Inst.activityInfo.GetAllActivity();
        //        UIManager.Inst.ShowUI(UIModuleEnum.UICommonTipWithTitle, "检查网络", "请检查网络并重试", "ok");
        //    }
        //});







        return finalCfg;
    }
}
