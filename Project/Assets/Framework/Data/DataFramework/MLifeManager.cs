using System;
using System.Collections;
using System.Collections.Generic;
using Framework.Storage;
using UnityEngine;

public class MLifeManager : D_MonoSingleton<MLifeManager>
{
    /**
     *距离生成life的时间 
     */
    public int RemainRecoverSecond;
    public int RemainInfiniteSecond;

    public MLife life;
    protected override void OnAwake()
    {
        MLife.LifeRecoverTime = Table.GameConst.life_time;
        MLife.LifeMaxCount = Table.GameConst.default_life;
        life = new MLife();
        SystemClock.AddListener(PassOneSecond);
        //StartCountDown();
    }
    public void StartCountDown()
    {
        if (MLifeManager.Inst.life.CountDownTime < (int)SystemClock.Now)
        {
            life.CountDownTime = (int)SystemClock.Now;
        }
    }
    public void ResetCountDown()
    {
        life.CountDownTime = (int) SystemClock.Now;
    }

    private void PassOneSecond()
    {
        if (InfiniteLife())
        {
            //无限体力
            RemainInfiniteSecond = (int)(life.InfiniteLifeTimeSpan -(SystemClock.Now - life.InfiniteLifeStartTime));
        }
        else
        {
            //非无限
            if (DataManager.Inst.userInfo.Energy >= MLife.LifeMaxCount)
            {
                return;
            }
            double elapsedSecond = SystemClock.Now - life.CountDownTime;

            if (elapsedSecond < 0)
            {
                //elapsedSecond<0 说明调过时间
                RemainRecoverSecond = MLife.LifeRecoverTime - (int)(elapsedSecond);
            }
            else
            {
                int newLife = (int)(elapsedSecond / MLife.LifeRecoverTime);
                RemainRecoverSecond = MLife.LifeRecoverTime - (int)(elapsedSecond - newLife * MLife.LifeRecoverTime);
                if (newLife > 0)
                {
                    int maxAdd = MLife.LifeMaxCount - DataManager.Inst.userInfo.Energy;
                    newLife = newLife > maxAdd ? maxAdd : newLife;
                    life.CountDownTime = life.CountDownTime + newLife * MLife.LifeRecoverTime;
                    DataManager.Inst.userInfo.ChangeEnergy(newLife);

                }
            }
        }
       
    }

    public bool InfiniteLife()
    {
        return life.InfiniteLife();
    }

    public int GetCurCD()
    {
        return RemainRecoverSecond;
    }
	public bool LifeFull()
	{
		return DataManager.Inst.userInfo.Energy >= MLife.LifeMaxCount;
	}
    //public int GetLifeCount()
    //{
    //    if (life != null)
    //    {
    //        return life.LifeCount;
    //    }
    //    return 0;
    //}
}

public class MLife
{
    public static int LifeMaxCount = 5;//Table.GameConst.default_life;
    public static int LifeRecoverTime = 1800;//Table.GameConst.life_time;//1800


    /**
    * 无限生命的开始时间
    */
    public static string InfiniteLifeStartTimeKey = "InfiniteLifeStartTime";
    public int InfiniteLifeStartTime
    {
        get
        {
            int st = StorageManager.Inst.GetStorage<StorageUserInfo>().InfiniteLifeStartTime;
            return st;
        }
        set
        {
            StorageManager.Inst.GetStorage<StorageUserInfo>().InfiniteLifeStartTime = value;
        }
    }
    /**
     *无限生命的时长
     */
    public static string InfiniteLifeTimeSpanKey = "InfiniteLifeTimeSpanKey";
    public int InfiniteLifeTimeSpan
    {
        get {
            int sp = StorageManager.Inst.GetStorage<StorageUserInfo>().InfiniteLifeTimeSpan;
           return sp;
        }
        set {
            if (value <= 0)
            {
                value = 0;
            }
            StorageManager.Inst.GetStorage<StorageUserInfo>().InfiniteLifeTimeSpan = value; //DecorationProj.GlobalData.storageDeco.CommonData.GoodsData;
        }
    }
    public void AddInfiniteLife(int second)
    {
        if (InfiniteLife())
        {
            InfiniteLifeTimeSpan += second;
        }
        else
        {
            InfiniteLifeTimeSpan = second;
            StorageManager.Inst.GetStorage<StorageUserInfo>().InfiniteLifeStartTime = (int)SystemClock.Now;
            if (DataManager.Inst.userInfo.Energy < LifeMaxCount)
            {
                DataManager.Inst.userInfo.ChangeEnergy(LifeMaxCount- DataManager.Inst.userInfo.Energy);
            }

        }
    }

    public bool InfiniteLife()
    {
        if (InfiniteLifeStartTime < SystemClock.Now &&
            InfiniteLifeStartTime + InfiniteLifeTimeSpan >= SystemClock.Now)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /**
    * 倒计时的时间
    */
    public int CountDownTime
    {
        get
        {
            int cd = StorageManager.Inst.GetStorage<StorageUserInfo>().CountDownTime;
            if (cd <= 0f)
            {
                Debug.Log("=======================get cd: 0");
                CountDownTime = (int)SystemClock.Now;
                return CountDownTime;
            }
            else
            {
				return cd;
            }
        }
        set
        {
            StorageManager.Inst.GetStorage<StorageUserInfo>().CountDownTime = value;
            Debug.Log("=======================set cd: "+value.ToString());
        }
    }
}



































//if (pause)
//{
//    //Debug.Log("life.waitSecond: " + life.waitSecond);
//    if (life.waitSecond > 0 && life.count == life.default_life - 1)
//    {
//            string textTitle = DialogueAssisstant.main.GetUIText(2101140, CacheManager.currentLanguage);//"lifes full!"
//            string textDes = DialogueAssisstant.main.GetUIText(2101141, CacheManager.currentLanguage);//"Your hearts is full, Please come back and continue play."
//            LocalNotificationAndroid.SendNotification(1, new TimeSpan(0, 0, life.waitSecond), textTitle , textDes, new Color32(0xff, 0x44, 0x44, 255));
//    }
//}
//else
//{
//    //Debug.Log("ClearNotifications");
//    LocalNotificationAndroid.ClearNotifications();
//}