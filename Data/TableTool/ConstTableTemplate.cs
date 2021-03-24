using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Asset;
namespace Framework.Tables {
    public class TableClassName
    {
        private static string FileName = "Table/#DataFile";
        private static bool Inited = false;

        public TableClassName()
		{
		}
        public void Clear()
        {
            Inited = false;
        }

#properties

        public void Init () {
            if (!Table.Inited)
            {
                throw new Exception("Tabele has not been initialised,it is intialised in GameController");
            }
            if (!Inited) {
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode itemData = JSONNode.Parse (tableStr);
#setValues
                Inited = true;
            }
        }

        public void Init (string tableStr) {


                JSONNode itemData = JSONNode.Parse (tableStr);
#setValues
                Inited = true;
        }


    }
}