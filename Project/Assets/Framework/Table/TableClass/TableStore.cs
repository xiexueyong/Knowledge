using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using Framework.Asset;
namespace Framework.Tables
{
    public class TableStore {
        private static int curIndex = 0;
        private static string FileName = "Table/Store";
        private static Dictionary<int, TableStore> dic = new Dictionary<int, TableStore> ();
        private static List<TableStore> Items = new List<TableStore>();
        private static TableStore First;
        private static bool Inited = false;
        public TableStore Next = null;

		//物品ID
		public int Id { private set; get; }
		//物品名称
		public string StoreId { private set; get; }
		//物品名称
		public string Name { private set; get; }
		//物品图标
		public string Icon { private set; get; }
		//礼包出现等级
		public int ShowLevel { private set; get; }
		//金币
		public int Coin { private set; get; }
		//商品内容
		public Dictionary<int,int> Content { private set; get; }
		//去广告
		public int NoAd { private set; get; }
		//价格（美分）
		public int Price { private set; get; }
		//排序
		public int Rank { private set; get; }


        private static void AddItem (TableStore item) {
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
                TableStore preItem = null;
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode data = JSONNode.Parse (tableStr);
                foreach (var itemData in data.Children) {
                    TableStore item = new TableStore ();
					item.Id = itemData["Id"];
					item.StoreId = itemData["StoreId"];
					item.Name = itemData["Name"];
					item.Icon = itemData["Icon"];
					item.ShowLevel = itemData["ShowLevel"];
					item.Coin = itemData["Coin"];
					item.Content = Table.string2Dic_int_int(itemData["Content"]);
					item.NoAd = itemData["NoAd"];
					item.Price = itemData["Price"];
					item.Rank = itemData["Rank"];


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
              
            TableStore preItem = null;
            JSONNode data = JSONNode.Parse (tableStr);
            foreach (var itemData in data.Children) {
                TableStore item = new TableStore ();
					item.Id = itemData["Id"];
					item.StoreId = itemData["StoreId"];
					item.Name = itemData["Name"];
					item.Icon = itemData["Icon"];
					item.ShowLevel = itemData["ShowLevel"];
					item.Coin = itemData["Coin"];
					item.Content = Table.string2Dic_int_int(itemData["Content"]);
					item.NoAd = itemData["NoAd"];
					item.Price = itemData["Price"];
					item.Rank = itemData["Rank"];


                AddItem (item);
                if (preItem != null) {
                    preItem.Next = item;
                }
                preItem = item;

            }
        }
        public TableStore Head () {
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
        public TableStore Get (int _id) {
            if (!Inited) {
                Init ();
            }
             try
            {
                TableStore config;
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
        public List<TableStore> GetAll()
        {
            if (!Inited)
            {
                Init();
            }
            return Items;
        }
        public List<TableStore> FindAll(Predicate<TableStore> match)
        {
            if (!Inited)
            {
                Init();
            }
            return Items.FindAll(match);
        }
    }
}