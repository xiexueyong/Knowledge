using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Framework.Asset;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;
using Debug = UnityEngine.Debug;

#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif

public static class PerformBuild
{
    private static Process RunTerminal(string terminal, string command, string workingDir = "")
    {
        ProcessStartInfo pStartInfo = new ProcessStartInfo(terminal, command)
        {
            CreateNoWindow = false,
            UseShellExecute = false,
            RedirectStandardError = false,
            RedirectStandardInput = false,
            RedirectStandardOutput = true,
        };
        if (!string.IsNullOrEmpty(workingDir))
        {
            pStartInfo.WorkingDirectory = workingDir;
        }

        return Process.Start(pStartInfo);
    }


    private static string FormatPath(string path)
    {
        path = path.Replace("/", "\\");
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            path = path.Replace("\\", "/");
        }

        return path;
    }

    private static string[] GetBuildScenes()
    {
        List<string> names = new List<string>();

        foreach (EditorBuildSettingsScene e in EditorBuildSettings.scenes)
        {
            if (e == null)
            {
                continue;
            }

            if (e.enabled)
            {
                names.Add(e.path);
            }
        }

        return names.ToArray();
    }

    private static void ExportTable()
    {
        string terminal = "Python";
        //if (Application.platform == RuntimePlatform.OSXEditor)
        //{
        //    terminal = "/Applications/Utilities/Terminal.app/Contents/MacOS/Terminal";
        //}
        //else if (Application.platform == RuntimePlatform.WindowsEditor)
        //{
        //    terminal = "cmd.exe";
        //}
        string command = "ExcelToJson.py";
        string formatPath = FormatPath("../Data/TableTool/");
        Debug.LogError("begin export table" + DateTime.Now.ToLongTimeString());
        Process runTerminal = RunTerminal(terminal, command, formatPath);
        string outPutStr = runTerminal.StandardOutput.ReadToEnd();
        if (!outPutStr.Contains("SUCESS"))
        {
            throw new Exception("export table error!!");
        }

        runTerminal.WaitForExit();
        runTerminal.Close();
        Debug.LogError("export table over" + DateTime.Now.ToLongTimeString());
        AssetDatabase.Refresh();
    }

    private static void ClearBundle()
    {
        //清空streamingAsset文件夹
        if (Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.Delete(Application.streamingAssetsPath, true);
        }
        //清空AssetBundleOutRelease文件夹
        string outReleasePath = Application.dataPath + "/AssetBundleOutRelease/";
        if (Directory.Exists(outReleasePath))
        {
            Directory.Delete(outReleasePath, true);
        }

        string delBundle = GetJenkinsParameter("DelBundle");
        if (delBundle.Trim().ToLower() != "true")
        {
            return;
        }
        
        //清空AssetBundleOut文件夹
        string outPath = Application.dataPath + "/AssetBundleOut/";
        if (Directory.Exists(outPath))
        {
            Directory.Delete(outPath, true);
        }
    }

    private static void CommonAssetSetting()
    {
        PlayerSettings.SplashScreen.showUnityLogo = false;

        string buildNum = GetJenkinsParameter("BuildNum");
        GameConfig.Inst.CurrentBuildNumber = buildNum;

        string dis = GetJenkinsParameter("Distribute");
        if (string.IsNullOrEmpty(dis))
        {
            PlayerSettings.Android.useAPKExpansionFiles = true;
            Debug.unityLogger.logEnabled = false;
            GameConfig.Inst.DebugEnable = false;
            GameConfig.Inst.RewardSucess = false;
            GameConfig.Inst.serverType = GameConfig.ServerType.Fotoable;

        }
        else if (dis.ToLower() == "true")
        {
            PlayerSettings.Android.useAPKExpansionFiles = true;
            Debug.unityLogger.logEnabled = false;
            GameConfig.Inst.DebugEnable = false;
            GameConfig.Inst.RewardSucess = false;
            GameConfig.Inst.serverType = GameConfig.ServerType.Fotoable;
            
        }
        else if (dis.ToLower() == "false")
        {
            PlayerSettings.Android.useAPKExpansionFiles = false;
            Debug.unityLogger.logEnabled = true;
            GameConfig.Inst.DebugEnable = true;
        }
        else
        {
            PlayerSettings.Android.useAPKExpansionFiles = true;
            Debug.unityLogger.logEnabled = false;
            GameConfig.Inst.DebugEnable = false;
            GameConfig.Inst.RewardSucess = false;
            GameConfig.Inst.serverType = GameConfig.ServerType.Fotoable;
        }

        string assetVersion = GetJenkinsParameter("AssetVersion");
        if (!string.IsNullOrEmpty(assetVersion))
        {
            AssetBundleConfig.Inst.AssetVersion = assetVersion;
        }

        string complete = GetJenkinsParameter("CompleteAsset");
        if (string.IsNullOrEmpty(complete))
        {
            AssetBundleConfig.Inst.CompleteAsset = true;
        }
        else
        {
            AssetBundleConfig.Inst.CompleteAsset = complete.ToLower() == "true";
        }

        string storeVersion = GetJenkinsParameter("StoreVersion");
        if (!string.IsNullOrEmpty(storeVersion))
        {
            PlayerSettings.bundleVersion = storeVersion;
        }

        ExportTable();

        //检测字体文件是否存在
        bool isExistFont = CheckFontSettingExist();
        if (!isExistFont)
        {
            throw new Exception("some font not exist");
        }

        //        EditorUtils.CheckMaterialMiss();
    }

    private static bool CheckFontSettingExist()
    {
        if (!File.Exists("Assets/Art/Font/map_font.fontsettings"))
        {
            return false;
        }

        if (!File.Exists("Assets/Art/Font/flower_add_font/flower_add_font.fontsettings"))
        {
            return false;
        }

        if (!File.Exists("Assets/Art/Font/game_font_01/combo_font.fontsettings"))
        {
            return false;
        }

        if (!File.Exists("Assets/Art/Font/game_font_01/combo_x_num.fontsettings"))
        {
            return false;
        }

        if (!File.Exists("Assets/Art/Font/game_font_01/game_font_01.fontsettings"))
        {
            return false;
        }

        if (!File.Exists("Assets/Art/Font/game_font_02/game_font_02.fontsettings"))
        {
            return false;
        }

        if (!File.Exists("Assets/Art/Font/game_font_03/game_font_03.fontsettings"))
        {
            return false;
        }

        if (!File.Exists("Assets/Art/Font/shop_number/shop_number.fontsettings"))
        {
            return false;
        }

        return true;
    }

    private static void BuildBundleApk()
    {
        PlayerSettings.Android.preferredInstallLocation = AndroidPreferredInstallLocation.ForceInternal;
        PlayerSettings.Android.forceInternetPermission = true;
        CommonAssetSetting();
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android)
        {
            throw new Exception(" unity not apk platform now");
        }

        Debug.LogError("begin build bundle" + DateTime.Now.ToLongTimeString());
        ClearBundle();
        BuildAssetBundles.BuildAllAssetBundle();
    }

    private static void BuildApk()
    {
        BuildBundleApk();
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel28;
        string buildNum = GetJenkinsParameter("BuildNum");
        PlayerSettings.Android.bundleVersionCode = int.Parse(buildNum);
        PlayerSettings.applicationIdentifier = "com.fotoable.WordVillas";
        PlayerSettings.Android.keystoreName = "";
//        PlayerSettings.keystorePass = "123456789";
//        PlayerSettings.Android.keyaliasName = "villaskeystore";
//        PlayerSettings.Android.keyaliasPass = "123456789";
        // SetNewAssetVersion(buildNum);
        string[] scenes = GetBuildScenes();
        string fileName = "WordVillas.apk";
        string filePath = "/Library/WebServer/Documents/";
        string location = Path.Combine(filePath, fileName);
        BuildReport buildReport = BuildPipeline.BuildPlayer(scenes, location, BuildTarget.Android, BuildOptions.None);
        if (buildReport.summary.result != BuildResult.Succeeded)
        {
            throw new Exception("build error：" + buildReport.summary.totalErrors);
        }
    }

    private static void BuildBundleIos()
    {
        CommonAssetSetting();
        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS)
        {
            throw new Exception(" unity not ios platform now");
        }

        Debug.LogError("begin build bundle" + DateTime.Now.ToLongTimeString());
        ClearBundle();
        BuildAssetBundles.BuildAllAssetBundle();
    }

    private static void BuildIOS()
    {
        BuildBundleIos();
        string buildNum = GetJenkinsParameter("BuildNum");
        PlayerSettings.iOS.buildNumber = buildNum;
        PlayerSettings.stripEngineCode = false;
        // SetNewAssetVersion(buildNum);
        string dis = GetJenkinsParameter("Distribute");
        string location;
        PlayerSettings.applicationIdentifier = "com.fotoable.WordVillas";
        if (string.IsNullOrEmpty(dis))
        {
            location = "/Users/fotoable/Documents/AutoBuildOutput/WordXcode/";
        }
        else if (dis.ToLower() == "true")
        {
            location = "/Users/fotoable/Documents/AutoBuildOutput/WordXcodeDis/";
        }
        else
        {
            location = "/Users/fotoable/Documents/AutoBuildOutput/WordXcodeDev/";
        }

        string[] scenes = GetBuildScenes();
        BuildReport buildReport = BuildPipeline.BuildPlayer(scenes, location, BuildTarget.iOS, BuildOptions.AcceptExternalModificationsToPlayer);
        if (buildReport.summary.result != BuildResult.Succeeded)
        {
            throw new Exception("build error：" + buildReport.summary.totalErrors);
        }
    }

    // private static void SetNewAssetVersion(string buildNum)
    // {
    //     string assetVersion = GetNewVersionStr(AssetBundleConfig.Inst.AssetVersion, buildNum);
    //
    //     AssetBundleConfig.Inst.AssetVersion = assetVersion;
    //     Debug.Log("new AssetBundleConfig.Inst.AssetVersion:" + assetVersion);
    //
    //     string newAppVersionStr = GetNewVersionStr(PlayerSettings.bundleVersion, buildNum);
    //     PlayerSettings.bundleVersion = newAppVersionStr;
    //     Debug.Log("new app version: " + newAppVersionStr);
    // }
    //
    // private static string GetNewVersionStr(string old, string buildNum)
    // {
    //     string[] oldChars = old.Split('.');
    //     string newLast = buildNum;
    //     string newStr = "";
    //     for (int i = 0; i < oldChars.Length; i++)
    //     {
    //         if (i == oldChars.Length - 1)
    //         {
    //             newStr += newLast;
    //         }
    //         else
    //         {
    //             newStr += oldChars[i] + ".";
    //         }
    //     }
    //
    //     return newStr;
    // }

    //解释jenkins传输的参数
    private static string GetJenkinsParameter(string name)
    {
        foreach (string arg in Environment.GetCommandLineArgs())
        {
            if (arg.StartsWith(name))
            {
                return arg.Split("-"[0])[1];
            }
        }

        return null;
    }

    [PostProcessBuild]
    public static void OnPostprocessingBuild(BuildTarget buildTarget, string path)
    {
        if (buildTarget == BuildTarget.iOS)
        {
#if UNITY_IOS
            string projPath = PBXProject.GetPBXProjectPath(path);
            PBXProject proj = new PBXProject();
            proj.ReadFromFile(projPath);
            string target = proj.GetUnityMainTargetGuid();
            proj.SetBuildProperty(target, "ENABLE_BITCODE", "NO");
            proj.AddFrameworkToProject(target, "StoreKit.framework", true);
            proj.AddFrameworkToProject(target, "AdSupport.framework", true);
            proj.AddCapability(target, PBXCapabilityType.InAppPurchase, null, true);
            //proj.AddCapability(target, PBXCapabilityType.ApplePay, entitlementsFilePath, true);
            proj.WriteToFile(projPath);

            // 修改Info.plist文件
            string plistPath = Path.Combine(path, "Info.plist");
            PlistDocument plist = new PlistDocument();
            plist.ReadFromFile(plistPath);
            PlistElementDict dict = plist.root.CreateDict("NSAppTransportSecurity");
            dict.SetBoolean("NSAllowsArbitraryLoads", true);

            plist.root.SetString("GADApplicationIdentifier", "ca-app-pub-3041081834653045~6836057851");
            plist.root.SetBoolean("GADIsAdManagerApp", true);
            plist.root.SetBoolean("ITSAppUsesNonExemptEncryption", false);
            plist.root.SetString("FirebaseMessagingAutoInitEnabled", "NO");
            //IronSource需求
            PlistElementArray a = plist.root.CreateArray("SKAdNetworkItems");
            PlistElementDict b = a.AddDict();
            b.SetString("SKAdNetworkIdentifier","SU67R6K2V3.skadnetwork");
            

            plist.WriteToFile(plistPath);
            Debug.Log("post build ios success");
#endif
        }
    }
}
