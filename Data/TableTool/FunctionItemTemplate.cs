#annotation
    public static void #FunName(#arguments)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();
        try
        {
#dataItem
        }
        catch(Exception e)
        {
            DebugUtil.LogError("#eventName:"+e.ToString());
        }
        AnalyticsTool.Analytics("#eventName", dic);
    }
