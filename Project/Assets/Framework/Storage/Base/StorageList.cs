using EventUtil;
using Framework.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Storage
{
    [Serializable]
    public class StorageList<TValue> : List<TValue>
    {
        private string _eventName;
        public StorageList(string eventName, bool syncForce = false)
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

        public new void Add(TValue item)
        {
            base.Add(item);
            OnStorageChanged();
            DebugUtil.Log("StorageList,Add:" + item);
            EventDispatcher.TriggerEvent<TValue>(_eventName, item);
        }

        public new void AddRange(IEnumerable<TValue> collection)
        {
            base.AddRange(collection);
            OnStorageChanged();
        }

        public new void Clear()
        {
            Debug.LogWarning("You are deleting some of the contents of the storage, please double check if you really want to do this");
            base.Clear();
            DebugUtil.Log("StorageList,Clear:");
            OnStorageChanged();
        }

        public new List<TOutput> ConvertAll<TOutput>(Converter<TValue, TOutput> converter)
        {
            return base.ConvertAll(converter);
        }

        public new void Insert(int index, TValue item)
        {
            base.Insert(index, item);
            OnStorageChanged();
        }

        public new void InsertRange(int index, IEnumerable<TValue> collection)
        {
            base.InsertRange(index, collection);
            OnStorageChanged();
        }

        public new bool Remove(TValue item)
        {
            Debug.LogWarning("You are deleting some of the contents of the storage, please double check if you really want to do this");
            OnStorageChanged();
            bool result = base.Remove(item);
            if (result)
            {
                DebugUtil.Log("StorageList,Remove:" + item);
                EventDispatcher.TriggerEvent<TValue>(_eventName, item);
            }
               
            return result;
        }

        public new int RemoveAll(Predicate<TValue> match)
        {
            Debug.LogWarning("You are deleting some of the contents of the storage, please double check if you really want to do this");
            OnStorageChanged();
            DebugUtil.Log("StorageList,RemoveAll");
            return base.RemoveAll(match);
        }

        public new void RemoveAt(int index)
        {
            Debug.LogWarning("You are deleting some of the contents of the storage, please double check if you really want to do this");
            base.RemoveAt(index);
            OnStorageChanged();
        }

        public new void RemoveRange(int index, int count)
        {
            Debug.LogWarning("You are deleting some of the contents of the storage, please double check if you really want to do this");
            base.RemoveRange(index, count);
            OnStorageChanged();
        }

        public new void Reverse()
        {
            Debug.LogWarning("You are changing some structures of the storage, please double check if you really want to do this");
            base.Reverse();
            OnStorageChanged();
        }

        public new void Sort(Comparison<TValue> comparison)
        {
            Debug.LogWarning("You are changing some structures of the storage, please double check if you really want to do this");
            base.Sort(comparison);
            OnStorageChanged();
        }

        public new TValue this[int index]
        {
            get
            {
                return base[index];
            }
            set
            {
                Debug.Assert(value != null);
                if (!value.Equals(base[index]))
                {
                    DebugUtil.Log("StorageList,[]:"+value.ToString());
                    base[index] = value;
                    OnStorageChanged();
                }
            }
        }
    }
}
