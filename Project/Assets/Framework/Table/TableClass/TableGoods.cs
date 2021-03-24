using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using Framework.Asset;
namespace Framework.Tables
{
    public class TableGoods {
        private static int curIndex = 0;
        private static string FileName = "Table/Goods";
        private static Dictionary<int, TableGoods> dic = new Dictionary<int, TableGoods> ();
        private static List<TableGoods> Items = new List<TableGoods>();
        private static TableGoods First;
        private static bool Inited = false;
        public TableGoods Next = null;

		//物品ID
		public int Id { private set; get; }
		//物品名称
		public string Name { private set; get; }
		//物品图标
		public string Icon { private set; get; }
		//预制体
		public string Prefab { private set; get; }
		//添加booster的prefab
		public string AddBoosterLogicName { private set; get; }
		//前置道具
		public int FrontBooster { private set; get; }
		//中置道具
		public int MiddleBooster { private set; get; }
		//后置道具
		public int BackBooster { private set; get; }
		//道具类型
		public int GoodsType { private set; get; }
		//解锁等级
		public int UnlockLevel { private set; get; }
		//备注
		public string Des { private set; get; }


        private static void AddItem (TableGoods item) {
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
                TableGoods preItem = null;
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode data = JSONNode.Parse (tableStr);
                foreach (var itemData in data.Children) {
                    TableGoods item = new TableGoods ();
					item.Id = itemData["Id"];
					item.Name = itemData["Name"];
					item.Icon = itemData["Icon"];
					item.Prefab = itemData["Prefab"];
					item.AddBoosterLogicName = itemData["AddBoosterLogicName"];
					item.FrontBooster = itemData["FrontBooster"];
					item.MiddleBooster = itemData["MiddleBooster"];
					item.BackBooster = itemData["BackBooster"];
					item.GoodsType = itemData["GoodsType"];
					item.UnlockLevel = itemData["UnlockLevel"];
					item.Des = itemData["Des"];


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
              
            TableGoods preItem = null;
            JSONNode data = JSONNode.Parse (tableStr);
            foreach (var itemData in data.Children) {
                TableGoods item = new TableGoods ();
					item.Id = itemData["Id"];
					item.Name = itemData["Name"];
					item.Icon = itemData["Icon"];
					item.Prefab = itemData["Prefab"];
					item.AddBoosterLogicName = itemData["AddBoosterLogicName"];
					item.FrontBooster = itemData["FrontBooster"];
					item.MiddleBooster = itemData["MiddleBooster"];
					item.BackBooster = itemData["BackBooster"];
					item.GoodsType = itemData["GoodsType"];
					item.UnlockLevel = itemData["UnlockLevel"];
					item.Des = itemData["Des"];


                AddItem (item);
                if (preItem != null) {
                    preItem.Next = item;
                }
                preItem = item;

            }
        }
        public TableGoods Head () {
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
        public TableGoods Get (int _id) {
            if (!Inited) {
                Init ();
            }
             try
            {
                TableGoods config;
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
        public List<TableGoods> GetAll()
        {
            if (!Inited)
            {
                Init();
            }
            return Items;
        }
        public List<TableGoods> FindAll(Predicate<TableGoods> match)
        {
            if (!Inited)
            {
                Init();
            }
            return Items.FindAll(match);
        }
    }
}