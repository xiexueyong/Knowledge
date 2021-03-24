using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using Framework.Asset;
namespace Framework.Tables
{
    public class TableLoginReward {
        private static int curIndex = 0;
        private static string FileName = "Table/LoginReward";
        private static Dictionary<int, TableLoginReward> dic = new Dictionary<int, TableLoginReward> ();
        private static List<TableLoginReward> Items = new List<TableLoginReward>();
        private static TableLoginReward First;
        private static bool Inited = false;
        public TableLoginReward Next = null;

		//编号
		public int Id { private set; get; }
		//第几天
		public int Level { private set; get; }
		//奖励道具及数量
		public Dictionary<int,int> Rewards { private set; get; }
		//礼包Icon
		public string GiftIcon { private set; get; }


        private static void AddItem (TableLoginReward item) {
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
                TableLoginReward preItem = null;
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode data = JSONNode.Parse (tableStr);
                foreach (var itemData in data.Children) {
                    TableLoginReward item = new TableLoginReward ();
					item.Id = itemData["Id"];
					item.Level = itemData["Level"];
					item.Rewards = Table.string2Dic_int_int(itemData["Rewards"]);
					item.GiftIcon = itemData["GiftIcon"];


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
              
            TableLoginReward preItem = null;
            JSONNode data = JSONNode.Parse (tableStr);
            foreach (var itemData in data.Children) {
                TableLoginReward item = new TableLoginReward ();
					item.Id = itemData["Id"];
					item.Level = itemData["Level"];
					item.Rewards = Table.string2Dic_int_int(itemData["Rewards"]);
					item.GiftIcon = itemData["GiftIcon"];


                AddItem (item);
                if (preItem != null) {
                    preItem.Next = item;
                }
                preItem = item;

            }
        }
        public TableLoginReward Head () {
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
        public TableLoginReward Get (int _id) {
            if (!Inited) {
                Init ();
            }
             try
            {
                TableLoginReward config;
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
        public List<TableLoginReward> GetAll()
        {
            if (!Inited)
            {
                Init();
            }
            return Items;
        }
        public List<TableLoginReward> FindAll(Predicate<TableLoginReward> match)
        {
            if (!Inited)
            {
                Init();
            }
            return Items.FindAll(match);
        }
    }
}