using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Asset;
namespace Framework.Tables {
    public class TableGameConst
    {
        private static string FileName = "Table/GameConst";
        private static bool Inited = false;

        public TableGameConst()
		{
		}
        public void Clear()
        {
            Inited = false;
        }

        /// <summary>
        /// 游戏的开始时间2020-08-25
        /// </summary>
        private int _GameStartTime;
        public int GameStartTime
        {
            private set
            {
                _GameStartTime = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _GameStartTime;
            }
        }
        /// <summary>
        /// 开放关卡
        /// </summary>
        private int _levelMax;
        public int levelMax
        {
            private set
            {
                _levelMax = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _levelMax;
            }
        }
        /// <summary>
        /// 初始金币
        /// </summary>
        private int _default_coin;
        public int default_coin
        {
            private set
            {
                _default_coin = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _default_coin;
            }
        }
        /// <summary>
        /// 初始精力
        /// </summary>
        private int _default_life;
        public int default_life
        {
            private set
            {
                _default_life = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _default_life;
            }
        }
        /// <summary>
        /// 生命倒计时，单位秒
        /// </summary>
        private int _life_time;
        public int life_time
        {
            private set
            {
                _life_time = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _life_time;
            }
        }
        /// <summary>
        /// 绑定FB奖励
        /// </summary>
        private int _reward_bind_fb;
        public int reward_bind_fb
        {
            private set
            {
                _reward_bind_fb = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _reward_bind_fb;
            }
        }
        /// <summary>
        /// 打开宝箱所消耗的星星
        /// </summary>
        private int _star_box_cost;
        public int star_box_cost
        {
            private set
            {
                _star_box_cost = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _star_box_cost;
            }
        }
        /// <summary>
        /// 炸弹销毁棋子得分的翻倍数
        /// </summary>
        private int _mult_destroy_score;
        public int mult_destroy_score
        {
            private set
            {
                _mult_destroy_score = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _mult_destroy_score;
            }
        }
        /// <summary>
        /// 连胜的最大奖励倍数，基数为一个炸弹一个火箭
        /// </summary>
        private int _continuedWin_maxBouns;
        public int continuedWin_maxBouns
        {
            private set
            {
                _continuedWin_maxBouns = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _continuedWin_maxBouns;
            }
        }
        /// <summary>
        /// 免费转盘恢复时间
        /// </summary>
        private int _FreeWheelRecoverTime;
        public int FreeWheelRecoverTime
        {
            private set
            {
                _FreeWheelRecoverTime = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _FreeWheelRecoverTime;
            }
        }
        /// <summary>
        /// 付费转盘耗费的金币
        /// </summary>
        private int _FreeWheelCostCoin;
        public int FreeWheelCostCoin
        {
            private set
            {
                _FreeWheelCostCoin = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _FreeWheelCostCoin;
            }
        }
        /// <summary>
        /// 一天看广告免费转转盘的次数
        /// </summary>
        private int _AdWheelCountOneDay;
        public int AdWheelCountOneDay
        {
            private set
            {
                _AdWheelCountOneDay = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _AdWheelCountOneDay;
            }
        }
        /// <summary>
        /// 每天转盘广告次数
        /// </summary>
        private int _DailyAdWheelCountOneDay;
        public int DailyAdWheelCountOneDay
        {
            private set
            {
                _DailyAdWheelCountOneDay = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _DailyAdWheelCountOneDay;
            }
        }
        /// <summary>
        /// 首充的商品id
        /// </summary>
        private int _FirstSalePromotionProductId;
        public int FirstSalePromotionProductId
        {
            private set
            {
                _FirstSalePromotionProductId = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _FirstSalePromotionProductId;
            }
        }
        /// <summary>
        /// 在首页显示每日目标的等级
        /// </summary>
        private int _ShowDailyTargetLevel;
        public int ShowDailyTargetLevel
        {
            private set
            {
                _ShowDailyTargetLevel = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _ShowDailyTargetLevel;
            }
        }
        /// <summary>
        /// 个人军备开放等级
        /// </summary>
        private int _PersonalStarRaceOpenLevel;
        public int PersonalStarRaceOpenLevel
        {
            private set
            {
                _PersonalStarRaceOpenLevel = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _PersonalStarRaceOpenLevel;
            }
        }
        /// <summary>
        /// 连胜解锁等级
        /// </summary>
        private int _continuedWin_unlockLevel;
        public int continuedWin_unlockLevel
        {
            private set
            {
                _continuedWin_unlockLevel = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _continuedWin_unlockLevel;
            }
        }
        /// <summary>
        /// 每日转盘解锁等级
        /// </summary>
        private int _dailyWheel_unlockLevel;
        public int dailyWheel_unlockLevel
        {
            private set
            {
                _dailyWheel_unlockLevel = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _dailyWheel_unlockLevel;
            }
        }
        /// <summary>
        /// 在首页显示首充破冰图标的等级
        /// </summary>
        private int _ShowFirstSalePromotionLevel;
        public int ShowFirstSalePromotionLevel
        {
            private set
            {
                _ShowFirstSalePromotionLevel = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _ShowFirstSalePromotionLevel;
            }
        }
        /// <summary>
        /// 皇冠面板自动弹出，在此等级后不自动弹出
        /// </summary>
        private int _notPopContinueWinPanelAfterThisLevel;
        public int notPopContinueWinPanelAfterThisLevel
        {
            private set
            {
                _notPopContinueWinPanelAfterThisLevel = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _notPopContinueWinPanelAfterThisLevel;
            }
        }
        /// <summary>
        /// 每天看广告开始获得初始道具次数
        /// </summary>
        private int _adPlayCountOneDay;
        public int adPlayCountOneDay
        {
            private set
            {
                _adPlayCountOneDay = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _adPlayCountOneDay;
            }
        }
        /// <summary>
        /// 广告开始获得初始道具功能开放等级
        /// </summary>
        private int _adplay_unlockLevel;
        public int adplay_unlockLevel
        {
            private set
            {
                _adplay_unlockLevel = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _adplay_unlockLevel;
            }
        }
        /// <summary>
        /// 是否弹出5星面板
        /// </summary>
        private int _popRateUsPanel;
        public int popRateUsPanel
        {
            private set
            {
                _popRateUsPanel = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _popRateUsPanel;
            }
        }
        /// <summary>
        /// 弹出5星面板的等级
        /// </summary>
        private int _popRateUsLevel;
        public int popRateUsLevel
        {
            private set
            {
                _popRateUsLevel = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _popRateUsLevel;
            }
        }


        public void Init () {
            if (!Table.Inited)
            {
                throw new Exception("Tabele has not been initialised,it is intialised in GameController");
            }
            if (!Inited) {
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode itemData = JSONNode.Parse (tableStr);
				GameStartTime = itemData["GameStartTime"];
				levelMax = itemData["levelMax"];
				default_coin = itemData["default_coin"];
				default_life = itemData["default_life"];
				life_time = itemData["life_time"];
				reward_bind_fb = itemData["reward_bind_fb"];
				star_box_cost = itemData["star_box_cost"];
				mult_destroy_score = itemData["mult_destroy_score"];
				continuedWin_maxBouns = itemData["continuedWin_maxBouns"];
				FreeWheelRecoverTime = itemData["FreeWheelRecoverTime"];
				FreeWheelCostCoin = itemData["FreeWheelCostCoin"];
				AdWheelCountOneDay = itemData["AdWheelCountOneDay"];
				DailyAdWheelCountOneDay = itemData["DailyAdWheelCountOneDay"];
				FirstSalePromotionProductId = itemData["FirstSalePromotionProductId"];
				ShowDailyTargetLevel = itemData["ShowDailyTargetLevel"];
				PersonalStarRaceOpenLevel = itemData["PersonalStarRaceOpenLevel"];
				continuedWin_unlockLevel = itemData["continuedWin_unlockLevel"];
				dailyWheel_unlockLevel = itemData["dailyWheel_unlockLevel"];
				ShowFirstSalePromotionLevel = itemData["ShowFirstSalePromotionLevel"];
				notPopContinueWinPanelAfterThisLevel = itemData["notPopContinueWinPanelAfterThisLevel"];
				adPlayCountOneDay = itemData["adPlayCountOneDay"];
				adplay_unlockLevel = itemData["adplay_unlockLevel"];
				popRateUsPanel = itemData["popRateUsPanel"];
				popRateUsLevel = itemData["popRateUsLevel"];

                Inited = true;
            }
        }

        public void Init (string tableStr) {


                JSONNode itemData = JSONNode.Parse (tableStr);
				GameStartTime = itemData["GameStartTime"];
				levelMax = itemData["levelMax"];
				default_coin = itemData["default_coin"];
				default_life = itemData["default_life"];
				life_time = itemData["life_time"];
				reward_bind_fb = itemData["reward_bind_fb"];
				star_box_cost = itemData["star_box_cost"];
				mult_destroy_score = itemData["mult_destroy_score"];
				continuedWin_maxBouns = itemData["continuedWin_maxBouns"];
				FreeWheelRecoverTime = itemData["FreeWheelRecoverTime"];
				FreeWheelCostCoin = itemData["FreeWheelCostCoin"];
				AdWheelCountOneDay = itemData["AdWheelCountOneDay"];
				DailyAdWheelCountOneDay = itemData["DailyAdWheelCountOneDay"];
				FirstSalePromotionProductId = itemData["FirstSalePromotionProductId"];
				ShowDailyTargetLevel = itemData["ShowDailyTargetLevel"];
				PersonalStarRaceOpenLevel = itemData["PersonalStarRaceOpenLevel"];
				continuedWin_unlockLevel = itemData["continuedWin_unlockLevel"];
				dailyWheel_unlockLevel = itemData["dailyWheel_unlockLevel"];
				ShowFirstSalePromotionLevel = itemData["ShowFirstSalePromotionLevel"];
				notPopContinueWinPanelAfterThisLevel = itemData["notPopContinueWinPanelAfterThisLevel"];
				adPlayCountOneDay = itemData["adPlayCountOneDay"];
				adplay_unlockLevel = itemData["adplay_unlockLevel"];
				popRateUsPanel = itemData["popRateUsPanel"];
				popRateUsLevel = itemData["popRateUsLevel"];

                Inited = true;
        }


    }
}