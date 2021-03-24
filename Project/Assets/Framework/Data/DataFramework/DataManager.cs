using System;
using System.Collections.Generic;
using System.Linq;
using EventUtil;
using FFF.Scripts.Framework.Data.DataFramework;
using Framework.Tables;
using UnityEngine;
using Newtonsoft.Json;
using Framework.Storage;

public class DataManager : D_MonoSingleton<DataManager>
{
    public bool SimulateChinScreen = false;

    /// <summary>
	/// 用户相关信息
    /// </summary>
    private UserInfo _userInfo;
    public UserInfo userInfo
    {
        get {
			if (_userInfo == null) {
				_userInfo = new UserInfo ();
				_userInfo.Initialize ();
			}
            return _userInfo;
        }
    }

    /// <summary>
    /// 关卡数据变化信息（关卡内使用了什么道具、买了多少次步数、看了多少广告等）
    /// </summary>
    private ChangedDataInLevel _changedDataInLevel;
    public ChangedDataInLevel ChangedDataInLevel
    {
        get
        {
            if (_changedDataInLevel == null)
            {
                _changedDataInLevel = new ChangedDataInLevel();
                _changedDataInLevel.Initialize();
            }
            return _changedDataInLevel;
        }
    }

    public void SetDefaultValue()
    {
        if (!StorageManager.Inst.GetStorage<StorageUserInfo>().HasSetDefault)
        {
            StorageManager.Inst.GetStorage<StorageUserInfo>().HasSetDefault = true;
            StorageManager.Inst.GetStorage<StorageUserInfo>().Level = 1;
            //StorageManager.Inst.GetStorage<StorageUserInfo>().HeadIconID = Table.GameConst.defaultHeadIcon;
            //StorageManager.Inst.GetStorage<StorageUserInfo>().NickName = LanguageTool.Get("name_1");
            userInfo.SetGoodsCount(Table.GoodsId.Hammer, 0);
            userInfo.SetGoodsCount(Table.GoodsId.Cropper, 0);
            userInfo.SetGoodsCount(Table.GoodsId.Weight, 0);
            userInfo.SetGoodsCount(Table.GoodsId.Tornado, 0);

            userInfo.SetGoodsCount(Table.GoodsId.Rocket, 0);
            userInfo.SetGoodsCount(Table.GoodsId.Bomb, 0);
            userInfo.SetGoodsCount(Table.GoodsId.RainbowBall, 0);
            userInfo.Energy = Table.GameConst.default_life;
            userInfo.SetGoodsCount(Table.GoodsId.Coin, Table.GameConst.default_coin);

            StorageManager.Inst.GetStorage<StorageUserInfo>().task_chapter_id = 1;
            StorageManager.Inst.GetStorage<StorageUserInfo>().Furniture["wall"] = "dirty_wall";
            StorageManager.Inst.GetStorage<StorageUserInfo>().Furniture["dirty_desk"] = "dirty_desk";
            StorageManager.Inst.GetStorage<StorageUserInfo>().Furniture["dirty_table"] = "dirty_table";
            StorageManager.Inst.GetStorage<StorageUserInfo>().Furniture["floor"] = "dirty_floor";
            StorageManager.Inst.GetStorage<StorageUserInfo>().Furniture["desk_diary"] = "desk_diary";
            //TableBI.Goods(new Dictionary<string, string>()
            //    {
            //        { "Action", "1"}
            //    }, Table.GoodsId.Coin,
            //    StorageManager.Inst.GetStorage<StorageUserInfo>().GoodsCount[Table.GoodsId.Coin], 
            //    Table.GameConst.defaultCoin);
            //TableBI.Goods(new Dictionary<string, string>()
            //    {
            //        { "Action", "1"}
            //    }, Table.GoodsId.MultiHint,
            //    StorageManager.Inst.GetStorage<StorageUserInfo>().GoodsCount[Table.GoodsId.MultiHint], 
            //    Table.GameConst.defaultMultiHint);

            //TableBI.Goods(new Dictionary<string, string>()
            //    {
            //        { "Action", "1"}
            //    }, Table.GoodsId.RandomHint,
            //    StorageManager.Inst.GetStorage<StorageUserInfo>().GoodsCount[Table.GoodsId.RandomHint],
            //    Table.GameConst.defaultRandomHint);

            //TableBI.Goods(new Dictionary<string, string>()
            //    {
            //        { "Action", "1"}
            //    }, Table.GoodsId.AppointedHint,
            //    StorageManager.Inst.GetStorage<StorageUserInfo>().GoodsCount[Table.GoodsId.AppointedHint],
            //    Table.GameConst.defaultAppointedHint);
            //StorageManager.Inst.GetStorage<StorageAccountInfo>().DevId = Framework.DeviceHelper.GetDeviceId();
            //StorageManager.Inst.GetStorage<StorageAccountInfo>().StorageId = SystemClock.Now;
            //StorageManager.Inst.GetStorage<StorageAccountInfo>().InstallTime = SystemClock.Now;
        }

        if (StorageManager.Inst.GetStorage<StorageUserInfo>().task_chapter_id <= 3)
        {
            StorageManager.Inst.GetStorage<StorageUserInfo>().Furniture["wall"] = "dirty_wall";
            StorageManager.Inst.GetStorage<StorageUserInfo>().Furniture["dirty_desk"] = "dirty_desk";
            StorageManager.Inst.GetStorage<StorageUserInfo>().Furniture["dirty_table"] = "dirty_table";
            StorageManager.Inst.GetStorage<StorageUserInfo>().Furniture["floor"] = "dirty_floor";
            StorageManager.Inst.GetStorage<StorageUserInfo>().Furniture["desk_diary"] = "desk_diary";
        }
        //重置测试
        // StorageManager.Inst.GetStorage<StorageUserInfo>().Furniture.Clear();
        // StorageManager.Inst.GetStorage<StorageUserInfo>().Level = 1;
        // StorageManager.Inst.GetStorage<StorageUserInfo>().task_chapter_id = 1;
        // StorageManager.Inst.GetStorage<StorageUserInfo>().Furniture["wall"] = "dirty_wall";
        // StorageManager.Inst.GetStorage<StorageUserInfo>().Furniture["dirty_desk"] = "dirty_desk";
        // StorageManager.Inst.GetStorage<StorageUserInfo>().Furniture["dirty_table"] = "dirty_table";
        // StorageManager.Inst.GetStorage<StorageUserInfo>().Furniture["floor"] = "dirty_floor";
        // StorageManager.Inst.GetStorage<StorageUserInfo>().Furniture["desk_diary"] = "desk_diary";
        
        //检测数据
        {
	        if (StorageManager.Inst.GetStorage<StorageAccountInfo>().InstallTime <= 0) {
		        StorageManager.Inst.GetStorage<StorageAccountInfo>().InstallTime = SystemClock.Now;
	        }
        }
    }
}