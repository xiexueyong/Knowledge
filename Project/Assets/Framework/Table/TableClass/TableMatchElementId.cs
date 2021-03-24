using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Asset;
namespace Framework.Tables {
    public class TableMatchElementId
    {
        private static string FileName = "Table/MatchElementId";
        private static bool Inited = false;

        public TableMatchElementId()
		{
		}
        public void Clear()
        {
            Inited = false;
        }

        /// <summary>
        /// 出生点
        /// </summary>
        private int _Generator;
        public int Generator
        {
            private set
            {
                _Generator = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _Generator;
            }
        }
        /// <summary>
        /// 鸭子回收
        /// </summary>
        private int _IngredientHolder;
        public int IngredientHolder
        {
            private set
            {
                _IngredientHolder = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _IngredientHolder;
            }
        }
        /// <summary>
        /// 随机元素组
        /// </summary>
        private int _ElementGroup;
        public int ElementGroup
        {
            private set
            {
                _ElementGroup = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _ElementGroup;
            }
        }
        /// <summary>
        /// 色块
        /// </summary>
        private int _SimpleChip;
        public int SimpleChip
        {
            private set
            {
                _SimpleChip = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _SimpleChip;
            }
        }
        /// <summary>
        /// 箱子
        /// </summary>
        private int _Box;
        public int Box
        {
            private set
            {
                _Box = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _Box;
            }
        }
        /// <summary>
        /// 2*2鸭子
        /// </summary>
        private int _BigIngredient;
        public int BigIngredient
        {
            private set
            {
                _BigIngredient = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _BigIngredient;
            }
        }
        /// <summary>
        /// 鸭子
        /// </summary>
        private int _Ingredient;
        public int Ingredient
        {
            private set
            {
                _Ingredient = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _Ingredient;
            }
        }
        /// <summary>
        /// 气球
        /// </summary>
        private int _Balloon;
        public int Balloon
        {
            private set
            {
                _Balloon = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _Balloon;
            }
        }
        /// <summary>
        /// 可乐
        /// </summary>
        private int _TriggerChip;
        public int TriggerChip
        {
            private set
            {
                _TriggerChip = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _TriggerChip;
            }
        }
        /// <summary>
        /// 火箭
        /// </summary>
        private int _HorVGrayBomb;
        public int HorVGrayBomb
        {
            private set
            {
                _HorVGrayBomb = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _HorVGrayBomb;
            }
        }
        /// <summary>
        /// 炸弹
        /// </summary>
        private int _GrayBomb;
        public int GrayBomb
        {
            private set
            {
                _GrayBomb = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _GrayBomb;
            }
        }
        /// <summary>
        /// 彩虹球
        /// </summary>
        private int _DiscoBomb;
        public int DiscoBomb
        {
            private set
            {
                _DiscoBomb = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _DiscoBomb;
            }
        }
        /// <summary>
        /// 粘液
        /// </summary>
        private int _Slime;
        public int Slime
        {
            private set
            {
                _Slime = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _Slime;
            }
        }
        /// <summary>
        /// 锁链
        /// </summary>
        private int _IronCage;
        public int IronCage
        {
            private set
            {
                _IronCage = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _IronCage;
            }
        }
        /// <summary>
        /// 泡泡
        /// </summary>
        private int _Bubble;
        public int Bubble
        {
            private set
            {
                _Bubble = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _Bubble;
            }
        }
        /// <summary>
        /// 彩色气球
        /// </summary>
        private int _ColorBalloon;
        public int ColorBalloon
        {
            private set
            {
                _ColorBalloon = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _ColorBalloon;
            }
        }
        /// <summary>
        /// 彩虹马
        /// </summary>
        private int _RainbowHorseChip;
        public int RainbowHorseChip
        {
            private set
            {
                _RainbowHorseChip = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _RainbowHorseChip;
            }
        }
        /// <summary>
        /// 彩虹羊
        /// </summary>
        private int _RainbowSheep;
        public int RainbowSheep
        {
            private set
            {
                _RainbowSheep = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _RainbowSheep;
            }
        }
        /// <summary>
        /// 铁箱子
        /// </summary>
        private int _IronBox;
        public int IronBox
        {
            private set
            {
                _IronBox = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _IronBox;
            }
        }
        /// <summary>
        /// 彩色箱子
        /// </summary>
        private int _ColorBox;
        public int ColorBox
        {
            private set
            {
                _ColorBox = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _ColorBox;
            }
        }
        /// <summary>
        /// 易拉罐塔
        /// </summary>
        private int _CanTower;
        public int CanTower
        {
            private set
            {
                _CanTower = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _CanTower;
            }
        }
        /// <summary>
        /// 剥皮鸡蛋
        /// </summary>
        private int _Egg;
        public int Egg
        {
            private set
            {
                _Egg = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _Egg;
            }
        }
        /// <summary>
        /// 贝壳
        /// </summary>
        private int _Shell;
        public int Shell
        {
            private set
            {
                _Shell = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _Shell;
            }
        }
        /// <summary>
        /// 汽水瓶盒子
        /// </summary>
        private int _SodaBottle;
        public int SodaBottle
        {
            private set
            {
                _SodaBottle = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _SodaBottle;
            }
        }
        /// <summary>
        /// 西瓜
        /// </summary>
        private int _Watermelon;
        public int Watermelon
        {
            private set
            {
                _Watermelon = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _Watermelon;
            }
        }
        /// <summary>
        /// 灯泡
        /// </summary>
        private int _Bulb;
        public int Bulb
        {
            private set
            {
                _Bulb = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _Bulb;
            }
        }
        /// <summary>
        /// 泡泡鱼
        /// </summary>
        private int _BubbleFishTriggerChip;
        public int BubbleFishTriggerChip
        {
            private set
            {
                _BubbleFishTriggerChip = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _BubbleFishTriggerChip;
            }
        }
        /// <summary>
        /// 蜂蜜罐
        /// </summary>
        private int _HoneyPot;
        public int HoneyPot
        {
            private set
            {
                _HoneyPot = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _HoneyPot;
            }
        }
        /// <summary>
        /// 蜂蜜
        /// </summary>
        private int _Honey;
        public int Honey
        {
            private set
            {
                _Honey = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _Honey;
            }
        }
        /// <summary>
        /// 珍珠
        /// </summary>
        private int _PearlChip;
        public int PearlChip
        {
            private set
            {
                _PearlChip = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _PearlChip;
            }
        }
        /// <summary>
        /// 灯条
        /// </summary>
        private int _LightGroup;
        public int LightGroup
        {
            private set
            {
                _LightGroup = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _LightGroup;
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
				Generator = itemData["Generator"];
				IngredientHolder = itemData["IngredientHolder"];
				ElementGroup = itemData["ElementGroup"];
				SimpleChip = itemData["SimpleChip"];
				Box = itemData["Box"];
				BigIngredient = itemData["BigIngredient"];
				Ingredient = itemData["Ingredient"];
				Balloon = itemData["Balloon"];
				TriggerChip = itemData["TriggerChip"];
				HorVGrayBomb = itemData["HorVGrayBomb"];
				GrayBomb = itemData["GrayBomb"];
				DiscoBomb = itemData["DiscoBomb"];
				Slime = itemData["Slime"];
				IronCage = itemData["IronCage"];
				Bubble = itemData["Bubble"];
				ColorBalloon = itemData["ColorBalloon"];
				RainbowHorseChip = itemData["RainbowHorseChip"];
				RainbowSheep = itemData["RainbowSheep"];
				IronBox = itemData["IronBox"];
				ColorBox = itemData["ColorBox"];
				CanTower = itemData["CanTower"];
				Egg = itemData["Egg"];
				Shell = itemData["Shell"];
				SodaBottle = itemData["SodaBottle"];
				Watermelon = itemData["Watermelon"];
				Bulb = itemData["Bulb"];
				BubbleFishTriggerChip = itemData["BubbleFishTriggerChip"];
				HoneyPot = itemData["HoneyPot"];
				Honey = itemData["Honey"];
				PearlChip = itemData["PearlChip"];
				LightGroup = itemData["LightGroup"];

                Inited = true;
            }
        }

        public void Init (string tableStr) {


                JSONNode itemData = JSONNode.Parse (tableStr);
				Generator = itemData["Generator"];
				IngredientHolder = itemData["IngredientHolder"];
				ElementGroup = itemData["ElementGroup"];
				SimpleChip = itemData["SimpleChip"];
				Box = itemData["Box"];
				BigIngredient = itemData["BigIngredient"];
				Ingredient = itemData["Ingredient"];
				Balloon = itemData["Balloon"];
				TriggerChip = itemData["TriggerChip"];
				HorVGrayBomb = itemData["HorVGrayBomb"];
				GrayBomb = itemData["GrayBomb"];
				DiscoBomb = itemData["DiscoBomb"];
				Slime = itemData["Slime"];
				IronCage = itemData["IronCage"];
				Bubble = itemData["Bubble"];
				ColorBalloon = itemData["ColorBalloon"];
				RainbowHorseChip = itemData["RainbowHorseChip"];
				RainbowSheep = itemData["RainbowSheep"];
				IronBox = itemData["IronBox"];
				ColorBox = itemData["ColorBox"];
				CanTower = itemData["CanTower"];
				Egg = itemData["Egg"];
				Shell = itemData["Shell"];
				SodaBottle = itemData["SodaBottle"];
				Watermelon = itemData["Watermelon"];
				Bulb = itemData["Bulb"];
				BubbleFishTriggerChip = itemData["BubbleFishTriggerChip"];
				HoneyPot = itemData["HoneyPot"];
				Honey = itemData["Honey"];
				PearlChip = itemData["PearlChip"];
				LightGroup = itemData["LightGroup"];

                Inited = true;
        }


    }
}