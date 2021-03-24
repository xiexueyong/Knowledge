using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using Framework.Asset;
namespace Framework.Tables
{
    public class TableLanguage {
        private static int curIndex = 0;
        private static string FileName = "Table/Language";
        private static Dictionary<string, TableLanguage> dic = new Dictionary<string, TableLanguage> ();
        private static List<TableLanguage> Items = new List<TableLanguage>();
        private static TableLanguage First;
        private static bool Inited = false;
        public TableLanguage Next = null;

		//文本ID
		public string Id { private set; get; }
		//汉语
		public string CHA { private set; get; }
		//英语
		public string EN { private set; get; }


        private static void AddItem (TableLanguage item) {
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
                TableLanguage preItem = null;
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode data = JSONNode.Parse (tableStr);
                foreach (var itemData in data.Children) {
                    TableLanguage item = new TableLanguage ();
					item.Id = itemData["Id"];
					item.CHA = itemData["CHA"];
					item.EN = itemData["EN"];


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
              
            TableLanguage preItem = null;
            JSONNode data = JSONNode.Parse (tableStr);
            foreach (var itemData in data.Children) {
                TableLanguage item = new TableLanguage ();
					item.Id = itemData["Id"];
					item.CHA = itemData["CHA"];
					item.EN = itemData["EN"];


                AddItem (item);
                if (preItem != null) {
                    preItem.Next = item;
                }
                preItem = item;

            }
        }
        public TableLanguage Head () {
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
        public TableLanguage Get (string _id) {
            if (!Inited) {
                Init ();
            }
             try
            {
                TableLanguage config;
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
        public List<TableLanguage> GetAll()
        {
            if (!Inited)
            {
                Init();
            }
            return Items;
        }
        public List<TableLanguage> FindAll(Predicate<TableLanguage> match)
        {
            if (!Inited)
            {
                Init();
            }
            return Items.FindAll(match);
        }
    }
}