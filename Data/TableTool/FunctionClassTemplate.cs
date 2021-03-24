using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public static class #TableClassName
{
#functions


    private static void AddDicAsJsonToDic<T1,T2>(Dictionary<string, string> dic, string key, Dictionary<T1, T2> dic2)
    {
        dic.Add(key, JsonConvert.SerializeObject(dic2));
    }
}