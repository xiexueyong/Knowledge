using System.Collections;
using System.Collections.Generic;
using Framework.Asset;
using SimpleJSON;
using static GameController;

public class ConstUpdate
{
    public static IEnumerator Update()
    {
        if (GameConfig.Inst.DebugEnable)
        {
            yield break;
        }
        //请求服务器版本信息
        bool finishCheckVersion = false;
        Body body = new Body(URLConfig.ConstUpdate);
        HttpRequestTool.SendMessage(body,
            (sucessData) =>
            {
                DebugUtil.Log("update const sucess:" + sucessData);
                JSONNode jsonNode = JSONNode.Parse(sucessData);

                string constStr = jsonNode["data"];
                if (!string.IsNullOrEmpty(constStr))
                {
                    Table.GameConst.Init(constStr);
                }

                finishCheckVersion = true;
            },
            (failData) =>
            {
                DebugUtil.LogError("appversion fail:" + failData);
                finishCheckVersion = true;
            },
            null,
            false
        );

        //等待版本信息
        while (!finishCheckVersion)
            yield return null;

    }
    static void ParseConstUpdateResponse(JSONNode jsonNode)
    {
        string constStr = jsonNode["data"];
        if (!string.IsNullOrEmpty(constStr))
        {
            Table.GameConst.Init(constStr);
        }
    }

}
