/*-------------------------------------------------------------------------------------------
// 模块名：CachedAssetBundleContainer
// 模块描述：AssetBundle包的缓存池
//-------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.U2D;
using Spine.Unity;
using UObject = UnityEngine.Object;

namespace Framework.Asset
{
    /// <summary>
    /// 缓存一组AssetBundle的容器
    /// </summary>
    public class CachedEditorObjectContainer : IdleClass
    {
        public readonly Dictionary<string, CachedObject> CachedObjectDictionary = new Dictionary<string, CachedObject>();


        public CachedObject LoadCachedObject<T>(string assetPath)where T:UObject
        {
            CachedObject caobj = null;
            if (!CachedObjectDictionary.ContainsKey(assetPath) || CachedObjectDictionary[assetPath] == null)
            {
                T t = LoadEditorResource<T>(assetPath);
                if (t == null)
                {
                    DebugUtil.Log("Res,CachedEditorObjectContainer, the file {0} is not exist ", assetPath);
                }
                else
                {
                    caobj = new CachedObject(t, null);
                    CachedObjectDictionary[assetPath] = caobj;
                }
            }
            else
            {
                caobj = CachedObjectDictionary[assetPath];
            }
            return caobj;
        }

        public T LoadWithOutCache<T>(string assetPath) where T : UObject
        {
            T t = LoadEditorResource<T>(assetPath);
            if (t == null)
            {
                DebugUtil.Log("Res,CachedEditorObjectContainer, the file {0} is not exist ", assetPath);
                return null;
            }

            return t;
        }

        public void Release()
        {
            List<string> releaseKeys = new List<string>();
            foreach (var item in CachedObjectDictionary)
            {
                BundleInfo bundleInfo = AssetBundleConfig.Inst.GuessBundleByAssetName(item.Key);
                if (bundleInfo != null && bundleInfo.releaseMode == ReleaseMode.Persistent)
                {
                    continue;
                }
                if (item.Value.Idle)
                {
                    item.Value.Release();
                    releaseKeys.Add(item.Key);
                }
            }
            foreach (var key in releaseKeys)
            {
                CachedObjectDictionary.Remove(key);
            }
        }


            /// <summary>
            /// 从编辑器里加载资源
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="name"></param>
            /// <returns></returns>
            private T LoadEditorResource<T>(string name) where T : UObject
        {
            List<string> path = new List<string>();
            if (typeof(T) == typeof(Sprite))
            {
                path.Add(name + ".png");
                path.Add(name + ".jpg");
            }
            else if (typeof(T) == typeof(Texture2D))
            {
                path.Add(name + ".png");
                path.Add(name + ".jpg");
            }
            else if (typeof(T) == typeof(GameObject))
            {
                path.Add(name + ".prefab");
            }
            else if (typeof(T) == typeof(AudioClip))
            {
                path.Add(name + ".mp3");
                path.Add(name + ".wav");
                path.Add(name + ".ogg");
            }
            else if (typeof(T) == typeof(Material))
            {
                path.Add(name + ".mat");
            }
            else if (typeof(T) == typeof(TextAsset))
            {
                path.Add(name + ".txt");
                path.Add(name + ".json");
                path.Add(name + ".xml");
            }
            else if (typeof(T) == typeof(SpriteAtlas))
            {
                path.Add(name + ".spriteatlas");
            }
            //else if (typeof(T) == typeof(TMPro.TMP_FontAsset))
            //{
            //    path.Add(name + ".asset");
            //}
            else if (typeof(T) == typeof(Material))
            {
                path.Add(name + ".mat");
            }
            else if (typeof(T) == typeof(SkeletonDataAsset))
            {
                path.Add(name + ".asset");
            }




            foreach (string itempath in path)
            {
                string normalizepath = FilePathTools.NormalizePath(itempath);
#if UNITY_EDITOR
                string _editorPath = FilePathTools.GetAssetEditorPath(normalizepath);
                UObject Obj = UnityEditor.AssetDatabase.LoadAssetAtPath(_editorPath, typeof(T));

                if (Obj != null)
                {
                    return (T)Obj;
                }
#endif
            }
            return null;
        }


    }


}
