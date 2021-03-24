/*-------------------------------------------------------------------------------------------
// 模块名：CachedAssetBundleContainer
// 模块描述：AssetBundle包的缓存池
//-------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;
using System.IO;
using UObject = UnityEngine.Object;
using Spine.Unity;
using Framework.Utils;

namespace Framework.Asset
{
    /// <summary>
    /// 缓存的CachedObject
    /// </summary>
    public class CachedObject : IdleClass
    {
        public enum ResourceType
        {
            NotInit = 0,
            Single = 1,
            Multi = 2
        }

        public CachedAssetBundle parent;
        public UObject source;
        public readonly List<UObject> activeCopies = new List<UObject>();
        public readonly Stack<UObject> unactiveCopies = new Stack<UObject>();
        public int RefCount = 0;

        private bool _inited;
        public ResourceType rt;
        private string _tyName = string.Empty;

        public string GetSingleMeta()
        {
            return string.Format("RefCount:{1},  {2}", rt.ToString(),RefCount.ToString(), _tyName);

        }
        public string GetMultiMeta()
        {
            return string.Format("Live:{0}, Dead:{1}", activeCopies.Count.ToString(), unactiveCopies.Count.ToString());
        }
        public CachedObject(UObject source, CachedAssetBundle _parent)
        {
            this.parent = _parent;
            this.source = source;
        }
        private void Init<T>()
        {
            if (_inited)
                return;

            _inited = true;
            _tyName = typeof(T).Name;
            if (typeof(T) == typeof(GameObject))
            {
                rt = ResourceType.Multi;
            }
            else
            {
                rt = ResourceType.Single;
            }
        }
        /// <summary>
        /// 请求资源，使用后记得还回Recycle
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetAsset<T>(bool cache) where T : UObject
        {
            Init<T>();

            UObject obj = null;
            if (rt == ResourceType.Multi)
            {
                if (unactiveCopies.Count > 0 && cache)
                {
                    obj = unactiveCopies.Pop();
                    if (obj == null)
                    {
                        return GetAsset<T>(cache);//防止已经被外部Destroy
                    }
                    activeCopies.Add(obj);
                    try
                    {
                        GameObject unactiveObj = (obj as GameObject);
                        unactiveObj.transform.SetParent(null);                       
                    }catch(Exception e)
                    {
                        DebugUtil.LogError(this.source.name+":"+ e.ToString());
                    }
                }
                else
                {
                    obj = UObject.Instantiate(source);
                    obj.name = obj.name.Replace("(Clone)", "");

                    if (cache)
                    {
                        activeCopies.Add(obj);
#if UNITY_EDITOR
                        AddDestroyNotify(obj as GameObject);
                        resetShader(obj as GameObject);
#endif
                    }
                }
#if UNITY_EDITOR
                if(cache)
                    AddUseCount(obj as GameObject);
#endif
            }
            else if(rt == ResourceType.Single)
            {
                RefCount++;
                obj = source;
            }
            if (obj != null)
            {
                CheckIdle();
                if(parent != null)
                    parent.CheckIdle();
                return obj as T;
            }
            return null;
        }
        private void AddUseCount(GameObject go)
        {
#if UNITY_EDITOR
            if (go != null && go.GetComponent<ResNotifyDestroy>() != null)
                go.GetComponent<ResNotifyDestroy>().UseCount++;
#endif
        }
        private void AddRecyleCount(GameObject go)
        {
#if UNITY_EDITOR
            if (go != null && go.GetComponent<ResNotifyDestroy>() != null)
                go.GetComponent<ResNotifyDestroy>().RecyltCount++;
#endif
        }
        private void RemoveDestroyNotify(GameObject go)
        {
#if UNITY_EDITOR
            if (go == null)
                return;
            ResNotifyDestroy rnf = go.GetComponent<ResNotifyDestroy>();
            if (go != null && rnf != null)
            {
                rnf.activeNotify = false;
                GameObject.Destroy(rnf);
            }
#endif 
        }
        private void AddDestroyNotify(GameObject go)
        {
#if UNITY_EDITOR
            if (go != null && go.GetComponent<ResNotifyDestroy>() == null)
                go.AddComponent<ResNotifyDestroy>();
#endif
        }
        private void resetShader(GameObject go)
        {
#if UNITY_EDITOR
            //SkeletonGraphic skeletonGraphic = go.transform.GetComponent<SkeletonGraphic>();
            SkeletonGraphic[] skeletonGraphics = go.transform.GetComponentsInChildren<SkeletonGraphic>();
            if (skeletonGraphics != null && skeletonGraphics.Length > 0)
            {
                foreach (var item in skeletonGraphics)
                {
                    string shaderName = item.material.shader.name;
                    item.material.shader = Shader.Find(shaderName);
                }
            }

            SkinnedMeshRenderer[] skinnedMeshRenderer = go.transform.GetComponentsInChildren<SkinnedMeshRenderer>();
            if (skinnedMeshRenderer != null && skinnedMeshRenderer.Length > 0)
            {
                foreach (var item in skinnedMeshRenderer)
                {
                    string shaderName = item.material.shader.name;
                    item.material.shader = Shader.Find(shaderName);
                }
            }
            MeshRenderer[] meshRenderers = go.transform.GetComponentsInChildren<MeshRenderer>();
            if (meshRenderers != null && meshRenderers.Length > 0)
            {
                foreach (var item in meshRenderers)
                {
                    string shaderName = item.material.shader.name;
                    item.material.shader = Shader.Find(shaderName);
                }
            }
#endif
        }

        /// <summary>
        /// 还回不使用的资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="_object"></param>
        /// <returns></returns>
        public bool Recycle(UObject _object)
        {
            if (_object == null)
            {
                DebugUtil.LogError("Res.Recyle a null obj,maybe it is destryed by yourself,you should not do that");
                return false;
            }
#if UNITY_EDITOR
            //DebugUtil.Log("====CachedObject Recycle : "+ _object.name);
#endif
            if (rt == ResourceType.Multi)
            {
                //===临时
                //GameObject.Destroy(_object);
                //return true;
                //======
                GameObject gameObj = _object as GameObject;
                if (activeCopies.Contains(_object))
                {
                    activeCopies.Remove(_object);
                    unactiveCopies.Push(_object);
                    Res.Unactive(gameObj);
#if UNITY_EDITOR
                    AddRecyleCount(gameObj);
#endif
                    CheckIdle();
                    if (Idle && parent != null)
                        parent.CheckIdle();

                    if ((AssetBundleConfig.Inst.UseAssetBundle && this.parent == null) || (this.parent != null && this.parent.bundleInfo.releaseMode == ReleaseMode.Immediate)|| AssetBundleConfig.Inst.ReleaseImmediate)
                        Release();
                    return true;
                }
            }
            else
            {
                if (RefCount > 0  && _object == source)
                {
                    RefCount--;
                    CheckIdle();
                    if (Idle && parent != null)
                        parent.CheckIdle();
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 检查是否处于闲置状态
        /// </summary>
        public override void CheckIdle()
        {
            base.CheckIdle();
            if (activeCopies.Count <= 0 && RefCount <= 0)
            {
                Idle = true;
            }

        }


        /// <summary>
        /// 释放不使用的资源
        /// </summary>
        public void Release()
        {
            while (unactiveCopies.Count > 0)
            {
                UObject uo = unactiveCopies.Pop();
                if (uo != null)
                {
                    RemoveDestroyNotify(uo as GameObject);
                    GameObject.Destroy(uo);
                }
                    
            }
        }
        /// <summary>
        /// 所有缓存全部卸载,只能被父类调用
        /// </summary>
        public void Unload(bool unloadallLoadedObjects = false)
        {
            Release();
            if (unloadallLoadedObjects)
            {
                while (activeCopies.Count > 0)
                {
                    GameObject.Destroy(activeCopies[0]);
#if UNITY_EDITOR
                    RemoveDestroyNotify(activeCopies[0] as GameObject);
#endif
                    activeCopies.RemoveAt(0);
                }
                RefCount = 0;
            }
            //GameObject.Destroy(source);
            source = null;
            parent = null;
        }

    }
}
