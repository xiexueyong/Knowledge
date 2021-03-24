using Framework.Tables;
using UnityEngine;


public static class LanguageTool
{
    public enum LangType
    {
        NONE = 0,
        EN = 2,
        CHN = 3
    }
    private static LangType _LangType = LangType.NONE;
    public static void SetLangType(LangType langType)
    {
        if (langType == LangType.NONE)
            return;
        PlayerPrefs.SetInt("DefaultLang", (int)langType);
        _LangType = langType;
    }
  
    public static LangType GetLangType()
    {

        if (_LangType == LangType.NONE)
        {
            LangType _defaultLanguage = (LangType)PlayerPrefs.GetInt("DefaultLang",0);
            if (_defaultLanguage == LangType.NONE)
            {
                if (Application.systemLanguage == SystemLanguage.Chinese 
                    || Application.systemLanguage == SystemLanguage.ChineseSimplified
                    || Application.systemLanguage == SystemLanguage.ChineseTraditional)
                {
                    _LangType = LangType.CHN;
                }
                else
                {
                    _LangType = LangType.EN;
                }
            }
            else
            {
                _LangType = _defaultLanguage;
            }
        }

        return _LangType;

    }
    public static string country2Lang(string country)
    {
        if (country == "Chinese")
        {
            return "CN";
        }

        return "US";
    }
    public static string Get(string id, LangType langType = LangType.NONE)
    {
        if (id == null)
        {
            return "";
        }

        LangType _langType;
        if (langType == LangType.NONE)
            _langType = GetLangType();
        else
            _langType = langType;
        var temp = Table.Language.Get(id);
        if (temp == null)
            return "id:"+id;

        string txt;
        switch (_langType)
        {
            case LangType.EN:
                txt = Table.Language.Get(id).EN;
                break;
            case LangType.CHN:
                txt = Table.Language.Get(id).CHA;
                break;
            default:
                txt = "No LangType";
                break;
        }

        txt = ReplaceNames(txt);
        return txt;
    }
    /// <summary>
    /// 角色 [c:id] 任务[t:id] 家具[i:id] 玩家[p:id]
    /// </summary>
    /// <param name="txt"></param>
    /// <returns></returns>
    //private static string ReplaceNames(string txt, int len = -1)
    private static string ReplaceNames(string txt, int lastIndex = int.MinValue)
    {
        if (lastIndex == -1)
            return txt;
        if (lastIndex == int.MinValue)
            lastIndex = txt.Length - 1;
        if (lastIndex == -1)
            return txt;
        
        int Lindex = txt.LastIndexOf('[', lastIndex);
        if (Lindex == -1)
            return txt;
        int Rindex = txt.IndexOf(']', Lindex);
        if (Rindex - Lindex + 1 < 5)
            return ReplaceNames(txt, Lindex - 1);

        string tempS = txt.Substring(Lindex, Rindex - Lindex + 1);

        string temp = null;
        string id = tempS.Substring(3, tempS.Length - 4);

       //if (tempS.StartsWith("[t"))
        //{
        //    TableTask task = Table.Task.Get(int.Parse(id));
        //    if (task != null)
        //        temp = task.Id.ToString();
        //}
        //else if (tempS.StartsWith("[i"))
        //{
        //    TableMapObj mapObj = Table.MapObj.Get(id);
        //    if (mapObj != null)
        //        temp = mapObj.Id.ToString();
        //}
        //else if (tempS.StartsWith("[p"))
        //{
        //    if (id == "0")
        //    {
        //        temp = DataManager.Inst.userInfo.NickName;
        //    }
        //}

        //if (temp != null)
            //txt = txt.Replace(tempS, Get(temp));
        return ReplaceNames(txt, Lindex - 1);
    }
}
