using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using Framework.Asset;
namespace Framework.Tables
{
    public class TableGoodsBundle {
        private static int curIndex = 0;
        private static string FileName = "Table/GoodsBundle";
        private static Dictionary<int, TableGoodsBundle> dic = new Dictionary<int, TableGoodsBundle> ();
        private static List<TableGoodsBundle> Items = new List<TableGoodsBundle>();
        private static TableGoodsBundle First;
        private static bool Inited = false;
        public TableGoodsBundle Next = null;

		//套装ID
		public int Id { private set; get; }
		//套装名称
		public string Name { private set; get; }
		//物品图标
		public string Icon { private set; get; }
		//商品内容
		public Dictionary<int,int> Content { private set; get; }
		//价格
		public int Price { private set; get; }


        private static void AddItem (TableGoodsBundle item) {
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
                TableGoodsBundle preItem = null;
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode data = JSONNode.Parse (tableStr);
                foreach (var itemData in data.Children) {
                    TableGoodsBundle item = new TableGoodsBundle ();
					item.Id = itemData["Id"];
					item.Name = itemData["Name"];
					item.Icon = itemData["Icon"];
					item.Content = Table.string2Dic_int_int(itemData["Content"]);
					item.Price = itemData["Price"];


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
              
            TableGoodsBundle preItem = null;
            JSONNode data = JSONNode.Parse (tableStr);
            foreach (var itemData in data.Children) {
                TableGoodsBundle item = new TableGoodsBundle ();
					item.Id = itemData["Id"];
					item.Name = itemData["Name"];
					item.Icon = itemData["Icon"];
					item.Content = Table.string2Dic_int_int(itemData["Content"]);
					item.Price = itemData["Price"];


                AddItem (item);
                if (preItem != null) {
                    preItem.Next = item;
                }
                preItem = item;

            }
        }
        public TableGoodsBundle Head () {
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
        public TableGoodsBundle Get (int _id) {
            if (!Inited) {
                Init ();
            }
             try
            {
                TableGoodsBundle config;
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
        public List<TableGoodsBundle> GetAll()
        {
            if (!Inited)
            {
                Init();
            }
            return Items;
        }
        public List<TableGoodsBundle> FindAll(Predicate<TableGoodsBundle> match)
        {
            if (!Inited)
            {
                Init();
            }
            return Items.FindAll(match);
        }
    }
}