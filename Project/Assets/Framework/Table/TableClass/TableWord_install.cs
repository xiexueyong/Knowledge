using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using Framework.Asset;
namespace Framework.Tables
{
    public class TableWord_install {
        private static int curIndex = 0;
        private static string FileName = "Table/Word_install";
        private static Dictionary<int, TableWord_install> dic = new Dictionary<int, TableWord_install> ();
        private static List<TableWord_install> Items = new List<TableWord_install>();
        private static TableWord_install First;
        private static bool Inited = false;
        public TableWord_install Next = null;

		//关卡ID
		public int Id { private set; get; }
		//地图生成
		public string Layout { private set; get; }
		//道具Id、数量
		public List<Triangle<int,int,int>> Reward { private set; get; }


        private static void AddItem (TableWord_install item) {
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
                TableWord_install preItem = null;
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode data = JSONNode.Parse (tableStr);
                foreach (var itemData in data.Children) {
                    TableWord_install item = new TableWord_install ();
					item.Id = itemData["Id"];
					item.Layout = itemData["Layout"];
					item.Reward = Table.string2List_Triangle_int_int_int(itemData["Reward"]);


                    AddItem (item);
                    if (preItem != null) {
                        preItem.Next = item;
                    }
                    preItem = item;

                }
                Inited = true;
            }
        }
        public TableWord_install Head () {
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
        public TableWord_install Get (int _id) {
            if (!Inited) {
                Init ();
            }
             try
            {
                TableWord_install config;
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
        public List<TableWord_install> GetAll()
        {
            if (!Inited)
            {
                Init();
            }
            return Items;
        }
        public List<TableWord_install> FindAll(Predicate<TableWord_install> match)
        {
            if (!Inited)
            {
                Init();
            }
            return Items.FindAll(match);
        }
    }
}