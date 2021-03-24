using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using Framework.Asset;
namespace Framework.Tables {
    public class TableSoundDialog
    {
        private static string FileName = "Table/SoundDialog";
        private static bool Inited = false;

        public TableSoundDialog()
		{
		}
        public void Clear()
        {
            Inited = false;
        }

        /// <summary>
        /// 通用按钮
        /// </summary>
        private string _sfx_common_btn;
        public string sfx_common_btn
        {
            private set
            {
                _sfx_common_btn = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_common_btn;
            }
        }
        /// <summary>
        /// 通用界面弹出
        /// </summary>
        private string _sfx_common_panel_show;
        public string sfx_common_panel_show
        {
            private set
            {
                _sfx_common_panel_show = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_common_panel_show;
            }
        }
        /// <summary>
        /// 爸爸高兴
        /// </summary>
        private string _sfx_dialog_papa_happy;
        public string sfx_dialog_papa_happy
        {
            private set
            {
                _sfx_dialog_papa_happy = value;
            }
            get
            {
                if (!Inited)
                    Init();
                return _sfx_dialog_papa_happy;
            }
        }


        public void Init () {
            if (!Table.Inited)
            {
                throw new Exception("Tabele has not been initialised,it is intialised in GameController");
            }
            if (!Inited) {
                string tableStr = TableTool.GetTxt(FileName);
                JSONNode itemData = JSONNode.Parse (tableStr);
				sfx_common_btn = itemData["sfx_common_btn"];
				sfx_common_panel_show = itemData["sfx_common_panel_show"];
				sfx_dialog_papa_happy = itemData["sfx_dialog_papa_happy"];

                Inited = true;
            }
        }

        public void Init (string tableStr) {


                JSONNode itemData = JSONNode.Parse (tableStr);
				sfx_common_btn = itemData["sfx_common_btn"];
				sfx_common_panel_show = itemData["sfx_common_panel_show"];
				sfx_dialog_papa_happy = itemData["sfx_dialog_papa_happy"];

                Inited = true;
        }


    }
}