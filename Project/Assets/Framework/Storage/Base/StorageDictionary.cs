using EventUtil;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Storage
{
    [Serializable]
    public class StorageDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        private string _eventName;
        public StorageDictionary(string eventName,bool syncForce = false)
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
        public void StorageChanged()
        {
            OnStorageChanged();
        }
        public new void Add(TKey key, TValue value)
        {
            base.Add(key, value);
            EventDispatcher.TriggerEvent<TKey,TValue>(_eventName, key,value);
            OnStorageChanged();
        }

        public new void Clear()
        {
            Debug.LogWarning("You are deleting some of the contents of the storage, please double check if you really want to do this");
            base.Clear();
            OnStorageChanged();
        }

        public new bool Remove(TKey key)
        {
            Debug.LogWarning("You are deleting some of the contents of the storage, please double check if you really want to do this");
            OnStorageChanged();
            bool result = base.Remove(key);
            if(result)
                EventDispatcher.TriggerEvent<TKey>(_eventName, key);
            return result;
        }

        public new TValue this[TKey key]
        {
            get
            {
                if (!base.ContainsKey(key))
                    return default(TValue);
                else
                    return base[key];
            }
            set
            {
                if (!base.ContainsKey(key))
                {
                    TValue oldValue = default(TValue);
                    base.Add(key,value);
                    OnStorageChanged();
                    EventDispatcher.TriggerEvent<TKey, TValue, TValue>(_eventName, key,oldValue, value);
                    return;
                }
                Debug.Assert(value != null);
                if (!value.Equals(base[key]))
                {
                    TValue oldValue = base[key];
                    base[key] = value;
                    OnStorageChanged();
                    EventDispatcher.TriggerEvent<TKey, TValue, TValue>(_eventName, key, oldValue, value);
                }
            }
        }
    }
}