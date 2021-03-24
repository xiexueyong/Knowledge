using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using Framework.Asset;
namespace Framework.Tables
{
    public class TableGift {
        private static int curIndex = 0;
        private static string FileName = "Table/Gift";
        private static Dictionary<int, TableGift> dic = new Dictionary<int, TableGift> ();
        private static List<TableGift> Items = new List<TableGift>();
        private static TableGift First;
        private static bool Inited = false;
        public TableGift Next = null;

		//礼包Id
		public int Id { private set; get; }
		//礼包Icon
		public string GiftIcon { private set; get; }
		//礼包名称
		public string GiftName { private set; get; }
		//奖励道具及数量
		public List<Triangle<int,int,int>> GiftGoods { private set; get; }
		//随机奖励道具及数量
		public int[] RandomGiftGoods { private set; get; }


        private static void AddItem (TableGift item) {
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
                TableGift preItem = null;
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode data = JSONNode.Parse (tableStr);
                foreach (var itemData in data.Children) {
                    TableGift item = new TableGift ();
					item.Id = itemData["Id"];
					item.GiftIcon = itemData["GiftIcon"];
					item.GiftName = itemData["GiftName"];
					item.GiftGoods = Table.string2List_Triangle_int_int_int(itemData["GiftGoods"]);
					item.RandomGiftGoods = Table.string2ArrayInt(itemData["RandomGiftGoods"]);


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
              
            TableGift preItem = null;
            JSONNode data = JSONNode.Parse (tableStr);
            foreach (var itemData in data.Children) {
                TableGift item = new TableGift ();
					item.Id = itemData["Id"];
					item.GiftIcon = itemData["GiftIcon"];
					item.GiftName = itemData["GiftName"];
					item.GiftGoods = Table.string2List_Triangle_int_int_int(itemData["GiftGoods"]);
					item.RandomGiftGoods = Table.string2ArrayInt(itemData["RandomGiftGoods"]);


                AddItem (item);
                if (preItem != null) {
                    preItem.Next = item;
                }
                preItem = item;

            }
        }
        public TableGift Head () {
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
        public TableGift Get (int _id) {
            if (!Inited) {
                Init ();
            }
             try
            {
                TableGift config;
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
        public List<TableGift> GetAll()
        {
            if (!Inited)
            {
                Init();
            }
            return Items;
        }
        public List<TableGift> FindAll(Predicate<TableGift> match)
        {
            if (!Inited)
            {
                Init();
            }
            return Items.FindAll(match);
        }
    }
}