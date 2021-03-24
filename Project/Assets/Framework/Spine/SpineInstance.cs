using System;
using System.Collections;
using Spine;
using Spine.Unity;
using UnityEngine;

namespace Framework.Common
{
    public class SpineInstance : MonoBehaviour
    {
        private GameObject m_SpineObj;
        SkeletonAnimation m_SpineAnimation;
        public void Init(){

            m_SpineObj = gameObject;
            m_SpineAnimation = m_SpineObj.GetComponent<SkeletonAnimation>();
            

        }

         public bool SetSkin(string skin_name)
        {
            if (m_SpineAnimation != null)
            {
                var sk = m_SpineAnimation.skeleton;
                if (sk != null)
                {
                    try
                    {
                        sk.SetSkin(skin_name);
                    }
                    catch (Exception e) {
                        Debug.LogError(e.Message+"/n"+e.StackTrace);
                    };
                    return true;
                }
            }
            Debug.LogError("failed to set skin ,because the spineObje is null::" + m_SpineObj.name);
            return false;
        }

    }
}