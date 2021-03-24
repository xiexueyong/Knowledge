using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using Framework.Asset;
namespace Framework.Tables
{
    public class TableMail {
        private static int curIndex = 0;
        private static string FileName = "Table/Mail";
        private static Dictionary<int, TableMail> dic = new Dictionary<int, TableMail> ();
        private static List<TableMail> Items = new List<TableMail>();
        private static TableMail First;
        private static bool Inited = false;
        public TableMail Next = null;

		//邮件ID
		public int Id { private set; get; }
		//邮件照片
		public string Photo { private set; get; }
		//邮件标题
		public string Title { private set; get; }
		//邮件正文
		public string Content { private set; get; }
		//邮件奖励
		public List<Triangle<int,int,int>> Reward { private set; get; }
		//发件人
		public string Sender { private set; get; }
		//Yours
		public string Yours { private set; get; }


        private static void AddItem (TableMail item) {
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
                TableMail preItem = null;
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode data = JSONNode.Parse (tableStr);
                foreach (var itemData in data.Children) {
                    TableMail item = new TableMail ();
					item.Id = itemData["Id"];
					item.Photo = itemData["Photo"];
					item.Title = itemData["Title"];
					item.Content = itemData["Content"];
					item.Reward = Table.string2List_Triangle_int_int_int(itemData["Reward"]);
					item.Sender = itemData["Sender"];
					item.Yours = itemData["Yours"];


                    AddItem (item);
                    if (preItem != null) {
                        preItem.Next = item;
                    }
                    preItem = item;

                }
                Inited = true;
            }
        }
        public TableMail Head () {
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
        public TableMail Get (int _id) {
            if (!Inited) {
                Init ();
            }
             try
            {
                TableMail config;
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
        public List<TableMail> GetAll()
        {
            if (!Inited)
            {
                Init();
            }
            return Items;
        }
        public List<TableMail> FindAll(Predicate<TableMail> match)
        {
            if (!Inited)
            {
                Init();
            }
            return Items.FindAll(match);
        }
    }
}