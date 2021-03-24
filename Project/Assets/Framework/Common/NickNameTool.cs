using Framework.Tables;
using UnityEngine;


public static class NickNameTool
{


    public static bool SetNickName(string nickName)
    {
        string kong = " ";
        nickName = nickName.Trim();
        if (nickName.Contains(kong))
            return false;

        //var tableNickNameLimit = Table.NickNameLimit.Head();
        //while (tableNickNameLimit != null)
        //{
        //    string limit = tableNickNameLimit.Id;
        //    if (string.Equals(limit, nickName))
        //    {
        //        return false;
        //    }
        //    tableNickNameLimit = tableNickNameLimit.Next;
        //}


        return true;
    }

}
