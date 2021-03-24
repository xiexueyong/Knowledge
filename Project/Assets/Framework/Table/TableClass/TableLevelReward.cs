using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using Framework.Asset;
namespace Framework.Tables
{
    public class TableLevelReward {
        private static int curIndex = 0;
        private static string FileName = "Table/LevelReward";
        private static Dictionary<int, TableLevelReward> dic = new Dictionary<int, TableLevelReward> ();
        private static List<TableLevelReward> Items = new List<TableLevelReward>();
        private static TableLevelReward First;
        private static bool Inited = false;
        public TableLevelReward Next = null;

		//礼包Id
		public int Id { private set; get; }
		//玩家等级
		public int Level { private set; get; }
		//奖励道具及数量
		public Dictionary<int,int> Rewards { private set; get; }


        private static void AddItem (TableLevelReward item) {
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
                TableLevelReward preItem = null;
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode data = JSONNode.Parse (tableStr);
                foreach (var itemData in data.Children) {
                    TableLevelReward item = new TableLevelReward ();
					item.Id = itemData["Id"];
					item.Level = itemData["Level"];
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
              
            TableLevelReward preItem = null;
            JSONNode data = JSONNode.Parse (tableStr);
            foreach (var itemData in data.Children) {
                TableLevelReward item = new TableLevelReward ();
					item.Id = itemData["Id"];
					item.Level = itemData["Level"];
					item.Rewards = Table.string2Dic_int_int(itemData["Rewards"]);


                AddItem (item);
                if (preItem != null) {
                    preItem.Next = item;
                }
                preItem = item;

            }
        }
        public TableLevelReward Head () {
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
        public TableLevelReward Get (int _id) {
            if (!Inited) {
                Init ();
            }
             try
            {
                TableLevelReward config;
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
        public List<TableLevelReward> GetAll()
        {
            if (!Inited)
            {
                Init();
            }
            return Items;
        }
        public List<TableLevelReward> FindAll(Predicate<TableLevelReward> match)
        {
            if (!Inited)
            {
                Init();
            }
            return Items.FindAll(match);
        }
    }
}