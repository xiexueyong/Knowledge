using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using Framework.Asset;
namespace Framework.Tables
{
    public class TableDegree {
        private static int curIndex = 0;
        private static string FileName = "Table/Degree";
        private static Dictionary<int, TableDegree> dic = new Dictionary<int, TableDegree> ();
        private static List<TableDegree> Items = new List<TableDegree>();
        private static TableDegree First;
        private static bool Inited = false;
        public TableDegree Next = null;

		//Id
		public int Id { private set; get; }
		//等级上线
		public int levelTop { private set; get; }
		//学位名称
		public string degreeName { private set; get; }
		//升阶
		public string degreeRaise { private set; get; }
		//奖励
		public int Coin { private set; get; }


        private static void AddItem (TableDegree item) {
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
                TableDegree preItem = null;
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode data = JSONNode.Parse (tableStr);
                foreach (var itemData in data.Children) {
                    TableDegree item = new TableDegree ();
					item.Id = itemData["Id"];
					item.levelTop = itemData["levelTop"];
					item.degreeName = itemData["degreeName"];
					item.degreeRaise = itemData["degreeRaise"];
					item.Coin = itemData["Coin"];


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
              
            TableDegree preItem = null;
            JSONNode data = JSONNode.Parse (tableStr);
            foreach (var itemData in data.Children) {
                TableDegree item = new TableDegree ();
					item.Id = itemData["Id"];
					item.levelTop = itemData["levelTop"];
					item.degreeName = itemData["degreeName"];
					item.degreeRaise = itemData["degreeRaise"];
					item.Coin = itemData["Coin"];


                AddItem (item);
                if (preItem != null) {
                    preItem.Next = item;
                }
                preItem = item;

            }
        }
        public TableDegree Head () {
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
        public TableDegree Get (int _id) {
            if (!Inited) {
                Init ();
            }
             try
            {
                TableDegree config;
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
        public List<TableDegree> GetAll()
        {
            if (!Inited)
            {
                Init();
            }
            return Items;
        }
        public List<TableDegree> FindAll(Predicate<TableDegree> match)
        {
            if (!Inited)
            {
                Init();
            }
            return Items.FindAll(match);
        }
    }
}