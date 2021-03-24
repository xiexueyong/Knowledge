using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using Framework.Asset;
namespace Framework.Tables
{
    public class TableTutorialReward {
        private static int curIndex = 0;
        private static string FileName = "Table/TutorialReward";
        private static Dictionary<int, TableTutorialReward> dic = new Dictionary<int, TableTutorialReward> ();
        private static List<TableTutorialReward> Items = new List<TableTutorialReward>();
        private static TableTutorialReward First;
        private static bool Inited = false;
        public TableTutorialReward Next = null;

		//Id（Level）
		public int Id { private set; get; }
		//奖励道具及数量
		public Dictionary<int,int> Rewards { private set; get; }


        private static void AddItem (TableTutorialReward item) {
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
                TableTutorialReward preItem = null;
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode data = JSONNode.Parse (tableStr);
                foreach (var itemData in data.Children) {
                    TableTutorialReward item = new TableTutorialReward ();
					item.Id = itemData["Id"];
					item.Rewards = Table.string2Dic_int_int(itemData["Rewards"]);


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
              
            TableTutorialReward preItem = null;
            JSONNode data = JSONNode.Parse (tableStr);
            foreach (var itemData in data.Children) {
                TableTutorialReward item = new TableTutorialReward ();
					item.Id = itemData["Id"];
					item.Rewards = Table.string2Dic_int_int(itemData["Rewards"]);


                AddItem (item);
                if (preItem != null) {
                    preItem.Next = item;
                }
                preItem = item;

            }
        }
        public TableTutorialReward Head () {
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
        public TableTutorialReward Get (int _id) {
            if (!Inited) {
                Init ();
            }
             try
            {
                TableTutorialReward config;
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
        public List<TableTutorialReward> GetAll()
        {
            if (!Inited)
            {
                Init();
            }
            return Items;
        }
        public List<TableTutorialReward> FindAll(Predicate<TableTutorialReward> match)
        {
            if (!Inited)
            {
                Init();
            }
            return Items.FindAll(match);
        }
    }
}