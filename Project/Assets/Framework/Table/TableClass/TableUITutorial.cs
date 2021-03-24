using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using Framework.Asset;
namespace Framework.Tables
{
    public class TableUITutorial {
        private static int curIndex = 0;
        private static string FileName = "Table/UITutorial";
        private static Dictionary<string, TableUITutorial> dic = new Dictionary<string, TableUITutorial> ();
        private static List<TableUITutorial> Items = new List<TableUITutorial>();
        private static TableUITutorial First;
        private static bool Inited = false;
        public TableUITutorial Next = null;

		//表Id
		public string Id { private set; get; }
		//引导的信息
		public string Msg { private set; get; }
		//引导文本
		public string Text { private set; get; }
		//文本距中心点Y偏移量
		public int TextYOffset { private set; get; }
		//是否有继续按钮
		public bool HasContiueBtn { private set; get; }


        private static void AddItem (TableUITutorial item) {
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
                TableUITutorial preItem = null;
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode data = JSONNode.Parse (tableStr);
                foreach (var itemData in data.Children) {
                    TableUITutorial item = new TableUITutorial ();
					item.Id = itemData["Id"];
					item.Msg = itemData["Msg"];
					item.Text = itemData["Text"];
					item.TextYOffset = itemData["TextYOffset"];
					item.HasContiueBtn = Table.string2Bool(itemData["HasContiueBtn"]);


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
              
            TableUITutorial preItem = null;
            JSONNode data = JSONNode.Parse (tableStr);
            foreach (var itemData in data.Children) {
                TableUITutorial item = new TableUITutorial ();
					item.Id = itemData["Id"];
					item.Msg = itemData["Msg"];
					item.Text = itemData["Text"];
					item.TextYOffset = itemData["TextYOffset"];
					item.HasContiueBtn = Table.string2Bool(itemData["HasContiueBtn"]);


                AddItem (item);
                if (preItem != null) {
                    preItem.Next = item;
                }
                preItem = item;

            }
        }
        public TableUITutorial Head () {
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
        public TableUITutorial Get (string _id) {
            if (!Inited) {
                Init ();
            }
             try
            {
                TableUITutorial config;
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
        public List<TableUITutorial> GetAll()
        {
            if (!Inited)
            {
                Init();
            }
            return Items;
        }
        public List<TableUITutorial> FindAll(Predicate<TableUITutorial> match)
        {
            if (!Inited)
            {
                Init();
            }
            return Items.FindAll(match);
        }
    }
}