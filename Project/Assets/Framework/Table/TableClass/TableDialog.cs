using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using Framework.Asset;
namespace Framework.Tables
{
    public class TableDialog {
        private static int curIndex = 0;
        private static string FileName = "Table/Dialog";
        private static Dictionary<string, TableDialog> dic = new Dictionary<string, TableDialog> ();
        private static List<TableDialog> Items = new List<TableDialog>();
        private static TableDialog First;
        private static bool Inited = false;
        public TableDialog Next = null;

		//对话Id
		public string Id { private set; get; }
		//对话组Id
		public string Group { private set; get; }
		//顺序
		public int Index { private set; get; }
		//文本Id
		public string RoleId { private set; get; }
		//出现位置
		public string Position { private set; get; }
		//对话内容
		public string Content { private set; get; }


        private static void AddItem (TableDialog item) {
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
                TableDialog preItem = null;
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode data = JSONNode.Parse (tableStr);
                foreach (var itemData in data.Children) {
                    TableDialog item = new TableDialog ();
					item.Id = itemData["Id"];
					item.Group = itemData["Group"];
					item.Index = itemData["Index"];
					item.RoleId = itemData["RoleId"];
					item.Position = itemData["Position"];
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
              
            TableDialog preItem = null;
            JSONNode data = JSONNode.Parse (tableStr);
            foreach (var itemData in data.Children) {
                TableDialog item = new TableDialog ();
					item.Id = itemData["Id"];
					item.Group = itemData["Group"];
					item.Index = itemData["Index"];
					item.RoleId = itemData["RoleId"];
					item.Position = itemData["Position"];
					item.Content = itemData["Content"];


                AddItem (item);
                if (preItem != null) {
                    preItem.Next = item;
                }
                preItem = item;

            }
        }
        public TableDialog Head () {
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
        public TableDialog Get (string _id) {
            if (!Inited) {
                Init ();
            }
             try
            {
                TableDialog config;
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
        public List<TableDialog> GetAll()
        {
            if (!Inited)
            {
                Init();
            }
            return Items;
        }
        public List<TableDialog> FindAll(Predicate<TableDialog> match)
        {
            if (!Inited)
            {
                Init();
            }
            return Items.FindAll(match);
        }
    }
}