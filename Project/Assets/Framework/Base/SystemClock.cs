using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventUtil;
using System.Text;

public class SystemClock : D_MonoSingleton<SystemClock>
{
    private static float _sumTime = 0;
    private static int _serverTime = 0;
    private static float _acquire_ServerTime_time = 0;
    private static Action OnSecond;

    public static int Now
    {
        get
        {
            return (int)(DateTime.UtcNow - StartTime).TotalSeconds;
        }
    }

    public static int Get0ClockOfUtcTime(DateTime utcDateTime)
    {
        return (int)(new DateTime(utcDateTime.Year, utcDateTime.Month, utcDateTime.Day, 0, 0, 0, DateTimeKind.Utc) - StartTime).TotalSeconds;
    }

    public static int ServerTime
    {
        get
        {
            if (_acquire_ServerTime_time > 0)
                _serverTime = ServerDataManager.Inst.heartBeatInfo.serverTime + (int) (Time.realtimeSinceStartup - _acquire_ServerTime_time);
            return _serverTime;
        }
    }
    public static string DateKey
    {
        get
        {
            int year = DateTime.UtcNow.Year;
            int month = DateTime.UtcNow.Month;
            int day = DateTime.UtcNow.Day;
            return string.Format("{0}_{1}_{2}", year, month, day);
        }
    }
    public static void AddListener(Action action)
    {
        OnSecond += action;
    }
    public static void RemoveListener(Action action)
    {
        OnSecond -= action;
    }
    public void Init()
    {
        EventDispatcher.AddEventListener(EventKey.HeartBeat, OnHeartBeat);
    }
    private static void OnHeartBeat()
    {
        _serverTime = ServerDataManager.Inst.heartBeatInfo.serverTime;
        _acquire_ServerTime_time = Time.realtimeSinceStartup;
    }

    public static DateTime StartTime = new DateTime(1970, 1, 1,0,0,0,DateTimeKind.Utc);


    public static DateTime SecondToDateTime(double second)
    {
        long ticks = (long)(second * 10000000);
        return StartTime + new TimeSpan(ticks);
    }
	public int day(){
		return DateTime.Now.Day;
	}
    public static bool SameDay(DateTime dt1, DateTime dt2)
    {
        if (dt1 != null && dt2 != null && dt1.Year == dt2.Year && dt1.Month == dt2.Month && dt1.Day == dt2.Day)
        {
            return true;
        }
        return false;
    }
    public static bool SameDay(int dt1, int dt2)
    {
        return SameDay(SecondToDateTime(dt1), SecondToDateTime(dt2));
    }
    public static int DaySpan(DateTime endTime, DateTime startTime)
    {
        TimeSpan sp = endTime.Subtract(startTime);
        return sp.Days;
    }

    void Update ()
	{
        _sumTime += Time.fixedDeltaTime;

        while (_sumTime >= 1.0f)
	    {
            _sumTime -= 1.0f;
            OnSecond?.Invoke();
        }
	}

}
