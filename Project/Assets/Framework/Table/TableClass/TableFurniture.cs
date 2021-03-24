using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using Framework.Asset;
namespace Framework.Tables
{
    public class TableFurniture {
        private static int curIndex = 0;
        private static string FileName = "Table/Furniture";
        private static Dictionary<string, TableFurniture> dic = new Dictionary<string, TableFurniture> ();
        private static List<TableFurniture> Items = new List<TableFurniture>();
        private static TableFurniture First;
        private static bool Inited = false;
        public TableFurniture Next = null;

		//家具ID
		public string Id { private set; get; }
		//名称
		public string Name { private set; get; }
		//风格
		public string Skin { private set; get; }


        private static void AddItem (TableFurniture item) {
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
                TableFurniture preItem = null;
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode data = JSONNode.Parse (tableStr);
                foreach (var itemData in data.Children) {
                    TableFurniture item = new TableFurniture ();
					item.Id = itemData["Id"];
					item.Name = itemData["Name"];
					item.Skin = itemData["Skin"];


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
              
            TableFurniture preItem = null;
            JSONNode data = JSONNode.Parse (tableStr);
            foreach (var itemData in data.Children) {
                TableFurniture item = new TableFurniture ();
					item.Id = itemData["Id"];
					item.Name = itemData["Name"];
					item.Skin = itemData["Skin"];


                AddItem (item);
                if (preItem != null) {
                    preItem.Next = item;
                }
                preItem = item;

            }
        }
        public TableFurniture Head () {
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
        public TableFurniture Get (string _id) {
            if (!Inited) {
                Init ();
            }
             try
            {
                TableFurniture config;
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
        public List<TableFurniture> GetAll()
        {
            if (!Inited)
            {
                Init();
            }
            return Items;
        }
        public List<TableFurniture> FindAll(Predicate<TableFurniture> match)
        {
            if (!Inited)
            {
                Init();
            }
            return Items.FindAll(match);
        }
    }
}