using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public static class TableBI
{
    /// <summary>
    /// 激励广告
    /// </summary>
    /// <param name="step">1每日转盘,2关卡失败</param>
    /// <param name="hasAd">有无拉取到的广告:0没有,1有</param>
    /// <param name="maxTimes">是否达到每日上限:0没有到达,1到达</param>
    /// <param name="leftCoin">金币</param>
    /// <param name="adcount">当天观看的第几个奖励视频广告，次日清零</param>
    /// <param name="levelAdCount">关卡内观看广告,当天第几次</param>
    public static void Ad_Reward(int step,int hasAd,int maxTimes,long leftCoin,int adcount,int levelAdCount)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        try
        {
			dic.Add("step", step.ToString());
			dic.Add("hasAd", hasAd.ToString());
			dic.Add("maxTimes", maxTimes.ToString());
			dic.Add("leftCoin", leftCoin.ToString());
			dic.Add("adcount", adcount.ToString());
			dic.Add("levelAdCount", levelAdCount.ToString());

        }
        catch(Exception e)
        {
            DebugUtil.LogError("Ad_Reward:"+e.ToString());
        }
        AnalyticsTool.Analytics("Ad_Reward", dic);
    }
    /// <summary>
    /// 打开商城界面记录
    /// </summary>
    /// <param name="state">1:点开;2:返回</param>
    /// <param name="Source">来源 1:主动充值,2:金币免看广告获得家具,3:金币获得家具,4:金币加速任务,5:加速卡道具,6:道具多个提示,7:道具单个随机提示,8:道具单个指定提示,9:家具套装购买关卡</param>
    /// <param name="ShopType">商店类型,1:充值商店; 2:存钱罐</param>
    public static void Open_Close_Shop(int state,int Source,int ShopType)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        try
        {
			dic.Add("state", state.ToString());
			dic.Add("Source", Source.ToString());
			dic.Add("ShopType", ShopType.ToString());

        }
        catch(Exception e)
        {
            DebugUtil.LogError("Open_Shop:"+e.ToString());
        }
        AnalyticsTool.Analytics("Open_Shop", dic);
    }
    /// <summary>
    /// 金币充值商品按钮点击
    /// </summary>
    /// <param name="productId">商品ID</param>
    public static void Custume_Click(string productId)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        try
        {
			dic.Add("productId", productId.ToString());

        }
        catch(Exception e)
        {
            DebugUtil.LogError("Custume_Click:"+e.ToString());
        }
        AnalyticsTool.Analytics("Custume_Click", dic);
    }
    /// <summary>
    /// 充值成功
    /// </summary>
    /// <param name="productId">商品ID</param>
    /// <param name="storeId">商店skuId</param>
    /// <param name="price">价格(美元)</param>
    /// <param name="userLevel">购买时的等级</param>
    /// <param name="coinsBeforePay">购买前的金币数量</param>
    /// <param name="payTotalBeforePay">购买前的历史付费金额(美元)</param>
    /// <param name="pf">支付时的平台</param>
    public static void PaySuccess(string productId,string storeId,int price,int userLevel,int coinsBeforePay,int payTotalBeforePay,string pf)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        try
        {
			dic.Add("productId", productId.ToString());
			dic.Add("storeId", storeId.ToString());
			dic.Add("price", price.ToString());
			dic.Add("userLevel", userLevel.ToString());
			dic.Add("coinsBeforePay", coinsBeforePay.ToString());
			dic.Add("payTotalBeforePay", payTotalBeforePay.ToString());
			dic.Add("pf", pf.ToString());

        }
        catch(Exception e)
        {
            DebugUtil.LogError("PaySuccess:"+e.ToString());
        }
        AnalyticsTool.Analytics("PaySuccess", dic);
    }
    /// <summary>
    /// 点击关卡按钮
    /// </summary>
    /// <param name="LevelId">当前关卡</param>
    public static void Click_Level(int LevelId)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        try
        {
			dic.Add("LevelId", LevelId.ToString());

        }
        catch(Exception e)
        {
            DebugUtil.LogError("Click_Level:"+e.ToString());
        }
        AnalyticsTool.Analytics("Click_Level", dic);
    }
    /// <summary>
    /// 进入关卡记录
    /// </summary>
    /// <param name="LevelId">当前关卡</param>
    public static void Enter_Level(int LevelId)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        try
        {
			dic.Add("LevelId", LevelId.ToString());

        }
        catch(Exception e)
        {
            DebugUtil.LogError("Enter_Level:"+e.ToString());
        }
        AnalyticsTool.Analytics("Enter_Level", dic);
    }
    /// <summary>
    /// 关卡返回
    /// </summary>
    /// <param name="LevelId">当前关卡</param>
    public static void Return_Level(int LevelId)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        try
        {
			dic.Add("LevelId", LevelId.ToString());

        }
        catch(Exception e)
        {
            DebugUtil.LogError("Return_Level:"+e.ToString());
        }
        AnalyticsTool.Analytics("Return_Level", dic);
    }
    /// <summary>
    /// 通关关卡记录
    /// </summary>
    /// <param name="LevelId">当前关卡</param>
    /// <param name="RemainMoves">剩余步数</param>
    /// <param name="CoinNum">金币数量</param>
    /// <param name="PreUsedGoods">关卡前置道具使用</param>
    /// <param name="InUsedGoods">关卡内道具使用</param>
    /// <param name="buyLifeCount">金币买步数次数</param>
    /// <param name="watchAdCount">看广告次数</param>
    /// <param name="wheelGotLifeCount">转盘获得步数的次数</param>
    /// <param name="continuedWinRewardLevel">进入关卡时的连胜奖励(皇冠)等级</param>
    public static void Complete_Level(int LevelId,int RemainMoves,int CoinNum,Dictionary<int, int> PreUsedGoods,Dictionary<int, int> InUsedGoods,int buyLifeCount,int watchAdCount,int wheelGotLifeCount,int continuedWinRewardLevel)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        try
        {
			dic.Add("LevelId", LevelId.ToString());
			dic.Add("RemainMoves", RemainMoves.ToString());
			dic.Add("CoinNum", CoinNum.ToString());
			AddDicAsJsonToDic(dic, "PreUsedGoods", PreUsedGoods);
			AddDicAsJsonToDic(dic, "InUsedGoods", InUsedGoods);
			dic.Add("buyLifeCount", buyLifeCount.ToString());
			dic.Add("watchAdCount", watchAdCount.ToString());
			dic.Add("wheelGotLifeCount", wheelGotLifeCount.ToString());
			dic.Add("continuedWinRewardLevel", continuedWinRewardLevel.ToString());

        }
        catch(Exception e)
        {
            DebugUtil.LogError("Complete_Level:"+e.ToString());
        }
        AnalyticsTool.Analytics("Complete_Level", dic);
    }
    /// <summary>
    /// 关卡失败记录
    /// </summary>
    /// <param name="LevelId">当前关卡</param>
    /// <param name="CoinNum">金币数量</param>
    /// <param name="PreUsedGoods">关卡前置道具使用</param>
    /// <param name="InUsedGoods">关卡内道具使用</param>
    /// <param name="buyLifeCount">金币买步数次数</param>
    /// <param name="watchAdCount">看广告次数</param>
    /// <param name="wheelGotLifeCount">转盘获得步数的次数</param>
    /// <param name="continuedWinRewardLevel">进入关卡时的连胜奖励(皇冠)等级</param>
    public static void Fail_Level(int LevelId,int CoinNum,Dictionary<int, int> PreUsedGoods,Dictionary<int, int> InUsedGoods,int buyLifeCount,int watchAdCount,int wheelGotLifeCount,int continuedWinRewardLevel)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        try
        {
			dic.Add("LevelId", LevelId.ToString());
			dic.Add("CoinNum", CoinNum.ToString());
			AddDicAsJsonToDic(dic, "PreUsedGoods", PreUsedGoods);
			AddDicAsJsonToDic(dic, "InUsedGoods", InUsedGoods);
			dic.Add("buyLifeCount", buyLifeCount.ToString());
			dic.Add("watchAdCount", watchAdCount.ToString());
			dic.Add("wheelGotLifeCount", wheelGotLifeCount.ToString());
			dic.Add("continuedWinRewardLevel", continuedWinRewardLevel.ToString());

        }
        catch(Exception e)
        {
            DebugUtil.LogError("Fail_Level:"+e.ToString());
        }
        AnalyticsTool.Analytics("Fail_Level", dic);
    }
    /// <summary>
    /// 关卡中没有可点击色块了（卡死了）
    /// </summary>
    /// <param name="LevelId">当前关卡</param>
    /// <param name="RemainMoves">剩余步数</param>
    public static void NoWayOut_Level(int LevelId,int RemainMoves)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        try
        {
			dic.Add("LevelId", LevelId.ToString());
			dic.Add("RemainMoves", RemainMoves.ToString());

        }
        catch(Exception e)
        {
            DebugUtil.LogError("NoWayOut_Level:"+e.ToString());
        }
        AnalyticsTool.Analytics("NoWayOut_Level", dic);
    }
    /// <summary>
    /// 登陆记录
    /// </summary>
    /// <param name="Time">每日最早登陆时间</param>
    public static void Login(int Time)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        try
        {
			dic.Add("Time", Time.ToString());

        }
        catch(Exception e)
        {
            DebugUtil.LogError("Login:"+e.ToString());
        }
        AnalyticsTool.Analytics("Login", dic);
    }
    /// <summary>
    /// 每日签到领取记录
    /// </summary>
    /// <param name="Date">日期</param>
    /// <param name="DayNum">签到天数</param>
    /// <param name="Award">道具ID及数量</param>
    public static void Daily_Sign(int Date,int DayNum,string Award)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        try
        {
			dic.Add("Date", Date.ToString());
			dic.Add("DayNum", DayNum.ToString());
			dic.Add("Award", Award.ToString());

        }
        catch(Exception e)
        {
            DebugUtil.LogError("Daily_Sign:"+e.ToString());
        }
        AnalyticsTool.Analytics("Daily_Sign", dic);
    }
    /// <summary>
    /// 版本更新记录
    /// </summary>
    /// <param name="OldAppVersion">更新前版本号</param>
    /// <param name="NewAppVersion">更新后版本号</param>
    /// <param name="OldAssetVersion">更新前版本号</param>
    /// <param name="NewAssetVersion">更新后版本号</param>
    public static void Version_Updating(string OldAppVersion,string NewAppVersion,string OldAssetVersion,string NewAssetVersion)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        try
        {
			dic.Add("OldAppVersion", OldAppVersion.ToString());
			dic.Add("NewAppVersion", NewAppVersion.ToString());
			dic.Add("OldAssetVersion", OldAssetVersion.ToString());
			dic.Add("NewAssetVersion", NewAssetVersion.ToString());

        }
        catch(Exception e)
        {
            DebugUtil.LogError("Version_Updating:"+e.ToString());
        }
        AnalyticsTool.Analytics("Version_Updating", dic);
    }
    /// <summary>
    /// 获得排行榜活动奖励记录
    /// </summary>
    /// <param name="ActId">活动ID</param>
    /// <param name="Award">道具ID及数量</param>
    public static void Activity_Award(string ActId,string Award)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        try
        {
			dic.Add("ActId", ActId.ToString());
			dic.Add("Award", Award.ToString());

        }
        catch(Exception e)
        {
            DebugUtil.LogError("Activity_Award:"+e.ToString());
        }
        AnalyticsTool.Analytics("Activity_Award", dic);
    }
    /// <summary>
    /// 新手引导成功
    /// </summary>
    /// <param name="StepId">步骤ID</param>
    public static void Guild_succeed(int StepId)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        try
        {
			dic.Add("StepId", StepId.ToString());

        }
        catch(Exception e)
        {
            DebugUtil.LogError("Guild_succeed:"+e.ToString());
        }
        AnalyticsTool.Analytics("Guild_succeed", dic);
    }
    /// <summary>
    /// 点击开始游戏按钮
    /// </summary>
    public static void Game_Start()
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        try
        {

        }
        catch(Exception e)
        {
            DebugUtil.LogError("Game_Start:"+e.ToString());
        }
        AnalyticsTool.Analytics("Game_Start", dic);
    }
    /// <summary>
    /// 道具
    /// </summary>
    /// <param name="Action">消耗/获得类型</param>
    /// <param name="GoodsId">道具Id</param>
    /// <param name="NewNum">变化后数量</param>
    /// <param name="ChangeNum">变化数量</param>
    public static void Goods(string Action,int GoodsId,long NewNum,long ChangeNum)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        try
        {
			dic.Add("Action", Action.ToString());
			dic.Add("GoodsId", GoodsId.ToString());
			dic.Add("NewNum", NewNum.ToString());
			dic.Add("ChangeNum", ChangeNum.ToString());

        }
        catch(Exception e)
        {
            DebugUtil.LogError("Goods:"+e.ToString());
        }
        AnalyticsTool.Analytics("Goods", dic);
    }
    /// <summary>
    /// 绑定账号
    /// </summary>
    /// <param name="accountType">第三方类型, 1:fackbook</param>
    /// <param name="accountId">第三方ID</param>
    public static void BindAccount(int accountType,string accountId)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        try
        {
			dic.Add("accountType", accountType.ToString());
			dic.Add("accountId", accountId.ToString());

        }
        catch(Exception e)
        {
            DebugUtil.LogError("BindAccount:"+e.ToString());
        }
        AnalyticsTool.Analytics("BindAccount", dic);
    }
    /// <summary>
    /// 错误信息
    /// </summary>
    /// <param name="msg">错误信息</param>
    /// <param name="action">报错场景(原因)</param>
    public static void ErrorMsg(string msg,string action)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        try
        {
			dic.Add("msg", msg.ToString());
			dic.Add("action", action.ToString());

        }
        catch(Exception e)
        {
            DebugUtil.LogError("ErrorMsg:"+e.ToString());
        }
        AnalyticsTool.Analytics("ErrorMsg", dic);
    }
    /// <summary>
    /// 限制礼包统计
    /// </summary>
    /// <param name="type">弹框:1弹出,2购买,3关闭</param>
    /// <param name="push">推送入口:1登陆,2回到地图,3金币不足</param>
    /// <param name="giftId">礼包id</param>
    /// <param name="coin">金币</param>
    /// <param name="intervalTime">距上次礼包弹出时间(秒)</param>
    /// <param name="showTime">显示时间(秒):step=2(弹出至购买的时间),step=3(弹出至关闭的时间)</param>
    public static void LimitGift(int type,int push,int giftId,long coin,int intervalTime,int showTime)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        try
        {
			dic.Add("type", type.ToString());
			dic.Add("push", push.ToString());
			dic.Add("giftId", giftId.ToString());
			dic.Add("coin", coin.ToString());
			dic.Add("intervalTime", intervalTime.ToString());
			dic.Add("showTime", showTime.ToString());

        }
        catch(Exception e)
        {
            DebugUtil.LogError("LimitGift:"+e.ToString());
        }
        AnalyticsTool.Analytics("LimitGift", dic);
    }
    /// <summary>
    /// 评论(五星好评)
    /// </summary>
    /// <param name="type">弹框:1弹出,2关闭,3评论后返回</param>
    /// <param name="showNum">累计弹出次数</param>
    /// <param name="showTime">停留时间</param>
    public static void Reviews(int type,int showNum,int showTime)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        try
        {
			dic.Add("type", type.ToString());
			dic.Add("showNum", showNum.ToString());
			dic.Add("showTime", showTime.ToString());

        }
        catch(Exception e)
        {
            DebugUtil.LogError("Reviews:"+e.ToString());
        }
        AnalyticsTool.Analytics("Reviews", dic);
    }
    /// <summary>
    /// 记录初始化时间
    /// </summary>
    /// <param name="initCostTimeByStep">启动初始化时各步骤的用时</param>
    public static void InitTime(Dictionary<string, string> initCostTimeByStep)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        try
        {
			AddDicAsJsonToDic(dic, "initCostTimeByStep", initCostTimeByStep);

        }
        catch(Exception e)
        {
            DebugUtil.LogError("InitTime:"+e.ToString());
        }
        AnalyticsTool.Analytics("InitTime", dic);
    }



    private static void AddDicAsJsonToDic<T1,T2>(Dictionary<string, string> dic, string key, Dictionary<T1, T2> dic2)
    {
        dic.Add(key, JsonConvert.SerializeObject(dic2));
    }
}