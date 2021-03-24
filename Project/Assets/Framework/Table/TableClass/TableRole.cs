using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using Framework.Asset;
namespace Framework.Tables
{
    public class TableRole {
        private static int curIndex = 0;
        private static string FileName = "Table/Role";
        private static Dictionary<string, TableRole> dic = new Dictionary<string, TableRole> ();
        private static List<TableRole> Items = new List<TableRole>();
        private static TableRole First;
        private static bool Inited = false;
        public TableRole Next = null;

		//ID
		public string Id { private set; get; }
		//角色名称
		public string Name { private set; get; }
		//人物性别
		public int Sex { private set; get; }
		//职业
		public string Job { private set; get; }
		//简介
		public string Intro { private set; get; }
		//人物立绘
		public string Figure { private set; get; }
		//静态1
		public string Anim_Motionless { private set; get; }
		//正面说话2
		public string Anim_Speaking { private set; get; }
		//沉默3
		public string Anim_Silent { private set; get; }
		//开心4
		public string Anim_Happy { private set; get; }
		//叹气5
		public string Anim_Sigh { private set; get; }
		//生气6
		public string Anim_Anger { private set; get; }
		//认错7
		public string Anim_Apologize { private set; get; }
		//哭泣8
		public string Anim_Cry1 { private set; get; }
		//哭9
		public string Anim_Cry2 { private set; get; }


        private static void AddItem (TableRole item) {
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
                TableRole preItem = null;
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode data = JSONNode.Parse (tableStr);
                foreach (var itemData in data.Children) {
                    TableRole item = new TableRole ();
					item.Id = itemData["Id"];
					item.Name = itemData["Name"];
					item.Sex = itemData["Sex"];
					item.Job = itemData["Job"];
					item.Intro = itemData["Intro"];
					item.Figure = itemData["Figure"];
					item.Anim_Motionless = itemData["Anim_Motionless"];
					item.Anim_Speaking = itemData["Anim_Speaking"];
					item.Anim_Silent = itemData["Anim_Silent"];
					item.Anim_Happy = itemData["Anim_Happy"];
					item.Anim_Sigh = itemData["Anim_Sigh"];
					item.Anim_Anger = itemData["Anim_Anger"];
					item.Anim_Apologize = itemData["Anim_Apologize"];
					item.Anim_Cry1 = itemData["Anim_Cry1"];
					item.Anim_Cry2 = itemData["Anim_Cry2"];


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
              
            TableRole preItem = null;
            JSONNode data = JSONNode.Parse (tableStr);
            foreach (var itemData in data.Children) {
                TableRole item = new TableRole ();
					item.Id = itemData["Id"];
					item.Name = itemData["Name"];
					item.Sex = itemData["Sex"];
					item.Job = itemData["Job"];
					item.Intro = itemData["Intro"];
					item.Figure = itemData["Figure"];
					item.Anim_Motionless = itemData["Anim_Motionless"];
					item.Anim_Speaking = itemData["Anim_Speaking"];
					item.Anim_Silent = itemData["Anim_Silent"];
					item.Anim_Happy = itemData["Anim_Happy"];
					item.Anim_Sigh = itemData["Anim_Sigh"];
					item.Anim_Anger = itemData["Anim_Anger"];
					item.Anim_Apologize = itemData["Anim_Apologize"];
					item.Anim_Cry1 = itemData["Anim_Cry1"];
					item.Anim_Cry2 = itemData["Anim_Cry2"];


                AddItem (item);
                if (preItem != null) {
                    preItem.Next = item;
                }
                preItem = item;

            }
        }
        public TableRole Head () {
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
        public TableRole Get (string _id) {
            if (!Inited) {
                Init ();
            }
             try
            {
                TableRole config;
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
        public List<TableRole> GetAll()
        {
            if (!Inited)
            {
                Init();
            }
            return Items;
        }
        public List<TableRole> FindAll(Predicate<TableRole> match)
        {
            if (!Inited)
            {
                Init();
            }
            return Items.FindAll(match);
        }
    }
}