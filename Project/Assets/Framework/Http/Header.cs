using Framework.Asset;
using System;
using System.Collections.Generic;
/***
* Header数据
* */
public class Header
{
    public static string ClientKey = "b3BlbnNzaC1rZXktdjEAAAAABG5vbmUAAAA";

    public Dictionary<string,string> headers;
    public Header(Dictionary<string, string> _headers)
    {
        headers = _headers;
    }
    /// <summary>
    /// appid
    /// time
    /// sign
    /// prvc
    /// session
    /// </summary>
    /// 默认的Header
    /// <returns></returns>
    public static Header GetHeader()
    {
        Dictionary<string, string> headerDic = new Dictionary<string, string>();
        string timeStr = ((int)SystemClock.Now).ToString();
        headerDic["appid"] = GameConfig.Inst.Appid;
        headerDic["time"] = timeStr;
        headerDic["pver"] = GameConfig.Inst.Pver;
        headerDic["sign"] = AssetUtils.EncryptWithMD5(GameConfig.Inst.Appid + timeStr + ClientKey);

        //if (string.IsNullOrEmpty(LoginManager.Instance.Session))
        //{
        //    //headerDic["sign"] = AssetUtils.EncryptWithMD5(GameConfig.Instance.Appid + timeStr + ClientKey);
        //}
        //else
        //{
        //    headerDic["session"] = LoginManager.Instance.Session;
        //   // headerDic["sign"] = AssetUtils.EncryptWithMD5(GameConfig.Instance.Appid + timeStr + ClientKey + Session);
        //}

        return new Header(headerDic);

    }
    /// <summary>
    /// 自定义的Header
    /// </summary>
    /// <param name="_headers"></param>
    /// <returns></returns>
    public static Header GetHeader(Dictionary<string, string> _headers)
    {
        return new Header(_headers);

    }
}