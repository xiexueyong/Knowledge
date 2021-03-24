using System.Collections;
using System.Collections.Generic;
using Framework.Tables;
using UnityEngine;

public class DataTransfer
{
    public static int SkinStyleId(int skinId ,int skinIndex)
    {
        return skinId * 1000 + skinIndex;
    }

    public static Int2 SkinIdAndIndex(int skinStyleId)
    {
        int skinId = GetSkinId(skinStyleId); 
        int skinIndex = GetSkinIdx(skinStyleId);
        return new Int2(skinId,skinIndex);
    }
    
    
    public static int GetSkinId(int skinStyleId)
    {
        return skinStyleId / 1000;
    }
    
    
    public static int GetSkinIdx(int skinStyleId)
    {
        return skinStyleId % 1000;
    }

}
