using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using Framework.Asset;
namespace Framework.Tables
{
    public class TableArchives {
        private static int curIndex = 0;
        private static string FileName = "Table/Archives";
        private static Dictionary<string, TableArchives> dic = new Dictionary<string, TableArchives> ();
        private static List<TableArchives> Items = new List<TableArchives>();
        private static TableArchives First;
        private static bool Inited = false;
        public TableArchives Next = null;

		//档案ID
		public string Id { private set; get; }
		//角色Id
		public string RoleId { private set; get; }
		//顺序
		public int Index { private set; get; }
		//档案Title
		public string Title { private set; get; }
		//档案稀有度
		public int Important { private set; get; }
		//钥匙数量
		public int KeyCount { private set; get; }
		//插图
		public string Image { private set; get; }
		//档案内容
		public string Content { private set; get; }


        private static void AddItem (TableArchives item) {
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
                TableArchives preItem = null;
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode data = JSONNode.Parse (tableStr);
                foreach (var itemData in data.Children) {
                    TableArchives item = new TableArchives ();
					item.Id = itemData["Id"];
					item.RoleId = itemData["RoleId"];
					item.Index = itemData["Index"];
					item.Title = itemData["Title"];
					item.Important = itemData["Important"];
					item.KeyCount = itemData["KeyCount"];
					item.Image = itemData["Image"];
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
              
            TableArchives preItem = null;
            JSONNode data = JSONNode.Parse (tableStr);
            foreach (var itemData in data.Children) {
                TableArchives item = new TableArchives ();
					item.Id = itemData["Id"];
					item.RoleId = itemData["RoleId"];
					item.Index = itemData["Index"];
					item.Title = itemData["Title"];
					item.Important = itemData["Important"];
					item.KeyCount = itemData["KeyCount"];
					item.Image = itemData["Image"];
					item.Content = itemData["Content"];


                AddItem (item);
                if (preItem != null) {
                    preItem.Next = item;
                }
                preItem = item;

            }
        }
        public TableArchives Head () {
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
        public TableArchives Get (string _id) {
            if (!Inited) {
                Init ();
            }
             try
            {
                TableArchives config;
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
        public List<TableArchives> GetAll()
        {
            if (!Inited)
            {
                Init();
            }
            return Items;
        }
        public List<TableArchives> FindAll(Predicate<TableArchives> match)
        {
            if (!Inited)
            {
                Init();
            }
            return Items.FindAll(match);
        }
    }
}