using EventUtil;
using Framework.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework.Storage
{
    [Serializable]
    public class StorageHashSet<TValue> : HashSet<TValue>
    {
        private string _eventName;
        public StorageHashSet(string eventName, bool syncForce = false)
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
            DebugUtil.Log("StorageHashSet,Add:"+item);
            EventDispatcher.TriggerEvent<TValue>(_eventName, item);
            OnStorageChanged();
            
        }

  

        public new void Clear()
        {
           // Debug.LogError("You are deleting some of the contents of the storage, please double check if you really want to do this");
            base.Clear();
            DebugUtil.Log("StorageHashSet,Clear");
            OnStorageChanged();
        }

        public new bool Remove(TValue item)
        {
           // Debug.LogError("You are deleting some of the contents of the storage, please double check if you really want to do this");
            OnStorageChanged();
            bool result = base.Remove(item);
            if (result)
            {
                DebugUtil.Log("StorageHashSet,Remove:" + item);
                EventDispatcher.TriggerEvent<TValue>(_eventName, item);
            }
                
            return result;
        }
    
    }
}
