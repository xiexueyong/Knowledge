using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using Framework.Asset;
namespace Framework.Tables
{
    public class TableCommonLevel {
        private static int curIndex = 0;
        private static string FileName = "Table/CommonLevel";
        private static Dictionary<int, TableCommonLevel> dic = new Dictionary<int, TableCommonLevel> ();
        private static List<TableCommonLevel> Items = new List<TableCommonLevel>();
        private static TableCommonLevel First;
        private static bool Inited = false;
        public TableCommonLevel Next = null;

		//关卡Id
		public int Id { private set; get; }
		//关卡Prefab
		public string LevelPrefab { private set; get; }
		//目标数量
		public int TargetCount { private set; get; }
		//时间，单位：秒
		public int Time { private set; get; }
		//金币奖励
		public int CoinReward { private set; get; }
		//奖励道具及数量
		public List<Triangle<int,int,int>> GiftGoods { private set; get; }


        private static void AddItem (TableCommonLevel item) {
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
                TableCommonLevel preItem = null;
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode data = JSONNode.Parse (tableStr);
                foreach (var itemData in data.Children) {
                    TableCommonLevel item = new TableCommonLevel ();
					item.Id = itemData["Id"];
					item.LevelPrefab = itemData["LevelPrefab"];
					item.TargetCount = itemData["TargetCount"];
					item.Time = itemData["Time"];
					item.CoinReward = itemData["CoinReward"];
					item.GiftGoods = Table.string2List_Triangle_int_int_int(itemData["GiftGoods"]);


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
              
            TableCommonLevel preItem = null;
            JSONNode data = JSONNode.Parse (tableStr);
            foreach (var itemData in data.Children) {
                TableCommonLevel item = new TableCommonLevel ();
					item.Id = itemData["Id"];
					item.LevelPrefab = itemData["LevelPrefab"];
					item.TargetCount = itemData["TargetCount"];
					item.Time = itemData["Time"];
					item.CoinReward = itemData["CoinReward"];
					item.GiftGoods = Table.string2List_Triangle_int_int_int(itemData["GiftGoods"]);


                AddItem (item);
                if (preItem != null) {
                    preItem.Next = item;
                }
                preItem = item;

            }
        }
        public TableCommonLevel Head () {
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
        public TableCommonLevel Get (int _id) {
            if (!Inited) {
                Init ();
            }
             try
            {
                TableCommonLevel config;
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
        public List<TableCommonLevel> GetAll()
        {
            if (!Inited)
            {
                Init();
            }
            return Items;
        }
        public List<TableCommonLevel> FindAll(Predicate<TableCommonLevel> match)
        {
            if (!Inited)
            {
                Init();
            }
            return Items.FindAll(match);
        }
    }
}