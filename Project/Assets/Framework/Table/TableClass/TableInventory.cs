using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using Framework.Asset;
namespace Framework.Tables
{
    public class TableInventory {
        private static int curIndex = 0;
        private static string FileName = "Table/Inventory";
        private static Dictionary<int, TableInventory> dic = new Dictionary<int, TableInventory> ();
        private static List<TableInventory> Items = new List<TableInventory>();
        private static TableInventory First;
        private static bool Inited = false;
        public TableInventory Next = null;

		//表Id
		public int Id { private set; get; }
		//道具Id
		public int GoodsId { private set; get; }
		//道具数量
		public int Num { private set; get; }
		//金币价格
		public int Price { private set; get; }
		//折扣信息
		public string Discount { private set; get; }
		//物品描述
		public string Description { private set; get; }


        private static void AddItem (TableInventory item) {
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
                TableInventory preItem = null;
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode data = JSONNode.Parse (tableStr);
                foreach (var itemData in data.Children) {
                    TableInventory item = new TableInventory ();
					item.Id = itemData["Id"];
					item.GoodsId = itemData["GoodsId"];
					item.Num = itemData["Num"];
					item.Price = itemData["Price"];
					item.Discount = itemData["Discount"];
					item.Description = itemData["Description"];


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
              
            TableInventory preItem = null;
            JSONNode data = JSONNode.Parse (tableStr);
            foreach (var itemData in data.Children) {
                TableInventory item = new TableInventory ();
					item.Id = itemData["Id"];
					item.GoodsId = itemData["GoodsId"];
					item.Num = itemData["Num"];
					item.Price = itemData["Price"];
					item.Discount = itemData["Discount"];
					item.Description = itemData["Description"];


                AddItem (item);
                if (preItem != null) {
                    preItem.Next = item;
                }
                preItem = item;

            }
        }
        public TableInventory Head () {
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
        public TableInventory Get (int _id) {
            if (!Inited) {
                Init ();
            }
             try
            {
                TableInventory config;
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
        public List<TableInventory> GetAll()
        {
            if (!Inited)
            {
                Init();
            }
            return Items;
        }
        public List<TableInventory> FindAll(Predicate<TableInventory> match)
        {
            if (!Inited)
            {
                Init();
            }
            return Items.FindAll(match);
        }
    }
}