using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using Framework.Asset;
namespace Framework.Tables
{
    public class TableAdChannelPostfix {
        private static int curIndex = 0;
        private static string FileName = "Table/AdChannelPostfix";
        private static Dictionary<string, TableAdChannelPostfix> dic = new Dictionary<string, TableAdChannelPostfix> ();
        private static List<TableAdChannelPostfix> Items = new List<TableAdChannelPostfix>();
        private static TableAdChannelPostfix First;
        private static bool Inited = false;
        public TableAdChannelPostfix Next = null;

		//广告渠道
		public string Id { private set; get; }
		//后缀
		public string Postfix { private set; get; }


        private static void AddItem (TableAdChannelPostfix item) {
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
                TableAdChannelPostfix preItem = null;
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode data = JSONNode.Parse (tableStr);
                foreach (var itemData in data.Children) {
                    TableAdChannelPostfix item = new TableAdChannelPostfix ();
					item.Id = itemData["Id"];
					item.Postfix = itemData["Postfix"];


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
              
            TableAdChannelPostfix preItem = null;
            JSONNode data = JSONNode.Parse (tableStr);
            foreach (var itemData in data.Children) {
                TableAdChannelPostfix item = new TableAdChannelPostfix ();
					item.Id = itemData["Id"];
					item.Postfix = itemData["Postfix"];


                AddItem (item);
                if (preItem != null) {
                    preItem.Next = item;
                }
                preItem = item;

            }
        }
        public TableAdChannelPostfix Head () {
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
        public TableAdChannelPostfix Get (string _id) {
            if (!Inited) {
                Init ();
            }
             try
            {
                TableAdChannelPostfix config;
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
        public List<TableAdChannelPostfix> GetAll()
        {
            if (!Inited)
            {
                Init();
            }
            return Items;
        }
        public List<TableAdChannelPostfix> FindAll(Predicate<TableAdChannelPostfix> match)
        {
            if (!Inited)
            {
                Init();
            }
            return Items.FindAll(match);
        }
    }
}