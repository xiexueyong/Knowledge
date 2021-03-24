using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using Framework.Asset;
namespace Framework.Tables
{
    public class TableTask {
        private static int curIndex = 0;
        private static string FileName = "Table/Task";
        private static Dictionary<int, TableTask> dic = new Dictionary<int, TableTask> ();
        private static List<TableTask> Items = new List<TableTask>();
        private static TableTask First;
        private static bool Inited = false;
        public TableTask Next = null;

		//任务ID
		public int Id { private set; get; }
		//任务组Id
		public int Chapter { private set; get; }
		//任务子组id
		public int Part { private set; get; }
		//组内id
		public int Step { private set; get; }
		//消耗点数
		public int Cost { private set; get; }
		//任务人物
		public string Npc { private set; get; }
		//任务图标
		public string Icon { private set; get; }
		//任务名称
		public string Title { private set; get; }
		//前置任务对话
		public string PreDialogGroupId { private set; get; }
		//任务触发事件
		public string Event { private set; get; }
		//后置任务对话
		public string PostDialogGroupId { private set; get; }
		//奖励物品
		public Dictionary<int,int> Reward { private set; get; }
		//解锁日记
		public int UnlockDiary { private set; get; }
		//解锁档案
		public Dictionary<string,int> UnlockArchive { private set; get; }


        private static void AddItem (TableTask item) {
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
                TableTask preItem = null;
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode data = JSONNode.Parse (tableStr);
                foreach (var itemData in data.Children) {
                    TableTask item = new TableTask ();
					item.Id = itemData["Id"];
					item.Chapter = itemData["Chapter"];
					item.Part = itemData["Part"];
					item.Step = itemData["Step"];
					item.Cost = itemData["Cost"];
					item.Npc = itemData["Npc"];
					item.Icon = itemData["Icon"];
					item.Title = itemData["Title"];
					item.PreDialogGroupId = itemData["PreDialogGroupId"];
					item.Event = itemData["Event"];
					item.PostDialogGroupId = itemData["PostDialogGroupId"];
					item.Reward = Table.string2Dic_int_int(itemData["Reward"]);
					item.UnlockDiary = itemData["UnlockDiary"];
					item.UnlockArchive = Table.string2Dic_string_int(itemData["UnlockArchive"]);


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
              
            TableTask preItem = null;
            JSONNode data = JSONNode.Parse (tableStr);
            foreach (var itemData in data.Children) {
                TableTask item = new TableTask ();
					item.Id = itemData["Id"];
					item.Chapter = itemData["Chapter"];
					item.Part = itemData["Part"];
					item.Step = itemData["Step"];
					item.Cost = itemData["Cost"];
					item.Npc = itemData["Npc"];
					item.Icon = itemData["Icon"];
					item.Title = itemData["Title"];
					item.PreDialogGroupId = itemData["PreDialogGroupId"];
					item.Event = itemData["Event"];
					item.PostDialogGroupId = itemData["PostDialogGroupId"];
					item.Reward = Table.string2Dic_int_int(itemData["Reward"]);
					item.UnlockDiary = itemData["UnlockDiary"];
					item.UnlockArchive = Table.string2Dic_string_int(itemData["UnlockArchive"]);


                AddItem (item);
                if (preItem != null) {
                    preItem.Next = item;
                }
                preItem = item;

            }
        }
        public TableTask Head () {
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
        public TableTask Get (int _id) {
            if (!Inited) {
                Init ();
            }
             try
            {
                TableTask config;
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
        public List<TableTask> GetAll()
        {
            if (!Inited)
            {
                Init();
            }
            return Items;
        }
        public List<TableTask> FindAll(Predicate<TableTask> match)
        {
            if (!Inited)
            {
                Init();
            }
            return Items.FindAll(match);
        }
    }
}