using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using Framework.Asset;
namespace Framework.Tables
{
    public class TableGun {
        private static int curIndex = 0;
        private static string FileName = "Table/Gun";
        private static Dictionary<int, TableGun> dic = new Dictionary<int, TableGun> ();
        private static List<TableGun> Items = new List<TableGun>();
        private static TableGun First;
        private static bool Inited = false;
        public TableGun Next = null;

		//关卡Id
		public int Id { private set; get; }
		//枪Prefab
		public string GunPrefab { private set; get; }
		//枪的类型
		public int GunType { private set; get; }


        private static void AddItem (TableGun item) {
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
                TableGun preItem = null;
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode data = JSONNode.Parse (tableStr);
                foreach (var itemData in data.Children) {
                    TableGun item = new TableGun ();
					item.Id = itemData["Id"];
					item.GunPrefab = itemData["GunPrefab"];
					item.GunType = itemData["GunType"];


                    AddItem (item);
                    if (preItem != null) {
                        preItem.Next = item;
                    }
                    preItem = item;

                }
                Inited = true;
            }
        }
        public TableGun Head () {
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
        public TableGun Get (int _id) {
            if (!Inited) {
                Init ();
            }
             try
            {
                TableGun config;
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
        public List<TableGun> GetAll()
        {
            if (!Inited)
            {
                Init();
            }
            return Items;
        }
        public List<TableGun> FindAll(Predicate<TableGun> match)
        {
            if (!Inited)
            {
                Init();
            }
            return Items.FindAll(match);
        }
    }
}