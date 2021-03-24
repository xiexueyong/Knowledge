using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NoshCore;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using System.IO;
using System;
using System.Xml;

public class CITool
{
    private static string ANDROID_PROJECT_NAME = "unity-android";
    private static string IPHONE_PROJECT_NAME = "Unity-iPhone";
    private static string BUNDLE_IDENTIFIER = "com.noshgames.memoryios";

    public static string GetProjectPath(BuildTarget buildTarget)
    {
        if(buildTarget == BuildTarget.Android)
        {
            return CIFileUtil.GetFullPath(CIFileUtil.PathCombine(Application.dataPath, "..", "..", "Export", "Android"));
        }
        else
        {
            return CIFileUtil.GetFullPath(CIFileUtil.PathCombine(Application.dataPath, "..", "..", "Export", buildTarget.ToString()));
        }
    }

    public static void BuildAssetBundles(BuildTarget buildTarget)
    {
        if(buildTarget == BuildTarget.Android)
        {
            Framework.Asset.BuildAssetBundles.BuildAllAssetBundleAndroid();
        }
        else
        {
            Framework.Asset.BuildAssetBundles.BuildAllAssetBundleIOS();
        }
    }


    public static void BuildPlayer(BuildTarget buildTarget)
    {
        var originProductName = PlayerSettings.productName;
        var originAppId = "";
        if (buildTarget == BuildTarget.Android)
        {
            PlayerSettings.productName = ANDROID_PROJECT_NAME;

            if (String.IsNullOrEmpty(Environment.GetEnvironmentVariable("JAVA_HOME")))
            {
                Environment.SetEnvironmentVariable("JAVA_HOME", "/Library/Java/JavaVirtualMachines/jdk1.8.0_73.jdk/Contents/Home");
            }
            Debug.Log("JAVA_HOME" + Environment.GetEnvironmentVariable("JAVA_HOME"));
        }
        else
        {
            originAppId = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.iOS);
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, BUNDLE_IDENTIFIER);
        }        

        string projectPath = GetProjectPath(buildTarget);
        if (CIFileUtil.CreateDirectory(projectPath))
        {

            BuildAssetBundles(buildTarget);

            BuildPlayerOptions options = new BuildPlayerOptions
            {
                scenes = new[] {
                    "Assets/FFF/Scenes/InitScene.unity",
                    "Assets/FFF/Scenes/StartScene.unity",
                    "Assets/FFF/Scenes/HomeScene.unity",
                    "Assets/FFF/Scenes/IntermediaScene.unity",
                    "Assets/FFF/Scenes/ShootScene.unity",
                    "Assets/Match3Submodule/Scenes/Game.unity",
                },
                locationPathName = projectPath,
                target = buildTarget,
                options = BuildOptions.StrictMode,
            };

            if (buildTarget == BuildTarget.iOS)
            {
                CIFileUtil.ClearDirectory(projectPath);
            }
            else if (buildTarget == BuildTarget.Android)
            {
                CIFileUtil.ClearDirectory(CIFileUtil.PathCombine(projectPath, ANDROID_PROJECT_NAME));
            }

            Debug.Log("Start BuildPlayer..............");
            BuildPipeline.BuildPlayer(options);

            Debug.Log("Start Custom Process...........");
            if (buildTarget == BuildTarget.iOS)
            {
                string projPath = PBXProject.GetPBXProjectPath(projectPath);

                PBXProject proj = new PBXProject();
                proj.ReadFromFile(projPath);

                string target = proj.GetUnityMainTargetGuid();

                proj.SetBuildProperty(target, "ENABLE_BITCODE", "false");
                //proj.AddFrameworkToProject(target, "libz.1.tbd", false);
                //proj.AddFrameworkToProject(target, "libxml2.tbd", false);

                //frameworks
                string frameworks = CIFileUtil.GetFullPath(Path.Combine(projectPath, "../iOSFrameworks/"));
                proj.AddBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "$(PROJECT_DIR)/../iOSFrameworks");
                foreach (var file in CIFileUtil.ListDirectorys(frameworks))
                {
                    if (file.name.EndsWith(".framework", StringComparison.Ordinal))
                    {
                        proj.AddFileToBuild(target, proj.AddFile(file.path, "Frameworks/" + file.name));
                    }
                }

                //msdk
                //string msdkPath = CIFileUtil.GetFullPath(Path.Combine(projectPath, "../iOSFrameworks/MSDK/"));
                //proj.AddBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "$(PROJECT_DIR)/../iOSFrameworks/MSDK");
                //proj.AddFileToBuild(target, proj.AddFile(msdkPath + "MSDK.framework", "MSDK/MSDK.framework"));
                //proj.AddFileToBuild(target, proj.AddFile(msdkPath + "WGPlatformResources.bundle", "MSDK/WGPlatformResources.bundle"));

                //add gsp project
                //string gspPath = CIFileUtil.GetFullPath(Path.Combine(projectPath, "../iOSFrameworks/GspLib/"));
                //proj.AddBuildProperty(target, "FRAMEWORK_SEARCH_PATHS", "$(PROJECT_DIR)/../iOSFrameworks/GspLib");
                //proj.AddFileToBuild(target, proj.AddFile(gspPath + "GspLib.framework", "GspLib/GspLib.framework"));

                //add walnut headers
                //string wnInclude = "$(PROJECT_DIR)/../iOSFrameworks/WalnutInclude";
                //proj.AddBuildProperty(target, "HEADER_SEARCH_PATHS", wnInclude + "/external/tolua");

                //properties
                proj.SetBuildProperty(target, "EXECUTABLE_NAME", IPHONE_PROJECT_NAME);
                proj.SetBuildProperty(target, "PRODUCT_NAME", IPHONE_PROJECT_NAME);

                //overwrite default UnityAppController.mm
                //string classesPath = Path.Combine(projectPath, "../iOSFrameworks/Classes");
                //foreach (var file in CIFileUtil.ListFiles(classesPath))
                //{
                //    string dstFile = CIFileUtil.GetFullPath(Path.Combine(projectPath, "Classes/" + file.name));
                //    File.Copy(file.path, dstFile, true);
                //    proj.AddFileToBuild(target, proj.AddFile(dstFile, "Classes/" + file.name));
                //}

                CIFileUtil.WriteTextToFile(proj.WriteToString(), projPath);


                //Handle plist  
                string plistPath = projectPath + "/Info.plist";
                Debug.Log("plistPath=" + plistPath);
                PlistDocument plist = new PlistDocument();
                plist.ReadFromString(File.ReadAllText(plistPath));
                PlistElementDict rootDict = plist.root;

                rootDict.SetString("CFBundleDisplayName", "似水流年");
                rootDict.SetString("CFBundleName", IPHONE_PROJECT_NAME);
                rootDict.SetString("CFBundleIdentifier", BUNDLE_IDENTIFIER);

                //add for gsp init
                //var gameConfig = new GameConfig();
                //rootDict.SetString("dc_unique_key", gameConfig.DcUniqueKey);
                //rootDict.SetBoolean("development", gameConfig.Development); //will be removed by ios_resign
                File.WriteAllText(plistPath, plist.WriteToString());

                //process image assets
                //var assetsDir = projectPath + "/Unity-iPhone/Images.xcassets";
                //UnityEditor.FileUtil.DeleteFileOrDirectory(assetsDir);
                //string srcDir = Path.Combine(projectPath, "../iOSFrameworks/Images.xcassets");
                //UnityEditor.FileUtil.CopyFileOrDirectory(srcDir, assetsDir);


            }
            else if (buildTarget == BuildTarget.Android)
            {
                string filesPath = CIFileUtil.GetFullPath(CIFileUtil.PathCombine(Application.dataPath, "..", "..", "Export", "Files"));
                
                string src = CIFileUtil.PathCombine(filesPath, "Android","gradle");
                string dst = CIFileUtil.PathCombine(projectPath, "gradle");
                // gradle files -> project
                if (CIFileUtil.IsDirectoryExist(dst))
                {
                    CIFileUtil.RemoveDirectory(dst);
                }
                UnityEditor.FileUtil.CopyFileOrDirectory(src, dst);


                // ovrride build.gradle
                src = CIFileUtil.PathCombine(filesPath, "Android", "launcher", "build.gradle");
                dst = CIFileUtil.PathCombine(projectPath, "launcher", "build.gradle");
                UnityEditor.FileUtil.ReplaceFile(src, dst);

                //modify Manifest(remove application)
                //XmlDocument xmlDoc = new XmlDocument();
                //string manifestFile = CIFileUtil.PathCombine(unityAndroid, "AndroidManifest.xml");
                //xmlDoc.Load(manifestFile);
                //XmlNode rootNode = xmlDoc.SelectSingleNode("manifest");
                //XmlNode appNode = rootNode.SelectSingleNode("application");
                //rootNode.RemoveChild(appNode);
                //xmlDoc.Save(manifestFile);

                //change project to android lib
                //string projectPropertiesFile = CIFileUtil.PathCombine(unityAndroid, "project.properties");
                //string content = CIFileUtil.ReadTextFromFile(projectPropertiesFile).TrimEnd() + "\nandroid.library=true\n";
                //CIFileUtil.WriteTextToFile(content, projectPropertiesFile);

                //copy files
                //string templateDir = CIFileUtil.PathCombine(projectPath, "template");
                //foreach (FileEntry f in CIFileUtil.ListFiles(templateDir))
                //{
                //    string dstFile = CIFileUtil.PathCombine(unityAndroid, f.name);
                //    if (CIFileUtil.IsFileExist(dstFile))
                //    {
                //        CIFileUtil.DeleteFile(dstFile);
                //    }
                //    CIFileUtil.CopyFile(f.path, CIFileUtil.PathCombine(unityAndroid, f.name));
                //}

            }

            //CopyFilesToProject(buildTarget, projectPath);
        }
        PlayerSettings.productName = originProductName;
        if(buildTarget == BuildTarget.iOS)
        {
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, originAppId);
        }
    }

    [MenuItem("CI/iOS/Build Project")]
    public static void BuildProject_iOS()
    {
        BuildPlayer(BuildTarget.iOS);
    }

    [MenuItem("CI/iOS/Refresh Project Assets")]
    public static void RefreshProjectAssets_iOS()
    {
        RefreshProjectAssets(BuildTarget.iOS);
    }

    [MenuItem("CI/Android/Build Project")]
    public static void BuildProject_Android()
    {
        BuildPlayer(BuildTarget.Android);
    }

    [MenuItem("CI/Android/Refresh Project Assets")]
    public static void RefreshProjectAssets_Android()
    {
        RefreshProjectAssets(BuildTarget.Android);
    }

    public static void RefreshProjectAssets(BuildTarget buildTarget)
    {
        string projectPath = GetProjectPath(buildTarget);
        if (CIFileUtil.CreateDirectory(projectPath))
        {
            BuildAssetBundles(buildTarget);
            // BuildAssetBundleConfig(buildTarget);
            //CopyFilesToProject(buildTarget, projectPath);
        }
    }
}

