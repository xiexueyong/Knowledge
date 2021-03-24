using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Asset;
namespace Framework.Tables {
    public class TableGoodsId
    {
        private static string FileName = "Table/GoodsId";
        private static bool Inited = false;

        public TableGoodsId()
		{
		}
        public void Clear()
        {
            Inited = false;
        }

        /// <summary>
        /// 金币
        /// </summary>
        private int _Coin;
        public int Coin
        {
            private set
            {
                _Coin = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _Coin;
            }
        }
        /// <summary>
        /// 纸币
        /// </summary>
        private int _Cash;
        public int Cash
        {
            private set
            {
                _Cash = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _Cash;
            }
        }
        /// <summary>
        /// 能量
        /// </summary>
        private int _Energy;
        public int Energy
        {
            private set
            {
                _Energy = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _Energy;
            }
        }
        /// <summary>
        /// 星
        /// </summary>
        private int _Star;
        public int Star
        {
            private set
            {
                _Star = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _Star;
            }
        }
        /// <summary>
        /// 无限体力，一个代表1小时
        /// </summary>
        private int _InfiniteEnergy1H;
        public int InfiniteEnergy1H
        {
            private set
            {
                _InfiniteEnergy1H = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _InfiniteEnergy1H;
            }
        }
        /// <summary>
        /// 无限体力，一个代表6小时
        /// </summary>
        private int _InfiniteEnergy6H;
        public int InfiniteEnergy6H
        {
            private set
            {
                _InfiniteEnergy6H = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _InfiniteEnergy6H;
            }
        }
        /// <summary>
        /// 无限体力，一个代表12小时
        /// </summary>
        private int _InfiniteEnergy12H;
        public int InfiniteEnergy12H
        {
            private set
            {
                _InfiniteEnergy12H = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _InfiniteEnergy12H;
            }
        }
        /// <summary>
        /// 无限体力，一个代表24小时
        /// </summary>
        private int _InfiniteEnergy24H;
        public int InfiniteEnergy24H
        {
            private set
            {
                _InfiniteEnergy24H = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _InfiniteEnergy24H;
            }
        }
        /// <summary>
        /// 钥匙
        /// </summary>
        private int _Key;
        public int Key
        {
            private set
            {
                _Key = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _Key;
            }
        }
        /// <summary>
        /// 火箭
        /// </summary>
        private int _Rocket;
        public int Rocket
        {
            private set
            {
                _Rocket = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _Rocket;
            }
        }
        /// <summary>
        /// 炸弹
        /// </summary>
        private int _Bomb;
        public int Bomb
        {
            private set
            {
                _Bomb = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _Bomb;
            }
        }
        /// <summary>
        /// 彩虹球
        /// </summary>
        private int _RainbowBall;
        public int RainbowBall
        {
            private set
            {
                _RainbowBall = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _RainbowBall;
            }
        }
        /// <summary>
        /// Removess any cube or obstacle!，锤子
        /// </summary>
        private int _Hammer;
        public int Hammer
        {
            private set
            {
                _Hammer = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _Hammer;
            }
        }
        /// <summary>
        /// Removess a line of cubes!，割草机
        /// </summary>
        private int _Cropper;
        public int Cropper
        {
            private set
            {
                _Cropper = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _Cropper;
            }
        }
        /// <summary>
        /// Removess a column of cubes!，称砣
        /// </summary>
        private int _Weight;
        public int Weight
        {
            private set
            {
                _Weight = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _Weight;
            }
        }
        /// <summary>
        /// Shuffles all of the cubes!，炸弹
        /// </summary>
        private int _TNT;
        public int TNT
        {
            private set
            {
                _TNT = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _TNT;
            }
        }
        /// <summary>
        /// 打乱顺序，龙卷风
        /// </summary>
        private int _Tornado;
        public int Tornado
        {
            private set
            {
                _Tornado = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _Tornado;
            }
        }
        /// <summary>
        /// 加五步
        /// </summary>
        private int _Add5Moves;
        public int Add5Moves
        {
            private set
            {
                _Add5Moves = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _Add5Moves;
            }
        }


        public void Init () {
            if (!Table.Inited)
            {
                throw new Exception("Tabele has not been initialised,it is intialised in GameController");
            }
            if (!Inited) {
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode itemData = JSONNode.Parse (tableStr);
				Coin = itemData["Coin"];
				Cash = itemData["Cash"];
				Energy = itemData["Energy"];
				Star = itemData["Star"];
				InfiniteEnergy1H = itemData["InfiniteEnergy1H"];
				InfiniteEnergy6H = itemData["InfiniteEnergy6H"];
				InfiniteEnergy12H = itemData["InfiniteEnergy12H"];
				InfiniteEnergy24H = itemData["InfiniteEnergy24H"];
				Key = itemData["Key"];
				Rocket = itemData["Rocket"];
				Bomb = itemData["Bomb"];
				RainbowBall = itemData["RainbowBall"];
				Hammer = itemData["Hammer"];
				Cropper = itemData["Cropper"];
				Weight = itemData["Weight"];
				TNT = itemData["TNT"];
				Tornado = itemData["Tornado"];
				Add5Moves = itemData["Add5Moves"];

                Inited = true;
            }
        }

        public void Init (string tableStr) {


                JSONNode itemData = JSONNode.Parse (tableStr);
				Coin = itemData["Coin"];
				Cash = itemData["Cash"];
				Energy = itemData["Energy"];
				Star = itemData["Star"];
				InfiniteEnergy1H = itemData["InfiniteEnergy1H"];
				InfiniteEnergy6H = itemData["InfiniteEnergy6H"];
				InfiniteEnergy12H = itemData["InfiniteEnergy12H"];
				InfiniteEnergy24H = itemData["InfiniteEnergy24H"];
				Key = itemData["Key"];
				Rocket = itemData["Rocket"];
				Bomb = itemData["Bomb"];
				RainbowBall = itemData["RainbowBall"];
				Hammer = itemData["Hammer"];
				Cropper = itemData["Cropper"];
				Weight = itemData["Weight"];
				TNT = itemData["TNT"];
				Tornado = itemData["Tornado"];
				Add5Moves = itemData["Add5Moves"];

                Inited = true;
        }


    }
}