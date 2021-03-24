/*-------------------------------------------------------------------------------------------
// 模块名：ResourcesManager
// 模块描述：资源加载管理器
//-------------------------------------------------------------------------------------------*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Framework.Utils;
using Framework.Asset;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

using UObject = UnityEngine.Object;

namespace Framework.Asset
{
    public class Res : D_MonoSingleton<Res>
    {
        private static Transform UnactiveContainer;

        private static bool _init;
        private CachedAssetBundleContainer _cachedAssetBundleContainer;
        private CachedEditorObjectContainer _cachedEditorObjectContainer;
        void Awake()
        {
            if (AssetBundleConfig.Inst.UseAssetBundle)
            {
                DebugUtil.Log("==Res Create CachedAssetBundleContainer");
                _cachedAssetBundleContainer = new CachedAssetBundleContainer();
            }
            else
            {
                DebugUtil.Log("==Res Create CachedEditorObjectContainer");
                _cachedEditorObjectContainer = new CachedEditorObjectContainer();
            }
                

            GameObject unactiveObj =  new GameObject();
            unactiveObj.name = "UnactiveContainer";
            UnactiveContainer = unactiveObj.transform;
            UnactiveContainer.SetParent(this.transform);
            unactiveObj.SetActive(false);
        }
        public static void Unactive(GameObject gameObject)
        {
            gameObject.transform.SetParent(UnactiveContainer,false);
        }
        public static void Init()
        {
            _init = true;
        }
        public static bool IsInPool(Transform transform)
        {
            return transform.parent == UnactiveContainer;
        }
        /// <summary>
        /// 加载资源的总接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">相对于Asset/Export的目录资源</param>
        /// <returns></returns>
        public static T LoadResource<T>(string name,bool cache = true) where T : UObject
        {
            if (GameController.ApplicationQuit)
            {
                return null;
            }
            return Inst.actualLoadResource<T>(name, cache);

        }

        public static T LoadResourceWithoutCache<T>(string name) where T : UObject
        {
            if (GameController.ApplicationQuit)
            {
                return null;
            }
            return Inst._cachedEditorObjectContainer.LoadWithOutCache<T>(name);

        }
        /// <summary>
        /// 加载资源的总接口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">相对于Asset/Export的目录资源</param>
        /// <returns></returns>
        protected T actualLoadResource<T>(string name, bool cache) where T : UObject
        {
            if (!_init)
                throw new Exception("Res has not initialised,It can be used after initialised in GameController !");
            T t = null;
            CachedObject cachedObject = null;
            if (AssetBundleConfig.Inst.UseAssetBundle)
            {
                AssetBundleInfo bundleInfo = AssetBundleManager.Inst.GuessBundleByAssetName(name);
                if (bundleInfo.isEmpty())
                {
                    DebugUtil.LogError("Res.LoadResource(),AssetBundleManager.GueessBundleByAssetName can't find the AssetBundle container the asset :__ {0} __", name);
                }
                else
                {
                    CachedAssetBundle cab = _cachedAssetBundleContainer.LoadCachedAssetBundle(bundleInfo);
                    if (cab != null)
                    {
                        string assetPureName = ParseAssetName(name);
                        if (!string.IsNullOrEmpty(assetPureName))
                        {
                            cachedObject = cab.LoadCachedObject<T>(assetPureName);
                            //return cab.LoadAsset<T>(assetPureName);
                            if (cachedObject != null)
                                t = cachedObject.GetAsset<T>(cache);
                        }
                    }
                }
            }
            else
            {
                cachedObject = _cachedEditorObjectContainer.LoadCachedObject<T>(name);
                if (cachedObject != null)
                {
                    t = cachedObject.GetAsset<T>(cache);
                }
            }
            RegisterAsset(t, cachedObject,cache);
            return t;
        }


        private Dictionary<UObject, CachedObject> assetBook = new Dictionary<UObject, CachedObject>();
        private void RegisterAsset(UObject assetObject, CachedObject cachedObject,bool cache)
        {
            if (!cache)
                return;
            if (assetObject != null && cachedObject != null)
                assetBook[assetObject] = cachedObject;
        }


        /// <summary>
        /// 返还素材
        /// </summary>
        /// <param name="originPath">文件的路径，以Asset/Export为根目录</param>
        /// <param name="uObject">文件对象</param>
        /// <returns></returns>
        //[Obsolete]
        //private bool Recycle(string originPath, UObject uObject)
        //{
        //    string guessBundleName = AssetBundleManager.Inst.GueessBundleNameByAssetName(originPath);
        //    CachedAssetBundle cab = _cachedAssetBundleContainer.GetCachedAssetBundle(guessBundleName);
        //    if (cab == null)
        //        return false;
        //    CachedObject cobj = cab.GetCachedObject(ParseAssetName(originPath));
        //    if (cobj != null)
        //    {
        //        return cobj.Recycle(uObject);
        //    }
        //    return false;
        //}
      /// <summary>
      /// 延时Recyle某个资源
      /// </summary>
      /// <param name="uObject"></param>
      /// <param name="delay">以秒为单位</param>
        public static void RecycleDelay(UObject uObject,float delay)
        {
            CoroutineManager.Inst.StartCoroutine(RecycleEnumerator(uObject,delay));
        }
        private static IEnumerator RecycleEnumerator(UObject uObject, float delay)
        {
            yield return new WaitForSeconds(delay);
            Inst.actualRecycle(uObject);
        }
        public static bool Recycle(UObject uObject)
        {
            if (uObject.name == "MatchClick")
            {
                Debug.Log("--");
            }
            if (GameController.ApplicationQuit)
            {
                return false;
            }
            return Inst.actualRecycle(uObject);
        }

        /// <summary>
        /// 返还素材
        /// </summary>
        /// <param name="originPath">文件的路径，以Asset/Export为根目录</param>
        /// <param name="uObject">文件对象</param>
        /// <returns></returns>
        protected bool actualRecycle(UObject uObject)
        {
#if DEBUG
            if (uObject is Component )
            {
                DebugUtil.LogError("Res can't recyle Compomet or Transform ");
            }
#endif
            if (!_init)
                throw new Exception("Res has not initialised,It can be used after initialised in GameController !");

            if (uObject == null)
            {
                DebugUtil.LogError("Res Recycle a null Object !!");
                return false;
            }

            if (assetBook.ContainsKey(uObject))
            {
                CachedObject co = assetBook[uObject];
                bool recyleSucess = false;
                if (co != null)
                {
                    recyleSucess = co.Recycle(uObject);
                    if (co.rt != CachedObject.ResourceType.Single || co.RefCount == 0)
                        assetBook.Remove(uObject);
                }
               
                return recyleSucess;
            }
            else
            {
#if UNITY_EDITOR
                if (uObject is GameObject)
                {
                    ResNotifyDestroy rnd = (uObject as GameObject).GetComponent<ResNotifyDestroy>();
                    if (rnd == null)
                        DebugUtil.LogWarning(uObject.name + ",   The Object is not in assetBook when Res Recyle");
                    else
                        DebugUtil.LogError(string.Format("{0},   The Object is not in assetBook when Res Recyle,useCount:{1},recyleCount{2}", uObject.name,rnd.UseCount,rnd.RecyltCount));
                }
                else
                {
                    DebugUtil.LogWarning(uObject.name + ",   The Object is not in assetBook when Res Recyle");
                }
#endif
                if (uObject is GameObject)
                {
                    Destroy(uObject);
                }
            }

            return false;
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        public void Release()
        {
            if (!_init)
                throw new Exception("Res has not initialised,It can be used after initialised in GameController !");
          
            if (AssetBundleConfig.Inst.UseAssetBundle)
                _cachedAssetBundleContainer.Release();
            else
                _cachedEditorObjectContainer.Release();
      
            Resources.UnloadUnusedAssets();
            GC.Collect();
        }

        public void UnloadAll(bool unloadAllLoadedObject = false)
        {
            if (!_init)
                throw new Exception("Res has not initialised,It can be used after initialised in GameController !");

            _cachedAssetBundleContainer.UnloadAll();
        }

        //float passedTime = 0f;
        //private void Update()
        //{
        //    if (passedTime <= 30)
        //    {
        //        passedTime += Time.deltaTime;
        //        return;
        //    }
        //    passedTime = 0f;
        //    Release();
        //}



        public static string ParseAssetName(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
            int lastSplit = path.LastIndexOf('/');
            if (lastSplit > 0 && path.Length > lastSplit - 1)
            {
                return path.Substring(lastSplit + 1);
            }
            return null;
        }



#if UNITY_EDITOR
        [CustomEditor(typeof(Res))]
        public class ResEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                if (AssetBundleConfig.Inst.UseAssetBundle)
                {
                    OnBundleGui();
                }
                else
                {
                    OnNotBundleGui();
                }
            }
            private void OnBundleGui()
            {
                base.OnInspectorGUI();
                //DrawDefaultInspector();
                //Dictionary<string, CachedAssetBundle> cabDic = Res.Inst._cachedAssetBundleContainer.CachedAssetBundleDictionary;
                List<KeyValuePair<string, CachedAssetBundle>> cabList = Res.Inst._cachedAssetBundleContainer.CachedAssetBundleDictionary.ToList();
                
                IdleSort(cabList);
                foreach (var cab in cabList)
                {
                    //Dictionary<string, CachedObject> caoDic = cab.Value.CachedObjectDictionary;
                    List<KeyValuePair<string, CachedObject>> CachedObjectList = cab.Value.CachedObjectDictionary.ToList();
                    GUILayout.Label("AB: " + cab.Key+"======");
                    GUILayout.Label("Idle: " + cab.Value.Idle.ToString());

                    foreach (var item in CachedObjectList)
                    {
                        GUILayout.Label("   ---"+item.Key);
                        if (item.Value.rt == CachedObject.ResourceType.Single)
                        {
                            GUILayout.Label("        " + item.Value.GetSingleMeta());
                        }
                        else
                        {
                            GUILayout.Label("        " + item.Value.GetMultiMeta());
                        }
                    }
                }
            }
            private void OnNotBundleGui()
            {
                List<KeyValuePair<string, CachedObject>> CachedObjectList = Res.Inst._cachedEditorObjectContainer.CachedObjectDictionary.ToList();
                IdleSort(CachedObjectList);
                foreach (var item in CachedObjectList)
                {
                    GUILayout.Label(item.Key+"------------");
                    if (item.Value.rt == CachedObject.ResourceType.Single)
                    {
                        GUILayout.Label("      " + item.Value.GetSingleMeta());
                    }
                    else
                    {
                        GUILayout.Label("      " + item.Value.GetMultiMeta());
                    }
                }
            }

            private void IdleSort(List<KeyValuePair<string, CachedObject>> IdleList)
            {
                IdleList.Sort(
                (x, y) =>
                {
                    if (x.Value.Idle)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
                );
            }
            private void IdleSort(List<KeyValuePair<string, CachedAssetBundle>> IdleList)
            {
                IdleList.Sort(
                (x, y) =>
                {
                    if (x.Value.Idle)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
                );
            }
        }
#endif


    }
}