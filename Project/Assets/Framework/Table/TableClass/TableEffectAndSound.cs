using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using Framework.Asset;
namespace Framework.Tables
{
    public class TableEffectAndSound {
        private static int curIndex = 0;
        private static string FileName = "Table/EffectAndSound";
        private static Dictionary<int, TableEffectAndSound> dic = new Dictionary<int, TableEffectAndSound> ();
        private static List<TableEffectAndSound> Items = new List<TableEffectAndSound>();
        private static TableEffectAndSound First;
        private static bool Inited = false;
        public TableEffectAndSound Next = null;

		//编号
		public int Id { private set; get; }
		//脚本名字
		public string name { private set; get; }
		//特效名字
		public string effectName { private set; get; }
		//音乐音效id
		public int Sound_ID { private set; get; }
		//声音名字
		public string soundName { private set; get; }
		//描述
		public string description { private set; get; }
		//备注
		public string beizhu { private set; get; }


        private static void AddItem (TableEffectAndSound item) {
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
                TableEffectAndSound preItem = null;
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode data = JSONNode.Parse (tableStr);
                foreach (var itemData in data.Children) {
                    TableEffectAndSound item = new TableEffectAndSound ();
					item.Id = itemData["Id"];
					item.name = itemData["name"];
					item.effectName = itemData["effectName"];
					item.Sound_ID = itemData["Sound_ID"];
					item.soundName = itemData["soundName"];
					item.description = itemData["description"];
					item.beizhu = itemData["beizhu"];


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
              
            TableEffectAndSound preItem = null;
            JSONNode data = JSONNode.Parse (tableStr);
            foreach (var itemData in data.Children) {
                TableEffectAndSound item = new TableEffectAndSound ();
					item.Id = itemData["Id"];
					item.name = itemData["name"];
					item.effectName = itemData["effectName"];
					item.Sound_ID = itemData["Sound_ID"];
					item.soundName = itemData["soundName"];
					item.description = itemData["description"];
					item.beizhu = itemData["beizhu"];


                AddItem (item);
                if (preItem != null) {
                    preItem.Next = item;
                }
                preItem = item;

            }
        }
        public TableEffectAndSound Head () {
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
        public TableEffectAndSound Get (int _id) {
            if (!Inited) {
                Init ();
            }
             try
            {
                TableEffectAndSound config;
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
        public List<TableEffectAndSound> GetAll()
        {
            if (!Inited)
            {
                Init();
            }
            return Items;
        }
        public List<TableEffectAndSound> FindAll(Predicate<TableEffectAndSound> match)
        {
            if (!Inited)
            {
                Init();
            }
            return Items.FindAll(match);
        }
    }
}