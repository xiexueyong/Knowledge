//#define ENABLE_TEST_SROPTIONS

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;
using System.Collections;
using System.IO;
using System.Text;
using SimpleJSON;
using Newtonsoft.Json;
using Framework.Storage;

public partial class SROptions
{
    [Category("Args"), DisplayName("参数2")] public string Args2 { get; set; }
    [Category("Args"), DisplayName("参数1")] public string Args1 { get; set; }



    [Category("UserInfo"), DisplayName("ClearAllPlayerPrefs")]
    public void ClearCache()
    {
        UnityEngine.PlayerPrefs.DeleteAll();
        Debug.Log("清除缓存成功");
        Application.Quit();
    }
    [Category("UserInfo"), DisplayName("关卡")]
    [Increment(1), NumberRange(0, 1000)]
    public int Level
    {
        get { return DataManager.Inst.userInfo.Level; }
        set
        {
            DataManager.Inst.userInfo.Level = value;
        }
    }

    [Category("UserInfo"), DisplayName("金币")]
    [Increment(5000), NumberRange(0, 99999999)]
    public int Coin
    {
        get { return DataManager.Inst.userInfo.Coins; }
        set { DataManager.Inst.userInfo.SetGoodsCount(Table.GoodsId.Coin,value); }
    }
    [Category("UserInfo"), DisplayName("体力")]
    [Increment(1), NumberRange(0, 10)]
    public int Energy
    {
        get { return DataManager.Inst.userInfo.Energy; }
        set { DataManager.Inst.userInfo.Energy = value; }
    }
    [Category("UserInfo"), DisplayName("星星")]
    [Increment(1), NumberRange(0, 20)]
    public int Star
    {
        get { return StorageManager.Inst.GetStorage<StorageUserInfo>().CurStar; }
        set { StorageManager.Inst.GetStorage<StorageUserInfo>().CurStar = value; }
    }
    [Category("UserInfo"), DisplayName("无限体力")]
    public void AddInfiniteLife()
    {
        MLifeManager.Inst.life.AddInfiniteLife(int.Parse(Args1));
    }

    [Category("开关"), DisplayName("连胜奖励")]
    public bool giveContinueWinBonus
    {
        get { return GameConfig.Inst.giveContinueWinBonus; }
        set
        {
            GameConfig.Inst.giveContinueWinBonus = value;
        }
    }
    }

    //    [Category("Login"), DisplayName("跳到某任务")]
    //    public int Task
    //    {
    //        get
    //        {
    //            int cTask = 0;
    //            List<int> list = DataManager.Inst.taskInfo.GetActiveTasks();
    //            for (int i = list.Count - 1; i >= 0; i--) {
    //                cTask = list[i];
    //                if (Table.Task.Get(cTask).Type == 1) {
    //                    //优先主线
    //                    break;
    //                }
    //            }

    //            return cTask;
    //        }
    //        set
    //        {
    //            if (!DataManager.Inst.taskInfo.TaskIsFinshed(value)) {
    //                DataManager.Inst.taskInfo.CheatToSkipToTask(value);
    //            }
    //        }
    //    }


    //    [Category("Map"), DisplayName("播放剧情")]
    //    public void PlayCutscene()
    //    {
    //        if (string.IsNullOrEmpty(Args1)) {
    //            return;
    //        }

    //        CutsceneManager.Play(Args1);
    //        SRDebug.Instance.HideDebugPanel();
    //    }

    //    [Category("Map"), DisplayName("速度")]
    //    [Increment(0.5f), NumberRange(0, 8)]
    //    public float TimeScale
    //    {
    //        set { Time.timeScale = value; }
    //        get { return Time.timeScale; }
    //    }

    //    [Category("Account"), DisplayName("新建账号")]
    //    public void newAccount()
    //    {
    //        //PlayerPrefs.DeleteKey(StorageManager.storageKey);
    //        PlayerPrefs.DeleteAll();
    //        if (string.IsNullOrEmpty(Args1)) {
    //            PlayerPrefs.SetString("TestDevice", "Dev" + (int) (DateTime.UtcNow.Ticks / 1000000));
    //        }
    //        else {
    //            PlayerPrefs.SetString("TestDevice", Args1.Trim());
    //        }

    //        Application.Quit();
    //    }

    //    [Category("Account"), DisplayName("设置广告账号")]
    //    public void setIronSourceApp()
    //    {
    //        PlayerPrefs.DeleteKey(SDKIniter.IronSourceAppKey);

    //        if (!string.IsNullOrEmpty(Args1.Trim())) {
    //            PlayerPrefs.SetString(SDKIniter.IronSourceAppKey, Args1.Trim());
    //        }

    //        Debug.Log("IronSourceAppKey:" + PlayerPrefs.GetString(SDKIniter.IronSourceAppKey));
    //    }

    //    [Category("Account"), DisplayName("上传IDFA")]
    //    public void uploadIDFA()
    //    {
    //#if UNITY_IOS
    //        DebugUtil.Log("IDFA:" + UnityEngine.iOS.Device.advertisingIdentifier);
    //        StorageManager.Inst.GetStorage<StorageAccountInfo>().OtherData = "IDFA:" + UnityEngine.iOS.Device.advertisingIdentifier;
    //#endif
    //    }

    //    [Category("Account"), DisplayName("打印Game信息")]
    //    public void PrintGameInfo()
    //    {
    //#if UNITY_IOS
    //        DebugUtil.Log("IDFA:" + UnityEngine.iOS.Device.advertisingIdentifier);
    //#endif
    //    DebugUtil.Log("DevId:" + DeviceHelper.GetDeviceId());
    //    DebugUtil.Log("Storage_DevId:" + StorageManager.Inst.GetStorage<StorageAccountInfo>().DevId);
    //    DebugUtil.Log("AssetVersionInfo:" + AssetBundleManager.Inst.GetVersion());
    //    DebugUtil.Log("AppVersionInfo:" + AssetBundleConfig.Inst.appVersion);
    //    DebugUtil.Log("Uid:" + StorageManager.Inst.GetStorage<StorageAccountInfo>().Uid);
    //}

    //[Category("Account"), DisplayName("选择Server")]
    //public GameConfig.ServerType serverType
    //{
    //    get { return GameConfig.Instance.serverType; }
    //    set { GameConfig.Instance.serverType = value; }
    //}

    //[Category("Account"), DisplayName("设置FBID")]
    //public void SetFBId()
    //{
    //    if (string.IsNullOrEmpty(Args1))
    //    {
    //        LoginManager.Instance.fbid = "fbid_123466";
    //    }
    //    else
    //    {
    //        LoginManager.Instance.fbid = Args1;
    //    }
    //}


    //[Category("Account"), DisplayName("设置uid 如果设置不上传数据")]
    //public string GetUId
    //{
    //    get { return PlayerPrefs.GetString("Setuid",string.Empty); }
    //    set
    //    {
    //        PlayerPrefs.SetString("Setuid", value);
    //        PlayerPrefs.Save();
    //    }
    //}


    //[Category("Login"), DisplayName("设置语言")]
    //public LanguageTool.LangType Language
    //{
    //    get { return LanguageTool.GetLangType(); }
    //    set { LanguageTool.SetLangType(value); }
    //}

    //[Category("Login"), DisplayName("跳过引导")]
    //public bool SkipGuide
    //{
    //    get { return PlayerPrefs.GetInt("tmp_skip_guide", 0) == 1; }
    //    set
    //    {
    //        if (value) {
    //            PlayerPrefs.SetInt("tmp_skip_guide", 1);
    //            PlayerPrefs.Save();
    //        }
    //        else {
    //            PlayerPrefs.SetInt("tmp_skip_guide", 0);
    //            PlayerPrefs.Save();
    //        }
    //    }
    //}

    //[Category("Login"), DisplayName("挂机")]
    //public bool HangUp
    //{
    //    get { return PlayerPrefs.GetInt("hang_up", 0) == 1; }
    //    set
    //    {
    //        if (value)
    //        {
    //            PlayerPrefs.SetInt("hang_up", 1);
    //            PlayerPrefs.Save();
    //            CutsceneAutoRun.Instance.startRunning();
    //        }
    //        else
    //        {
    //            PlayerPrefs.SetInt("hang_up", 0);
    //            PlayerPrefs.Save();
    //            CutsceneAutoRun.Instance.stopRunning();
    //        }
    //    }
    //}

    //[Category("Login"), DisplayName("弹出所有礼包")]
    //public bool PopAllGift
    //{
    //    get { return PlayerPrefs.GetInt("PopAllGift", 0) == 1; }
    //    set
    //    {
    //        if (value)
    //        {
    //            PlayerPrefs.SetInt("PopAllGift", 1);
    //            PlayerPrefs.Save();
    //        }
    //        else
    //        {
    //            PlayerPrefs.SetInt("PopAllGift", 0);
    //            PlayerPrefs.Save();
    //        }
    //    }
    //}

    //[Category("Login"), DisplayName("重置章节")]
    //public bool ResetChapter
    //{
    //    get { return PlayerPrefs.GetInt("tmp_reset_chapter", 0) == 1; }
    //    set
    //    {
    //        if (value)
    //        {
    //            PlayerPrefs.SetInt("tmp_reset_chapter", 1);
    //            PlayerPrefs.Save();
    //        }
    //        else
    //        {
    //            PlayerPrefs.SetInt("tmp_reset_chapter", 0);
    //            PlayerPrefs.Save();
    //        }
    //    }
    //}

    //[Category("Login"), DisplayName("重置喷泉")]
    //public bool ResetFountain
    //{
    //    get { return PlayerPrefs.GetInt("tmp_reset_fountain", 0) == 1; }
    //    set
    //    {
    //        if (value)
    //        {
    //            PlayerPrefs.SetInt("tmp_reset_fountain", 1);
    //            PlayerPrefs.Save();
    //        }
    //        else
    //        {
    //            PlayerPrefs.SetInt("tmp_reset_fountain", 0);
    //            PlayerPrefs.Save();
    //        }
    //    }
    //}


    //[Category("Login"), DisplayName("额外词测试")]
    //public bool TestExtraWord
    //{
    //    get { return PlayerPrefs.GetInt("tmp_extra", 0) == 1; }
    //    set
    //    {
    //        if (value)
    //        {
    //            PlayerPrefs.SetInt("tmp_extra", 1);
    //            PlayerPrefs.Save();
    //        }
    //        else
    //        {
    //            PlayerPrefs.SetInt("tmp_extra", 0);
    //            PlayerPrefs.Save();
    //        }
    //    }
    //}


    //[Category("Login"), DisplayName("自动跑关")]
    //public bool AutoRunLevel
    //{
    //    get { return LevelManager.Instance.autoRunLevel; }
    //    set { LevelManager.Instance.autoRunLevel = value; }
    //}


    //[Category("Login"), DisplayName("跳关")]
    //public bool SkipLevel
    //{
    //    get { return PlayerPrefs.GetInt("tmp_skip_level", 0) == 1; }
    //    set
    //    {
    //        if (value) {
    //            PlayerPrefs.SetInt("tmp_skip_level", 1);
    //            PlayerPrefs.Save();
    //        }
    //        else {
    //            PlayerPrefs.SetInt("tmp_skip_level", 0);
    //            PlayerPrefs.Save();
    //        }
    //    }
    //}


    //[Category("Login"), DisplayName("花瓣")]
    //[Increment(100), NumberRange(0, 999999)]
    //public int Flower
    //{
    //    get { return DataManager.Inst.userInfo.Flowers; }
    //    set { DataManager.Inst.userInfo.SetGoodsCount(Table.GoodsId.Flower, value); }
    //}


    //[Category("Login"), DisplayName("星星")]
    //[Increment(20), NumberRange(0, 9999)]
    //public int Star
    //{
    //    get { return DataManager.Inst.userInfo.Stars; }
    //    set { DataManager.Inst.userInfo.Stars = value; }
    //}




    //[Category("Login"), DisplayName("活动币")]
    //[Increment(1000), NumberRange(0, 99999999)]
    //public int HuoDongBi
    //{
    //    get { return DataManager.Inst.userInfo.ACoins; }
    //    set { DataManager.Inst.userInfo.ACoins = value; }
    //}



    //[Category("Login"), DisplayName("积累额外词")]
    //[Increment(1), NumberRange(0, 12376)]
    //public int ExtraWordCount
    //{
    //    get { return DataManager.Inst.userInfo.ExtraWordCount; }
    //    set
    //    {
    //        DataManager.Inst.userInfo.ExtraWordCount = value;
    //    }
    //}

    //[Category("Test"), DisplayName("关闭Cheat")]
    //public void CloseCheat()
    //{
    //    PlayerPrefs.DeleteKey("whoisyoudaddy");
    //    Application.Quit();
    //}


    //[Category("Test"), DisplayName("跳转Restart")]
    //public void JumpToLoginScene()
    //{
    //    SceneLoadManager.Inst.LoadScene(SceneName.Restart, null);
    //    Debug.Log("跳转到Login场景");
    //}

    //[Category("Test"), DisplayName("版本号")]
    //public string BuildNum
    //{
    //    get { return GameConfig.Instance.CurrentBuildNumber; }
    //}

    //[Category("Test"), DisplayName("RealDevID")]
    //public string RealDevId
    //{
    //    set
    //    {
    //        //nothing to do
    //    }
    //    get { return SystemInfo.deviceUniqueIdentifier; }
    //}

    //[Category("Test"), DisplayName("临时")]
    //public void Whatever()
    //{
    //    //Res.Inst.Release();
    //    if (AssetBundleManager.Inst.IsBundleGroupLoaded("Room1Group")) {
    //        DebugUtil.Log("=====Room1Group loaded");
    //    }
    //    else {
    //        ProgressInfo progressInfo = AssetBundleManager.Inst.LoadBundleGroup("Room1Group");
    //        AssetBundleManager.Inst.StartCoroutine(PrintInfo(progressInfo));
    //    }
    //}

    //[Category("Test"), DisplayName("收邮件")]
    //public void RequestMail()
    //{
    //    ServerDataManager.Inst.serverMailInfo.RequestMail(new List<int>());
    //}

    //[Category("Test"), DisplayName("删除邮件")]
    //public void DeleteEmail()
    //{
    //    List<int> emailIds = new List<int>();
    //    emailIds.Add(int.Parse(Args1));
    //    ServerDataManager.Inst.serverMailInfo.DeleteMail(emailIds);
    //}

    //[Category("Test"), DisplayName("FreeSpace")]
    //public void SimpleDisk()
    //{
    //    int space = SimpleDiskUtils.DiskUtils.CheckAvailableSpace();
    //    Debug.Log("===============spcae  :" + space);
    //}

    //[Category("Test"), DisplayName("制造崩溃")]
    //public void MakeCrush()
    //{
    //    GameObject go = null;
    //    go.name = "a";
    //}

    //[Category("Test"), DisplayName("制造Update崩溃")]
    //public void MakeUpdateCrush()
    //{
    //    DataManager.Inst.testList = null;
    //    DataManager.Inst.isClick = true;
    //}


    //private IEnumerator PrintInfo(ProgressInfo progressInfo)
    //{
    //    while (true) {
    //        yield return new WaitForSeconds(1f);
    //        DebugUtil.Log("=============progressInfo :  " + progressInfo.progress);
    //    }
    //}

    //[Category("Test"), DisplayName("验证单词")]
    //public void CheckAllWord()
    //{

    //    CoroutineManager.Instance.StartCoroutine(CheckExtraWord());
    //}
    //private IEnumerator CheckWord()
    //{
    //    int last = 0;
    //    int curLevel = 1;

    //    List<string> result = new List<string>();
    //    while (true)
    //    {
    //        yield return null;
    //    List<string> words = new List<string>();
    //    var maxLevel = Table.Word.Count();
    //    if (curLevel > last)
    //    {
    //        if (curLevel > maxLevel)
    //        {
    //            DebugUtil.Log("<color=#9400D3>" + "查询结束" + "</color>");
    //            foreach (var s in result)
    //            {
    //                DebugUtil.LogError(s);
    //            }
    //            yield  break;

    //        }
    //        words.Clear();
    //        last = curLevel;

    //        var layout = new LevelLayout(curLevel, Table.Word.Get(curLevel).Layout);
    //        foreach (AnswerLayout item in layout.layoutData)
    //        {
    //            words.Add(item.answer.ToLower());
    //        }
    //       // words.Add("22");

    //        WordDicBody body = new WordDicBody(string.Join(",", words.ToArray()), "QWordSearch");
    //        var header = Header.GetHeader();
    //        header.headers["session"] = string.Empty;



    //        HttpRequestTool.SendMessage(
    //            body,
    //            (x) =>
    //            {
    //                    //  DebugUtil.Log("<color=#9400D3>" + x + "</color>");
    //                    var node = JSONNode.Parse(x);


    //                    List<string> list = new List<string>();
    //                    foreach (var child in node["data"].AsArray.Children)
    //                    {
    //                        list.Add(child["word"].ToString().Replace('"', ' ').Trim().ToLower());
    //                    }

    //                    List<string> except = words.Except(list).ToList();
    //                    if(except.Count!=0)
    //                        result.Add(curLevel + "单词" + string.Join(",", except.ToArray()) + "无法查询");
    //                    curLevel++;
    //            },
    //            (y) =>
    //            {

    //                DebugUtil.LogError(curLevel + "查询失败");
    //                curLevel++;
    //            }, header, false
    //        );

    //    }
    //    }
    //}


    //private IEnumerator CheckExtraWord()
    //{
    //    int last = -1;
    //    int curLevel = 0;
    //    int step = 20;

    //    List<string> result = new List<string>();
    //    var bonusAsset = Res.LoadResource<TextAsset>("Table/Bonus");
    //    List<string> answers = JsonConvert.DeserializeObject<List<string>>(bonusAsset.text);
    //    int maxLevel = answers.Count;
    //    while (true)
    //    {
    //        yield return null;
    //        List<string> words = new List<string>();
    //        if (curLevel > last)
    //        {
    //            if (curLevel > maxLevel)
    //            {
    //                DebugUtil.Log("<color=#9400D3>" + "查询结束" + "</color>");
    //                foreach (var s in result)
    //                {
    //                    DebugUtil.LogError(s);
    //                }
    //                yield break;

    //            }
    //            words.Clear();
    //            last = curLevel;

    //            for (int i = curLevel; i < Mathf.Min(curLevel+step,maxLevel); i++)
    //            {
    //                words.Add(answers[i]);
    //            }
    //            // words.Add("22");

    //            WordDicBody body = new WordDicBody(string.Join(",", words.ToArray()), "QWordSearch");
    //            var header = Header.GetHeader();
    //            header.headers["session"] = string.Empty;



    //            HttpRequestTool.SendMessage(
    //                body,
    //                (x) =>
    //                {
    //                //  DebugUtil.Log("<color=#9400D3>" + x + "</color>");
    //                var node = JSONNode.Parse(x);


    //                    List<string> list = new List<string>();
    //                    foreach (var child in node["data"].AsArray.Children)
    //                    {
    //                        list.Add(child["word"].ToString().Replace('"', ' ').Trim().ToLower());
    //                    }

    //                    List<string> except = words.Except(list).ToList();
    //                    if (except.Count != 0)
    //                        result.Add(curLevel + "单词" + string.Join(",", except.ToArray()) + "无法查询");
    //                    curLevel+=step;
    //                },
    //                (y) =>
    //                {

    //                    DebugUtil.LogError(curLevel + "查询失败");
    //                    curLevel+=step;
    //                }, header, false
    //            );

    //        }
    //    }
    //}

    //[Category("存钱罐"), DisplayName("设置记录ID")]
    //[Increment(1), NumberRange(1, 10)]
    //public int PiggyBankId
    //{
    //    get
    //    {
    //        if (StorageManager.Inst.GetStorage<StorageUserInfo>().PiggyBankReward.Count > 0) {
    //            return -1;
    //        }
    //        else {
    //            return StorageManager.Inst.GetStorage<StorageUserInfo>().PiggyBankReward.LastOrDefault().Key;
    //        }
    //    }
    //    set
    //    {
    //        StorageManager.Inst.GetStorage<StorageUserInfo>().PiggyBankReward.Clear();
    //        StorageManager.Inst.GetStorage<StorageUserInfo>().PiggyBankReward.Add(value, CommonUtil.GetTimeStamp() + 60);
    //    }
    //}

    //[Category("存钱罐"), DisplayName("终止时间")]
    //[Increment(60), NumberRange(0, 60)]
    //public int PiggyBankEndTime
    //{
    //    get
    //    {
    //        if (StorageManager.Inst.GetStorage<StorageUserInfo>().PiggyBankReward.Count > 0) {
    //            return -1;
    //        }
    //        else {
    //            return StorageManager.Inst.GetStorage<StorageUserInfo>().PiggyBankReward.LastOrDefault().Value;
    //        }
    //    }
    //    set
    //    {
    //        if (StorageManager.Inst.GetStorage<StorageUserInfo>().PiggyBankReward.Count > 0) {
    //            int id = StorageManager.Inst.GetStorage<StorageUserInfo>().PiggyBankReward.LastOrDefault().Key;
    //            StorageManager.Inst.GetStorage<StorageUserInfo>().PiggyBankReward[id] = value;
    //        }
    //        else {
    //            StorageManager.Inst.GetStorage<StorageUserInfo>().PiggyBankReward[1] = value;
    //        }
    //    }
    //}

    //[Category("存钱罐"), DisplayName("当前收集金币")]
    //[Increment(100), NumberRange(0, 10000)]
    //public int PiggyBankCoin
    //{
    //    get { return StorageManager.Inst.GetStorage<StorageUserInfo>().PiggyBankCoin; }
    //    set { StorageManager.Inst.GetStorage<StorageUserInfo>().PiggyBankCoin = value; }
    //}

    //[Category("存钱罐"), DisplayName("清空记录")]
    //public void CleardHistory()
    //{
    //    StorageManager.Inst.GetStorage<StorageUserInfo>().PiggyBankReward.Clear();
    //}

    ////StringBuilder sb = new StringBuilder();
    ////[Category("购买信息"), DisplayName("购买时间    商品ID")]
    ////public string PUrchaseHistory
    ////{
    ////    get
    ////    {
    ////        sb.Remove(0, sb.Length);
    ////        foreach (KeyValuePair<int, int> pair in DataManager.Inst.userInfo.PayHistory)
    ////        {
    ////            sb.Append(SystemClock.Instance.SecondToDateTime(pair.Key) + ":     " + pair.Value+"\n");
    ////        }
    ////        return sb.ToString();
    ////    }
    ////}

    //StringBuilder sb = new StringBuilder();
    ////[Category("引导记录"), DisplayName("完成的引导")]
    ////public string FinishGuideHistory
    ////{
    ////    get
    ////    {
    ////        sb.Remove(0, sb.Length);
    ////        foreach (var pair in DataManager.Inst.userInfo.AlreadyGuide)
    ////        {
    ////            sb.Append( pair.ToString()+"\n");
    ////        }
    ////        return sb.ToString();
    ////    }
    ////}

    ////[Category("最近礼包触发时间"), DisplayName("组别     最近触发时间")]
    ////public string LimitriggerLately
    ////{
    ////    get
    ////    {
    ////        sb.Remove(0, sb.Length);
    ////        foreach (KeyValuePair<int, int> pair in ActivityManager.Instance.LimitGiftLogic.latelyTriggerTime)
    ////        {
    ////            sb.Append(pair.Key + ":     " + SystemClock.Instance.SecondToDateTime(pair.Value).ToString()+"\n");
    ////        }
    ////        return sb.ToString();
    ////    }
    ////}

    ////[Category("弹出礼包历史"), DisplayName("限时礼包Id 最近触发时间")]
    ////public string LimitGiftHistory
    ////{
    ////    get
    ////    {
    ////        sb.Remove(0, sb.Length);
    ////        foreach (KeyValuePair<int, Int3> pair in StorageManager.Inst.GetStorage<StorageUserInfo>().LevelGifts)
    ////        {
    ////            sb.Append(pair.Key + ":     " + SystemClock.Instance.SecondToDateTime(pair.Value.field1).ToString() + "\n");
    ////        }
    ////        return sb.ToString();
    ////    }
    ////}


    //[Category("静默下载"), DisplayName("清空需要下载的文件")]
    //public void DeleteDownloadFile()
    //{
    //    string persistentDataPath_Platform = FilePathTools.persistentDataPath_Platform;
    //    Debug.Log(persistentDataPath_Platform);
    //    for (int i = 1; i < 5; i++) {
    //        string path = persistentDataPath_Platform + "/room" + i;
    //        if (File.Exists(path)) {
    //            File.Delete(path);
    //            DebugUtil.Log("delete {0}", path);
    //        }
    //    }
    //}

    //[Category("广告相关"), DisplayName("花费金钱")]
    //[Increment(100), NumberRange(0, 1000000)]
    //public int PayCount
    //{
    //    get { return StorageManager.Inst.GetStorage<StorageUserInfo>().PayCount; }
    //    set { StorageManager.Inst.GetStorage<StorageUserInfo>().PayCount = value; }
    //}

    //[Category("广告相关"), DisplayName("上次购买到现在的间隔时间")]
    //[Increment(86400), NumberRange(0, 2147483647)]
    //public int PayLastTime
    //{
    //    get { return SystemClock.Now - StorageManager.Inst.GetStorage<StorageUserInfo>().LastPayTime; }
    //    set { StorageManager.Inst.GetStorage<StorageUserInfo>().LastPayTime = SystemClock.Now - value; }
    //}

    //[Category("广告相关"), DisplayName("去广告")]
    //public bool RemoveAd
    //{
    //    get { return CommonUtil.IsRemovedInterstitialAd(); }
    //    set
    //    {
    //        if (value) {
    //            DataManager.Inst.userInfo.NoAdExp = SystemClock.Now + 1000;
    //        }
    //        else {
    //            DataManager.Inst.userInfo.NoAdExp = SystemClock.Now - 1000;
    //        }
    //    }
    //}

    //[Category("广告相关"), DisplayName("去广告剩余时间")]
    //public int AdExp
    //{
    //    get
    //    {
    //        int expTime = DataManager.Inst.userInfo.NoAdExp - SystemClock.Now;
    //        return Mathf.Clamp(expTime, 0, int.MaxValue);
    //    }
    //    set { DataManager.Inst.userInfo.NoAdExp = SystemClock.Now + value; }
    //}


    //[Category("静默下载"), DisplayName("打印房间文件信息")]
    //public void PrintRoomInfo()
    //{
    //    string rooms = "room1,room2,room3,garden,bar,catroom";
    //    string[] roomNams = rooms.Split(',');
    //    string persistentDataPath_Platform = FilePathTools.persistentDataPath_Platform;
    //    Debug.Log(persistentDataPath_Platform);
    //    for (int i = 0; i < roomNams.Length; i++) {
    //        DebugUtil.Log("- room({0}) exist: {1}", roomNams[i], File.Exists(persistentDataPath_Platform + "/"+ roomNams[i]));
    //    }
    //}
    //[Category("静默下载"), DisplayName("删除Persistent文件")]
    //public void DelePersistent()
    //{
    //    string persistentDataPath_Platform = FilePathTools.persistentDataPath_Platform;
    //    Debug.Log(persistentDataPath_Platform);
    //    if (Directory.Exists(persistentDataPath_Platform))
    //    {
    //        Debug.Log("dele persistent:"+persistentDataPath_Platform);
    //        DirectoryInfo directory = new DirectoryInfo(persistentDataPath_Platform);
    //        foreach (System.IO.FileInfo file in directory.GetFiles()) file.Delete();
    //        foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
    //    }
    //}

    //[Category("用户分层相关"), DisplayName("打印用户分层信息")]
    //public void UserLayerDetail()
    //{
    //    Debug.Log(string.Format("用户所在付费分层:{0}",PayUserLayer.Instance.GetLayerName()));
    //    Debug.Log(string.Format("付费总额:{0}",StorageManager.Inst.GetStorage<StorageUserInfo>().PayCount));
    //    int intervalTime = SystemClock.Now - StorageManager.Inst.GetStorage<StorageUserInfo>().LastPayTime;
    //    Debug.Log(string.Format("上次付费距离现在时间:{0}",CommonUtil.SecondToTimeFormat(intervalTime)));
    //}

    //[Category("用户分层相关"), DisplayName("用户来源")]
    //public string AdChannel
    //{
    //    get { return StorageManager.Inst.GetStorage<StorageAccountInfo>().adChannel; }
    //    set { StorageManager.Inst.GetStorage<StorageAccountInfo>().adChannel = value; }
    //}
