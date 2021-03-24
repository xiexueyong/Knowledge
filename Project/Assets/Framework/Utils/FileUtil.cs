using System.Collections;
using Framework.Asset;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class FileUtil : MonoBehaviour {

    // 改名方法
    public static void Rename(string oldPath, string newPath)
    {
        if (File.Exists(newPath))
        {
            File.Delete(newPath);
        }
        FileInfo ofi = new FileInfo(oldPath);
        ofi.MoveTo(newPath);
    }
    public static void CreateFile(string path, string filename, string info)
    {
        StreamWriter sw;
        FileInfo t = new FileInfo(path + "/" + filename);
        DirectoryInfo dir = t.Directory;
        if (!dir.Exists)
        {
            dir.Create();
        }

        sw = t.CreateText();
        //以行的形式写入信息
        sw.WriteLine(info);
        //关闭流
        sw.Flush();
        sw.Close();
        //销毁流
        sw.Dispose();
    }
    public static void AppendText(string path, string filename, string info)
    {
        StreamWriter sw;
        FileInfo t = new FileInfo(path + "/" + filename);
        DirectoryInfo dir = t.Directory;
        if (!dir.Exists)
        {
            dir.Create();
        }

        sw = t.AppendText();
        //以行的形式写入信息
        sw.WriteLine(info);
        //关闭流
        sw.Flush();
        sw.Close();
        //销毁流
        sw.Dispose();
    }

    // 获取文件夹下的所有文件，包括子文件夹 不包含.meta文件
    public static FileInfo[] GetFiles(string path)
    {
        List<FileInfo> filesList = new List<FileInfo>();

        if (Directory.Exists(path))
        {
            DirectoryInfo folder = new DirectoryInfo(path);
            DirectoryInfo[] subFolders = folder.GetDirectories();
            foreach (DirectoryInfo subFolder in subFolders)
            {
                FileInfo[] fileInfos = GetFiles(subFolder.FullName);
                if(fileInfos.Length > 0)
                    filesList.AddRange(fileInfos);
            }

            FileInfo[] files = folder.GetFiles();
            foreach (FileInfo file in files)
            {
                if (file.Extension != ".meta")
                {
                    filesList.Add(file);
                }
            }
        }
        return filesList.ToArray();
    }

 

    // 创建目录
    public static void CreateDirectory(string filePath)
    {
        if (!string.IsNullOrEmpty(filePath))
        {
            string dirName = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dirName))
            {
                Directory.CreateDirectory(dirName);
            }
        }
    }

   
    // 文件是否存在
    public static bool IsFileExistsInPersistetntDataPath(string path,string md5)
    {
        string localVersionPath = string.Format("{0}/{1}", FilePathTools.persistentDataPath_Platform, path);
        if (File.Exists(localVersionPath))
        {
            return md5.ToLower() == AssetUtils.BuildFileMd5(localVersionPath).ToLower();
        }
        return false;
    }

    // 拷贝文件
    public static void CopyFile(string fromFilePath, string toFilePath)
    {
        CreateDirectory(toFilePath);
        File.Copy(fromFilePath, toFilePath, true);
    }



    public static bool HasNoFile(DirectoryInfo dir)
    {
        bool noFile = true;
        foreach (var file in dir.GetFiles())
        {
            if (file.Name == ".DS_Store")
                continue;

            if (file.Name.EndsWith(".meta") && Directory.Exists(
                    Path.Combine(dir.FullName, file.Name.Substring(0, file.Name.IndexOf(".meta")))))
                continue;

            noFile = false;
            break;
        }
        return noFile;
    }

    public static bool IsEmptyOrNotExit(string path)
    {
        if (!Directory.Exists(path))
        {
            return true;
        }
        DirectoryInfo di = new DirectoryInfo(path);
        return IsEmptyDirectory(di);
    }
    public static void DeleteFile(string path)
    {
        File.Delete(path);
    }


    public static bool IsEmptyDirectory(DirectoryInfo dir)
    {
        if (HasNoFile(dir))
        {
            var subDirs = dir.GetDirectories();
            bool allEmpty = true;
            foreach (var subDir in subDirs)
            {
                if (!IsEmptyDirectory(subDir))
                {
                    allEmpty = false;
                    break;
                }
            }
            return allEmpty;
        }
        return false;
    }

}
