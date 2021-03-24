public static class EliteUtil
{
    public static string TrimString(string source,int maxLength,string trimSign ="")
    {
        if (string.IsNullOrEmpty(source))
        {
            return source;
        }else if (source.Length <= maxLength)
        {
            return source;
        }
        else
        {
            return source.Substring(0, maxLength) + trimSign;
        }
    }

}