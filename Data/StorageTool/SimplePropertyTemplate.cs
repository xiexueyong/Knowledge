
        /// <summary>
        /// #description
        /// </summary>
        [JsonIgnore]
        private bool _#propertyName_immediately = #immediateValue;
        [JsonProperty]
        private #propertyType _#propertyName;
        [JsonIgnore]
        public #propertyType #propertyName {
            get {
                return _#propertyName;
            }
            set {
                if (_#propertyName != value) {
                    #propertyType oldValue = _#propertyName;
                    _#propertyName = value;
                    EventDispatcher.TriggerEvent<#propertyType, #propertyType>(#eventName, oldValue, value);
                    if(_#propertyName_immediately)
                        StorageManager.Inst.LocalVersion++;

                }
            }
        }
