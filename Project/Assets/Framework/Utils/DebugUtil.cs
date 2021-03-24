using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System;
using System.IO;

public class DebugUtil
{
    [Conditional("UNITY_EDITOR")]
    static public void Assert(bool test, string assertString)
    {
#if UNITY_EDITOR
        if (!test)
        {
            StackTrace trace = new StackTrace(true);
            StackFrame frame = trace.GetFrame(1);

            string assertInformation;
            assertInformation = "Filename: " + frame.GetFileName() + "\n";
            assertInformation += "Method: " + frame.GetMethod() + "\n";
            assertInformation += "Line: " + frame.GetFileLineNumber();

            UnityEngine.Debug.Break();

            string assertMessage = assertString + "\n\n" + assertInformation;
            if (UnityEditor.EditorUtility.DisplayDialog("Assert!", assertMessage, "OK"))
            {
                UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(frame.GetFileName(), frame.GetFileLineNumber());
                UnityEngine.Debug.Log(assertInformation);
            }
        }
#endif
    }

    [Conditional("UNITY_EDITOR")]
    static public void FixMaterialInEditor(GameObject go)
    {
#if UNITY_EDITOR
        var renders = go.GetComponentsInChildren<Renderer>();
        foreach (var render in renders)
        {
            render.material.shader = Shader.Find(render.material.shader.name);
        }
#endif
    }


    public static void Log(string str, params object[] args)
    {
        if (GameConfig.Inst.DebugEnable)
        {
            if (args != null && args.Length > 0)
            {
                str = GetLogString(str, args);
            }
            UnityEngine.Debug.Log("[I]> " + str);

            ExportToLocal("[I]> " + str);
        }
      
    }

    public static void LogWarning(string str, params object[] args)
    {
        if (GameConfig.Inst.DebugEnable)
        {
            if (args != null && args.Length > 0)
            {
                str = GetLogString(str, args);
            }
            UnityEngine.Debug.LogWarning("[W]> " + str);
            ExportToLocal("[W]> " + str);
        }
    }

    public static void LogError(string str, params object[] args)
    {
        if (GameConfig.Inst.DebugEnable)
        {
            if (args != null && args.Length > 0)
            {
                str = GetLogString(str, args);
            }
            UnityEngine.Debug.LogError("[E]> " + str);
            ExportToLocal("[E]> " + str);
        }
    }

    private static string GetLogString(string str, params object[] args)
    {
        string retStr = "";
        //DateTime now = DateTime.Now;
        //retStr = now.ToString("HH:mm:ss.fff") + " ";

        retStr = retStr + string.Format(str, args);

        return retStr;
    }

    private static void ExportToLocal(string info)
    {
#if DEBUG
        //FileUtil.AppendText(Application.persistentDataPath+"/log","log.txt",info);
        AppendTextToPersistent("log/log.txt", info);
#endif
    }

    public static void AppendTextToPersistent(string filePath, string txt)
    {
//        string ap = Path.Combine(Application.persistentDataPath, filePath);
//#if UNITY_EDITOR || UNITY_IPHONE
//        ap = Path.Combine(Application.persistentDataPath, filePath);
//#elif UNITY_ANDROID
//            ap = Path.Combine("/sdcard/blast", filePath);
//#endif
//        string dn = Path.GetDirectoryName(ap);

//        if (!Directory.Exists(dn))
//        {
//            Directory.CreateDirectory(dn);
//        }
//        File.AppendAllText(ap, txt);
//        File.AppendAllText(ap, "\n");
    }

}


