using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using Framework.Asset;
namespace Framework.Tables
{
    public class TableDiary {
        private static int curIndex = 0;
        private static string FileName = "Table/Diary";
        private static Dictionary<int, TableDiary> dic = new Dictionary<int, TableDiary> ();
        private static List<TableDiary> Items = new List<TableDiary>();
        private static TableDiary First;
        private static bool Inited = false;
        public TableDiary Next = null;

		//日记ID
		public int Id { private set; get; }
		//日记日期
		public string Date { private set; get; }
		//日记信息
		public string Content { private set; get; }


        private static void AddItem (TableDiary item) {
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
                TableDiary preItem = null;
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode data = JSONNode.Parse (tableStr);
                foreach (var itemData in data.Children) {
                    TableDiary item = new TableDiary ();
					item.Id = itemData["Id"];
					item.Date = itemData["Date"];
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
              
            TableDiary preItem = null;
            JSONNode data = JSONNode.Parse (tableStr);
            foreach (var itemData in data.Children) {
                TableDiary item = new TableDiary ();
					item.Id = itemData["Id"];
					item.Date = itemData["Date"];
					item.Content = itemData["Content"];


                AddItem (item);
                if (preItem != null) {
                    preItem.Next = item;
                }
                preItem = item;

            }
        }
        public TableDiary Head () {
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
        public TableDiary Get (int _id) {
            if (!Inited) {
                Init ();
            }
             try
            {
                TableDiary config;
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
        public List<TableDiary> GetAll()
        {
            if (!Inited)
            {
                Init();
            }
            return Items;
        }
        public List<TableDiary> FindAll(Predicate<TableDiary> match)
        {
            if (!Inited)
            {
                Init();
            }
            return Items.FindAll(match);
        }
    }
}