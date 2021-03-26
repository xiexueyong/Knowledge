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
		public static string StorageUserInfo_Change_HeadIconID = "StorageUserInfo_Change_HeadIconID";
		public static string StorageUserInfo_Change_NickName = "StorageUserInfo_Change_NickName";
		public static string StorageUserInfo_Change_Level = "StorageUserInfo_Change_Level";
		public static string StorageUserInfo_Change_GoodsCount = "StorageUserInfo_Change_GoodsCount";
		public static string StorageUserInfo_Change_AlreadyGuide = "StorageUserInfo_Change_AlreadyGuide";
		public static string StorageUserInfo_Change_Purchased = "StorageUserInfo_Change_Purchased";
		public static string StorageUserInfo_Change_CountDownTime = "StorageUserInfo_Change_CountDownTime";
		public static string StorageUserInfo_Change_InfiniteLifeStartTime = "StorageUserInfo_Change_InfiniteLifeStartTime";
		public static string StorageUserInfo_Change_InfiniteLifeTimeSpan = "StorageUserInfo_Change_InfiniteLifeTimeSpan";
		
        
        /// <summary>
        /// 设置默认信息
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
        /// 头像ID
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
        /// 用户昵称
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
        /// 当前关卡主线关卡
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
        /// 开始倒计时时间
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
        /// 无限生命开始的时间
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
        /// 无限生命的时长
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
        /// 物品数量（物品ID，物品数量）
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
        /// 引导记录
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
        /// 购买过的商品
        /// </summary>
        [JsonProperty]
        private StorageList<string> _Purchased = new StorageList<string> (StorageUserInfo_Change_Purchased,true);
        [JsonIgnore]
        public StorageList<string> Purchased {
            get {
                return _Purchased;
            }
        }


    }
}