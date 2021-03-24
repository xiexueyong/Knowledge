using EventUtil;
using Framework.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Storage
{
    [Serializable]
    public class StoragePair<TKey, TValue>
    {
        private string _eventName;
        public StoragePair(string eventName, bool syncForce = false)
        {
            _eventName = eventName;
            SyncForce = syncForce;
        }
        bool SyncForce { get; set; }

        void OnStorageChanged()
        {
            StorageManager.Inst.LocalVersion++;
            if (SyncForce)
            {
                StorageManager.Inst.SyncForce = true;
            }
        }
        [JsonProperty]
        private TKey _key;
        [JsonIgnore]
        public TKey Key
        {
            get
            {
                return _key;
            }
            set
            {
                TKey oldKey = _key;
                _key = value;
                OnStorageChanged();
                EventDispatcher.TriggerEvent<TKey, TKey>(_eventName, oldKey, value);
            }
        }

        [JsonProperty]
        private TValue _value;
        [JsonIgnore]
        public TValue Value
        {
            get
            {
                return _value;
            }
            set
            {
                TValue oldValue = _value;
                _value = value;
                OnStorageChanged();
                EventDispatcher.TriggerEvent<TValue, TValue>(_eventName, oldValue, value);
            }
        }



    }
}
