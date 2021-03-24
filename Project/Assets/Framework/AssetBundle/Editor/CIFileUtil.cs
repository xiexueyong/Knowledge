using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NoshCore
{
    public class FileEntry
    {
        public string path;
        public string name;
        public long timestamp;

        public FileEntry(string path, string name, long timestamp)
        {
            this.path = path;
            this.name = name;
            this.timestamp = timestamp;
        }
    }

    public class CIFileUtil
    {
        public static bool IsAbsolutePath(string path)
        {
#if UNITY_EDITOR_WIN
            if (path.Length > 2
                && ((path[0] >= 'a' && path[0] <= 'z') || (path[0] >= 'A' && path[0] <= 'Z'))
                && path[1] == ':')
            {
                return true;
            }
            else
            {
                return false;
            }
#elif UNITY_EDITOR_OSX
            if (!string.IsNullOrEmpty(path) && path[0] == '/')
            {
                return true;
            }
            else
            {
                return false;
            }
#else
            return false;
#endif
        }
        public static string GetFullPath(string path)
        {
            return Normalize(Path.GetFullPath(path));
        }

        public static string Normalize(string path)
        {
            return path.Replace('\\', '/');
        }

        public static void CheckDirectory(string filename)
        {
            if (!IsAbsolutePath(filename))
            {
                filename = GetFullPath(filename);
            }
            string dirname = Path.GetDirectoryName(filename);
            if (!IsDirectoryExist(dirname))
            {
                Directory.CreateDirectory(dirname);
            }
        }

        public static string PathCombine(string root, params string[] paths)
        {
            StringBuilder path = new StringBuilder(root);

            var iter = paths.GetEnumerator();
            while (iter.MoveNext())
            {
                string subPath = (string)iter.Current;
                if (path.Length < 1)
                {
                    path.Append(subPath);
                }
                else
                {
                    if (path[path.Length - 1] != '/') path.Append('/');
                    if (!string.IsNullOrEmpty(subPath)) path.Append(subPath);
                }
            }

            return path.ToString();
        }

        public static bool IsDirectoryExist(string path)
        {
            return Directory.Exists(path);
        }

        public static bool CreateDirectory(string path)
        {
            if (!IsDirectoryExist(path))
            {
                return Directory.CreateDirectory(path) != null;
            }
            return true;
        }

        public static void CopyFile(string srcFilename, string dstFilename)
        {
            File.Copy(srcFilename, dstFilename);
        }
        public static void WriteBytesToFile(byte[] content, string filename, bool flush = true)
        {
            CheckDirectory(filename);
            File.WriteAllBytes(filename, content);
        }

        public static void WriteTextToFile(string content, string filename, bool flush = true)
        {
            CheckDirectory(filename);
            File.WriteAllText(filename, content);
        }

        public static long GetFileSize(string filename)
        {
            FileInfo info = new FileInfo(filename);
            if (info != null)
            {
                return info.Length;
            }
            return 0;
        }

        public static void ClearDirectory(string path)
        {
            if (IsDirectoryExist(path))
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                if (dir != null)
                {
                    foreach (var file in dir.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (var subDir in dir.GetDirectories())
                    {
                        subDir.Delete(true);
                    }
                }
            }
        }
        public static void DeleteDirectory(string path)
        {
            if (IsDirectoryExist(path))
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                dir.Delete(true);
            }
        }

        public static void RemoveDirectory(string path)
        {
            Directory.Delete(path, true);
        }

        public static List<FileEntry> ListFiles(string path, bool recursive = false)
        {
            SearchOption option = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            List<FileEntry> list = new List<FileEntry>();
            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (var file in dir.GetFiles("*", option))
            {
                list.Add(new FileEntry(file.FullName, file.Name, file.CreationTimeUtc.ToFileTimeUtc()));
            }
            return list;
        }
        public static List<FileEntry> ListDirectorys(string path, bool recursive = false)
        {
            SearchOption option = recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
            List<FileEntry> list = new List<FileEntry>();
            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (var subDir in dir.GetDirectories("*", option))
            {
                list.Add(new FileEntry(subDir.FullName, subDir.Name, subDir.CreationTimeUtc.ToFileTimeUtc()));
            }
            return list;
        }

        public static byte[] ReadBytesFromFile(string filename)
        {
            return File.ReadAllBytes(filename);
        }

        public static string ReadTextFromFile(string filename)
        {
            return File.ReadAllText(filename);
        }
        public static bool IsFileExist(string filename)
        {
            return File.Exists(filename);
        }
        public static void DeleteFile(string filename)
        {
            File.Delete(filename);
        }


    }
}
