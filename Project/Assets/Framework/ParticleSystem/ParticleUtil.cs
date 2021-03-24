using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ParticleUtil
{
    public static void StopParticle(GameObject obj){
        if(obj != null){
            ParticleSystem ps = obj.GetComponentInChildren<ParticleSystem>();
            if(ps != null){
                ps.Stop();
            }
        }
    }
    public static void PlayParticle(GameObject obj){
        if(obj != null){
            ParticleSystem ps = obj.GetComponentInChildren<ParticleSystem>();
            if(ps != null){
                ps.Play();
            }
        }
    }
}
