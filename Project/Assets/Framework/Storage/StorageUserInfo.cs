/************************************************
 * Storage class : StorageCommon
 * This file is can not be modify !!!
 * If there is some problem, ask XieXueyong.
 ************************************************/

using System;
using EventUtil;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic; 

namespace Framework.Storage {
    [System.Serializable]
    public class StorageUserInfo : StorageBase {

        public static string StorageUserInfo_Change_HasSetDefault = "StorageUserInfo_Change_HasSetDefault";
		public static string StorageUserInfo_Change_HasGive5Star = "StorageUserInfo_Change_HasGive5Star";
		public static string StorageUserInfo_Change_HeadIconID = "StorageUserInfo_Change_HeadIconID";
		public static string StorageUserInfo_Change_NickName = "StorageUserInfo_Change_NickName";
		public static string StorageUserInfo_Change_Level = "StorageUserInfo_Change_Level";
		public static string StorageUserInfo_Change_CurStar = "StorageUserInfo_Change_CurStar";
		public static string StorageUserInfo_Change_MailData = "StorageUserInfo_Change_MailData";
		public static string StorageUserInfo_Change_GoodsCount = "StorageUserInfo_Change_GoodsCount";
		public static string StorageUserInfo_Change_Furniture = "StorageUserInfo_Change_Furniture";
		public static string StorageUserInfo_Change_AlreadyGuide = "StorageUserInfo_Change_AlreadyGuide";
		public static string StorageUserInfo_Change_Purchased = "StorageUserInfo_Change_Purchased";
		public static string StorageUserInfo_Change_Rated = "StorageUserInfo_Change_Rated";
		public static string StorageUserInfo_Change_HasTutorialPersonalRace = "StorageUserInfo_Change_HasTutorialPersonalRace";
		public static string StorageUserInfo_Change_HasTutorialDailyTarget = "StorageUserInfo_Change_HasTutorialDailyTarget";
		public static string StorageUserInfo_Change_HasTutorialDailyWheel = "StorageUserInfo_Change_HasTutorialDailyWheel";
		public static string StorageUserInfo_Change_HasFirstPurchase = "StorageUserInfo_Change_HasFirstPurchase";
		public static string StorageUserInfo_Change_PayTotal = "StorageUserInfo_Change_PayTotal";
		public static string StorageUserInfo_Change_CountDownTime = "StorageUserInfo_Change_CountDownTime";
		public static string StorageUserInfo_Change_InfiniteLifeStartTime = "StorageUserInfo_Change_InfiniteLifeStartTime";
		public static string StorageUserInfo_Change_InfiniteLifeTimeSpan = "StorageUserInfo_Change_InfiniteLifeTimeSpan";
		public static string StorageUserInfo_Change_GuideFinishLevel = "StorageUserInfo_Change_GuideFinishLevel";
		public static string StorageUserInfo_Change_LevelBox_GainLevel = "StorageUserInfo_Change_LevelBox_GainLevel";
		public static string StorageUserInfo_Change_continuedWinCount = "StorageUserInfo_Change_continuedWinCount";
		public static string StorageUserInfo_Change_FreeWheelStarTime = "StorageUserInfo_Change_FreeWheelStarTime";
		public static string StorageUserInfo_Change_AdWheelCount = "StorageUserInfo_Change_AdWheelCount";
		public static string StorageUserInfo_Change_DailyWheel = "StorageUserInfo_Change_DailyWheel";
		public static string StorageUserInfo_Change_DailyAdWheelCount = "StorageUserInfo_Change_DailyAdWheelCount";
		public static string StorageUserInfo_Change_LoginRewardDate = "StorageUserInfo_Change_LoginRewardDate";
		public static string StorageUserInfo_Change_LoginRewardDay = "StorageUserInfo_Change_LoginRewardDay";
		public static string StorageUserInfo_Change_TutorialRewardLevel = "StorageUserInfo_Change_TutorialRewardLevel";
		public static string StorageUserInfo_Change_PurchaseCoinsAmount = "StorageUserInfo_Change_PurchaseCoinsAmount";
		public static string StorageUserInfo_Change_PurchaRewardLevel = "StorageUserInfo_Change_PurchaRewardLevel";
		public static string StorageUserInfo_Change_DailyBI = "StorageUserInfo_Change_DailyBI";
		public static string StorageUserInfo_Change_DailyTargetRewardCollected = "StorageUserInfo_Change_DailyTargetRewardCollected";
		public static string StorageUserInfo_Change_LevelScore = "StorageUserInfo_Change_LevelScore";
		public static string StorageUserInfo_Change_DailyAdPlayCount = "StorageUserInfo_Change_DailyAdPlayCount";
		public static string StorageUserInfo_Change_LoginRewardLastDay = "StorageUserInfo_Change_LoginRewardLastDay";
		public static string StorageUserInfo_Change_task_chapter_id = "StorageUserInfo_Change_task_chapter_id";
		public static string StorageUserInfo_Change_task_part_progress = "StorageUserInfo_Change_task_part_progress";
		public static string StorageUserInfo_Change_key = "StorageUserInfo_Change_key";
		public static string StorageUserInfo_Change_unlockDiary = "StorageUserInfo_Change_unlockDiary";
		public static string StorageUserInfo_Change_unlockArchives = "StorageUserInfo_Change_unlockArchives";
		public static string StorageUserInfo_Change_collections = "StorageUserInfo_Change_collections";
		public static string StorageUserInfo_Change_TutorialSteps = "StorageUserInfo_Change_TutorialSteps";
		
        
        /// <summary>
        /// ??????????????????
        /// </summary>
        [JsonIgnore]
        private bool _HasSetDefault_immediately = false;
        [JsonProperty]
        private bool _HasSetDefault;
        [JsonIgnore]
        public bool HasSetDefault {
            get {
                return _HasSetDefault;
            }
            set {
                if (_HasSetDefault != value) {
                    bool oldValue = _HasSetDefault;
                    _HasSetDefault = value;
                    EventDispatcher.TriggerEvent<bool, bool>(StorageUserInfo_Change_HasSetDefault, oldValue, value);
                    if(_HasSetDefault_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// ??????????????????
        /// </summary>
        [JsonIgnore]
        private bool _HasGive5Star_immediately = false;
        [JsonProperty]
        private bool _HasGive5Star;
        [JsonIgnore]
        public bool HasGive5Star {
            get {
                return _HasGive5Star;
            }
            set {
                if (_HasGive5Star != value) {
                    bool oldValue = _HasGive5Star;
                    _HasGive5Star = value;
                    EventDispatcher.TriggerEvent<bool, bool>(StorageUserInfo_Change_HasGive5Star, oldValue, value);
                    if(_HasGive5Star_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// ??????ID
        /// </summary>
        [JsonIgnore]
        private bool _HeadIconID_immediately = true;
        [JsonProperty]
        private int _HeadIconID;
        [JsonIgnore]
        public int HeadIconID {
            get {
                return _HeadIconID;
            }
            set {
                if (_HeadIconID != value) {
                    int oldValue = _HeadIconID;
                    _HeadIconID = value;
                    EventDispatcher.TriggerEvent<int, int>(StorageUserInfo_Change_HeadIconID, oldValue, value);
                    if(_HeadIconID_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// ????????????
        /// </summary>
        [JsonIgnore]
        private bool _NickName_immediately = true;
        [JsonProperty]
        private string _NickName;
        [JsonIgnore]
        public string NickName {
            get {
                return _NickName;
            }
            set {
                if (_NickName != value) {
                    string oldValue = _NickName;
                    _NickName = value;
                    EventDispatcher.TriggerEvent<string, string>(StorageUserInfo_Change_NickName, oldValue, value);
                    if(_NickName_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// ????????????????????????
        /// </summary>
        [JsonIgnore]
        private bool _Level_immediately = true;
        [JsonProperty]
        private int _Level;
        [JsonIgnore]
        public int Level {
            get {
                return _Level;
            }
            set {
                if (_Level != value) {
                    int oldValue = _Level;
                    _Level = value;
                    EventDispatcher.TriggerEvent<int, int>(StorageUserInfo_Change_Level, oldValue, value);
                    if(_Level_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// ?????????????????????Star??????star??????goodsId.star???
        /// </summary>
        [JsonIgnore]
        private bool _CurStar_immediately = true;
        [JsonProperty]
        private int _CurStar;
        [JsonIgnore]
        public int CurStar {
            get {
                return _CurStar;
            }
            set {
                if (_CurStar != value) {
                    int oldValue = _CurStar;
                    _CurStar = value;
                    EventDispatcher.TriggerEvent<int, int>(StorageUserInfo_Change_CurStar, oldValue, value);
                    if(_CurStar_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// ?????????????????????
        /// </summary>
        [JsonIgnore]
        private bool _Rated_immediately = true;
        [JsonProperty]
        private bool _Rated;
        [JsonIgnore]
        public bool Rated {
            get {
                return _Rated;
            }
            set {
                if (_Rated != value) {
                    bool oldValue = _Rated;
                    _Rated = value;
                    EventDispatcher.TriggerEvent<bool, bool>(StorageUserInfo_Change_Rated, oldValue, value);
                    if(_Rated_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// ???????????????????????????
        /// </summary>
        [JsonIgnore]
        private bool _HasTutorialPersonalRace_immediately = true;
        [JsonProperty]
        private bool _HasTutorialPersonalRace;
        [JsonIgnore]
        public bool HasTutorialPersonalRace {
            get {
                return _HasTutorialPersonalRace;
            }
            set {
                if (_HasTutorialPersonalRace != value) {
                    bool oldValue = _HasTutorialPersonalRace;
                    _HasTutorialPersonalRace = value;
                    EventDispatcher.TriggerEvent<bool, bool>(StorageUserInfo_Change_HasTutorialPersonalRace, oldValue, value);
                    if(_HasTutorialPersonalRace_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// ?????????????????????????????????
        /// </summary>
        [JsonIgnore]
        private bool _HasTutorialDailyTarget_immediately = true;
        [JsonProperty]
        private bool _HasTutorialDailyTarget;
        [JsonIgnore]
        public bool HasTutorialDailyTarget {
            get {
                return _HasTutorialDailyTarget;
            }
            set {
                if (_HasTutorialDailyTarget != value) {
                    bool oldValue = _HasTutorialDailyTarget;
                    _HasTutorialDailyTarget = value;
                    EventDispatcher.TriggerEvent<bool, bool>(StorageUserInfo_Change_HasTutorialDailyTarget, oldValue, value);
                    if(_HasTutorialDailyTarget_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// ???????????????????????????
        /// </summary>
        [JsonIgnore]
        private bool _HasTutorialDailyWheel_immediately = true;
        [JsonProperty]
        private bool _HasTutorialDailyWheel;
        [JsonIgnore]
        public bool HasTutorialDailyWheel {
            get {
                return _HasTutorialDailyWheel;
            }
            set {
                if (_HasTutorialDailyWheel != value) {
                    bool oldValue = _HasTutorialDailyWheel;
                    _HasTutorialDailyWheel = value;
                    EventDispatcher.TriggerEvent<bool, bool>(StorageUserInfo_Change_HasTutorialDailyWheel, oldValue, value);
                    if(_HasTutorialDailyWheel_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// ???????????????
        /// </summary>
        [JsonIgnore]
        private bool _HasFirstPurchase_immediately = true;
        [JsonProperty]
        private bool _HasFirstPurchase;
        [JsonIgnore]
        public bool HasFirstPurchase {
            get {
                return _HasFirstPurchase;
            }
            set {
                if (_HasFirstPurchase != value) {
                    bool oldValue = _HasFirstPurchase;
                    _HasFirstPurchase = value;
                    EventDispatcher.TriggerEvent<bool, bool>(StorageUserInfo_Change_HasFirstPurchase, oldValue, value);
                    if(_HasFirstPurchase_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// ???????????????
        /// </summary>
        [JsonIgnore]
        private bool _PayTotal_immediately = true;
        [JsonProperty]
        private int _PayTotal;
        [JsonIgnore]
        public int PayTotal {
            get {
                return _PayTotal;
            }
            set {
                if (_PayTotal != value) {
                    int oldValue = _PayTotal;
                    _PayTotal = value;
                    EventDispatcher.TriggerEvent<int, int>(StorageUserInfo_Change_PayTotal, oldValue, value);
                    if(_PayTotal_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// ?????????????????????
        /// </summary>
        [JsonIgnore]
        private bool _CountDownTime_immediately = true;
        [JsonProperty]
        private int _CountDownTime;
        [JsonIgnore]
        public int CountDownTime {
            get {
                return _CountDownTime;
            }
            set {
                if (_CountDownTime != value) {
                    int oldValue = _CountDownTime;
                    _CountDownTime = value;
                    EventDispatcher.TriggerEvent<int, int>(StorageUserInfo_Change_CountDownTime, oldValue, value);
                    if(_CountDownTime_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// ???????????????????????????
        /// </summary>
        [JsonIgnore]
        private bool _InfiniteLifeStartTime_immediately = true;
        [JsonProperty]
        private int _InfiniteLifeStartTime;
        [JsonIgnore]
        public int InfiniteLifeStartTime {
            get {
                return _InfiniteLifeStartTime;
            }
            set {
                if (_InfiniteLifeStartTime != value) {
                    int oldValue = _InfiniteLifeStartTime;
                    _InfiniteLifeStartTime = value;
                    EventDispatcher.TriggerEvent<int, int>(StorageUserInfo_Change_InfiniteLifeStartTime, oldValue, value);
                    if(_InfiniteLifeStartTime_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// ?????????????????????
        /// </summary>
        [JsonIgnore]
        private bool _InfiniteLifeTimeSpan_immediately = true;
        [JsonProperty]
        private int _InfiniteLifeTimeSpan;
        [JsonIgnore]
        public int InfiniteLifeTimeSpan {
            get {
                return _InfiniteLifeTimeSpan;
            }
            set {
                if (_InfiniteLifeTimeSpan != value) {
                    int oldValue = _InfiniteLifeTimeSpan;
                    _InfiniteLifeTimeSpan = value;
                    EventDispatcher.TriggerEvent<int, int>(StorageUserInfo_Change_InfiniteLifeTimeSpan, oldValue, value);
                    if(_InfiniteLifeTimeSpan_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// ?????????????????????
        /// </summary>
        [JsonIgnore]
        private bool _GuideFinishLevel_immediately = true;
        [JsonProperty]
        private int _GuideFinishLevel;
        [JsonIgnore]
        public int GuideFinishLevel {
            get {
                return _GuideFinishLevel;
            }
            set {
                if (_GuideFinishLevel != value) {
                    int oldValue = _GuideFinishLevel;
                    _GuideFinishLevel = value;
                    EventDispatcher.TriggerEvent<int, int>(StorageUserInfo_Change_GuideFinishLevel, oldValue, value);
                    if(_GuideFinishLevel_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// ??????????????????????????????
        /// </summary>
        [JsonIgnore]
        private bool _LevelBox_GainLevel_immediately = true;
        [JsonProperty]
        private int _LevelBox_GainLevel;
        [JsonIgnore]
        public int LevelBox_GainLevel {
            get {
                return _LevelBox_GainLevel;
            }
            set {
                if (_LevelBox_GainLevel != value) {
                    int oldValue = _LevelBox_GainLevel;
                    _LevelBox_GainLevel = value;
                    EventDispatcher.TriggerEvent<int, int>(StorageUserInfo_Change_LevelBox_GainLevel, oldValue, value);
                    if(_LevelBox_GainLevel_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// ?????????????????????
        /// </summary>
        [JsonIgnore]
        private bool _continuedWinCount_immediately = true;
        [JsonProperty]
        private int _continuedWinCount;
        [JsonIgnore]
        public int continuedWinCount {
            get {
                return _continuedWinCount;
            }
            set {
                if (_continuedWinCount != value) {
                    int oldValue = _continuedWinCount;
                    _continuedWinCount = value;
                    EventDispatcher.TriggerEvent<int, int>(StorageUserInfo_Change_continuedWinCount, oldValue, value);
                    if(_continuedWinCount_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// ?????????????????????????????????
        /// </summary>
        [JsonIgnore]
        private bool _FreeWheelStarTime_immediately = true;
        [JsonProperty]
        private int _FreeWheelStarTime;
        [JsonIgnore]
        public int FreeWheelStarTime {
            get {
                return _FreeWheelStarTime;
            }
            set {
                if (_FreeWheelStarTime != value) {
                    int oldValue = _FreeWheelStarTime;
                    _FreeWheelStarTime = value;
                    EventDispatcher.TriggerEvent<int, int>(StorageUserInfo_Change_FreeWheelStarTime, oldValue, value);
                    if(_FreeWheelStarTime_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// ???????????????????????????
        /// </summary>
        [JsonIgnore]
        private bool _LoginRewardDate_immediately = true;
        [JsonProperty]
        private int _LoginRewardDate;
        [JsonIgnore]
        public int LoginRewardDate {
            get {
                return _LoginRewardDate;
            }
            set {
                if (_LoginRewardDate != value) {
                    int oldValue = _LoginRewardDate;
                    _LoginRewardDate = value;
                    EventDispatcher.TriggerEvent<int, int>(StorageUserInfo_Change_LoginRewardDate, oldValue, value);
                    if(_LoginRewardDate_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// ?????????????????????????????????
        /// </summary>
        [JsonIgnore]
        private bool _LoginRewardDay_immediately = true;
        [JsonProperty]
        private int _LoginRewardDay;
        [JsonIgnore]
        public int LoginRewardDay {
            get {
                return _LoginRewardDay;
            }
            set {
                if (_LoginRewardDay != value) {
                    int oldValue = _LoginRewardDay;
                    _LoginRewardDay = value;
                    EventDispatcher.TriggerEvent<int, int>(StorageUserInfo_Change_LoginRewardDay, oldValue, value);
                    if(_LoginRewardDay_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// ?????????????????????????????????
        /// </summary>
        [JsonIgnore]
        private bool _TutorialRewardLevel_immediately = true;
        [JsonProperty]
        private int _TutorialRewardLevel;
        [JsonIgnore]
        public int TutorialRewardLevel {
            get {
                return _TutorialRewardLevel;
            }
            set {
                if (_TutorialRewardLevel != value) {
                    int oldValue = _TutorialRewardLevel;
                    _TutorialRewardLevel = value;
                    EventDispatcher.TriggerEvent<int, int>(StorageUserInfo_Change_TutorialRewardLevel, oldValue, value);
                    if(_TutorialRewardLevel_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// ????????????????????????
        /// </summary>
        [JsonIgnore]
        private bool _PurchaseCoinsAmount_immediately = true;
        [JsonProperty]
        private int _PurchaseCoinsAmount;
        [JsonIgnore]
        public int PurchaseCoinsAmount {
            get {
                return _PurchaseCoinsAmount;
            }
            set {
                if (_PurchaseCoinsAmount != value) {
                    int oldValue = _PurchaseCoinsAmount;
                    _PurchaseCoinsAmount = value;
                    EventDispatcher.TriggerEvent<int, int>(StorageUserInfo_Change_PurchaseCoinsAmount, oldValue, value);
                    if(_PurchaseCoinsAmount_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// ???????????????????????????
        /// </summary>
        [JsonIgnore]
        private bool _PurchaRewardLevel_immediately = true;
        [JsonProperty]
        private int _PurchaRewardLevel;
        [JsonIgnore]
        public int PurchaRewardLevel {
            get {
                return _PurchaRewardLevel;
            }
            set {
                if (_PurchaRewardLevel != value) {
                    int oldValue = _PurchaRewardLevel;
                    _PurchaRewardLevel = value;
                    EventDispatcher.TriggerEvent<int, int>(StorageUserInfo_Change_PurchaRewardLevel, oldValue, value);
                    if(_PurchaRewardLevel_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// ???????????????????????????
        /// </summary>
        [JsonIgnore]
        private bool _LoginRewardLastDay_immediately = true;
        [JsonProperty]
        private int _LoginRewardLastDay;
        [JsonIgnore]
        public int LoginRewardLastDay {
            get {
                return _LoginRewardLastDay;
            }
            set {
                if (_LoginRewardLastDay != value) {
                    int oldValue = _LoginRewardLastDay;
                    _LoginRewardLastDay = value;
                    EventDispatcher.TriggerEvent<int, int>(StorageUserInfo_Change_LoginRewardLastDay, oldValue, value);
                    if(_LoginRewardLastDay_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// ??????????????????
        /// </summary>
        [JsonIgnore]
        private bool _task_chapter_id_immediately = true;
        [JsonProperty]
        private int _task_chapter_id;
        [JsonIgnore]
        public int task_chapter_id {
            get {
                return _task_chapter_id;
            }
            set {
                if (_task_chapter_id != value) {
                    int oldValue = _task_chapter_id;
                    _task_chapter_id = value;
                    EventDispatcher.TriggerEvent<int, int>(StorageUserInfo_Change_task_chapter_id, oldValue, value);
                    if(_task_chapter_id_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// ???????????????
        /// </summary>
        [JsonIgnore]
        private bool _key_immediately = true;
        [JsonProperty]
        private int _key;
        [JsonIgnore]
        public int key {
            get {
                return _key;
            }
            set {
                if (_key != value) {
                    int oldValue = _key;
                    _key = value;
                    EventDispatcher.TriggerEvent<int, int>(StorageUserInfo_Change_key, oldValue, value);
                    if(_key_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// ???????????????id?????????
        /// </summary>
        [JsonIgnore]
        private bool _unlockDiary_immediately = true;
        [JsonProperty]
        private int _unlockDiary;
        [JsonIgnore]
        public int unlockDiary {
            get {
                return _unlockDiary;
            }
            set {
                if (_unlockDiary != value) {
                    int oldValue = _unlockDiary;
                    _unlockDiary = value;
                    EventDispatcher.TriggerEvent<int, int>(StorageUserInfo_Change_unlockDiary, oldValue, value);
                    if(_unlockDiary_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        
        /// <summary>
        /// ??????
        /// </summary>
        [JsonProperty]
        private StorageList<MailData> _MailData = new StorageList<MailData> (StorageUserInfo_Change_MailData,true);
        [JsonIgnore]
        public StorageList<MailData> MailData {
            get {
                return _MailData;
            }
        }

        /// <summary>
        /// ?????????????????????ID??????????????????
        /// </summary>
        [JsonProperty]
        private StorageDictionary<int,int> _GoodsCount = new StorageDictionary<int,int> (StorageUserInfo_Change_GoodsCount,true);
        [JsonIgnore]
        public StorageDictionary<int,int> GoodsCount {
            get {
                return _GoodsCount;
            }
        }

        /// <summary>
        /// ?????????????????????ID??????????????????
        /// </summary>
        [JsonProperty]
        private StorageDictionary<string,string> _Furniture = new StorageDictionary<string,string> (StorageUserInfo_Change_Furniture,true);
        [JsonIgnore]
        public StorageDictionary<string,string> Furniture {
            get {
                return _Furniture;
            }
        }

        /// <summary>
        /// ????????????
        /// </summary>
        [JsonProperty]
        private StorageList<int> _AlreadyGuide = new StorageList<int> (StorageUserInfo_Change_AlreadyGuide,true);
        [JsonIgnore]
        public StorageList<int> AlreadyGuide {
            get {
                return _AlreadyGuide;
            }
        }

        /// <summary>
        /// ??????????????????
        /// </summary>
        [JsonProperty]
        private StorageList<string> _Purchased = new StorageList<string> (StorageUserInfo_Change_Purchased,true);
        [JsonIgnore]
        public StorageList<string> Purchased {
            get {
                return _Purchased;
            }
        }

        /// <summary>
        /// key????????????value??????wheel????????????
        /// </summary>
        [JsonProperty]
        private StorageDictionary<string,int> _AdWheelCount = new StorageDictionary<string,int> (StorageUserInfo_Change_AdWheelCount,true);
        [JsonIgnore]
        public StorageDictionary<string,int> AdWheelCount {
            get {
                return _AdWheelCount;
            }
        }

        /// <summary>
        /// ??????????????????daily Wheel
        /// </summary>
        [JsonProperty]
        private StorageDictionary<string,int> _DailyWheel = new StorageDictionary<string,int> (StorageUserInfo_Change_DailyWheel,true);
        [JsonIgnore]
        public StorageDictionary<string,int> DailyWheel {
            get {
                return _DailyWheel;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty]
        private StorageDictionary<string,int> _DailyAdWheelCount = new StorageDictionary<string,int> (StorageUserInfo_Change_DailyAdWheelCount,true);
        [JsonIgnore]
        public StorageDictionary<string,int> DailyAdWheelCount {
            get {
                return _DailyAdWheelCount;
            }
        }

        /// <summary>
        /// ?????????key????????????key2,???????????????value???????????????
        /// </summary>
        [JsonProperty]
        private StorageDictionary<string,Dictionary<int,int>> _DailyBI = new StorageDictionary<string,Dictionary<int,int>> (StorageUserInfo_Change_DailyBI,true);
        [JsonIgnore]
        public StorageDictionary<string,Dictionary<int,int>> DailyBI {
            get {
                return _DailyBI;
            }
        }

        /// <summary>
        /// ???????????????????????????????????????
        /// </summary>
        [JsonProperty]
        private StorageDictionary<string,bool> _DailyTargetRewardCollected = new StorageDictionary<string,bool> (StorageUserInfo_Change_DailyTargetRewardCollected,true);
        [JsonIgnore]
        public StorageDictionary<string,bool> DailyTargetRewardCollected {
            get {
                return _DailyTargetRewardCollected;
            }
        }

        /// <summary>
        /// ???????????????
        /// </summary>
        [JsonProperty]
        private StorageDictionary<int,int> _LevelScore = new StorageDictionary<int,int> (StorageUserInfo_Change_LevelScore,true);
        [JsonIgnore]
        public StorageDictionary<int,int> LevelScore {
            get {
                return _LevelScore;
            }
        }

        /// <summary>
        /// ??????????????????????????????????????????????????????
        /// </summary>
        [JsonProperty]
        private StorageDictionary<string,int> _DailyAdPlayCount = new StorageDictionary<string,int> (StorageUserInfo_Change_DailyAdPlayCount,true);
        [JsonIgnore]
        public StorageDictionary<string,int> DailyAdPlayCount {
            get {
                return _DailyAdPlayCount;
            }
        }

        /// <summary>
        /// key???partId???value???stepId
        /// </summary>
        [JsonProperty]
        private StorageDictionary<int,int> _task_part_progress = new StorageDictionary<int,int> (StorageUserInfo_Change_task_part_progress,true);
        [JsonIgnore]
        public StorageDictionary<int,int> task_part_progress {
            get {
                return _task_part_progress;
            }
        }

        /// <summary>
        /// ?????????key???roleId???list value,??????id
        /// </summary>
        [JsonProperty]
        private StorageDictionary<string,List<int>> _unlockArchives = new StorageDictionary<string,List<int>> (StorageUserInfo_Change_unlockArchives,true);
        [JsonIgnore]
        public StorageDictionary<string,List<int>> unlockArchives {
            get {
                return _unlockArchives;
            }
        }

        /// <summary>
        /// ??????????????????
        /// </summary>
        [JsonProperty]
        private StorageList<int> _collections = new StorageList<int> (StorageUserInfo_Change_collections,true);
        [JsonIgnore]
        public StorageList<int> collections {
            get {
                return _collections;
            }
        }

        /// <summary>
        /// key???tutoriaName???value???1,?????????0????????????
        /// </summary>
        [JsonProperty]
        private StorageDictionary<string,int> _TutorialSteps = new StorageDictionary<string,int> (StorageUserInfo_Change_TutorialSteps,true);
        [JsonIgnore]
        public StorageDictionary<string,int> TutorialSteps {
            get {
                return _TutorialSteps;
            }
        }


    }
}