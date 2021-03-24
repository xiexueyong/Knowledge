using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using Framework.Asset;
namespace Framework.Tables
{
    public class TableLevelInfo {
        private static int curIndex = 0;
        private static string FileName = "Table/LevelInfo";
        private static Dictionary<int, TableLevelInfo> dic = new Dictionary<int, TableLevelInfo> ();
        private static List<TableLevelInfo> Items = new List<TableLevelInfo>();
        private static TableLevelInfo First;
        private static bool Inited = false;
        public TableLevelInfo Next = null;

		//编号
		public int Id { private set; get; }
		//关卡 ID
		public int fileId { private set; get; }
		//不使用皇冠功能
		public int noCrown { private set; get; }
		//难度标志：1-3：易-难，-1：不可用
		public int difficulty { private set; get; }


        private static void AddItem (TableLevelInfo item) {
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
                TableLevelInfo preItem = null;
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode data = JSONNode.Parse (tableStr);
                foreach (var itemData in data.Children) {
                    TableLevelInfo item = new TableLevelInfo ();
					item.Id = itemData["Id"];
					item.fileId = itemData["fileId"];
					item.noCrown = itemData["noCrown"];
					item.difficulty = itemData["difficulty"];


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
              
            TableLevelInfo preItem = null;
            JSONNode data = JSONNode.Parse (tableStr);
            foreach (var itemData in data.Children) {
                TableLevelInfo item = new TableLevelInfo ();
					item.Id = itemData["Id"];
					item.fileId = itemData["fileId"];
					item.noCrown = itemData["noCrown"];
					item.difficulty = itemData["difficulty"];


                AddItem (item);
                if (preItem != null) {
                    preItem.Next = item;
                }
                preItem = item;

            }
        }
        public TableLevelInfo Head () {
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
        public TableLevelInfo Get (int _id) {
            if (!Inited) {
                Init ();
            }
             try
            {
                TableLevelInfo config;
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
        public List<TableLevelInfo> GetAll()
        {
            if (!Inited)
            {
                Init();
            }
            return Items;
        }
        public List<TableLevelInfo> FindAll(Predicate<TableLevelInfo> match)
        {
            if (!Inited)
            {
                Init();
            }
            return Items.FindAll(match);
        }
    }
}