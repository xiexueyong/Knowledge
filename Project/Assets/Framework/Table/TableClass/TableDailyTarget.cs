using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using Framework.Asset;
namespace Framework.Tables
{
    public class TableDailyTarget {
        private static int curIndex = 0;
        private static string FileName = "Table/DailyTarget";
        private static Dictionary<int, TableDailyTarget> dic = new Dictionary<int, TableDailyTarget> ();
        private static List<TableDailyTarget> Items = new List<TableDailyTarget>();
        private static TableDailyTarget First;
        private static bool Inited = false;
        public TableDailyTarget Next = null;

		//编号
		public int Id { private set; get; }
		//活动触发星期
		public int Index { private set; get; }
		//目标类型
		public string TargetType { private set; get; }
		//目标图标
		public string Icon { private set; get; }
		//目标数量
		public int GoalNum { private set; get; }
		//活动标题
		public string TitleTxtId { private set; get; }
		//活动描述
		public string descriptionTxtId { private set; get; }
		//奖励道具及数量
		public Dictionary<int,int> Rewards { private set; get; }


        private static void AddItem (TableDailyTarget item) {
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
                TableDailyTarget preItem = null;
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode data = JSONNode.Parse (tableStr);
                foreach (var itemData in data.Children) {
                    TableDailyTarget item = new TableDailyTarget ();
					item.Id = itemData["Id"];
					item.Index = itemData["Index"];
					item.TargetType = itemData["TargetType"];
					item.Icon = itemData["Icon"];
					item.GoalNum = itemData["GoalNum"];
					item.TitleTxtId = itemData["TitleTxtId"];
					item.descriptionTxtId = itemData["descriptionTxtId"];
					item.Rewards = Table.string2Dic_int_int(itemData["Rewards"]);


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
              
            TableDailyTarget preItem = null;
            JSONNode data = JSONNode.Parse (tableStr);
            foreach (var itemData in data.Children) {
                TableDailyTarget item = new TableDailyTarget ();
					item.Id = itemData["Id"];
					item.Index = itemData["Index"];
					item.TargetType = itemData["TargetType"];
					item.Icon = itemData["Icon"];
					item.GoalNum = itemData["GoalNum"];
					item.TitleTxtId = itemData["TitleTxtId"];
					item.descriptionTxtId = itemData["descriptionTxtId"];
					item.Rewards = Table.string2Dic_int_int(itemData["Rewards"]);


                AddItem (item);
                if (preItem != null) {
                    preItem.Next = item;
                }
                preItem = item;

            }
        }
        public TableDailyTarget Head () {
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
        public TableDailyTarget Get (int _id) {
            if (!Inited) {
                Init ();
            }
             try
            {
                TableDailyTarget config;
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
        public List<TableDailyTarget> GetAll()
        {
            if (!Inited)
            {
                Init();
            }
            return Items;
        }
        public List<TableDailyTarget> FindAll(Predicate<TableDailyTarget> match)
        {
            if (!Inited)
            {
                Init();
            }
            return Items.FindAll(match);
        }
    }
}