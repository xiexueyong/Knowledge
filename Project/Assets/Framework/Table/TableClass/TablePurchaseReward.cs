using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using Framework.Asset;
namespace Framework.Tables
{
    public class TablePurchaseReward {
        private static int curIndex = 0;
        private static string FileName = "Table/PurchaseReward";
        private static Dictionary<int, TablePurchaseReward> dic = new Dictionary<int, TablePurchaseReward> ();
        private static List<TablePurchaseReward> Items = new List<TablePurchaseReward>();
        private static TablePurchaseReward First;
        private static bool Inited = false;
        public TablePurchaseReward Next = null;

		//编号
		public int Id { private set; get; }
		//累充所需金币数量
		public int Coin { private set; get; }
		//奖励道具及数量
		public Dictionary<int,int> Rewards { private set; get; }
		//宣传金币数量
		public int CoinAdv { private set; get; }


        private static void AddItem (TablePurchaseReward item) {
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
                TablePurchaseReward preItem = null;
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode data = JSONNode.Parse (tableStr);
                foreach (var itemData in data.Children) {
                    TablePurchaseReward item = new TablePurchaseReward ();
					item.Id = itemData["Id"];
					item.Coin = itemData["Coin"];
					item.Rewards = Table.string2Dic_int_int(itemData["Rewards"]);
					item.CoinAdv = itemData["CoinAdv"];


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
              
            TablePurchaseReward preItem = null;
            JSONNode data = JSONNode.Parse (tableStr);
            foreach (var itemData in data.Children) {
                TablePurchaseReward item = new TablePurchaseReward ();
					item.Id = itemData["Id"];
					item.Coin = itemData["Coin"];
					item.Rewards = Table.string2Dic_int_int(itemData["Rewards"]);
					item.CoinAdv = itemData["CoinAdv"];


                AddItem (item);
                if (preItem != null) {
                    preItem.Next = item;
                }
                preItem = item;

            }
        }
        public TablePurchaseReward Head () {
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
        public TablePurchaseReward Get (int _id) {
            if (!Inited) {
                Init ();
            }
             try
            {
                TablePurchaseReward config;
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
        public List<TablePurchaseReward> GetAll()
        {
            if (!Inited)
            {
                Init();
            }
            return Items;
        }
        public List<TablePurchaseReward> FindAll(Predicate<TablePurchaseReward> match)
        {
            if (!Inited)
            {
                Init();
            }
            return Items.FindAll(match);
        }
    }
}