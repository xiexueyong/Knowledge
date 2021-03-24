using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using Framework.Asset;
namespace Framework.Tables
{
    public class TableStarReward {
        private static int curIndex = 0;
        private static string FileName = "Table/StarReward";
        private static Dictionary<int, TableStarReward> dic = new Dictionary<int, TableStarReward> ();
        private static List<TableStarReward> Items = new List<TableStarReward>();
        private static TableStarReward First;
        private static bool Inited = false;
        public TableStarReward Next = null;

		//礼包Id
		public int Id { private set; get; }
		//等级范围
		public int LevelBottom { private set; get; }
		//等级范围
		public int LevelTop { private set; get; }
		//奖励道具及数量
		public Dictionary<int,int> Rewards { private set; get; }


        private static void AddItem (TableStarReward item) {
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
                TableStarReward preItem = null;
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode data = JSONNode.Parse (tableStr);
                foreach (var itemData in data.Children) {
                    TableStarReward item = new TableStarReward ();
					item.Id = itemData["Id"];
					item.LevelBottom = itemData["LevelBottom"];
					item.LevelTop = itemData["LevelTop"];
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
              
            TableStarReward preItem = null;
            JSONNode data = JSONNode.Parse (tableStr);
            foreach (var itemData in data.Children) {
                TableStarReward item = new TableStarReward ();
					item.Id = itemData["Id"];
					item.LevelBottom = itemData["LevelBottom"];
					item.LevelTop = itemData["LevelTop"];
					item.Rewards = Table.string2Dic_int_int(itemData["Rewards"]);


                AddItem (item);
                if (preItem != null) {
                    preItem.Next = item;
                }
                preItem = item;

            }
        }
        public TableStarReward Head () {
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
        public TableStarReward Get (int _id) {
            if (!Inited) {
                Init ();
            }
             try
            {
                TableStarReward config;
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
        public List<TableStarReward> GetAll()
        {
            if (!Inited)
            {
                Init();
            }
            return Items;
        }
        public List<TableStarReward> FindAll(Predicate<TableStarReward> match)
        {
            if (!Inited)
            {
                Init();
            }
            return Items.FindAll(match);
        }
    }
}