        /// <summary>
        /// #des
        /// </summary>
        private #type _#property;
        public #type #property
        {
            private set
            {
                _#property = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _#property;
            }
        }
