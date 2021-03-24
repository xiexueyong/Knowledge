
        /// <summary>
        /// #description
        /// </summary>
        [JsonProperty]
        private #propertyType _#propertyName = new #propertyType (#eventName,true);
        [JsonIgnore]
        public #propertyType #propertyName {
            get {
                return _#propertyName;
            }
        }
