using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Utils;
using Framework.Asset;
namespace Framework.Tables
{
    public class TableMatchElement {
        private static int curIndex = 0;
        private static string FileName = "Table/MatchElement";
        private static Dictionary<int, TableMatchElement> dic = new Dictionary<int, TableMatchElement> ();
        private static List<TableMatchElement> Items = new List<TableMatchElement>();
        private static TableMatchElement First;
        private static bool Inited = false;
        public TableMatchElement Next = null;

		//编号
		public int Id { private set; get; }
		//说明
		public string annotation { private set; get; }
		//类型名称
		public string name { private set; get; }
		//目标图标
		public string goalIcon { private set; get; }
		//图标
		public string icon { private set; get; }
		//尺寸
		public string size { private set; get; }
		//销毁得分
		public int destroyScore { private set; get; }
		//收集得分
		public int collectScore { private set; get; }
		//销毁类型
		public int destroyType { private set; get; }
		//内容类型
		public string eType { private set; get; }
		//层级名称
		public string layerName { private set; get; }
		//层级
		public int layer { private set; get; }
		//Goal的prefab name
		public string collection { private set; get; }
		//prefab名称
		public string prefabName { private set; get; }
		//目标编辑器
		public string goalEditor { private set; get; }
		//slot内容编辑器
		public string slotEditor { private set; get; }
		//元素内部权重编辑器
		public string EleInnerWeightEditor { private set; get; }
		//元素内部权重数据类型
		public string EleInnerWeightGroupType { private set; get; }


        private static void AddItem (TableMatchElement item) {
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
                TableMatchElement preItem = null;
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode data = JSONNode.Parse (tableStr);
                foreach (var itemData in data.Children) {
                    TableMatchElement item = new TableMatchElement ();
					item.Id = itemData["Id"];
					item.annotation = itemData["annotation"];
					item.name = itemData["name"];
					item.goalIcon = itemData["goalIcon"];
					item.icon = itemData["icon"];
					item.size = itemData["size"];
					item.destroyScore = itemData["destroyScore"];
					item.collectScore = itemData["collectScore"];
					item.destroyType = itemData["destroyType"];
					item.eType = itemData["eType"];
					item.layerName = itemData["layerName"];
					item.layer = itemData["layer"];
					item.collection = itemData["collection"];
					item.prefabName = itemData["prefabName"];
					item.goalEditor = itemData["goalEditor"];
					item.slotEditor = itemData["slotEditor"];
					item.EleInnerWeightEditor = itemData["EleInnerWeightEditor"];
					item.EleInnerWeightGroupType = itemData["EleInnerWeightGroupType"];


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
              
            TableMatchElement preItem = null;
            JSONNode data = JSONNode.Parse (tableStr);
            foreach (var itemData in data.Children) {
                TableMatchElement item = new TableMatchElement ();
					item.Id = itemData["Id"];
					item.annotation = itemData["annotation"];
					item.name = itemData["name"];
					item.goalIcon = itemData["goalIcon"];
					item.icon = itemData["icon"];
					item.size = itemData["size"];
					item.destroyScore = itemData["destroyScore"];
					item.collectScore = itemData["collectScore"];
					item.destroyType = itemData["destroyType"];
					item.eType = itemData["eType"];
					item.layerName = itemData["layerName"];
					item.layer = itemData["layer"];
					item.collection = itemData["collection"];
					item.prefabName = itemData["prefabName"];
					item.goalEditor = itemData["goalEditor"];
					item.slotEditor = itemData["slotEditor"];
					item.EleInnerWeightEditor = itemData["EleInnerWeightEditor"];
					item.EleInnerWeightGroupType = itemData["EleInnerWeightGroupType"];


                AddItem (item);
                if (preItem != null) {
                    preItem.Next = item;
                }
                preItem = item;

            }
        }
        public TableMatchElement Head () {
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
        public TableMatchElement Get (int _id) {
            if (!Inited) {
                Init ();
            }
             try
            {
                TableMatchElement config;
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
        public List<TableMatchElement> GetAll()
        {
            if (!Inited)
            {
                Init();
            }
            return Items;
        }
        public List<TableMatchElement> FindAll(Predicate<TableMatchElement> match)
        {
            if (!Inited)
            {
                Init();
            }
            return Items.FindAll(match);
        }
    }
}