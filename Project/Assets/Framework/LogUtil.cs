using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LogUtil
{
    public static void Log(string info)
    {
        Debug.Log(info);
    }


    public static void LogError(string info)
    {
        Debug.LogError(info);
    }

    public static void LogWarning(string info)
    {
        Debug.LogWarning(info);
    }

    public static void Exception(Exception exception, UnityEngine.Object context = null)
    {
        if (context == null)
        {
            UnityEngine.Debug.LogException(exception);
        }
        else
        {
            UnityEngine.Debug.LogException(exception, context);
        }
    }
}
