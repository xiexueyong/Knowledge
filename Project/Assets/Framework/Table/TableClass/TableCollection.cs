using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using Framework.Asset;
namespace Framework.Tables
{
    public class TableCollection {
        private static int curIndex = 0;
        private static string FileName = "Table/Collection";
        private static Dictionary<int, TableCollection> dic = new Dictionary<int, TableCollection> ();
        private static List<TableCollection> Items = new List<TableCollection>();
        private static TableCollection First;
        private static bool Inited = false;
        public TableCollection Next = null;

		//物品ID
		public int Id { private set; get; }
		//物品类型
		public string Type { private set; get; }
		//物品图片
		public string Image { private set; get; }
		//物品名称
		public string Name { private set; get; }
		//物品描述
		public string Content { private set; get; }


        private static void AddItem (TableCollection item) {
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
                TableCollection preItem = null;
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode data = JSONNode.Parse (tableStr);
                foreach (var itemData in data.Children) {
                    TableCollection item = new TableCollection ();
					item.Id = itemData["Id"];
					item.Type = itemData["Type"];
					item.Image = itemData["Image"];
					item.Name = itemData["Name"];
					item.Content = itemData["Content"];


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
              
            TableCollection preItem = null;
            JSONNode data = JSONNode.Parse (tableStr);
            foreach (var itemData in data.Children) {
                TableCollection item = new TableCollection ();
					item.Id = itemData["Id"];
					item.Type = itemData["Type"];
					item.Image = itemData["Image"];
					item.Name = itemData["Name"];
					item.Content = itemData["Content"];


                AddItem (item);
                if (preItem != null) {
                    preItem.Next = item;
                }
                preItem = item;

            }
        }
        public TableCollection Head () {
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
        public TableCollection Get (int _id) {
            if (!Inited) {
                Init ();
            }
             try
            {
                TableCollection config;
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
        public List<TableCollection> GetAll()
        {
            if (!Inited)
            {
                Init();
            }
            return Items;
        }
        public List<TableCollection> FindAll(Predicate<TableCollection> match)
        {
            if (!Inited)
            {
                Init();
            }
            return Items.FindAll(match);
        }
    }
}