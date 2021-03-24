/*-------------------------------------------------------------------------------------------
// 模块名：AssetUtils
// 模块描述：assetbundle模块用到的工具类
//-------------------------------------------------------------------------------------------*/

using System;
using System.IO;
using System.Security.Cryptography;
using System.Collections;
using UnityEngine;
using System.Text;

namespace Framework.Asset
{
    public static class AssetUtils
    {
        #region MD5文件校验

        public static string EncryptWithMD5(string source)
        {
            byte[] sor = Encoding.UTF8.GetBytes(source);
            MD5 md5 = MD5.Create();
            byte[] result = md5.ComputeHash(sor);
            StringBuilder strbul = new StringBuilder(40);
            for (int i = 0; i < result.Length; i++)
            {
                strbul.Append(result[i].ToString("x2"));//加密结果"x2"结果为32位,"x3"结果为48位,"x4"结果为64位
            }
            return strbul.ToString();
        }

        // 生成文件的md5
        public static String BuildFileMd5(String filename)
        {
            String filemd5 = null;
            if (File.Exists(filename))
            {
                try
                {
                    using (var fileStream = File.OpenRead(filename))
                    {
                        var md5 = MD5.Create();
                        var fileMD5Bytes = md5.ComputeHash(fileStream);
                        filemd5 = FormatMD5(fileMD5Bytes);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError(ex.ToString());
                }
            }
            return filemd5;
        }

        // 文件的体积，以K为单位
        public static int FileSize(String filename)
        {
            if (File.Exists(filename))
            {
                FileInfo fileInfo = new FileInfo(filename);
                return (int)(fileInfo.Length / 1024);
            }
            return 0;
        }

        public static Byte[] CreateMD5(Byte[] data)
        {
            using (var md5 = MD5.Create())
            {
                return md5.ComputeHash(data);
            }
        }

        public static string FormatMD5(Byte[] data)
        {
            return BitConverter.ToString(data).Replace("-", "").ToLower();
        }
        #endregion

        public static void Empty(this DirectoryInfo directory)
        {
            foreach (FileInfo file in directory.GetFiles()) file.Delete();
            foreach (DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }


        public static AssetBundle LoadLocalAssetBundle(string path)
        {
            AssetBundle ab = AssetBundle.LoadFromFile(path);
            if (ab == null)
            {
                DebugUtil.LogError("AssetBundle __{0}__ is not exist", path);
            }
            return ab;
        }




        public static string ReadTextFromLocal(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }
            FileStream fs = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string _text = sr.ReadToEnd();

            if (fs != null)
            {
                fs.Close();
            }
            if (sr != null)
            {
                sr.Close();
            }
            return _text;
        }

        public static string ReadTextFromPersistent(string path)
        {
            if (!File.Exists(path))
            {
                return null;
            }
            FileStream fs = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(fs);
            string _text = sr.ReadToEnd();

            if (fs != null)
            {
                fs.Close();
            }
            if (sr != null)
            {
                sr.Close();
            }
            return _text;
        }
        public static bool FileExistAtPersistent(string filePath)
        {
            return File.Exists(filePath);
        }
        public static void SaveTextToPersistent(string filePath, string txt)
        {
            string dn = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dn))
            {
                Directory.CreateDirectory(dn);
            }
            File.WriteAllText(filePath, txt);
        }
        public static void SaveBytesToPersistent(string filePath, byte[] bytes)
        {
            string dn = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dn))
            {
                Directory.CreateDirectory(dn);
            }
            File.WriteAllBytes(filePath, bytes);
        }
        public static void AppendTextToPersistent(string filePath, string txt)
        {
            string dn = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dn))
            {
                Directory.CreateDirectory(dn);
            }
            File.AppendAllText(filePath, txt);
        }
        /// <summary>
        /// 拷贝oldlab的文件到newlab下面
        /// </summary>
        /// <param name="sourcePath">lab文件所在目录(@"~\labs\oldlab")</param>
        /// <param name="savePath">保存的目标目录(@"~\labs\newlab")</param>
        /// <returns>返回:true-拷贝成功;false:拷贝失败</returns>
        public static bool CopyFiles(string sourcePath, string savePath)
        {
            if (!Directory.Exists(savePath))
            {
                Directory.CreateDirectory(savePath);
            }

            #region //拷贝labs文件夹到savePath下
            try
            {
                string[] labDirs = Directory.GetDirectories(sourcePath);//目录
                string[] labFiles = Directory.GetFiles(sourcePath);//文件
                if (labFiles.Length > 0)
                {
                    for (int i = 0; i < labFiles.Length; i++)
                    {
                        if (Path.GetExtension(labFiles[i]) != ".lab")//排除.lab文件
                        {
                            File.Copy(sourcePath + "/" + Path.GetFileName(labFiles[i]), savePath + "/" + Path.GetFileName(labFiles[i]), true);
                        }
                    }
                }
                if (labDirs.Length > 0)
                {
                    for (int j = 0; j < labDirs.Length; j++)
                    {
                        Directory.GetDirectories(sourcePath + "/" + Path.GetFileName(labDirs[j]));

                        //递归调用
                        CopyFiles(sourcePath + "/" + Path.GetFileName(labDirs[j]), savePath + "/" + Path.GetFileName(labDirs[j]));
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            #endregion
            return true;
        }





    }
}
