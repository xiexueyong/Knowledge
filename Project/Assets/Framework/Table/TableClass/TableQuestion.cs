using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using Framework.Asset;
namespace Framework.Tables
{
    public class TableQuestion {
        private static int curIndex = 0;
        private static string FileName = "Table/Question";
        private static Dictionary<int, TableQuestion> dic = new Dictionary<int, TableQuestion> ();
        private static List<TableQuestion> Items = new List<TableQuestion>();
        private static TableQuestion First;
        private static bool Inited = false;
        public TableQuestion Next = null;

		//Id
		public int Id { private set; get; }
		//类型
		public string type { private set; get; }
		//题目
		public string question { private set; get; }
		//插图
		public string Illustrations { private set; get; }
		//答案A
		public string anwserA { private set; get; }
		//答案B
		public string anwserB { private set; get; }
		//答案C
		public string anwserC { private set; get; }
		//答案D
		public string anwserD { private set; get; }
		//选择答案
		public string chooseAnwer { private set; get; }
		//判断答案
		public string judgeAnwser { private set; get; }
		//详细解释
		public string explanation { private set; get; }


        private static void AddItem (TableQuestion item) {
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
                TableQuestion preItem = null;
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode data = JSONNode.Parse (tableStr);
                foreach (var itemData in data.Children) {
                    TableQuestion item = new TableQuestion ();
					item.Id = itemData["Id"];
					item.type = itemData["type"];
					item.question = itemData["question"];
					item.Illustrations = itemData["Illustrations"];
					item.anwserA = itemData["anwserA"];
					item.anwserB = itemData["anwserB"];
					item.anwserC = itemData["anwserC"];
					item.anwserD = itemData["anwserD"];
					item.chooseAnwer = itemData["chooseAnwer"];
					item.judgeAnwser = itemData["judgeAnwser"];
					item.explanation = itemData["explanation"];


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
              
            TableQuestion preItem = null;
            JSONNode data = JSONNode.Parse (tableStr);
            foreach (var itemData in data.Children) {
                TableQuestion item = new TableQuestion ();
					item.Id = itemData["Id"];
					item.type = itemData["type"];
					item.question = itemData["question"];
					item.Illustrations = itemData["Illustrations"];
					item.anwserA = itemData["anwserA"];
					item.anwserB = itemData["anwserB"];
					item.anwserC = itemData["anwserC"];
					item.anwserD = itemData["anwserD"];
					item.chooseAnwer = itemData["chooseAnwer"];
					item.judgeAnwser = itemData["judgeAnwser"];
					item.explanation = itemData["explanation"];


                AddItem (item);
                if (preItem != null) {
                    preItem.Next = item;
                }
                preItem = item;

            }
        }
        public TableQuestion Head () {
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
        public TableQuestion Get (int _id) {
            if (!Inited) {
                Init ();
            }
             try
            {
                TableQuestion config;
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
        public List<TableQuestion> GetAll()
        {
            if (!Inited)
            {
                Init();
            }
            return Items;
        }
        public List<TableQuestion> FindAll(Predicate<TableQuestion> match)
        {
            if (!Inited)
            {
                Init();
            }
            return Items.FindAll(match);
        }
    }
}