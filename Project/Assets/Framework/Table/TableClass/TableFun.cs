using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using Framework.Asset;
namespace Framework.Tables
{
    public class TableFun {
        private static int curIndex = 0;
        private static string FileName = "Table/Fun";
        private static Dictionary<int, TableFun> dic = new Dictionary<int, TableFun> ();
        private static List<TableFun> Items = new List<TableFun>();
        private static TableFun First;
        private static bool Inited = false;
        public TableFun Next = null;

		//ID
		public int Id { private set; get; }
		//角色
		public string RoldId { private set; get; }
		//内容
		public string Context { private set; get; }
		//解锁任务
		public int ActiveTask { private set; get; }


        private static void AddItem (TableFun item) {
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
                TableFun preItem = null;
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode data = JSONNode.Parse (tableStr);
                foreach (var itemData in data.Children) {
                    TableFun item = new TableFun ();
					item.Id = itemData["Id"];
					item.RoldId = itemData["RoldId"];
					item.Context = itemData["Context"];
					item.ActiveTask = itemData["ActiveTask"];


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
              
            TableFun preItem = null;
            JSONNode data = JSONNode.Parse (tableStr);
            foreach (var itemData in data.Children) {
                TableFun item = new TableFun ();
					item.Id = itemData["Id"];
					item.RoldId = itemData["RoldId"];
					item.Context = itemData["Context"];
					item.ActiveTask = itemData["ActiveTask"];


                AddItem (item);
                if (preItem != null) {
                    preItem.Next = item;
                }
                preItem = item;

            }
        }
        public TableFun Head () {
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
        public TableFun Get (int _id) {
            if (!Inited) {
                Init ();
            }
             try
            {
                TableFun config;
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
        public List<TableFun> GetAll()
        {
            if (!Inited)
            {
                Init();
            }
            return Items;
        }
        public List<TableFun> FindAll(Predicate<TableFun> match)
        {
            if (!Inited)
            {
                Init();
            }
            return Items.FindAll(match);
        }
    }
}