using System;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Framework.Asset;
using Framework.Storage;
using Framework.Tables;


public class CommonUtil
{

    private const int m_DaySeconds = 86400;
    static bool isAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
                     return true;
#else
        return false;
#endif
    }

    /// <summary>
    /// 比较两个版本的大小，版本格式为 3.0.9
    /// </summary>
    /// <param name="v1"></param>
    /// <param name="v2"></param>
    /// <returns>v1大返回1，v1小返回-1，v1==v2 返回0</returns>
    public static int CompareVersion(string v1,string v2)
    {

        string[] v1Nums = v1.Split('.');
        string[] v2Nums = v2.Split('.');
        for (int i=0;i<3;i++)
        {
            int v1num = int.Parse(v1Nums[i]);
            int v2num = int.Parse(v2Nums[i]);
            if (v1num > v2num)
            {
                return 1;
            }
            else if(v1num < v2num)
            {
                return -1;
            }
        }
        return 0;
    }

    public static string SubAssetVersionIncrease(string Version)
    {
        string[] verNums = Version.Split('.');
        if (verNums.Length >= 3)
        {
            int sunAssetVersion = int.Parse(verNums[2]);
            return string.Format("{0}.{1}.{2}", verNums[0], verNums[1], ++sunAssetVersion);
        }
        else
        {
            DebugUtil.LogError("Asset version formate error ");
        }
        return Version;
    }

    public static string ParseId(string id)
    {
        if (id.Contains(":")) {
            string[] t = id.Split(':');
            return t[0];
        }

        return id;
    }

    public static string SecondToTimeFormat(int second)
    {
        //if (second > m_DaySeconds) {
        //    return string.Format("{0} {1}", Mathf.Ceil(second * 1.0f / m_DaySeconds), "day");
        //}
        if (second <= 0) {
            return "00:00";
        }
        int h = (int) (second / 3600);
        int m = (int) ((second - 3600 * h) / 60);
        int s = second - h * 3600 - m * 60;

        if (h == 0) {
            return fillFormat(m) + ":" + fillFormat(s);
        }
        else {
            return fillFormat(h) + ":" + fillFormat(m) + ":" + fillFormat(s);
        }
    }

    /// <summary>
    /// 以 h m s 格式输出时间
    /// </summary>
    /// <param name="secondTime"></param>
    /// <returns></returns>
    public static string TimeFormat(int secondTime)
    {
        string timeFormat = "";
        int hour = secondTime / 3600;
        int minute = (secondTime % 3600) / 60;
        int second = secondTime % 60;

        if (hour > 0) {
            timeFormat += hour + "H";
        }

        if (minute > 0) {
            timeFormat += minute + "M";
        }

        if (second > 0) {
            if (second < 10) {
                timeFormat += "0" + second + "S";
            }
            else {
                timeFormat += second + "S";
            }
        }

        return timeFormat;
    }

    private static readonly DateTime StampStartDateTime = new DateTime(1970, 1, 1);

    /// <summary>
    /// 获取时间戳,单位秒
    /// </summary>
    /// <returns></returns>
    public static int GetTimeStamp(bool isServerTime = false)
    {
        if (!isServerTime)
        {
            TimeSpan timespan = DateTime.UtcNow - StampStartDateTime;
            return (int) timespan.TotalSeconds;
        }

        return SystemClock.ServerTime;
    }

    private static string fillFormat(int value)
    {
        if (value < 10) {
            return "0" + value;
        }
        else {
            return value.ToString();
        }
    }


    public static string ToBase64String(string value)
    {
        if (value == null || value == "") {
            return "";
        }

        byte[] bytes = Encoding.UTF8.GetBytes(value);
        return Convert.ToBase64String(bytes);
    }

    public static byte[] ToBytes(string value)
    {
        if (value == null || value == "") {
            return null;
        }

        return Encoding.UTF8.GetBytes(value);
    }

    public static string UnBase64String(string value)
    {
        if (value == null || value == "") {
            return "";
        }

        byte[] bytes = Convert.FromBase64String(value);
        return Encoding.UTF8.GetString(bytes);
    }

    /// <summary>
    /// 二次贝塞尔曲线
    /// </summary>
    /// <param name="point_1"></param>
    /// <param name="point_2"></param>
    /// <param name="point_3"></param>
    /// <param name="vertexCount"></param>
    /// <returns></returns>
    public static Vector3[] GetBezierCurveWithThreePoints(Vector3 point_1, Vector3 point_2, Vector3 point_3, int vertexCount)
    {
        List<Vector3> pointList = new List<Vector3>();
        for (float ratio = 0; ratio <= 1; ratio += 1.0f / vertexCount) {
            Vector3 tangentLineVertex1 = Vector3.Lerp(point_1, point_2, ratio);
            Vector3 tangentLineVertex2 = Vector3.Lerp(point_2, point_3, ratio);
            Vector3 bezierPoint = Vector3.Lerp(tangentLineVertex1, tangentLineVertex2, ratio);
            pointList.Add(bezierPoint);
        }

        pointList.Add(point_3);
        return pointList.ToArray();
    }

    public static void DelayCall(float time, Action callback)
    {
        CoroutineManager.Inst.StartCoroutine(DelayCallIEnumerator(time, () =>
        {
            if (null != callback) {
                callback.Invoke();
            }
        }));
    }

    /// <summary>
    /// 延时函数携程
    /// </summary>
    /// <param name="time"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    private static IEnumerator DelayCallIEnumerator(float time, Action callback)
    {
        yield return new WaitForSeconds(time);
        if (null != callback) {
            callback.Invoke();
        }
    }

    /// <summary>
    ///  获取道具精灵
    ///  201:1:1008003
    ///  1:100
    /// </summary>
    /// <param name="goodsInfo"> </param>
    /// <returns></returns>
    public static Sprite GetItemSprite(Triangle<int,int,int> goodsInfo)
    {
        string path = null;
        int goodsId = goodsInfo.V1;
        int count = goodsInfo.V2;
        int itemId = goodsInfo.V3;
        return Res.LoadResource<Sprite>(path);
    }

}


public class StringLengthComparer : System.Collections.IComparer
{
    public int Compare(object x, object y)
    {
        if (x == null)
            return 1;
        if (y == null)
            return -1;

        string s1 = (string) x;
        string s2 = (string) y;

        if (s1.Length > s2.Length)
            return -1;
        if (s1.Length < s2.Length)
            return 1;

        return 0;
    }
}