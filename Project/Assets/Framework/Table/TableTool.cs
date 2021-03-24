using Framework.Asset;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using Framework.Storage;
using UnityEngine;

namespace Framework.Tables
{
    public class TableTool
    {
        public static string GetTxt(string filePath)
        {
            //从配置表中获取到source对应的表后缀
            string newFilePath = filePath;

            if (StorageManager.Inst.Inited && !filePath.Contains("AdChannelPostfix"))
            {
                string source = StorageManager.Inst.GetStorage<StorageAccountInfo>().adChannel;
                if (!string.IsNullOrEmpty(source))
                {
                    TableAdChannelPostfix adChannelPostfix = Table.AdChannelPostfix.Get(source);
                    if (adChannelPostfix != null)
                    {
                        DebugUtil.LogWarning("has new file path: " + filePath + "====>" + newFilePath);
                        newFilePath += adChannelPostfix.Postfix;
                    }
                    else
                    {
                        DebugUtil.LogWarning("TableAdChannelPostfix 为null:" + source);
                    }
                }
                else
                {
                    DebugUtil.LogWarning("ad channel 为空或者null:" + source);
                }
            }
            else
            {
                DebugUtil.LogWarning("StorageManager has not init, but read: " + filePath);
            }

            string str;
            TextAsset newTa = Res.LoadResource<TextAsset>(newFilePath);
            if (newTa != null)
            {
                //新路径下, 存在文件,使用新文件.
                str = newTa.text;
                Res.Recycle(newTa);
            }
            else
            {
                //新路径下不存在文件,则仍用原始路径
                TextAsset ta = Res.LoadResource<TextAsset>(filePath);
                str = ta.text;
                Res.Recycle(ta);
            }

            return str;
        }
    }
}
