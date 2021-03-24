using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using Framework.Asset;
namespace Framework.Tables
{
    public class TableSounds {
        private static int curIndex = 0;
        private static string FileName = "Table/Sounds";
        private static Dictionary<int, TableSounds> dic = new Dictionary<int, TableSounds> ();
        private static List<TableSounds> Items = new List<TableSounds>();
        private static TableSounds First;
        private static bool Inited = false;
        public TableSounds Next = null;

		//Id
		public int Id { private set; get; }
		//资源路径
		public string Sound_Res { private set; get; }
		//声音名
		public string Sound_Name { private set; get; }
		//声音类型
		public string Sound_Type { private set; get; }
		//备注
		public string beizhu { private set; get; }


        private static void AddItem (TableSounds item) {
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
                TableSounds preItem = null;
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode data = JSONNode.Parse (tableStr);
                foreach (var itemData in data.Children) {
                    TableSounds item = new TableSounds ();
					item.Id = itemData["Id"];
					item.Sound_Res = itemData["Sound_Res"];
					item.Sound_Name = itemData["Sound_Name"];
					item.Sound_Type = itemData["Sound_Type"];
					item.beizhu = itemData["beizhu"];


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
              
            TableSounds preItem = null;
            JSONNode data = JSONNode.Parse (tableStr);
            foreach (var itemData in data.Children) {
                TableSounds item = new TableSounds ();
					item.Id = itemData["Id"];
					item.Sound_Res = itemData["Sound_Res"];
					item.Sound_Name = itemData["Sound_Name"];
					item.Sound_Type = itemData["Sound_Type"];
					item.beizhu = itemData["beizhu"];


                AddItem (item);
                if (preItem != null) {
                    preItem.Next = item;
                }
                preItem = item;

            }
        }
        public TableSounds Head () {
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
        public TableSounds Get (int _id) {
            if (!Inited) {
                Init ();
            }
             try
            {
                TableSounds config;
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
        public List<TableSounds> GetAll()
        {
            if (!Inited)
            {
                Init();
            }
            return Items;
        }
        public List<TableSounds> FindAll(Predicate<TableSounds> match)
        {
            if (!Inited)
            {
                Init();
            }
            return Items.FindAll(match);
        }
    }
}