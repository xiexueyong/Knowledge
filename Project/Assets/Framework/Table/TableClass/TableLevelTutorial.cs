using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using Framework.Asset;
namespace Framework.Tables
{
    public class TableLevelTutorial {
        private static int curIndex = 0;
        private static string FileName = "Table/LevelTutorial";
        private static Dictionary<int, TableLevelTutorial> dic = new Dictionary<int, TableLevelTutorial> ();
        private static List<TableLevelTutorial> Items = new List<TableLevelTutorial>();
        private static TableLevelTutorial First;
        private static bool Inited = false;
        public TableLevelTutorial Next = null;

		//表Id
		public int Id { private set; get; }
		//关卡Id
		public int Level { private set; get; }
		//引导的信息
		public string Msg { private set; get; }
		//引导逻辑
		public string TutorialLogic { private set; get; }
		//触发顺序
		public int index { private set; get; }
		//高亮格子
		public List<Triangle<int,int,int>> HLCells { private set; get; }
		//点击格子
		public List<Triangle<int,int,int>> ClickCells { private set; get; }
		//引导文本
		public string Text { private set; get; }
		//文本距中心点Y偏移量
		public int TextYOffset { private set; get; }
		//是否有继续按钮
		public bool HasContiueBtn { private set; get; }
		//高亮目标
		public bool HLGoals { private set; get; }
		//高亮宠物
		public bool HLPet { private set; get; }
		//箭头指向目标
		public bool ArrowAtGoals { private set; get; }
		//提示收集物下落
		public bool TipDrop { private set; get; }
		//高亮Booster
		public int HLBooster { private set; get; }
		//箭头指向Booster
		public int ArrowAtBooster { private set; get; }


        private static void AddItem (TableLevelTutorial item) {
            if (First == null) {
                First = item;
            }
            if (dic.ContainsKey(item.Id))
                DebugUtil.LogError(FileName+" id duplicate :"+item.Id);
            else
            {
                dic.Add(item.Id, item);
                Items.Add(item);
            }
        }
        public void Clear()
        {
            Inited = false;
            First = null;
            dic.Clear();
            Items.Clear();
        }
        public void Init () {
            if (!Table.Inited)
            {
                throw new Exception("Tabele has not been initialised,it is intialised in GameController");
            }
            if (!Inited) {
                TableLevelTutorial preItem = null;
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode data = JSONNode.Parse (tableStr);
                foreach (var itemData in data.Children) {
                    TableLevelTutorial item = new TableLevelTutorial ();
					item.Id = itemData["Id"];
					item.Level = itemData["Level"];
					item.Msg = itemData["Msg"];
					item.TutorialLogic = itemData["TutorialLogic"];
					item.index = itemData["index"];
					item.HLCells = Table.string2List_Triangle_int_int_int(itemData["HLCells"]);
					item.ClickCells = Table.string2List_Triangle_int_int_int(itemData["ClickCells"]);
					item.Text = itemData["Text"];
					item.TextYOffset = itemData["TextYOffset"];
					item.HasContiueBtn = Table.string2Bool(itemData["HasContiueBtn"]);
					item.HLGoals = Table.string2Bool(itemData["HLGoals"]);
					item.HLPet = Table.string2Bool(itemData["HLPet"]);
					item.ArrowAtGoals = Table.string2Bool(itemData["ArrowAtGoals"]);
					item.TipDrop = Table.string2Bool(itemData["TipDrop"]);
					item.HLBooster = itemData["HLBooster"];
					item.ArrowAtBooster = itemData["ArrowAtBooster"];


                    AddItem (item);
                    if (preItem != null) {
                        preItem.Next = item;
                    }
                    preItem = item;

                }
                Inited = true;
            }
        }
        public void Init (string tableStr) {
              
            TableLevelTutorial preItem = null;
            JSONNode data = JSONNode.Parse (tableStr);
            foreach (var itemData in data.Children) {
                TableLevelTutorial item = new TableLevelTutorial ();
					item.Id = itemData["Id"];
					item.Level = itemData["Level"];
					item.Msg = itemData["Msg"];
					item.TutorialLogic = itemData["TutorialLogic"];
					item.index = itemData["index"];
					item.HLCells = Table.string2List_Triangle_int_int_int(itemData["HLCells"]);
					item.ClickCells = Table.string2List_Triangle_int_int_int(itemData["ClickCells"]);
					item.Text = itemData["Text"];
					item.TextYOffset = itemData["TextYOffset"];
					item.HasContiueBtn = Table.string2Bool(itemData["HasContiueBtn"]);
					item.HLGoals = Table.string2Bool(itemData["HLGoals"]);
					item.HLPet = Table.string2Bool(itemData["HLPet"]);
					item.ArrowAtGoals = Table.string2Bool(itemData["ArrowAtGoals"]);
					item.TipDrop = Table.string2Bool(itemData["TipDrop"]);
					item.HLBooster = itemData["HLBooster"];
					item.ArrowAtBooster = itemData["ArrowAtBooster"];


                AddItem (item);
                if (preItem != null) {
                    preItem.Next = item;
                }
                preItem = item;

            }
        }
        public TableLevelTutorial Head () {
            if (!Inited) {
                Init ();
            }
            return First;
        }
        public int Count () {
            if (!Inited) {
                Init ();
            }
            return dic.Count;
        }
        public TableLevelTutorial Get (int _id) {
            if (!Inited) {
                Init ();
            }
             try
            {
                TableLevelTutorial config;
                if (dic.TryGetValue (_id, out config)) {
                    return config;
                } else {
                    return null;
                }
            }catch (Exception e)
            {
                DebugUtil.LogError(string.Format("{0} get value with key __{1}__,error:{2}",this.GetType().Name,_id,e.ToString()));
                return null;
            }
        }
        public List<TableLevelTutorial> GetAll()
        {
            if (!Inited)
            {
                Init();
            }
            return Items;
        }
        public List<TableLevelTutorial> FindAll(Predicate<TableLevelTutorial> match)
        {
            if (!Inited)
            {
                Init();
            }
            return Items.FindAll(match);
        }
    }
}