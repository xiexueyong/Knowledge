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
    public class StorageAccountInfo : StorageBase {

        public static string StorageAccountInfo_Change_Uid = "StorageAccountInfo_Change_Uid";
		public static string StorageAccountInfo_Change_FbId = "StorageAccountInfo_Change_FbId";
		public static string StorageAccountInfo_Change_DevId = "StorageAccountInfo_Change_DevId";
		public static string StorageAccountInfo_Change_AppVersion = "StorageAccountInfo_Change_AppVersion";
		public static string StorageAccountInfo_Change_StorageVersion = "StorageAccountInfo_Change_StorageVersion";
		public static string StorageAccountInfo_Change_StorageId = "StorageAccountInfo_Change_StorageId";
		public static string StorageAccountInfo_Change_OtherData = "StorageAccountInfo_Change_OtherData";
		public static string StorageAccountInfo_Change_InstallTime = "StorageAccountInfo_Change_InstallTime";
		public static string StorageAccountInfo_Change_OnlineDay = "StorageAccountInfo_Change_OnlineDay";
		public static string StorageAccountInfo_Change_adChannel = "StorageAccountInfo_Change_adChannel";
		
        
        /// <summary>
        /// 用户的Uid
        /// </summary>
        [JsonIgnore]
        private bool _Uid_immediately = false;
        [JsonProperty]
        private string _Uid;
        [JsonIgnore]
        public string Uid {
            get {
                return _Uid;
            }
            set {
                if (_Uid != value) {
                    string oldValue = _Uid;
                    _Uid = value;
                    EventDispatcher.TriggerEvent<string, string>(StorageAccountInfo_Change_Uid, oldValue, value);
                    if(_Uid_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// 用户的FbId
        /// </summary>
        [JsonIgnore]
        private bool _FbId_immediately = true;
        [JsonProperty]
        private string _FbId;
        [JsonIgnore]
        public string FbId {
            get {
                return _FbId;
            }
            set {
                if (_FbId != value) {
                    string oldValue = _FbId;
                    _FbId = value;
                    EventDispatcher.TriggerEvent<string, string>(StorageAccountInfo_Change_FbId, oldValue, value);
                    if(_FbId_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// 用户的设备号
        /// </summary>
        [JsonIgnore]
        private bool _DevId_immediately = false;
        [JsonProperty]
        private string _DevId;
        [JsonIgnore]
        public string DevId {
            get {
                return _DevId;
            }
            set {
                if (_DevId != value) {
                    string oldValue = _DevId;
                    _DevId = value;
                    EventDispatcher.TriggerEvent<string, string>(StorageAccountInfo_Change_DevId, oldValue, value);
                    if(_DevId_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// 应用的版本号
        /// </summary>
        [JsonIgnore]
        private bool _AppVersion_immediately = false;
        [JsonProperty]
        private string _AppVersion;
        [JsonIgnore]
        public string AppVersion {
            get {
                return _AppVersion;
            }
            set {
                if (_AppVersion != value) {
                    string oldValue = _AppVersion;
                    _AppVersion = value;
                    EventDispatcher.TriggerEvent<string, string>(StorageAccountInfo_Change_AppVersion, oldValue, value);
                    if(_AppVersion_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// 数据信息版本
        /// </summary>
        [JsonIgnore]
        private bool _StorageVersion_immediately = false;
        [JsonProperty]
        private int _StorageVersion;
        [JsonIgnore]
        public int StorageVersion {
            get {
                return _StorageVersion;
            }
            set {
                if (_StorageVersion != value) {
                    int oldValue = _StorageVersion;
                    _StorageVersion = value;
                    EventDispatcher.TriggerEvent<int, int>(StorageAccountInfo_Change_StorageVersion, oldValue, value);
                    if(_StorageVersion_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// 数据信息的Id
        /// </summary>
        [JsonIgnore]
        private bool _StorageId_immediately = false;
        [JsonProperty]
        private int _StorageId;
        [JsonIgnore]
        public int StorageId {
            get {
                return _StorageId;
            }
            set {
                if (_StorageId != value) {
                    int oldValue = _StorageId;
                    _StorageId = value;
                    EventDispatcher.TriggerEvent<int, int>(StorageAccountInfo_Change_StorageId, oldValue, value);
                    if(_StorageId_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// 用户的其他信息
        /// </summary>
        [JsonIgnore]
        private bool _OtherData_immediately = true;
        [JsonProperty]
        private string _OtherData;
        [JsonIgnore]
        public string OtherData {
            get {
                return _OtherData;
            }
            set {
                if (_OtherData != value) {
                    string oldValue = _OtherData;
                    _OtherData = value;
                    EventDispatcher.TriggerEvent<string, string>(StorageAccountInfo_Change_OtherData, oldValue, value);
                    if(_OtherData_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// 安装时间
        /// </summary>
        [JsonIgnore]
        private bool _InstallTime_immediately = true;
        [JsonProperty]
        private int _InstallTime;
        [JsonIgnore]
        public int InstallTime {
            get {
                return _InstallTime;
            }
            set {
                if (_InstallTime != value) {
                    int oldValue = _InstallTime;
                    _InstallTime = value;
                    EventDispatcher.TriggerEvent<int, int>(StorageAccountInfo_Change_InstallTime, oldValue, value);
                    if(_InstallTime_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// 在线天数
        /// </summary>
        [JsonIgnore]
        private bool _OnlineDay_immediately = true;
        [JsonProperty]
        private int _OnlineDay;
        [JsonIgnore]
        public int OnlineDay {
            get {
                return _OnlineDay;
            }
            set {
                if (_OnlineDay != value) {
                    int oldValue = _OnlineDay;
                    _OnlineDay = value;
                    EventDispatcher.TriggerEvent<int, int>(StorageAccountInfo_Change_OnlineDay, oldValue, value);
                    if(_OnlineDay_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        /// <summary>
        /// 用户来源
        /// </summary>
        [JsonIgnore]
        private bool _adChannel_immediately = true;
        [JsonProperty]
        private string _adChannel;
        [JsonIgnore]
        public string adChannel {
            get {
                return _adChannel;
            }
            set {
                if (_adChannel != value) {
                    string oldValue = _adChannel;
                    _adChannel = value;
                    EventDispatcher.TriggerEvent<string, string>(StorageAccountInfo_Change_adChannel, oldValue, value);
                    if(_adChannel_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }

        

    }
}