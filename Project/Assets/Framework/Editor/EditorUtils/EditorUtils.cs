using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Diagnostics;
using Framework.Storage;
using System;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
using UnityEditor.Build.Reporting;

public static class EditorMenuUtils
{
    static EditorMenuUtils()
    {
        //EditorApplication.delayCall += () =>
        //{
        //    PerformAction(AssetBundleConfig.Inst.UseAssetBundle);
        //};

        //EditorApplication.update += () => {
        //    PerformAction(AssetConfigController.Instance.UseAssetBundle);
        //};
    }

    //private const string UseBundle_MENU_NAME = "AssetBundle/UseBundle/Toggle";
    //[MenuItem(UseBundle_MENU_NAME)]
    //private static void ToggleUseBundleAction()
    //{
    //    Selection.activeObject  = AssetDatabase.LoadAssetAtPath<ScriptableObject>("Assets/Resources/Settings/AssetBundleConfig.asset");

    //    PerformAction(!AssetBundleConfig.Inst.UseAssetBundle);
    //}
    [MenuItem("Select/ABConfig")]
    private static void SelectABConfig()
    {
        Selection.activeInstanceID = AssetDatabase.LoadAssetAtPath<ScriptableObject>("Assets/Resources/Settings/AssetBundleConfig.asset").GetInstanceID();
    }

    [MenuItem("Select/GameConfig")]
    private static void SelectGameConfig()
    {
        Selection.activeInstanceID = AssetDatabase.LoadAssetAtPath<ScriptableObject>("Assets/Resources/Settings/GameConfig.asset").GetInstanceID();
    }
    [MenuItem("Select/UICommonLayer")]
    private static void SelectUICommonLayer()
    {
        GameObject commonLayerObj = null;
       
        Action a = () =>
        {
            var gs = new List<GameObject>(Object.FindObjectsOfType<GameObject>());
            GameObject uiMgr = gs.Where(x => x.name == "UIManager").ToList().FirstOrDefault();
            commonLayerObj = uiMgr?.transform.Find("Canvas/Common")?.gameObject;
        };

        a.Invoke();
        if (commonLayerObj == null)
        {
            UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/FFF/Scenes/InitScene.unity");
            a.Invoke();
        }
        Selection.objects = new List<GameObject> { commonLayerObj }.ToArray();
    }
    //[MenuItem("Select/Gun In Scene")]
    //private static void SelectGun()
    //{
    //    var gs = new List<GameObject>(Object.FindObjectsOfType<GameObject>());
    //    GameObject gunObj = gs.Where(x=>x.transform.GetComponent<HandGunController>()!= null).ToList().FirstOrDefault();
    //    Selection.objects = new List<GameObject> { gunObj }.ToArray();
    //}

    [MenuItem("Select/InitScene")]
    private static void SelectLogin()
    {
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/FFF/Scenes/InitScene.unity");
    }
    [MenuItem("Select/HomeScene")]
    private static void SelectHomeScene()
    {
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/FFF/Scenes/HomeScene.unity");
    }
    [MenuItem("Select/GameScene")]
    private static void SelectShootScene()
    {
        UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Match3Submodule/Scenes/Game.unity");
    }




    //public static void PerformAction(bool enabled)
    //{
    //    Menu.SetChecked(UseBundle_MENU_NAME, enabled);
    //    AssetBundleConfig.Inst.UseAssetBundle = enabled;
    //}


    [MenuItem("DevHelper/Build Android")]
    static void BuildCurTarget()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/FFF/Scenes/InitScene.unity", "Assets/FFF/Scenes/StartScene.unity", "Assets/FFF/Scenes/HomeScene.unity", "Assets/FFF/Scenes/ShootScene.unity", "Assets/FFF/Scenes/IntermediaScene.unity" };
        buildPlayerOptions.locationPathName = "BattleStar.apk";
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = BuildOptions.None;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded: " + summary.totalSize + " bytes");
        }

        if (summary.result == BuildResult.Failed)
        {
            Debug.Log("Build failed");
        }
    }

    [MenuItem("DevHelper/New Account")]
    static void ClearUserData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetString("TestDevice", "Dev" + DateTime.UtcNow.Ticks);
    }

    [MenuItem("DevHelper/Export Table")]
    static void ExportTable()
    {
        var finalPath = Application.dataPath.Replace("FFFP/Assets", "") + "Data/TableTool/ExcelToJson.py";
        string result = RunCMD("python", finalPath);
        Debug.Log("===== 详细Log ===== >"+result);
        if (result.Contains("SUCESS")) {
            Debug.Log("=== 导出成功 ===");
        }
        else {
            Debug.Log("=== 导出失败 ===");
        }
    }
    [MenuItem("DevHelper/Export Select Table ")]
    static void ExportTableByName()
    {
        string path = EditorUtility.OpenFilePanel("选择你想要导出的数据表", "../Data/Table/", "xlsx");
        if (path.Length != 0) {
            string fileName = path.Split('/').Last().Split('.')[0];
            Debug.Log(fileName);
            var finalPath = Application.dataPath.Replace("FFFP/Assets", "") + "Data/TableTool/ExcelToJson.py " +fileName;
            string result = RunCMD("python", finalPath);
            Debug.Log("===== 详细Log ===== >" + result);
            if (result.Contains("SUCESS")) {
                Debug.Log("=== 导出成功 ===");
            }
            else {
                Debug.Log("=== 导出失败 ===");
            }
        }
    }

    [MenuItem("DevHelper/Export Storage")]
    static void ExportStorage()
    {
        var finalPath = Application.dataPath.Replace("WordBuild/Assets", "") + "Data/StorageTool/ExcelToStorageCode.py";
        string result = RunCMD("python", finalPath);
        
        Debug.Log("===== 详细Log ===== >"+result);
        if (result.Contains("SUCESS")) {
            Debug.Log("=== 导出成功 ===");
        }
        else {
            Debug.Log("=== 导出失败 ===");
            
        }
    }

    [MenuItem("DevHelper/ClearAllPlayerPrefs", false, 4)]
    static void DeleteAll()
    {
        PlayerPrefs.DeleteAll();
    }

    public static string RunCMD(string bashName, string args)
    {
        Process p = new Process();
        p.StartInfo.FileName = bashName; //确定程序名
        p.StartInfo.Arguments = args;
        p.StartInfo.UseShellExecute = false; //是否使用Shell
        p.StartInfo.RedirectStandardInput = true; //重定向输入
        p.StartInfo.RedirectStandardOutput = true; //重定向输出
        p.StartInfo.RedirectStandardError = true; //重定向输出错误
        p.StartInfo.CreateNoWindow = false; //设置不显示窗口
        p.Start();
        return p.StandardOutput.ReadToEnd();
    }

    public static void CreateAsset<T>(string path) where T : ScriptableObject
    {
        var folder = Path.GetDirectoryName(Application.dataPath + "/Resources/" + path);
        if (!Directory.Exists(folder))
            Directory.CreateDirectory(folder);
        T ac = ScriptableObject.CreateInstance<T>();
        AssetDatabase.CreateAsset(ac, "Assets/Resources/" + path + ".asset");
    }

    [MenuItem("DevHelper/Create/AssetBundleConfig", false, 4)]
    private static void CreateBundleConfig()
    {
        CreateAsset<AssetBundleConfig>(AssetBundleConfig.AssetConfigPath);
    }

    [MenuItem("DevHelper/Create/GameConfig", false, 4)]
    private static void CreateGameConfig()
    {
        CreateAsset<GameConfig>(GameConfig.ConfigurationControllerPath);
    }


    [MenuItem("DevHelper/DeleteAllEmptyDirectories", false, 5)]
    static void FindAndRemove()
    {
        var root = Application.dataPath;
        string[] dirs = Directory.GetDirectories(root, "*", SearchOption.AllDirectories);
        List<DirectoryInfo> emptyDirs = new List<DirectoryInfo>();
        foreach (var dir in dirs) {
            DirectoryInfo di = new DirectoryInfo(dir);
            if (FileUtil.IsEmptyDirectory(di))
                emptyDirs.Add(di);
        }

        foreach (var emptyDir in emptyDirs) {
            if (Directory.Exists(emptyDir.FullName)) {
                Directory.Delete(emptyDir.FullName, true);
                UnityEngine.Debug.Log("Recursively delete folder: " + emptyDir.FullName);
            }
        }

        AssetDatabase.Refresh();
    }

    [MenuItem("DevHelper/ShowChin", false, 6)]
    static void ShowChin()
    {
        ChinScreenSimulate.OpenSimulate();
    }
    
    
    
    public enum TextType
    {
        Button,
        Title
    }

    static Color ButtonTextColor = new Color(1f, 247f / 255f, 197f / 255f, 1f);
    static Color TitleTextColor = new Color(228f / 255f, 215f / 255f, 152f / 255f, 1);

    [MenuItem("GameObject/默认文字样式/----- 字体大小自己设置 -----", false, 9)]
    [MenuItem("GameObject/默认文字样式/(T) 标题文字", false, 10)]
    static void SetDefaultTitleText(MenuCommand menuCommand)
    {
        Text text = Selection.activeGameObject.GetComponent<Text>();
        SetFont(TextType.Title, text);
    }

    [MenuItem("GameObject/默认文字样式/(B) 按钮文字", false, 10)]
    static void SetDefaultButtnText(MenuCommand menuCommand)
    {
        Text text = Selection.activeGameObject.GetComponent<Text>();
        SetFont(TextType.Button, text);
    }

    static void SetFont(TextType textType, Text text, bool bold = true)
    {
        Font targetFont = (Font) AssetDatabase.LoadAssetAtPath("Assets/Art/Font/WordCrossy.ttf", typeof(Font));

        Color fontColor = Color.white;
        switch (textType) {
            case TextType.Title:
                fontColor = TitleTextColor;
                break;
            case TextType.Button:
                fontColor = ButtonTextColor;
                break;
        }

        text.font = targetFont;
        text.color = fontColor;
        if (bold) {
            text.fontStyle = FontStyle.Bold;
        }
        else {
            text.fontStyle = FontStyle.Normal;
        }

        text.alignment = TextAnchor.MiddleCenter;
        text.supportRichText = false;
        text.raycastTarget = false;
        if (!text.GetComponent<Shadow>()) {
            text.gameObject.AddComponent<Shadow>();
        }

        text.GetComponent<Shadow>().effectDistance = new Vector2(2, -2);
    }

    /// <summary>
    /// 获取一个物体下所有的某个控件, 递归
    /// </summary>
    /// <param name="trans"></param>
    /// <param name="list"></param>
    /// <typeparam name="T"></typeparam>
    static void GetSelfAndChildComponent<T>(Transform trans, ref List<T> list)
    {
        T component1 = trans.GetComponent<T>();
        if (component1 !=null)
        {
            if (!component1.Equals(null))
            {
                list.Add(component1);
            }
        }

        //把自己身上的加完了, 加自己所有的子物体上
        for (int i = 0; i < trans.childCount; i++)
        {
            GetSelfAndChildComponent(trans.GetChild(i), ref list);
        }
    }

    static void GetGoPath(Transform trans,ref string p)
    {
        p = trans.name + p;
        if (trans.parent != null)
        {
            p = "/" + p;
           GetGoPath(trans.parent,ref p);
        }
    }

    static void GetTaskName(Transform trans, ref string p)
    {
        if (trans.parent != null)
        {
            GetTaskName(trans.parent, ref p);
        }
        else
        {
            p = trans.name;
        }
    }
}