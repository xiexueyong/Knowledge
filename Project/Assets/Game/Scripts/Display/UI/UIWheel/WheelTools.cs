using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework.Tables;
using System.Linq;
using System;
using Framework.Storage;

public class WheelTools
{
    public enum WheelType
    {
        CommonWheel,
        FreeWheel,
        AdWheel,
        DailyWheel,
        DailyAdWheel
    }
    //免费转盘已经恢复了多长时间
    public static int FreeWheelRecoverTime()
    {
        // int recoverSecond = SystemClock.Now - StorageManager.Inst.GetStorage<StorageUserInfo>().FreeWheelStarTime;
        // return recoverSecond;
        return 0;
    }
    //免费转盘剩余多长时间恢复免费
    public static int FreeWheelLeftTime()
    {
        //免费转盘的剩余时间
        int leftTime = Table.GameConst.FreeWheelRecoverTime - WheelTools.FreeWheelRecoverTime();
        return leftTime;
    }
    public static bool FreeWheelFree()
    {
        //免费转盘的剩余时间
        int leftTime = Table.GameConst.FreeWheelRecoverTime - WheelTools.FreeWheelRecoverTime();
        return leftTime <= 0;
    }

    //是否还有看广告转转盘的次数
    public static bool hasAdWheel()
    {
        // string dayKey = SystemClock.DateKey;
        // int c = StorageManager.Inst.GetStorage<StorageUserInfo>().AdWheelCount[dayKey];
        // return c < Table.GameConst.AdWheelCountOneDay;
        return false;
    }
    //看一次广告转盘
    public static void useAdWheel(int count = 1)
    {
        string dayKey = SystemClock.DateKey;
        // StorageManager.Inst.GetStorage<StorageUserInfo>().AdWheelCount[dayKey] += count;
    }
    //每日转盘是否还有看广告的次数
    public static bool hasDailyAdWheel()
    {
        // string dayKey = SystemClock.DateKey;
        // int c = StorageManager.Inst.GetStorage<StorageUserInfo>().DailyAdWheelCount[dayKey];
        // return c < Table.GameConst.DailyAdWheelCountOneDay;
        return false;
    }
    //每日转盘看一次广告转盘
    public static void useDailyAdWheel(int count = 1)
    {
        string dayKey = SystemClock.DateKey;
        // StorageManager.Inst.GetStorage<StorageUserInfo>().DailyAdWheelCount[dayKey] += count;
    }

    //获取每日转盘当日已看次数
    public static int getDailyAdWheelCount(){
        string dayKey = SystemClock.DateKey;
        // int c = StorageManager.Inst.GetStorage<StorageUserInfo>().DailyAdWheelCount[dayKey];
        // return c;
        return 0;
    }
    //是否还有每日转盘的次数
    public static bool hasDailyWheel()
    {
        string dayKey = SystemClock.DateKey;
        // int c = StorageManager.Inst.GetStorage<StorageUserInfo>().DailyWheel[dayKey];
        // return c < 1;
        return false;
    }
    //看一次每日转盘
    public static void useDailyWheel(int count = 1)
    {
        string dayKey = SystemClock.DateKey;
        // StorageManager.Inst.GetStorage<StorageUserInfo>().DailyWheel[dayKey] += count;
    }

    public static int getAdWheelCount(){
        string dayKey = SystemClock.DateKey;
        // int c = StorageManager.Inst.GetStorage<StorageUserInfo>().AdWheelCount[dayKey];
        // return c;
        return 0;
    }

    public static  void UpdateFreeWheelStarTime()
    {
        // StorageManager.Inst.GetStorage<StorageUserInfo>().FreeWheelStarTime = SystemClock.Now;
    }

    public static List<TableWheels> GetWheel(int wheelId)
    {
        List<TableWheels> wheelItem = Table.Wheels.GetAll().Where(x => x.WheelId == wheelId).ToList();
        return wheelItem;
    }

    public static int getLuckyGrid(List<TableWheels> tableWheels)
    {
        int sum = 0;
        tableWheels.ForEach(x=>sum+=x.Weight);
        float r = UnityEngine.Random.Range(0,1f)*sum;

        int p = 0;
        for (int i = 0;i<tableWheels.Count;i++)
        {
            p += tableWheels[i].Weight;
            if (r <= p)
            {
                return i;
            }
        }
        return tableWheels.Count;
    }
}
