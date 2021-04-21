using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using Framework.Asset;
namespace Framework.Tables
{
    public class TableWheels {
        private static int curIndex = 0;
        private static string FileName = "Table/Wheels";
        private static Dictionary<int, TableWheels> dic = new Dictionary<int, TableWheels> ();
        private static List<TableWheels> Items = new List<TableWheels>();
        private static TableWheels First;
        private static bool Inited = false;
        public TableWheels Next = null;

		//表Id
		public int Id { private set; get; }
		//转盘Id
		public int WheelId { private set; get; }
		//道具Id
		public int GoodsId { private set; get; }
		//道具数量
		public int Num { private set; get; }
		//权重
		public int Weight { private set; get; }


        private static void AddItem (TableWheels item) {
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
                TableWheels preItem = null;
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode data = JSONNode.Parse (tableStr);
                foreach (var itemData in data.Children) {
                    TableWheels item = new TableWheels ();
					item.Id = itemData["Id"];
					item.WheelId = itemData["WheelId"];
					item.GoodsId = itemData["GoodsId"];
					item.Num = itemData["Num"];
					item.Weight = itemData["Weight"];


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
              
            TableWheels preItem = null;
            JSONNode data = JSONNode.Parse (tableStr);
            foreach (var itemData in data.Children) {
                TableWheels item = new TableWheels ();
					item.Id = itemData["Id"];
					item.WheelId = itemData["WheelId"];
					item.GoodsId = itemData["GoodsId"];
					item.Num = itemData["Num"];
					item.Weight = itemData["Weight"];


                AddItem (item);
                if (preItem != null) {
                    preItem.Next = item;
                }
                preItem = item;

            }
        }
        public TableWheels Head () {
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
        public TableWheels Get (int _id) {
            if (!Inited) {
                Init ();
            }
             try
            {
                TableWheels config;
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
        public List<TableWheels> GetAll()
        {
            if (!Inited)
            {
                Init();
            }
            return Items;
        }
        public List<TableWheels> FindAll(Predicate<TableWheels> match)
        {
            if (!Inited)
            {
                Init();
            }
            return Items.FindAll(match);
        }
    }
}