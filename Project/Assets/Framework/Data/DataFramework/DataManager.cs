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
            userInfo.Energy = Table.GameConst.default_life;
            userInfo.SetGoodsCount(Table.GoodsId.Coin, Table.GameConst.default_coin);
        }
        //重置测试
        // StorageManager.Inst.GetStorage<StorageUserInfo>().Level = 1;
        //检测数据
	    if (StorageManager.Inst.GetStorage<StorageAccountInfo>().InstallTime <= 0) {
		    StorageManager.Inst.GetStorage<StorageAccountInfo>().InstallTime = SystemClock.Now;
	    }
    }
}